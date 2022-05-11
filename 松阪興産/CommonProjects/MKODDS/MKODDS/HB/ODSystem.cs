using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MKODDS.HB
{
    /// <summary>
    /// [販売大臣] ODDS システム共通クラス
    /// </summary>
    public class HBODSystem : ODSystem
    {
        #region プロパティ
        /// <summary>ODDSオブジェクト</summary>
        public HBODDSLib.Application ObjOdds { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HBODSystem()
            : base()
        {
            try
            {
                this.ObjOdds = new HBODDSLib.Application();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~HBODSystem()
        {
            try
            {
                this.ObjOdds = null;
                GC.Collect();
            }
            catch
            {

            }
        }
        #endregion

        #region データ設定 (override)
        /// <summary>
        /// データを設定します。
        /// </summary>
        /// <param name="sdo">データ設定オプション</param>
        /// <returns>
        /// true : 選択
        /// false: 未選択
        /// </returns>
        public override bool SetCurrentData(SetDataOptions sdo = SetDataOptions.SetLastData)
        {
            try
            {
                // オブジェクトの初期化
                this.ObjOdds = null;
                this.ObjOdds = new HBODDSLib.Application();

                if (sdo == SetDataOptions.NotSet)
                {
                    return (base.SelectedData = false);
                }

                // 大臣認証
                if (this.ObjOdds.lAuthenticationMode == 1)
                {
                    // 現在大臣本体を起動しているユーザー (または、最後に起動したユーザー) でログインする
                    this.ObjOdds.lUseCurrentUserLogin = 1;
                }

                string path = this.ObjOdds.strCurrentDataPath;

                int ret = (sdo == SetDataOptions.SelectData || !Directory.Exists(path)) ? this.ObjOdds.SelectData(out path) : 1;
                if (ret == 1)
                {
                    this.ObjOdds.SetCurrentData(path);

                    base.SelectedDataPath = path;

                    return (base.SelectedData = true);
                }

                return (base.SelectedData = false);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
