using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 表实例类
    /// 可模拟数组和字典
    /// </summary>
    public class Table : IContainer
    {
        /// <summary>
        /// 数组当前长度(一个元素坐标)
        /// </summary>
        private float _len = 0;

        /// <summary>
        /// 哈希表
        /// </summary>
        private Dictionary<object, object> _dic = new Dictionary<object, object>();

        /// <summary>
        /// 数组长度（获取下一个最大数字下标）
        /// </summary>
        /// <returns></returns>
        public float Length
        {
            get
            {
                return _len;
            }
        }

        /// <summary>
        /// 总元素个数
        /// </summary>
        public float Count
        {
            get
            {
                return _dic.Keys.Count;
            }
        }

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="value"></param>
        public void Set(object key, object value)
        {
            //数组下标更新处理(取其中最大值)
            if (key is float)
            {
                float fKey = (float)key;
                if (fKey >= _len)
                {
                    _len = fKey + 1;
                }
            }
            _dic[key] = value;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            if (Has(key) == false)
            {
                throw new Exception("没有这个表元素:" + key);
            }
            return _dic[key];
        }

        public bool Has(object key)
        {
            if (_dic.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据顺序index获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetByIndex(int index)
        {
            return _dic[new List<object>(_dic.Keys)[index]];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            var itr = _dic.GetEnumerator();
            itr.MoveNext();
            while (true)
            {
                var item = itr.Current;
                var obj = item.Value;
                if (item.Key is float)
                {
                    sb.Append(obj);
                }
                else
                {
                    sb.Append(string.Format("{0}={1}", item.Key, obj));
                }
                if (itr.MoveNext())
                {
                    sb.Append(",");
                }
                else
                {
                    break;
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        #region 操作符重载
        public static Table operator +(Table t1, Table t2)
        {
            Table rt = new Table();

            foreach (var key in t1._dic.Keys)
            {
                try
                {
                    rt._dic[key] = (float)(t1._dic[key]) + (float)(t2._dic[key]);
                }
                catch (Exception e)
                {
                    throw new Exception("Table + error:" + e.Message, e);
                }
            }

            return rt;
        }

        public static Table operator -(Table t1, Table t2)
        {
            Table rt = new Table();

            foreach (var key in t1._dic.Keys)
            {
                try
                {
                    rt._dic[key] = (float)(t1._dic[key]) - (float)(t2._dic[key]);
                }
                catch (Exception e)
                {
                    throw new Exception("Table - error:" + e.Message, e);
                }
            }

            return rt;
        }

        public static Table operator *(Table t1, float t2)
        {
            Table rt = new Table();

            foreach (var key in t1._dic.Keys)
            {
                try
                {
                    rt._dic[key] = (float)(t1._dic[key]) * t2;
                }
                catch (Exception e)
                {
                    throw new Exception("Table * error:" + e.Message, e);
                }
            }

            return rt;
        }

        public static Table operator *(float t1, Table t2)
        {
            return t2 * t1;
        }

        public static Table operator /(Table t1, float t2)
        {
            Table rt = new Table();

            foreach (var key in t1._dic.Keys)
            {
                try
                {
                    rt._dic[key] = (float)(t1._dic[key]) / t2;
                }
                catch (Exception e)
                {
                    throw new Exception("Table / error:" + e.Message, e);
                }
            }

            return rt;
        }

        public static Table operator /(float t1, Table t2)
        {
            return t2 / t1;
        }

        #endregion
    }
}
