using System;
using System.Linq;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 词法解析器
    /// </summary>
    class MyLexer : BaseLexer
    {
        /// <summary>
        /// 特殊符号
        /// </summary>
        public static string Symbols = " \t\n\r\\'\".,;:?|!@#$%^&<>()[]{}=+-*/";

        /// <summary>
        /// 是否已经生成最后的结束符
        /// </summary>
        private bool _isEnd = false;

        public MyLexer(string input) : base(input)
        {
        }

        /// <summary>
        /// 获取下一个词元
        /// </summary>
        /// <returns></returns>
        public override Token NextToken()
        {
            while (c != EOF)
            {
                switch (c)
                {
                    //跳过注释
                    case '#':
                        DoNotes();
                        continue;
                    //跳过空白
                    case ' ':
                    case '\t':
                    case '\r':
                        DoWhiteSpace();
                        continue;
                    //End:换行或分号
                    case ';':
                    case '\n':
                        return DoEnd();                    
                    case '+':
                        Consume();
                        return new Token(TokenType.Plus, "+");
                    case '-':
                        Consume();
                        return new Token(TokenType.Minus, "-");
                    case '*':
                        Consume();
                        return new Token(TokenType.Multiply, "*");
                    case '/':
                        Consume();
                        //判断注释
                        if (CheckNotes())
                        {
                            DoNotes();
                            continue;
                        }
                        else
                        {                            
                            return new Token(TokenType.Divide, "/");
                        }
                    case '=':
                        Consume();
                        if (c == '=')
                        {
                            Consume();
                            return new Token(TokenType.CheckEquals, "==");
                        }
                        return new Token(TokenType.Equals, "=");
                    case '!':
                        Consume();
                        if (c == '=')
                        {
                            Consume();
                            return new Token(TokenType.CheckNotEquals, "!=");
                        }
                        throw new Exception("invalid character: !");
                    case '>':
                        Consume();
                        if (c == '=')
                        {
                            Consume();
                            return new Token(TokenType.CheckGreaterThanOrEquals, ">");
                        }
                        return new Token(TokenType.CheckGreaterThan, ">");
                    case '<':                        
                        Consume();
                        if (c == '=')
                        {
                            Consume();
                            return new Token(TokenType.CheckLessThanOrEquals, "<");
                        }
                        return new Token(TokenType.CheckLessThan, "<");
                    case '.':
                        Consume();
                        return new Token(TokenType.Dot, ".");
                    case ',':
                        Consume();
                        return new Token(TokenType.Comma, ",");
                    case ':':
                        Consume();
                        return new Token(TokenType.Colon, ":");
                    case '(':
                        Consume();
                        return new Token(TokenType.LParenthese, "(");
                    case ')':
                        Consume();
                        return new Token(TokenType.RParenthese, ")");
                    case '[':
                        Consume();
                        return new Token(TokenType.LBracket, "[");
                    case ']':
                        Consume();
                        return new Token(TokenType.RBracket, "]");
                    case '{':
                        Consume();
                        return new Token(TokenType.LBrace, "{");
                    case '}':
                        Consume();
                        return new Token(TokenType.RBrace, "}");                    
                    case '\'':
                    case '"':
                        return DoString(c);
                    default:
                        //数字
                        if (IsNumber())
                            return DoNumber();
                        //字母开头
                        if (IsLetter8Begin())
                            return DoName();
                        throw new Exception("invalid character: " + c);
                }
            }
            if (_isEnd == false)
            {
                _isEnd = true;
                //最后给一个换行结束符,防止每次都要判断结束还是文件尾
                return new Token(TokenType.End, "\\n");
            }
            //真没有了
            return new Token(TokenType.EOF, "<EOF>");
        }

        /// <summary>
        /// 判断注释
        /// </summary>
        /// <returns></returns>
        private bool CheckNotes()
        {
            if (c == '/' || c == '*')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 跳过注释
        /// 单行: // 多行: /* ... */
        /// </summary>
        private void DoNotes()
        {
            //单行注释
            if (c == '/')
            {
                Consume();
                while (c != '\n')
                    Consume();
            }
            if (c == '*')
            {
                Consume();
                while (true)
                {
                    if (c == '*')
                    {
                        Consume();
                        if (c == '/')
                        {
                            Consume();
                            return;
                        }
                    }
                    Consume();
                }
            }            
        }

        /// <summary>
        /// 跳过空白
        /// </summary>
        private void DoWhiteSpace()
        {
            while (c == ' ' || c == '\t' || c == '\r')
                Consume();
        }

        /// <summary>
        /// 寻找名词
        /// </summary>
        /// <returns></returns>
        Token DoName()
        {
            StringBuilder buf = new StringBuilder();
            do
            {
                buf.Append(c);
                Consume();
            } while (IsLetter8());
            string str = buf.ToString();
            //关键字过滤
            switch (str)
            {                
                case "if":
                    return new Token(TokenType.If, "if");
                case "elif":
                    return new Token(TokenType.Elif, "elif");
                case "else":
                    return new Token(TokenType.Else, "else");
                case "for":
                    return new Token(TokenType.For, "for");
                case "in":
                    return new Token(TokenType.In, "in");
                case "and":
                    return new Token(TokenType.And, "and");
                case "or":
                    return new Token(TokenType.Or, "or");
                case "return":
                    return new Token(TokenType.Return, "return");
                case "wait":
                    return new Token(TokenType.Wait, "wait");
                case "load":
                    return new Token(TokenType.Load, "load");
            }
            return new Token(TokenType.Name, buf.ToString());
        }

        /// <summary>
        /// 是否是符号
        /// </summary>
        /// <returns></returns>
        bool IsSymbol()
        {
            return Symbols.Any<char>((i) => i == c);
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <returns></returns>
        bool IsNumber()
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// 是否是英文字符
        /// </summary>
        /// <returns></returns>
        bool IsLetter()
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        /// <summary>
        /// 是否是名词开始（数字不能作为名词开始）
        /// </summary>
        /// <returns></returns>
        bool IsLetter8Begin()
        {
            return IsLetter8() && !IsNumber();
        }

        /// <summary>
        /// 是否是名词字符（包含除符号外的所有字符）
        /// </summary>
        /// <returns></returns>
        bool IsLetter8()
        {
            return c != EOF && !IsSymbol();
        }

        /// <summary>
        /// 数字处理
        /// </summary>
        /// <returns></returns>
        Token DoNumber()
        {
            //数值只能出现一次.
            bool findDot = false;
            StringBuilder buf = new StringBuilder();
            do
            {
                if (c == '.')
                {
                    findDot = true;
                }
                buf.Append(c);
                Consume();
            } while (IsNumber() || (findDot == false && c == '.'));
            return new Token(TokenType.Number, buf.ToString());
        }

        /// <summary>
        /// 字符串处理
        /// </summary>
        /// <returns></returns>
        Token DoString(char q)
        {
            StringBuilder buf = new StringBuilder();
            Consume();//去除前置引号
            while (c != q)
            {
                buf.Append(c);
                Consume();
            }
            Consume();//去除后置引号
            return new Token(TokenType.String, buf.ToString());
        }

        /// <summary>
        /// 处理换行结束符
        /// </summary>
        Token DoEnd()
        {
            //去除连续的，只留一个
            while (c == ' ' || c == '\t' || c == '\n' || c == '\r' || c == ';')
                Consume();
            return new Token(TokenType.End, "\\n");
        }
    }
}
