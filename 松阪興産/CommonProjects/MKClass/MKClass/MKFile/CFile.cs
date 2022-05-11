using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MKClass.MKFile
{
    /// <summary>
    /// ファイル操作クラス (abstract)
    /// </summary>
    abstract public class CFile
    {
        #region プロパティ
        /// <summary>設定ファイルのパス</summary>
        protected static string CONFIG_PATH { get { return Path.Combine(DLLParentPath, "App.config"); } }

        /// <summary>自身 (このdll) のフルパス</summary>
        public static string DLLPath { get { return GetPathOfDLL(); } }
        /// <summary>自身 (このdll) の親ディレクトリのフルパス</summary>
        public static string DLLParentPath { get { return GetParentPathOfDLL(); } }
        #endregion

        #region ディレクトリ操作
        /// <summary>
        /// 指定したパス文字列が絶対パスかどうかを判断します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>
        /// true : 絶対パス
        /// false: それ以外
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
        /// false: それ以外
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

        /// <summary>
        /// 自身 (このdll) のフルパスを取得します。
        /// </summary>
        /// <returns>自身 (このdll) のフルパス</returns>
        /// <remarks>
        /// Assembly.GetExecutingAssemblyメソッドはdll内で呼び出すと、
        /// そのdllを表すAssemblyを返します。
        /// </remarks>
        private static string GetPathOfDLL()
        {
            try
            {
                return Assembly.GetExecutingAssembly().Location;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 自身 (このdll) の親ディレクトリのフルパスを取得します。
        /// </summary>
        /// <returns>自身 (このdll) の親ディレクトリのフルパス</returns>
        private static string GetParentPathOfDLL()
        {
            try
            {
                return Directory.GetParent(GetPathOfDLL()).FullName;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// アプリケーションデータを格納するディレクトリのフルパスを取得します。
        /// </summary>
        /// <returns>アプリケーションデータを格納するディレクトリのフルパス</returns>
        public static string GetDataPath()
        {
            return GetDataPath("");
        }

        /// <summary>
        /// アプリケーションデータを格納するディレクトリのフルパスを取得します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>アプリケーションデータを格納するディレクトリのフルパス</returns>
        public static string GetDataPath(string systemName)
        {
            return GetDataPath("", systemName);
        }

        /// <summary>
        /// アプリケーションデータを格納するディレクトリのフルパスを取得します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>アプリケーションデータを格納するディレクトリのフルパス</returns>
        public static string GetDataPath(string configPath, string systemName)
        {
            string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string defaultPath = Path.GetFullPath(Path.Combine(appDataLocal, "Manac", "Data"));
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return defaultPath;
                }

                XDocument xDoc = XDocument.Load(configPath);

                var dataPath = (
                        from x in xDoc.Root.Elements("system")
                        where x.Attribute("name").Value == systemName
                        select x.Element("data")
                    ).SingleOrDefault().Attribute("path").Value;

                return (IsAbsolatePath(dataPath)) ? Path.GetFullPath(dataPath) : Path.GetFullPath(appDataLocal + dataPath);
            }
            catch
            {
                return defaultPath;
            }
        }
        #endregion

        #region ファイル操作
        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        /// <param name="path">読み込むファイルパス</param>
        /// <param name="encode">エンコード (null = Shift_JIS)</param>
        /// <returns>読み込んだファイルの中身</returns>
        public static string ReadFile(string path, Encoding encode = null)
        {
            string ret = "";
            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                using (StreamReader sr = new StreamReader(path, encode))
                {
                    ret = sr.ReadToEnd();
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
        /// <param name="encode">エンコード (null = Shift_JIS)</param>
        public static void WriteFile(string path, string value, bool append = true, bool newLine = true, Encoding encode = null)
        {
            try
            {
                if (encode == null)
                {
                    encode = Encoding.GetEncoding("Shift_JIS");
                }

                CreateDirectorySafely(Path.GetDirectoryName(path));

                using (StreamWriter sw = new StreamWriter(path, append, encode))
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
        /// false: それ以外
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
        /// false: それ以外
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
}
