using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 符号类型
    /// </summary>
    enum TokenType
    {
        /// <summary>
        /// 文件结尾
        /// </summary>
        EOF,
        /// <summary>
        /// 结束符 \n或;
        /// </summary>
        End,
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
        /// for 循环中的 in 标记
        /// </summary>
        In,
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
        /// .
        /// </summary>
        Dot,
        /// <summary>
        /// ,
        /// </summary>
        Comma,
        /// <summary>
        /// :
        /// </summary>
        Colon,
        /// <summary>
        /// =
        /// </summary>
        Equals,
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
        /// (
        /// </summary>
        LParenthese,
        /// <summary>
        /// )
        /// </summary>
        RParenthese,
        /// <summary>
        /// [
        /// </summary>
        LBracket,
        /// <summary>
        /// ]
        /// </summary>
        RBracket,
        /// <summary>
        /// {
        /// </summary>
        LBrace,
        /// <summary>
        /// }
        /// </summary>
        RBrace,
    }

    /// <summary>
    /// Token类
    /// </summary>
    class Token
    {
        public const int INVALID_TOKEN_TYPE = 0;
        public const int ASSIGN = 7;
        public const int PRINT = 8;
        public const int STAT_LIST = 9;       

        /// <summary>
        /// 类型
        /// </summary>
        public TokenType type;
        /// <summary>
        /// 文本
        /// </summary>
        public string text;

        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public Token(TokenType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return this.text;
            //return string.Format("<{0}:{1}>",this.type.ToString(),this.text);
        }
    }
}
