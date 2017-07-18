using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 
    /// 范围函数
    /// </summary>
    class RangeBlock : BaseBuildInBlock
    {

        public override object DoBuildIn(List<object> paramValueList)
        {
            if (paramValueList.Count == 1)
            {
                return new RangeIterator(0, (float)paramValueList[0]);
            }
            if (paramValueList.Count == 2)
            {
                return new RangeIterator((float)paramValueList[0], (float)paramValueList[1]);
            }
            if (paramValueList.Count == 3)
            {
                return new RangeIterator((float)paramValueList[0], (float)paramValueList[1], (float)paramValueList[2]);
            }
            return null;
        }
    }
}
