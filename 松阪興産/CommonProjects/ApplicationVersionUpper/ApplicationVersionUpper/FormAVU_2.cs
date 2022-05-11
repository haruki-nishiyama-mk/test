using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ApplicationVersionUpper
{
    using CClass = AVULibrary;

    internal partial class FormAVU_2 : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormAVU_2()
        {
            try
            {
                InitializeComponent();

                this.DoubleBuffered = true;

                ////// App.configの読み込み
                SetBackColor(CClass.GetAppSettingValue("BackColor"));
                SetForeColor(CClass.GetAppSettingValue("ForeColor"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region フォームイベント
        /// <summary>
        /// フォームが初めて表示される直前に発生します。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private void FormAVU_2_Load(object sender, EventArgs e)
        {
            try
            {
                SetPropertyToProgressBar();

                InitControls();

                Running();
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.ToString(), CClass.ProgramId);

                this.DialogResult = DialogResult.Abort;
                this.Close();

                return;
            }
        }
        #endregion

        #region コントロール プロパティ設定
        #region Form プロパティ設定
        /// <summary>
        /// Formの背景色を設定します。
        /// </summary>
        /// <param name="color">RGB (カンマ区切り)</param>
        private void SetBackColor(string color)
        {
            try
            {
                var rgb = color.Split(',');
                if (rgb.Length == 3)
                {
                    int r = CConvert.ToIntDef(rgb[0].Trim(), 255);
                    int g = CConvert.ToIntDef(rgb[1].Trim(), 255);
                    int b = CConvert.ToIntDef(rgb[2].Trim(), 255);

                    this.BackColor = Color.FromArgb(r, g, b);
                }
            }
            catch (Exception)
            {
                this.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// Formの前景色を設定します。
        /// </summary>
        /// <param name="color">RGB (カンマ区切り)</param>
        private void SetForeColor(string color)
        {
            try
            {
                var rgb = color.Split(',');
                if (rgb.Length == 3)
                {
                    int r = CConvert.ToIntDef(rgb[0].Trim(), 0);
                    int g = CConvert.ToIntDef(rgb[1].Trim(), 0);
                    int b = CConvert.ToIntDef(rgb[2].Trim(), 0);

                    this.ForeColor = Color.FromArgb(r, g, b);
                    lblStatus.ForeColor = Color.FromArgb(r, g, b);
                }
            }
            catch (Exception)
            {
                this.ForeColor = SystemColors.ControlText;
            }
        }
        #endregion

        #region ProgressBar プロパティ設定
        /// <summary>
        /// ProgressBarのプロパティを設定します。
        /// </summary>
        private void SetPropertyToProgressBar()
        {
            try
            {
                pbStatus.Maximum = 1;
                pbStatus.Step = 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region コントロール初期化
        /// <summary>
        /// コントロールの初期化を行います。
        /// </summary>
        private void InitControls()
        {
            try
            {
                lblStatus.Text = "";

                pbStatus.Value = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 更新処理
        /// <summary>
        /// アプリケーションの更新を行います。
        /// </summary>
        private async void Running()
        {
            try
            {
                pbStatus.Value = 0;

                await Task.Run(() =>
                {
                    try
                    {
                        UpdateApplications();
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (CClass.ShowMessage)
                            {
                                CMessage.ShowErrorMessage(ex.ToString(), CClass.ProgramId);
                            }
                        }));

                        this.DialogResult = DialogResult.Abort;

                        return;
                    }
                    finally
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.Close();
                        }));
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// アプリケーションの更新を行います。
        /// </summary>
        private void UpdateApplications()
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "アップデート中...";
                }));

                var updateApplications = CClass.GetUpdateApplications();
                if (updateApplications.Count() > 0)
                {
                    this.Invoke(new Action(() =>
                    {
                        pbStatus.Maximum = updateApplications.Count();
                    }));

                    foreach (var serverFilePath in updateApplications)
                    {
                        // サブディレクトリを考慮
                        string localFilePath = CClass.LocalDirPath + Regex.Replace(serverFilePath, @"^" + Regex.Escape(CClass.ServerDirPath), "");

                        CFile.CreateDirectorySafely(Path.GetDirectoryName(localFilePath));
                        CFile.CopyFileSafely(serverFilePath, localFilePath, true);

                        this.Invoke(new Action(() =>
                        {
                            pbStatus.PerformStep();
                        }));
                    }
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        pbStatus.Maximum = 1;
                        pbStatus.PerformStep();
                    }));
                }

                if (!String.IsNullOrEmpty(CClass.ServerVersion))
                {
                    CFile.WriteFile(CClass.LocalVersionControlFilePath, CClass.ServerVersion, false, false);
                }

                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "アップデートが完了しました。";
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
