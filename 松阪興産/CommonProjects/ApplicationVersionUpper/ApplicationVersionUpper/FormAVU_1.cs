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

namespace ApplicationVersionUpper
{
    using CClass = AVULibrary;

    public partial class FormAVU_1 : CFormlessApplication
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormAVU_1()
        {
            try
            {
                var commonClass = new AVULibrary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                CSystem.GetAssemblyName<FormAVU_1>(this),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                throw new Exception();
            }
        }
        #endregion

        #region フォームレス処理
        /// <summary>
        /// アプリケーションを初期化します。
        /// </summary>
        /// <returns>
        /// true : アプリケーション継続
        /// false: アプリケーション終了
        /// </returns>
        public override bool Initialize()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.ToString(), CClass.ProgramId);
            }

            return false;
        }

        /// <summary>
        /// アプリケーションの処理を実行します。
        /// </summary>
        public override void DoWork()
        {
            try
            {
                bool doStartupFile = true;

                //// アプリケーションの更新
                try
                {
                    if (CClass.DoUpdate())
                    {
                        using (FormAVU_2 form = new FormAVU_2())
                        {
                            if (form.ShowDialog() == DialogResult.Abort)
                            {
                                doStartupFile = CClass.DoForceStart;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CMessage.ShowErrorMessage(ex.ToString(), CClass.ProgramId);

                    doStartupFile = CClass.DoForceStart;
                }

                if (!doStartupFile)
                {
                    return;
                }

                //// 指定のアプリケーションを起動
                try
                {
                    if (File.Exists(CClass.StartupFilePath))
                    {
                        Process.Start(CClass.StartupFilePath);
                    }
                }
                catch (Exception ex)
                {
                    CMessage.ShowErrorMessage(ex.ToString(), CClass.ProgramId);
                }
            }
            finally
            {
                this.ExitApp();
            }
        }

        /// <summary>
        /// 全てのリソースを破棄します。
        /// </summary>
        /// <param name="disposing">マネージドリソース破棄の有無</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}
