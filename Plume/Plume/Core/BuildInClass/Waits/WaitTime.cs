using System;

namespace Plume.Core
{
    /// <summary>
    /// 按时间秒数等待 (提供wait的时间等待功能)
    /// </summary>
    class WaitTime : IWait
    {
        /// <summary>
        /// 开始等待时间
        /// </summary>
        private DateTime _startWaitTime;
        /// <summary>
        /// 等待多少秒
        /// </summary>
        private float _waitTime;

        /// <summary>
        /// 等待
        /// </summary>
        /// <param name="sec">秒</param>
        public WaitTime(float sec)
        {
            _waitTime = sec;
            _startWaitTime = DateTime.Now;
        }

        public bool WaitOK()
        {
            return (DateTime.Now - _startWaitTime).TotalSeconds >= _waitTime;
        }
    }
}
