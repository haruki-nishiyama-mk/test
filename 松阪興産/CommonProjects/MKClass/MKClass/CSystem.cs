using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace MKClass
{
    #region 列挙型
    /// <summary>
    /// 様々な処理結果の状態を示します。必要に応じて使用してください。
    /// </summary>
    public enum ResultStatus : int
    {
        /// <summary>(なし)</summary>
        None = 0,
        /// <summary>成功</summary>
        Success = 1,
        /// <summary>警告</summary>
        Warning = 9,
        /// <summary>エラー</summary>
        Error = -1
    }
    #endregion



    #region システム管理クラス
    /// <summary>
    /// システム管理クラス
    /// </summary>
    public class CSystem
    {
        /// <summary>数値型リスト</summary>
        public static List<Type> NUMERIC_TYPES { get { return _NUMERIC_TYPES; } }
        private static readonly List<Type> _NUMERIC_TYPES =
            new List<Type>() {
                typeof(byte),
                typeof(sbyte),
                typeof(int),
                typeof(uint),
                typeof(short),
                typeof(ushort),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal)
            };

        /// <summary>
        /// プロセス名 (プログラムID) を取得します。
        /// </summary>
        /// <returns>プロセス名 (プログラムID)</returns>
        /// <remarks>
        /// 親子関係がある場合、親のプロセス名になります。
        /// 自身のプロセス名を取得したい場合、GetAssemblyName()メソッドで取得してください。
        /// </remarks>
        public static string GetProcessName()
        {
            try
            {
                return Regex.Replace(Process.GetCurrentProcess().ProcessName, ".vshost", "");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// アセンブリ名 (プログラムID) を取得します。
        /// </summary>
        /// <typeparam name="T">クラス</typeparam>
        /// <param name="obj">取得したいオブジェクト</param>
        /// <returns>アセンブリ名 (プログラムID)</returns>
        public static string GetAssemblyName<T>(T obj)
        {
            try
            {
                return obj.GetType().Assembly.GetName().Name;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// プロセス管理クラス
        /// </summary>
        public class MyProcess
        {
            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            private static extern bool SetForegroundWindow(IntPtr hWnd);

            private const int SW_NORMAL = 1;

            /// <summary>
            /// 同名のプロセスが起動中の場合、メインウィンドウをアクティブにします。
            /// </summary>
            /// <returns>
            /// true : 同名のプロセスが起動中 
            /// false: それ以外
            /// </returns>
            public static bool ShowPrevProcess()
            {
                Process process = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(process.ProcessName);
                int processId = process.Id;

                foreach (Process hProcess in processes)
                {
                    if (hProcess.Id != processId)
                    {
                        ShowWindow(hProcess.MainWindowHandle, SW_NORMAL);
                        SetForegroundWindow(hProcess.MainWindowHandle);

                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 同名のプロセスが起動中の場合、終了させます。
            /// </summary>
            public static void ExitPrevProcess()
            {
                Process process = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(process.ProcessName);
                int processId = process.Id;

                foreach (Process hProcess in processes)
                {
                    if (hProcess.Id != processId)
                    {
                        hProcess.Kill();
                    }
                }
            }
        }
    }
    #endregion
}
