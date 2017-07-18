using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Plume.Core
{
    /// <summary>
    /// 虚拟机类 - 代码块调用
    /// </summary>
    public partial class PlumeVM
    {
        //调用代码块
        void _Call()
        {
            //获取代码块
            BaseBlock def = (BaseBlock)operands[op--];

            #region 参数处理
            //获取传入参数个数
            int count = Convert.ToInt32(operands[op--]);
            //参数保存到数组
            List<object> paramValueList;
            //没有参数列表的，直接按顺序取参数
            if (def.paramNameList == null)
            {
                paramValueList = new List<object>();
                //注意栈中顺序是反的
                for (int i = count - 1; i >= 0; i--)
                {
                    paramValueList.Insert(0, operands[op--]);
                    op--;//skip:index
                }
            }
            else//如果有参数列表：按顺序或用户传递或默认值取
            {
                paramValueList = new List<object>(new object[def.paramNameList.Count]);
                List<bool> isSetList = new List<bool>(new bool[def.paramNameList.Count]);//是否设置了某个参数,没有要取默认值
                                                                                         //注意栈中顺序是反的
                for (int i = count * 2 - 1; i >= 0; i -= 2)
                {
                    int index;
                    var key = operands[op - i];//index or paramName
                    var value = operands[op - i + 1];//value
                    if (key is float || key is int)
                    {
                        index = Convert.ToInt32(key);
                    }
                    else
                    {
                        index = def.paramNameList.IndexOf((string)key);
                    }
                    paramValueList[index] = value;
                    isSetList[index] = true;
                }
                op -= count * 2;
                //取默认值
                for (int i = 0; i < def.paramNameList.Count; i++)
                {
                    if (isSetList[i] == false)
                    {
                        paramValueList[i] = def.paramDefaultValueList[i];
                        isSetList[i] = true;
                    }
                }
            }
            #endregion

            
            if (def is BaseBuildInBlock)//内建函数调用
            {
                BaseBuildInBlock defBuildIn = def as BaseBuildInBlock;
                operands[++op] = defBuildIn.DoBuildIn(paramValueList);
            }
            else if(def is Block)//用户编写的代码块调用
            {
                Block defBlock = def as Block;
                //调用前先清空上次存储的数据，防止出问题
                defBlock.Clear();
                //把下一代码地址加入帧(函数返回用)
                frames[++fp] = new VMStackFrame("Call", cp, defBlock);
                //参数存储
                for (int i = 0; i < paramValueList.Count; i++)
                {
                    defBlock.Set(def.paramNameList[i], paramValueList[i]);
                }
                //跳转到函数代码执行                        
                cp = defBlock.address;
            }
        }

    }
}
