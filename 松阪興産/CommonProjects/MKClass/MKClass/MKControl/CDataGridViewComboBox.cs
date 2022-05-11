using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using MKClass.MKData;

namespace MKClass.MKControl
{
    #region CComboBoxColumnクラス
    /// <summary>
    /// DataGridViewComboBoxColumn拡張クラス
    /// </summary>
    public class CComboBoxColumn : DataGridViewComboBoxColumn, IDataGridViewEditingControlColumnEx
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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CComboBoxCell)))
                {
                    throw new InvalidCastException("Must be a CComboBoxCell");
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
        /// スタイルを指定する値を取得または設定します。
        /// </summary>
        public ComboBoxStyle DropDownStyle { get; set; }

        /// <summary>
        /// ドロップダウンリスト (2列目) に表示するプロパティを示します。
        /// </summary>
        public string DisplayMember2 { get; set; }

        /// <summary>
        /// DropDownWidthプロパティを自動で設定するかどうかを示します。
        /// </summary>
        public bool AutoDropDownWidth { get; set; }

        /// <summary>
        /// このコントロール内で許可する入力モードを決定します。
        /// </summary>
        public ComboBoxInputMode InputMode { get; set; }

        /// <summary>
        /// 文字列を大文字、小文字に変換するかどうかを示します。
        /// </summary>
        public ControlConvertCase ConvertCase { get; set; }

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
        /// 必須かどうかを示します。
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Validatingイベントで必須チェックを行うかどうかを示します。
        /// </summary>
        public bool IsRequiredValidating { get; set; }

        /// <summary>
        /// 指定した文字列と厳密に一致する項目を検索するかどうかを示します。
        /// </summary>
        public bool IsFindStringExact { get; set; }

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
        public CComboBoxColumn()
            : base()
        {
            this.CellTemplate = new CComboBoxCell();

            this.ControlName = "";
            this.ControlLabel = "";
            this.BaseBackColor = SystemColors.Window;
            this.RequiredBackColor = CColor.RequiredBackColor;
            this.ActiveBackColor = CColor.ActiveBackColor;
            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.DisplayMember2 = "";
            this.AutoDropDownWidth = true;
            this.InputMode = ComboBoxInputMode.NoControl;
            this.ConvertCase = ControlConvertCase.NormalCase;
            this.MaxLength = 0;
            this.DecimalScale = 0;
            this.LengthType = LengthType.Char;
            this.NumericDelimiter = false;
            this.PaddingChar = '\0';
            this.IsRequired = false;
            this.IsRequiredValidating = false;
            this.IsFindStringExact = true;
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
            var col = base.Clone() as CComboBoxColumn;
            if (col != null)
            {
                col.ControlName = this.ControlName;
                col.ControlLabel = this.ControlLabel;
                col.BaseBackColor = this.BaseBackColor;
                col.RequiredBackColor = this.RequiredBackColor;
                col.ActiveBackColor = this.ActiveBackColor;
                col.DropDownStyle = this.DropDownStyle;
                col.DisplayMember2 = this.DisplayMember2;
                col.AutoDropDownWidth = this.AutoDropDownWidth;
                col.InputMode = this.InputMode;
                col.ConvertCase = this.ConvertCase;
                col.MaxLength = this.MaxLength;
                col.DecimalScale = this.DecimalScale;
                col.LengthType = this.LengthType;
                col.NumericDelimiter = this.NumericDelimiter;
                col.PaddingChar = this.PaddingChar;
                col.IsRequired = this.IsRequired;
                col.IsRequiredValidating = this.IsRequiredValidating;
                col.IsFindStringExact = this.IsFindStringExact;
                col.ToolStripLabel = this.ToolStripLabel;
                col.ToolStripLabelText = this.ToolStripLabelText;
            }

            return col;
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
            var cell = this.CellTemplate as CComboBoxCell;
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



    #region CComboBoxCellクラス
    /// <summary>
    /// DataGridViewComboBoxCell拡張クラス
    /// </summary>
    public class CComboBoxCell : DataGridViewComboBoxCell, IDataGridViewEditingControlCellEx
    {
        #region プロパティ (基底)
        /// <summary>
        /// セルのホストされる編集コントロールの型を取得します。
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(CComboBoxEditingControl);
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
        public CComboBoxColumn OwnerColumn { get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CComboBoxCell()
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
            var cell = base.Clone() as CComboBoxCell;
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

            var ctl = this.DataGridView.EditingControl as CComboBoxEditingControl;
            if (ctl != null)
            {
                ctl.InitCComboBoxEditingControl();

                ctl.OwnerCell = this;

                //// プロパティ
                var col = this.OwningColumn as CComboBoxColumn;
                if (col != null)
                {
                    ctl.Name = col.ControlName;
                    ctl.ControlLabel = col.ControlLabel;
                    ctl.BaseBackColor = col.BaseBackColor;
                    ctl.RequiredBackColor = col.RequiredBackColor;
                    ctl.ActiveBackColor = col.ActiveBackColor;
                    ctl.DropDownStyle = col.DropDownStyle;
                    ctl.DisplayMember = col.DisplayMember;
                    ctl.DisplayMember2 = col.DisplayMember2;
                    ctl.ValueMember = col.ValueMember;
                    ctl.DataSource = col.DataSource;
                    ctl.AutoDropDownWidth = col.AutoDropDownWidth;
                    ctl.InputMode = col.InputMode;
                    ctl.ConvertCase = col.ConvertCase;
                    ctl.MaxLength = col.MaxLength;
                    ctl.DecimalScale = col.DecimalScale;
                    ctl.LengthType = col.LengthType;
                    ctl.NumericDelimiter = col.NumericDelimiter;
                    ctl.PaddingChar = col.PaddingChar;
                    ctl.IsRequired = col.IsRequired;
                    ctl.IsRequiredValidating = col.IsRequiredValidating;
                    ctl.IsFindStringExact = col.IsFindStringExact;
                    ctl.ToolStripLabel = col.ToolStripLabel;
                    ctl.ToolStripLabelText = col.ToolStripLabelText;

                    // 列のドロップダウンリストの幅を調整 (これをしないと反映されない)
                    col.DropDownWidth = ctl.GetAutoDropDownWidth();
                }

                ctl.Text = value;
                if (String.IsNullOrEmpty(value) || ctl.Items.Count > 0 && ctl.FindStringExact(ctl.GetFormatText()) == -1)
                {
                    ctl.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// セルのデータの書式指定済みの値を取得します。
        /// </summary>
        /// <param name="value">書式指定される値</param>
        /// <param name="rowIndex">セルの親行のインデックス</param>
        /// <param name="cellStyle">セルに反映される DataGridViewCellStyle</param>
        /// <param name="valueTypeConverter">書式指定済みの値の型へカスタムの変換を実行する、元の値の型に関連付けられた TypeConverter</param>
        /// <param name="formattedValueTypeConverter">書式指定済みの値の型からカスタムの変換を実行する、その値の型に関連付けられた TypeConverter</param>
        /// <param name="context">書式指定済みの値が必要とされているコンテキストを示す DataGridViewDataErrorContexts 値のビットごとの組み合わせ</param>
        /// <returns>書式指定適用後のセルのデータの値</returns>
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            var col = this.OwningColumn as CComboBoxColumn;
            if (col != null)
            {
                var dt = col.DataSource as DataTable;
                if (!dt.IsNullOrEmpty() && dt.Columns.Contains(col.ValueMember))
                {
                    var dr = dt.AsEnumerable()
                        .Where(r => r[col.ValueMember].ToString() == value.ToString())
                        .Select(x => x);
                    if (dr.IsNullOrEmpty())
                    {
                        value = DBNull.Value;
                    }
                }
                else
                {
                    value = DBNull.Value;
                }
            }

            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
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
            using (var ctl = new CComboBox())
            {
                //// プロパティ
                var col = this.OwnerColumn;
                if (col != null)
                {
                    ctl.DisplayMember = col.DisplayMember;
                    ctl.ValueMember = col.ValueMember;
                    var bindingSource = new BindingSource();
                    bindingSource.DataSource = col.DataSource;
                    ctl.BindingContext = new BindingContext();
                    ctl.DataSource = bindingSource.DataSource;
                    ctl.InputMode = col.InputMode;
                    ctl.MaxLength = col.MaxLength;
                    ctl.DecimalScale = col.DecimalScale;
                    ctl.LengthType = col.LengthType;
                    ctl.IsRequired = col.IsRequired;
                    ctl.IsFindStringExact = col.IsFindStringExact;

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



    #region CComboBoxEditingControlクラス
    /// <summary>
    /// DataGridViewEditingControl拡張クラス (CComboBox)
    /// </summary>
    [ToolboxItem(false)]
    public class CComboBoxEditingControl : CComboBox, IDataGridViewEditingControl, IDataGridViewEditingControlEx
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CComboBoxEditingControl()
            : base()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.IntegralHeight = false;
            this.IsFindStringExact = true;
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
                if (v != null)
                {
                    this.Text = v;
                    if (String.Compare(v, this.Text, true, CultureInfo.CurrentCulture) != 0)
                    {
                        this.Text = "";
                        this.SelectedIndex = -1;
                    }
                }
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
        public CComboBoxCell OwnerCell { get; set; }
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
            if ((keyData & Keys.KeyCode) == Keys.Down ||
                (keyData & Keys.KeyCode) == Keys.Up ||
                (keyData & Keys.KeyCode) == Keys.Left ||
                (keyData & Keys.KeyCode) == Keys.Right ||
                (keyData & Keys.KeyCode) == Keys.Home ||
                (keyData & Keys.KeyCode) == Keys.End ||
                (keyData & Keys.KeyCode) == Keys.PageDown ||
                (keyData & Keys.KeyCode) == Keys.PageUp ||
                (this.DroppedDown && ((keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Enter))
            {
                return true;
            }

            return !dataGridViewWantsInputKey;
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
        /// ComboBoxのドロップダウン部分が表示されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        /// <remarks>
        /// DataGridViewComboBoxCell.ComboBox_DropDownイベントを無効化する必要があります。
        /// </remarks>
        protected override void OnDropDown(EventArgs e)
        {

        }

        /// <summary>
        /// SelectedIndexプロパティが変更された場合に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

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
        /// CComboBoxEditingControlの初期化を行います。
        /// </summary>
        public void InitCComboBoxEditingControl()
        {
            this.OwnerCell = null;

            //this.Text = "";
            //this.SelectedIndex = -1;

            //this.Name = "";
            //this.BaseBackColor = SystemColors.Window;
            //this.RequiredBackColor = CColor.RequiredBackColor;
            //this.ActiveBackColor = CColor.ActiveBackColor;
            //this.DropDownStyle = ComboBoxStyle.DropDown;
            //this.DisplayMember = "";
            //this.DisplayMember2 = "";
            //this.ValueMember = "";
            //this.DataSource = null;
            //this.AutoDropDownWidth = true;
            //this.InputMode = ComboBoxInputMode.NoControl;
            //this.ConvertCase = ControlConvertCase.NormalCase;
            this.MaxLength = 0;
            this.DecimalScale = 0;
            //this.LengthType = LengthType.Char;
            //this.NumericDelimiter = false;
            //this.PaddingChar = '\0';
            //this.IsRequired = false;
            //this.IsRequiredValidating = false;
            //this.IsFindStringExact = true;
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
                case ComboBoxInputMode.Int:
                case ComboBoxInputMode.UInt:
                case ComboBoxInputMode.Decimal:
                case ComboBoxInputMode.UDecimal:
                    this.Text = this.Text.Replace(",", "");

                    break;
            }

            if (selectAll)
            {
                SelectAll();
            }

            base.BackColor = this.ActiveBackColor;

            if (this.ToolStripLabel != null)
            {
                this.ToolStripLabel.Text = this.ToolStripLabelText;
            }

            base.SetDropDownWidth();
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
