using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ApplicationVersionUpper
{
    #region ApplicationVersionUpper 共通ライブラリクラス
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
    /// ApplicationVersionUpper 共通ライブラリクラス
    /// </summary>
    internal class AVULibrary
    {
        #region プロパティ
        /// <summary>プログラムID</summary>
        public static string ProgramId { get; private set; }

        /// <summary>システムのアプリケーションが格納されているローカルのディレクトリパス</summary>
        public static string LocalDirPath { get; private set; }
        /// <summary>システムのアプリケーションが格納されているサーバのディレクトリパス (アプリケーション管理元)</summary>
        public static string ServerDirPath { get; private set; }
        /// <summary>バージョン管理ファイルパス (ローカル)</summary>
        public static string LocalVersionControlFilePath { get; private set; }
        /// <summary>バージョン管理ファイルパス (アプリケーション管理元)</summary>
        public static string ServerVersionControlFilePath { get; private set; }
        /// <summary>バージョン (ローカル)</summary>
        public static string LocalVersion { get; private set; }
        /// <summary>バージョン (アプリケーション管理元)</summary>
        public static string ServerVersion { get; private set; }
        /// <summary>ファイルのバージョン一覧 (CSV)のパス</summary>
        public static string ServerVersionListPath { get; private set; }
        /// <summary>ハッシュ</summary>
        public static CryptoType Hash { get; private set; }
        /// <summary>アップデート後に起動するアプリケーションのパス</summary>
        public static string StartupFilePath { get; private set; }
        /// <summary>エラー時でもアプリケーションを実行するかどうか</summary>
        public static bool DoForceStart { get; private set; }
        /// <summary>メッセージを表示するかどうか</summary>
        public static bool ShowMessage { get; private set; }
        /// <summary>デバッグモード</summary>
        public static bool Debug { get; private set; }
        /// <summary>デバッグのパス</summary>
        public static string DebugPath { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AVULibrary()
        {
            try
            {
                ProgramId = CSystem.GetAssemblyName<AVULibrary>(this);

                //// App.configの読み込み
                LocalDirPath = GetAppSettingValue("LocalDirPath");
                if (!CFile.IsAbsolatePath(LocalDirPath))
                {
                    LocalDirPath = Application.StartupPath + LocalDirPath;
                }

                ServerDirPath = GetAppSettingValue("ServerDirPath");

                LocalVersionControlFilePath = LocalDirPath + @"\" + GetAppSettingValue("VersionControlFile");
                ServerVersionControlFilePath = ServerDirPath + @"\" + GetAppSettingValue("VersionControlFile");

                LocalVersion = CFile.ReadFile(LocalVersionControlFilePath);
                ServerVersion = CFile.ReadFile(ServerVersionControlFilePath);

                ServerVersionListPath = ServerDirPath + @"\" + GetAppSettingValue("VersionList");

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

                StartupFilePath = LocalDirPath + @"\" + GetAppSettingValue("StartupFile");

                DoForceStart = CConvert.ToBoolean(GetAppSettingValue("DoForceStart"));
                ShowMessage = CConvert.ToBoolean(GetAppSettingValue("ShowMessage"));

                Debug = CConvert.ToBoolean(GetAppSettingValue("Debug"));
                DebugPath = Application.StartupPath + @"\debug.log";        // 固定

                CFile.DeleteFileSafely(DebugPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region アップデートの実行有無
        /// <summary>
        /// アプリケーションをアップデートするかしないかをチェックします。
        /// </summary>
        /// <returns>
        /// true : アップデートする
        /// false: アップデートしない
        /// </returns>
        public static bool DoUpdate()
        {
            try
            {
                if (String.IsNullOrEmpty(ServerDirPath))
                {
                    return false;
                }

                if (!Directory.Exists(ServerDirPath))
                {
                    throw new DirectoryNotFoundException("'" + ServerDirPath + "' が見つかりませんでした。");
                }

                if (LocalVersion == ServerVersion)
                {
                    return false;
                }

                return true;
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
        /// <param name="key">キー</param>
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
        /// アップデート対象のファイルパスを取得する
        /// </summary>
        /// <returns>アップデート対象のファイルパス</returns>
        public static List<string> GetUpdateApplications()
        {
            try
            {
                List<string> updateApplications = new List<string>();
                using (StreamReader sr = new StreamReader(ServerVersionListPath, Encoding.GetEncoding("utf-8")))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var values = line.Split(',');
                        if (values.Length == 3)
                        {
                            string file = values[0];
                            string version = values[1];
                            string hash = values[2];

                            string serverFilePath = ServerDirPath + file;
                            string localFilePath = LocalDirPath + file;

                            if (!File.Exists(serverFilePath))
                            {
                                continue;
                            }
                            else if (serverFilePath == ServerVersionControlFilePath)    // Version.vmfファイルは除く
                            {
                                continue;
                            }
                            else if (serverFilePath == ServerVersionListPath)           // VersionList.csvファイルは除く
                            {
                                continue;
                            }
                            else if (String.IsNullOrEmpty(hash))
                            {
                                continue;
                            }

                            // ハッシュ値が異なる場合、更新対象とする
                            if (GetFileHash(localFilePath) != hash)
                            {
                                if (Debug)
                                {
                                    CFile.WriteFile(DebugPath, serverFilePath);
                                }

                                updateApplications.Add(serverFilePath);
                            }
                        }
                    }
                }

                return updateApplications;
            }
            catch (Exception)
            {
                throw;
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



    #region データ検証クラス
    /// <summary>
    /// データ検証クラス
    /// </summary>
    public class CValidate
    {
        #region 型変換チェック
        /// <summary>
        /// char型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToChar(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            char result = default(char);
            try
            {
                result = Convert.ToChar(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// byte型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            byte result = default(byte);
            try
            {
                result = Convert.ToByte(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// int型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            int result = default(int);
            try
            {
                result = Convert.ToInt32(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// decimal型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToDecimal(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            decimal result = default(decimal);
            try
            {
                result = Convert.ToDecimal(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// double型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToDouble(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            double result = default(double);
            try
            {
                result = Convert.ToDouble(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// float型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToFloat(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            float result = default(float);
            try
            {
                result = float.Parse(value.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// DateTime型に変換できるかどうかをチェックします。
        /// </summary>
        /// <param name="value">チェックするオブジェクト</param>
        /// <returns>
        /// true : 変換できる
        /// false: 変換できない
        /// </returns>
        public static bool IsToDateTime(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            DateTime result = default(DateTime);
            try
            {
                result = Convert.ToDateTime(value);
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion
    }
    #endregion



    #region データ変換クラス
    /// <summary>
    /// データ変換クラス
    /// </summary>
    public class CConvert
    {
        #region 型変換
        /// <summary>
        /// char型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>char型の文字列</returns>
        public static char ToChar(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(char);
            }

            try
            {
                return Convert.ToChar(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// char型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>char型の文字列</returns>
        public static char ToCharDef(object value, char defaultValue = default(char))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            char result = default(char);
            try
            {
                result = Convert.ToChar(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// byte型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>byte型の値</returns>
        public static byte ToByte(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(byte);
            }

            try
            {
                return Convert.ToByte(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// byte型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>byte型の値</returns>
        public static byte ToByteDef(object value, byte defaultValue = default(byte))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            byte result = default(byte);
            try
            {
                result = Convert.ToByte(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// int型 (32 ビット符号付き整数) に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>int型 (32 ビット符号付き整数) の値</returns>
        public static int ToInt(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(int);
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// int型 (32 ビット符号付き整数) に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>int型 (32 ビット符号付き整数) の値</returns>
        public static int ToIntDef(object value, int defaultValue = default(int))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            int result = default(int);
            try
            {
                result = Convert.ToInt32(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// decimal型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>decimal型の値</returns>
        public static decimal ToDecimal(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(decimal);
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// decimal型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>decimal型の値</returns>
        public static decimal ToDecimalDef(object value, decimal defaultValue = default(decimal))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            decimal result = default(decimal);
            try
            {
                result = Convert.ToDecimal(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// double型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>double型の値</returns>
        public static double ToDouble(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(double);
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// double型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>double型の値</returns>
        public static double ToDoubleDef(object value, double defaultValue = default(double))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            double result = default(double);
            try
            {
                result = Convert.ToDouble(value);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// float型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>float型の値</returns>
        public static float ToFloat(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(float);
            }

            try
            {
                return float.Parse(value.ToString());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// float型に変換します。
        /// 変換できない場合、指定したデフォルト値を返します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>float型の値</returns>
        public static float ToFloatDef(object value, float defaultValue = default(float))
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            float result = default(float);
            try
            {
                result = float.Parse(value.ToString());
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// bool型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>bool型の値</returns>
        public static bool ToBoolean(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            try
            {
                Type t = value.GetType();
                if (t.Equals(typeof(char)))
                {
                    return Convert.ToBoolean(ToCharDef(value));
                }
                else if (t.Equals(typeof(byte)))
                {
                    return Convert.ToBoolean(ToByteDef(value));
                }
                else if (t.Equals(typeof(int)))
                {
                    return Convert.ToBoolean(ToIntDef(value));
                }
                else if (t.Equals(typeof(decimal)))
                {
                    return Convert.ToBoolean(ToDecimalDef(value));
                }
                else if (t.Equals(typeof(double)))
                {
                    return Convert.ToBoolean(ToDoubleDef(value));
                }
                else if (t.Equals(typeof(float)))
                {
                    return Convert.ToBoolean(ToFloatDef(value));
                }
                else
                {
                    if (CValidate.IsToDecimal(value))
                    {
                        return Convert.ToBoolean(ToDecimalDef(value));
                    }
                }

                return Convert.ToBoolean(value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DateTime型に変換します。
        /// </summary>
        /// <param name="value">変換するオブジェクト</param>
        /// <returns>DateTime型の値</returns>
        public static DateTime ToDateTime(object value)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return default(DateTime);
            }

            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
    #endregion



    #region コントロール管理クラス
    #region フォームレスアプリケーションクラス
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
        /// false: 処理を終了する
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
        static class SafeNativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct POINT
            {
                public int x;
                public int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct MSG
            {
                public IntPtr hwnd;
                public int message;
                public IntPtr wParam;
                public IntPtr lParam;
                public int time;
                public POINT pt;
            }

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PeekMessage(
                out MSG msg, IntPtr hWnd,
                int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);
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
    #endregion
    #endregion
}
