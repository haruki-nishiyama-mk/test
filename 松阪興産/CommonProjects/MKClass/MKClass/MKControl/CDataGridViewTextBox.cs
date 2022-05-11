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
    #region CTextBoxColumnクラス
    /// <summary>
    /// DataGridViewTextBoxColumn拡張クラス
    /// </summary>
    public class CTextBoxColumn : DataGridViewTextBoxColumn, IDataGridViewEditingControlColumnEx
    {
        #region プロパティ (基底)
        /// <summary>
        /// 新しいセルの作成に使用するテンプレートを取得または設定します。
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CTextBoxCell)))
                {
                    throw new InvalidCastException("Must be a CTextBoxCell");
                }

                base.CellTemplate = value;
            }
        }
        #endregion

        #region プロパティ (実装)
        /// <summary>
        /// コントロールを識別するコードで使われる名前です。
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// コントロールのラベルを示します。
        /// </summary>
        public string ControlLabel { get; set; }
        #endregion

        #region プロパティ (追加)
        /// <summary>
        /// 標準の背景色です。
        /// </summary>
        public Color BaseBackColor { get; set; }

        /// <summary>
        /// 必須項目の背景色です。
        /// </summary>
        public Color RequiredBackColor { get; set; }

        /// <summary>
        /// アクティブ時の背景色です。
        /// </summary>
        public Color ActiveBackColor { get; set; }

        /// <summary>
        /// このコントロール内で許可する入力モードを決定します。
        /// </summary>
        public TextBoxInputMode InputMode { get; set; }

        /// <summary>
        /// 文字列を大文字、小文字に変換するかどうかを示します。
        /// </summary>
        public ControlConvertCase ConvertCase { get; set; }

        /// <summary>
        /// テキストをどのように配置するかを取得または設定します。
        /// HorizontalAlignment列挙型以外の指定はInputModeプロパティに応じて設定します。
        /// </summary>
        public object TextAlign { get; set; }

        /// <summary>
        /// 入力モードに応じた最大文字数を指定します。
        /// 数値型の場合、有効桁数となります。 (ハイフン(-)、ピリオド(.)、カンマ(,)は除きます。)
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// decimal型の場合、少数点以下の桁数を指定します。
        /// </summary>
        public int DecimalScale { get; set; }

        /// <summary>
        /// 文字長の種別を指定します。
        /// </summary>
        public LengthType LengthType { get; set; }

        /// <summary>
        /// 数値型の場合、
        /// OnLeaveイベントで3桁ごとにカンマで区切るかどうか、
        /// OnEnterイベントでそのカンマを削除するかどうかを示します。
        /// </summary>
        public bool NumericDelimiter { get; set; }

        /// <summary>
        /// Code型の場合、現在の文字列の先頭に指定されたUnicode文字でパディングします。
        /// MaxLengthが規定値の場合、パディングしません。
        /// </summary>
        public char PaddingChar { get; set; }

        /// <summary>
        /// 日付のフォーマットを指定します。
        /// </summary>
        public DateFormatType DateFormat { get; set; }

        /// <summary>
        /// 時間間隔のフォーマットを指定します。
        /// </summary>
        public TimeSpanFormatType TimeSpanFormat { get; set; }

        /// <summary>
        /// 必須かどうかを示します。
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Validatingイベントで必須チェックを行うかどうかを示します。
        /// </summary>
        public bool IsRequiredValidating { get; set; }

        /// <summary>
        /// ツールバーに使用するToolStripLabelを示します。
        /// </summary>
        public ToolStripLabel ToolStripLabel { get; set; }

        /// <summary>
        /// ToolStripLabelに関連付けられたテキストです。
        /// </summary>
        public string ToolStripLabelText { get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTextBoxColumn()
            : base()
        {
            this.CellTemplate = new CTextBoxCell();

            this.ControlName = "";
            this.ControlLabel = "";
            this.BaseBackColor = SystemColors.Window;
            this.RequiredBackColor = CColor.RequiredBackColor;
            this.ActiveBackColor = CColor.ActiveBackColor;
            this.InputMode = TextBoxInputMode.NoControl;
            this.ConvertCase = ControlConvertCase.NormalCase;
            this.TextAlign = null;
            this.MaxLength = 0;
            this.DecimalScale = 0;
            this.LengthType = LengthType.Char;
            this.NumericDelimiter = false;
            this.PaddingChar = '\0';
            this.DateFormat = DateFormatType.YYYYMMDD;
            this.TimeSpanFormat = TimeSpanFormatType.HMS;
            this.IsRequired = false;
            this.IsRequiredValidating = false;
            this.ToolStripLabel = null;
            this.ToolStripLabelText = "";
        }
        #endregion

        #region メソッド (基底)
        /// <summary>
        /// このバンドの同一コピーを作成します。
        /// </summary>
        /// <returns>複製された DataGridViewBand を表すオブジェクト</returns>
        /// <remarks>
        /// プロパティを追加しているため、Cloneメソッドをオーバーライドする必要があります。
        /// </remarks>
        public override object Clone()
        {
            var col = base.Clone() as CTextBoxColumn;
            if (col != null)
            {
                col.ControlName = this.ControlName;
                col.ControlLabel = this.ControlLabel;
                col.BaseBackColor = this.BaseBackColor;
                col.RequiredBackColor = this.RequiredBackColor;
                col.ActiveBackColor = this.ActiveBackColor;
                col.InputMode = this.InputMode;
                col.ConvertCase = this.ConvertCase;
                col.TextAlign = this.TextAlign;
                col.MaxLength = this.MaxLength;
                col.DecimalScale = this.DecimalScale;
                col.LengthType = this.LengthType;
                col.NumericDelimiter = this.NumericDelimiter;
                col.PaddingChar = this.PaddingChar;
                col.DateFormat = this.DateFormat;
                col.TimeSpanFormat = this.TimeSpanFormat;
                col.IsRequired = this.IsRequired;
                col.IsRequiredValidating = this.IsRequiredValidating;
                col.ToolStripLabel = this.ToolStripLabel;
                col.ToolStripLabelText = this.ToolStripLabelText;
            }

            return col;
        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="text">検証対象のテキスト</param>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        public ControlValidationStatus Validation(string text, bool strict = true, bool required = true)
        {
            var cell = this.CellTemplate as CTextBoxCell;
            if (cell != null)
            {
                cell.OwnerColumn = this;

                return cell.Validation(text, strict, required);
            }
            else
            {
                return ControlValidationStatus.Normal;
            }
        }
        #endregion
    }
    #endregion



    #region CTextBoxCellクラス
    /// <summary>
    /// DataGridViewTextBoxCell拡張クラス
    /// </summary>
    public class CTextBoxCell : DataGridViewTextBoxCell, IDataGridViewEditingControlCellEx
    {
        #region プロパティ (基底)
        /// <summary>
        /// セルのホストされる編集コントロールの型を取得します。
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(CTextBoxEditingControl);
            }
        }

        /// <summary>
        /// セル内の値のデータ型を取得または設定します。
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(object);
            }
        }

        /// <summary>
        /// 新しいレコードの行のセルに対する既定値を取得します。
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return base.DefaultNewRowValue;
            }
        }
        #endregion

        #region プロパティ (追加)
        /// <summary>
        /// 自身の列を取得または設定します。
        /// </summary>
        public CTextBoxColumn OwnerColumn { get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTextBoxCell()
            : base()
        {
            this.OwnerColumn = null;
        }
        #endregion

        #region メソッド (基底)
        /// <summary>
        /// このバンドの同一コピーを作成します。
        /// </summary>
        /// <returns>複製された DataGridViewBand を表すオブジェクト</returns>
        /// <remarks>
        /// プロパティを追加しているため、Cloneメソッドをオーバーライドする必要があります。
        /// </remarks>
        public override object Clone()
        {
            var cell = base.Clone() as CTextBoxCell;
            if (cell != null)
            {
                cell.OwnerColumn = this.OwnerColumn;
            }

            return cell;
        }

        /// <summary>
        /// ホストされる編集コントロールを追加して初期化します。
        /// </summary>
        /// <param name="rowIndex">セルの親行のインデックス</param>
        /// <param name="initialFormattedValue">コントロールに表示される初期値</param>
        /// <param name="dataGridViewCellStyle">ホストされるコントロールの外観を決定する DataGridViewCellStyle</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            var value = initialFormattedValue as string;
            if (value == null)
            {
                value = String.Empty;
            }

            var ctl = this.DataGridView.EditingControl as CTextBoxEditingControl;
            if (ctl != null)
            {
                ctl.InitCTextBoxEditingControl();

                ctl.OwnerCell = this;

                //// プロパティ
                var col = this.OwningColumn as CTextBoxColumn;
                if (col != null)
                {
                    ctl.Name = col.ControlName;
                    ctl.ControlLabel = col.ControlLabel;
                    ctl.BaseBackColor = col.BaseBackColor;
                    ctl.RequiredBackColor = col.RequiredBackColor;
                    ctl.ActiveBackColor = col.ActiveBackColor;
                    ctl.InputMode = col.InputMode;
                    ctl.ConvertCase = col.ConvertCase;
                    if (col.TextAlign != null && col.TextAlign.GetType() == typeof(HorizontalAlignment))
                    {
                        ctl.TextAlign = (HorizontalAlignment)col.TextAlign;
                    }
                    ctl.MaxLength = col.MaxLength;
                    ctl.DecimalScale = col.DecimalScale;
                    ctl.LengthType = col.LengthType;
                    ctl.NumericDelimiter = col.NumericDelimiter;
                    ctl.PaddingChar = col.PaddingChar;
                    ctl.DateFormat = col.DateFormat;
                    ctl.TimeSpanFormat = col.TimeSpanFormat;
                    ctl.IsRequired = col.IsRequired;
                    ctl.IsRequiredValidating = col.IsRequiredValidating;
                    ctl.ToolStripLabel = col.ToolStripLabel;
                    ctl.ToolStripLabelText = col.ToolStripLabelText;
                }

                ctl.Text = value;
            }
        }

        /// <summary>
        /// 表示用に書式設定された値を、実際のセル値に変換します。
        /// </summary>
        /// <param name="formattedValue">セルの表示値</param>
        /// <param name="cellStyle">DataGridViewCellStyle</param>
        /// <param name="formattedValueTypeConverter">表示値の型のTypeConverter</param>
        /// <param name="valueTypeConverter">セル値の型のTypeConverter</param>
        /// <returns>セル値</returns>
        public override object ParseFormattedValue(object formattedValue,
                                                   DataGridViewCellStyle cellStyle,
                                                   TypeConverter formattedValueTypeConverter,
                                                   TypeConverter valueTypeConverter)
        {
            if (!(formattedValue == null || Convert.IsDBNull(formattedValue) || String.IsNullOrEmpty(formattedValue.ToString())))
            {
                if (this.ColumnIndex >= 0)
                {
                    if (CSystem.NUMERIC_TYPES.Contains(this.DataGridView.Columns[this.ColumnIndex].ValueType))
                    {
                        formattedValue = formattedValue.ToString().Replace(",", "");
                    }
                }
            }

            return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }

        /// <summary>
        /// セルの値を設定します。
        /// </summary>
        /// <param name="rowIndex">セルの親行のインデックス</param>
        /// <param name="value">設定するセル値</param>
        /// <returns>
        /// true : 値を設定
        /// false: それ以外
        /// </returns>
        protected override bool SetValue(int rowIndex, object value)
        {
            if (value == null || Convert.IsDBNull(value) || String.IsNullOrEmpty(value.ToString()))
            {
                if (this.ColumnIndex >= 0)
                {
                    value = DBNull.Value;
                }
            }

            return base.SetValue(rowIndex, value);
        }
        #endregion

        #region メソッド (実装)
        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="text">検証対象のテキスト</param>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        public ControlValidationStatus Validation(string text, bool strict = true, bool required = true)
        {
            using (var ctl = new CTextBox())
            {
                //// プロパティ
                var col = this.OwnerColumn;
                if (col != null)
                {
                    ctl.InputMode = col.InputMode;
                    ctl.MaxLength = col.MaxLength;
                    ctl.DecimalScale = col.DecimalScale;
                    ctl.LengthType = col.LengthType;
                    ctl.DateFormat = col.DateFormat;
                    ctl.TimeSpanFormat = col.TimeSpanFormat;
                    ctl.IsRequired = col.IsRequired;

                    return ctl.Validation(text, strict, required);
                }
                else
                {
                    return ControlValidationStatus.Normal;
                }
            }
        }
        #endregion
    }
    #endregion



    #region CTextBoxEditingControlクラス
    /// <summary>
    /// DataGridViewEditingControl拡張クラス (CTextBox)
    /// </summary>
    [ToolboxItem(false)]
    public class CTextBoxEditingControl : CTextBox, IDataGridViewEditingControl, IDataGridViewEditingControlEx
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTextBoxEditingControl()
            : base()
        {
            this.TabStop = false;
        }
        #endregion

        #region プロパティ (実装)
        DataGridView _dataGridView;
        /// <summary>
        /// セルを格納するDataGridViewを取得または設定します。
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _dataGridView;
            }
            set
            {
                _dataGridView = value;
            }
        }

        /// <summary>
        /// コントロールの現在の値の書式設定された表現を取得または設定します。
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
            }
            set
            {
                string v = value as string;
                this.Text = (v != null) ? v : "";
            }
        }

        int _rowIndex;
        /// <summary>
        /// 所有しているセルの親行のインデックスを取得または設定します。
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }

        private bool _valueChanged = false;
        /// <summary>
        /// コントロールの現在の値が変更されたかどうかを示す値を取得または設定します。
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }

        /// <summary>
        /// 編集時に使用するカーソルを取得します。
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        /// <summary>
        /// 値が変更されるたびに、セルの内容の位置を変更する必要があるかどうかを示す値を取得します。
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// コントロールのラベルを示します。
        /// </summary>
        public string ControlLabel { get; set; }
        #endregion

        #region プロパティ (追加)
        /// <summary>
        /// 自身のセルを取得または設定します。
        /// </summary>
        public CTextBoxCell OwnerCell { get; set; }
        #endregion

        #region メソッド (実装)
        /// <summary>
        /// 指定されたセルスタイルと矛盾しないように、コントロールのユーザーインターフェースを変更します。
        /// </summary>
        /// <param name="dataGridViewCellStyle">UIのモデルとして使用する DataGridViewCellStyle</param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }

        /// <summary>
        /// 指定されたキーが、編集コントロールによって処理される通常の入力キーか、
        /// DataGridViewによって処理される特殊なキーであるかを確認します。
        /// </summary>
        /// <param name="keyData">押されたキー</param>
        /// <param name="dataGridViewWantsInputKey">押されたキーに処理させるかどうか</param>
        /// <returns>
        /// true : 指定されたキーが編集コントロールによって処理される通常の入力キー
        /// false: それ以外
        /// </returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        /// <summary>
        /// セルの書式設定された値を取得します。
        /// </summary>
        /// <param name="context">データエラーのコンテキストを表す DataGridViewDataErrorContexts 値のビットごとの組み合わせ</param>
        /// <returns>セルの内容の書式設定されたバージョンを表すオブジェクト</returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        /// <summary>
        /// 現在選択されているセルの編集を準備します。
        /// </summary>
        /// <param name="selectAll">セルの内容をすべて選択するかどうか</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            PrepareEditing(selectAll);
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。代わりにPrepareEditingControlForEditメソッドで実行します。
        /// </remarks>
        protected override void OnEnter(EventArgs e)
        {

        }

        /// <summary>
        /// マウスポインタによってコントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。
        /// </remarks>
        protected override void OnMouseEnter(EventArgs e)
        {

        }

        /// <summary>
        /// マウスポインタがコントロール上にある状態でマウスボタンが離されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。
        /// </remarks>
        protected override void OnMouseUp(MouseEventArgs e)
        {

        }

        /// <summary>
        /// Textプロパティの値が変化すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _valueChanged = true;
            _dataGridView.NotifyCurrentCellDirty(true);
        }

        /// <summary>
        /// 入力フォーカスがコントロールを離れると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。
        /// </remarks>
        protected override void OnLeave(EventArgs e)
        {

        }

        /// <summary>
        /// コントロールが検証しているときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。
        /// </remarks>
        protected override void OnValidating(CancelEventArgs e)
        {

        }

        /// <summary>
        /// コントロールの検証が終了すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// 呼び出し元の機能を無効にします。
        /// </remarks>
        protected override void OnValidated(EventArgs e)
        {

        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// InitCTextBoxEditingControlの初期化を行います。
        /// </summary>
        public void InitCTextBoxEditingControl()
        {
            this.OwnerCell = null;

            //this.Text = "";

            //this.Name = "";
            //this.BaseBackColor = SystemColors.Window;
            //this.RequiredBackColor = CColor.RequiredBackColor;
            //this.ActiveBackColor = CColor.ActiveBackColor;
            //this.InputMode = TextBoxInputMode.NoControl;
            //this.ConvertCase = ControlConvertCase.NormalCase;
            //this.TextAlign = HorizontalAlignment.Left;
            this.MaxLength = 0;
            this.DecimalScale = 0;
            //this.LengthType = LengthType.Char;
            //this.NumericDelimiter = false;
            //this.PaddingChar = '\0';
            //this.DateFormat = DateFormatType.YYYYMMDD;
            //this.TimeSpanFormat = TimeSpanFormatType.HMS;
            //this.IsRequired = false;
            //this.IsRequiredValidating = false;
            //this.ControlLabel = "";
            //this.ToolStripLabel = null;
            //this.ToolStripLabelText = "";
        }

        /// <summary>
        /// 背景色を設定します。
        /// </summary>
        /// <remarks>
        /// アクティブ時の背景色の設定は行いません。
        /// </remarks>
        public new void SetBackColor()
        {
            var backColor = (this.IsRequired) ? this.RequiredBackColor : this.BaseBackColor;

            base.BackColor = backColor;
            this.OwnerCell.Style.BackColor = backColor;
        }

        /// <summary>
        /// 編集を開始します。
        /// </summary>
        /// <param name="selectAll">セルの内容をすべて選択するかどうか</param>
        public void PrepareEditing(bool selectAll)
        {
            switch (this.InputMode)
            {
                case TextBoxInputMode.Int:
                case TextBoxInputMode.UInt:
                case TextBoxInputMode.Decimal:
                case TextBoxInputMode.UDecimal:
                    this.Text = this.Text.Replace(",", "");

                    break;
            }

            if (selectAll)
            {
                SelectAll();
            }
            else
            {
                this.Select(this.Text.Length, 0);
            }

            base.BackColor = this.ActiveBackColor;

            if (this.ToolStripLabel != null)
            {
                this.ToolStripLabel.Text = this.ToolStripLabelText;
            }
        }

        /// <summary>
        /// 編集を終了します。
        /// </summary>
        public void FinishEditing()
        {
            SetBackColor();

            this.OwnerCell.ErrorText = "";

            if (this.ToolStripLabel != null)
            {
                this.ToolStripLabel.Text = "";
            }
        }
        #endregion
    }
    #endregion
}
