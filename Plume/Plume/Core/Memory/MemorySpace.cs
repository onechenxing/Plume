using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 存储空间类
    /// </summary>
    class MemorySpace
    {
        /// <summary>
        /// 空间名
        /// </summary>
        public string name;
        /// <summary>
        /// 存储字典
        /// </summary>
        public Dictionary<string, object> members = new Dictionary<string, object>();

        public MemorySpace(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Has(string id)
        {
            return members.ContainsKey(id);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(string id)
        {
            if (members.ContainsKey(id))
            {
                return members[id];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 存储或更新值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Set(string id, object value)
        {
            members[id] = value;
        }

        /// <summary>
        /// 清空空间
        /// </summary>
        public void Clear()
        {
            members.Clear();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append("->");
            foreach (var i in members)
            {
                sb.Append(i.Key);
                sb.Append(":");
                sb.Append(i.Value);
            }
            return sb.ToString();
        }
    }
}
