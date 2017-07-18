using System;
using Plume.Core;

namespace PlumeTest
{
    /// <summary>
    /// 测试虚拟机语言的所有功能
    /// </summary>
    public class Test_Lan
    {
        /// <summary>
        /// 测试
        /// </summary>
        public void DoTest()
        {
            Console.WriteLine("-------------------Test Lan-------------------");

            //构建虚拟机
            var vm = new PlumeVM();
            //设置代码搜寻路径
            vm.AddCodeFilePath(@"../../Scripts/");            

            //绑定C#类
            vm.BindCSharpClass(typeof(BindCSharpTestData));
            //绑定C#实例
            vm.BindCSharpInstance(BindCSharpTestData.instance, "Data");
            //绑定C#类静态方法
            vm.BindCSharpClassMethod(typeof(BindCSharpTestData), "GetGameName");
            //绑定C#实例方法
            vm.BindCSharpInstanceMethod(BindCSharpTestData.instance, "GetTaskStep");

            //运行虚拟机主代码文件
            vm.Start("test_lan.txt");
            vm.Update();
            vm.Update();
            vm.Update();

            //调用脚本方法
            vm.InvokeCall("fun1");
            vm.InvokeCall("fun2", 10);
            //获取脚本变量
            Console.WriteLine("var1:" + vm.InvokeGet("var1"));
            Console.WriteLine("var2:" + vm.InvokeGet("var2"));

            //主循环
            while (true)
            {
                if (vm.Update())
                    break;
            }
        }
    }
}
