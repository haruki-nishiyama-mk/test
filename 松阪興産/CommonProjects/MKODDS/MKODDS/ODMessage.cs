using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using MKClass;
using MKClass.MKFile;

namespace MKODDS
{
    /// <summary>
    /// メッセージ操作クラス (static)
    /// </summary>
    public static class ODMessage
    {
        #region プロパティ
        /// <summary>メッセージファイルのパス</summary>
        private static string MESSAGE_PATH { get { return Path.Combine(CFile.DLLParentPath, "Message.xml"); } }
        #endregion

        /// <summary>
        /// XML定義
        /// </summary>
        /// <remarks>
        /// [接頭語の説明]
        ///   E : 要素名
        ///   A : 属性名
        /// </remarks>
        private struct XmlDef
        {
            /// <summary>ルート</summary>
            public const string E_ROOT = "messages";

            /// <summary>ステータス</summary>
            public const string E_STATUS = "status";

            /// <summary>行 (メッセージ)</summary>
            public const string E_ROW = "row";

            /// <summary>識別ID</summary>
            public const string A_ID = "id";
            /// <summary>識別名</summary>
            public const string A_NAME = "name";
        }

        /// <summary>
        /// メッセージ内容を返します。
        /// </summary>
        /// <param name="statusId">ステータスID</param>
        /// <param name="messageId">メッセージID</param>
        /// <returns>メッセージ内容</returns>
        public static string Message(string statusId, string messageId)
        {
            if (String.IsNullOrEmpty(statusId) || String.IsNullOrEmpty(messageId))
            {
                return "";
            }

            try
            {
                if (!File.Exists(MESSAGE_PATH))
                {
                    return "";
                }

                XDocument xDoc = XDocument.Load(MESSAGE_PATH);

                var elStatus = (
                    from x in xDoc.Root.Elements(XmlDef.E_STATUS)
                    where x.Attribute(XmlDef.A_ID).Value == statusId
                    select x
                    ).SingleOrDefault();
                if (elStatus != null)
                {
                    return (
                        from x in elStatus.Elements(XmlDef.E_ROW)
                        where x.Attribute(XmlDef.A_ID).Value == messageId
                        select x
                        ).SingleOrDefault().Value;
                }
            }
            catch
            {

            }

            return "";
        }
    }
}
