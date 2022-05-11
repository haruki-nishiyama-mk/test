using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MKClass.MKControl
{
    #region 列挙型
    /// <summary>
    /// TextBoxの入力モードを示します。
    /// </summary>
    public enum TextBoxInputMode
    {
        /// <summary>制御しない</summary>
        NoControl,
        /// <summary>全角ひらがな</summary>
        Hiragana,
        /// <summary>全角カナ</summary>
        Katakana,
        /// <summary>半角カナ</summary>
        KatakanaHalf,
        /// <summary>英数字・記号</summary>
        Alpha,
        /// <summary>整数</summary>
        Int,
        /// <summary>符号なし整数</summary>
        UInt,
        /// <summary>浮動小数点数値</summary>
        Decimal,
        /// <summary>符号なし浮動小数点数値</summary>
        UDecimal,
        /// <summary>コード体系 (英数字)</summary>
        Code,
        /// <summary>コード体系 (英数字・ハイフン・アンダーライン)</summary>
        CodeSymbol,
        /// <summary>コード体系 (数字のみ)</summary>
        CodeNumOnly,
        /// <summary>コード体系 (英字のみ)</summary>
        CodeAlphaOnly,
        /// <summary>コード体系 (英数字・記号 (半角スペース含む) ・半角カナ)</summary>
        CodeKatakanaHalf,
        /// <summary>日付</summary>
        Date,
        /// <summary>時間間隔</summary>
        TimeSpan,
        /// <summary>電話番号</summary>
        PhoneNumber,
        /// <summary>郵便番号</summary>
        PostalCode
    }

    /// <summary>
    /// ComboBoxの入力モードを示します。
    /// </summary>
    public enum ComboBoxInputMode
    {
        /// <summary>制御しない</summary>
        NoControl,
        /// <summary>全角ひらがな</summary>
        Hiragana,
        /// <summary>全角カナ</summary>
        Katakana,
        /// <summary>半角カナ</summary>
        KatakanaHalf,
        /// <summary>英数字・記号</summary>
        Alpha,
        /// <summary>整数</summary>
        Int,
        /// <summary>符号なし整数</summary>
        UInt,
        /// <summary>浮動小数点数値</summary>
        Decimal,
        /// <summary>符号なし浮動小数点数値</summary>
        UDecimal,
        /// <summary>コード体系 (英数字) </summary>
        Code,
        /// <summary>コード体系 (英数字・ハイフン・アンダーライン) </summary>
        CodeSymbol,
        /// <summary>コード体系 (数字のみ) </summary>
        CodeNumOnly,
        /// <summary>コード体系 (英字のみ)</summary>
        CodeAlphaOnly,
        /// <summary>コード体系 (英数字・記号 (半角スペース含む) ・半角カナ)</summary>
        CodeKatakanaHalf
    }

    /// <summary>
    /// 入力コントロールの文字変換を示します。
    /// </summary>
    public enum ControlConvertCase
    {
        /// <summary>変換しない</summary>
        NormalCase,
        /// <summary>大文字変換</summary>
        UpperCase,
        /// <summary>小文字変換</summary>
        LowerCase
    }

    /// <summary>
    /// 入力コントロールの検証状態を示します。
    /// </summary>
    public enum ControlValidationStatus
    {
        /// <summary>正常</summary>
        Normal,
        /// <summary>値不正</summary>
        InValidValueError,
        /// <summary>桁溢れエラー</summary>
        OverflowError,
        /// <summary>必須エラー</summary>
        RequiredError
    }

    /// <summary>
    /// グリッド内でEnterを捕捉した際の挙動を示します。
    /// </summary>
    public enum DataGridViewActionPattern : int
    {
        /// <summary>DataGridView標準</summary>
        Default = 0,
        /// <summary>ダブルクリックと同等の挙動をする</summary>
        DoubleClick = 1,
        /// <summary>フォーカスを移動する</summary>
        MoveFocus = 2
    }
    #endregion



    #region コントロールカラー管理クラス (static)
    /// <summary>
    /// コントロールカラー管理クラス (static)
    /// </summary>
    public static class CColor
    {
        /// <summary>無効の背景色を示します。</summary>
        public static Color DisableBackColor { get { return Color.LightGray; } }
        /// <summary>読み取り専用の背景色を示します。</summary>
        public static Color ReadOnlyBackColor { get { return SystemColors.Info; } }
        /// <summary>必須項目の背景色を示します。</summary>
        public static Color RequiredBackColor { get { return Color.LightCyan; } }
        /// <summary>アクティブ時の背景色を示します。</summary>
        public static Color ActiveBackColor { get { return Color.Yellow; } }
    }
    #endregion



    #region コンポーネント検証クラス
    /// <summary>
    /// コンポーネント検証クラス
    /// </summary>
    public static class ComponentValidator
    {
        /// <summary>
        /// 指定されたIListSourceを実装したオブジェクトがnull、または要素数が0かどうかを示す値を取得します。
        /// </summary>
        /// <param name="self">IListSourceを実装したオブジェクト</param>
        /// <returns>
        /// true : null、または要素数が0
        /// false: それ以外
        /// </returns>
        public static bool IsNullOrEmpty(this IListSource self)
        {
            return (self == null || self.GetList().Count == 0);
        }

        /// <summary>
        /// 指定されたIEnumerableを実装したオブジェクトがnull、または要素数が0かどうかを示す値を取得します。
        /// </summary>
        /// <typeparam name="T">列挙するオブジェクトの型</typeparam>
        /// <param name="self">IEnumerableを実装したオブジェクト</param>
        /// <returns>
        /// true : null、または要素数が0
        /// false: それ以外
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return (self == null || !self.Any());
        }

        /// <summary>
        /// 指定されたXDocumentオブジェクトがnull、または子要素が0かどうかを示す値を取得します。
        /// </summary>
        /// <param name="self">XDocumentオブジェクト</param>
        /// <returns>
        /// true : null、または要素数が0
        /// false: それ以外
        /// </returns>
        public static bool IsNullOrEmpty(this XDocument self)
        {
            return (self == null || !self.Root.HasElements);
        }

        /// <summary>
        /// 指定されたXElementオブジェクトがnull、またはこの要素に内容が格納されていないかどうかを示す値を取得します。
        /// </summary>
        /// <param name="self">XElementオブジェクト</param>
        /// <returns>
        /// true : null、または要素数が0
        /// false: それ以外
        /// </returns>
        public static bool IsNullOrEmpty(this XElement self)
        {
            return (self == null || self.IsEmpty);
        }
    }
    #endregion



    #region コントロール変更検出インターフェース
    /// <summary>
    /// コントロール変更検出インターフェース
    /// </summary>
    /// <remarks>
    /// コントロールの変更を検出するためのインターフェースです。
    /// </remarks>
    public interface IControlChanged
    {
        /// <summary>
        /// 変更状態を取得します。
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// Validatedが発生するまでの一時的な変更状態を取得します。
        /// </summary>
        bool IsTempChanged { get; }

        /// <summary>
        /// 変更状態をクリアします。
        /// </summary>
        void Clear();
    }
    #endregion



    #region TextChangingイベント
    /// <summary>
    /// TextChangingイベント
    /// </summary>
    /// <remarks>
    /// TextChangingイベントのデータを提供します。
    /// </remarks>
    public class TextChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 変更後テキスト
        /// </summary>
        public string ChangedText { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextChangingEventArgs()
        {
            this.ChangedText = String.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text">変更後テキスト</param>
        public TextChangingEventArgs(string text)
        {
            this.ChangedText = text;
        }
    }
    #endregion
}
