using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKODDS
{
    /// <summary>
    /// ODDS 基本フォームクラス
    /// </summary>
    public partial class BaseForm : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }
        #endregion
    }
}
