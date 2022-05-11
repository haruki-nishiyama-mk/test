using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MKClass.MKException
{
    /// <summary>
    /// データベース例外クラス
    /// </summary>
    [Serializable()]
    public class CDataBaseException : Exception
    {
        /// <summary>エラーメッセージの詳細</summary>
        public string Detail { get; private set; }
        /// <summary>例外エラーが発生した際のクエリ文字列</summary>
        public string QueryString { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CDataBaseException()
            : base()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="detail">エラーメッセージの詳細</param>
        /// <param name="queryString">例外エラーが発生した際のクエリ文字列</param>
        public CDataBaseException(string message, string detail = "", string queryString = "")
            : base(message)
        {
            this.Detail = detail;
            this.QueryString = queryString;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="inner">内部例外</param>
        /// <param name="detail">エラーメッセージの詳細</param>
        /// <param name="queryString">例外エラーが発生した際のクエリ文字列</param>
        public CDataBaseException(string message, Exception inner, string detail = "", string queryString = "")
            : base(message, inner)
        {
            this.Detail = detail;
            this.QueryString = queryString;
        }

        /// <summary>
        /// シリアル化したデータを使用して、Exceptionクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">シリアル化済みオブジェクトデータ</param>
        /// <param name="context">コンテキスト情報</param>
        protected CDataBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
