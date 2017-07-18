using System;
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 代码块定义类
    /// </summary>
    class Block : BaseBlock, IContainer
    {
        public string name;
        /// <summary>
        /// 代码所在地址
        /// </summary>
        public int address;

        /// <summary>
        /// 父代码块
        /// </summary>
        public Block parent = null;

        /// <summary>
        /// 内部作用域存储空间
        /// </summary>
        private MemorySpace localSpace = new MemorySpace("block");

        public Block()
        {

        }

        public Block(int address)
        {
            this.address = address;
        }

        /// <summary>
        /// 遍历获取字段
        /// </summary>
        /// <returns></returns>
        public object DeepGet(string id)
        {
            object v = localSpace.Get(id);
            if (v != null)
                return v;
            if (parent != null)
            {
                return parent.DeepGet(id);
            }
            return null;
        }

        /// <summary>
        /// 设置局部字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(object key, object value)
        {
            localSpace.Set((string)key, value);
        }

        /// <summary>
        /// 获取局部字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            return localSpace.Get((string)key);
        }

        /// <summary>
        /// 清空空间
        /// </summary>
        public void Clear()
        {
            localSpace.Clear();
        }
    }
}
