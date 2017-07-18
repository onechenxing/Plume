using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 标准输出函数 (逗号分割，最后换行)
    /// </summary>
    class PrintBlock : BaseBuildInBlock
    {
        /// <summary>
        /// 输出函数(默认为Console.Write)
        /// </summary>
        public static Action<string> OutFunc = Console.Write;

        public override object DoBuildIn(List<object> paramValueList)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paramValueList.Count; i++)
            {
                sb.Append(paramValueList[i]);
                if (i != paramValueList.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.AppendLine();
            OutFunc(sb.ToString());

            return null;
        }
    }
}
