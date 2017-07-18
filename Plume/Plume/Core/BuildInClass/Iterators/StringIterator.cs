using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 字符串迭代器
    /// </summary>
    class StringIterator : IIterator
    {
        private string _str;
        private int _index;

        public StringIterator(string str)
        {
            this._str = str;
            this._index = 0;
        }

        public bool HasNext()
        {
            if (_index >= _str.Length)
            {
                return false;
            }
            return true;
        }


        public object MoveNext()
        {
            return _str[_index++].ToString();
        }
    }
}
