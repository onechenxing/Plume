using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 单元测试输出函数 (类似print,最后传入一个测试字符串来对比结果)
    /// </summary>
    class PrintuBlock : BaseBuildInBlock
    {
        /// <summary>
        /// 测试总数
        /// </summary>
        public static int Test_Num = 0;
        /// <summary>
        /// 测试通过数量
        /// </summary>
        public static int Test_Ok = 0;

        /// <summary>
        /// 重置测试数据统计
        /// </summary>
        public static void ResetTest()
        {
            Test_Num = 0;
            Test_Ok = 0;
        }

        public override object DoBuildIn(List<object> paramValueList)
        {
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paramValueList.Count -1 ; i++)
            {
                sb.Append(paramValueList[i]);
                if (i != paramValueList.Count - 2)
                {
                    sb.Append(",");
                }
            }
            string checkStr = paramValueList[paramValueList.Count - 1].ToString();
            string userStr = sb.ToString();
            //美观处理，让显示长度一样
            while (sb.Length < 30)
            {
                sb.Append(" ");
            }
            Test_Num++;
            //显示测试结果
            if (checkStr == userStr)
            {
                sb.Append("(test ok)");
                Test_Ok++;
            }
            else
            {
                sb.Append("(test failure)");
            }
            sb.AppendLine();
            PrintBlock.OutFunc(sb.ToString());

            return null;
        }
    }
}
