using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GenerateFileVersionList
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (!CSystem.MyProcess.ShowPrevProcess())
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormGFVL_1());
                }
            }
            catch
            {
                return;
            }
        }
    }
}
