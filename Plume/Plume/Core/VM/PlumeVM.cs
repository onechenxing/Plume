using System;
using System.IO;

//整体流程： Lexer(解析词法，产生Token) -> Parser(解析语法,生成AST树) -> AST(抽象语法树) -> VMCode(虚拟机中间语言代码) -> VM(虚拟机执行程序)

namespace Plume.Core
{
    /// <summary>
    /// Plume语言虚拟机类
    /// </summary>
    public partial class PlumeVM
    {
        public PlumeVM()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //添加内建代码块
            globalSpace.Set("print", new PrintBlock());
            globalSpace.Set("printc", new PrintcBlock());
            globalSpace.Set("printu", new PrintuBlock());
            globalSpace.Set("len", new LenBlock());
            globalSpace.Set("range", new RangeBlock());            
        }

        /// <summary>
        /// 运行
        /// <param name="mainCodeFile">主代码文件</param>
        /// </summary>
        public void Start(string mainCodeFile)
        {
            //重置参数
            PrintuBlock.ResetTest();
            cp = 0;
            op = -1;
            fp = -1;
            _isFinish = false;
            //加载代码
            string codeStr = LoadCodeFile(mainCodeFile);
            Block startBlock = BuildCodesBlock(codeStr);
            startBlock.name = "main";
            codes = codeBuilder.GetCodes();
            //初始第一针空间为主代码块入口(-1表示Return返回时直接退出主循环了)
            frames[++fp] = new VMStackFrame("main", -1, startBlock);
            
            if (DEBUG)
            {
                PrintBlock.OutFunc("\n------ VM Start ------\n");
            }
        }              

        /// <summary>
        /// 虚拟机更新
        /// </summary>
        /// <returns>是否当前代码执行完毕，如果执行完毕返回true，如果没有完毕返回false</returns>
        public bool Update()
        {
            return Cpu();
        }

        /// <summary>
        /// 是否当前代码执行完毕
        /// </summary>
        /// <returns></returns>
        public bool IsFinish
        {
            get
            {
                return _isFinish;
            }
        }

        /// <summary>
        /// 设置Print的输出函数
        /// </summary>
        /// <param name="func"></param>
        public void SetPrintOutFunc(Action<string> func)
        {
            PrintBlock.OutFunc = func;
        }

        /// <summary>
        /// 添加代码搜寻路径
        /// </summary>
        /// <param name="path"></param>
        public void AddCodeFilePath(string path)
        {
            loadCodePaths.Add(path);
        }

        /// <summary>
        /// 加载代码文本
        /// </summary>
        /// <param name="codeFilePath">代码路径</param>
        /// <returns></returns>
        string LoadCodeFile(string codeFilePath)
        {
            string findPath = "";
            if (File.Exists(codeFilePath))
            {
                findPath = codeFilePath;
            }
            else//搜寻配置路径
            {
                for (int i = loadCodePaths.Count - 1; i >= 0; i--)
                {
                    string path = loadCodePaths[i];
                    path = Path.Combine(path, codeFilePath);
                    if (File.Exists(path))
                    {
                        findPath = path;
                        break;
                    }
                }
            }
            if (findPath == "")
            {
                throw new Exception("Not find code file:" + codeFilePath);
            }
            //读取文本
            string codeStr = File.ReadAllText(findPath);
            return codeStr;
        }

        /// <summary>
        /// 根据字符串生成代码块
        /// </summary>
        /// <param name="codeStr"></param>
        /// <returns></returns>
        Block BuildCodesBlock(string codeStr)
        {
            var lexer = new MyLexer(codeStr);
            var parser = new MyParser(lexer);
            var astNode = parser.Build();
            return codeBuilder.BuildCodesBlock(astNode);
        }
    }
}
