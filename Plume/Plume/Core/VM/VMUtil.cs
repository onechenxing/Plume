using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// VM辅助类
    /// </summary>
    class VMUtil
    {
        /// <summary>
        /// 是否是虚拟机里能够识别的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSupportType(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            if (obj is float)
            {
                return true;
            }
            if (obj is string)
            {
                return true;
            }
            if (obj is BaseBlock)
            {
                return true;
            }
            if (obj is IContainer)
            {
                return true;
            }
            if (obj is IIterator)
            {
                return true;
            }
            if (obj is IWait)
            {
                return true;
            }
            return false;
        }
    }
}
