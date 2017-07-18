using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 
    //取长度函数 (支持表(仅获取下一个最大数字下标)和字符串)
    /// </summary>
    class LenBlock : BaseBuildInBlock
    {

        public override object DoBuildIn(List<object> paramValueList)
        {
            var obj = paramValueList[0];
            if (obj is Table)
            {
                return (obj as Table).Length;
            }
            if (obj is string)
            {
                return (obj as string).Length;
            }

            return null;
        }
    }
}
