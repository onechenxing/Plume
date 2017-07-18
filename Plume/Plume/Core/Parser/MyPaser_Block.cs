using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器 - 内置函数 wait  load
    /// </summary>
    partial class MyParser : BaseParser
    {
        /// <summary>
        /// Wait等待调用
        /// </summary>
        /// <returns></returns>
        WaitNode mWait()
        {
            Match(TokenType.Wait);
            Match(TokenType.Colon);
            var value = mExpr();
            return new WaitNode(value);
        }

        /// <summary>
        /// 代码块调用(不用用返回值)
        /// </summary>
        LoadStatNode mLoadStat()
        {
            Match(TokenType.Load);
            Match(TokenType.Colon);
            var path = mString();
            return new LoadStatNode(path);
        }

        /// <summary>
        /// 代码块调用(用返回值)
        /// </summary>
        LoadExprNode mLoadExpr()
        {
            Match(TokenType.Load);
            Match(TokenType.Colon);
            var path = mString();
            return new LoadExprNode(path);
        }          

    }
}
