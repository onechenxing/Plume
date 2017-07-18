using System;
using System.Collections;
using System.Collections.Generic;


namespace Plume.Core
{
    /// <summary>
    /// VR代码生成器 (AST->VMCode)
    /// </summary>
    class VMCodeBuilder
    {
        /// <summary>
        /// 生成的代码数据列表
        /// </summary>
        private List<VMCode> codes = new List<VMCode>();

        /// <summary>
        /// 代码块堆栈：实现作用域，默认有一个底层代码块（地址为0）
        /// </summary>
        private List<Block> blockStack = new List<Block>();
                
        /// <summary>
        /// 构建并添加新的代码块（与前面的代码独立：适用于代码文件的添加,重新分配独立作用域，最后添加return null）
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns>添加代码块的起始作用域</returns>
        public Block BuildCodesBlock(ASTNode astNode)
        {
            Block startBock = new Block(codes.Count);
            //重新分配作用域
            blockStack.Clear();
            blockStack.Add(startBock);

            Exec(astNode);
            //如果没有返回最后默认返回空
            AddToCode(VMCodeType.Null);
            AddToCode(VMCodeType.Return);

            return startBock;
        }

        /// <summary>
        /// 构建并添加新代码（代码累加：适用于命令行式交互）
        /// </summary>
        /// <param name="astNode"></param>
        public void BuildCodesAdded(ASTNode astNode)
        {
            Exec(astNode);
        }

        /// <summary>
        /// 获取代码数据列表的引用
        /// </summary>
        /// <returns></returns>
        public List<VMCode> GetCodes()
        {
            return codes;
        }

        /// <summary>
        /// 清除已有代码数据列表
        /// </summary>
        public void ClearCodes()
        {
            codes.Clear();
        }

        /// <summary>
        /// 添加到代码数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        VMCode AddToCode(VMCodeType type)
        {
            VMCode code = new VMCode(type);
            codes.Add(code);
            return code;
        }

        /// <summary>
        /// 添加到代码数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        VMCode AddToCode(VMCodeType type, object data)
        {
            VMCode code = new VMCode(type, data);
            codes.Add(code);
            return code;
        }

        /// <summary>
        /// 获得当前代码位置
        /// </summary>
        /// <returns></returns>
        int GetCodeAddress()
        {
            return codes.Count - 1;
        }

        /// <summary>
        /// 获得下一条代码位置
        /// </summary>
        /// <returns></returns>
        int GetNextCodeAddress()
        {
            return codes.Count;
        }

        /// <summary>
        /// 执行代码转换
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        void Exec(ASTNode n)
        {
            switch (n.type)
            {
                //------------ 指令列表 -------------
                case NodeType.Stat_List:
                    _StateList(n);
                    break;

                //------------ 变量存储获取 -------------
                case NodeType.Assign:      //定义和赋值
                    _Assign(n);
                    break;
                case NodeType.QueueAssign: //连续变量赋值
                    _QueueAssign(n);
                    break;
                case NodeType.Name:        //获取存储的变量或定义
                    _LoadName(n);
                    break;
                case NodeType.QueueName:       //获取连续变量
                    _LoadQueueName(n);
                    break;

                case NodeType.Table_GetItem:   //中括号表达式获取元素
                    _GetItem(n);
                    break;
                case NodeType.Table_SetItem:   //中括号表达式设置元素
                    _SetItem(n);
                    break;

                //------------ 数据类型 -------------
                case NodeType.Number:          //数值
                    AddToCode(VMCodeType.Number, Convert.ToSingle(n.data));
                    break;
                case NodeType.String:          //字符串
                    AddToCode(VMCodeType.String, n.data);
                    break;
                case NodeType.Table:           //表
                    _Table(n);
                    break;

                //------------ 运算 -------------
                case NodeType.Plus:        //加法
                case NodeType.Minus:       //减法
                case NodeType.Multiply:    //乘法
                case NodeType.Divide:      //除法
                    _OP(n);
                    break;

                case NodeType.For:     //循环语句
                    _For(n);
                    break;

                case NodeType.If:      //条件语句
                    _If(n);
                    break;

                //判断
                case NodeType.CheckEquals:// ==
                case NodeType.CheckNotEquals:// !=
                case NodeType.CheckGreaterThan:// >
                case NodeType.CheckGreaterThanOrEquals:// >=
                case NodeType.CheckLessThan:// <
                case NodeType.CheckLessThanOrEquals:// <=
                    _Check(n);
                    break;

                //条件
                case NodeType.And://and
                    _And(n);
                    break;
                case NodeType.Or://or
                    _Or(n);
                    break;


                //------------ 代码块相关 ----------
                case NodeType.Block://代码块
                    _Block(n);
                    break;

                case NodeType.Call_Expr:   //代码块调用（要用返回值）
                    _Call(n);
                    break;
                case NodeType.Call_Stat:   //（不用返回值）
                    _Call(n);
                    AddToCode(VMCodeType.Pop);//丢弃返回值
                    break;

                case NodeType.Return:      //代码块返回值
                    _Return(n);
                    break;

                //------------ 特殊函数 ----------
                case NodeType.Wait:        //等待
                    _Wait(n);
                    break;

                case NodeType.Load_Expr:   //加载代码（不执行，返回代码块）
                    _LoadCodeExpr(n);
                    break;
                case NodeType.Load_Stat:   //加载代码（直接执行）
                    _LoadCodeStat(n);
                    break;

                default:
                    throw new Exception("节点 " + n.GetType().Name + " 没有对应处理方法");
            }
        }

        //调用函数
        public void _Call(ASTNode n)
        {
            var param = (ExprListNode)(n.GetChild(1));
            float index = 0;
            int paramNum = 0;
            if (param.children != null)
            {
                paramNum = param.children.Count;

                foreach (var i in param.children)
                {
                    if (i.type == NodeType.InnerAssign)//传递参数名加值：param=value
                    {
                        AddToCode(VMCodeType.String, i.GetChild(0).data);//param
                        Exec(i.GetChild(1));//value
                    }
                    else//直接传值：value
                    {
                        AddToCode(VMCodeType.Number, index++);//index
                        Exec(i);//value
                    }
                }
            }
            //压入参数数量
            AddToCode(VMCodeType.Number, paramNum);
            Exec(n.GetChild(0));//加载代码块
            AddToCode(VMCodeType.Call);//呼叫
        }

        //函数返回
        public void _Return(ASTNode n)
        {
            if (n.children == null || n.children.Count == 0)//空返回
            {
                AddToCode(VMCodeType.Null);
            }
            else
            {
                Exec(n.GetChild(0));
            }
            AddToCode(VMCodeType.Return);
        }

        //加载代码运行并返回代码块
        public void _LoadCodeExpr(ASTNode n)
        {
            //加载和运行代码块
            _LoadCodeStat(n);
            //返回代码块
            Exec(n.GetChild(0));//加载文件路径
            AddToCode(VMCodeType.LoadCodeFile);//加载并保存为代码块，放入操作栈顶
        }

        //加载代码并运行
        public void _LoadCodeStat(ASTNode n)
        {
            AddToCode(VMCodeType.Number, 0);//调用参数数量0
            Exec(n.GetChild(0));//加载文件路径
            AddToCode(VMCodeType.LoadCodeFile);//加载并保存代码块，放入操作栈顶
            AddToCode(VMCodeType.Call);//调用代码块
            AddToCode(VMCodeType.Pop);//丢弃返回值
        }

        //定义代码块
        void _Block(ASTNode n)
        {
            Block nowBlock = new Block();
            if (blockStack.Count > 0)
            {
                nowBlock.parent = blockStack[blockStack.Count - 1];
            }
            blockStack.Add(nowBlock);
            ExprListNode param = (ExprListNode)(n.GetChild(0));
            List<string> paramNameList = new List<string>();
            List<object> defaultValueList = new List<object>();
            if (param.children != null)
            {
                foreach (var i in param.children)
                {
                    if (i.type == NodeType.InnerAssign)//默认值：param=value
                    {
                        paramNameList.Add(i.GetChild(0).data);
                        defaultValueList.Add(i.GetChild(1).data);
                    }
                    else//param
                    {
                        paramNameList.Add(i.data);
                        defaultValueList.Add(null);
                    }
                }
            }

            var jumpCode = AddToCode(VMCodeType.Jump);//执行时跳过代码块定义代码
            nowBlock.address = GetNextCodeAddress();//块代码开始地址
            Exec(n.GetChild(1));//代码列表 Body
            //如果没有返回最后默认返回空
            AddToCode(VMCodeType.Null);
            AddToCode(VMCodeType.Return);
            jumpCode.data = GetNextCodeAddress();//设置跳过代码块定义的代码位置
            nowBlock.paramNameList = paramNameList;
            nowBlock.paramDefaultValueList = defaultValueList;
            AddToCode(VMCodeType.Block, nowBlock);
            blockStack.Remove(nowBlock);
        }

        //指令列表 (多条指令都会进入这里执行)
        public void _StateList(ASTNode n)
        {
            foreach (var child in n.children)
            {
                Exec(child);
            }
        }

        //等待语句
        void _Wait(ASTNode n)
        {
            Exec(n.GetChild(0));
            AddToCode(VMCodeType.Wait);
        }

        //循环语句
        void _For(ASTNode n)
        {
            //存储循环变量
            string name = n.GetChild(0).data;
            string itrName = name + "__itr__";
            //取容器
            Exec(n.GetChild(1));
            //创建容器的迭代器
            AddToCode(VMCodeType.Iterator);
            //存储迭代器
            AddToCode(VMCodeType.Store, itrName);
            //循环开始位置
            int forStartAddress = GetNextCodeAddress();
            //加载迭代器:判断
            AddToCode(VMCodeType.Load, itrName);
            AddToCode(VMCodeType.Iterator_HasNext);
            //判断
            var jumpCode = AddToCode(VMCodeType.If_False_Jump);//如果没有元素了，跳出循环
            //加载迭代器:取值
            AddToCode(VMCodeType.Load, itrName);
            AddToCode(VMCodeType.Iterator_MoveNext);
            AddToCode(VMCodeType.Store, name);//存储变量
            Exec(n.GetChild(2));//代码列表
            AddToCode(VMCodeType.Jump, forStartAddress);//回到循环开始
            jumpCode.data = GetNextCodeAddress();
        }

        //条件语句
        void _If(ASTNode n)
        {
            int num = n.children.Count;
            List<VMCode> jumpToEndList = new List<VMCode>();
            for (int i = 0; i < num - 1; i += 2)
            {
                Exec(n.GetChild(i));
                var jumpCode = AddToCode(VMCodeType.If_False_Jump);
                Exec(n.GetChild(i + 1));//代码列表
                jumpToEndList.Add(AddToCode(VMCodeType.Jump));//执行完成后跳到if语句的结束位置
                jumpCode.data = GetNextCodeAddress();
            }
            //else
            var elseNode = n.GetChild(num - 1);
            if (elseNode != null)
            {
                Exec(elseNode);//else代码列表
            }
            int endCodeIndex = GetNextCodeAddress();//结束位置
            foreach (var code in jumpToEndList)
            {
                code.data = endCodeIndex;
            }
        }

        //赋值（存储变量）
        void _Assign(ASTNode n)
        {
            var name = n.GetChild(0).data;
            Exec(n.GetChild(1));
            AddToCode(VMCodeType.Store, name);
        }

        //连续变量赋值
        void _QueueAssign(ASTNode n)
        {
            var queueName = n.GetChild(0);
            //只有一个名字，直接存储
            if (queueName.children.Count == 1)
            {
                var name = queueName.GetChild(0).data;
                Exec(n.GetChild(1));
                AddToCode(VMCodeType.Store, name);
            }
            else//又连续的名字要获取最终需要设置的容器来设置
            {
                AddToCode(VMCodeType.Load, queueName.GetChild(0).data);
                Exec(queueName.GetChild(1));
                for (int i = 2; i < queueName.children.Count; i++)
                {
                    AddToCode(VMCodeType.GetItem);
                    Exec(queueName.GetChild(i));
                }
                Exec(n.GetChild(1));
                AddToCode(VMCodeType.SetItem);
            }
        }

        //中括号获取元素
        void _GetItem(ASTNode n)
        {
            AddToCode(VMCodeType.Load, n.GetChild(0).data);
            Exec(n.GetChild(1));
            AddToCode(VMCodeType.GetItem);
        }

        //中括号设置元素
        void _SetItem(ASTNode n)
        {
            AddToCode(VMCodeType.Load, n.GetChild(0).data);
            Exec(n.GetChild(1));
            Exec(n.GetChild(2));
            AddToCode(VMCodeType.SetItem);
        }

        // 读取数据（加载变量）
        void _LoadName(ASTNode n)
        {
            string name = n.data;
            AddToCode(VMCodeType.Load, name);
        }

        // 连续的内部变量（加载变量）
        void _LoadQueueName(ASTNode n)
        {
            AddToCode(VMCodeType.Load, n.GetChild(0).data);
            for (int i = 1; i < n.children.Count; i++)
            {
                Exec(n.GetChild(i));
                AddToCode(VMCodeType.GetItem);
            }
        }

        // and
        void _And(ASTNode n)
        {
            Exec(n.GetChild(0));
            Exec(n.GetChild(1));
            AddToCode(VMCodeType.And);
        }

        // or
        void _Or(ASTNode n)
        {
            Exec(n.GetChild(0));
            Exec(n.GetChild(1));
            AddToCode(VMCodeType.Or);
        }

        // 判断
        void _Check(ASTNode n)
        {
            Exec(n.GetChild(0));
            Exec(n.GetChild(1));
            switch (n.type)
            {
                case NodeType.CheckEquals:
                    AddToCode(VMCodeType.Check_Equals);
                    break;
                case NodeType.CheckNotEquals:
                    AddToCode(VMCodeType.Check_NotEquals);
                    break;
                case NodeType.CheckGreaterThan:
                    AddToCode(VMCodeType.Check_GreaterThan);
                    break;
                case NodeType.CheckGreaterThanOrEquals:
                    AddToCode(VMCodeType.Check_GreaterThanOrEquals);
                    break;
                case NodeType.CheckLessThan:
                    AddToCode(VMCodeType.Check_LessThan);
                    break;
                case NodeType.CheckLessThanOrEquals:
                    AddToCode(VMCodeType.Check_LessThanOrEquals);
                    break;
            }
        }

        // 四则运算
        void _OP(ASTNode n)
        {
            Exec(n.GetChild(0));
            Exec(n.GetChild(1));
            switch (n.type)
            {
                case NodeType.Plus:
                    AddToCode(VMCodeType.Plus);
                    break;
                case NodeType.Minus:
                    AddToCode(VMCodeType.Minus);
                    break;
                case NodeType.Multiply:
                    AddToCode(VMCodeType.Multiply);
                    break;
                case NodeType.Divide:
                    AddToCode(VMCodeType.Divide);
                    break;
                default:
                    throw new Exception("数字只能加减乘除:" + n.type);
            }

        }

        // 表
        void _Table(ASTNode n)
        {
            AddToCode(VMCodeType.Table);
            float index = 0;
            float count = 0;
            if (n.children != null)
            {
                count = n.children.Count;
                foreach (var i in n.children)
                {
                    if (i.type == NodeType.InnerAssign)//字典：key=value
                    {
                        AddToCode(VMCodeType.String, i.GetChild(0).data);//key
                        Exec(i.GetChild(1));//value
                    }
                    else//数组：value, key为数组下标
                    {
                        AddToCode(VMCodeType.Number, index++);//index
                        Exec(i);//value
                    }
                }
            }
            //插入表数据个数
            AddToCode(VMCodeType.Number, count);
            AddToCode(VMCodeType.Table_InitItem);
        }
    }
}
