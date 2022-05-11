using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MKClass;
using MKClass.MKFile;
using MKClass.MKData;
using MKClass.MKControl;
using MKDB2;

namespace ASViews
{
    using CClass = ASViewsClass;

    /// <summary>
    /// ASViews ASデータ参照クラス
    /// </summary>
    public partial class FormASViews_1 : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormASViews_1()
        {
            try
            {
                InitializeComponent();
                var commonClass = new CClass();

                this.DoubleBuffered = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                CSystem.GetAssemblyName<FormASViews_1>(this),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                throw new Exception();
            }
        }
        #endregion

        #region フォームイベント
        /// <summary>
        /// フォームが初めて表示される直前に発生します。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private void ASViews_1_Load(object sender, EventArgs e)
        {
            try
            {
                this.CustomizeForm();

                this.Text = CClass.PROGRAM_NAME;
                lblTitle.Text = CClass.PROGRAM_NAME;
                lblPgid.Text = CClass.ProgramId;

                SetPropertyToButtons();
                SetPropertyToDataGridViews();

                SetButtonEvents();
                CForm.SetControlEvent<CTextBox>(this, SetCTextBoxEvents);

                InitControls();

                this.ActiveControl = ctbLibNo;
                ctbLibNo.BackColor = ctbLibNo.ActiveBackColor;
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.Message);

                this.DialogResult = DialogResult.Abort;
                this.Close();

                return;
            }
        }
        #endregion

        #region コントロール プロパティ設定
        #region Button プロパティ設定
        /// <summary>
        /// ファンクションボタンの配列を取得します。
        /// </summary>
        /// <returns>ファンクションボタンの配列</returns>
        private List<CButton> GetFunctionButtons()
        {
            try
            {
                return new List<CButton>() {
                    cbtnF1,
                    cbtnF11,
                    cbtnF12
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Buttonのプロパティを設定します。
        /// </summary>
        private void SetPropertyToButtons()
        {
            try
            {
                //// ファンクションボタン
                foreach (CButton cbtn in GetFunctionButtons())
                {
                    if (cbtn == cbtnF1)
                    {
                        cbtn.Text = "F1:実行";
                        cbtn.Enabled = true;
                        cbtn.TabStop = true;
                    }
                    else if (cbtn == cbtnF11)
                    {
                        cbtn.Text = "F11:CSV";
                        cbtn.Enabled = true;
                        cbtn.TabStop = false;
                    }
                    else if (cbtn == cbtnF12)
                    {
                        cbtn.Text = "F12:閉じる";
                        cbtn.Enabled = true;
                        cbtn.TabStop = false;
                    }
                    else
                    {
                        cbtn.Text = null;
                        cbtn.Enabled = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DataGridView プロパティ設定
        /// <summary>
        /// DataGridViewのプロパティを設定します。
        /// </summary>
        private void SetPropertyToDataGridViews()
        {
            var cdgv = cdgvResult;

            try
            {
                cdgv.DataSource = null;

                cdgv.Columns.Clear();
                cdgv.Rows.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region イベントセット (ラムダ式)
        #region イベントセット (Button)
        /// <summary>
        /// Buttonのイベントをセットします。 (ラムダ式) 
        /// </summary>
        private void SetButtonEvents()
        {
            //// F1:実行
            cbtnF1.Click += (sender_, e_) => RunQuery();
            //// F11:CSV
            cbtnF11.Click += (sender_, e_) => ExportCSV();
            //// F12:閉じる
            cbtnF12.Click += (sender_, e_) => this.Close();
        }
        #endregion

        #region イベントセット (CTextBox)
        /// <summary>
        /// CTextBoxのイベントをセットします。 (ラムダ式)
        /// </summary>
        /// <param name="ctb">CTextBox</param>
        private void SetCTextBoxEvents(CTextBox ctb)
        {
            try
            {
                ctb.Validating += new CancelEventHandler(cTextBox_Validating);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールが検証しているときに発生します。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private void cTextBox_Validating(object sender, CancelEventArgs e)
        {
            var ctb = (CTextBox)sender;

            var message = "";
            try
            {
                if (this.ActiveControl == ctb || cbtnF12.Focused)
                {
                    e.Cancel = false;

                    return;
                }

                if (!((IControlChanged)ctb).IsTempChanged)
                {
                    e.Cancel = false;

                    return;
                }

                if (e.Cancel)
                {
                    switch (ctb.ValidationStatus)
                    {
                        case ControlValidationStatus.InValidValueError:
                        case ControlValidationStatus.OverflowError:
                            message = this.GetCaptionLabelText(ctb) + "に入力された値は正しくありません。";
                            CMessage.ShowErrorMessage(message, CClass.ProgramId);

                            break;
                        case ControlValidationStatus.RequiredError:
                            message = this.GetCaptionLabelText(ctb) + "は入力必須項目です。";
                            CMessage.ShowErrorMessage(message, CClass.ProgramId);

                            break;
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.Message);
            }
        }
        #endregion
        #endregion

        #region コントロール初期化
        /// <summary>
        /// コントロールの初期化を行います。
        /// </summary>
        /// <remarks>
        /// DataGridViewは初期化しません。
        /// </remarks>
        private void InitControls()
        {
            try
            {
                CForm.InitBackColor(this);

                CForm.ClearControl(this);
                CForm.ClearContentLabel(this);

                lblResultCount.Text = "";

                CForm.ClearControlChanged(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region バリデーション
        /// <summary>
        /// コントロールの検証を行います。
        /// </summary>
        /// <param name="messages">メッセージを格納</param>
        /// <returns>
        /// true : 正当
        /// false: それ以外
        /// </returns>
        private bool Validation(out string messages)
        {
            messages = "";
            try
            {
                var errorMessages = new StringBuilder();

                // 入力文字列チェック
                foreach (var c in CForm.GetValidationErrorContols(this))
                {
                    switch (CForm.ValidationStatus)
                    {
                        case ControlValidationStatus.InValidValueError:
                        case ControlValidationStatus.OverflowError:
                            errorMessages.AppendLine(this.GetCaptionLabelText(c) + "に入力された値は正しくありません。");
                            c.BackColor = ControlColor.Back.Error;

                            break;
                        case ControlValidationStatus.RequiredError:
                            errorMessages.AppendLine(this.GetCaptionLabelText(c) + "は入力必須項目です。");
                            c.BackColor = ControlColor.Back.Error;

                            break;
                    }
                }

                messages = errorMessages.ToString();

                return String.IsNullOrEmpty(messages);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region クエリ実行
        /// <summary>
        /// クエリを実行します。
        /// </summary>
        private void RunQuery()
        {
            var cdgv = cdgvResult;

            var message = "";
            try
            {
                if (!Validation(out message))
                {
                    CMessage.ShowErrorMessage(message, CClass.ProgramId);

                    return;
                }

                var q = new StringBuilder();

                for (int i = 0; i < ctbQuery.Lines.Length; i++)
                {
                    q.AppendLine(ctbQuery.Lines[i]);
                }

                using (var db2 = new DB2Controller(CConvert.ToInt(ctbLibNo.Text)))
                using (var dt = db2.Select(q.ToString()))
                {
                    cdgv.DataSource = dt;

                    //cdgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    lblResultCount.Text = cdgv.RowCount.ToString(CClass.COUNT_FORMAT) + " " + CClass.COUNT_UNIT;
                }
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region CSV出力
        /// <summary>
        /// CSV (UTF-8) 出力します。
        /// </summary>
        private void ExportCSV()
        {
            var cdgv = cdgvResult;

            try
            {
                using (DataTable dt = CConvert.ConvertToDataTable(cdgv))
                {
                    if (dt.IsNullOrEmpty())
                    {
                        CMessage.ShowErrorMessage("対象データが存在しません。");

                        return;
                    }

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = lblTitle.Text + "_" + DateTime.Today.ToString("yyyyMMdd") + ".csv";
                    sfd.Filter = "CSV (コンマ区切り) (*.csv)|*.csv";
                    sfd.Title = "名前を付けて保存";
                    sfd.RestoreDirectory = true;
                    sfd.OverwritePrompt = true;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        CCSV.ExportCSV(dt, sfd.FileName, true, Encoding.GetEncoding("utf-8"));

                        CMessage.ShowInformationMessage("'" + sfd.FileName + "' に出力しました。");
                    }
                }
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.Message);
            }
        }
        #endregion
    }
}
