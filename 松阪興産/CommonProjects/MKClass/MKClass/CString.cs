using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKClass
{
    #region 列挙型
    /// <summary>
    /// 文字長の種別を示します。
    /// </summary>
    public enum LengthType
    {
        /// <summary>文字数</summary>
        Char,
        /// <summary>バイト数</summary>
        Byte,
        /// <summary>バイト数 (DB2)</summary>
        DB2Byte
    }
    #endregion



    #region Stringクラス (static)
    /// <summary>
    /// Stringクラス (static)
    /// </summary>
    public static class CString
    {
        #region 部分文字列取得
        /// <summary>
        /// 指定した文字列からバイト単位の部分文字列を取得します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="startIndex">開始文字位置</param>
        /// <param name="length">部分文字列のバイト数</param>
        /// <param name="encode">エンコード (null = Shift_JIS)</param>
        /// <returns>バイト単位の部分文字列</returns>
        public static string SubstringByte(this string value, int startIndex, int length, Encoding encode = null)
        {
            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                return new String(value.TakeWhile((c, i) => encode.GetByteCount(value.Substring(startIndex, i + 1)) <= length).ToArray());
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region パディング
        /// <summary>
        /// 指定されたバイト数になるまで左側に指定されたUnicode文字を埋め込み、
        /// このインスタンス内の文字を右寄せした文字列を取得します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="totalByte">結果として生成される文字列のバイト数</param>
        /// <param name="paddingChar">Unicode埋め込み文字</param>
        /// <param name="encode">エンコード (null = Shift_JIS)</param>
        /// <returns>このインスタンス内の文字を右寄せした文字列</returns>
        /// <remarks>
        /// totalByteがこのインスタンスのバイト数以下の場合、対象文字列をそのまま返します。
        /// </remarks>
        public static string PadLeftByte(this string value, int totalByte, char paddingChar = '\0', Encoding encode = null)
        {
            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                var stringLength = value.Length;
                var byteLength = encode.GetByteCount(value);
                var paddingByteLength = totalByte - byteLength;
                var paddingCharByte = encode.GetByteCount(paddingChar.ToString());
                var totalWidth = stringLength + (paddingByteLength / paddingCharByte);  // 端数切捨て

                if (paddingByteLength <= 0)
                {
                    return value;
                }

                return value.PadLeft(totalWidth, ((paddingChar != '\0') ? paddingChar : ' '));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定されたバイト数になるまで右側に指定されたUnicode文字を埋め込み、
        /// このインスタンス内の文字を左寄せした文字列を取得します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="totalByte">結果として生成される文字列のバイト数</param>
        /// <param name="paddingChar">Unicode埋め込み文字</param>
        /// <param name="encode">エンコード (null = Shift_JIS)</param>
        /// <returns>このインスタンス内の文字を左寄せした文字列</returns>
        /// <remarks>
        /// totalByteがこのインスタンスのバイト数以下の場合、対象文字列をそのまま返します。
        /// </remarks>
        public static string PadRightByte(this string value, int totalByte, char paddingChar = '\0', Encoding encode = null)
        {
            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                var stringLength = value.Length;
                var byteLength = encode.GetByteCount(value);
                var paddingByteLength = totalByte - byteLength;
                var paddingCharByte = encode.GetByteCount(paddingChar.ToString());
                var totalWidth = stringLength + (paddingByteLength / paddingCharByte);  // 端数切捨て

                if (paddingByteLength <= 0)
                {
                    return value;
                }

                return value.PadRight(totalWidth, ((paddingChar != '\0') ? paddingChar : ' '));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region シングルバイト判定
        /// <summary>
        /// 指定した文字列がシングルバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="encode">エンコード</param>
        /// <returns>
        /// true : シングルバイト
        /// false: それ以外
        /// </returns>
        public static bool IsSingleByte(this string value, Encoding encode)
        {
            try
            {
                return value.ToCharArray().IsSingleByte(encode);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した文字配列がシングルバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字配列</param>
        /// <param name="encode">エンコード</param>
        /// <returns>
        /// true : シングルバイト
        /// false: それ以外
        /// </returns>
        public static bool IsSingleByte(this char[] value, Encoding encode)
        {
            try
            {
                return value.IsSingleByte(encode, 0, value.Length);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した文字配列がシングルバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字配列</param>
        /// <param name="encode">エンコード</param>
        /// <param name="index">エンコードする最初の文字のインデックス</param>
        /// <param name="count">エンコードする文字数</param>
        /// <returns>
        /// true : シングルバイト
        /// false: それ以外
        /// </returns>
        public static bool IsSingleByte(this char[] value, Encoding encode, int index, int count)
        {
            if ((value == null || value.Length == 0) || index < 0 || count < 1)
            {
                return false;
            }

            try
            {
                var vArr = value.Skip(index).Take(count).ToArray();
                if (vArr == null || vArr.Length == 0)
                {
                    return false;
                }

                for (int i = 0; i < vArr.Length; i++)
                {
                    if (encode.GetByteCount(vArr, i, 1) != 1)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region マルチバイト判定
        /// <summary>
        /// 指定した文字列がマルチバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="encode">エンコード</param>
        /// <returns>
        /// true : マルチバイト
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// サロゲートペアを考慮します。
        /// </remarks>
        public static bool IsMultiByte(this string value, Encoding encode)
        {
            try
            {
                return value.ToCharArray().IsMultiByte(encode);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した文字配列がマルチバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字配列</param>
        /// <param name="encode">エンコード</param>
        /// <returns>
        /// true : マルチバイト
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// サロゲートペアを考慮します。
        /// </remarks>
        public static bool IsMultiByte(this char[] value, Encoding encode)
        {
            try
            {
                return value.IsMultiByte(encode, 0, value.Length);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した文字配列がマルチバイトかどうかを判定します。
        /// </summary>
        /// <param name="value">対象文字配列</param>
        /// <param name="encode">エンコード</param>
        /// <param name="index">エンコードする最初の文字のインデックス</param>
        /// <param name="count">エンコードする文字数</param>
        /// <returns>
        /// true : マルチバイト
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// サロゲートペアを考慮します。
        /// </remarks>
        public static bool IsMultiByte(this char[] value, Encoding encode, int index, int count)
        {
            if ((value == null || value.Length == 0) || index < 0 || count < 1)
            {
                return false;
            }

            try
            {
                var vArr = value.Skip(index).Take(count).ToArray();
                if (vArr == null || vArr.Length == 0)
                {
                    return false;
                }

                for (int i = 0; i < vArr.Length; i++)
                {
                    if (vArr.IsSingleByte(encode, i, 1))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 文字長の種別に応じた長さを取得
        /// <summary>
        /// 文字長の種別に応じた長さを取得します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="lengthType">文字長の種別</param>
        /// <param name="encode">エンコード</param>
        /// <param name="index">エンコードする最初の文字のインデックス</param>
        /// <param name="count">エンコードする文字数</param>
        /// <returns>文字長の種別に応じた長さ</returns>
        public static int GetLengthCount(this string value, LengthType lengthType, Encoding encode = null, int index = -1, int count = 1)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }

            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                int ret = 0;
                switch (lengthType)
                {
                    case LengthType.Char:
                        ret = value.Length;

                        break;
                    case LengthType.Byte:
                        if (index < 0)
                        {
                            ret = encode.GetByteCount(value);
                        }
                        else
                        {
                            ret = encode.GetByteCount(value.ToCharArray(), index, count);
                        }

                        break;
                    case LengthType.DB2Byte:
                        char[] vArr = value.ToCharArray();
                        for (int i = 0; i < vArr.Length; i++)
                        {
                            ret += encode.GetByteCount(vArr, i, 1);

                            if (vArr.IsMultiByte(encode, i, 1))
                            {
                                // SO (0x0E)
                                if (!vArr.IsMultiByte(encode, i - 1, 1))
                                {
                                    ret += 1;
                                }
                                // SI (0x0F)
                                if (!vArr.IsMultiByte(encode, i + 1, 1))
                                {
                                    ret += 1;
                                }
                            }
                        }

                        break;
                }

                return ret;
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
