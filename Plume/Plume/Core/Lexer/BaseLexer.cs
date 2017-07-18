using System;

namespace Plume.Core
{
    /// <summary>
    /// 词法解析器基类
    /// </summary>
    abstract class BaseLexer
    {
        /// <summary>
        /// 结束符
        /// </summary>
        public static readonly char EOF = char.MaxValue;
        /// <summary>
        /// 输入数据
        /// </summary>
        private string input;
        /// <summary>
        /// 当前位置
        /// </summary>
        private int p = 0;
        /// <summary>
        /// 当前字符
        /// </summary>
        protected char c;

        public BaseLexer(string input)
        {
            this.input = input;
            c = input[p];
        }

        /// <summary>
        /// 向前移动一个位置，直到结尾
        /// </summary>
        public void Consume()
        {
            p++;
            if (p >= input.Length)
                c = EOF;
            else
                c = input[p];
        }
        
        /// <summary>
        /// 匹配当前字符并前进一个位置
        /// </summary>
        /// <param name="x"></param>
        public void Match(char x)
        {
            if (c == x)
                Consume();
            else
                throw new Exception("expecting " + x + "; found " + c);
        }


        /// <summary>
        /// 获得当前代码位置和信息(用于报错提示)
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPosMsg()
        {
            string re = input.Substring(0, p);
            string[] res = re.Split('\n');
            int i;
            for (i = res.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(res[i]))
                {
                    re = res[i];
                    break;
                }
            }
            re = string.Format("line:{0} code:{1}",i+1,re);//输出代码行和内容
            return re;
        }

        /// <summary>
        /// 获得下一个Token
        /// </summary>
        /// <returns></returns>
        public abstract Token NextToken();
    }

}