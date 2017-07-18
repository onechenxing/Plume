using System;
using Plume.Core;

namespace PlumeTest
{
    /// <summary>
    /// 测试虚拟机小功能 test.txt
    /// </summary>
    public class Test_Test
    {
        /// <summary>
        /// 测试
        /// </summary>
        public void DoTest()
        {
            Console.WriteLine("-------------------Test VM-------------------");

            //构建虚拟机
            var vm = new PlumeVM();
            //设置代码搜寻路径
            vm.AddCodeFilePath(@"../../Scripts/");            
            //绑定C#实例
            vm.BindCSharpInstance(BindCSharpTestData.instance, "Data");
            //运行虚拟机主代码文件
            vm.Start("test.txt");
            //主循环
            while (true)
            {
                if (vm.Update())
                    break;
            }
        }
    }
}
