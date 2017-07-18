using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器 - 表达式递归
    /// </summary>
    partial class MyParser : BaseParser
    {
        // 多表达式 , , ,
        ExprListNode mExprs()
        {
            var node = new ExprListNode();
            node.Add(mExpr());
            while (GetTokenType(1) == TokenType.Comma)
            {
                Match(TokenType.Comma);
                node.Add(mExpr());
            }
            return node;
        }


        // 括号 表达式 ()
        IExprNode mInExpr()
        {
            Match(TokenType.LParenthese);
            var expr = mExpr();
            Match(TokenType.RParenthese);
            return expr;
        }

        // 单表达式
        IExprNode mExpr()
        {
            return mOrExpr();//又or开始一直递归下去
        }

        // 条件或 表达式 or
        IExprNode mOrExpr()
        {
            IExprNode node = mAndExpr();
            while (GetTokenType() == TokenType.Or)
            {
                Match(TokenType.Or);
                var right = mAndExpr();
                node = new OrNode(node, right);
            }
            return node;
        }

        // 条件与 表达式 and
        IExprNode mAndExpr()
        {
            IExprNode node = mCheckExpr();
            while (GetTokenType() == TokenType.And)
            {
                Match(TokenType.And);
                var right = mCheckExpr();
                node = new AndNode(node, right);
            }
            return node;
        }

        // 关系 表达式 < > <= >=
        IExprNode mCheckExpr()
        {
            IExprNode node = mAddExpr();
            //条件判断 == > < >= <=
            while (GetTokenType() == TokenType.CheckEquals || GetTokenType() == TokenType.CheckNotEquals ||
                GetTokenType() == TokenType.CheckGreaterThan || GetTokenType() == TokenType.CheckGreaterThanOrEquals ||
                GetTokenType() == TokenType.CheckLessThan || GetTokenType() == TokenType.CheckLessThanOrEquals)
            {
                NodeType type = NodeType.CheckEquals;
                switch (GetTokenType())
                {
                    case TokenType.CheckEquals:
                        type = NodeType.CheckEquals;
                        break;
                    case TokenType.CheckNotEquals:
                        type = NodeType.CheckNotEquals;
                        break;
                    case TokenType.CheckGreaterThan:
                        type = NodeType.CheckGreaterThan;
                        break;
                    case TokenType.CheckGreaterThanOrEquals:
                        type = NodeType.CheckGreaterThanOrEquals;
                        break;
                    case TokenType.CheckLessThan:
                        type = NodeType.CheckLessThan;
                        break;
                    case TokenType.CheckLessThanOrEquals:
                        type = NodeType.CheckLessThanOrEquals;
                        break;
                }
                var token = GetToken();
                Consume();
                var right = mAddExpr();               
                node = new CheckNode(type, node , right);
            }

            return node;
        }

        // 加减 表达式 + -
        IExprNode mAddExpr()
        {
            IExprNode node = mMulExpr();
            while (GetTokenType() == TokenType.Plus || GetTokenType() == TokenType.Minus)
            {
                if (GetTokenType() == TokenType.Plus)
                {
                    Match(TokenType.Plus);
                    var right = mMulExpr();
                    node = new PlusNode(node,right);
                }
                if (GetTokenType() == TokenType.Minus)
                {
                    Match(TokenType.Minus);
                    var right = mMulExpr();
                    node = new MinusNode(node, right);
                }
            }

            return node;
        }

        // 乘除 表达式 * /
        IExprNode mMulExpr()
        {
            IExprNode node = mElement();
            while (GetTokenType() == TokenType.Multiply || GetTokenType() == TokenType.Divide)
            {
                if (GetTokenType() == TokenType.Multiply)
                {
                    Match(TokenType.Multiply);
                    var right = mElement();
                    node = new MultiplyNode(node, right);
                }
                if (GetTokenType() == TokenType.Divide)
                {
                    Match(TokenType.Divide);
                    var right = mElement();
                    node = new DivideNode(node, right);
                }
            }

            return node;
        }
    }
}
