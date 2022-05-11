using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MKClass.MKData
{
    /// <summary>
    /// データ検証クラス
    /// </summary>
    public class CValidate : CData
    {
        #region 型変換チェック
        /// <summary>
        /// char型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        public static bool IsToChar(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            char result = default(char);
            try
            {
                result = Convert.ToChar(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// sbyte型 (8ビット符号付き整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToSByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            sbyte result = default(sbyte);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToSByte(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// byte型 (8ビット符号なし整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            byte result = default(byte);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToByte(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// int型 (16ビット符号付き整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToInt16(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            short result = default(short);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToInt16(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// uint型 (16ビット符号なし整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToUInt16(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            ushort result = default(ushort);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToUInt16(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// int型 (32ビット符号付き整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            int result = default(int);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToInt32(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// uint型 (32ビット符号なし整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToUInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            uint result = default(uint);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToUInt32(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// int型 (64ビット符号付き整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToInt64(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            long result = default(long);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToInt64(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// uint型 (64ビット符号なし整数) に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 小数点以下は切捨てして、判断します。
        /// </remarks>
        public static bool IsToUInt64(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            ulong result = default(ulong);
            try
            {
                if (IsToDecimal(value))
                {
                    value = Math.Floor(CConvert.ToDecimal(value));
                }

                result = Convert.ToUInt64(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// decimal型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        public static bool IsToDecimal(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            decimal result = default(decimal);
            try
            {
                result = Convert.ToDecimal(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// double型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        public static bool IsToDouble(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            double result = default(double);
            try
            {
                result = Convert.ToDouble(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// float型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        public static bool IsToFloat(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            float result = default(float);
            try
            {
                result = float.Parse(value.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// DateTime型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        public static bool IsToDateTime(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            DateTime result = default(DateTime);
            try
            {
                result = Convert.ToDateTime(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定した文字列形式の日付と時刻をDateTime型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <param name="format">文字列形式の日付と時刻</param>
        /// <returns>
        /// true : 変換できる
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 文字列形式の書式は、指定した書式と完全に一致する必要があります。
        /// </remarks>
        public static bool IsToDateTimeParseExact(object value, string format)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            DateTime result = default(DateTime);
            try
            {
                result = DateTime.ParseExact(value.ToString(), format, null);
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 入力文字バリデーションチェック
        /// <summary>
        /// 漢字を含んでいるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <returns>
        /// true : 漢字を含む
        /// false: それ以外
        /// </returns>
        public static bool IsIncludingKanji(string value)
        {
            if (Regex.IsMatch(value,
                              @"[\p{IsCJKUnifiedIdeographs}" +
                              @"\p{IsCJKCompatibilityIdeographs}" +
                              @"\p{IsCJKUnifiedIdeographsExtensionA}]|" +
                              @"[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF]")
                )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 全角カナに相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 全角カナ
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 漢字を含むかどうかで判断します。
        /// </remarks>
        public static bool IsKatakanaMatch(char value)
        {
            return !IsIncludingKanji(value.ToString());
        }

        /// <summary>
        /// 全角カナかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <returns>
        /// true : 全角カナ
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 漢字を含むかどうかで判断します。
        /// </remarks>
        public static bool IsKatakanaMatch(string value)
        {
            return !IsIncludingKanji(value);
        }

        /// <summary>
        /// 半角カナに相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 半角カナ
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 漢字を含むかどうかで判断します。
        /// </remarks>
        public static bool IsKatakanaHalfMatch(char value)
        {
            return !IsIncludingKanji(value.ToString());
        }

        /// <summary>
        /// 半角カナかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <returns>
        /// true : 半角カナ
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 漢字を含むかどうかで判断します。
        /// </remarks>
        public static bool IsKatakanaHalfMatch(string value)
        {
            return !IsIncludingKanji(value);
        }

        /// <summary>
        /// 整数に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 整数
        /// false: それ以外
        /// </returns>
        public static bool IsIntMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\-\b]");
        }

        /// <summary>
        /// 整数かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 整数
        /// false: それ以外
        /// </returns>
        public static bool IsIntMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^([\-]?[1-9][0-9]*|0)$" : @"^([\-]?|[\-]?[1-9][0-9]*|0)?$"));
        }

        /// <summary>
        /// 符号なし整数に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 符号なし整数
        /// false: それ以外
        /// </returns>
        public static bool IsUIntMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\b]");
        }

        /// <summary>
        /// 符号なし整数かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 符号なし整数
        /// false: それ以外
        /// </returns>
        public static bool IsUIntMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^([1-9][0-9]*|0)$" : @"^([1-9][0-9]*|0)?$"));
        }

        /// <summary>
        /// 浮動小数点数値に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 浮動小数点数値
        /// false: それ以外
        /// </returns>
        public static bool IsDecimalMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\-\.\b]");
        }

        /// <summary>
        /// 浮動小数点数値かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 浮動小数点数値
        /// false: それ以外
        /// </returns>
        public static bool IsDecimalMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[\-]?(([1-9][0-9]*)+(\.[0-9]*)?|0(\.[0-9]*)?)$" : @"^[\-]?(([1-9][0-9]*)+(\.[0-9]*)?|0(\.[0-9]*)?)?$"));
        }

        /// <summary>
        /// 符号なし浮動小数点数値に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 符号なし浮動小数点数値
        /// false: それ以外
        /// </returns>
        public static bool IsUDecimalMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\.\b]");
        }

        /// <summary>
        /// 符号なし浮動小数点数値かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 符号なし浮動小数点数値
        /// false: それ以外
        /// </returns>
        public static bool IsUDecimalMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^(([1-9][0-9]*)+(\.[0-9]*)?|0(\.[0-9]*)?)$" : @"^(([1-9][0-9]*)+(\.[0-9]*)?|0(\.[0-9]*)?)?$"));
        }

        /// <summary>
        /// コード体系 (英数字) に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : コード体系 (英数字) の文字
        /// false: それ以外
        /// </returns>
        public static bool IsCodeMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9a-zA-Z\b]");
        }

        /// <summary>
        /// コード体系 (英数字) かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : コード体系 (英数字) の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// フォーマットが定められていないので、相応しい文字列かどうかをチェックします。
        /// </remarks>
        public static bool IsCodeMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[0-9a-zA-Z]*$" : @"^[0-9a-zA-Z]*$"));
        }

        /// <summary>
        /// コード体系 (一部記号含む) に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : コード体系 (一部記号含む) の文字
        /// false: それ以外
        /// </returns>
        public static bool IsCodeSymbolMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9a-zA-Z\-_\.\b]");
        }

        /// <summary>
        /// コード体系 (一部記号含む) かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : コード体系 (一部記号含む) の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// フォーマットが定められていないので、相応しい文字列かどうかをチェックします。
        /// </remarks>
        public static bool IsCodeSymbolMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[0-9a-zA-Z\-_\.]*$" : @"^[0-9a-zA-Z\-_\.]*$"));
        }

        /// <summary>
        /// コード体系 (数字のみ) に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : コード体系 (数字のみ) の文字
        /// false: それ以外
        /// </returns>
        public static bool IsCodeNumOnlyMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\b]");
        }

        /// <summary>
        /// コード体系 (数字のみ) かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : コード体系 (数字のみ) の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// フォーマットが定められていないので、相応しい文字列かどうかをチェックします。
        /// </remarks>
        public static bool IsCodeNumOnlyMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[0-9]*$" : @"^[0-9]*$"));
        }

        /// <summary>
        /// コード体系 (英字のみ) に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : コード体系 (英字のみ) の文字
        /// false: それ以外
        /// </returns>
        public static bool IsCodeAlphaOnlyMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[a-zA-Z\b]");
        }

        /// <summary>
        /// コード体系 (英字のみ) かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : コード体系 (英字のみ) の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// フォーマットが定められていないので、相応しい文字列かどうかをチェックします。
        /// </remarks>
        public static bool IsCodeAlphaOnlyMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[a-zA-Z]*$" : @"^[a-zA-Z]*$"));
        }

        /// <summary>
        /// コード体系 (英数字・記号 (半角スペース含む) ・半角カナ) に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : コード体系 (英数字・記号 (半角スペース含む) ・半角カナ) の文字
        /// false: それ以外
        /// </returns>
        public static bool IsCodeKatakanaHalf(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9a-zA-Zｦ-ﾟ!-~\s\b]");
        }

        /// <summary>
        /// コード体系 (英数字・記号 (半角スペース含む) ・半角カナ) かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : コード体系 (英数字・記号 (半角スペース含む) ・半角カナ) の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// フォーマットが定められていないので、相応しい文字列かどうかをチェックします。
        /// </remarks>
        public static bool IsCodeKatakanaHalf(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^[0-9a-zA-Zｦ-ﾟ!-~\s]*$" : @"^[0-9a-zA-Zｦ-ﾟ!-~\s]*$"));
        }

        /// <summary>
        /// 日付に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <param name="format">日付フォーマット</param>
        /// <returns>
        /// true : 日付の文字
        /// false: それ以外
        /// </returns>
        public static bool IsDateMatch(char value, DateFormatType format = DateFormatType.YYYYMMDD)
        {
            switch (format)
            {
                case DateFormatType.YYYY:
                case DateFormatType.YY:
                case DateFormatType.MM:
                case DateFormatType.M:
                case DateFormatType.DD:
                case DateFormatType.D:
                    return Regex.IsMatch(value.ToString(), @"[0-9\b]");
                default:
                    return Regex.IsMatch(value.ToString(), @"[0-9\/\b]");
            }
        }

        /// <summary>
        /// 日付かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="format">日付フォーマット</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 日付の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 厳格にチェックする場合は、DateTime型に変換できるかを検証します。
        /// </remarks>
        public static bool IsDateMatch(string value, DateFormatType format = DateFormatType.YYYYMMDD, bool strict = true)
        {
            if (strict)
            {
                return IsToDateTimeParseExact(value, format.GetStringValue());
            }

            switch (format)
            {
                case DateFormatType.YYYYMM:
                case DateFormatType.YYYYM:
                case DateFormatType.YYMM:
                case DateFormatType.YYM:
                case DateFormatType.MMDD:
                case DateFormatType.MD:
                    return Regex.IsMatch(value, @"^(([0-9]+)?([^\/]+\/|\/[0-9]+)?)?$");
                case DateFormatType.YYYY:
                case DateFormatType.YY:
                case DateFormatType.MM:
                case DateFormatType.M:
                case DateFormatType.DD:
                case DateFormatType.D:
                    return Regex.IsMatch(value, @"^(([0-9]+)?)?$");
                default:
                case DateFormatType.YYYYMMDD:
                case DateFormatType.YYYYMD:
                case DateFormatType.YYMMDD:
                case DateFormatType.YYMD:
                    return Regex.IsMatch(value, @"^(([0-9]+)?(([^\/]+\/|\/[0-9]+)?){2})?$");
            }
        }

        /// <summary>
        /// 時間間隔に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <param name="format">時間間隔フォーマット</param>
        /// <returns>
        /// true : 時間間隔の文字
        /// false: それ以外
        /// </returns>
        public static bool IsTimeSpanMatch(char value, TimeSpanFormatType format = TimeSpanFormatType.HMS)
        {
            switch (format)
            {
                case TimeSpanFormatType.H:
                case TimeSpanFormatType.M:
                case TimeSpanFormatType.S:
                    return Regex.IsMatch(value.ToString(), @"[0-9\b]");
                default:
                    return Regex.IsMatch(value.ToString(), @"[0-9\:\b]");
            }
        }

        /// <summary>
        /// 時間間隔かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="format">時間間隔フォーマット</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 時間間隔の文字列
        /// false: それ以外
        /// </returns>
        public static bool IsTimeSpanMatch(string value, TimeSpanFormatType format = TimeSpanFormatType.HMS, bool strict = true)
        {
            switch (format)
            {
                case TimeSpanFormatType.HM:
                case TimeSpanFormatType.MS:
                    return Regex.IsMatch(value, ((strict) ? @"^((0|[1-9][0-9]*)+(\:[0-5][0-9])+)$" : @"^((0|[1-9][0-9]*)?([^\:]+\:|\:[0-5]|\:[0-5][0-9])?)?$"));
                case TimeSpanFormatType.H:
                case TimeSpanFormatType.M:
                case TimeSpanFormatType.S:
                    return Regex.IsMatch(value, ((strict) ? @"^((0|[1-9][0-9]*)+)$" : @"^((0|[1-9][0-9]*)?)?$"));
                default:
                case TimeSpanFormatType.HMS:
                    return Regex.IsMatch(value, ((strict) ? @"^((0|[1-9][0-9]*)+((\:[0-5][0-9])+){2})$" : @"^((0|[1-9][0-9]*)?(([^\:]+\:|\:[0-5]|\:[0-5][0-9])?){2})?$"));
            }
        }

        /// <summary>
        /// 電話番号に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 電話番号の文字
        /// false: それ以外
        /// </returns>
        public static bool IsPhoneNumberMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\-\b]");
        }

        /// <summary>
        /// 電話番号かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 電話番号の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 厳格にチェックしない場合、相応しい文字かどうかをチェックします。
        /// </remarks>
        public static bool IsPhoneNumberMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"\A0\d{2,4}-\d{2,4}-\d{4}\z|^\d{10}$|^\d{11}$" : @"^[0-9\-]*$"));
        }

        /// <summary>
        /// 郵便番号に相応しい文字かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字</param>
        /// <returns>
        /// true : 郵便番号の文字
        /// false: それ以外
        /// </returns>
        public static bool IsPostalCodeMatch(char value)
        {
            return Regex.IsMatch(value.ToString(), @"[0-9\-\b]");
        }

        /// <summary>
        /// 郵便番号かどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <param name="strict">厳格にチェックするかどうか</param>
        /// <returns>
        /// true : 郵便番号の文字列
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// 厳格にチェックしない場合、相応しい文字かどうかをチェックします。
        /// 7桁 (ハイフンあり8桁) のみ許容します。
        /// </remarks>
        public static bool IsPostalCodeMatch(string value, bool strict = true)
        {
            return Regex.IsMatch(value, ((strict) ? @"^\d{3}[\-]\d{4}$|^\d{7}$" : @"^[0-9\-]*$"));
            //return Regex.IsMatch(value, ((strict) ? @"^\d{3}[\-]\d{4}$|^\d{3}[\-]\d{2}$|^\d{3}$|^\d{5}$|^\d{7}$" : @"^[0-9\-]*$"));
        }
        #endregion
    }
}
