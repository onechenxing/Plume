using System;
using System.Collections.Generic;
using Plume.Core;

namespace PlumeTest
{
    /// <summary>
    /// 测试并行执行多个VM程序
    /// </summary>
    public class Test_MultiVM
    {
        /// <summary>
        /// 测试
        /// </summary>
        public void DoTest()
        {
            Console.WriteLine("-------------------Test multi Exe-------------------");

            string scriptFolder = @"../../Scripts/";
            string demoSpeak = scriptFolder + "demo_speak.txt";
            string demoSpeak2 = scriptFolder + "demo_speak2.txt";


            List<PlumeVM> vms = new List<PlumeVM>();
            vms.Add(BuildFileVM(demoSpeak));
            vms.Add(BuildFileVM(demoSpeak2));
            while (true)
            {
                bool isAllFinish = true;
                //同时更新
                foreach (var vm in vms)
                {
                    if (vm.IsFinish == false)
                    {
                        isAllFinish = false;
                        vm.Update();
                    }
                }
                //都执行完毕,跳出循环
                if (isAllFinish)
                    break;
            }
        }

        public PlumeVM BuildFileVM(string path)
        {
            var vm = new PlumeVM();
            vm.Start(path);
            return vm;
        }
    }
}
