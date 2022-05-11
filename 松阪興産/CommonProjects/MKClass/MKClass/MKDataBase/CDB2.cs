using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using MKClass.MKFile;
using MKClass.MKData;

namespace MKClass.MKDataBase
{
    /// <summary>
    /// DB2 (IBM AS/400) 操作クラス
    /// </summary>
    public sealed class CDB2 : CDataBase
    {
        #region public
        /// <summary>
        /// DB2 (IBM AS/400) の接続文字列を作成します。
        /// </summary>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        /// <returns>DB2 (IBM AS/400) の接続文字列</returns>
        public static string CreateDb2Connection(int defaultCollectionNo)
        {
            try
            {
                return CreateDb2Connection("", defaultCollectionNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DB2 (IBM AS/400) の接続文字列を作成します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        /// <returns>DB2 (IBM AS/400) の接続文字列</returns>
        public static string CreateDb2Connection(string systemName, int defaultCollectionNo)
        {
            try
            {
                return CreateDb2Connection("", systemName, defaultCollectionNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DB2 (IBM AS/400) の接続文字列を作成します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        /// <returns>DB2 (IBM AS/400) の接続文字列</returns>
        public static string CreateDb2Connection(string configPath, string systemName, int defaultCollectionNo)
        {
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CDataBase.CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return "";
                }

                XDocument xDoc = XDocument.Load(configPath);

                var elDb2 = (
                        from x in xDoc.Root.Elements("system")
                        where x.Attribute("name").Value == systemName
                        select x.Element("db2")
                    ).SingleOrDefault();
                if (elDb2 != null)
                {
                    string dataSource = elDb2.Attribute("dataSource").Value;
                    string userId = elDb2.Attribute("userId").Value;
                    string password = elDb2.Attribute("password").Value;
                    string defaultCollection = (
                            from x in elDb2.Elements("defaultCollection").Elements("row")
                            where x.Attribute("id").Value == defaultCollectionNo.ToString()
                            select x
                        ).SingleOrDefault().Value;

                    return "DATASOURCE=" + dataSource + ";DefaultCollection=" + defaultCollection + ";USERID=" + userId + ";PASSWORD=" + password;
                }

                return "";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データソース (IPアドレス) を取得します。
        /// </summary>
        /// <returns>データソース (IPアドレス)</returns>
        public static string GetDatasource()
        {
            try
            {
                return GetDatasource("");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データソース (IPアドレス) を取得します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>データソース (IPアドレス)</returns>
        public static string GetDatasource(string systemName)
        {
            try
            {
                return GetDatasource("", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データソース (IPアドレス) を取得します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>データソース (IPアドレス)</returns>
        public static string GetDatasource(string configPath, string systemName)
        {
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CDataBase.CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return "";
                }

                XDocument xDoc = XDocument.Load(configPath);

                return (
                        from x in xDoc.Root.Elements("system")
                        where x.Attribute("name").Value == systemName
                        select x.Element("db2")
                    ).SingleOrDefault().Attribute("dataSource").Value;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
