using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace GenerateFileVersionList
{
    #region GenerateFileVersionList 共通ライブラリクラス
    /// <summary>
    /// ハッシュ種別
    /// </summary>
    internal enum CryptoType : int
    {
        /// <summary>MD5</summary>
        MD5 = 0,
        /// <summary>SHA1</summary>
        SHA1 = 1,
        /// <summary>SHA256</summary>
        SHA256 = 2
    }

    /// <summary>
    /// GenerateFileVersionList 共通ライブラリクラス
    /// </summary>
    internal class GFVLLibrary
    {
        #region プロパティ
        /// <summary>プログラムID</summary>
        public static string ProgramId { get; private set; }

        /// <summary>走査するディレクトリパス</summary>
        public static string ScanDirPath { get; private set; }
        /// <summary>ファイルのバージョン一覧 (CSV)</summary>
        public static string VersionListPath { get; private set; }
        /// <summary>ハッシュ</summary>
        public static CryptoType Hash { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GFVLLibrary()
        {
            try
            {
                ProgramId = CSystem.GetAssemblyName<GFVLLibrary>(this);

                ////// App.configの読み込み
                ScanDirPath = GetAppSettingValue("ScanDirPath");
                if (!CFile.IsAbsolatePath(ScanDirPath))
                {
                    ScanDirPath = Environment.CurrentDirectory + ScanDirPath;
                }

                VersionListPath = ScanDirPath + @"\" + GetAppSettingValue("VersionList");

                switch (GetAppSettingValue("Hash"))
                {
                    case "MD5":
                        Hash = CryptoType.MD5;

                        break;
                    case "SHA1":
                        Hash = CryptoType.SHA1;

                        break;
                    default:
                        Hash = CryptoType.SHA256;

                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region データ取得
        /// <summary>
        /// アプリケーション構成ファイルから指定したキーに対する値を取得します。
        /// </summary>
        /// <param name="nvc">キー</param>
        /// <returns>値</returns>
        public static string GetAppSettingValue(string key)
        {
            try
            {
                return (ConfigurationManager.AppSettings[key] != null) ? ConfigurationManager.AppSettings[key].ToString() : "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// ファイルのハッシュ値を取得します。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>ファイルのハッシュ値</returns>
        public static string GetFileHash(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return "";
                }

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    switch (Hash)
                    {
                        case CryptoType.MD5:
                            return BitConverter.ToString(MD5.Create().ComputeHash(fs));
                        case CryptoType.SHA1:
                            return BitConverter.ToString(SHA1.Create().ComputeHash(fs));
                        default:
                            return BitConverter.ToString(SHA256.Create().ComputeHash(fs));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
    #endregion



    #region システム管理クラス
    /// <summary>
    /// システム管理クラス
    /// </summary>
    public class CSystem
    {
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
            private static extern int ShowWindow(System.IntPtr hWnd, int nCmdShow);

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            private static extern bool SetForegroundWindow(System.IntPtr hWnd);

            private const int SW_NORMAL = 1;

            /// <summary>
            /// 同名のプロセスが起動中の場合、メインウィンドウをアクティブにします。
            /// </summary>
            /// <returns>
            /// true : 同名のプロセスが起動中 
            /// false: 起動していない
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



    #region ファイル操作クラス
    /// <summary>
    /// ファイル操作クラス
    /// </summary>
    public class CFile
    {
        #region ディレクトリ操作
        /// <summary>
        /// 指定したパス文字列が絶対パスかどうかを判断します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>
        /// true : 絶対パス
        /// false: 相対パス
        /// </returns>
        public static bool IsAbsolatePath(string path)
        {
            try
            {
                // UNCパス (example. \\)
                if (path.Length >= 2 &&
                        (path[0] == Path.DirectorySeparatorChar && path[1] == Path.DirectorySeparatorChar))
                {
                    return true;
                }

                // ローカルパス (example. C:\)
                if (path.Length >= 3 &&
                        (Regex.IsMatch(path[0].ToString(), @"[a-zA-Z]") && path[1] == Path.VolumeSeparatorChar && path[2] == Path.DirectorySeparatorChar))
                {
                    return true;
                }
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// ディレクトリを安全に作成します。
        /// </summary>
        /// <param name="path">作成パス</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        /// <remarks>
        /// 既に存在する場合、trueを返します。
        /// </remarks>
        public static bool CreateDirectorySafely(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            try
            {
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion

        #region ファイル操作
        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        /// <param name="path">読み込むファイルパス</param>
        /// <param name="encode">エンコード</param>
        /// <returns>読み込んだファイルの中身</returns>
        public static string ReadFile(string path, string encode = @"shift_jis")
        {
            string ret = "";

            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(encode)))
                {
                    try
                    {
                        ret = sr.ReadToEnd();
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                        }
                    }
                }
            }
            catch
            {

            }

            return ret;
        }

        /// <summary>
        /// ファイルに出力します。
        /// </summary>
        /// <param name="path">出力パス</param>
        /// <param name="value">出力値</param>
        /// <param name="append">追記するかどうか</param>
        /// <param name="newLine">行終端記号の有無</param>
        /// <param name="encode">エンコード</param>
        public static void WriteFile(string path, string value, bool append = true, bool newLine = true, string encode = @"shift_jis")
        {
            try
            {
                CreateDirectorySafely(Path.GetDirectoryName(path));

                using (StreamWriter sw = new StreamWriter(path, append, Encoding.GetEncoding(encode)))
                {
                    try
                    {
                        if (newLine)
                        {
                            sw.WriteLine(value);
                        }
                        else
                        {
                            sw.Write(value);
                        }
                        sw.Flush();
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// ファイルを安全に削除します。
        /// </summary>
        /// <param name="filePath">削除するファイルパス</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        public static bool DeleteFileSafely(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ファイルを安全にコピー、または移動します。
        /// </summary>
        /// <param name="sourceFilePath">コピー元のファイルパス</param>
        /// <param name="destFilePath">コピー先のファイルパス</param>
        /// <param name="overwrite">上書きの有無</param>
        /// <param name="fileMove">移動の有無 (Copy => Delete)</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        public static bool CopyFileSafely(string sourceFilePath, string destFilePath, bool overwrite = false, bool fileMove = false)
        {
            try
            {
                File.Copy(sourceFilePath, destFilePath, overwrite);

                if (fileMove)
                {
                    File.Delete(sourceFilePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
    #endregion



    #region メッセージクラス (static)
    /// <summary>
    /// メッセージクラス (static)
    /// </summary>
    public static class CMessage
    {
        #region メッセージ表示
        /// <summary>
        /// インフォメーションを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowInformationMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// 質問メッセージを表示します。 (OK選択)
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        /// <param name="defaultButton">既定のボタン</param>
        /// <returns>
        /// true : ダイアログボックスの戻り値に対して一致 (OK選択)
        /// false: 上記以外
        /// </returns>
        public static bool ShowQuestionMessageOK(string text,
                                                 string caption = "",
                                                 MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            if (MessageBox.Show(text,
                                caption,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question,
                                defaultButton) == DialogResult.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 質問メッセージを表示します。 (キャンセル選択)
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        /// <param name="defaultButton">既定のボタン</param>
        /// <returns>
        /// true : ダイアログボックスの戻り値に対して一致
        /// false: 上記以外
        /// </returns>
        public static bool ShowQuestionMessageCancel(string text,
                                                     string caption = "",
                                                     MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            if (MessageBox.Show(text,
                                caption,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question,
                                defaultButton) == DialogResult.Cancel)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowWarningMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowErrorMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        #endregion
    }
    #endregion
}
