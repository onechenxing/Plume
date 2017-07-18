using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 反射C#辅助类
    /// </summary>
    class ReflectionUtil
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">参数名</param>
        /// <param name="obj">实例对象(如果是取类的静态元素不用传)</param>
        /// <returns></returns>
        public static object GetValue(Type type, string name, object obj = null)
        {
            FieldInfo fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }
            PropertyInfo propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }
            throw new Exception(string.Format("Reflection Class GetValue Error:{0}.{1}", type.Name, name));
        }


        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">参数名</param>
        /// <param name="value">设置的值</param>
        /// <param name="obj">实例对象(如果是取类的静态元素不用传)</param>
        public static void SetValue(Type type, string name, object value, object obj = null)
        {
            FieldInfo fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, value);
                return;
            }
            PropertyInfo propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, value, null);
                return;
            }
            throw new Exception(string.Format("Reflection Class SetValue Error:{0}.{1}", type.Name, name));
        }

        /// <summary>
        /// 判断是否是方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">方法名</param>
        /// <returns></returns>
        public static bool IsMethod(Type type, string name)
        {
            MethodInfo methodInfo = type.GetMethod(name);
            if (methodInfo == null)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">方法名</param>
        /// <param name="valueList">参数值列表</param>
        /// <param name="obj">实例对象(如果是取类的静态元素不用传)</param>
        public static object CallMethod(Type type, string name, List<object> valueList, object obj = null)
        {
            MethodInfo methodInfo = type.GetMethod(name);
            if (methodInfo == null)
            {
                throw new Exception(string.Format("Reflection Class CallMethod Error:{0}.{1}", type.Name, name));
            }
            ParameterInfo[] paramList = methodInfo.GetParameters();
            for (int i = 0; i < paramList.Length; i++)
            {
                ParameterInfo paramInfo = paramList[i];
                //参数类型转换成方法真正需要的类型
                if (valueList[i] != null && paramInfo.ParameterType != valueList[i].GetType())
                {
                    //包装类型解包
                    if (valueList[i] is ReflectionClassAsContainer)
                    {
                        var rc = valueList[i] as ReflectionClassAsContainer;
                        if (paramInfo.ParameterType == rc.type)
                        {
                            valueList[i] = rc.obj;
                        }
                        else
                        {
                            throw new Exception(string.Format("Reflection Class Param Error:{0}(ReflectionClass.type not match)", paramInfo.Name));
                        }
                    }
                    else//基本类型转换
                    {
                        valueList[i] = ConvertSimpleType(valueList[i], paramInfo.ParameterType);
                    }
                }
            }
            return methodInfo.Invoke(obj, valueList.ToArray());
        }


        /// <summary>
        /// 绑定方法参数列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">方法名</param>
        /// <param name="paramNameList">参数名列表</param>
        /// <param name="paramDefaultValueList">默认值列表</param>
        /// <param name="obj">实例对象(如果是取类的静态元素不用传)</param>
        public static void MethodParameters(Type type, string name, List<string> paramNameList, List<object> paramDefaultValueList, object obj = null)
        {
            MethodInfo methodInfo = type.GetMethod(name);
            if (methodInfo == null)
            {
                throw new Exception(string.Format("Reflection Class MethodParameters Error:{0}.{1}", type.Name, name));
            }
            ParameterInfo[] paramList = methodInfo.GetParameters();
            for (int i = 0; i < paramList.Length; i++)
            {
                ParameterInfo paramInfo = paramList[i];
                paramNameList.Add(paramInfo.Name);
                paramDefaultValueList.Add(paramInfo.DefaultValue);
            }
        }

        /// <summary>
        /// 包装对象成脚本语言可以识别的类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object PackageScriptType(object value)
        {
            //支持的类型
            if (VMUtil.IsSupportType(value))
            {
                return value;
            }
            //可以转换成支持的float类型
            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertTo(typeof(float)) || value is bool)//bool也要转换成0,1
            {
                return Convert.ToSingle(value);
            }
            //其他类型 包装成实例对象
            return new ReflectionClassAsContainer(value);
        }

        /// <summary>
        /// C#内置数据类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;
            if ((value == null) || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.GetType() + "==>" + destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.GetType() + "==>" + destinationType, e);
            }
            return returnValue;
        }

    }
}
