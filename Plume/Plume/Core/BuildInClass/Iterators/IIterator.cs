using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 容器迭代器  迭代表和字符串
    /// </summary>
    interface IIterator
    {
        /// <summary>
        /// 是否有下一个元素
        /// </summary>
        /// <returns></returns>
        bool HasNext();
        /// <summary>
        /// 移动并获取下一个元素
        /// </summary>
        /// <returns></returns>
        object MoveNext();
    }
}
