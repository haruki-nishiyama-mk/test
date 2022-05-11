using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Transactions;
using MKClass;
using MKClass.MKData;
using MKClass.MKControl;
using MKODDS.HB.Data.DataBase;

namespace MKODDS.HB.Data
{
    /// <summary>
    /// [販売大臣] ODDS APIクラス
    /// </summary>
    /// <remarks>
    /// データベース操作クラスをインスタンス化して使用します。 (未継承)
    /// </remarks>
    public class HBODAPI
    {
        /// <summary>データベース操作クラスのオブジェクト</summary>
        private HBODDataBase _db = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HBODAPI()
            : this(SetDataOptions.SetLastData)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        public HBODAPI(SetDataOptions sdo)
            : this(sdo, "")
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="systemName">接続先システム名</param>
        public HBODAPI(SetDataOptions sdo, string systemName)
            : this(sdo, systemName, HBODDataBase.RDBMS.SQLServer)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="rdbms">RDBMSの種類</param>
        public HBODAPI(SetDataOptions sdo, string systemName, HBODDataBase.RDBMS rdbms)
            : this(sdo, "", systemName, rdbms)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        public HBODAPI(SetDataOptions sdo, string configPath, string systemName)
            : this(sdo, configPath, systemName, HBODDataBase.RDBMS.SQLServer)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <param name="rdbms">RDBMSの種類</param>
        public HBODAPI(SetDataOptions sdo, string configPath, string systemName, HBODDataBase.RDBMS rdbms)
        {
            try
            {
                switch (rdbms)
                {
                    default:
                        _db = new HBODSQLServer();

                        break;
                }

                SetCurrentData(sdo, configPath, systemName);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region データ設定
        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        public bool SetCurrentData()
        {
            try
            {
                return SetCurrentData(SetDataOptions.SetLastData);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        public bool SetCurrentData(SetDataOptions sdo)
        {
            try
            {
                return SetCurrentData(sdo, "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        public bool SetCurrentData(SetDataOptions sdo, string systemName)
        {
            try
            {
                return SetCurrentData(sdo, "", systemName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <param name="configPath">設定ファイルのパス</param>
        /// <param name="systemName">接続先システム名</param>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        public bool SetCurrentData(SetDataOptions sdo, string configPath, string systemName)
        {
            try
            {
                if (_db.SetCurrentData(sdo))
                {
                    _db.SetConnectionString(configPath, systemName);
                }

                return _db.SelectedData;
            }
            catch
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
        public DataTable Select(string q,
                                Dictionary<HBODDataBase.Declare, dynamic> values = null)
        {
            try
            {
                return Select(_db.ConnectionString, q, values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Select結果を取得します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="q">クエリ</param>
        /// <param name="values">ストアドプロシージャの変数</param>
        /// <returns>実行結果 (DataTable)</returns>
        public DataTable Select(string connectionString,
                                string q,
                                Dictionary<HBODDataBase.Declare, dynamic> values = null)
        {
            try
            {
                return _db.Select(connectionString, q, values);
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
        /// false: 失敗
        /// </returns>
        public bool ExecuteNonQuery(string q,
                                    Dictionary<HBODDataBase.Declare, dynamic> values = null,
                                    bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQuery(_db.ConnectionString, q, values, doTransaction);
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
        public bool ExecuteNonQuery(string connectionString,
                                    string q,
                                    Dictionary<HBODDataBase.Declare, dynamic> values = null,
                                    bool doTransaction = true)
        {
            try
            {
                return _db.ExecuteNonQuery(connectionString, q, values, doTransaction);
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
        /// false: 失敗
        /// </returns>
        public bool ExecuteNonQueryAsync(string q,
                                         Dictionary<HBODDataBase.Declare, dynamic> values = null,
                                         bool doTransaction = true)
        {
            try
            {
                return ExecuteNonQueryAsync(_db.ConnectionString, q, values, doTransaction);
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
        public bool ExecuteNonQueryAsync(string connectionString,
                                         string q,
                                         Dictionary<HBODDataBase.Declare, dynamic> values = null,
                                         bool doTransaction = true)
        {
            try
            {
                return _db.ExecuteNonQuery(connectionString, q, values, doTransaction);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region キャプションの取得
        /// <summary>
        /// キャプションを取得します。
        /// </summary>
        /// <param name="programName">プログラム名称</param>
        /// <returns>キャプション</returns>
        public string GetCaption(string programName = "")
        {
            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT TOP(1)");
                q.AppendLine("  KCODE,");
                q.AppendLine("  DCODE,");
                q.AppendLine("  SMC");
                q.AppendLine("FROM");
                q.AppendLine("  SYSKAN WITH (NOLOCK)");
                using (DataTable dt = Select(q.ToString()))
                {
                    if (!dt.IsNullOrEmpty())
                    {
                        string kCode = CData.GetDataRow<Int16>(dt.Rows[0], "KCODE").ToString("D4");
                        string dCode = CData.GetDataRow<Int16>(dt.Rows[0], "DCODE").ToString("D3");
                        string smc = CData.GetDataRow<string>(dt.Rows[0], "SMC").Trim();

                        string caption = String.Format("会社コード[{0}] データコード[{1}] 会社名[{2}]", kCode, dCode, smc);
                        if (!String.IsNullOrEmpty(programName))
                        {
                            caption += String.Format(" - [{0}]", programName);
                        }

                        return caption;
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

        #region 税率取得
        /// <summary>
        /// 税率を取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <param name="taxType">税率種別
        /// 1 : 標準税率 (default)
        /// 2 : 軽減税率
        /// </param>
        /// <returns>税率</returns>
        public decimal GetTaxRate(string date, long taxType = 1)
        {
            try
            {
                if (CValidate.IsToDateTime(date))
                {
                    return GetTaxRate(CConvert.ToDateTime(date), taxType);
                }

                throw new Exception(ODMessage.Message("E", "3").Replace("%s", "日付が"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 税率を取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <param name="taxType">税率種別
        /// 1 : 標準税率 (default)
        /// 2 : 軽減税率
        /// </param>
        /// <returns>税率</returns>
        public decimal GetTaxRate(DateTime date, long taxType = 1)
        {
            decimal ret = -1;
            try
            {
                var taxData = _db.ObjOdds.ReadTax(date);

                if (taxData.Length > 0)
                {
                    switch (taxType)
                    {
                        // 標準税率
                        case 1:
                            ret = CConvert.ToDecimal(((HBODDSLib.arrayReadTaxData)taxData.GetValue(2)).pdZeiRitsu) / 100m;

                            break;
                        // 軽減税率
                        case 2:
                            ret = CConvert.ToDecimal(((HBODDSLib.arrayReadTaxData)taxData.GetValue(4)).pdZeiRitsu) / 100m;

                            break;
                    }
                }

                if (ret == -1)
                {
                    throw new Exception(ODMessage.Message("E", "6").Replace("%s", "税率が"));
                }

                return ret;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 自社情報マスタ
        #region 構造体
        /// <summary>
        /// 自社情報
        /// </summary>
        public struct CompanyData
        {
            /// <summary>会社コード</summary>
            public long CompanyCode;
            /// <summary>データコード</summary>
            public long DataCode;
            /// <summary>会社名称</summary>
            public string CompanyName;
            /// <summary>会社郵便番号</summary>
            public string Post;
            /// <summary>会社住所１</summary>
            public string Address1;
            /// <summary>会社住所２</summary>
            public string Address2;
            /// <summary>会社電話番号</summary>
            public string TEL;
            /// <summary>会社ＦＡＸ番号</summary>
            public string FAX;
            /// <summary>期首日付</summary>
            public DateTime? BeginningDate;
        }
        #endregion

        #region 自社情報の取得
        /// <summary>
        /// 自社情報を取得します。
        /// </summary>
        /// <returns>自社情報 (構造体)</returns>
        public CompanyData GetCompanyInformation()
        {
            try
            {
                var cData = new CompanyData();

                cData.CompanyCode = _db.ObjOdds.lSysKcode;
                cData.DataCode = _db.ObjOdds.lSysDcode;
                cData.CompanyName = _db.ObjOdds.strSysSmc;
                cData.Post = _db.ObjOdds.strSysSzip;
                cData.Address1 = _db.ObjOdds.strSysSad1;
                cData.Address2 = _db.ObjOdds.strSysSad2;
                cData.TEL = _db.ObjOdds.strSysStel;
                cData.FAX = _db.ObjOdds.strSysSfax;
                if (_db.ObjOdds.dateSysKhd == CConvert.ToDateTime("1900/01/01"))
                {
                    cData.BeginningDate = null;
                }
                else
                {
                    cData.BeginningDate = _db.ObjOdds.dateSysKhd;
                }

                return cData;
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region 得意先マスタ
        #region バリデーション
        /// <summary>
        /// 指定した得意先コードのマスタが存在するかどうかをチェックします。
        /// </summary>
        /// <param name="code">得意先コード</param>
        /// <returns>
        /// true : 存在する
        /// false: 存在しない
        /// </returns>
        public bool ExistsTokui(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return false;
            }

            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                q.AppendLine("  1");
                q.AppendLine("FROM");
                q.AppendLine("  TOKUI WITH (NOLOCK)");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine("  AND ICODE >= 0");
                q.AppendLine("  AND CODE = @CODE");

                var values = new Dictionary<HBODDataBase.Declare, dynamic>
                {
                    {new HBODDataBase.Declare("@CODE", SqlDbType.Char), code},
                };

                using (DataTable dt = Select(q.ToString(), values))
                {
                    return !dt.IsNullOrEmpty();
                }
            }
            catch
            {
                throw;
            }

            //// ※ GetByCode() : ICODEが0以下 (システム固定の識別ID) の場合、取得されない
            //try
            //{
            //    HBTOKLib.Tokui objTok = _db.ObjOdds.Tok;

            //    objTok.GetByCode(code);

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        #endregion

        #region 検索
        /// <summary>
        /// 得意先マスタを検索し、得意先コードを取得します。
        /// </summary>
        /// <param name="code">得意先コード</param>
        /// <returns>
        /// 1 : 選択
        /// 2 : 未選択
        /// </returns>
        public int SearchTokui(out string code)
        {
            code = "";
            try
            {
                HBKanakenLib.Kanaken objKana = _db.ObjOdds.TokKanaken;
                int ret = 0;

                ret = objKana.SelectMaster();
                if (ret == 1)
                {
                    code = objKana.strMasterCode.Trim();
                }

                return ret;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 得意先名の取得
        /// <summary>
        /// 得意先名を取得します。
        /// </summary>
        /// <param name="code">得意先コード</param>
        /// <param name="mergeName">得意先名の結合有無
        /// 0 : 結合 (default)
        /// 1 : 得意先名１のみ
        /// 2 : 得意先名２のみ
        /// </param>
        /// <returns>得意先名</returns>
        public string GetTokuiNM(string code, byte mergeName = 0)
        {
            if (String.IsNullOrEmpty(code))
            {
                return "";
            }

            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                switch (mergeName)
                {
                    // 得意先名１
                    case 1:
                        q.AppendLine("  RTRIM(NM1) AS NM");

                        break;
                    // 得意先名２
                    case 2:
                        q.AppendLine("  RTRIM(NM2) AS NM");

                        break;
                    // 得意先名１ + " " + 得意先名２
                    default:
                        q.AppendLine("  CONCAT(");
                        q.AppendLine("    RTRIM(NM1),");
                        q.AppendLine("    CASE");
                        q.AppendLine("      WHEN ISNULL(RTRIM(NM1), '') <> '' AND ISNULL(RTRIM(NM2), '') <> '' THEN ' '");
                        q.AppendLine("      ELSE NULL");
                        q.AppendLine("    END,");
                        q.AppendLine("    RTRIM(NM2)");
                        q.AppendLine("  ) AS NM");

                        break;
                }
                q.AppendLine("FROM");
                q.AppendLine("  TOKUI WITH (NOLOCK)");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine("  AND ICODE >= 0");
                q.AppendLine("  AND CODE = @CODE");

                var values = new Dictionary<HBODDataBase.Declare, dynamic>
                {
                    {new HBODDataBase.Declare("@CODE", SqlDbType.Char), code},
                };

                using (DataTable dt = Select(q.ToString(), values))
                {
                    return (!dt.IsNullOrEmpty()) ? CData.GetDataRow<string>(dt.Rows[0], "NM") : "";
                }
            }
            catch
            {
                throw;
            }

            //// ※ GetByCode() : ICODEが0以下 (システム固定の識別ID) の場合、取得されない
            //try
            //{
            //    HBTOKLib.Tokui objTok = _db.ObjOdds.Tok;

            //    try
            //    {
            //        objTok.GetByCode(code);
            //    }
            //    catch
            //    {
            //        throw;
            //    }

            //    string tokuiNM1 = objTok.strNm1.Trim();
            //    string tokuiNM2 = objTok.strNm2.Trim();

            //    switch (mergeName)
            //    {
            //        case 1:
            //            return tokuiNM1;
            //        case 2:
            //            return tokuiNM2;
            //        default:
            //            string tokuiNM = "";

            //            tokuiNM += tokuiNM1;
            //            if (!String.IsNullOrEmpty(tokuiNM1) && !String.IsNullOrEmpty(tokuiNM2))
            //            {
            //                tokuiNM += " ";
            //            }
            //            tokuiNM += tokuiNM2;

            //            return tokuiNM;
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
        }
        #endregion
        #endregion

        #region 商品マスタ
        #region バリデーション
        /// <summary>
        /// 指定した商品コードのマスタが存在するかどうかをチェックします。
        /// </summary>
        /// <param name="code">商品コード</param>
        /// <returns>
        /// true : 存在する
        /// false: 存在しない
        /// </returns>
        public bool ExistsShohin(string code)
        {
            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                q.AppendLine("  1");
                q.AppendLine("FROM");
                q.AppendLine("  SHOHIN WITH (NOLOCK)");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine("  AND ICODE >= 0");
                q.AppendLine("  AND CODE = @CODE");

                var values = new Dictionary<HBODDataBase.Declare, dynamic>
                {
                    {new HBODDataBase.Declare("@CODE", SqlDbType.Char), code},
                };

                using (DataTable dt = Select(q.ToString(), values))
                {
                    return !dt.IsNullOrEmpty();
                }
            }
            catch
            {
                throw;
            }

            //// ※ GetByCode() : ICODEが0以下 (システム固定の識別ID) の場合、取得されない
            //try
            //{
            //    HBSHOLib.Shohin objSho = _db.ObjOdds.Sho;

            //    objSho.GetByCode(code);

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        #endregion

        #region 検索
        /// <summary>
        /// 商品マスタを検索し、商品コードを取得します。
        /// </summary>
        /// <param name="code">商品コード</param>
        /// <returns>
        /// 1 : 選択
        /// 2 : 未選択
        /// </returns>
        public int SearchShohin(out string code)
        {
            code = "";
            try
            {
                HBKanakenLib.Kanaken objKana = _db.ObjOdds.ShoKanaken;
                int ret = 0;

                ret = objKana.SelectMaster();
                if (ret == 1)
                {
                    code = objKana.strMasterCode.Trim();
                }

                return ret;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 商品名の取得
        /// <summary>
        /// 商品名を取得します。
        /// </summary>
        /// <param name="code">商品コード</param>
        /// <returns>商品名</returns>
        public string GetShohinNM(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return "";
            }

            try
            {
                StringBuilder q = new StringBuilder();

                q.AppendLine("SELECT");
                q.AppendLine("  RTRIM(NM1) AS NM");
                q.AppendLine("FROM");
                q.AppendLine("  SHOHIN WITH (NOLOCK)");
                q.AppendLine("WHERE 1 = 1");
                q.AppendLine("  AND ICODE >= 0");
                q.AppendLine("  AND CODE = @CODE");

                var values = new Dictionary<HBODDataBase.Declare, dynamic>
                {
                    {new HBODDataBase.Declare("@CODE", SqlDbType.Char), code},
                };

                using (DataTable dt = Select(q.ToString(), values))
                {
                    return (!dt.IsNullOrEmpty()) ? CData.GetDataRow<string>(dt.Rows[0], "NM") : "";
                }
            }
            catch
            {
                throw;
            }

            //// ※ GetByCode() : ICODEが0以下 (システム固定の識別ID) の場合、取得されない
            //try
            //{
            //    HBSHOLib.Shohin objSho = _db.ObjOdds.Sho;

            //    try
            //    {
            //        objSho.GetByCode(code);
            //    }
            //    catch
            //    {
            //        throw;
            //    }

            //    return objSho.strNm1.Trim();
            //}
            //catch
            //{
            //    throw;
            //}
        }
        #endregion
        #endregion
    }
}
