using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKClass.MKControl
{
    /// <summary>
    /// ContextMenuStrip拡張クラス
    /// </summary>
    public class CContextMenuStrip : ContextMenuStrip
    {
        #region イベント
        /// <summary>
        /// マウスポインタがコントロール上にある状態でマウスボタンが離されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                base.OnMouseUp(e);
            }
        }
        #endregion
    }
}
