using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKODDS
{
    /// <summary>
    /// Panel拡張クラス (処理中のパネル表示)
    /// </summary>
    public partial class CPanelProcessing : UserControl
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CPanelProcessing()
            : this("")
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        public CPanelProcessing(string message)
        {
            InitializeComponent();

            lblMessage.Text = message;
        }
        #endregion
    }
}
