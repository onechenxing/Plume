using System;
using Plume.Core;

namespace PlumeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //测试
            PlumeVM.DEBUG = true;

            DateTime startTime,endTime;
            startTime = DateTime.Now;

            //具体测试内容
            new Test_Lan().DoTest();//所有语法的单元测试：test_lan.txt
            //new Test_Test().DoTest();//自定义虚拟机测试：test.txt
            //new Test_MultiVM().DoTest();//多虚拟机执行测试：demo_speak

            endTime = DateTime.Now;
            Console.WriteLine("time:" + (endTime - startTime).TotalSeconds);

            Console.ReadLine();
        }
    }
}
