using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 节点类型
    /// </summary>
    enum NodeType
    {
        /// <summary>
        /// Wait 等待
        /// </summary>
        Wait,
        /// <summary>
        /// Load 加载代码
        /// </summary>
        Load,
        /// <summary>
        /// 名词/变量/符号
        /// </summary>
        Name,
        /// <summary>
        /// if 判断
        /// </summary>
        If,
        /// <summary>
        /// if 后的 elif 判断
        /// </summary>
        Elif,
        /// <summary>
        /// if 后的 else 判断
        /// </summary>
        Else,
        /// <summary>
        /// for 循环
        /// </summary>
        For,
        /// <summary>
        /// and 并且
        /// </summary>
        And,
        /// <summary>
        /// or 或者
        /// </summary>
        Or,
        /// <summary>
        /// 返回
        /// </summary>
        Return,
        /// <summary>
        /// 数字(取data)
        /// </summary>
        Number,
        /// <summary>
        /// 字符串(取data)
        /// </summary>
        String,
        /// <summary>
        /// +
        /// </summary>
        Plus,
        /// <summary>
        /// -
        /// </summary>
        Minus,
        /// <summary>
        /// *
        /// </summary>
        Multiply,
        /// <summary>
        /// /
        /// </summary>
        Divide,
        /// <summary>
        /// ==
        /// </summary>
        CheckEquals,
        /// <summary>
        /// !=
        /// </summary>
        CheckNotEquals,
        /// <summary>
        /// >
        /// </summary>
        CheckGreaterThan,
        /// <summary>
        /// >=
        /// </summary>
        CheckGreaterThanOrEquals,
        /// <summary>
        /// <
        /// </summary>
        CheckLessThan,
        /// <summary>
        /// <=
        /// </summary>
        CheckLessThanOrEquals,
        /// <summary>
        /// 指令数组
        /// </summary>
        Stat_List,
        /// <summary>
        /// 表达式数组
        /// </summary>
        Expr_List,
        /// <summary>
        /// 函数调用
        /// </summary>
        Call_Stat,
        /// <summary>
        /// 函数调用
        /// </summary>
        Call_Expr,
        /// <summary>
        /// 加载运行
        /// </summary>
        Load_Stat,
        /// <summary>
        /// 加载赋值
        /// </summary>
        Load_Expr,
        /// <summary>
        /// 连续的变量 name.a.b.c
        /// </summary>
        QueueName,
        /// <summary>
        /// 赋值语句
        /// </summary>
        Assign,
        /// <summary>
        /// 连续变量赋值语句 name.a.b.c = ..
        /// </summary>
        QueueAssign,
        /// <summary>
        /// 指定赋值语句 代码块的指定参数调用：say:name='cx'，代码块的参数默认值，表的初始化[name='cx',sex='male']
        /// </summary>
        InnerAssign,
        /// <summary>
        /// 表
        /// </summary>
        Table,
        /// <summary>
        /// 获取表内元素
        /// </summary>
        Table_GetItem,
        /// <summary>
        /// 设置表内元素
        /// </summary>
        Table_SetItem,
        /// <summary>
        /// 代码块
        /// </summary>
        Block,
    }

    /// <summary>
    /// 节点基类
    /// </summary>
    class ASTNode
    {
        /// <summary>
        /// 类型
        /// </summary>
        public NodeType type;
        /// <summary>
        /// 携带数据
        /// </summary>
        public string data;
        /// <summary>
        /// 子节点
        /// </summary>
        public List<ASTNode> children;

        public ASTNode(NodeType type)
        {
            this.type = type;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ASTNode GetChild(int i)
        {
            return children[i];
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="n"></param>
        protected void AddChild(ASTNode n)
        {
            if (children == null)
                children = new List<ASTNode>();
            children.Add(n);
        }

        public override string ToString()
        {
            return type.ToString();
        }

        /// <summary>
        /// 遍历子节点输出
        /// </summary>
        /// <returns></returns>
        public virtual string ToStringTree(int level = 0)
        {
            if (children == null || children.Count == 0)
                return this.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(this.ToString());
            for (int i = 0; i < children.Count; i++)
            {
                ASTNode child = children[i];
                sb.Append(" ");
                sb.Append(child.ToStringTree(level + 1));
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
