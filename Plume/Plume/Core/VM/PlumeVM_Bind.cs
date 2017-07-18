using System;

namespace Plume.Core
{
    /// <summary>
    /// 虚拟机类 - 与C#绑定和交互
    /// </summary>
    public partial class PlumeVM
    {
        /// <summary>
        /// 运行脚本内函数
        /// </summary>
        /// <param name="functionName">脚本内的全局代码块名字</param>
        /// <param name="param">按顺序传递所有参数</param>
        public object InvokeCall(string functionName, params object[] param)
        {
            if (DEBUG)
            {
                PrintBlock.OutFunc("<InvokeCall>");
            }

            //准备调用函数栈数据
            BaseBlock def = (BaseBlock)InvokeGet(functionName);

            //压入参数
            for (int i = 0; i < param.Length; i++)
            {
                operands[++op] = i;//index
                operands[++op] = param[i];//value
            }
            //压入参数数量
            operands[++op] = param.Length;
            operands[++op] = def;
            //记录当前的帧空间位置
            int stopFp = fp;
            //执行函数
            _Call();
            //执行
            Cpu(stopFp);
            //捕获返回值
            object re = operands[op--];
            return re;
        }

        /// <summary>
        /// 获取脚本内变量值
        /// </summary>
        /// <param name="varName">脚本内的全局变量名</param>
        /// <returns></returns>
        public object InvokeGet(string varName)
        {
            if (DEBUG)
            {
                PrintBlock.OutFunc("<InvokeGet>");
            }

            //先查找主文件
            object obj = frames[0].block.DeepGet(varName);
            //查找全局文件
            if (obj == null)
            {
                obj = globalSpace.Get(varName);
            }
            return obj;
        }

        /// <summary>
        /// 绑定C#类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="useClassName"></param>
        public void BindCSharpClass(Type type, string useClassName = null)
        {
            if (useClassName == null)
            {
                useClassName = type.Name;
            }
            globalSpace.Set(useClassName, new ReflectionClassAsContainer(type));
        }

        /// <summary>
        /// 绑定C#实例
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useObjName"></param>
        public void BindCSharpInstance(object obj, string useObjName)
        {
            globalSpace.Set(useObjName, ReflectionUtil.PackageScriptType(obj));
        }

        /// <summary>
        /// 绑定C#类方法(静态方法)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="useMethodName"></param>
        public void BindCSharpClassMethod(Type type, string methodName, string useMethodName = null)
        {
            if (useMethodName == null)
            {
                useMethodName = methodName;
            }
            globalSpace.Set(useMethodName, new ReflectionMethodAsBlock(type, methodName));
        }

        /// <summary>
        /// 绑定C#实例方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="useMethodName"></param>
        public void BindCSharpInstanceMethod(object obj, string methodName, string useMethodName = null)
        {
            if (useMethodName == null)
            {
                useMethodName = methodName;
            }
            globalSpace.Set(useMethodName, new ReflectionMethodAsBlock(obj, methodName));
        }
    }
}
