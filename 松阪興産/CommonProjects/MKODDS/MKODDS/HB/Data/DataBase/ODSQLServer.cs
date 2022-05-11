using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using MKClass;
using MKClass.MKException;
using MKClass.MKData;
using MKClass.MKDataBase;

namespace MKODDS.HB.Data.DataBase
{
    /// <summary>
    /// [販売大臣] ODDS SQLServer操作クラス
    /// </summary>
    public sealed class HBODSQLServer : HBODDataBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HBODSQLServer()
            : base()
        {

        }
        #endregion

        #region 接続文字列のセット (override)
        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        public override void SetConnectionString()
        {
            try
            {
                SetConnectionString("");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        /// <param name="systemName">接続先システム名</param>
        public override void SetConnectionString(string systemName)
        {
            try
            {
                SetConnectionString("", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 接続文字列をセットします。
        /// </summary>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        public override void SetConnectionString(string configPath, string systemName)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(CSQLServer.CreateSqlConnection(configPath, systemName));

                // 以下、ODDSの接続情報に上書き
                if (String.IsNullOrEmpty(builder.DataSource))
                {
                    builder.DataSource = base.ObjOdds.strServerName;
                }
                if (String.IsNullOrEmpty(builder.InitialCatalog))
                {
                    builder.InitialCatalog = OHKENProductsExt.GetDataBaseName(OHKENProducts.HB, base.SelectedDataPath);
                }

                base.ConnectionString = builder.ToString();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region クエリ実行 (override)
        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public override DataTable Select(string connectionString,
                                         string q,
                                         Dictionary<Declare, dynamic> values = null)
        {
            try
            {
                DataTable dt = new DataTable();

                using (var connection = new SqlConnection(connectionString))
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
                            throw new CDataBaseException(base.GetUnableToConnectMessage(), ex, ex.Message);
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
        public override bool ExecuteNonQuery(string connectionString,
                                             string q,
                                             Dictionary<Declare, dynamic> values = null,
                                             bool doTransaction = true)
        {
            int ret = -1;
            try
            {
                using (TransactionScope scope = new TransactionScope((doTransaction) ? TransactionScopeOption.Required : TransactionScopeOption.Suppress))
                {
                    using (var connection = new SqlConnection(connectionString))
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
                                throw new CDataBaseException(base.GetUnableToConnectMessage(), ex, ex.Message);
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
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <param name="doTransaction">トランザクションの実行有無</param>
        /// <returns>
        /// true : 成功
        /// false: 失敗
        /// </returns>
        public override bool ExecuteNonQueryAsync(string connectionString,
                                                  string q,
                                                  Dictionary<Declare, dynamic> values = null,
                                                  bool doTransaction = true)
        {
            int ret = -1;
            try
            {
                using (TransactionScope scope = new TransactionScope((doTransaction) ? TransactionScopeOption.Required : TransactionScopeOption.Suppress))
                {
                    using (var connection = new SqlConnection(connectionString))
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
                                throw new CDataBaseException(base.GetUnableToConnectMessage(), ex, ex.Message);
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
        #endregion

        #region 実行クエリの取得
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
        #endregion
    }
}
