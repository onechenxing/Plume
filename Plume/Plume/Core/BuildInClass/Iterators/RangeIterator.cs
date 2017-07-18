using System;
using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 范围迭代器
    /// </summary>
    class RangeIterator : IIterator
    {
        private float _now;
        private float _end;
        private float _step;

        public RangeIterator(float start,float end,float step = 1)
        {
            this._now = start;
            this._end = end;
            this._step = step;
        }

        public bool HasNext()
        {
            if (_now >= _end)
            {
                return false;
            }
            return true;
        }


        public object MoveNext()
        {
            float re = _now;
            _now += _step;
            return re;
        }
    }
}
