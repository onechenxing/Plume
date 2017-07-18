using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器
    /// </summary>
    partial class MyParser : BaseParser
    {
        public MyParser(BaseLexer input) : base(input)
        {

        }

        /// <summary>
        /// 执行并生成AST树
        /// </summary>
        public StatListNode Build()
        {
            var statList = new StatListNode();
            try
            {
                //忽略前面的换行结束
                while (GetTokenType() == TokenType.End)
                {
                    Consume();
                }
                while (GetTokenType() != TokenType.EOF)
                {
                    statList.Add(mStatement());
                    //每句语句过后要匹配结束符(换行或;)
                    while (GetTokenType() == TokenType.End)
                    {
                        Consume();
                    }
                }
            }
            catch (Exception e)
            {
                //报错提示信息
                string msg = string.Format("Paser Code Error:{1}",e.Message,input.GetCurrentPosMsg());
                throw new Exception(msg, e);
            }
            return statList;
        }

        /// <summary>
        /// 跳过End
        /// </summary>
        public void SkipEnd()
        {
            while (GetTokenType() == TokenType.End)
            {
                Consume();
            }
        }

        // 一个代码语句
        IStatNode mStatement()
        {
            if (GetTokenType(1) == TokenType.Name)
            {
                //检测：调用代码块 丢弃返回值
                if (Speculate_Call())
                {
                    return mCallStat();
                }
            }
            //赋值 x=..
            if (GetTokenType(1) == TokenType.Name && GetTokenType(2) == TokenType.Equals)
            {
                return mAssign();
            }
            //可连续赋值 x.a.b=.. or  x['a']['b']=..
            if (GetTokenType(1) == TokenType.Name &&
                (GetTokenType(2) == TokenType.Dot || GetTokenType(2) == TokenType.LBracket))
            {
                return mQueueAssign();
            }
            //其他单个词可判断的操作
            switch (GetTokenType())
            {
                case TokenType.If://判断
                    return mIf();
                case TokenType.For://循环
                    return mFor();
                case TokenType.Return://返回语句
                    return mReturn();
                case TokenType.Wait://等待
                    return mWait();
                case TokenType.Load://加载代码
                    return mLoadStat();
            }
            throw new Exception("Paser not match Statement:" + GetToken().text + ", type:" + GetTokenType());
        }

        // 大括号的代码语句列表{...}
        StatListNode mStatList()
        {
            SkipEnd();
            var bodyNode = new StatListNode();
            Match(TokenType.LBrace);
            SkipEnd();
            while (GetTokenType() != TokenType.RBrace)
            {
                bodyNode.Add(mStatement());
                SkipEnd();
            }
            Match(TokenType.RBrace);

            return bodyNode;
        }

        // 多个元素
        ExprListNode mElements()
        {
            var node = new ExprListNode();
            node.Add(mElement());
            while (GetTokenType(1) == TokenType.Comma)
            {
                Match(TokenType.Comma);
                node.Add(mElement());
            }
            return node;
        }

        // 单个元素
        IExprNode mElement()
        {
            if (GetTokenType(1) == TokenType.Name)
            {
                //检测：调用代码块 取返回值
                if (Speculate_Call())
                {
                    return mCallExpr();
                }
            }
            //可连续变量获取 x.a.b or x['a']['b']
            if ((GetTokenType(1) == TokenType.Name) &&
                (GetTokenType(2) == TokenType.Dot || GetTokenType(2) == TokenType.LBracket))
            {
                return mQueueName();
            }
            //代码块 :{} or var xx = {}
            if ((GetTokenType() == TokenType.Colon || GetTokenType() == TokenType.LBrace))
            {
                return mBlock();
            }
            //默认值表达式 x=1
            if (GetTokenType(1) == TokenType.Name && GetTokenType(2) == TokenType.Equals && GetTokenType(3) != TokenType.Equals)
            {
                return mInnerAssign();
            }
            //负数处理
            if (GetTokenType(1) == TokenType.Minus && GetTokenType(2) == TokenType.Number)
            {
                Match(TokenType.Minus);
                return mNumber(true);
            }
            //单个判断
            switch (GetTokenType())
            {
                case TokenType.Name:
                    return mName();
                case TokenType.Number:
                    return mNumber();
                case TokenType.String:
                    return mString();
                case TokenType.LBracket://列表
                    return mTable();
                case TokenType.LParenthese://括号表达式
                    return mInExpr();
                case TokenType.Load://加载代码
                    return mLoadExpr();
            }
            throw new Exception("Paser not match Element:" + GetToken().text + ", type:" + GetTokenType());
        }


        #region 推断函数

        //推断 连续变量 代码块调用
        public bool Speculate_Call()
        {
            bool success = true;
            Mark();
            try
            {
                mQueueName();
                Match(TokenType.Colon);
            }
            catch (Exception) { success = false; }
            Release();
            return success;
        }

        //推断 连续变量 赋值
        public bool Speculate_Assign()
        {
            bool success = true;
            Mark();
            try
            {
                mQueueName();
                Match(TokenType.Equals);
            }
            catch (Exception) { success = false; }
            Release();
            return success;
        }

        #endregion

    }
}
