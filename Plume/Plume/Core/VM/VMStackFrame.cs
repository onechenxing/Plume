using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 调用栈的帧空间
    /// </summary>
    class VMStackFrame
    {
        /// <summary>
        /// 帧名字
        /// </summary>
        public string name;
        /// <summary>
        /// 返回代码地址
        /// </summary>
        public int returnAddress;
        /// <summary>
        /// 关联block
        /// </summary>
        public Block block;

        public VMStackFrame(string name, int returnAddress, Block block)
        {
            this.name = name;
            this.returnAddress = returnAddress;
            this.block = block;
        }
    }
}
