using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 内建代码块定义的基类
    /// </summary>
    abstract class BaseBuildInBlock : BaseBlock
    {
        public virtual object DoBuildIn(List<object> paramValueList)
        {
            return null;
        }
    }
}
