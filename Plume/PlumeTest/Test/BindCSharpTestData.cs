using Plume.Core;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 脚本绑定到C#类的测试数据
/// </summary>
public class BindCSharpTestData
{
    private static BindCSharpTestData _instance;
    /// <summary>
    /// 单例
    /// </summary>
    public static BindCSharpTestData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BindCSharpTestData();
            }
            return _instance;
        }
    }


    /// <summary>
    /// 游戏版本(测试静态属性)
    /// </summary>
    public static float ver = 1.01f;

    /// <summary>
    /// 获取游戏名字(测试静态方法)
    /// </summary>
    /// <returns></returns>
    public static string GetGameName()
    {
        return "Test Game";
    }

    /// <summary>
    /// 游戏时间(测试常规属性)
    /// </summary>
    public int money = 100;

    /// <summary>
    /// 任务记录字典
    /// </summary>
    private Dictionary<string, int> taskStepDic = new Dictionary<string, int>();

    BindCSharpTestData()
    {
        InitData();
    }

    void InitData()
    {

    }

    /// <summary>
    /// 获取任务步骤标记(测试常规方法)
    /// </summary>
    /// <param name="name">任务名</param>
    /// <returns></returns>
    public int GetTaskStep(string name)
    {
        if (taskStepDic.ContainsKey(name))
        {
            return taskStepDic[name];
        }
        return 0;
    }

    /// <summary>
    /// 设置任务步骤标记(测试常规方法)
    /// </summary>
    /// <param name="id">任务名</param>
    /// <param name="step">步骤</param>
    /// <returns></returns>
    public void SetTaskStep(string name, int step)
    {
        taskStepDic[name] = step;
    }

    /// <summary>
    /// 等待测试(测试自定义等待)
    /// </summary>
    /// <returns>等待对象</returns>
    public IWait WaitTest()
    {
        var w = new WaitSignal();
        new Thread(() =>
        {
            Thread.Sleep(500);
            lock (w)
            {
                w.isFinish = true;
            }
        }).Start();
        return w;
    }

}

