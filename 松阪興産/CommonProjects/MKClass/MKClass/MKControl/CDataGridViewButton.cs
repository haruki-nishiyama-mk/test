using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKClass.MKControl
{
    #region CButtonColumnクラス
    /// <summary>
    /// DataGridViewButtonColumn拡張クラス
    /// </summary>
    public class CButtonColumn : DataGridViewButtonColumn
    {
        #region プロパティ (基底)
        /// <summary>
        /// 新しいセルの作成に使用するテンプレートを取得または設定します。
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CButtonCell)))
                {
                    throw new InvalidCastException("Must be a CButtonCell");
                }

                base.CellTemplate = value;
            }
        }
        #endregion

        #region プロパティ (追加)
        /// <summary>
        /// コントロールを識別するコードで使われる名前です。
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// ボタン画像を示します。
        /// </summary>
        public Image Image { get; set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CButtonColumn()
        {
            this.CellTemplate = new CButtonCell();

            this.ControlName = "";
            this.Image = null;
        }
        #endregion

        #region メソッド (基底)
        /// <summary>
        /// このバンドの同一コピーを作成します。
        /// </summary>
        /// <returns>複製された DataGridViewBand を表すオブジェクト</returns>
        /// <remarks>
        /// プロパティを追加しているため、Cloneメソッドをオーバーライドする必要があります。
        /// </remarks>
        public override object Clone()
        {
            var col = base.Clone() as CButtonColumn;
            if (col != null)
            {
                col.ControlName = this.ControlName;
                col.Image = this.Image;
            }

            return col;
        }
        #endregion
    }
    #endregion



    #region CButtonCellクラス
    /// <summary>
    /// DataGridViewButtonCell拡張クラス
    /// </summary>
    public class CButtonCell : DataGridViewButtonCell
    {
        #region イベント
        /// <summary>
        /// セルの描画を行います。
        /// </summary>
        /// <param name="graphics">セルの描画に使用する Graphics</param>
        /// <param name="clipBounds">再描画が必要な DataGridView の領域を表す Rectangle</param>
        /// <param name="cellBounds">描画されるセルの境界が格納された Rectangle</param>
        /// <param name="rowIndex">描画されるセルの行インデックス</param>
        /// <param name="elementState">セルの状態を指定する DataGridViewElementStates 値のビットごとの組み合わせ</param>
        /// <param name="Value">描画されるセルのデータ</param>
        /// <param name="formattedValue">描画されるセルの書式指定済みデータ</param>
        /// <param name="errorText">セルに関連付けられたエラーメッセージ</param>
        /// <param name="cellStyle">セルに関する書式とスタイルの情報を格納する DataGridViewCellStyle</param>
        /// <param name="advancedBorderStyle">描画されるセルの境界線スタイルが格納された DataGridViewAdvancedBorderStyle</param>
        /// <param name="paintParts">セルのどの部分を描画する必要があるのかを指定する、DataGridViewPaintParts 値のビットごとの組み合わせ</param>
        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates elementState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            var col = this.OwningColumn as CButtonColumn;
            if (col != null)
            {
                if (col.Image != null)
                {
                    var w = col.Image.Width - 2;
                    var h = col.Image.Height - 2;
                    var x = cellBounds.Left + ((cellBounds.Width - w) / 2);
                    var y = cellBounds.Top + ((cellBounds.Height - h) / 2);

                    graphics.DrawImage(col.Image, new Rectangle(x, y, w - 1, h - 1));
                }
            }
        }
        #endregion
    }
    #endregion
}
