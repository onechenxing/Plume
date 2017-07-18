using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Plume.Core
{
    /// <summary>
    /// 虚拟机类 - 核心运行方法
    /// </summary>
    public partial class PlumeVM
    {
        /// <summary>
        /// 代码执行核心
        /// </summary>
        /// <param name="stopFp">执行到某个帧空间位置暂停,用于外部代码调用函数返回</param>
        /// <returns>是否当前代码执行完毕，如果执行完毕返回true，如果没有完毕返回false</returns>
        bool Cpu(int stopFp = -1)
        {
            object objA, objB;
            float number;
            Table table;
            IIterator itr;
            string name;
            VMCode code;            
            while (cp >= 0 && cp < codes.Count)
            {
                //取当前代码并且把代码指针指向下一个
                code = codes[cp++];
                //终止指令
                if (code.type == VMCodeType.Halt)
                    break;
                switch (code.type)
                {
                    #region ----------数据结构----------
                    //数据:常量
                    case VMCodeType.Number:
                    case VMCodeType.String:
                    case VMCodeType.Block:
                        operands[++op] = code.data;
                        break;

                    //迭代器:变量
                    case VMCodeType.Iterator:
                        {
                            itr = null;
                            objA = operands[op--];
                            if (objA is IIterator)
                            {
                                itr = (IIterator)objA;
                            }
                            else if (objA is string)
                            {
                                itr = new StringIterator((string)objA);
                            }
                            else if (objA is Table)
                            {
                                itr = new TableIterator((Table)objA);
                            }
                            else
                            {
                                throw new Exception("No Iterator with Type : " + objA.GetType());
                            }
                            operands[++op] = itr;
                        }
                        break;
                    //迭代器:判断是否还有下一个
                    case VMCodeType.Iterator_HasNext:
                        itr = (IIterator)operands[op--];
                        operands[++op] = itr.HasNext() ? 1f : 0f;
                        break;
                    //迭代器:移动并获取下一个元素
                    case VMCodeType.Iterator_MoveNext:
                        itr = (IIterator)operands[op--];
                        operands[++op] = itr.MoveNext();
                        break;

                    //表:变量
                    case VMCodeType.Table:
                        operands[++op] = new Table();//新建一个表
                        break;
                    //表:初始化
                    case VMCodeType.Table_InitItem:
                        {
                            int count = Convert.ToInt32(operands[op--]);//插入表数据个数
                            table = (Table)operands[op - count * 2];
                            //注意栈中顺序是反的
                            for (int i = count * 2 - 1; i >= 0; i -= 2)
                            {
                                objA = operands[op - i];//key
                                objB = operands[op - i + 1];//value
                                table.Set(objA, objB);
                            }
                            op -= count * 2;
                        }
                        break;
                    #endregion

                    #region ----------运算----------
                    //四则运算
                    case VMCodeType.Plus:
                    case VMCodeType.Minus:
                    case VMCodeType.Multiply:
                    case VMCodeType.Divide:
                        objA = operands[op - 1];
                        objB = operands[op];
                        op -= 2;
                        operands[++op] = _OP(objA, objB, code.type);
                        break;

                    //判断
                    case VMCodeType.Check_Equals:
                    case VMCodeType.Check_NotEquals:
                    case VMCodeType.Check_GreaterThan:
                    case VMCodeType.Check_GreaterThanOrEquals:
                    case VMCodeType.Check_LessThan:
                    case VMCodeType.Check_LessThanOrEquals:
                        objA = operands[op - 1];
                        objB = operands[op];
                        op -= 2;
                        operands[++op] = _Check(objA, objB, code.type);
                        break;

                    //条件
                    case VMCodeType.And:
                        objA = operands[op - 1];
                        objB = operands[op];
                        op -= 2;
                        operands[++op] = _And(objA, objB);
                        break;
                    case VMCodeType.Or:
                        objA = operands[op - 1];
                        objB = operands[op];
                        op -= 2;
                        operands[++op] = _Or(objA, objB);
                        break;

                    #endregion

                    #region ----------跳转到代码行----------
                    //直接跳转到代码行
                    case VMCodeType.Jump:
                        cp = Convert.ToInt32(code.data);
                        break;
                    //True条件跳转到代码行
                    case VMCodeType.If_True_Jump:
                        number = (float)operands[op--];
                        if (number != 0)
                        {
                            cp = Convert.ToInt32(code.data);
                        }
                        break;
                    //False条件跳转到代码行
                    case VMCodeType.If_False_Jump:
                        number = (float)operands[op--];
                        if (number == 0)
                        {
                            cp = Convert.ToInt32(code.data);
                        }
                        break;

                    #endregion

                    #region ----------变量 存储.载入 设置.获取 ----------
                    //存储
                    case VMCodeType.Store:
                        name = (string)(code.data);
                        objA = operands[op--];
                        if (VMUtil.IsSupportType(objA) == false)
                        {
                            throw new Exception(string.Format("no support type to Store:name:{0},value:{1},type:{2}",name,objA,objA.GetType()));
                        }
                        //存储在当前帧顶空间
                        frames[fp].block.Set(name, objA);
                        break;

                    //载入
                    case VMCodeType.Load:
                        name = (string)(code.data);
                        if (frames[fp].block.DeepGet(name) != null)//当前帧顶空间查找
                        {
                            operands[++op] = frames[fp].block.DeepGet(name);
                        }
                        else if (globalSpace.Has(name))//全局查找
                        {
                            operands[++op] = globalSpace.Get(name);
                        }
                        else
                        {
                            throw new Exception("has no name to load:" + name);
                        }
                        break;

                    //取容器或代码块内部元素
                    case VMCodeType.GetItem:
                        {
                            IContainer container = (IContainer)operands[op - 1];//container                     
                            objA = operands[op];//key
                            op -= 2;
                            operands[++op] = container.Get(objA);
                        }
                        break;

                    //设置容器或代码块内部原始
                    case VMCodeType.SetItem:
                        {
                            IContainer container = (IContainer)operands[op - 2];//container
                            objA = operands[op - 1];//key
                            objB = operands[op];//value
                            op -= 3;
                            container.Set(objA, objB);
                        }
                        break;
                    #endregion

                    #region ----------代码块相关----------
                    //调用块
                    case VMCodeType.Call:
                        _Call();
                        break;

                    //块返回
                    case VMCodeType.Return:
                        VMStackFrame fr = frames[fp];
                        cp = fr.returnAddress;
                        if (fp == 0)//第0帧是主代码，退出不移除，方便重新运行
                        {
                            --op;//丢弃主代码返回值
                        }
                        else//其他代码移除帧和作用域
                        {
                            fp--;
                            if (fp == stopFp)
                            {
                                return false;
                            }
                        }
                        break;
                    #endregion

                    #region ----------内置特殊函数----------
                    //等待
                    case VMCodeType.Wait:
                        {
                            var value = operands[op];
                            IWait wt;
                            if (value is IWait)//等待IWait对象
                            {
                                wt = (IWait)value;
                            }
                            else if (value is float)//等待时间，自动构建一个WaitTime等待
                            {
                                wt = new WaitTime((float)value);
                                operands[op] = wt;//存到当前的操作栈，为了下次更新时使用
                            }                             
                            else
                            {
                                throw new Exception("Wait value mast implement IWait:" + value);
                            }
                            //等待操作
                            if (wt.WaitOK() == false)
                            {
                                cp--;//代码回退到当前位置，等待下一次执行
                                return false;
                            }
                            op--;//不用等待了，把IWait对象移除栈
                        }
                        break;
                    //加载代码文件
                    case VMCodeType.LoadCodeFile:
                        {
                            string filePath = (string)operands[op--];
                            //实际保存为代码块
                            Block bs;
                            //缓存里面有取缓存
                            if (loadCodeDic.ContainsKey(filePath))
                            {
                                bs = loadCodeDic[filePath];
                            }
                            else//没有缓存，加载新的
                            {
                                string codeStr = LoadCodeFile(filePath);
                                bs = BuildCodesBlock(codeStr);
                                bs.name = filePath;
                                loadCodeDic[filePath] = bs;
                            }
                            operands[++op] = bs;//放入栈顶
                        }
                        break;
                    #endregion

                    #region ----------其他操作----------
                    //置入Null
                    case VMCodeType.Null:
                        operands[++op] = null;
                        break;

                    //弹出一个操作数
                    case VMCodeType.Pop:
                        --op;
                        break;

                        #endregion
                }
            }
            if (DEBUG)
            {
                //代码执行完毕
                PrintBlock.OutFunc(string.Format(
                        "\n------ VM Finish ------\ncp:{0,-5} op:{1,-5} fp:{2,-5}\nprintu:{3}/{4}\n\n",
                        cp, op, fp, PrintuBlock.Test_Ok, PrintuBlock.Test_Num
                        ));
            }
            _isFinish = true;
            return true;
        }

    }
}
