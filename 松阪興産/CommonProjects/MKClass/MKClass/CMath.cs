using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKClass.MKData;

namespace MKClass
{
    #region 列挙体
    /// <summary>
    /// 端数処理種別
    /// </summary>
    public enum RoundingType
    {
        /// <summary>切捨て</summary>
        Floor = 0,
        /// <summary>切上げ</summary>
        Ceiling = 1,
        /// <summary>四捨五入</summary>
        Round = 2,
        /// <summary>五捨六入</summary>
        Round56 = 3
    }
    #endregion



    /// <summary>
    /// Mathクラス (static)
    /// </summary>
    public static class CMath
    {
        /// <summary>
        /// 除算を安全に行います。
        /// </summary>
        /// <param name="a">分子</param>
        /// <param name="b">分母</param>
        /// <returns>除算結果</returns>
        public static decimal Div(object a, object b, decimal defaultValue = default(decimal))
        {
            decimal ret = defaultValue;
            try
            {
                ret = (CConvert.ToDecimal(a) / CConvert.ToDecimal(b));
            }
            catch
            {
                // DivideByZeroException
            }

            return ret;
        }

        /// <summary>
        /// 除算を安全に行います。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="a">分子</param>
        /// <param name="b">分母</param>
        /// <returns>除算結果</returns>
        /// <remarks>
        /// 整数数値型は、除算結果に小数点が含まれる場合、切捨てします。
        /// </remarks>
        public static T Div<T>(object a, object b, T defaultValue = default(T))
        {
            T ret = defaultValue;
            try
            {
                Type type = typeof(T);

                decimal d = CConvert.ToDecimal(a) / CConvert.ToDecimal(b);
                if (type == typeof(byte))
                {
                    ret = (T)(object)CConvert.ToByte(d);
                }
                else if (type == typeof(sbyte))
                {
                    ret = (T)(object)CConvert.ToSByte(d);
                }
                else if (type == typeof(int))
                {
                    ret = (T)(object)CConvert.ToInt(d);
                }
                else if (type == typeof(uint))
                {
                    ret = (T)(object)CConvert.ToUInt(d);
                }
                else if (type == typeof(short))
                {
                    ret = (T)(object)CConvert.ToInt16(d);
                }
                else if (type == typeof(ushort))
                {
                    ret = (T)(object)CConvert.ToUInt16(d);
                }
                else if (type == typeof(long))
                {
                    ret = (T)(object)CConvert.ToInt64(d);
                }
                else if (type == typeof(ulong))
                {
                    ret = (T)(object)CConvert.ToUInt64(d);
                }
                else if (type == typeof(float))
                {
                    ret = (T)(object)CConvert.ToFloat(d);
                }
                else if (type == typeof(double))
                {
                    ret = (T)(object)CConvert.ToDouble(d);
                }
                else if (type == typeof(decimal))
                {
                    ret = (T)(object)CConvert.ToDecimal(d);
                }
                else if (type == typeof(string))
                {
                    ret = (T)(object)d.ToString();
                }
                else
                {
                    ret = (T)(object)d;
                }
            }
            catch
            {
                // DivideByZeroException
            }

            return ret;
        }

        /// <summary>
        /// 剰余算を安全に行います。
        /// </summary>
        /// <param name="a">分子</param>
        /// <param name="b">分母</param>
        /// <returns>剰余算結果</returns>
        public static decimal DivRem(object a, object b, decimal defaultValue = default(decimal))
        {
            decimal ret = defaultValue;
            try
            {
                ret = (CConvert.ToDecimal(a) % CConvert.ToDecimal(b));
            }
            catch
            {
                // DivideByZeroException
            }

            return ret;
        }

        /// <summary>
        /// 剰余算を安全に行います。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="a">分子</param>
        /// <param name="b">分母</param>
        /// <returns>剰余算結果</returns>
        public static T DivRem<T>(object a, object b, T defaultValue = default(T))
        {
            T ret = defaultValue;
            try
            {
                Type type = typeof(T);

                decimal d = CConvert.ToDecimal(a) % CConvert.ToDecimal(b);
                if (type == typeof(byte))
                {
                    ret = (T)(object)CConvert.ToByte(d);
                }
                else if (type == typeof(sbyte))
                {
                    ret = (T)(object)CConvert.ToSByte(d);
                }
                else if (type == typeof(int))
                {
                    ret = (T)(object)CConvert.ToInt(d);
                }
                else if (type == typeof(uint))
                {
                    ret = (T)(object)CConvert.ToUInt(d);
                }
                else if (type == typeof(short))
                {
                    ret = (T)(object)CConvert.ToInt16(d);
                }
                else if (type == typeof(ushort))
                {
                    ret = (T)(object)CConvert.ToUInt16(d);
                }
                else if (type == typeof(long))
                {
                    ret = (T)(object)CConvert.ToInt64(d);
                }
                else if (type == typeof(ulong))
                {
                    ret = (T)(object)CConvert.ToUInt64(d);
                }
                else if (type == typeof(float))
                {
                    ret = (T)(object)CConvert.ToFloat(d);
                }
                else if (type == typeof(double))
                {
                    ret = (T)(object)CConvert.ToDouble(d);
                }
                else if (type == typeof(decimal))
                {
                    ret = (T)(object)CConvert.ToDecimal(d);
                }
                else if (type == typeof(string))
                {
                    ret = (T)(object)d.ToString();
                }
                else
                {
                    ret = (T)(object)d;
                }
            }
            catch
            {
                // DivideByZeroException
            }

            return ret;
        }

        /// <summary>
        /// 指定の数値を指定した値で累乗した値を返します。
        /// </summary>
        /// <param name="x">累乗対象の浮動小数点数</param>
        /// <param name="y">累乗を指定する浮動小数点数</param>
        /// <returns>累乗計算した結果</returns>
        public static decimal Pow(decimal x, decimal y)
        {
            if (x == 0.00m)
            {
                return 0.00m;
            }

            decimal ret = 1.00m;
            try
            {
                if (y < 0)
                {
                    for (int i = 0; i > y; i--)
                    {
                        ret /= x;
                    }
                }
                else
                {
                    for (int i = 0; i < y; i++)
                    {
                        ret *= x;
                    }
                }

                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 対象の10進数の丸め処理を行います。
        /// </summary>
        /// <param name="roundingType">端数処理種別</param>
        /// <param name="value">丸め対象の10進数</param>
        /// <param name="decimals">戻り値の小数部の桁数</param>
        /// <returns>丸め処理した10進数</returns>
        public static decimal Rounding(RoundingType roundingType, object value, int decimals = 0)
        {
            if (value == null)
            {
                return 0.00m;
            }

            decimal ret = 0.00m;
            try
            {
                decimal d = CConvert.ToDecimal(value);
                decimal abs = Math.Abs(d);
                decimal pow = Pow(10, decimals);

                switch (roundingType)
                {
                    case RoundingType.Floor:
                        ret = Math.Floor(abs * pow);

                        break;
                    case RoundingType.Ceiling:
                        ret = Math.Ceiling(abs * pow);

                        break;
                    case RoundingType.Round:
                        ret = Math.Round(abs * pow, MidpointRounding.AwayFromZero);

                        break;
                    case RoundingType.Round56:
                        ret = Math.Round((abs * pow) - 0.10m, MidpointRounding.AwayFromZero);

                        break;
                    default:
                        return d;
                }

                ret = (ret / pow) * Math.Sign(d);

                return ret;
            }
            catch
            {
                throw;
            }
        }
    }
}
