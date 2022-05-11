using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MKClass.MKFile
{
    /// <summary>
    /// SQLファイル操作クラス
    /// </summary>
    public class CSQL : CFile
    {
        #region public
        /// <summary>
        /// SQLファイルからGOステートメント単位で分割したクエリを取得します。
        /// </summary>
        /// <param name="sqlPath">SQLファイルパス</param>
        /// <param name="enc">エンコード</param>
        /// <returns>分割したクエリ</returns>
        public static IEnumerable<string> GetQueries(string sqlPath, Encoding enc)
        {
            using (StreamReader sr = new StreamReader(sqlPath, enc))
            {
                var qs = Regex.Split(sr.ReadToEnd(), @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (var q in qs.Where(x => !String.IsNullOrWhiteSpace(x)).Select(x => x.Trim(' ', '\r', '\n')))
                {
                    yield return q;
                }
            }
        }
        #endregion
    }
}
