using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MKClass.MKData;

namespace MKClass.MKFile
{
    /// <summary>
    /// TSVファイル操作クラス
    /// </summary>
    public class CTSV : CFile
    {
        #region public
        /// <summary>
        /// DataTableの内容をTSV形式にエクスポートします。
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="toTSVFile">TSVファイル</param>
        /// <param name="bAddHeader">ヘッダー有無フラグ</param>
        /// <param name="enc">エンコード</param>
        /// <remarks>
        /// 文字列フィールドに限り、区切り文字を"\t,"にします。
        /// </remarks>
        public static void ExportTSV(DataTable dt, string toTSVFile, bool bAddHeader, Encoding enc)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(toTSVFile, false, enc))
                {
                    int colCount = dt.Columns.Count;
                    int lastColIndex = colCount - 1;

                    if (bAddHeader)
                    {
                        for (int i = 0; i < colCount; i++)
                        {
                            string field = CConvert.EncloseDoubleQuotes(dt.Columns[i].Caption);
                            sr.Write(field);
                            if (lastColIndex > i)
                            {
                                sr.Write('\t');
                            }
                        }

                        sr.Write("\r\n");
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < colCount; i++)
                        {
                            string field = CConvert.EncloseDoubleQuotes(row[i].ToString());
                            sr.Write(field);
                            if (lastColIndex > i)
                            {
                                sr.Write('\t');
                            }
                        }

                        sr.Write("\r\n");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
