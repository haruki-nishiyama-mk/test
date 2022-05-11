using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKClass
{
    #region 列挙型
    /// <summary>
    /// 時間間隔のフォーマットを示します。
    /// </summary>
    public enum TimeSpanFormatType
    {
        /// <summary>時間数:分数:秒数</summary>
        HMS,
        /// <summary>時間数:分数</summary>
        HM,
        /// <summary>時間数</summary>
        H,
        /// <summary>分数:秒数</summary>
        MS,
        /// <summary>分数</summary>
        M,
        /// <summary>秒数</summary>
        S
    }
    #endregion
}
