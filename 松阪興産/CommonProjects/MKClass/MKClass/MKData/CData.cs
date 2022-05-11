using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MKClass.MKData
{
    #region データ操作クラス (abstract)
    /// <summary>
    /// データ操作クラス (abstract)
    /// </summary>
    /// <remarks>
    /// DataTable, DataRowクラス等に対するアクセサを定義します。
    /// </remarks>
    abstract public class CData
    {
        /// <summary>
        /// DataTableより指定したセルの値を取得します。 (Deprecated)
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="dr">行</param>
        /// <param name="columnName">列名</param>
        /// <returns>セルの値</returns>
        public static T GetDataRow<T>(DataRow dr, String columnName)
        {
            try
            {
                return (dr == null || dr.IsNull(columnName)) ? default(T) : dr.Field<T>(columnName);
            }
            catch
            {
                throw;
            }
        }
    }
    #endregion



    #region データ取得クラス (static)
    /// <summary>
    /// データ取得クラス (static)
    /// </summary>
    public static class DataGetter
    {
        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <param name="self">データ行</param>
        /// <param name="dc">列のスキーマ</param>
        /// <returns>セルの値</returns>
        public static object GetData(this DataRow self, DataColumn dc)
        {
            try
            {
                return self.GetData<object>(dc);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <param name="self">データ行</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <returns>セルの値</returns>
        public static object GetData(this DataRow self, int columnIndex)
        {
            try
            {
                return self.GetData<object>(columnIndex);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <param name="self">データ行</param>
        /// <param name="columnName">列名</param>
        /// <returns>セルの値</returns>
        public static object GetData(this DataRow self, String columnName)
        {
            try
            {
                return self.GetData<object>(columnName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="self">データ行</param>
        /// <param name="dc">列のスキーマ</param>
        /// <returns>セルの値</returns>
        public static T GetData<T>(this DataRow self, DataColumn dc)
        {
            try
            {
                return (self == null || self.IsNull(dc)) ? default(T) : self.Field<T>(dc);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="self">データ行</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <returns>セルの値</returns>
        public static T GetData<T>(this DataRow self, int columnIndex)
        {
            try
            {
                return (self == null || self.IsNull(columnIndex)) ? default(T) : self.Field<T>(columnIndex);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataTableより指定したセルの値を取得します。
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="self">データ行</param>
        /// <param name="columnName">列名</param>
        /// <returns>セルの値</returns>
        public static T GetData<T>(this DataRow self, String columnName)
        {
            try
            {
                return (self == null || self.IsNull(columnName)) ? default(T) : self.Field<T>(columnName);
            }
            catch
            {
                throw;
            }
        }
    }
    #endregion
}
