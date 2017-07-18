using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器 - 语法语句 if for call return
    /// </summary>
    partial class MyParser : BaseParser
    {

        /// <summary>
        /// 判断
        /// </summary>
        /// <returns></returns>
        IfNode mIf()
        {
            List<IExprNode> exprList = new List<IExprNode>();
            List<StatListNode> bodyList = new List<StatListNode>();
            //if
            Match(TokenType.If);
            exprList.Add(mExpr());
            bodyList.Add(mStatList());
            //elif
            while (true)
            {
                SkipEnd();
                if (GetTokenType() != TokenType.Elif)
                {
                    break;
                }
                Match(TokenType.Elif);
                exprList.Add(mExpr());
                bodyList.Add(mStatList());
            }
            //else
            SkipEnd();
            StatListNode elseNode = null;
            if (GetTokenType() == TokenType.Else)
            {
                Match(TokenType.Else);
                elseNode = mStatList();
            }
            return new IfNode(exprList, bodyList, elseNode);
        }

        /// <summary>
        /// 循环
        /// </summary>
        /// <returns></returns>
        ForNode mFor()
        {
            //For
            Match(TokenType.For);
            var paramName = mName();
            Match(TokenType.In);
            var containerName = mElement();
            var body = mStatList();
            return new ForNode(paramName, containerName, body);
        }

        /// <summary>
        /// 代码块调用(不用用返回值)
        /// </summary>
        CallStatNode mCallStat()
        {
            var id = mQueueName();
            Match(TokenType.Colon);
            //无参调用
            if (GetTokenType() == TokenType.End)
            {
                return new CallStatNode(id, new ExprListNode());
            }
            var value = mExprs();
            return new CallStatNode(id, value);
        }

        /// <summary>
        /// 代码块调用(用返回值)
        /// </summary>
        CallExprNode mCallExpr()
        {
            var id = mQueueName();
            Match(TokenType.Colon);
            //无参调用
            if (GetTokenType() == TokenType.End ||
                GetTokenType() == TokenType.Comma ||
                GetTokenType() == TokenType.RParenthese)
            {
                return new CallExprNode(id, new ExprListNode());
            }
            var value = mExprs();
            return new CallExprNode(id, value);
        }

        /// <summary>
        /// 返回
        /// </summary>
        ReturnNode mReturn()
        {
            Token t = GetToken();
            Match(TokenType.Return);
            if (GetTokenType() != TokenType.End)
            {
                var value = mExpr();
                return new ReturnNode(value);
            }
            return new ReturnNode();
        }
    }
}
