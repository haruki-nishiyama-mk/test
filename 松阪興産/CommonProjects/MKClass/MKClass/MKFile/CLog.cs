using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Linq;
using MKClass.MKException;

namespace MKClass.MKFile
{
    /// <summary>
    /// ログ操作クラス
    /// </summary>
    public class CLog : CFile
    {
        /// <summary>プロセス名 (プログラムID)</summary>
        private readonly string _PROCESS_NAME = "";

        /// <summary>ログの保存ファイルパス</summary>
        private string _logPath = "";
        /// <summary>エンコード</summary>
        private Encoding _logEncode = Encoding.GetEncoding("Shift_JIS");

        #region public
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CLog()
            : this("")
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        public CLog(string systemName)
            : this("", systemName)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        public CLog(string configPath, string systemName)
        {
            try
            {
                _PROCESS_NAME = CSystem.GetProcessName();

                Initialize();

                ReadConfig(configPath, systemName);
            }
            catch (Exception ex)
            {
                CMessage.ShowErrorMessage(ex.Message, CSystem.GetProcessName());

                throw new Exception();
            }
        }

        /// <summary>
        /// 例外エラーを出力します。
        /// </summary>
        /// <param name="ex">例外クラス (Exception)</param>
        /// <param name="logPath">ログの保存ファイルパス</param>
        /// <param name="bShowMessage">メッセージ表示有無</param>
        public void OutputMessage(Exception ex, string logPath = "", bool bShowMessage = true)
        {
            try
            {
                string message = GetOutputMessage(ex);

                CFile.WriteFile(
                    String.IsNullOrEmpty(logPath) ? _logPath : logPath,
                    message,
                    true,
                    false,
                    _logEncode);

                if (bShowMessage)
                {
                    CMessage.ShowErrorMessage(ex.Message, _PROCESS_NAME);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 任意のメッセージを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="logPath">ログの保存ファイルパス</param>
        public void OutputMessage(string message, string logPath = "")
        {
            try
            {
                WriteFile(
                    String.IsNullOrEmpty(logPath) ? _logPath : logPath,
                    GetOutputMessage(message),
                    true,
                    false,
                    _logEncode);
            }
            catch
            {

            }
        }
        #endregion

        #region private
        /// <summary>
        /// ログ設定を初期化します。
        /// </summary>
        private void Initialize()
        {
            string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            try
            {
                _logPath = Path.GetFullPath(Path.Combine(appDataLocal, "Manac", "Data", "Log", "Application.log"));
                _logEncode = Encoding.GetEncoding("Shift_JIS");
            }
            catch
            {

            }
        }

        /// <summary>
        /// ログ設定ファイルを読み込みます。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        private void ReadConfig(string configPath, string systemName)
        {
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CFile.CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return;
                }

                XDocument xDoc = XDocument.Load(configPath);

                var xeLog = (
                        from x in xDoc.Root.Elements("system")
                        where x.Attribute("name").Value == systemName
                        select x.Element("data").Element("log")
                    ).SingleOrDefault();
                if (xeLog != null)
                {
                    string logPath = xeLog.Attribute("path").Value;
                    _logPath = (CFile.IsAbsolatePath(logPath)) ? Path.GetFullPath(logPath) : Path.GetFullPath(GetDataPath(systemName) + logPath);
                    _logEncode = Encoding.GetEncoding(xeLog.Attribute("encode").Value);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 出力メッセージを取得します。
        /// </summary>
        /// <param name="ex">例外クラス</param>
        /// <returns>出力メッセージ</returns>
        private string GetOutputMessage(Exception ex)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]") + " " + _PROCESS_NAME);
                if (ex is COMException)
                {
                    COMException ex_ = (COMException)ex;
                    sb.AppendLine(ex_.ErrorCode.ToString());
                    sb.AppendLine(ex_.Message);
                    sb.AppendLine(ex_.StackTrace);
                }
                else if (ex is CDataBaseException)
                {
                    CDataBaseException ex_ = (CDataBaseException)ex;
                    sb.AppendLine(ex_.Message);
                    if (!String.IsNullOrEmpty(ex_.Detail))
                    {
                        sb.AppendLine("(" + ex_.Detail + ")");
                    }
                    sb.AppendLine(ex_.StackTrace);
                    if (!String.IsNullOrEmpty(ex_.QueryString))
                    {
                        sb.AppendLine();
                        sb.AppendLine("** query start ******************************");
                        sb.AppendLine(ex_.QueryString.TrimEnd(Environment.NewLine.ToCharArray()));
                        sb.AppendLine("** query end ********************************");
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.StackTrace);
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 出力メッセージを取得します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>出力メッセージ</returns>
        private string GetOutputMessage(string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]") + " " + _PROCESS_NAME);
                sb.AppendLine(message);

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
