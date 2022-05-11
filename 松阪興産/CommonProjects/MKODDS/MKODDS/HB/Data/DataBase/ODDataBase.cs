using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using MKClass.MKFile;

namespace MKODDS.HB.Data.DataBase
{
    /// <summary>
    /// [販売大臣] ODDS データベース操作クラス (abstract)
    /// </summary>
    abstract public class HBODDataBase : HBODData
    {
        #region 列挙型
        /// <summary>
        /// RDBMSの種類を示します。
        /// </summary>
        public enum RDBMS : int
        {
            /// <summary>SQLServer</summary>
            SQLServer = 1,
            /// <summary>DB2 (IBM AS/400)</summary>
            DB2 = 2
        }
        #endregion

        #region ストアドプロシージャの変数定義クラス
        /// <summary>
        /// ストアドプロシージャの変数定義クラス
        /// </summary>
        /// <remarks>
        /// データベースエンジンに応じて拡張が必要となります。
        /// </remarks>
        public class Declare
        {
            /// <summary>変数名</summary>
            public string Variable = "";

            /// <summary>データ型 (SQLServer)</summary>
            public SqlDbType DbType = SqlDbType.NVarChar;

            /// <summary>
            /// コンストラクタ (SQLServer)
            /// </summary>
            /// <param name="variable">変数名</param>
            /// <param name="dbType">データ型</param>
            public Declare(string variable, SqlDbType dbType)
            {
                this.Variable = variable;
                this.DbType = dbType;
            }
        }
        #endregion

        #region プロパティ
        /// <summary>設定ファイルのパス</summary>
        protected static string CONFIG_PATH { get { return Path.Combine(CFile.DLLParentPath, "App.config"); } }

        /// <summary>データベース接続エラー</summary>
        protected static string UNABLE_TO_CONNECT { get { return "%sに接続できません。サーバを確認してください。"; } }

        /// <summary>接続文字列</summary>
        public string ConnectionString { get; protected set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HBODDataBase()
            : base()
        {
            try
            {
                this.ConnectionString = "";
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 接続文字列のセット (abstract)
        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        abstract public void SetConnectionString();

        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        abstract public void SetConnectionString(string systemName);

        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        abstract public void SetConnectionString(string configPath, string systemName);
        #endregion

        #region クエリ実行 (abstract)
        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        abstract public DataTable Select(string connectionString,
                                         string q,
                                         Dictionary<Declare, dynamic> values = null);

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        abstract public bool ExecuteNonQuery(string connectionString,
                                             string q,
                                             Dictionary<Declare, dynamic> values = null,
                                             bool doTransaction = true);

        /// <summary>
        /// クエリ、またはストアドプロシージャを実行します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        abstract public bool ExecuteNonQueryAsync(string connectionString,
                                                  string q,
                                                  Dictionary<Declare, dynamic> values = null,
                                                  bool doTransaction = true);
        #endregion

        #region メッセージ取得
        /// <summary>
        /// データベース接続エラー時のメッセージを取得します。
        /// </summary>
        /// <returns>データベース接続エラー時のメッセージ</returns>
        protected string GetUnableToConnectMessage()
        {
            try
            {
                var unableToConnectMessage = ODMessage.Message("E", "2");
                return ((!String.IsNullOrEmpty(unableToConnectMessage))
                    ? unableToConnectMessage
                    : UNABLE_TO_CONNECT
                    ).Replace("%s", base.ObjOdds.strCurrentDatabase);
            }
            catch
            {

            }

            return "";
        }
        #endregion
    }
}
