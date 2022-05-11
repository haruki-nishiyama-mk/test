using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using MKClass.MKException;
using MKClass.MKFile;
using MKClass.MKData;
using MKClass.MKControl;

namespace MKClass.MKDataBase
{
    /// <summary>
    /// SQLServer操作クラス
    /// </summary>
    public sealed class CSQLServer : CDataBase
    {
        #region public
        #region 内部クラス
        /// <summary>
        /// 接続文字列情報クラス
        /// </summary>
        public class ConnectionInfo : IDisposable
        {
            #region プロパティ
            /// <summary>データソース</summary>
            public string DataSource { get; private set; }
            /// <summary>データベース</summary>
            public string DataBase { get; private set; }
            /// <summary>認証フラグ</summary>
            public bool IntegratedSecurity { get; private set; }
            /// <summary>ユーザーID</summary>
            public string UserId { get; private set; }
            /// <summary>パスワード</summary>
            public string Password { get; private set; }
            /// <summary>接続タイムアウト値</summary>
            public int ConnectTimeout { get; private set; }
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ConnectionInfo()
                : this("")
            {

            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="systemName">接続先システム名</param>
            public ConnectionInfo(string systemName)
                : this("", systemName)
            {

            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="configPath">設定ファイルのパス</param>
            /// <param name="systemName">接続先システム名</param>
            public ConnectionInfo(string configPath, string systemName)
            {
                try
                {
                    InitProperties();

                    if (String.IsNullOrEmpty(configPath))
                    {
                        configPath = CDataBase.CONFIG_PATH;
                    }

                    if (!File.Exists(configPath))
                    {
                        return;
                    }

                    XDocument xDoc = XDocument.Load(configPath);

                    var elSqlServer = (
                            from x in xDoc.Root.Elements("system")
                            where x.Attribute("name").Value == systemName
                            select x.Element("sqlServer")
                        ).SingleOrDefault();
                    if (elSqlServer == null)
                    {
                        return;
                    }

                    if (elSqlServer.Attribute("dataSource") != null)
                    {
                        this.DataSource = elSqlServer.Attribute("dataSource").Value;
                    }
                    if (elSqlServer.Attribute("database") != null)
                    {
                        this.DataBase = elSqlServer.Attribute("database").Value;
                    }
                    if (elSqlServer.Attribute("integratedSecurity") != null)
                    {
                        this.IntegratedSecurity = CConvert.ToBoolean(elSqlServer.Attribute("integratedSecurity").Value);
                    }
                    if (!this.IntegratedSecurity)
                    {
                        if (elSqlServer.Attribute("userId") != null)
                        {
                            this.UserId = elSqlServer.Attribute("userId").Value;
                        }
                        if (elSqlServer.Attribute("password") != null)
                        {
                            this.Password = elSqlServer.Attribute("password").Value;
                        }
                    }

                    this.ConnectTimeout = (elSqlServer.Attribute("connectTimeout") != null)
                        ? CConvert.ToIntDef(elSqlServer.Attribute("connectTimeout").Value, 5)
                        : 5;
                }
                catch
                {
                    throw;
                }
            }
            #endregion

            #region IDisposable Support
            // Track whether Dispose has been called.
            private bool _disposed = false;

            /// <summary>
            /// リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
            /// </summary>
            /// <param name="disposing">マネージドリソースの解放有無</param>
            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // TODO: Dispose managed resources here.
                    }

                    // Note disposing has been done.
                    _disposed = true;
                }
            }
            #endregion

            #region デストラクタ
            /// <summary>
            /// デストラクタ
            /// </summary>
            ~ConnectionInfo()
            {
                Dispose(false);
            }
            #endregion

            #region メソッド
            /// <summary>
            /// プロパティを初期化します。
            /// </summary>
            private void InitProperties()
            {
                this.DataSource = "";
                this.DataBase = "";
                this.IntegratedSecurity = false;
                this.UserId = "";
                this.Password = "";
                this.ConnectTimeout = 5;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// SQLServerの接続文字列を作成します。
        /// </summary>
        /// <returns>SQLServerの接続文字列</returns>
        public static string CreateSqlConnection()
        {
            try
            {
                return CreateSqlConnection("");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// SQLServerの接続文字列を作成します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>SQLServerの接続文字列</returns>
        public static string CreateSqlConnection(string systemName)
        {
            try
            {
                return CreateSqlConnection("", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// SQLServerの接続文字列を作成します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>SQLServerの接続文字列</returns>
        public static string CreateSqlConnection(string configPath, string systemName)
        {
            try
            {
                using (ConnectionInfo ci = new ConnectionInfo(configPath, systemName))
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = ci.DataSource;
                    builder.InitialCatalog = ci.DataBase;
                    builder.IntegratedSecurity = ci.IntegratedSecurity;
                    builder.UserID = ci.UserId;
                    builder.Password = ci.Password;
                    builder.ConnectTimeout = ci.ConnectTimeout;

                    return builder.ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ESCAPE句を取得します。
        /// </summary>
        /// <param name="escape">ESCAPE文字 ['#']</param>
        /// <returns>ESCAPE句</returns>
        public static string GetEscapeClause(char escape = '#')
        {
            try
            {
                return String.Format(" ESCAPE '{0}' ", escape);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// LIKE述語に適した文字列にエスケープします。
        /// </summary>
        /// <param name="value">文字列</param>
        /// <param name="lp">LIKE述語のパターン</param>
        /// <param name="escape">ESCAPE文字 ['#']</param>
        /// <returns>エスケープ後の文字列</returns>
        public static string GetEscapeString(string value, LikePattern lp = LikePattern.PartialMatch, char escape = '#')
        {
            try
            {
                string ret = Regex.Replace(value, "[_%\\[" + escape + "]", escape + "$0");
                switch (lp)
                {
                    case LikePattern.PrefixMatch:
                        ret = ret + "%";

                        break;
                    case LikePattern.SuffixMatch:
                        ret = "%" + ret;

                        break;
                    case LikePattern.PartialMatch:
                        ret = "%" + ret + "%";

                        break;
                }

                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データベースの接続確認を行います。
        /// </summary>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool CheckDatabaseConnection()
        {
            try
            {
                return CheckDatabaseConnection("");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データベースの接続確認を行います。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool CheckDatabaseConnection(string systemName)
        {
            try
            {
                return CheckDatabaseConnection("", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データベースの接続確認を行います。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool CheckDatabaseConnection(string configPath, string systemName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CSQLServer.CreateSqlConnection(configPath, systemName)))
                {
                    connection.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new CDataBaseException(
                    CDataBase.UNABLE_TO_CONNECT.Replace("%s", (!String.IsNullOrEmpty(systemName)) ? systemName : "データベース"),
                    ex,
                    ex.Message);
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public static DataTable Select(string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                return Select("", q, values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public static DataTable Select(string systemName, string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                return Select("", systemName, q, values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public static DataTable Select(string configPath, string systemName, string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                DataTable dt = new DataTable();

                using (var connection = new SqlConnection(CreateSqlConnection(configPath, systemName)))
                using (var command = new SqlCommand(q, connection))
                {
                    try
                    {
                        try
                        {
                            connection.Open();
                        }
                        catch (SqlException ex)
                        {
                            throw new CDataBaseException(
                                CDataBase.UNABLE_TO_CONNECT.Replace("%s", (!String.IsNullOrEmpty(systemName)) ? systemName : "データベース"),
                                ex,
                                ex.Message);
                        }
                        catch
                        {
                            throw;
                        }

                        if (values != null)
                        {
                            foreach (var v in values)
                            {
                                command.Parameters.Add(v.Key.Variable, v.Key.DbType).Value = v.Value;
                            }
                        }

                        try
                        {
                            var adapter = new SqlDataAdapter(command);
                            adapter.Fill(dt);
                        }
                        catch (SqlException ex)
                        {
                            throw new CDataBaseException(ex.Message, ex, "", GetQueryCommand(command));
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行し、クエリによって返される結果セットの先頭行の最初の列を返します。
        /// その他の列または行は無視されます。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (object)</returns>
        public static object ExecuteScalar(string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                return ExecuteScalar("", q, values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行し、クエリによって返される結果セットの先頭行の最初の列を返します。
        /// その他の列または行は無視されます。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (object)</returns>
        public static object ExecuteScalar(string systemName, string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                return ExecuteScalar("", systemName, q, values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行し、クエリによって返される結果セットの先頭行の最初の列を返します。
        /// その他の列または行は無視されます。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (object)</returns>
        public static object ExecuteScalar(string configPath, string systemName, string q, Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                object obj = null;

                using (var connection = new SqlConnection(CreateSqlConnection(configPath, systemName)))
                using (var command = new SqlCommand(q, connection))
                {
                    try
                    {
                        try
                        {
                            connection.Open();
                        }
                        catch (SqlException ex)
                        {
                            throw new CDataBaseException(
                                CDataBase.UNABLE_TO_CONNECT.Replace("%s", (!String.IsNullOrEmpty(systemName)) ? systemName : "データベース"),
                                ex,
                                ex.Message);
                        }
                        catch
                        {
                            throw;
                        }

                        if (values != null)
                        {
                            foreach (var v in values)
                            {
                                command.Parameters.Add(v.Key.Variable, v.Key.DbType).Value = v.Value;
                            }
                        }

                        try
                        {
                            obj = command.ExecuteScalar();
                        }
                        catch (SqlException ex)
                        {
                            throw new CDataBaseException(ex.Message, ex, "", GetQueryCommand(command));
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return obj;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQuery(string q,
                                           Dictionary<Declare, dynamic> values = null,
                                           bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQuery("", q, values, doTransaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQuery(string systemName,
                                           string q,
                                           Dictionary<Declare, dynamic> values = null,
                                           bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQuery("", systemName, q, values, doTransaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQuery(string configPath,
                                           string systemName,
                                           string q,
                                           Dictionary<Declare, dynamic> values = null,
                                           bool doTransaction = true)
        {
            int ret = -1;
            try
            {
                using (TransactionScope scope = new TransactionScope((doTransaction) ? TransactionScopeOption.Required : TransactionScopeOption.Suppress))
                {
                    using (var connection = new SqlConnection(CreateSqlConnection(configPath, systemName)))
                    using (var command = new SqlCommand(q, connection))
                    {
                        try
                        {
                            try
                            {
                                connection.Open();
                            }
                            catch (SqlException ex)
                            {
                                throw new CDataBaseException(
                                    CDataBase.UNABLE_TO_CONNECT.Replace("%s", (!String.IsNullOrEmpty(systemName)) ? systemName : "データベース"),
                                    ex,
                                    ex.Message);
                            }
                            catch
                            {
                                throw;
                            }

                            if (values != null)
                            {
                                foreach (var v in values)
                                {
                                    command.Parameters.Add(v.Key.Variable, v.Key.DbType).Value = v.Value;
                                }
                            }

                            try
                            {
                                ret = command.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new CDataBaseException(ex.Message, ex, "", GetQueryCommand(command));
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                    scope.Complete();
                }

                return (ret > 0) ? true : false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリ、またはストアドプロシージャを実行します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQueryAsync(string q,
                                                Dictionary<Declare, dynamic> values = null,
                                                bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQueryAsync("", q, values, doTransaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリ、またはストアドプロシージャを実行します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQueryAsync(string systemName,
                                                string q,
                                                Dictionary<Declare, dynamic> values = null,
                                                bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQueryAsync("", systemName, q, values, doTransaction);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// クエリ、またはストアドプロシージャを実行します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: それ以外
        /// </returns>
        public static bool ExecuteNonQueryAsync(string configPath,
                                                string systemName,
                                                string q,
                                                Dictionary<Declare, dynamic> values = null,
                                                bool doTransaction = true)
        {
            int ret = -1;
            try
            {
                using (TransactionScope scope = new TransactionScope((doTransaction) ? TransactionScopeOption.Required : TransactionScopeOption.Suppress))
                {
                    using (var connection = new SqlConnection(CreateSqlConnection(configPath, systemName)))
                    using (var command = new SqlCommand(q, connection))
                    {
                        try
                        {
                            try
                            {
                                connection.Open();
                            }
                            catch (SqlException ex)
                            {
                                throw new CDataBaseException(
                                    CDataBase.UNABLE_TO_CONNECT.Replace("%s", (!String.IsNullOrEmpty(systemName)) ? systemName : "データベース"),
                                    ex,
                                    ex.Message);
                            }
                            catch
                            {
                                throw;
                            }

                            if (values != null)
                            {
                                foreach (var v in values)
                                {
                                    command.Parameters.Add(v.Key.Variable, v.Key.DbType).Value = v.Value;
                                }
                            }

                            try
                            {
                                IAsyncResult r = command.BeginExecuteNonQuery();
                                while (!r.IsCompleted)
                                {

                                }
                                ret = command.EndExecuteNonQuery(r);
                            }
                            catch (SqlException ex)
                            {
                                throw new CDataBaseException(ex.Message, ex, "", GetQueryCommand(command));
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                    scope.Complete();
                }

                return (ret > 0) ? true : false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// プレースホルダの中身を置き換えて、実行クエリを取得します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行クエリ</returns>
        public static string GetQueryCommand(string q,
                                             Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                using (var command = new SqlCommand(q))
                {
                    if (values != null)
                    {
                        foreach (var v in values)
                        {
                            command.Parameters.Add(v.Key.Variable, v.Key.DbType).Value = v.Value;
                        }
                    }

                    return GetQueryCommand(command);
                }
            }
            catch
            {

            }

            return "";
        }

        /// <summary>
        /// プレースホルダの中身を置き換えて、実行クエリを取得します。
        /// </summary>
        /// <param name="sc">クエリを格納したSqlCommand</param>
        /// <returns>実行クエリ</returns>
        public static string GetQueryCommand(SqlCommand sc)
        {
            StringBuilder q = new StringBuilder();
            try
            {
                if (sc.Parameters.Count > 0)
                {
                    foreach (SqlParameter p in sc.Parameters)
                    {
                        q.AppendLine("DECLARE" + " " + p.ParameterName + " " + p.SqlDbType.ToString() + " = " + CConvert.EncloseSingleQuotes(p.Value.ToString()) + ";");
                    }
                    q.AppendLine();
                }
                q.AppendLine(sc.CommandText);
            }
            catch
            {

            }

            return q.ToString();
        }

        /// <summary>
        /// リンクサーバーのリストを取得します。
        /// </summary>
        /// <returns>リンクサーバーのリスト</returns>
        public static IEnumerable<XElement> GetLinkServerList()
        {
            try
            {
                return GetLinkServerList("");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// リンクサーバーのリストを取得します。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>リンクサーバーのリスト</returns>
        public static IEnumerable<XElement> GetLinkServerList(string systemName)
        {
            try
            {
                return GetLinkServerList("", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// リンクサーバーのリストを取得します。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>リンクサーバーのリスト</returns>
        public static IEnumerable<XElement> GetLinkServerList(string configPath, string systemName)
        {
            try
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = CDataBase.CONFIG_PATH;
                }

                if (!File.Exists(configPath))
                {
                    return null;
                }

                XDocument xDoc = XDocument.Load(configPath);

                return (
                        from x in xDoc.Root.Elements("system")
                        where x.Attribute("name").Value == systemName
                        select x.Element("sqlServer").Element("linkServer")
                    ).OrderBy(x => x.Element("row").Attribute("id"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// m_messageテーブルからメッセージ (message) を取得します。
        /// </summary>
        /// <param name="messageId">メッセージID (message_id)</param>
        /// <returns>メッセージ (message)</returns>
        /// <remarks>
        /// プロジェクトでこのテーブルを作成した場合のみ、使用できます。
        /// </remarks>
        public static string GetMessage(string messageId)
        {
            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                q.AppendLine("  message");
                q.AppendLine("FROM");
                q.AppendLine("  m_message");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine(String.Format("  AND message_id = '{0}'", messageId));
                using (DataTable dt = Select(q.ToString()))
                {
                    if (!dt.IsNullOrEmpty())
                    {
                        return dt.Rows[0].GetData<string>("message");
                    }
                }

                return "";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// m_postcodeテーブルから住所 (address) を取得します。
        /// </summary>
        /// <param name="postalCode">郵便番号 (post_code)</param>
        /// <returns>住所 (address)</returns>
        /// <remarks>
        /// プロジェクトでこのテーブルを作成した場合のみ、使用できます。
        /// </remarks>
        public static string GetAddress(string postalCode)
        {
            if (!CValidate.IsPostalCodeMatch(postalCode))
            {
                return "";
            }

            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                q.AppendLine("  address");
                q.AppendLine("FROM");
                q.AppendLine("  m_postcode");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine(String.Format("  AND post_code = '{0}'", postalCode));
                using (DataTable dt = Select(q.ToString()))
                {
                    if (!dt.IsNullOrEmpty())
                    {
                        return dt.Rows[0].GetData<string>("address");
                    }
                }

                return "";
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
