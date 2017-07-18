
namespace Plume.Core
{
    /// <summary>
    /// 虚拟机代码的操作类型
    /// </summary>
    enum VMCodeType
    {
        
        /// <summary>
        /// 弹出栈元素
        /// </summary>
        Pop,
        /// <summary>
        /// 代码终止
        /// </summary>
        Halt,
        /// <summary>
        /// 空
        /// </summary>
        Null,

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
        Check_Equals,
        /// <summary>
        /// !=
        /// </summary>
        Check_NotEquals,
        /// <summary>
        /// >
        /// </summary>
        Check_GreaterThan,
        /// <summary>
        /// >=
        /// </summary>
        Check_GreaterThanOrEquals,
        /// <summary>
        /// <
        /// </summary>
        Check_LessThan,
        /// <summary>
        /// <=
        /// </summary>
        Check_LessThanOrEquals,

        /// <summary>
        /// and
        /// </summary>
        And,
        /// <summary>
        /// or
        /// </summary>
        Or,

        /// <summary>
        /// 跳转到代码行(取data)
        /// </summary>
        Jump,
        /// <summary>
        /// 条件True跳转到代码行(取data)
        /// </summary>
        If_True_Jump,
        /// <summary>
        /// 条件False跳转到代码行(取data)
        /// </summary>
        If_False_Jump,

        /// <summary>
        /// 数值(取data)
        /// </summary>
        Number,
        /// <summary>
        /// 字符串(取data)
        /// </summary>
        String,
        /// <summary>
        /// 迭代器(新建)
        /// </summary>
        Iterator,
        /// <summary>
        /// 迭代器 有没有下一个
        /// </summary>
        Iterator_HasNext,
        /// <summary>
        /// 迭代器 移动并获取下一个元素
        /// </summary>
        Iterator_MoveNext,
        /// <summary>
        /// 表(新建)
        /// </summary>
        Table,
        /// <summary>
        /// 初始化数据
        /// </summary>
        Table_InitItem,        
        /// <summary>
        /// 代码块(取data)
        /// </summary>
        Block,
        /// <summary>
        /// 调用 入栈顺序：参数列表-参数数量-代码块引用(栈顶)
        /// </summary>
        Call,
        /// <summary>
        /// 返回值
        /// </summary>
        Return,

        /// <summary>
        /// 设置容器或代码块内部元素 入栈顺序：容器-key-value(栈顶)
        /// </summary>
        SetItem,
        /// <summary>
        /// 取出容器或代码块内部元素 入栈顺序：容器-key-value(栈顶)
        /// </summary>
        GetItem,
                
        /// <summary>
        /// 加载作用域数据(取data)
        /// </summary>
        Load,
        /// <summary>
        /// 存储作用域数据(取data)
        /// </summary>
        Store,

        /// <summary>
        /// 等待
        /// </summary>
        Wait,
        /// <summary>
        /// 加载代码(新建Block)
        /// </summary>
        LoadCodeFile,
    }

    /// <summary>
    /// 虚拟机单个代码类
    /// </summary>
    class VMCode
    {
        /// <summary>
        /// 类型
        /// </summary>
        public VMCodeType type;
        /// <summary>
        /// 携带数据
        /// </summary>
        public object data;

        public VMCode(VMCodeType type)
        {
            this.type = type;
        }

        public VMCode(VMCodeType type, object data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
