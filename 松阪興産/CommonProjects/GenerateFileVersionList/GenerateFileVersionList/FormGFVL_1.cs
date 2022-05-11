using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GenerateFileVersionList
{
    using CClass = GFVLLibrary;

    public partial class FormGFVL_1 : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormGFVL_1()
        {
            InitializeComponent();
            var commonClass = new GFVLLibrary();
        }
        #endregion

        #region フォームイベント
        /// <summary>
        /// フォームが初めて表示される直前に発生します。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private void FormGFHL_1_Load(object sender, EventArgs e)
        {
            try
            {
                SetPropertyToProgressBar();

                InitControls();

                Running();
            }
            catch
            {
                // アプリケーションを強制終了します
                Environment.Exit(-1);
            }
        }
        #endregion

        #region コントロール プロパティ設定
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

        #region バージョン一覧生成処理
        /// <summary>
        /// ファイルのバージョン一覧を生成します。
        /// </summary>
        private async void Running()
        {
            string message = "";
            try
            {
                pbStatus.Value = 0;

                await Task.Run(() =>
                {
                    try
                    {
                        this.Invoke(new Action(() =>
                        {
                            lblStatus.Text = "バージョン一覧の生成を開始します。";
                        }));

                        if (!GenerateFileVersionList(out message))
                        {
                            this.Invoke(new Action(() =>
                            {
                                lblStatus.Text = "バージョン一覧の生成に失敗しました。";

                                CMessage.ShowErrorMessage(message);
                            }));
                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                lblStatus.Text = "バージョン一覧を生成しました。";

                                CMessage.ShowInformationMessage(CClass.VersionListPath + " を生成しました。");
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            lblStatus.Text = "バージョン一覧の生成に失敗しました。";

                            CMessage.ShowErrorMessage(ex.ToString());
                        }));

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
            catch
            {

            }
        }

        /// <summary>
        /// ファイルのバージョン一覧を生成します。
        /// </summary>
        /// <param name="message">メッセージを格納</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        private bool GenerateFileVersionList(out string message)
        {
            message = "";
            try
            {
                if (!CFile.DeleteFileSafely(CClass.VersionListPath))
                {
                    message = CClass.VersionListPath + " の削除に失敗しました。";

                    return false;
                }

                var files = Directory.EnumerateFiles(CClass.ScanDirPath, "*", SearchOption.AllDirectories);
                if (files.Count() > 0)
                {
                    this.Invoke(new Action(() =>
                    {
                        pbStatus.Maximum = files.Count();

                        lblStatus.Text = "バージョン一覧生成中...";
                    }));

                    foreach (string file in files)
                    {
                        string row = Regex.Replace(file, @"^" + Regex.Escape(CClass.ScanDirPath), "")
                                   + ","
                                   + FileVersionInfo.GetVersionInfo(file).FileVersion
                                   + ","
                                   + CClass.GetFileHash(file);

                        CFile.WriteFile(CClass.VersionListPath, row, true, true, "utf-8");

                        this.Invoke(new Action(() =>
                        {
                            pbStatus.PerformStep();
                        }));
                    }
                }
                else
                {
                    message = "走査対象のファイルが存在しません。";

                    this.Invoke(new Action(() =>
                    {
                        pbStatus.Maximum = 1;
                        pbStatus.PerformStep();
                    }));

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.ToString();

                return false;
            }
        }
        #endregion
    }
}
