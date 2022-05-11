using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKODDS
{
    #region システム共通クラス
    /// <summary>
    /// ODDS データ設定オプションを示します。
    /// </summary>
    public enum SetDataOptions : int
    {
        /// <summary>データを設定しない</summary>
        NotSet = -1,
        /// <summary>データを選択する</summary>
        SelectData = 0,
        /// <summary>最後に起動したデータを設定する</summary>
        SetLastData = 1
    }

    /// <summary>
    /// ODDS システム共通クラス (abstract)
    /// </summary>
    abstract public class ODSystem
    {
        #region プロパティ
        /// <summary>データ選択の状態 [true : 選択, false : 未選択]</summary>
        public bool SelectedData { get; protected set; }

        /// <summary>選択したデータのパス</summary>
        public string SelectedDataPath { get; protected set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ODSystem()
        {
            try
            {
                this.SelectedData = false;
                this.SelectedDataPath = "";
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region データ設定
        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        abstract public bool SetCurrentData(SetDataOptions sdo = SetDataOptions.SetLastData);
        #endregion
    }
    #endregion



    #region コントロールの背景色を定義するクラス
    /// <summary>
    /// コントロールの背景色を定義するクラス
    /// </summary>
    public static class ControlBackColor
    {
        /// <summary>エラー時</summary>
        public static readonly Color Error = Color.PaleVioletRed;
    }
    #endregion
}
