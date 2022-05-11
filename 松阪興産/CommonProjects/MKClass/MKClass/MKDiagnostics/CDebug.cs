using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using MKClass.MKFile;
using MKClass.MKData;

namespace MKClass.MKDiagnostics
{
    /// <summary>
    /// デバッグ管理クラス
    /// </summary>
    public class CDebug
    {
        #region プロパティ
        /// <summary>設定ファイルのパス</summary>
        protected static string CONFIG_PATH { get { return Path.Combine(CFile.DLLParentPath, "App.config"); } }
        #endregion

        /// <summary>
        /// デバッグかどうかを取得します。
        /// </summary>
        /// <returns>
        /// true : デバッグON
        /// false: それ以外
        /// </returns>
        public static bool IsDebug()
        {
            return IsDebug("");
        }

        /// <summary>
        /// デバッグかどうかを取得します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : デバッグON
        /// false: それ以外
        /// </returns>
        public static bool IsDebug(string systemName)
        {
            return IsDebug("", systemName);
        }

        /// <summary>
        /// デバッグかどうかを取得します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : デバッグON
        /// false: それ以外
        /// </returns>
        public static bool IsDebug(string configPath, string systemName)
        {
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return false;
                }

                XDocument xDoc = XDocument.Load(configPath);

                return CConvert.ToBoolean(xDoc.Root.Element("debug").Attribute("on").Value);
            }
            catch
            {
                return false;
            }
        }
    }
}
