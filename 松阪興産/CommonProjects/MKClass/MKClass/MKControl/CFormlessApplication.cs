using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MKClass.MKControl
{
    /// <summary>
    /// フォームレスアプリケーションクラス
    /// </summary>
    /// <remarks>
    /// メインフォームを利用しないアプリケーションの処理を実装するクラスです。
    /// </remarks>
    public abstract class CFormlessApplication : IDisposable
    {
        /// <summary>
        /// このアプリケーションを実行しているコンテキストを取得します。
        /// </summary>
        protected internal CFormlessApplicationContext Context { get; internal set; }

        /// <summary>
        /// アプリケーションを初期化します。
        /// </summary>
        /// <returns>
        /// true : 処理を継続する
        /// false: それ以外
        /// </returns>
        public virtual bool Initialize()
        {
            return true;
        }

        /// <summary>
        /// アプリケーションの処理を実行します。
        /// </summary>
        public virtual void DoWork()
        {

        }

        /// <summary>
        /// 全てのリソースを破棄します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 全てのリソースを破棄します。
        /// </summary>
        /// <param name="disposing">マネージドリソース破棄の有無</param>
        protected virtual void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        protected void ExitApp()
        {
            this.Context.ExitThread();
        }
    }



    /// <summary>
    /// フォームレスアプリケーション終了クラス
    /// </summary>
    /// <remarks>
    /// メインフォームを利用しないアプリケーションの終了を定義します。
    /// </remarks>
    public class CFormlessApplicationContext : ApplicationContext
    {
        private static class SafeNativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct POINT
            {
                public int X;
                public int Y;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct MSG
            {
                public IntPtr HWND;
                public int Message;
                public IntPtr WParam;
                public IntPtr LParam;
                public int Time;
                public POINT Point;
            }

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PeekMessage(out MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);
        }

        /// <summary>
        /// 実行しているアプリケーションを取得します。
        /// </summary>
        public CFormlessApplication App { get; set; }

        /// <summary>
        /// FormlessApplicationContextクラスを初期化し、実行するアプリケーションを設定します。
        /// </summary>
        /// <param name="app">実行するアプリケーション</param>
        public CFormlessApplicationContext(CFormlessApplication app)
            : base()
        {
            this.App = app;
            this.App.Context = this;

            // アプリケーション初期化
            if (!this.App.Initialize())
            {
                // アプリケーション終了
                Application.Idle += (e, sender) => this.ExitThread();

                return;
            }

            // モーダル状態を捕捉
            Application.EnterThreadModal += this.Application_EnterThreadModal;
            Application.LeaveThreadModal += this.Application_LeaveThreadModal;

            // Idleイベント登録
            Application.Idle += this.Application_Idle;
        }

        /// <summary>
        /// ApplicationContextで使用されたアンマネージドリソースを解放し、
        /// 必要に応じてマネージドリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージドリソース破棄の有無</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // App破棄
                this.App.Dispose();
            }

            base.Dispose(disposing);
        }

        private void Application_EnterThreadModal(object sender, EventArgs e)
        {
            // Idleイベント一時解除
            Application.Idle -= this.Application_Idle;
        }

        private void Application_LeaveThreadModal(object sender, EventArgs e)
        {
            // Idleイベント再登録
            Application.Idle += this.Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            // ウィンドウメッセージを受信するまでループ処理
            SafeNativeMethods.MSG msg;
            while (!SafeNativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0))
            {
                // アプリケーションの処理を実行
                this.App.DoWork();

                Thread.Sleep(1);
            }
        }
    }
}
