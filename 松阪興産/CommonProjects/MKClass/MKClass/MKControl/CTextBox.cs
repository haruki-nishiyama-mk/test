using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using MKClass.MKData;

namespace MKClass.MKControl
{
    /// <summary>
    /// TextBox拡張クラス
    /// </summary>
    public class CTextBox : TextBox, IControlChanged
    {
        #region プロパティ (基底)
        /// <summary>
        /// コンポーネントの背景色です。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
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

        /// <summary>
        /// 単一行エディットコントロールで、フォントサイズを基に、サイズの自動調整を行います。
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
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
        public override int MaxLength
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

                if (!base.ReadOnly && !_isRequired)
                {
                    base.BackColor = _baseBackColor;
                }
            }
        }

        private Color _readOnlyBackColor = CColor.ReadOnlyBackColor;
        /// <summary>
        /// 読み取り専用項目の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("読み取り専用項目の背景色です。")]
        [DefaultValue(typeof(Color), "Info")]
        public Color ReadOnlyBackColor
        {
            get
            {
                return _readOnlyBackColor;
            }
            set
            {
                _readOnlyBackColor = value;

                if (base.ReadOnly)
                {
                    base.BackColor = _readOnlyBackColor;
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

                if (!base.ReadOnly && _isRequired)
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

        private TextBoxInputMode _inputMode = TextBoxInputMode.NoControl;
        /// <summary>
        /// このコントロール内で許可する入力モードを決定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("このコントロール内で許可する入力モードを決定します。")]
        [DefaultValue(TextBoxInputMode.NoControl)]
        public TextBoxInputMode InputMode
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
                    case TextBoxInputMode.Hiragana:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Hiragana;

                        break;
                    case TextBoxInputMode.Katakana:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Katakana;

                        break;
                    case TextBoxInputMode.KatakanaHalf:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.KatakanaHalf;

                        break;
                    case TextBoxInputMode.Alpha:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case TextBoxInputMode.Int:
                    case TextBoxInputMode.UInt:
                    case TextBoxInputMode.Decimal:
                    case TextBoxInputMode.UDecimal:
                        this.TextAlign = HorizontalAlignment.Right;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case TextBoxInputMode.Code:
                    case TextBoxInputMode.CodeSymbol:
                    case TextBoxInputMode.CodeNumOnly:
                    case TextBoxInputMode.CodeAlphaOnly:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case TextBoxInputMode.CodeKatakanaHalf:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.KatakanaHalf;

                        break;
                    case TextBoxInputMode.Date:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case TextBoxInputMode.TimeSpan:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    case TextBoxInputMode.PhoneNumber:
                    case TextBoxInputMode.PostalCode:
                        this.TextAlign = HorizontalAlignment.Left;
                        this.ImeMode = ImeMode.Disable;

                        break;
                    default:
                        this.TextAlign = HorizontalAlignment.Left;
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

                switch (_convertCase)
                {
                    case ControlConvertCase.LowerCase:
                        this.CharacterCasing = CharacterCasing.Lower;

                        break;
                    case ControlConvertCase.UpperCase:
                        this.CharacterCasing = CharacterCasing.Upper;

                        break;
                    default:
                        this.CharacterCasing = CharacterCasing.Normal;

                        break;
                }
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

        private DateFormatType _dateFormat = DateFormatType.YYYYMMDD;
        /// <summary>
        /// 日付のフォーマットを指定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("日付のフォーマットを指定します。")]
        [DefaultValue(DateFormatType.YYYYMMDD)]
        public DateFormatType DateFormat
        {
            get
            {
                return _dateFormat;
            }
            set
            {
                _dateFormat = value;
            }
        }

        private TimeSpanFormatType _timeSpanFormat = TimeSpanFormatType.HMS;
        /// <summary>
        /// 時間間隔のフォーマットを指定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("時間間隔のフォーマットを指定します。")]
        [DefaultValue(TimeSpanFormatType.HMS)]
        public TimeSpanFormatType TimeSpanFormat
        {
            get
            {
                return _timeSpanFormat;
            }
            set
            {
                _timeSpanFormat = value;
            }
        }

        private bool _selectAll = true;
        private bool _isSelectAll = true;
        /// <summary>
        /// テキストボックスの全てのテキストを選択するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("テキストボックスの全てのテキストを選択するかどうかを示します。")]
        [DefaultValue(true)]
        public bool IsSelectAll
        {
            get
            {
                return _isSelectAll;
            }
            set
            {
                _isSelectAll = value;
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

                base.BackColor = (base.ReadOnly) ? _readOnlyBackColor : ((_isRequired) ? _requiredBackColor : _baseBackColor);
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
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        public bool DoEvent
        {
            get
            {
                return _doEvent;
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
        public CTextBox()
            : base()
        {
            ////// プロパティ
            base.MaxLength = 0;
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            switch (_inputMode)
            {
                case TextBoxInputMode.Int:
                case TextBoxInputMode.UInt:
                case TextBoxInputMode.Decimal:
                case TextBoxInputMode.UDecimal:
                    this.Text = this.Text.Replace(",", "");

                    break;
            }

            if (_isSelectAll)
            {
                SelectAll();
            }
            else
            {
                this.Select(this.Text.Length, 0);
            }

            if (!this.ReadOnly)
            {
                base.BackColor = _activeBackColor;
            }

            if (_toolStripLabel != null)
            {
                _toolStripLabel.Text = _toolStripLabelText;
            }
        }

        /// <summary>
        /// マウスポインタによってコントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!this.Focused)
                _selectAll = true;
            else
                _selectAll = false;
        }

        /// <summary>
        /// マウスポインタがコントロール上にある状態でマウスボタンが離されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isSelectAll && _selectAll)
            {
                SelectAll();
            }

            _selectAll = false;
        }

        /// <summary>
        /// コントロールにフォーカスがあるときに、
        /// 文字、スペース、またはBackspaceキーが押された場合に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
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
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("Textプロパティの値がコントロールで変更される直前に発生します。")]
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
                                                            this.CreateChangedText(String.Empty, true));
                        this.OnTextChanging(e);
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
                                                            this.CreateChangedText(Convert.ToString((char)(m.WParam.ToInt32()))));
                        this.OnTextChanging(e);
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
                                                            this.CreateChangedText(clipText));
                        this.OnTextChanging(e);
                        if (e.Cancel)
                        {
                            // 入力された内容をキャンセルする
                            return;
                        }
                    }

                    break;
                case WM_IME_COMPOSITION:
                    // IMEから未確定文字列や確定された文字列がやってきたとき
                    _doEvent = false;

                    break;
                case WM_IME_ENDCOMPOSITION:
                    // IMEから未確定文字列がなくなったとき
                    _doEvent = true;

                    break;
            }

            base.WndProc(ref m);
        }

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
                if (this.SelectionLength == 0 && this.SelectionStart < this.TextLength)
                {
                    changedText = changedText.Remove(this.SelectionStart, 1);
                }

                return changedText;
            }

            if (!this.Multiline)
            {
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
                changedText = changedText.Insert(this.SelectionStart, inputText);
            }

            return changedText;
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

            _selectAll = true;
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

        /// <summary>
        /// ReadOnlyプロパティの値が変更されたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);

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
            if (!this.Enabled)
            {
                base.BackColor = CColor.DisableBackColor;
            }
            else if (this.ReadOnly)
            {
                base.BackColor = _readOnlyBackColor;
            }
            else
            {
                base.BackColor = (_isRequired) ? _requiredBackColor : _baseBackColor;
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
            if (!String.IsNullOrEmpty(text))
            {
                string validationText = text;
                switch (_inputMode)
                {
                    case TextBoxInputMode.Int:
                    case TextBoxInputMode.UInt:
                    case TextBoxInputMode.Decimal:
                    case TextBoxInputMode.UDecimal:
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
                    case TextBoxInputMode.Katakana:
                        text = CConvert.ConvertToKatakana(text);

                        break;
                    case TextBoxInputMode.KatakanaHalf:
                        text = CConvert.ConvertToKatakanaHalf(text);

                        break;
                    case TextBoxInputMode.Int:
                    case TextBoxInputMode.UInt:
                        text = CConvert.ToInt(text.Replace(",", "")).ToString(GetIntFormat());

                        break;
                    case TextBoxInputMode.Decimal:
                    case TextBoxInputMode.UDecimal:
                        text = CConvert.ToDecimal(text.Replace(",", "")).ToString(GetDecimalFormat());

                        break;
                    case TextBoxInputMode.Code:
                    case TextBoxInputMode.CodeSymbol:
                    case TextBoxInputMode.CodeNumOnly:
                    case TextBoxInputMode.CodeAlphaOnly:
                        text = GetCodeTypeFormatText(text);

                        break;
                    case TextBoxInputMode.CodeKatakanaHalf:
                        text = GetCodeTypeFormatText(CConvert.ConvertToKatakanaHalf(text));

                        break;
                    case TextBoxInputMode.Date:
                        text = CConvert.ComplementDate(text, _dateFormat);

                        break;
                    case TextBoxInputMode.TimeSpan:
                        text = CConvert.ComplementTimeSpan(text, _timeSpanFormat);

                        break;
                    case TextBoxInputMode.PostalCode:
                        text = CConvert.ComplementPostalCode(text);

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
        /// 日付にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>日付にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public string GetDateFormatText()
        {
            return GetDateFormatText(this.Text);
        }

        /// <summary>
        /// 日付にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>日付にフォーマット変換したテキスト</returns>
        public string GetDateFormatText(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            return CConvert.ComplementDate(text, _dateFormat);
        }

        /// <summary>
        /// 日付にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public void ToDateFormat()
        {
            this.Text = GetDateFormatText();
        }

        /// <summary>
        /// 日付にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        public void ToDateFormat(string text)
        {
            this.Text = GetDateFormatText(text);
        }

        /// <summary>
        /// 時間間隔にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>時間間隔にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public string GetTimeSpanFormatText()
        {
            return GetTimeSpanFormatText(this.Text);
        }

        /// <summary>
        /// 時間間隔にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>時間間隔にフォーマット変換したテキスト</returns>
        public string GetTimeSpanFormatText(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            return CConvert.ComplementTimeSpan(text, _timeSpanFormat);
        }

        /// <summary>
        /// 時間間隔にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public void ToTimeSpanFormat()
        {
            this.Text = GetTimeSpanFormatText();
        }

        /// <summary>
        /// 時間間隔にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        public void ToTimeSpanFormat(string text)
        {
            this.Text = GetTimeSpanFormatText(text);
        }

        /// <summary>
        /// 郵便番号にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <returns>郵便番号にフォーマット変換したテキスト</returns>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public string GetPostalCodeFormatText()
        {
            return GetPostalCodeFormatText(this.Text);
        }

        /// <summary>
        /// 郵便番号にフォーマット変換したテキストを取得します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        /// <returns>郵便番号にフォーマット変換したテキスト</returns>
        public string GetPostalCodeFormatText(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            return CConvert.ComplementPostalCode(text);
        }

        /// <summary>
        /// 郵便番号にフォーマット変換します。
        /// </summary>
        /// <remarks>
        /// Textプロパティを対象とします。
        /// </remarks>
        public void ToPostalCodeFormat()
        {
            this.Text = GetPostalCodeFormatText();
        }

        /// <summary>
        /// 郵便番号にフォーマット変換します。
        /// </summary>
        /// <param name="text">変換対象のテキスト</param>
        public void ToPostalCodeFormat(string text)
        {
            this.Text = GetPostalCodeFormatText(text);
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
                if (c == (char)Keys.Enter)
                {
                    if (this.Multiline)
                    {
                        if (!this.AcceptsReturn)
                        {
                            return false;
                        }
                    }
                }

                switch (_inputMode)
                {
                    case TextBoxInputMode.Katakana:
                        if (!CValidate.IsKatakanaMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.KatakanaHalf:
                        if (!CValidate.IsKatakanaHalfMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.Int:
                        if (!CValidate.IsIntMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.UInt:
                        if (!CValidate.IsUIntMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.Decimal:
                        if (!CValidate.IsDecimalMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.UDecimal:
                        if (!CValidate.IsUDecimalMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.Code:
                        if (!CValidate.IsCodeMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.CodeSymbol:
                        if (!CValidate.IsCodeSymbolMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.CodeNumOnly:
                        if (!CValidate.IsCodeNumOnlyMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.CodeAlphaOnly:
                        if (!CValidate.IsCodeAlphaOnlyMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.CodeKatakanaHalf:
                        if (!CValidate.IsCodeKatakanaHalf(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.Date:
                        if (!CValidate.IsDateMatch(c, _dateFormat))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.TimeSpan:
                        if (!CValidate.IsTimeSpanMatch(c, _timeSpanFormat))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.PhoneNumber:
                        if (!CValidate.IsPhoneNumberMatch(c))
                        {
                            return false;
                        }

                        break;
                    case TextBoxInputMode.PostalCode:
                        if (!CValidate.IsPostalCodeMatch(c))
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
                    case TextBoxInputMode.Int:
                    case TextBoxInputMode.UInt:
                    case TextBoxInputMode.Decimal:
                    case TextBoxInputMode.UDecimal:
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
                    case TextBoxInputMode.Katakana:
                        if (!CValidate.IsKatakanaMatch(text))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.KatakanaHalf:
                        if (!CValidate.IsKatakanaHalfMatch(text))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.Int:
                        if (!CValidate.IsIntMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.UInt:
                        if (!CValidate.IsUIntMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.Decimal:
                        if (!CValidate.IsDecimalMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.UDecimal:
                        if (!CValidate.IsUDecimalMatch(text.Replace(",", ""), strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.Code:
                        if (!CValidate.IsCodeMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.CodeSymbol:
                        if (!CValidate.IsCodeSymbolMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.CodeNumOnly:
                        if (!CValidate.IsCodeNumOnlyMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.CodeAlphaOnly:
                        if (!CValidate.IsCodeAlphaOnlyMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.CodeKatakanaHalf:
                        if (!CValidate.IsCodeKatakanaHalf(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.Date:
                        if (!CValidate.IsDateMatch(text, _dateFormat, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.TimeSpan:
                        if (!CValidate.IsTimeSpanMatch(text, _timeSpanFormat, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.PhoneNumber:
                        if (!CValidate.IsPhoneNumberMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
                    case TextBoxInputMode.PostalCode:
                        if (!CValidate.IsPostalCodeMatch(text, strict))
                        {
                            return ControlValidationStatus.InValidValueError;
                        }

                        break;
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

            string[] textArr = null;

            if (!String.IsNullOrEmpty(text))
            {
                switch (_inputMode)
                {
                    case TextBoxInputMode.Int:
                    case TextBoxInputMode.UInt:
                        text = text.Replace(",", "").Replace("-", "");

                        if (maxLength > 0)
                        {
                            if (text.GetLengthCount(lengthType) > maxLength)
                            {
                                return false;
                            }
                        }

                        break;
                    case TextBoxInputMode.Decimal:
                    case TextBoxInputMode.UDecimal:
                        text = text.Replace(",", "").Replace("-", "");

                        textArr = text.Split('.');

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
                    case TextBoxInputMode.Date:
                        text = text.Replace("/", "");

                        if (maxLength > 0)
                        {
                            if (text.GetLengthCount(lengthType) > maxLength)
                            {
                                return false;
                            }
                        }

                        break;
                    case TextBoxInputMode.TimeSpan:
                        textArr = text.Split(':');

                        if (maxLength > 0)
                        {
                            switch (_timeSpanFormat)
                            {
                                case TimeSpanFormatType.HMS:
                                    maxLength -= 4;     // 分数・秒数を考慮

                                    break;
                                case TimeSpanFormatType.HM:
                                    maxLength -= 2;     // 分数を考慮

                                    break;
                                case TimeSpanFormatType.MS:
                                    maxLength -= 2;     // 秒数を考慮

                                    break;
                            }

                            if (textArr[0].GetLengthCount(lengthType) > maxLength)
                            {
                                return false;
                            }
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
