using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MKClass
{
    /// <summary>
    /// レジストリ操作クラス
    /// </summary>
    public class CRegistry
    {
        /// <summary>
        /// 全てのサブキーの名前が格納されている文字列の配列を取得します。
        /// </summary>
        /// <param name="rk">Windowsレジストリのキーレベルノード</param>
        /// <param name="key">レジストリのキー</param>
        /// <returns>全てのサブキーの名前が格納されている文字列の配列</returns>
        public static string[] GetSubKeyNames(RegistryKey rk, string key)
        {
            RegistryKey rParentKey = null;
            try
            {
                rParentKey = rk.OpenSubKey(key);
                if (rParentKey == null)
                {
                    return new List<string>().ToArray();
                }

                var subKeyNames = rParentKey.GetSubKeyNames();

                return (subKeyNames != null) ? subKeyNames : new List<string>().ToArray();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rParentKey != null)
                {
                    rParentKey.Close();
                }
            }
        }

        /// <summary>
        /// 指定した名前に関連付けられている値を取得します。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="rk">Windowsレジストリのキーレベルノード</param>
        /// <param name="key">レジストリのキー</param>
        /// <param name="name">レジストリの名称</param>
        /// <param name="defaultValue">nameが存在しない場合に返す値</param>
        /// <returns>レジストリの値</returns>
        public static T GetValue<T>(RegistryKey rk, string key, string name, object defaultValue = null)
        {
            RegistryKey rKey = null;
            try
            {
                rKey = rk.OpenSubKey(key);
                if (rKey == null)
                {
                    return default(T);
                }

                var value = (defaultValue == null) ? (T)rKey.GetValue(name) : (T)rKey.GetValue(name, defaultValue);

                return (value != null) ? value : default(T);
            }
            catch (NullReferenceException)
            {
                throw new Exception("レジストリ [" + rk.Name + key + name + "] が存在しません。");
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rKey != null)
                {
                    rKey.Close();
                }
            }
        }

        /// <summary>
        /// レジストリキーに名前/値ペアの値を設定します。
        /// </summary>
        /// <param name="rk">Windowsレジストリのキーレベルノード</param>
        /// <param name="key">レジストリのキー</param>
        /// <param name="name">レジストリの名称</param>
        /// <param name="value">格納するデータ</param>
        public static void SetValue(RegistryKey rk, string key, string name, object value)
        {
            RegistryKey rKey = null;
            try
            {
                rKey = rk.CreateSubKey(key);
                if (rKey == null)
                {
                    return;
                }

                rKey.SetValue(name, value);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rKey != null)
                {
                    rKey.Close();
                }
            }
        }
    }
}
