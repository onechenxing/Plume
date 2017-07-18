using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 反射C#方法 (当作代码里的代码块使用)
    /// </summary>
    class ReflectionMethodAsBlock : BaseBuildInBlock
    {
        /// <summary>
        /// C#类型
        /// </summary>
        private Type _type;

        /// <summary>
        /// C#实例
        /// </summary>
        private object _obj;

        /// <summary>
        /// 方法名
        /// </summary>
        private string _methodName;

        /// <summary>
        /// 根据C#类构建反射调用
        /// </summary>
        /// <param name="classType">C#类型</param>
        /// <param name="methodName">方法名</param>
        public ReflectionMethodAsBlock(Type classType, string methodName)
        {
            this._type = classType;
            this._obj = null;
            this._methodName = methodName;

            Init();
        }

        /// <summary>
        /// 根据C#实例构建反射调用
        /// </summary>
        /// <param name="obj">C#实例</param>
        /// <param name="methodName">方法名</param>
        public ReflectionMethodAsBlock(object obj, string methodName)
        {
            this._type = obj.GetType();
            this._obj = obj;
            this._methodName = methodName;

            Init();
        }

        private void Init()
        {
            //绑定方法的参数列表
            paramNameList = new List<string>();
            paramDefaultValueList = new List<object>();
            ReflectionUtil.MethodParameters(_type, _methodName, paramNameList, paramDefaultValueList, _obj);
        }

        public override object DoBuildIn(List<object> paramValueList)
        {
            var re = ReflectionUtil.CallMethod(_type, _methodName, paramValueList, _obj);
            re = ReflectionUtil.PackageScriptType(re);//要包装一下再返回给脚本处理
            return re;
        }
    }
}
