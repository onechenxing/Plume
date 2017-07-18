using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器 - 基础 数据类型 获取 赋值
    /// </summary>
    partial class MyParser : BaseParser
    {
        #region 数据类型

        // 数字  negative:是否是负数
        NumberNode mNumber(bool negative = false)
        {
            var token = GetToken();
            string value = token.text;
            if (negative)
                value = "-" + token.text;
            Match(TokenType.Number);
            return new NumberNode(value);
        }

        // 字符串
        StringNode mString()
        {
            var token = GetToken();
            string value = token.text;
            Match(TokenType.String);
            return new StringNode(value);
        }

        // 表: [,]
        TableNode mTable()
        {
            var node = new TableNode();
            Match(TokenType.LBracket);
            if (GetTokenType() == TokenType.RBracket)//空表
            {
                Match(TokenType.RBracket);
            }
            else
            {
                var statList = mExprs();
                Match(TokenType.RBracket);
                node.children = statList.children;
            }
            return node;
        }

        /// <summary>
        /// 代码块: {...} or :xx{...}
        /// </summary>
        public BlockNode mBlock()
        {
            ExprListNode param = new ExprListNode();
            //param
            if (GetTokenType() == TokenType.Colon)
            {
                Match(TokenType.Colon);
                param = mElements();
            }
            SkipEnd();
            //body
            var bodyNode = new StatListNode();
            Match(TokenType.LBrace);
            SkipEnd();
            while (GetTokenType() != TokenType.RBrace)
            {
                bodyNode.Add(mStatement());
                SkipEnd();
            }
            Match(TokenType.RBrace);
            return new BlockNode(param, bodyNode);
        }

        #endregion

        #region 获取

        // 单词 名字/变量 获取
        NameNode mName()
        {
            var token = GetToken();
            string name = token.text;
            Match(TokenType.Name);
            return new NameNode(name);
        }

        // 可连续变量获取 x.a.b or x['a']['b'] or x[0][1]
        QueueNameNode mQueueName()
        {
            List<IExprNode> nameList = new List<IExprNode>();
            nameList.Add(mName());//第一个必然是名字
            while (GetTokenType() == TokenType.Dot || GetTokenType() == TokenType.LBracket)
            {
                if (GetTokenType() == TokenType.Dot)//.处理
                {
                    Match(TokenType.Dot);
                    //只能是名字，并把名字转换成String处理
                    if (GetTokenType() == TokenType.Name)
                    {
                        string nameStr = GetToken().text;
                        Consume();
                        var strNode = new StringNode(nameStr);
                        nameList.Add(strNode);
                    }
                    else
                    {
                        throw new Exception("x.b.c only can Name : " + GetTokenType());
                    }
                }
                else if (GetTokenType() == TokenType.LBracket)//[]处理
                {
                    Match(TokenType.LBracket);
                    nameList.Add(mElement());//支持表达式
                    Match(TokenType.RBracket);
                }
            }
            return new QueueNameNode(nameList);
        }

        #endregion

        #region 赋值/设置

        // 常规赋值 x=..
        AssignNode mAssign()
        {
            var name = mName();
            Match(TokenType.Equals);
            var expr = mExpr();
            return new AssignNode(name, expr);
        }

        // 可连续变量元素赋值 x.a.b=.. or x['a']['b']=.. or x[0][1]=..
        QueueAssignNode mQueueAssign()
        {
            var name = mQueueName();
            Match(TokenType.Equals);
            var expr = mExpr();
            return new QueueAssignNode(name, expr);
        }

        // 默认值设置 [a=..] or :a=..{}
        InnerAssignNode mInnerAssign()
        {
            var id = mName();
            Match(TokenType.Equals);
            var value = mElement();
            return new InnerAssignNode(id, value);
        }

        #endregion
                
    }
}
