using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKClass;

namespace ASViews
{
    /// <summary>
    /// コントロールの色を定義するクラス (static)
    /// </summary>
    internal static class ControlColor
    {
        /// <summary>
        /// 背景色を定義するクラス (static)
        /// </summary>
        public static class Back
        {
            /// <summary>エラー時</summary>
            public static readonly Color Error = Color.PaleVioletRed;
        }
    }



    /// <summary>
    /// ASViews 共通ライブラリクラス
    /// </summary>
    internal class ASViewsClass
    {
        #region プロパティ
        /// <summary>プログラム名</summary>
        public static string PROGRAM_NAME { get { return "ASデータ参照"; } }
        /// <summary>プログラムID</summary>
        public static string ProgramId { get; private set; }

        /// <summary>件数のフォーマット (#,0)</summary>
        public static string COUNT_FORMAT { get { return "#,0"; } }

        /// <summary>件数の単位 (件)</summary>
        public static string COUNT_UNIT { get { return "件"; } }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ASViewsClass()
        {
            try
            {
                ProgramId = CSystem.GetAssemblyName<ASViewsClass>(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
