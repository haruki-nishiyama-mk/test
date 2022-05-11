using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using MKClass.MKData;

namespace MKClass.MKControl
{
    /// <summary>
    /// ComboBox拡張クラス
    /// </summary>
    public class CComboBox : ComboBox, IControlChanged
    {
        #region 内部クラス
        #region TextBoxWindowクラス (テキストの入力制御)
        /// <summary>
        /// TextBoxWindowクラス (テキストの入力制御)
        /// </summary>
        private class TextBoxWindow : NativeWindow
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct ComboBoxInfo
            {
                public Int32 CBSize;
                public Rectangle RCItem;
                public Rectangle RCButton;
                public int ButtonState;
                public IntPtr HWNDCombo;
                public IntPtr HWNDEdit;
                public IntPtr HWNDList;
            }

            [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
            private static extern IntPtr SendMessageCB(IntPtr hWnd, UInt32 dwMsg, IntPtr wParam, out ComboBoxInfo lParam);

            private CComboBox _parent = null;

            public TextBoxWindow(CComboBox parent)
            {
                var cbInfo = new ComboBoxInfo();
                cbInfo.CBSize = Marshal.SizeOf(cbInfo);
                SendMessageCB(parent.Handle, 0x164, IntPtr.Zero, out cbInfo);
                this.AssignHandle(cbInfo.HWNDEdit);

                _parent = parent;
            }

            /// <summary>
            /// Windowsメッセージを処理します。
            /// </summary>
            /// <param name="m">処理するWindowsメッセージ</param>
            /// <remarks>
            /// InputModeプロパティに応じた入力中テキストの検証を行います。
            /// 　※ クリップボードからの入力を考慮します。
            /// DoEventプロパティを制御します。
            /// </remarks>
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            protected override void WndProc(ref Message m)
            {
                const int WM_KEYDOWN = 0x100;
                const int WM_CHAR = 0x102;
                const int WM_PASTE = 0x302;
                const int WM_IME_ENDCOMPOSITION = 0x010E;
                const int WM_IME_COMPOSITION = 0x010D;

                switch (m.Msg)
                {
                    case WM_KEYDOWN:
                        // Deleteキーが押された場合
                        if (m.WParam.ToInt32() == (int)Keys.Delete)
                        {
                            TextChangingEventArgs e = new TextChangingEventArgs(
                                                                _parent.CreateChangedText(String.Empty, true));
                            _parent.OnTextChanging(e);
                            if (e.Cancel)
                            {
                                // Deleteキーをキャンセルする
                                m.Result = new IntPtr(1);   // true

                                return;
                            }
                        }

                        break;
                    case WM_CHAR:
                        // 文字が入力された場合
                        {
                            TextChangingEventArgs e = new TextChangingEventArgs(
                                                                _parent.CreateChangedText(Convert.ToString((char)(m.WParam.ToInt32()))));
                            _parent.OnTextChanging(e);
                            if (e.Cancel)
                            {
                                // 入力された文字をキャンセルする
                                return;
                            }
                        }

                        break;
                    case WM_PASTE:
                        // クリップボードからの入力内容を取得する
                        string clipText = (string)Clipboard.GetDataObject().GetData(DataFormats.Text);
                        if (clipText != null)
                        {
                            TextChangingEventArgs e = new TextChangingEventArgs(
                                                                _parent.CreateChangedText(clipText));
                            _parent.OnTextChanging(e);
                            if (e.Cancel)
                            {
                                // 入力された内容をキャンセルする
                                return;
                            }
                        }

                        break;
                    case WM_IME_COMPOSITION:
                        // IMEから未確定文字列や確定された文字列がやってきたとき
                        _parent.DoEvent = false;

                        break;
                    case WM_IME_ENDCOMPOSITION:
                        // IMEから未確定文字列がなくなったとき
                        _parent.DoEvent = true;

                        break;
                    default:
                        break;
                }

                base.WndProc(ref m);
            }
        }
        #endregion
        #endregion

        #region 変数
        /// <summary>TextBoxWindowのオブジェクト</summary>
        private TextBoxWindow _tbw = null;
        #endregion

        #region プロパティ (基底)
        /// <summary>
        /// コンポーネントの背景色です。
        /// </summary>
        /// <remarks>
        /// base.BackColorは非表示にします。
        /// </remarks>
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        private int _maxLength = 0;
        /// <summary>
        /// 入力モードに応じた最大文字数を指定します。
        /// 数値型の場合、有効桁数となります。 (ハイフン(-)、ピリオド(.)、カンマ(,)は除きます。)
        /// </summary>
        /// <remarks>
        /// 基底のMaxLangthは常に無制限となります。
        /// テキストの文字数が範囲内であるかどうかをチェックしたい場合、
        /// IsLengthInOfRangeメソッドをコールしてください。
        /// </remarks>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("入力モードに応じた最大文字数を指定します。数値型の場合、有効桁数となります。 (ハイフン(-)、ピリオド(.)、カンマ(,)は除きます。)")]
        [DefaultValue(0)]
        public new int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                base.MaxLength = 0;     // 無制限

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                    "MaxLength",
                    value,
                    "0 <= must be");
                }
                else if (value > 0)
                {
                    if (value <= _decimalScale)
                    {
                        throw new ArgumentOutOfRangeException(
                        "MaxLength",
                        value,
                        "DecimalScale < must be");
                    }
                }

                _maxLength = value;
            }
        }

        /// <summary>
        /// コントロールの IME (Input Method Editor) モードを取得または設定します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        public new ImeMode ImeMode
        {
            get
            {
                return base.ImeMode;
            }
            set
            {
                base.ImeMode = value;
            }
        }

        /// <summary>
        /// リストの要素の描画を処理するのは、
        /// コードとオペレーティングシステムのどちらであるかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(DrawMode.OwnerDrawFixed)]
        public new DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            set
            {
                base.DrawMode = value;
            }
        }

        /// <summary>
        /// 一部の項目しか表示されない状況を避けるために、
        /// コントロールのサイズを変更するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool IntegralHeight
        {
            get
            {
                return base.IntegralHeight;
            }
            set
            {
                base.IntegralHeight = value;
            }
        }
        #endregion

        #region プロパティ (追加)
        private Color _baseBackColor = SystemColors.Window;
        /// <summary>
        /// 標準の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("標準の背景色です。")]
        [DefaultValue(typeof(Color), "Window")]
        public Color BaseBackColor
        {
            get
            {
                return _baseBackColor;
            }
            set
            {
                _baseBackColor = value;

                if (!_isRequired)
                {
                    base.BackColor = _baseBackColor;
                }
            }
        }

        private Color _requiredBackColor = CColor.RequiredBackColor;
        /// <summary>
        /// 必須項目の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("必須項目の背景色です。")]
        [DefaultValue(typeof(Color), "LightCyan")]
        public Color RequiredBackColor
        {
            get
            {
                return _requiredBackColor;
            }
            set
            {
                _requiredBackColor = value;

                if (_isRequired)
                {
                    base.BackColor = _requiredBackColor;
                }
            }
        }

        private Color _activeBackColor = CColor.ActiveBackColor;
        /// <summary>
        /// アクティブ時の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("アクティブ時の背景色です。")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color ActiveBackColor
        {
            get
            {
                return _activeBackColor;
            }
            set
            {
                _activeBackColor = value;
            }
        }

        private string _displayMember2 = "";
        /// <summary>
        /// ドロップダウンリスト (2列目) に表示するプロパティを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("ドロップダウンリスト (2列目) に表示するプロパティを示します。")]
        [DefaultValue("")]
        public string DisplayMember2
        {
            get
            {
                return _displayMember2;
            }
            set
            {
                _displayMember2 = value;
            }
        }

        private bool _autoDropDownWidth = true;
        /// <summary>
        /// DropDownWidthプロパティを自動で設定するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("DropDownWidthプロパティを自動で設定するかどうかを示します。")]
        [DefaultValue(true)]
        public bool AutoDropDownWidth
        {
            get
            {
                return _autoDropDownWidth;
            }
            set
            {
                _autoDropDownWidth = value;
            }
        }

        private ComboBoxInputMode _inputMode = ComboBoxInputMode.NoControl;
        /// <summary>
        /// このコントロール内で許可する入力モードを決定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("このコントロール内で許可する入力モードを決定します。")]
        [DefaultValue(ComboBoxInputMode.NoControl)]
        public ComboBoxInputMode InputMode
        {
            get
            {
                return _inputMode;
            }
            set
            {
                _inputMode = value;

                switch (_inputMode)
                {
                    case ComboBoxInputMode.Hiragana:
                        this.ImeMode = ImeMode.Hiragana;

                        break;
                    case ComboBoxInputMode.Katakana:
                        this.ImeMode = ImeMode.Katakana;

                        break;
                    case ComboBoxInputMode.KatakanaHalf:
                        this.ImeMode = ImeMode.KatakanaHalf;

                        break;
                    case ComboBoxInputMode.Alpha:
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case ComboBoxInputMode.Int:
                    case ComboBoxInputMode.UInt:
                    case ComboBoxInputMode.Decimal:
                    case ComboBoxInputMode.UDecimal:
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case ComboBoxInputMode.Code:
                    case ComboBoxInputMode.CodeSymbol:
                    case ComboBoxInputMode.CodeNumOnly:
                    case ComboBoxInputMode.CodeAlphaOnly:
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case ComboBoxInputMode.CodeKatakanaHalf:
                        this.ImeMode = ImeMode.KatakanaHalf;

                        break;
                    default:
                        this.ImeMode = ImeMode.NoControl;

                        break;
                }
            }
        }

        private ControlConvertCase _convertCase = ControlConvertCase.NormalCase;
        /// <summary>
        /// 文字列を大文字、小文字に変換するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("文字列を大文字、小文字に変換するかどうかを示します。")]
        [DefaultValue(ControlConvertCase.NormalCase)]
        public ControlConvertCase ConvertCase
        {
            get
            {
                return _convertCase;
            }
            set
            {
                _convertCase = value;
            }
        }

        private int _decimalScale = 0;
        /// <summary>
        /// decimal型の場合、少数点以下の桁数を指定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("decimal型の場合、少数点以下の桁数を指定します。")]
        [DefaultValue(0)]
        public int DecimalScale
        {
            get
            {
                return _decimalScale;
            }
            set
            {
                if (value < 0 || 38 < value)
                {
                    throw new ArgumentOutOfRangeException(
                    "DecimalScale",
                    value,
                    "0 <= must be <= 38");
                }

                _decimalScale = value;
            }
        }

        private LengthType _lengthType = LengthType.Char;
        /// <summary>
        /// 文字長の種別を指定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("文字長の種別を指定します。")]
        [DefaultValue(LengthType.Char)]
        public LengthType LengthType
        {
            get
            {
                return _lengthType;
            }
            set
            {
                _lengthType = value;
            }
        }

        private bool _numericDelimiter = false;
        /// <summary>
        /// 数値型の場合、3桁ごとにカンマで区切るかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("数値型の場合、3桁ごとにカンマで区切るかどうかを示します。")]
        [DefaultValue(false)]
        public bool NumericDelimiter
        {
            get
            {
                return _numericDelimiter;
            }
            set
            {
                _numericDelimiter = value;
            }
        }

        private char _paddingChar = '\0';
        /// <summary>
        /// Code型の場合、現在の文字列の先頭に指定されたUnicode文字でパディングします。
        /// MaxLengthが規定値の場合、パディングしません。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("Code型の場合、現在の文字列の先頭に指定されたUnicode文字でパディングします。MaxLengthが規定値の場合、パディングしません。")]
        [DefaultValue('\0')]
        public char PaddingChar
        {
            get
            {
                return _paddingChar;
            }
            set
            {
                _paddingChar = value;
            }
        }

        private bool _isRequired = false;
        /// <summary>
        /// 必須かどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("必須かどうかを示します。")]
        [DefaultValue(false)]
        public bool IsRequired
        {
            get
            {
                return _isRequired;
            }
            set
            {
                _isRequired = value;

                base.BackColor = (_isRequired) ? _requiredBackColor : _baseBackColor;
            }
        }

        private bool _isRequiredValidating = false;
        /// <summary>
        /// Validatingイベントで必須チェックを行うかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("Validatingイベントで必須チェックを行うかどうかを示します。")]
        [DefaultValue(false)]
        public bool IsRequiredValidating
        {
            get
            {
                return _isRequiredValidating;
            }
            set
            {
                _isRequiredValidating = value;
            }
        }

        private bool _isFindStringExact = true;
        /// <summary>
        /// 指定した文字列と厳密に一致する項目を検索するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("指定した文字列と厳密に一致する項目を検索するかどうかを示します。")]
        [DefaultValue(true)]
        public bool IsFindStringExact
        {
            get
            {
                return _isFindStringExact;
            }
            set
            {
                _isFindStringExact = value;
            }
        }

        private ToolStripLabel _toolStripLabel = null;
        /// <summary>
        /// ツールバーに使用するToolStripLabelを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("ツールバーに使用するToolStripLabelを示します。")]
        [DefaultValue(null)]
        public ToolStripLabel ToolStripLabel
        {
            get
            {
                return _toolStripLabel;
            }
            set
            {
                _toolStripLabel = value;
            }
        }

        private string _toolStripLabelText = "";
        /// <summary>
        /// ToolStripLabelに関連付けられたテキストです。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("ToolStripLabelに関連付けられたテキストです。")]
        [DefaultValue("")]
        public string ToolStripLabelText
        {
            get
            {
                return _toolStripLabelText;
            }
            set
            {
                _toolStripLabelText = value;
            }
        }

        private bool _doEvent = true;
        /// <summary>
        /// イベントを実行するかどうかを示します。
        /// </summary>
        public bool DoEvent
        {
            get
            {
                return _doEvent;
            }
            private set
            {
                _doEvent = value;
            }
        }

        private bool _isChanged = false;
        /// <summary>
        /// 変更状態を取得します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        bool IControlChanged.IsChanged
        {
            get
            {
                return _isChanged;
            }
        }

        private bool _isTempChanged = false;
        /// <summary>
        /// Validatedが発生するまでの一時的な変更状態を取得します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        bool IControlChanged.IsTempChanged
        {
            get
            {
                return _isTempChanged;
            }
        }

        ControlValidationStatus _validationStatus = ControlValidationStatus.Normal;
        /// <summary>
        /// 入力コントロールの検証状態を示します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        public ControlValidationStatus ValidationStatus
        {
            get
            {
                return _validationStatus;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CComboBox()
            : base()
        {
            ////// プロパティ
            base.MaxLength = 0;

            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.IntegralHeight = false;
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールに対してハンドルが作成されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            _tbw = new TextBoxWindow(this);

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// コントロールのハンドルが破棄されているときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            _tbw.ReleaseHandle();

            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// オーナー描画 ComboBox のビジュアルな部分を変更すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// リストに実際の値 (1列目) と表示の値 (2列目) を両方表示します。
        /// </remarks>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            var dt = this.DataSource as DataTable;
            if (dt.IsNullOrEmpty())
            {
                return;
            }

            if (e.Index == -1)
            {
                return;
            }

            var displayMembers = new List<string>() { this.DisplayMember, this.DisplayMember2 };

            var p = new Pen(Color.Gray);
            var b = new SolidBrush(e.ForeColor);

            var width = 0;

            try
            {
                e.DrawBackground();

                for (int i = 0; i < displayMembers.Count; i++)
                {
                    var displayMember = displayMembers[i];
                    if (String.IsNullOrEmpty(displayMember) || dt.Columns.IndexOf(displayMember) == -1)
                    {
                        continue;
                    }

                    // 境界線描画
                    e.Graphics.DrawLine(p, width, e.Bounds.Top, width, e.Bounds.Bottom);

                    // 文字列描画
                    e.Graphics.DrawString(dt.Rows[e.Index][displayMember].ToString(), e.Font, b, width, e.Bounds.Y);

                    string maxLengthString = dt.AsEnumerable()
                                                       .Select(row => row[displayMember].ToString())
                                                       .OrderByDescending(st => Encoding.GetEncoding("Shift_JIS").GetByteCount(st))
                                                       .FirstOrDefault();
                    width += TextRenderer.MeasureText(maxLengthString, this.Font).Width;
                }

                if (CConvert.ToBoolean(e.State & DrawItemState.Selected))
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
                }
            }
            catch
            {

            }
            finally
            {
                p.Dispose();
                b.Dispose();
            }
        }

        /// <summary>
        /// コントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            switch (_inputMode)
            {
                case ComboBoxInputMode.Int:
                case ComboBoxInputMode.UInt:
                case ComboBoxInputMode.Decimal:
                case ComboBoxInputMode.UDecimal:
                    this.Text = this.Text.Replace(",", "");

                    break;
            }

            base.BackColor = _activeBackColor;

            if (_toolStripLabel != null)
            {
                _toolStripLabel.Text = _toolStripLabelText;
            }

            SetDropDownWidth();
        }

        /// <summary>
        /// コントロールにフォーカスがあるときに、
        /// 文字、スペース、またはBackspaceキーが押された場合に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            string convertChar = "";
            switch (_convertCase)
            {
                case ControlConvertCase.LowerCase:
                    convertChar = e.KeyChar.ToString().ToLower();

                    break;
                case ControlConvertCase.UpperCase:
                    convertChar = e.KeyChar.ToString().ToUpper();

                    break;
            }
            if (!String.IsNullOrEmpty(convertChar) &&
                                    CValidate.IsToChar(convertChar))
            {
                e.KeyChar = CConvert.ToChar(convertChar);
            }

            e.Handled = !Validation(e.KeyChar);

            base.OnKeyPress(e);
        }

        #region TextChangingイベント
        /// <summary>
        /// TextChangingイベントハンドラ
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        public delegate void TextChangingEventHandler(object sender, TextChangingEventArgs e);

        /// <summary>
        /// Textプロパティの値がコントロールで変更される直前に発生します。
        /// </summary>
        public event TextChangingEventHandler TextChanging;

        /// <summary>
        /// Textプロパティの値がコントロールで変更される直前に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected virtual void OnTextChanging(TextChangingEventArgs e)
        {
            e.Cancel = (Validation(e.ChangedText, false, false) != ControlValidationStatus.Normal);

            // ユーザ定義のイベントを呼ぶ
            if (TextChanging != null)
            {
                TextChanging(this, e);
            }
        }
        #endregion

        /// <summary>
        /// 擬似的に変更後のテキストを作成します。
        /// </summary>
        /// <param name="inputText">入力テキスト</param>
        /// <returns>変更後テキスト</returns>
        private string CreateChangedText(string inputText)
        {
            return this.CreateChangedText(inputText, false);
        }

        /// <summary>
        /// 擬似的に変更後のテキストを作成します。
        /// </summary>
        /// <param name="inputText">入力テキスト</param>
        /// <param name="deleteKey">Deleteキー</param>
        /// <returns>変更後テキスト</returns>
        private string CreateChangedText(string inputText, bool deleteKey)
        {
            string changedText = this.Text;

            changedText = changedText.Remove(this.SelectionStart, this.SelectionLength);

            if (deleteKey)
            {
                if (this.SelectionLength == 0 && this.SelectionStart < this.Text.Length)
                {
                    changedText = changedText.Remove(this.SelectionStart, 1);
                }

                return changedText;
            }

            int crIndex = inputText.IndexOf("\r");
            if (crIndex > 0)
            {
                inputText = inputText.Remove(crIndex);
            }

            int lrIndex = inputText.IndexOf("\n");
            if (lrIndex > 0)
            {
                inputText = inputText.Remove(lrIndex);
            }

            if (inputText == String.Empty)
            {
                return changedText;
            }

            if (Convert.ToInt32(inputText[0]) == (int)Keys.Back)
            {
                if (this.SelectionLength == 0 && this.SelectionStart > 0)
                {
                    changedText = changedText.Remove(this.SelectionStart - 1, 1);
                }
            }
            else
            {
                switch (_convertCase)
                {
                    case ControlConvertCase.LowerCase:
                        inputText = inputText.ToLower();

                        break;
                    case ControlConvertCase.UpperCase:
                        inputText = inputText.ToUpper();

                        break;
                }

                changedText = changedText.Insert(this.SelectionStart, inputText);
            }

            return changedText;
        }

        /// <summary>
        /// SelectedValueプロパティが変更されたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);

            _isChanged = true;
            _isTempChanged = true;
        }

        /// <summary>
        /// Textプロパティの値が変化すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _isChanged = true;
            _isTempChanged = true;
        }

        /// <summary>
        /// 入力フォーカスがコントロールを離れると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            SetFormatText();
        }

        /// <summary>
        /// コントロールが検証しているときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnValidating(CancelEventArgs e)
        {
            e.Cancel = ((_validationStatus = Validation(true, _isRequiredValidating)) != ControlValidationStatus.Normal);

            base.OnValidating(e);
        }

        /// <summary>
        /// コントロールの検証が終了すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);

            SetBackColor();

            if (_toolStripLabel != null)
            {
                _toolStripLabel.Text = "";
            }

            _isTempChanged = false;
            _validationStatus = ControlValidationStatus.Normal;
        }

        /// <summary>
        /// Enabledプロパティ値が変更されたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            SetBackColor();
        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// 背景色を設定します。
        /// </summary>
        /// <remarks>
        /// アクティブ時の背景色の設定は行いません。
        /// </remarks>
        public void SetBackColor()
        {
            base.BackColor = (this.Enabled) ? ((_isRequired) ? _requiredBackColor : _baseBackColor) : CColor.DisableBackColor;
        }

        /// <summary>
        /// DropDownWidthプロパティを動的にセットします。
        /// </summary>
        public void SetDropDownWidth()
        {
            try
            {
                this.DropDownWidth = GetAutoDropDownWidth();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ドロップダウンリストの幅を動的に取得します。
        /// </summary>
        public int GetAutoDropDownWidth()
        {
            if (!_autoDropDownWidth)
            {
                return this.DropDownWidth;
            }

            var dt = this.DataSource as DataTable;
            if (dt.IsNullOrEmpty())
            {
                return this.DropDownWidth;
            }

            var displayMembers = new List<string>() { this.DisplayMember, this.DisplayMember2 };

            var width = 0;

            try
            {
                for (int i = 0; i < displayMembers.Count; i++)
                {
                    var displayMember = displayMembers[i];
                    if (String.IsNullOrEmpty(displayMember) || dt.Columns.IndexOf(displayMember) == -1)
                    {
                        continue;
                    }

                    string maxLengthString = dt.AsEnumerable()
                                                        .Select(row => row[displayMember].ToString())
                                                        .OrderByDescending(st => Encoding.GetEncoding("Shift_JIS").GetByteCount(st))
                                                        .FirstOrDefault();
                    width += TextRenderer.MeasureText(maxLengthString, this.Font).Width;
                }

                width += (dt.Rows.Count > this.MaxDropDownItems) ? 20 : 0;  // スクロールバーを考慮

                return width;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// フォーマット変換したテキストを設定します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public void SetFormatText()
        {
            this.Text = GetFormatText();
        }

        /// <summary>
        /// フォーマット変換したテキストを設定します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        public void SetFormatText(string text)
        {
            this.Text = GetFormatText(text);
        }

        /// <summary>
        /// フォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>フォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public string GetFormatText()
        {
            return GetFormatText(this.Text);
        }

        /// <summary>
        /// フォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>フォーマット変換したテキスト</returns>
        public string GetFormatText(string text)
        {
            switch (_convertCase)
            {
                case ControlConvertCase.LowerCase:
                    text = text.ToLower();

                    break;
                case ControlConvertCase.UpperCase:
                    text = text.ToUpper();

                    break;
            }

            if (!String.IsNullOrEmpty(text))
            {
                string validationText = text;
                switch (_inputMode)
                {
                    case ComboBoxInputMode.Int:
                    case ComboBoxInputMode.UInt:
                    case ComboBoxInputMode.Decimal:
                    case ComboBoxInputMode.UDecimal:
                        // カンマのみだった場合、そのまま返す
                        validationText = text.Replace(",", "");
                        if (String.IsNullOrEmpty(validationText))
                        {
                            return text;
                        }

                        // ハイフンのみだった場合、そのまま返す
                        validationText = text.Replace("-", "");
                        if (String.IsNullOrEmpty(validationText))
                        {
                            return text;
                        }

                        break;
                }

                switch (_inputMode)
                {
                    case ComboBoxInputMode.Katakana:
                        text = CConvert.ConvertToKatakana(text);

                        break;
                    case ComboBoxInputMode.KatakanaHalf:
                        text = CConvert.ConvertToKatakanaHalf(text);

                        break;
                    case ComboBoxInputMode.Int:
                    case ComboBoxInputMode.UInt:
                        text = CConvert.ToInt(text.Replace(",", "")).ToString(GetIntFormat());

                        break;
                    case ComboBoxInputMode.Decimal:
                    case ComboBoxInputMode.UDecimal:
                        text = CConvert.ToDecimal(text.Replace(",", "")).ToString(GetDecimalFormat());

                        break;
                    case ComboBoxInputMode.Code:
                    case ComboBoxInputMode.CodeSymbol:
                    case ComboBoxInputMode.CodeNumOnly:
                    case ComboBoxInputMode.CodeAlphaOnly:
                        text = GetCodeTypeFormatText(text);

                        break;
                    case ComboBoxInputMode.CodeKatakanaHalf:
                        text = GetCodeTypeFormatText(CConvert.ConvertToKatakanaHalf(text));

                        break;
                }
            }

            return text;
        }

        /// <summary>
        /// int型のフォーマットを取得します。
        /// </summary>
        /// <returns>int型のフォーマット</returns>
        public string GetIntFormat()
        {
            return (_numericDelimiter) ? @"#,0" : @"#0";
        }

        /// <summary>
        /// int型にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>int型にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// 変換対象のテキストが空、または変換できない場合、default(int)を取得します。
        /// </remarks>
        public string GetIntFormatText()
        {
            return GetIntFormatText(this.Text);
        }

        /// <summary>
        /// int型にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>int型にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// 変換対象のテキストが空、または変換できない場合、default(int)を取得します。
        /// </remarks>
        public string GetIntFormatText(string text)
        {
            return CConvert.ToIntDef(text).ToString(GetIntFormat());
        }

        /// <summary>
        /// int型にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// 変換対象のテキストが空、または変換できない場合、default(int)に変換します。
        /// </remarks>
        public void ToIntFormat()
        {
            this.Text = GetIntFormatText();
        }

        /// <summary>
        /// int型にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <remarks>
        /// 変換対象のテキストが空、または変換できない場合、default(int)に変換します。
        /// </remarks>
        public void ToIntFormat(string text)
        {
            this.Text = GetIntFormatText(text);
        }

        /// <summary>
        /// decimal型のフォーマットを取得します。
        /// </summary>
        /// <returns>decimal型のフォーマット</returns>
        public string GetDecimalFormat()
        {
            string format = (_numericDelimiter) ? @"#,0." : @"0.";
            for (int i = 0; i < _decimalScale; i++)
            {
                format += @"0";
            }

            return format;
        }

        /// <summary>
        /// decimal型にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>decimal型にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// 変換対象のテキストが空、または変換できない場合、default(decimal)を取得します。
        /// </remarks>
        public string GetDecimalFormatText()
        {
            return GetDecimalFormatText(this.Text);
        }

        /// <summary>
        /// decimal型にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>decimal型にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// 変換対象のテキストが空、または変換できない場合、default(decimal)を取得します。
        /// </remarks>
        public string GetDecimalFormatText(string text)
        {
            return CConvert.ToDecimalDef(text).ToString(GetDecimalFormat());
        }

        /// <summary>
        /// decimal型にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// 変換対象のテキストが空、または変換できない場合、default(decimal)に変換します。
        /// </remarks>
        public void ToDecimalFormat()
        {
            this.Text = GetDecimalFormatText();
        }

        /// <summary>
        /// decimal型にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <remarks>
        /// 変換対象のテキストが空、または変換できない場合、default(decimal)に変換します。
        /// </remarks>
        public void ToDecimalFormat(string text)
        {
            this.Text = GetDecimalFormatText(text);
        }

        /// <summary>
        /// コード体系にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>コード体系にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public string GetCodeTypeFormatText()
        {
            return GetCodeTypeFormatText(this.Text);
        }

        /// <summary>
        /// コード体系にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>コード体系にフォーマット変換したテキスト</returns>
        public string GetCodeTypeFormatText(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            return (_maxLength > 0 && _paddingChar != '\0') ? text.PadLeft(_maxLength, _paddingChar) : text;
        }

        /// <summary>
        /// コード体系にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public void ToCodeTypeFormat()
        {
            this.Text = GetCodeTypeFormatText();
        }

        /// <summary>
        /// コード体系にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        public void ToCodeTypeFormat(string text)
        {
            this.Text = GetCodeTypeFormatText(text);
        }

        /// <summary>
        /// 入力文字の検証を行います。
        /// </summary>
        /// <param name="c">検証対象の文字</param>
        /// <returns>
        /// true : 正当
        /// false: それ以外
        /// </returns>
        public bool Validation(char c)
        {
            if (!String.IsNullOrEmpty(c.ToString()))
            {
                switch (_inputMode)
                {
                    case ComboBoxInputMode.Katakana:
                        if (!CValidate.IsKatakanaMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.KatakanaHalf:
                        if (!CValidate.IsKatakanaHalfMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.Int:
                        if (!CValidate.IsIntMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.UInt:
                        if (!CValidate.IsUIntMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.Decimal:
                        if (!CValidate.IsDecimalMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.UDecimal:
                        if (!CValidate.IsUDecimalMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.Code:
                        if (!CValidate.IsCodeMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.CodeSymbol:
                        if (!CValidate.IsCodeSymbolMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.CodeNumOnly:
                        if (!CValidate.IsCodeNumOnlyMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.CodeAlphaOnly:
                        if (!CValidate.IsCodeAlphaOnlyMatch(c))
                        {
                            return false;
                        }

                        break;
                    case ComboBoxInputMode.CodeKatakanaHalf:
                        if (!CValidate.IsCodeKatakanaHalf(c))
                        {
                            return false;
                        }

                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        /// <remarks>
        /// Textプロパティの値を検証します。
        /// </remarks>
        public ControlValidationStatus Validation(bool strict = true, bool required = true)
        {
            return Validation(this.Text, strict, required);
        }

        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="text">検証対象のテキスト</param>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        public ControlValidationStatus Validation(string text, bool strict = true, bool required = true)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string validationText = text;
                switch (_inputMode)
                {
                    case ComboBoxInputMode.Int:
                    case ComboBoxInputMode.UInt:
                    case ComboBoxInputMode.Decimal:
                    case ComboBoxInputMode.UDecimal:
                        // カンマのみだった場合、エラー
                        validationText = text.Replace(",", "");
                        if (String.IsNullOrEmpty(validationText))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                }

                switch (_inputMode)
                {
                    case ComboBoxInputMode.Katakana:
                        if (!CValidate.IsKatakanaMatch(text))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.KatakanaHalf:
                        if (!CValidate.IsKatakanaHalfMatch(text))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.Int:
                        if (!CValidate.IsIntMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.UInt:
                        if (!CValidate.IsUIntMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.Decimal:
                        if (!CValidate.IsDecimalMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.UDecimal:
                        if (!CValidate.IsUDecimalMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.Code:
                        if (!CValidate.IsCodeMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.CodeSymbol:
                        if (!CValidate.IsCodeSymbolMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.CodeNumOnly:
                        if (!CValidate.IsCodeNumOnlyMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.CodeAlphaOnly:
                        if (!CValidate.IsCodeAlphaOnlyMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case ComboBoxInputMode.CodeKatakanaHalf:
                        if (!CValidate.IsCodeKatakanaHalf(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                }

                if (strict)
                {
                    if (_isFindStringExact)
                    {
                        if (this.FindStringExact(GetFormatText(text)) == -1)
                        {
                            return ControlValidationStatus.InValidValueError;
                        }
                    }
                }

                if (!IsLengthInOfRange(text))
                {
                    return ControlValidationStatus.OverflowError;
                }
            }
            else
            {
                if (required && _isRequired)
                {
                    return ControlValidationStatus.RequiredError;
                }
            }

            return ControlValidationStatus.Normal;
        }

        /// <summary>
        /// テキストの文字数が範囲内であるかどうかをチェックします。
        /// </summary>
        /// <returns>
        /// true : 範囲内
        /// false: それ以外
        /// </returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public bool IsLengthInOfRange()
        {
            return IsLengthInOfRange(this.Text);
        }

        /// <summary>
        /// テキストの文字数が範囲内であるかどうかをチェックします。
        /// </summary>
        /// <param name="text">チェック対象のテキスト</param>
        /// <returns>
        /// true : 範囲内
        /// false: それ以外
        /// </returns>
        public bool IsLengthInOfRange(string text)
        {
            int maxLength = _maxLength;
            int decimalScale = _decimalScale;
            LengthType lengthType = _lengthType;

            if (!String.IsNullOrEmpty(text))
            {
                switch (_inputMode)
                {
                    case ComboBoxInputMode.Int:
                    case ComboBoxInputMode.UInt:
                        text = text.Replace(",", "").Replace("-", "");

                        if (maxLength > 0)
                        {
                            if (text.GetLengthCount(lengthType) > maxLength)
                            {
                                return false;
                            }
                        }

                        break;
                    case ComboBoxInputMode.Decimal:
                    case ComboBoxInputMode.UDecimal:
                        text = text.Replace(",", "").Replace("-", "");

                        string[] textArr = text.Split('.');

                        if (maxLength > 0)
                        {
                            if (textArr[0].GetLengthCount(lengthType) > maxLength - decimalScale)
                            {
                                return false;
                            }
                        }

                        if (textArr.Length == 1)
                        {

                        }
                        else if (textArr.Length == 2)
                        {
                            if (decimalScale == 0)
                            {
                                return false;
                            }

                            if (textArr[1].GetLengthCount(lengthType) > decimalScale)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    default:
                        if (maxLength > 0)
                        {
                            if (text.GetLengthCount(lengthType) > maxLength)
                            {
                                return false;
                            }
                        }

                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 指定したメンバーの値を取得します。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="member">取得したい値</param>
        /// <returns>指定したメンバーの値</returns>
        public T GetMemberValue<T>(string member)
        {
            try
            {
                var obj = GetMemberValue(member);

                return (obj != null)
                    ? (T)obj
                    : (typeof(T) == typeof(string)) ? (T)(object)"" : default(T);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定したメンバーの値を取得します。
        /// </summary>
        /// <param name="member">取得したい値</param>
        /// <returns>指定したメンバーの値</returns>
        public object GetMemberValue(string member)
        {
            if (String.IsNullOrEmpty(member))
            {
                return null;
            }

            try
            {
                var dt = this.DataSource as DataTable;
                if (dt.IsNullOrEmpty())
                {
                    return null;
                }

                if (dt.Columns.IndexOf(member) == -1)
                {
                    return null;
                }

                var selectedIndex = this.FindStringExact(GetFormatText());
                if (selectedIndex == -1)
                {
                    return null;
                }

                return dt.Rows[selectedIndex].GetData(member);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 表示の値を取得します。
        /// </summary>
        /// <param name="valueMember">実際の値</param>
        /// <param name="displayMember">表示の値</param>
        /// <returns>表示の値</returns>
        public string GetDisplayMemberText(string valueMember = "value_member", string displayMember = "display_member")
        {
            if (String.IsNullOrEmpty(valueMember) || String.IsNullOrEmpty(displayMember))
            {
                return "";
            }

            var dt = this.DataSource as DataTable;
            if (dt.IsNullOrEmpty())
            {
                return null;
            }

            if (dt.Columns.IndexOf(valueMember) == -1 || dt.Columns.IndexOf(displayMember) == -1)
            {
                return "";
            }

            try
            {
                List<string> selectedName = dt.AsEnumerable()
                                                        .Where(r =>
                                                        {
                                                            var v = r.GetData(valueMember);

                                                            return (v != null && v.ToString() == this.Text);
                                                        })
                                                        .Select(r => r[displayMember].ToString())
                                                        .ToList<string>();

                return (selectedName.Count == 1) ? selectedName[0] : "";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 変更状態をクリアします。
        /// </summary>
        void IControlChanged.Clear()
        {
            _isChanged = false;
            _isTempChanged = false;
        }
        #endregion
    }
}
