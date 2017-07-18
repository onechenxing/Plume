using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 存储空间栈
    /// </summary>
    class MemorySpaceStack
    {
        /// <summary>
        /// 存储空间容器列表
        /// </summary>
        private List<MemorySpace> _stack = new List<MemorySpace>();

        public MemorySpaceStack()
        {

        }

        /// <summary>
        /// 获得栈顶存储空间
        /// </summary>
        /// <returns></returns>
        public MemorySpace currentSpace
        {
            get
            {
                return _stack[_stack.Count - 1];
            }
        }

        /// <summary>
        /// 获得栈低存储空间
        /// </summary>
        public MemorySpace firsetSpace
        {
            get
            {
                return _stack[0];
            }
        }

        /// <summary>
        /// 压入新的空间
        /// </summary>
        public void PushSpace(string name)
        {
            _stack.Add(new MemorySpace(name));
        }

        /// <summary>
        /// 弹出空间
        /// </summary>
        public void PopSpace()
        {
            _stack.RemoveAt(_stack.Count - 1);
        }

        /// <summary>
        /// 获取id存在的存储空间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemorySpace GetSpace(string id)
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                if (_stack[i].Has(id))
                {
                    return _stack[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否有id存在(遍历所有栈)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Has(string id)
        {
            var space = GetSpace(id);
            if (space == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得id存储的值(遍历所有栈)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(string id)
        {
            var space = GetSpace(id);
            if (space == null)
            {
                throw new Exception("变量没有定义:" + id);
            }
            return space.Get(id);
        }

        /// <summary>
        /// 更新值(遍历所有栈)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Set(string id, object value)
        {
            var space = GetSpace(id);
            if (space == null)
            {
                throw new Exception("变量没有定义:" + id);
            }
            space.Set(id, value);
        }
        
                
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var space in _stack)
            {
                sb.AppendLine(space.ToString());
            }
            return sb.ToString();
        }
    }
}
