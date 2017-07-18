using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 反射C#类 (当作代码里的容器使用)
    /// </summary>
    class ReflectionClassAsContainer : IContainer
    {
        /// <summary>
        /// C#类型
        /// </summary>
        private Type _type;

        /// <summary>
        /// C#实例
        /// </summary>
        private object _obj;

        public Type type
        {
            get
            {
                return _type;
            }
        }

        public object obj
        {
            get
            {
                return _obj;
            }
        }

        /// <summary>
        /// 根据C#类构建反射类
        /// </summary>
        /// <param name="classType">C#类型</param>
        public ReflectionClassAsContainer(Type classType)
        {
            this._type = classType;
            this._obj = null;
        }

        /// <summary>
        /// 根据C#实例构建反射类
        /// </summary>
        /// <param name="classType">C#类型</param>
        public ReflectionClassAsContainer(object obj)
        {
            this._type = obj.GetType();
            this._obj = obj;
        }

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="value"></param>
        public void Set(object key, object value)
        {
            if (key is string == false)
            {
                throw new Exception("Reflection Class key mast string:" + key.ToString());
            }

            ReflectionUtil.SetValue(_type, key.ToString(), value, _obj);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            if (key is string == false)
            {
                throw new Exception("Reflection Class key mast string:" + key.ToString());
            }
            string paramName = key as string;

            //方法
            if (ReflectionUtil.IsMethod(_type, paramName))
            {
                if (_obj != null)
                {
                    return new ReflectionMethodAsBlock(_obj, paramName);
                }
                else
                {
                    return new ReflectionMethodAsBlock(_type, paramName);
                }
            }

            //属性
            var v = ReflectionUtil.GetValue(_type, paramName, _obj);
            v = ReflectionUtil.PackageScriptType(v);//要包装一下再返回给脚本处理
            return v;
        }

    }
}
