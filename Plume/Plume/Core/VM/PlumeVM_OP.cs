using System;

namespace Plume.Core
{
    /// <summary>
    /// 虚拟机类 - 操作运算
    /// </summary>
    public partial class PlumeVM
    {
        /// <summary>
        /// 条件与运行
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <returns></returns>
        float _And(object objA, object objB)
        {
            if (objA is float && objB is float)
            {
                if ((float)objA == 0 || (float)objB == 0)
                {
                    return 0f;
                }
                else
                {
                    return 1f;
                }
            }
            else
            {
                throw new Exception(string.Format("{0} and {1} type error:",objA,objB));
            }
        }

        /// <summary>
        /// 条件或运行
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <returns></returns>
        float _Or(object objA, object objB)
        {
            if (objA is float && objB is float)
            {
                if ((float)objA == 0 && (float)objB == 0)
                {
                    return 0f;
                }
                else
                {
                    return 1f;
                }
            }
            else
            {
                throw new Exception(string.Format("{0} or {1} type error:", objA, objB));
            }
        }

        /// <summary>
        /// 判断运行
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        float _Check(object objA, object objB, VMCodeType type)
        {
            #region 数值
            if (objA is float && objB is float)
            {
                switch (type)
                {
                    case VMCodeType.Check_Equals:
                        if ((float)objA == (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_NotEquals:
                        if ((float)objA != (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_GreaterThan:
                        if ((float)objA > (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_GreaterThanOrEquals:
                        if ((float)objA >= (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_LessThan:
                        if ((float)objA < (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_LessThanOrEquals:
                        if ((float)objA <= (float)objB)
                        {
                            return 1f;
                        }
                        break;
                    default:
                        throw new Exception("check number error:" + type.ToString());
                }
                return 0f;
            }
            #endregion

            #region 字符串
            if (objA is string && objB is string)
            {
                switch (type)
                {
                    case VMCodeType.Check_Equals:
                        if ((string)objA == (string)objB)
                        {
                            return 1f;
                        }
                        break;
                    case VMCodeType.Check_NotEquals:
                        if ((string)objA != (string)objB)
                        {
                            return 1f;
                        }
                        break;
                    default:
                        throw new Exception("check string error:" + type.ToString());
                }
                return 0f;
            }
            #endregion

            #region 引用类型处理
            switch (type)
            {
                case VMCodeType.Check_Equals:
                    if (objA == objB)
                    {
                        return 1f;
                    }
                    break;
                case VMCodeType.Check_NotEquals:
                    if (objA != objB)
                    {
                        return 1f;
                    }
                    break;
                default:
                    throw new Exception("check object error:" + type.ToString());
            }
            return 0f;
            #endregion
        }


        /// <summary>
        /// 四则运算
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object _OP(object objA, object objB, VMCodeType type)
        {
            #region  数值:加减乘除
            if (objA is float && objB is float)
            {
                float x = (float)objA;
                float y = (float)objB;
                switch (type)
                {
                    case VMCodeType.Plus:
                        return x + y;
                    case VMCodeType.Minus:
                        return x - y;
                    case VMCodeType.Multiply:
                        return x * y;
                    case VMCodeType.Divide:
                        return x / y;
                    default:
                        throw new Exception("number only do op + - * /:" + type);
                }
            }
            #endregion

            #region 字符串：只能加法,只要其中一个是字符串就可以相加
            if (objA is string || objB is string)
            {
                switch (type)
                {
                    case VMCodeType.Plus:
                        return objA.ToString() + objB.ToString();
                    default:
                        throw new Exception("string only do op +:" + type);
                }
            }
            #endregion

            #region 表:加减表 或 乘除数值
            if (objA is Table && objB is Table)
            {
                Table x = (Table)objA;
                Table y = (Table)objB;
                switch (type)
                {
                    case VMCodeType.Plus:
                        return x + y;
                    case VMCodeType.Minus:
                        return x - y;
                    default:
                        throw new Exception("table only do op + -:" + type);
                }
            }
            if (objA is Table && objB is float)
            {
                Table x = (Table)objA;
                float y = (float)objB;
                switch (type)
                {
                    case VMCodeType.Multiply:
                        return x * y;
                    case VMCodeType.Divide:
                        return x / y;
                    default:
                        throw new Exception("table and number only do op * /:" + type);
                }
            }
            if (objA is float && objB is Table)
            {
                float x = (float)objA;
                Table y = (Table)objB;
                switch (type)
                {
                    case VMCodeType.Multiply:
                        return x * y;
                    case VMCodeType.Divide:
                        return x / y;
                    default:
                        throw new Exception("table and number only do op * /:" + type);
                }
            }
            #endregion

            throw new Exception("not have this op:" + objA + "," + objB);
        }

    }
}
