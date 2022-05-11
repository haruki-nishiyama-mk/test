using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MKClass.MKData;

namespace MKClass.MKControl
{
    #region DataGridView拡張クラス
    /// <summary>
    /// DataGridView拡張クラス
    /// </summary>
    public class CDataGridView : DataGridView
    {
        #region プロパティ (基底)
        /// <summary>
        /// 行を追加するオプションがユーザーに表示されるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool AllowUserToAddRows
        {
            get
            {
                return base.AllowUserToAddRows;
            }
            set
            {
                base.AllowUserToAddRows = value;
            }
        }

        /// <summary>
        /// ユーザーがDataGridViewからの行の削除を許可されているかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool AllowUserToDeleteRows
        {
            get
            {
                return base.AllowUserToDeleteRows;
            }
            set
            {
                base.AllowUserToDeleteRows = value;
            }
        }

        /// <summary>
        /// ユーザーが行のサイズを変更できるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool AllowUserToResizeRows
        {
            get
            {
                return base.AllowUserToResizeRows;
            }
            set
            {
                base.AllowUserToResizeRows = value;
            }
        }

        /// <summary>
        /// 既定の列ヘッダーのスタイルを取得または設定します。
        /// </summary>
        public new DataGridViewCellStyle ColumnHeadersDefaultCellStyle
        {
            get
            {
                return base.ColumnHeadersDefaultCellStyle;
            }
            set
            {
                base.ColumnHeadersDefaultCellStyle = value;
            }
        }

        /// <summary>
        /// ちらつきを軽減または回避するために、
        /// 2次バッファーを使用してコントロールの表面を再描画するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(true)]
        protected override bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                base.DoubleBuffered = value;
            }
        }

        /// <summary>
        /// ユーザーがDataGridViewの複数のセル、行、または列を同時に選択できるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool MultiSelect
        {
            get
            {
                return base.MultiSelect;
            }
            set
            {
                base.MultiSelect = value;
            }
        }

        /// <summary>
        /// DataGridViewのセルを選択できるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(DataGridViewSelectionMode.FullRowSelect)]
        public new DataGridViewSelectionMode SelectionMode
        {
            get
            {
                return base.SelectionMode;
            }
            set
            {
                base.SelectionMode = value;
            }
        }

        /// <summary>
        /// マウスポインターがセル上で一時停止したとき、
        /// またはユーザーがキーボードを使用してセルに移動したときに、
        /// ヒントを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool ShowCellToolTips
        {
            get
            {
                return base.ShowCellToolTips;
            }
            set
            {
                base.ShowCellToolTips = value;
            }
        }

        /// <summary>
        /// ユーザーがDataGridViewコントロールのセルを編集できるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(true)]
        public new bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        /// <summary>
        /// 行ヘッダーを格納している列が表示されるかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        public new bool RowHeadersVisible
        {
            get
            {
                return base.RowHeadersVisible;
            }
            set
            {
                base.RowHeadersVisible = value;
            }
        }

        /// <summary>
        /// 現在アクティブなセルを取得または設定します。
        /// </summary>
        [Browsable(false)]
        public new DataGridViewCell CurrentCell
        {
            get
            {
                return base.CurrentCell;
            }
            set
            {
                base.CurrentCell = value;

                if (base.CurrentCell == null)
                {
                    base.ClearSelection();
                }
            }
        }
        #endregion

        #region プロパティ (追加)
        private bool _changeEvenRowsBackColor = true;
        /// <summary>
        /// 偶数行の背景色を変更するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("偶数行の背景色を変更するかどうかを示します。")]
        [DefaultValue(true)]
        public bool ChangeEvenRowsBackColor
        {
            get
            {
                return _changeEvenRowsBackColor;
            }
            set
            {
                _changeEvenRowsBackColor = value;
            }
        }

        private Color _evenRowsBackColor = Color.AliceBlue;
        /// <summary>
        /// 偶数行の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("偶数行の背景色です。")]
        [DefaultValue(typeof(Color), "AliceBlue")]
        public Color EvenRowsBackColor
        {
            get
            {
                return _evenRowsBackColor;
            }
            set
            {
                _evenRowsBackColor = value;
            }
        }

        private DataGridViewActionPattern _actionOnEnter = DataGridViewActionPattern.DoubleClick;
        /// <summary>
        /// グリッド内でEnterを捕捉した際の挙動を示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("グリッド内でEnterを捕捉した際の挙動を示します。")]
        [DefaultValue(DataGridViewActionPattern.DoubleClick)]
        public DataGridViewActionPattern ActionOnEnter
        {
            get
            {
                return _actionOnEnter;
            }
            set
            {
                _actionOnEnter = value;
            }
        }

        private bool _moveToReadOnlyCell = false;
        /// <summary>
        /// 読み取り専用セルに移動することを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("読み取り専用セルに移動することを示します。")]
        [DefaultValue(false)]
        public bool MoveToReadOnlyCell
        {
            get
            {
                return _moveToReadOnlyCell;
            }
            set
            {
                _moveToReadOnlyCell = value;
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
        public CDataGridView()
            : base()
        {
            ////// プロパティ      
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            this.DoubleBuffered = true;
            this.MultiSelect = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ShowCellToolTips = false;
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
        }
        #endregion

        #region イベント
        /// <summary>
        /// コマンドキーを処理します。
        /// </summary>
        /// <param name="msg">処理するウィンドウメッセージを表す、参照渡しされたMessage</param>
        /// <param name="keyData">処理するキーを表すKeys値の1つ</param>
        /// <returns>
        /// true : 文字がコントロールによって処理された場合
        /// false: それ以外
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys keyCode = (keyData & Keys.KeyCode);
            Keys keyModifiers = (keyData & Keys.Modifiers);

            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            bool forward = true;
            bool nextControl = true;

            switch (msg.Msg)
            {
                case WM_SYSKEYDOWN:
                    if (keyModifiers == Keys.Alt && keyCode == Keys.F4)
                    {
                        // Alt + F4キーは有効
                    }
                    else
                    {
                        return true;
                    }

                    break;
                case WM_KEYDOWN:
                    switch (keyCode)
                    {
                        case Keys.Tab:
                            if (keyModifiers == Keys.None || keyModifiers == Keys.Shift)
                            {
                                forward = (keyModifiers != Keys.Shift);
                                switch (this.SelectionMode)
                                {
                                    case DataGridViewSelectionMode.FullRowSelect:
                                        SelectNextRow(forward, nextControl);

                                        break;
                                    case DataGridViewSelectionMode.FullColumnSelect:
                                        SelectNextColumn(forward, nextControl);

                                        break;
                                    default:
                                        SelectNextCell(forward, nextControl);

                                        break;
                                }

                                return true;
                            }

                            break;
                        case Keys.Enter:
                            switch (_actionOnEnter)
                            {
                                case DataGridViewActionPattern.DoubleClick:
                                    if (keyModifiers == Keys.None)
                                    {
                                        if (this.CurrentCell != null && this.ColumnCount > 0 && this.RowCount > 0)
                                        {
                                            this.OnCellDoubleClick(new DataGridViewCellEventArgs(this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex));
                                        }

                                        return true;
                                    }

                                    break;
                                case DataGridViewActionPattern.MoveFocus:
                                    if (keyModifiers == Keys.None || keyModifiers == Keys.Shift)
                                    {
                                        forward = (keyModifiers != Keys.Shift);
                                        switch (this.SelectionMode)
                                        {
                                            case DataGridViewSelectionMode.FullRowSelect:
                                                SelectNextRow(forward, nextControl);

                                                break;
                                            case DataGridViewSelectionMode.FullColumnSelect:
                                                SelectNextColumn(forward, nextControl);

                                                break;
                                            default:
                                                SelectNextCell(forward, nextControl);

                                                break;
                                        }

                                        return true;
                                    }

                                    break;
                            }

                            break;
                    }

                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// コントロールにフォーカスがあるときにキーが押されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // セルを移動しない
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// セルの内容が、表示用に書式指定されなければならないときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if (this.RowCount > 0)
            {
                if (_changeEvenRowsBackColor)
                {
                    e.CellStyle.BackColor = (e.RowIndex % 2 == 0) ? e.CellStyle.BackColor : _evenRowsBackColor;
                }
            }

            base.OnCellFormatting(e);
        }

        /// <summary>
        /// セルが入力フォーカスを失くし、現在のセルでなくなったときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == this.NewRowIndex)
            {
                return;
            }

            var ctb = this.EditingControl as CTextBoxEditingControl;
            var ccmb = this.EditingControl as CComboBoxEditingControl;

            if (ctb != null)
            {
                ctb.SetFormatText();
            }
            else if (ccmb != null)
            {
                ccmb.SetFormatText();
            }

            base.OnCellLeave(e);
        }

        /// <summary>
        /// セルが入力フォーカスを失い、内容の検証が有効になった場合に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == this.NewRowIndex)
            {
                return;
            }

            var ctb = this.EditingControl as CTextBoxEditingControl;
            var ccmb = this.EditingControl as CComboBoxEditingControl;

            if (ctb != null)
            {
                e.Cancel = ((_validationStatus = ctb.Validation(true, ctb.IsRequiredValidating)) != ControlValidationStatus.Normal);
            }
            else if (ccmb != null)
            {
                e.Cancel = ((_validationStatus = ccmb.Validation(true, ccmb.IsRequiredValidating)) != ControlValidationStatus.Normal);
            }

            base.OnCellValidating(e);
        }

        /// <summary>
        /// セルの検証が終了した後に発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == this.NewRowIndex)
            {
                return;
            }

            base.OnCellValidated(e);

            var ctb = this.EditingControl as CTextBoxEditingControl;
            var ccmb = this.EditingControl as CComboBoxEditingControl;

            if (ctb != null)
            {
                ctb.FinishEditing();
            }
            else if (ccmb != null)
            {
                ccmb.FinishEditing();
            }

            _validationStatus = ControlValidationStatus.Normal;
        }

        /// <summary>
        /// 外部のデータ解析または検証操作が例外をスローした場合、
        /// またはデータソースへのデータのコミットが失敗した場合に発生します。
        /// </summary>
        /// <param name="displayErrorDialogIfNoHandler">DataErrorイベントのハンドラーがないときにエラーダイアログボックスを表示するかどうか</param>
        /// <param name="e">イベントデータ</param>
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(String.Format("({0}, {1}) のセルでエラーが発生しました。", e.ColumnIndex, e.RowIndex));
                sb.AppendLine(String.Format("説明: {0}", e.Exception.Message));

                throw new Exception(sb.ToString());
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// 現在の位置から次、または前のセルをアクティブなセルに設定します。
        /// </summary>
        /// <param name="forward">移動の向き [true : 次のセル] [false : 前のセル]</param>
        /// <param name="nextControl">次、または前のコントロールに移動するかどうか</param>
        public void SelectNextCell(bool forward, bool nextControl = true)
        {
            if (this.CurrentCell == null)
            {
                return;
            }

            try
            {
                if (this.ColumnCount > 0 && this.RowCount > 0)
                {
                    int columnIndex = -1;
                    int rowIndex = -1;

                    if (forward)
                    {
                        columnIndex = this.CurrentCell.ColumnIndex + 1;
                        rowIndex = this.CurrentCell.RowIndex;

                        for (int row = rowIndex; row < this.RowCount; row++)
                        {
                            if (!this.Rows[row].Visible)
                            {
                                continue;
                            }

                            for (int col = columnIndex; col < this.ColumnCount; col++)
                            {
                                if (!this.Columns[col].Visible)
                                {
                                    continue;
                                }

                                if (!_moveToReadOnlyCell && this[col, row].ReadOnly)
                                {
                                    continue;
                                }

                                this.CurrentCell = this[col, row];

                                return;
                            }

                            columnIndex = 0;
                        }
                    }
                    else
                    {
                        columnIndex = this.CurrentCell.ColumnIndex - 1;
                        rowIndex = this.CurrentCell.RowIndex;

                        for (int row = rowIndex; row >= 0; row--)
                        {
                            if (!this.Rows[row].Visible)
                            {
                                continue;
                            }

                            for (int col = columnIndex; col >= 0; col--)
                            {
                                if (!this.Columns[col].Visible)
                                {
                                    continue;
                                }

                                if (!_moveToReadOnlyCell && this[col, row].ReadOnly)
                                {
                                    continue;
                                }

                                this.CurrentCell = this[col, row];

                                return;
                            }

                            columnIndex = this.ColumnCount - 1;
                        }
                    }
                }

                if (nextControl)
                {
                    CForm.GetForm(this).SelectNextControl(CForm.GetForm(this).ActiveControl, forward, true, true, false);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 現在の位置から次、または前の行をアクティブなセルに設定します。
        /// </summary>
        /// <param name="forward">移動の向き [true : 次の行] [false : 前の行]</param>
        /// <param name="nextControl">次、または前のコントロールに移動するかどうか</param>
        /// <remarks>
        /// MoveToReadOnlyCellプロパティは無視します。
        /// </remarks>
        public void SelectNextRow(bool forward, bool nextControl = true)
        {
            if (this.CurrentCell == null)
            {
                return;
            }

            try
            {
                if (this.ColumnCount > 0 && this.RowCount > 0)
                {
                    int columnIndex = -1;
                    int rowIndex = -1;

                    if (forward)
                    {
                        columnIndex = this.CurrentCell.ColumnIndex;
                        rowIndex = this.CurrentCell.RowIndex + 1;

                        for (int row = rowIndex; row < this.RowCount; row++)
                        {
                            if (!this.Rows[row].Visible)
                            {
                                continue;
                            }

                            this.CurrentCell = this[columnIndex, row];

                            return;
                        }
                    }
                    else
                    {
                        columnIndex = this.CurrentCell.ColumnIndex;
                        rowIndex = this.CurrentCell.RowIndex - 1;

                        for (int row = rowIndex; row >= 0; row--)
                        {
                            if (!this.Rows[row].Visible)
                            {
                                continue;
                            }

                            this.CurrentCell = this[columnIndex, row];

                            return;
                        }
                    }
                }

                if (nextControl)
                {
                    CForm.GetForm(this).SelectNextControl(CForm.GetForm(this).ActiveControl, forward, true, true, false);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 現在の位置から次、または前の列をアクティブなセルに設定します。
        /// </summary>
        /// <param name="forward">移動の向き [true : 次の列] [false : 前の列]</param>
        /// <param name="nextControl">次、または前のコントロールに移動するかどうか</param>
        /// <remarks>
        /// MoveToReadOnlyCellプロパティは無視します。
        /// </remarks>
        public void SelectNextColumn(bool forward, bool nextControl = true)
        {
            if (this.CurrentCell == null)
            {
                return;
            }

            try
            {
                if (this.ColumnCount > 0 && this.RowCount > 0)
                {
                    int columnIndex = -1;
                    int rowIndex = -1;

                    if (forward)
                    {
                        columnIndex = this.CurrentCell.ColumnIndex + 1;
                        rowIndex = this.CurrentCell.RowIndex;

                        for (int col = columnIndex; col < this.ColumnCount; col++)
                        {
                            if (!this.Columns[col].Visible)
                            {
                                continue;
                            }

                            this.CurrentCell = this[col, rowIndex];

                            return;
                        }
                    }
                    else
                    {
                        columnIndex = this.CurrentCell.ColumnIndex - 1;
                        rowIndex = this.CurrentCell.RowIndex;

                        for (int col = columnIndex; col >= 0; col--)
                        {
                            if (!this.Columns[col].Visible)
                            {
                                continue;
                            }

                            this.CurrentCell = this[col, rowIndex];

                            return;
                        }
                    }
                }

                if (nextControl)
                {
                    CForm.GetForm(this).SelectNextControl(CForm.GetForm(this).ActiveControl, forward, true, true, false);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// アクティブなセルを安全に設定します。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public bool CurrentCellSafely(string columnName, int rowIndex)
        {
            try
            {
                this.CurrentCell = this[columnName, rowIndex];
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// アクティブなセルを安全に設定します。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public bool CurrentCellSafely(int columnIndex, int rowIndex)
        {
            try
            {
                this.CurrentCell = this[columnIndex, rowIndex];
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 列名から列インデックスを取得します。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>列インデックス</returns>
        public int GetColumnIndex(string columnName)
        {
            try
            {
                for (int col = 0; col <= this.ColumnCount - 1; col++)
                {
                    if (columnName == this.Columns[col].Name)
                    {
                        return col;
                    }
                }
            }
            catch
            {

            }

            return -1;
        }

        /// <summary>
        /// 行に変更があったかどうかをチェックします。
        /// </summary>
        /// <returns>
        /// true : 変更有り
        /// false: それ以外
        /// </returns>
        public bool ModifiedRowData()
        {
            try
            {
                var dt = this.DataSource as DataTable;
                if (dt.IsNullOrEmpty())
                {
                    return false;
                }

                return !dt.GetChanges().IsNullOrEmpty();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
    #endregion



    #region セル内にホストされるコントロールの共通機能拡張インターフェース
    /// <summary>
    /// セル内にホストされるコントロールの共通機能拡張インターフェース (列に対する定義)
    /// </summary>
    public interface IDataGridViewEditingControlColumnEx
    {
        /// <summary>
        /// コントロールを識別するコードで使われる名前です。
        /// </summary>
        string ControlName { get; set; }

        /// <summary>
        /// コントロールのラベルを示します。
        /// </summary>
        string ControlLabel { get; set; }

        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="text">検証対象のテキスト</param>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        ControlValidationStatus Validation(string text, bool strict = true, bool required = true);
    }

    /// <summary>
    /// セル内にホストされるコントロールの共通機能拡張インターフェース (セルに対する定義)
    /// </summary>
    public interface IDataGridViewEditingControlCellEx
    {
        /// <summary>
        /// 入力テキストの検証を行います。
        /// </summary>
        /// <param name="text">検証対象のテキスト</param>
        /// <param name="strict">項目を厳格にチェックするかどうか</param>
        /// <param name="required">必須項目をチェックするかどうか</param>
        /// <returns>バリデーションステータス</returns>
        ControlValidationStatus Validation(string text, bool strict = true, bool required = true);
    }

    /// <summary>
    /// セル内にホストされるコントロールの共通機能拡張インターフェース
    /// </summary>
    public interface IDataGridViewEditingControlEx
    {
        /// <summary>
        /// コントロールのラベルを示します。
        /// </summary>
        string ControlLabel { get; set; }
    }
    #endregion
}
