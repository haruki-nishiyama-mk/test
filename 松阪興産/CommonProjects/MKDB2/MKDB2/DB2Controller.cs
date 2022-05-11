using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2.iSeries;
using MKClass.MKDataBase;

namespace MKDB2
{
    /// <summary>
    /// ストアドプロシージャの変数定義クラス (DB2 (IBM AS/400))
    /// </summary>
    public class DB2Declare
    {
        #region プロパティ
        /// <summary>変数名</summary>
        public string Variable = "";
        /// <summary>データ型</summary>
        public iDB2DbType DbType = iDB2DbType.iDB2VarChar;
        /// <summary>値</summary>
        public object Value = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ (DB2 (IBM AS/400))
        /// </summary>
        /// <param name="variable">変数名</param>
        /// <param name="dbType">データ型</param>
        /// <param name="value">値</param>
        public DB2Declare(string variable, iDB2DbType dbType, object value)
        {
            this.Variable = variable;
            this.DbType = dbType;
            this.Value = value;
        }
        #endregion
    }



    /// <summary>
    /// DB2 (IBM AS/400) 制御クラス
    /// </summary>
    public class DB2Controller : IDisposable
    {
        #region 内部クラス
        /// <summary>
        /// トランザクション管理クラス
        /// </summary>
        public class TransactionManager : IDisposable
        {
            #region 変数
            private DB2Controller _parent = null;
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="parent">DB2 (IBM AS/400) 制御オブジェクト</param>
            /// <param name="beginTransaction">トランザクションの開始有無</param>
            public TransactionManager(DB2Controller parent, bool beginTransaction = true)
            {
                _parent = parent;

                if (beginTransaction)
                {
                    BeginTransaction();
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
                        if (_parent.Transaction != null)
                        {
                            RollBack();
                        }

                        _parent.Transaction = null;
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
            ~TransactionManager()
            {
                Dispose(false);
            }
            #endregion

            #region メソッド
            /// <summary>
            /// トランザクションをデータベースに対してコミットします。
            /// </summary>
            public void BeginTransaction()
            {
                try
                {
                    _parent.OpenConnection();

                    _parent.Transaction = _parent.Connection.BeginTransaction();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// トランザクションをデータベースに対してコミットします。
            /// </summary>
            public void Commit()
            {
                try
                {
                    if (_parent.Transaction != null)
                    {
                        _parent.Transaction.Commit();
                        _parent.CloseConnection();

                        _parent.Transaction = null;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// トランザクションを保留状態からロールバックします。
            /// </summary>
            public void RollBack()
            {
                try
                {
                    if (_parent.Transaction != null)
                    {
                        _parent.Transaction.Rollback();
                        _parent.CloseConnection();

                        _parent.Transaction = null;
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

        #region プロパティ
        /// <summary>IBM DB2 for i データソースへの接続に対するオブジェクト</summary>
        public iDB2Connection Connection { get; private set; }
        /// <summary>IBM DB2 for i データソースに対する変更をコミットまたはロールバックするために使用できるオブジェクト</summary>
        public iDB2Transaction Transaction { get; private set; }

        /// <summary>接続文字列</summary>
        public string ConnectionString { private get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DB2Controller()
            : this(0)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        public DB2Controller(int defaultCollectionNo)
            : this("", defaultCollectionNo)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        public DB2Controller(string systemName, int defaultCollectionNo)
            : this("", systemName, defaultCollectionNo)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="defaultCollectionNo">接続ライブラリ番号</param>
        public DB2Controller(string configPath, string systemName, int defaultCollectionNo)
        {
            this.Connection = new iDB2Connection();
            this.Transaction = null;

            this.ConnectionString = (defaultCollectionNo > 0)
                ? CDB2.CreateDb2Connection(configPath, systemName, defaultCollectionNo)
                : "";
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
                    this.Transaction = null;
                    this.Connection = null;
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
        ~DB2Controller()
        {
            Dispose(false);
        }
        #endregion

        #region コネクション制御
        /// <summary>
        /// データソースへの接続を開きます。
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                if (this.Connection != null && this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.ConnectionString = this.ConnectionString;
                    this.Connection.Open();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// データソースへの接続を閉じます。
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (this.Connection != null && this.Connection.State == ConnectionState.Open)
                {
                    this.Connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region クエリ実行
        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public DataTable Select(string q, List<DB2Declare> values = null)
        {
            try
            {
                DataTable dt = new DataTable();

                using (var command = new iDB2Command(q, this.Connection))
                {
                    try
                    {
                        OpenConnection();

                        if (values != null)
                        {
                            foreach (var v in values)
                            {
                                command.Parameters.Add(v.Variable, v.DbType).Value = v.Value;
                            }
                        }

                        var adapter = new iDB2DataAdapter(command);
                        adapter.Fill(dt);

                        CDataBase.TrimData(dt);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>影響を与えた行数</returns>
        public int ExecuteNonQuery(string q, List<DB2Declare> values = null)
        {
            int ret = 0;
            try
            {
                using (var command = new iDB2Command(q, this.Connection))
                {
                    try
                    {
                        if (this.Transaction == null)
                        {
                            OpenConnection();
                        }

                        command.Transaction = (this.Transaction != null) ? this.Transaction : null;

                        if (values != null)
                        {
                            foreach (var v in values)
                            {
                                command.Parameters.Add(v.Variable, v.DbType).Value = v.Value;
                            }
                        }

                        ret = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (this.Transaction == null)
                        {
                            CloseConnection();
                        }
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// クエリを実行します。
        /// </summary>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数 (≒バルク処理)</param>
        /// <returns>影響を与えた行数</returns>
        public int ExecuteNonQuery(string q, List<List<DB2Declare>> values)
        {
            int ret = 0;
            try
            {
                using (var command = new iDB2Command(q, this.Connection))
                {
                    try
                    {
                        if (this.Transaction == null)
                        {
                            OpenConnection();
                        }

                        command.Transaction = (this.Transaction != null) ? this.Transaction : null;

                        if (values != null)
                        {
                            foreach (var vs in values)
                            {
                                command.Parameters.Clear();
                                foreach (var v in vs)
                                {
                                    command.Parameters.Add(v.Variable, v.DbType).Value = v.Value;
                                }

                                ret += command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (this.Transaction == null)
                        {
                            CloseConnection();
                        }
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
