using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 语法解析器基类
    /// </summary>
    abstract class BaseParser
    {
        /// <summary>
        /// 词法解析器
        /// </summary>
        protected BaseLexer input;
        /// <summary>
        /// 记录标记
        /// </summary>
        protected List<int> markers;
        /// <summary>
        /// 查找列表
        /// </summary>
        protected List<Token> lookahead;
        /// <summary>
        /// 当前位置
        /// </summary>
        int p = 0;

        public BaseParser(BaseLexer input)
        {
            this.input = input;
            markers = new List<int>();
            lookahead = new List<Token>();
            Sync(1);
        }

        /// <summary>
        /// 申请向后查看的词元空间
        /// </summary>
        /// <param name="i"></param>
        public void Sync(int i)
        {
            //空间不够
            if (p + i - 1 > (lookahead.Count - 1))
            {
                int n = (p + i - 1) - (lookahead.Count - 1);
                Fill(n);
            }
        }

        /// <summary>
        /// 填充向后查看的词元空间
        /// </summary>
        /// <param name="n"></param>
        public void Fill(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                lookahead.Add(input.NextToken());
            }
        }

        /// <summary>
        /// 第i个词元
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Token GetToken(int i = 1)
        {
            Sync(i);
            return lookahead[p + i - 1];
        }

        /// <summary>
        /// 第i个词元的类型
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TokenType GetTokenType(int i = 1)
        {
            return GetToken(i).type;
        }

        /// <summary>
        /// 匹配一个类型
        /// </summary>
        /// <param name="t"></param>
        public void Match(TokenType t)
        {
            /*输出匹配结果
            if (!IsSpeculating())
            {
                Console.WriteLine("Match:" + GetToken(1));
            }
            //*/
            if (GetTokenType(1) == t)
                Consume();
            else
                throw new Exception(string.Format("Match:{0},found:{1}", t.ToString(), GetTokenType().ToString()));
        }

        /// <summary>
        /// 匹配成功，进入下一个
        /// </summary>
        public void Consume()
        {
            p++;
            if (p == lookahead.Count && !IsSpeculating())
            {
                p = 0;
                lookahead.Clear();
            }
            Sync(1);
        }

        /// <summary>
        /// 标记开始位置
        /// </summary>
        /// <returns></returns>
        public int Mark()
        {
            markers.Add(p);
            return p;
        }

        /// <summary>
        /// 释放当前标记
        /// </summary>
        public void Release()
        {
            int marker = markers[markers.Count - 1];
            markers.RemoveAt(markers.Count - 1);
            Seek(marker);
        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="index"></param>
        public void Seek(int index)
        {
            p = index;
        }

        /// <summary>
        /// 是否在推断规则中
        /// </summary>
        /// <returns></returns>
        public bool IsSpeculating()
        {
            return markers.Count > 0;
        }
    }
}
