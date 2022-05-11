using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace ApplicationVersionUpper
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        /// <param name="args">プログラム引数</param>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (!CSystem.MyProcess.ShowPrevProcess())
                {
                    #region 管理者権限起動 (マニフェスト不要)
#if (!DEBUG)
                    Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                    var principal = (WindowsPrincipal)Thread.CurrentPrincipal;

                    if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                    {
                        var process = new ProcessStartInfo()
                        {
                            WorkingDirectory = Application.StartupPath,
                            FileName = Assembly.GetEntryAssembly().Location,
                            Verb = "RunAs"
                        };

                        if (args.Length >= 1)
                        {
                            process.Arguments = String.Join(" ", args);
                        }

                        Process.Start(process);

                        return;
                    }
#endif
                    #endregion

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new CFormlessApplicationContext(new FormAVU_1()));
                }
            }
            catch
            {
                return;
            }
        }
    }
}
