using System.Collections;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 虚拟机类 - 参数列表
    /// </summary>
    public partial class PlumeVM
    {
        /// <summary>
        /// 调试输出开关
        /// </summary>
        public static bool DEBUG = false;

        /// <summary>
        /// 代码构建器
        /// </summary>
        private VMCodeBuilder codeBuilder = new VMCodeBuilder();
        /// <summary>
        /// 代码数据列表
        /// </summary>
        private List<VMCode> codes;
        /// <summary>
        /// 当前代码位置
        /// </summary>
        private int cp = 0;

        /// <summary>
        /// 操作数栈
        /// </summary>
        private object[] operands = new object[1000];
        /// <summary>
        /// 操作数指针
        /// </summary>
        private int op = -1;

        /// <summary>
        /// 调用帧空间
        /// </summary>
        private VMStackFrame[] frames = new VMStackFrame[100];
        /// <summary>
        /// 帧空间指针
        /// </summary>
        private int fp = -1;

        /// <summary>
        /// 全局存储器
        /// </summary>
        private MemorySpace globalSpace = new MemorySpace("global");

        /// <summary>
        /// 加载代码和指针位置记录
        /// </summary>
        private Dictionary<string, Block> loadCodeDic = new Dictionary<string, Block>();

        /// <summary>
        /// 搜寻代码路径
        /// </summary>
        private List<string> loadCodePaths = new List<string>();

        /// <summary>
        /// 当前代码是否执行完毕
        /// </summary>
        private bool _isFinish = false;
    }
}
