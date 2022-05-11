using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MKClass
{
    #region StringWriter UTF-8 対応
    /// <summary>
    /// StringWriter UTF-8 対応
    /// </summary>
    public class StringWriterUTF8 : StringWriter
    {
        /// <summary>文字エンコーディング (UTF-8)</summary>
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
    #endregion
}
