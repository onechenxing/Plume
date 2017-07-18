using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 表迭代器
    /// </summary>
    class TableIterator : IIterator
    {
        private Table _table;
        private int _index;

        public TableIterator(Table table)
        {
            this._table = table;
            this._index = 0;
        }

        public bool HasNext()
        {
            if (_index >= _table.Count)
            {
                return false;
            }
            return true;
        }


        public object MoveNext()
        {
            return _table.GetByIndex(_index++);
        }
    }
}
