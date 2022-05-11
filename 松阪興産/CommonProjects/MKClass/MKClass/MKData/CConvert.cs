using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MKClass.MKControl;

namespace MKClass.MKData
{
    #region データ変換・補完クラス
    /// <summary>
    /// データ変換・補完クラス
    /// </summary>
    public class CConvert : CData
    {
        #region 型変換
        /// <summary>
        /// char型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>char型の文字列</returns>
        public static char ToChar(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(char);
            }

            try
            {
                return Convert.ToChar(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// char型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>char型の文字列</returns>
        public static char ToCharDef(object value, char defaultValue = default(char))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            char result = default(char);
            try
            {
                result = Convert.ToChar(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// sbyte型 (8ビット符号付き整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>sbyte型 (8ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static sbyte ToSByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(sbyte);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToSByte(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// sbyte型 (8ビット符号付き整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>sbyte型 (8ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static sbyte ToSByteDef(object value, sbyte defaultValue = default(sbyte))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            sbyte result = default(sbyte);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToSByte(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// byte型 (8ビット符号なし整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>byte型 (8ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static byte ToByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(byte);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToByte(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// byte型 (8ビット符号なし整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>byte型 (8ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static byte ToByteDef(object value, byte defaultValue = default(byte))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            byte result = default(byte);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToByte(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// int型 (16ビット符号付き整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>int型 (16ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static short ToInt16(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(short);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToInt16(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// int型 (16ビット符号付き整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>int型 (16ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static short ToInt16Def(object value, short defaultValue = default(short))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            short result = default(short);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToInt16(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// uint型 (16ビット符号なし数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>uint型 (16ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static ushort ToUInt16(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(ushort);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToUInt16(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// uint型 (16ビット符号なし整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>uint型 (16ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static ushort ToUInt16Def(object value, ushort defaultValue = default(ushort))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            ushort result = default(ushort);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToUInt16(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// int型 (32ビット符号付き整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>int型 (32ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static int ToInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(int);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToInt32(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// int型 (32ビット符号付き整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>int型 (32ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static int ToIntDef(object value, int defaultValue = default(int))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            int result = default(int);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToInt32(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// uint型 (32ビット符号なし整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>uint型 (32ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static uint ToUInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(uint);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToUInt32(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// uint型 (32ビット符号なし整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>uint型 (32ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static uint ToUIntDef(object value, uint defaultValue = default(uint))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            uint result = default(uint);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToUInt32(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// int型 (64ビット符号付き整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>int型 (64ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static long ToInt64(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(long);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToInt64(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// int型 (64ビット符号付き整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>int型 (64ビット符号付き整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static long ToInt64Def(object value, long defaultValue = default(long))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            long result = default(long);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToInt64(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// uint型 (64ビット符号なし整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>uint型 (64ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static ulong ToUInt64(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(ulong);
            }

            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                return Convert.ToUInt64(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// uint型 (64ビット符号なし整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>uint型 (64ビット符号なし整数) の値</returns>
        /// <remarks>
        /// 小数点以下は切捨てします。
        /// </remarks>
        public static ulong ToUInt64Def(object value, ulong defaultValue = default(ulong))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            ulong result = default(ulong);
            try
            {
                if (CValidate.IsToDecimal(value))
                {
                    value = Math.Floor(ToDecimal(value));
                }

                result = Convert.ToUInt64(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// decimal型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>decimal型の値</returns>
        public static decimal ToDecimal(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(decimal);
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// decimal型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>decimal型の値</returns>
        public static decimal ToDecimalDef(object value, decimal defaultValue = default(decimal))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            decimal result = default(decimal);
            try
            {
                result = Convert.ToDecimal(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// double型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>double型の値</returns>
        public static double ToDouble(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(double);
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// double型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>double型の値</returns>
        public static double ToDoubleDef(object value, double defaultValue = default(double))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            double result = default(double);
            try
            {
                result = Convert.ToDouble(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// float型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>float型の値</returns>
        public static float ToFloat(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(float);
            }

            try
            {
                return float.Parse(value.ToString());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// float型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>float型の値</returns>
        public static float ToFloatDef(object value, float defaultValue = default(float))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            float result = default(float);
            try
            {
                result = float.Parse(value.ToString());
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// bool型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>bool型の値</returns>
        public static bool ToBoolean(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            try
            {
                Type t = value.GetType();
                if (t.Equals(typeof(char)))
                {
                    return Convert.ToBoolean(ToCharDef(value));
                }
                else if (t.Equals(typeof(byte)))
                {
                    return Convert.ToBoolean(ToByteDef(value));
                }
                else if (t.Equals(typeof(int)))
                {
                    return Convert.ToBoolean(ToIntDef(value));
                }
                else if (t.Equals(typeof(decimal)))
                {
                    return Convert.ToBoolean(ToDecimalDef(value));
                }
                else if (t.Equals(typeof(double)))
                {
                    return Convert.ToBoolean(ToDoubleDef(value));
                }
                else if (t.Equals(typeof(float)))
                {
                    return Convert.ToBoolean(ToFloatDef(value));
                }
                else
                {
                    if (CValidate.IsToDecimal(value))
                    {
                        return Convert.ToBoolean(ToDecimalDef(value));
                    }
                }

                return Convert.ToBoolean(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DateTime型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>DateTime型の値</returns>
        public static DateTime ToDateTime(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(DateTime);
            }

            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DateTime型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>DateTime型の値</returns>
        public static DateTime ToDateTimeDef(object value, DateTime defaultValue = default(DateTime))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            DateTime result = default(DateTime);
            try
            {
                result = Convert.ToDateTime(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// 指定した文字列形式の日付と時刻をDateTime型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="format">文字列形式の書式</param>
        /// <returns>DateTime型の値</returns>
        /// 文字列形式の書式は、指定した書式と完全に一致する必要があります。
        /// </remarks>
        public static DateTime ToDateTimeParseExact(object value, string format)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(DateTime);
            }

            try
            {
                return DateTime.ParseExact(value.ToString(), format, null);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した文字列形式の日付と時刻をDateTime型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="format">文字列形式の書式</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>DateTime型の値</returns>
        /// 文字列形式の書式は、指定した書式と完全に一致する必要があります。
        /// </remarks>
        public static DateTime ToDateTimeParseExactDef(object value, string format, DateTime defaultValue = default(DateTime))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(DateTime);
            }

            DateTime result = default(DateTime);
            try
            {
                result = DateTime.ParseExact(value.ToString(), format, null);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }
        #endregion

        #region フォーマット変換
        /// <summary>
        /// 全角カタカナに変換します。
        /// </summary>
        /// <param name="value">変換する文字</param>
        /// <returns>全角カタカナ</returns>
        /// <remarks>
        /// 変換できない場合、null (\0) を返します。
        /// </remarks>
        public static char ConvertToKatakana(char value)
        {
            return (!CValidate.IsIncludingKanji(value.ToString()) ?
                                ToCharDef(Strings.StrConv(value.ToString(), VbStrConv.Katakana | VbStrConv.Wide, 0x411)) :
                                '\0');
        }

        /// <summary>
        /// 全角カタカナに変換します。
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>全角カタカナ</returns>
        /// <remarks>
        /// 変換できない場合、nullを返します。
        /// </remarks>
        public static string ConvertToKatakana(string value)
        {
            return (!CValidate.IsIncludingKanji(value) ?
                                Strings.StrConv(value, VbStrConv.Katakana | VbStrConv.Wide, 0x411) :
                                null);
        }

        /// <summary>
        /// 半角カタカナに変換します。
        /// </summary>
        /// <param name="value">変換する文字</param>
        /// <returns>半角カタカナ</returns>
        /// <remarks>
        /// 変換できない場合、null (\0) を返します。
        /// 2文字になるケース (濁点、半濁点など) では、例外エラーになるので注意してください。
        /// </remarks>
        public static char ConvertToKatakanaHalf(char value)
        {
            return (!CValidate.IsIncludingKanji(value.ToString()) ?
                                ToCharDef(Strings.StrConv(value.ToString(), VbStrConv.Katakana | VbStrConv.Narrow, 0x411)) :
                                '\0');
        }

        /// <summary>
        /// 半角カタカナに変換します。
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>半角カタカナ</returns>
        /// <remarks>
        /// 変換できない場合、nullを返します。
        /// </remarks>
        public static string ConvertToKatakanaHalf(string value)
        {
            return (!CValidate.IsIncludingKanji(value) ?
                                Strings.StrConv(value, VbStrConv.Katakana | VbStrConv.Narrow, 0x411) :
                                null);
        }

        /// <summary>
        /// DataGridViewをDataTableに変換します。 (Deprecated)
        /// </summary>
        /// <param name="dgv">変換元のDataGridView</param>
        /// <param name="includeHiddenRow">非表示行を含めるかどうか</param>
        /// <returns>変換後のDataTable</returns>
        public static DataTable ConvertToDataTable(DataGridView dgv, bool includeHiddenRow = true)
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                if (dc.ValueType != null)
                {
                    dt.Columns.Add(dc.Name, dc.ValueType);
                }
                else
                {
                    dt.Columns.Add(dc.Name);
                }
            }

            foreach (DataGridViewRow dr in dgv.Rows)
            {
                if (!includeHiddenRow)
                {
                    if (!dr.Visible)
                    {
                        continue;
                    }
                }

                List<object> array = new List<object>();
                foreach (DataGridViewCell cell in dr.Cells)
                {
                    array.Add(cell.Value);
                }

                dt.Rows.Add(array.ToArray());
            }

            return dt;
        }
        #endregion

        #region 補完機能
        /// <summary>
        /// 文字列をシングルクォートで囲みます。
        /// </summary>
        /// <param name="value">補完する文字列</param>
        /// <returns>補完後の文字列</returns>
        public static string EncloseSingleQuotes(string value)
        {
            if (value.IndexOf("'") > -1)
            {
                // 文字列中のシングルクォートをエスケープする
                value = value.Replace("'", "''");
            }

            value = "'" + value + "'";

            return value;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲みます。
        /// </summary>
        /// <param name="value">補完する文字列</param>
        /// <returns>補完後の文字列</returns>
        public static string EncloseDoubleQuotes(string value)
        {
            if (value.IndexOf('"') > -1)
            {
                // 文字列中のダブルクォートをエスケープする
                value = value.Replace("\"", "\"\"");
            }

            value = "\"" + value + "\"";

            return value;
        }

        /// <summary>
        /// xxx-xxxx形式の郵便番号に補完します。
        /// </summary>
        /// <param name="value">補完する郵便番号</param>
        /// <returns>補完後の郵便番号</returns>
        /// <remarks>
        /// 補完できない場合、そのまま返します。
        /// </remarks>
        public static string ComplementPostalCode(string value)
        {
            if (CValidate.IsPostalCodeMatch(value))
            {
                // ハイフンなし7桁
                if (Regex.IsMatch(value, @"^\d{7}$"))
                {
                    string postalCode3 = value.Substring(0, 3);
                    string postalCode4 = value.Substring(3, 4);
                    if ((ToInt(postalCode3) >= 0) && (ToInt(postalCode4) >= 0))
                    {
                        return postalCode3 + "-" + postalCode4;
                    }
                }
                // ハイフンあり7桁
                else if (Regex.IsMatch(value, @"^\d{3}[-]\d{4}$"))
                {
                    string postalCode3 = value.Substring(0, 3);
                    string postalCode4 = value.Substring(4, 4);
                    if ((ToInt(postalCode3) >= 0) && (ToInt(postalCode4) >= 0))
                    {
                        return value;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// 指定フォーマットで日付文字列に補完します。
        /// </summary>
        /// <param name="value">補完するオブジェクト</param>
        /// <param name="format">日付フォーマット</param>
        /// <returns>補完後の日付文字列</returns>
        /// <remarks>
        /// 補完できない場合、そのまま返します。
        /// </remarks>
        public static string ComplementDate(object value, DateFormatType format = DateFormatType.YYYYMMDD)
        {
            try
            {
                List<int> dates = value.ToString().Split('/').Select(r => ToInt(r)).ToList();

                int y = 0, m = 0, d = 0;
                switch (dates.Count)
                {
                    case 1:
                        switch (format)
                        {
                            case DateFormatType.YYYYMMDD:
                            case DateFormatType.YYYYMD:
                            case DateFormatType.YYMMDD:
                            case DateFormatType.YYMD:
                                y = dates[0] / 10000;
                                m = (dates[0] / 100) % 100;
                                d = dates[0] % 100;

                                break;
                            case DateFormatType.YYYYMM:
                            case DateFormatType.YYYYM:
                            case DateFormatType.YYMM:
                            case DateFormatType.YYM:
                                y = dates[0] / 100;
                                m = dates[0] % 100;

                                break;
                            case DateFormatType.MMDD:
                            case DateFormatType.MD:
                                m = dates[0] / 100;
                                d = dates[0] % 100;

                                break;
                            case DateFormatType.YYYY:
                            case DateFormatType.YY:
                                y = dates[0];

                                break;
                            case DateFormatType.MM:
                            case DateFormatType.M:
                                m = dates[0];

                                break;
                            case DateFormatType.DD:
                            case DateFormatType.D:
                                d = dates[0];

                                break;
                            default:
                                return value.ToString();
                        }

                        break;
                    case 2:
                        switch (format)
                        {
                            case DateFormatType.YYYYMMDD:
                            case DateFormatType.YYYYMD:
                            case DateFormatType.YYMMDD:
                            case DateFormatType.YYMD:
                            case DateFormatType.MMDD:
                            case DateFormatType.MD:
                                m = dates[0];
                                d = dates[1];

                                break;
                            case DateFormatType.YYYYMM:
                            case DateFormatType.YYYYM:
                            case DateFormatType.YYMM:
                            case DateFormatType.YYM:
                                y = dates[0];
                                m = dates[1];

                                break;
                            default:
                                return value.ToString();
                        }

                        break;
                    case 3:
                        switch (format)
                        {
                            case DateFormatType.YYYYMMDD:
                            case DateFormatType.YYYYMD:
                            case DateFormatType.YYMMDD:
                            case DateFormatType.YYMD:
                                y = dates[0];
                                m = dates[1];
                                d = dates[2];

                                break;
                            default:
                                return value.ToString();
                        }

                        break;
                    default:
                        return value.ToString();
                }

                if (y == 0)
                {
                    // 本年をセット
                    y = DateTime.Today.Year;
                }
                else if (y < 1000)
                {
                    y += 2000;
                }

                if (m == 0)
                {
                    // 本月をセット
                    m = DateTime.Today.Month;
                }

                if (d == 0)
                {
                    // 本日をセット
                    d = DateTime.Today.Day;
                }

                if (DateTime.DaysInMonth(y, m) <= d && d <= 31)
                {
                    // 該当年月の末日をセット
                    d = DateTime.DaysInMonth(y, m);
                }

                return new DateTime(y, m, d).ToString(format.GetStringValue());
            }
            catch
            {

            }

            return value.ToString();
        }

        /// <summary>
        /// 指定フォーマットで時間間隔文字列に補完します。
        /// </summary>
        /// <param name="value">補完するオブジェクト</param>
        /// <param name="format">時間間隔フォーマット</param>
        /// <returns>補完後の時間間隔文字列</returns>
        /// <remarks>
        /// 補完できない場合、そのまま返します。
        /// 端数が出る場合、切捨てします。
        /// </remarks>
        public static string ComplementTimeSpan(object value, TimeSpanFormatType format = TimeSpanFormatType.HMS)
        {
            try
            {
                List<int> times = value.ToString().Split(':').Select(r => ToInt(r)).ToList();

                if (1 <= times.Count && times.Count <= 3)
                {
                    TimeSpan t;
                    switch (format)
                    {
                        case TimeSpanFormatType.HM:
                            t = new TimeSpan(times[0], times.Count > 1 ? times[1] : 0, times.Count > 2 ? times[2] : 0);

                            return ((int)t.TotalHours).ToString() + ":" + t.Minutes.ToString("00");
                        case TimeSpanFormatType.H:
                            t = new TimeSpan(times[0], times.Count > 1 ? times[1] : 0, times.Count > 2 ? times[2] : 0);

                            return ((int)t.TotalHours).ToString();
                        case TimeSpanFormatType.MS:
                            t = new TimeSpan(0, times[0], times.Count > 1 ? times[1] : 0);

                            return ((int)t.TotalMinutes).ToString() + ":" + t.Seconds.ToString("00");
                        case TimeSpanFormatType.M:
                            t = new TimeSpan(0, times[0], times.Count > 1 ? times[1] : 0);

                            return ((int)t.TotalMinutes).ToString();
                        case TimeSpanFormatType.S:
                            t = new TimeSpan(0, 0, times[0]);

                            return ((int)t.TotalSeconds).ToString();
                        default:
                        case TimeSpanFormatType.HMS:
                            t = new TimeSpan(times[0], times.Count > 1 ? times[1] : 0, times.Count > 2 ? times[2] : 0);

                            return ((int)t.TotalHours).ToString() + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
                    }
                }
            }
            catch
            {

            }

            return value.ToString();
        }
        #endregion
    }
    #endregion



    #region DataTable拡張クラス (static)
    /// <summary>
    /// DataTable拡張クラス (static)
    /// </summary>
    public static class CDataTable
    {
        #region DataGridView
        /// <summary>
        /// DataGridViewをDataTableに変換します。
        /// </summary>
        /// <param name="self">変換元のDataGridView</param>
        /// <param name="includeHiddenRow">非表示行を含めるかどうか</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(this DataGridView self, bool includeHiddenRow = true)
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn dc in self.Columns)
            {
                if (dc.ValueType != null)
                {
                    dt.Columns.Add(dc.Name, dc.ValueType);
                }
                else
                {
                    dt.Columns.Add(dc.Name);
                }
            }

            foreach (DataGridViewRow dr in self.Rows)
            {
                if (!includeHiddenRow)
                {
                    if (!dr.Visible)
                    {
                        continue;
                    }
                }

                List<object> array = new List<object>();
                foreach (DataGridViewCell cell in dr.Cells)
                {
                    array.Add(cell.Value);
                }

                dt.Rows.Add(array.ToArray());
            }

            return dt;
        }
        #endregion

        #region List
        /// <summary>
        /// ListをDataTableに変換します。
        /// </summary>
        /// <typeparam name="T">リスト内の要素の型</typeparam>
        /// <param name="collection">オブジェクトのコレクション</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> collection)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable dt = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in collection)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            dt.AcceptChanges();

            return dt;
        }
        #endregion

        #region XElement
        /// <summary>
        /// XElementをDataTableに変換します。
        /// </summary>
        /// <param name="element">変換したい階層のXML要素</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(this IEnumerable<XElement> xEl)
        {
            if (xEl.IsNullOrEmpty())
            {
                return null;
            }

            try
            {
                return ToDataTable(new XElement("root", xEl));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// XElementをDataTableに変換します。
        /// </summary>
        /// <param name="xEl">変換したい階層のXML要素</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(this XElement xEl)
        {
            if (xEl.IsEmpty)
            {
                return null;
            }

            try
            {
                DataSet ds = new DataSet();

                ds.ReadXml(new StringReader(xEl.ToString()));

                return ds.Tables[0];
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
    #endregion
}
