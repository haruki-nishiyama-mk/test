using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using MKClass;

namespace MKODDS
{
    #region 応研製品
    /// <summary>
    /// 応研製品の種別を示します。
    /// </summary>
    public enum OHKENProducts
    {
        /// <summary>(指定無し)</summary>
        None,
        /// <summary>大蔵大臣</summary>
        OK,
        /// <summary>大蔵大臣 個別原価版</summary>
        JC,
        /// <summary>販売大臣</summary>
        HB,
        /// <summary>給与大臣</summary>
        KY,
        /// <summary>人事大臣</summary>
        JJ,
        /// <summary>顧客大臣</summary>
        CU,
        /// <summary>医療大臣</summary>
        IR,
        /// <summary>建設大臣</summary>
        KE,
        /// <summary>福祉大臣</summary>
        FK,
        /// <summary>公益大臣</summary>
        KD,
        /// <summary>就業大臣</summary>
        SG
    }

    /// <summary>
    /// 応研製品 enum定義のヘルパクラス (static)
    /// </summary>
    public static class OHKENProductsExt
    {
        private static readonly string _REG_KEY_OHKEN = @"Software\OHKEN\";
        private static readonly string _REG_NAME_OHKEN_PROD_INSTALL_PATH = "InstallLongPath";

        /// <summary>
        /// 応研製品の名称を取得します。
        /// </summary>
        /// <param name="self">応研製品の種別</param>
        /// <returns>応研製品の名称</returns>
        public static string GetName(this OHKENProducts self)
        {
            string[] ohkenProdNames =
                {
                    "",
                    "大蔵大臣",
                    "大蔵大臣 個別原価版",
                    "販売大臣",
                    "給与大臣",
                    "人事大臣",
                    "顧客大臣",
                    "医療大臣",
                    "建設大臣",
                    "福祉大臣",
                    "公益大臣",
                    "就業大臣"
                };

            return ohkenProdNames[(int)self];
        }

        /// <summary>
        /// 応研製品のデータベース記号を取得します。
        /// </summary>
        /// <param name="self">応研製品の種別</param>
        /// <returns>応研製品のデータベース記号</returns>
        public static string GetDataBaseSymbol(this OHKENProducts self)
        {
            string[] ohkenProdDataBaseSymbols =
                {
                    "",
                    "S3",
                    "G3",
                    "HB",
                    "KY",
                    "JJ",
                    "CU",
                    "M3",
                    "K3",
                    "W3",
                    "P3",
                    "SG"
                };

            return ohkenProdDataBaseSymbols[(int)self];
        }

        /// <summary>
        /// enum定義を返します。
        /// </summary>
        /// <returns>enum定義</returns>
        /// <remarks>
        /// None (指定無し) は除きます。
        /// </remarks>
        public static IEnumerator<OHKENProducts> GetValues()
        {
            foreach (OHKENProducts ohkenProduct in Enum.GetValues(typeof(OHKENProducts)).Cast<OHKENProducts>().Where(o => o != OHKENProducts.None))
            {
                yield return ohkenProduct;
            }
        }

        /// <summary>
        /// enum定義を配列で返します。
        /// </summary>
        /// <returns>enum定義の配列</returns>
        /// <remarks>
        /// None (指定無し) は除きます。
        /// </remarks>
        public static OHKENProducts[] GetValuesToArray()
        {
            return Enum.GetValues(typeof(OHKENProducts)).Cast<OHKENProducts>().Where(o => o != OHKENProducts.None).ToArray();
        }

        /// <summary>
        /// 対象製品のインストールパスを取得します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        /// <returns>対象製品のインストールパス</returns>
        /// <remarks>
        /// 製品のバージョン違いにより、複数のインストール先があることを考慮します。
        /// </remarks>
        public static IEnumerable<string> GetOHKENProdInstallPaths(OHKENProducts ohkenProd)
        {
            foreach (var key in CRegistry.GetSubKeyNames(Registry.CurrentUser, _REG_KEY_OHKEN).Where(o => Regex.IsMatch(o, @"^" + ohkenProd.GetName())))
            {
                string ohkenProdInstallPath =
                    CRegistry.GetValue<string>(
                        Registry.CurrentUser,
                        _REG_KEY_OHKEN + key,
                        _REG_NAME_OHKEN_PROD_INSTALL_PATH,
                        "");
                if (!Directory.Exists(ohkenProdInstallPath))
                {
                    continue;
                }

                yield return ohkenProdInstallPath;
            }
        }

        /// <summary>
        /// 対象製品がインストールされているかどうかを検証します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        /// <returns>
        /// true : インストール済み
        /// false: 未インストール
        /// </returns>
        public static bool ValidateOhkenProdInstalled(OHKENProducts ohkenProd)
        {
            try
            {
                var keys = CRegistry.GetSubKeyNames(Registry.CurrentUser, _REG_KEY_OHKEN).Where(o => Regex.IsMatch(o, @"^" + ohkenProd.GetName()));

                return (keys.Count() > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// データベース名を取得します。
        /// </summary>
        /// <param name="ohkenProd">応研製品の種別</param>
        /// <param name="dataPath">データパス</param>
        /// <returns>データベース名</returns>
        /// <remarks>
        /// 形式 : [応研製品のデータベース記号] + "DATA" + "[会社コード (4桁)]" + "_" + "[データコード (3桁)]" + "データが存在するドライブ"
        /// </remarks>
        public static string GetDataBaseName(OHKENProducts ohkenProd, string dataPath)
        {
            try
            {
                return
                    ohkenProd.GetDataBaseSymbol() +
                    Path.GetFileName(dataPath).Replace('.', '_') +
                    Path.GetPathRoot(dataPath).Replace(new String(new char[] { Path.VolumeSeparatorChar, Path.DirectorySeparatorChar }), "");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    #endregion
}
