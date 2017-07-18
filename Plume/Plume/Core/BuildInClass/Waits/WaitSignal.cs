using System;

namespace Plume.Core
{
    /// <summary>
    /// 等待信号类 (能让外部函数提供wait支持)
    /// </summary>
    public class WaitSignal : IWait
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isFinish
        {
            get;set;
        }

        /// <summary>
        /// 等待
        /// </summary>
        public WaitSignal()
        {
            isFinish = false;
        }

        public bool WaitOK()
        {
            return isFinish;
        }
    }
}
