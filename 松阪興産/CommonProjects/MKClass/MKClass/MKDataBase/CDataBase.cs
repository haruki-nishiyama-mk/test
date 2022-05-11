using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MKClass.MKFile;
using MKClass.MKControl;

namespace MKClass.MKDataBase
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

    /// <summary>
    /// LIKE述語のパターンを示します。
    /// </summary>
    public enum LikePattern
    {
        /// <summary>完全一致</summary>
        ExactMatch,
        /// <summary>前方一致</summary>
        PrefixMatch,
        /// <summary>後方一致</summary>
        SuffixMatch,
        /// <summary>部分一致</summary>
        PartialMatch
    }
    #endregion

    /// <summary>
    /// データベース操作クラス (abstract)
    /// </summary>
    abstract public class CDataBase
    {
        #region 内部クラス
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
        #endregion

        #region プロパティ
        /// <summary>設定ファイルのパス</summary>
        protected static string CONFIG_PATH { get { return Path.Combine(CFile.DLLParentPath, "App.config"); } }

        /// <summary>データベース接続エラー</summary>
        protected static string UNABLE_TO_CONNECT { get { return "%sに接続できません。サーバを確認してください。"; } }
        #endregion

        #region Trim
        /// <summary>
        /// データの先頭および末尾にある空白文字をすべて削除します。
        /// </summary>
        /// <param name="dt">対象データ (DataTable)</param>
        public static void TrimData(DataTable dt)
        {
            if (dt.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                foreach (var c in dt.Columns.Cast<DataColumn>().Where(c => c.DataType == typeof(string)).ToArray())
                {
                    dt.Rows.Cast<DataRow>().ToList().ForEach(r => r.SetField<string>(c, r[c.ColumnName].ToString().Trim()));
                }

                dt.AcceptChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
