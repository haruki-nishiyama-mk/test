using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKClass
{
    #region 列挙型
    /// <summary>
    /// 日付のフォーマットを示します。
    /// </summary>
    public enum DateFormatType
    {
        /// <summary>yyyy/MM/dd</summary>
        [StringValue("yyyy/MM/dd")]
        YYYYMMDD,
        /// <summary>yyyy/M/d</summary>
        [StringValue("yyyy/M/d")]
        YYYYMD,
        /// <summary>yy/MM/dd</summary>
        [StringValue("yy/MM/dd")]
        YYMMDD,
        /// <summary>yy/M/d</summary>
        [StringValue("yy/M/d")]
        YYMD,
        /// <summary>yyyy/MM</summary>
        [StringValue("yyyy/MM")]
        YYYYMM,
        /// <summary>yyyy/M</summary>
        [StringValue("yyyy/M")]
        YYYYM,
        /// <summary>yy/MM</summary>
        [StringValue("yy/MM")]
        YYMM,
        /// <summary>yy/M</summary>
        [StringValue("yy/M")]
        YYM,
        /// <summary>yyyy</summary>
        [StringValue("yyyy")]
        YYYY,
        /// <summary>yy</summary>
        [StringValue("yy")]
        YY,
        /// <summary>MM/dd</summary>
        [StringValue("MM/dd")]
        MMDD,
        /// <summary>M/d</summary>
        [StringValue("M/d")]
        MD,
        /// <summary>MM</summary>
        [StringValue("MM")]
        MM,
        /// <summary>M</summary>
        [StringValue("%M")]
        M,
        /// <summary>dd</summary>
        [StringValue("dd")]
        DD,
        /// <summary>d</summary>
        [StringValue("%d")]
        D
    }
    #endregion

    /// <summary>
    /// DateTimeクラス (static)
    /// </summary>
    public static class CDateTime
    {
        /// <summary>
        /// 指定した期間の経過月数を取得します。
        /// </summary>
        /// <param name="dtFrom">比較対象日時 (From)</param>
        /// <param name="dtTo">比較対象日時 (To)</param>
        /// <param name="considerDay">期間に日を考慮するかどうか</param>
        /// <returns>経過月数</returns>
        /// <remarks>
        /// DateTimeの時間は考慮しません。
        /// </remarks>
        public static int GetElapsedMonths(DateTime dtFrom, DateTime dtTo, bool considerDay = false)
        {
            int ret = 0;
            try
            {
                int sign = 0;
                DateTime dtFrom_ = DateTime.MinValue;
                DateTime dtTo_ = DateTime.MaxValue;

                if (dtFrom <= dtTo)
                {
                    sign = 1;
                    dtFrom_ = dtFrom;
                    dtTo_ = dtTo;
                }
                else
                {
                    sign = -1;
                    dtFrom_ = dtTo;
                    dtTo_ = dtFrom;
                }

                ret = (dtTo_.Month + ((dtTo_.Year - dtFrom_.Year) * 12)) - dtFrom_.Month;

                if (considerDay)
                {
                    if (dtFrom_.Day <= dtTo_.Day)
                    {
                        // 経過していると見做す
                    }
                    else if (dtFrom_.Day == DateTime.DaysInMonth(dtFrom_.Year, dtFrom_.Month) &&
                             dtTo_.Day == DateTime.DaysInMonth(dtTo_.Year, dtTo_.Month))
                    {
                        // 経過していると見做す
                    }
                    else
                    {
                        ret -= 1;
                    }
                }

                ret *= sign;

                return ret;
            }
            catch
            {
                throw;
            }
        }
    }
}
