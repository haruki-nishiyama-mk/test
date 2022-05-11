using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MKClass;
using MKClass.MKControl;
using MKODDS;

namespace MKODDS.Setup
{
    using CClass = ODDSSetupLibrary;

    /// <summary>
    /// ODDSetup メインクラス
    /// </summary>
    public partial class FormODDSSetup_1 : BaseForm
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormODDSSetup_1()
        {
            try
            {
                InitializeComponent();
                var commonClass = new CClass();
            }
            catch (Exception ex)
            {
                if (CClass.CLogObj != null)
                {
                    CClass.CLogObj.OutputMessage(ex);
                }
                else
                {
                    MessageBox.Show(ex.Message,
                                    CSystem.GetAssemblyName<FormODDSSetup_1>(this),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

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
        private void FormSetup_1_Load(object sender, EventArgs e)
        {
            try
            {
                this.CustomizeForm();

                this.Text = CClass.PROGRAM_NAME;

                SetPropertyToGroupBoxes();
                SetPropertyToButtons();

                SetButtonEvents();

                this.ActiveControl = cbtnESC;
            }
            catch (Exception ex)
            {
                CClass.CLogObj.OutputMessage(ex);

                Environment.Exit(-1);
            }
        }

        private void FormODDSSetup_1_Shown(object sender, EventArgs e)
        {
            try
            {
                this.SetActiveWindow();
            }
            catch (Exception ex)
            {
                CClass.CLogObj.OutputMessage(ex);

                Environment.Exit(-1);
            }
        }
        #endregion

        #region コントロール プロパティ設定
        #region GroupBox プロパティ設定
        /// <summary>
        /// GroupBoxのプロパティを設定します。
        /// </summary>
        private void SetPropertyToGroupBoxes()
        {
            try
            {
                gbHB.Enabled = OHKENProductsExt.ValidateOhkenProdInstalled(OHKENProducts.HB);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Button プロパティ設定
        /// <summary>
        /// テーブル設定ボタンの配列を返します。
        /// </summary>
        /// <returns>テーブル設定ボタンの配列</returns>
        private CButton[] GetSetTablesButtons()
        {
            try
            {
                CButton[] cbtn = {
                                    cbtnHBSetTables
                                 };
                return cbtn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ERP拡張スクリプトの設定ボタンの配列を返します。
        /// </summary>
        /// <returns>ERP拡張スクリプトの設定ボタンの配列</returns>
        private CButton[] GetSetERPScriptButtons()
        {
            try
            {
                CButton[] cbtn = {
                                    cbtnHBSetERPScript
                                 };
                return cbtn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ERPメニューの設定ボタンの配列を返します。
        /// </summary>
        /// <returns>ERPメニューの設定ボタンの配列</returns>
        private CButton[] GetSetERPMenuButtons()
        {
            try
            {
                CButton[] cbtn = {
                                    cbtnHBSetERPMenu
                                 };
                return cbtn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 販売大臣グループボックス内のボタンの配列を返します。
        /// </summary>
        /// <returns>販売大臣グループボックス内のボタンの配列</returns>
        private CButton[] GetHBButtons()
        {
            try
            {
                CButton[] cbtn = {
                                    cbtnHBSetTables,
                                    cbtnHBSetERPScript,
                                    cbtnHBSetERPMenu
                                 };
                return cbtn;
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
                cbtnESC.Text = "ESC:閉じる";
                cbtnESC.Enabled = true;
                cbtnESC.TabStop = false;

                //// テーブル設定ボタン
                foreach (CButton cbtn in GetSetTablesButtons())
                {
                    cbtn.Text = "テーブル設定";
                    cbtn.Enabled = true;
                    cbtn.TabStop = true;
                }

                //// ERP拡張スクリプトの設定ボタン
                foreach (CButton cbtn in GetSetERPScriptButtons())
                {
                    cbtn.Text = "ERP拡張スクリプトの設定";
                    cbtn.Enabled = true;
                    cbtn.TabStop = true;
                }

                //// ERPメニューの設定ボタン
                foreach (CButton cbtn in GetSetERPMenuButtons())
                {
                    cbtn.Text = "ERPメニューの設定";
                    cbtn.Enabled = true;
                    cbtn.TabStop = true;
                }

                //// 販売大臣グループボックス内のボタン
                foreach (CButton cbtn in GetHBButtons())
                {
                    cbtn.Tag = OHKENProducts.HB;
                }
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
            //// ESC:閉じる
            cbtnESC.Click += (sender_, e_) => this.Close();

            //// テーブル設定ボタン
            foreach (CButton cbtn in GetSetTablesButtons())
            {
                cbtn.Click += (sender_, e_) => SetTables((OHKENProducts)cbtn.Tag);
            }

            //// ERP拡張スクリプトの設定ボタン
            foreach (CButton cbtn in GetSetERPScriptButtons())
            {
                cbtn.Click += (sender_, e_) => SetERPScript((OHKENProducts)cbtn.Tag);
            }

            //// ERPメニューの設定ボタン
            foreach (CButton cbtn in GetSetERPMenuButtons())
            {
                cbtn.Click += (sender_, e_) => SetERPMenu((OHKENProducts)cbtn.Tag);
            }
        }
        #endregion
        #endregion

        #region テーブル設定
        /// <summary>
        /// テーブルを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        public void SetTables(OHKENProducts ohkenProd)
        {
            try
            {
                if (CClass.SetTables(ohkenProd))
                {
                    CMessage.ShowInformationMessage(ODMessage.Message("I", "5").Replace("%s", "テーブルを"), CClass.ProgramId);
                }
            }
            catch (Exception ex)
            {
                CClass.CLogObj.OutputMessage(ex);
            }
            finally
            {
                this.SetActiveWindow();
            }
        }
        #endregion

        #region ERP拡張スクリプトの設定
        /// <summary>
        /// ERP拡張スクリプトを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        public void SetERPScript(params OHKENProducts[] ohkenProds)
        {
            try
            {
                CClass.SetERPScript(ohkenProds);

                CMessage.ShowInformationMessage(ODMessage.Message("I", "5").Replace("%s", "ERP拡張スクリプトを"), CClass.ProgramId);
            }
            catch (Exception ex)
            {
                CClass.CLogObj.OutputMessage(ex);
            }
            finally
            {
                this.SetActiveWindow();
            }
        }
        #endregion

        #region ERPメニューの設定
        /// <summary>
        /// ERPメニューを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        public void SetERPMenu(params OHKENProducts[] ohkenProds)
        {
            try
            {
                CClass.SetERPMenu(ohkenProds);

                CMessage.ShowInformationMessage(ODMessage.Message("I", "5").Replace("%s", "ERPメニューを"), CClass.ProgramId);
            }
            catch (Exception ex)
            {
                CClass.CLogObj.OutputMessage(ex);
            }
            finally
            {
                this.SetActiveWindow();
            }
        }
        #endregion
    }
}
