using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 原始输出函数(元素输出,不做任何分割)
    /// </summary>
    class PrintcBlock : BaseBuildInBlock
    {

        public override object DoBuildIn(List<object> paramValueList)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paramValueList.Count; i++)
            {
                sb.Append(paramValueList[i]);
            }
            PrintBlock.OutFunc(sb.ToString());

            return null;
        }
    }
}
