using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MKClass;
using MKClass.MKFile;
using MKODDS;
using MKODDS.HB.Data;

namespace MKODDS.Setup
{
    /// <summary>
    /// ODDSSetup 共通ライブラリクラス
    /// </summary>
    public class ODDSSetupLibrary
    {
        #region 定数
        /// <summary>ERP拡張スクリプト設定ファイル</summary>
        private static readonly string _ERPSCRIPT_FILE = "ERPCUSTOMSCRIPT.INI";
        /// <summary>セットアップSQLファイル</summary>
        private static readonly string _SETUP_SQL_FILE = "Setup.sql";
        /// <summary>ERPメニュー設定ファイル</summary>
        private static readonly string _ERPMENU_FILE = "ERPMENU.DAT";

        /// <summary>エンコード (セットアップSQLファイル)</summary>
        private static readonly Encoding _ENCODE = Encoding.GetEncoding("utf-8");
        #endregion

        #region プロパティ
        /// <summary>プログラムID</summary>
        internal static string ProgramId { get; private set; }
        /// <summary>プログラム名</summary>
        internal static string PROGRAM_NAME { get { return "ODDSセットアップ"; } }

        /// <summary>ログ操作オブジェクト</summary>
        internal static CLog CLogObj { get; private set; }

        /// <summary>販売大臣ODDS APIオブジェクト</summary>
        internal static HBODAPI HBODObj { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ODDSSetupLibrary()
        {
            try
            {
                ProgramId = CSystem.GetAssemblyName<ODDSSetupLibrary>(this);

                CLogObj = new CLog();

                HBODObj = new HBODAPI(SetDataOptions.NotSet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region テーブル設定
        /// <summary>
        /// テーブルを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        /// <returns>
        /// true : OK
        /// false: キャンセル (データ未選択)
        /// </returns>
        public static bool SetTables(OHKENProducts ohkenProd)
        {
            try
            {
                StringBuilder errorStrings = new StringBuilder();

                if (!OHKENProductsExt.ValidateOhkenProdInstalled(ohkenProd))
                {
                    errorStrings.AppendLine(ODMessage.Message("E", "1").Replace("%s", ohkenProd.GetName() + "が"));

                    throw new Exception(errorStrings.ToString());
                }

                string setupSQLPath = Path.Combine(CFile.DLLParentPath, "OHKEN", ohkenProd.ToString(), "ERPScript", "DDL", _SETUP_SQL_FILE);

                switch (ohkenProd)
                {
                    case OHKENProducts.HB:
                        if (HBODObj.SetCurrentData(SetDataOptions.SelectData))
                        {
                            foreach (var q in CSQL.GetQueries(setupSQLPath, _ENCODE))
                            {
                                HBODObj.ExecuteNonQueryAsync(q);
                            }
                        }
                        else
                        {
                            return false;
                        }

                        break;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ERP拡張スクリプトの設定
        /// <summary>
        /// ERP拡張スクリプトを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        public static void SetERPScript(params OHKENProducts[] ohkenProds)
        {
            try
            {
                StringBuilder errorStrings = new StringBuilder();

                foreach (var ohkenProd in ohkenProds)
                {
                    if (!OHKENProductsExt.ValidateOhkenProdInstalled(ohkenProd))
                    {
                        errorStrings.AppendLine(ODMessage.Message("E", "1").Replace("%s", ohkenProd.GetName() + "が"));

                        continue;
                    }

                    string fromERPScriptPath = Path.Combine(CFile.DLLParentPath, "OHKEN", ohkenProd.ToString(), "ERPScript", _ERPSCRIPT_FILE);
                    if (!File.Exists(fromERPScriptPath))
                    {
                        errorStrings.AppendLine(ODMessage.Message("E", "7").Replace("%s", fromERPScriptPath));

                        continue;
                    }

                    foreach (var ohkenProdInstallPath in OHKENProductsExt.GetOHKENProdInstallPaths(ohkenProd))
                    {
                        string toERPScriptPath = Path.Combine(ohkenProdInstallPath, "BIN", _ERPSCRIPT_FILE);
                        try
                        {
                            File.Copy(fromERPScriptPath, toERPScriptPath, true);
                        }
                        catch (Exception ex)
                        {
                            errorStrings.AppendLine(ex.Message);
                        }
                    }
                }

                if (errorStrings.Length > 0)
                {
                    throw new Exception(errorStrings.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ERPメニューの設定
        /// <summary>
        /// ERPメニューを設定します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        public static void SetERPMenu(params OHKENProducts[] ohkenProds)
        {
            try
            {
                StringBuilder errorStrings = new StringBuilder();

                foreach (var ohkenProd in ohkenProds)
                {
                    if (!OHKENProductsExt.ValidateOhkenProdInstalled(ohkenProd))
                    {
                        errorStrings.AppendLine(ODMessage.Message("E", "1").Replace("%s", ohkenProd.GetName() + "が"));

                        continue;
                    }

                    string fromERPMenuPath = Path.Combine(CFile.DLLParentPath, "OHKEN", ohkenProd.ToString(), "ERPMenu", _ERPMENU_FILE);
                    if (!File.Exists(fromERPMenuPath))
                    {
                        errorStrings.AppendLine(ODMessage.Message("E", "7").Replace("%s", fromERPMenuPath));

                        continue;
                    }

                    foreach (var ohkenProdInstallPath in OHKENProductsExt.GetOHKENProdInstallPaths(ohkenProd))
                    {
                        foreach (var userDir in Directory.GetDirectories(ohkenProdInstallPath, @"USR*"))
                        {
                            string toERPMenuPath = Path.Combine(ohkenProdInstallPath, userDir, _ERPMENU_FILE);
                            try
                            {
                                File.Copy(fromERPMenuPath, toERPMenuPath, true);
                            }
                            catch (Exception ex)
                            {
                                errorStrings.AppendLine(ex.Message);
                            }
                        }
                    }
                }

                if (errorStrings.Length > 0)
                {
                    throw new Exception(errorStrings.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
