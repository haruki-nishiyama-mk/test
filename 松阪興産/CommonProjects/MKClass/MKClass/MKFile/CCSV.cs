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
    /// CSVファイル操作クラス
    /// </summary>
    public class CCSV : CFile
    {
        #region public
        /// <summary>
        /// DataTableの内容をCSV形式にエクスポートします。
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="toCSVFile">CSVファイル</param>
        /// <param name="bAddHeader">ヘッダー有無フラグ</param>
        /// <param name="enc">エンコード</param>
        public static void ExportCSV(DataTable dt, string toCSVFile, bool bAddHeader, Encoding enc)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(toCSVFile, false, enc))
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
                                sr.Write(',');
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
                                sr.Write(',');
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
