using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MKClass
{
    /// <summary>
    /// アプリケーション構成ファイル操作クラス (static)
    /// </summary>
    public static class CConfiguration
    {
        /// <summary>
        /// アプリケーション構成ファイルから指定したキーに対する値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>値</returns>
        public static string GetAppSettingValue(string key)
        {
            try
            {
                return (ConfigurationManager.AppSettings[key] != null) ? ConfigurationManager.AppSettings[key].ToString() : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
