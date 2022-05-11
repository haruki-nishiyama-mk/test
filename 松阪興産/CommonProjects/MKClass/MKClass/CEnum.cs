using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MKClass
{
    #region 列挙体クラス
    /// <summary>
    /// 列挙体クラス
    /// </summary>
    public static class CEnum
    {
        /// <summary>
        /// 文字列形式での1つ以上の列挙定数の名前または数値を、等価の列挙オブジェクトに変換します。
        /// </summary>
        /// <typeparam name="TEnum">変換先の列挙型</typeparam>
        /// <param name="value">変換する列挙定数の名前または基になる値の文字列形式</param>
        /// <param name="result">TEnumで表されるvalue型のオブジェクト</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return Enum.TryParse(value, out result) && Enum.IsDefined(typeof(TEnum), result);
        }
    }
    #endregion

    #region 列挙型クラス (abstract)
    /// <summary>
    /// 列挙型クラス (abstract)
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        #region プロパティ
        /// <summary>ID</summary>
        public int Id { get; private set; }
        /// <summary>クラス名</summary>
        public string Name { get; private set; }
        /// <summary>表示用のテキスト</summary>
        public string Text { get; private set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">クラス名</param>
        /// <param name="text">表示用のテキスト</param>
        protected Enumeration(int id, string name, string text)
        {
            this.Id = id;
            this.Name = name;
            this.Text = text;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを表す文字列</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">対象となる型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                            .Select(f => f.GetValue(null))
                            .Cast<T>();
        }

        /// <summary>
        /// 2つのオブジェクトインスタンスが等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在のオブジェクトと比較するオブジェクト</param>
        /// <returns>
        /// true : 指定したオブジェクトが現在のオブジェクトと等しい
        /// false: それ以外
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Enumeration))
            {
                return false;
            }

            var other = (Enumeration)obj;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(other.Id);

            return typeMatches && valueMatches;
        }

        /// <summary>
        /// 既定のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在のオブジェクトのハッシュコード</returns>
        public override int GetHashCode()
        {
            return this.Id ^ this.Name.GetHashCode();
        }

        /// <summary>
        /// 指定したオブジェクトとこのインスタンスを比較し、これらの相対値を示す値を返します。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <returns>このインスタンスとvalueの相対値を示す符号付き数値</returns>
        public int CompareTo(object other)
        {
            return Id.CompareTo(((Enumeration)other).Id);
        }
    }
    #endregion



    #region Enumに文字列を付加するためのAttributeクラス
    /// <summary>
    /// Enumに文字列を付加するためのAttributeクラス
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        /// <summary>
        /// 列挙型の文字列
        /// </summary>
        public string StringValue { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">列挙型の文字列</param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }

    /// <summary>
    /// Attributeクラス (共通定義)
    /// </summary>
    public static class CommonAttribute
    {
        /// <summary>
        /// 指定された列挙型の文字列を取得します。
        /// </summary>
        /// <param name="value">列挙型</param>
        /// <returns>列挙型の文字列</returns>
        public static string GetStringValue(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return null;
            }

            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
    #endregion
}
