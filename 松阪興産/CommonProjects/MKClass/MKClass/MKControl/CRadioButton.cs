using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKClass.MKControl
{
    /// <summary>
    /// RadioButton拡張クラス
    /// </summary>
    public class CRadioButton : RadioButton
    {
        #region プロパティ (基底)
        /// <summary>
        /// コンポーネントの背景色です。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        #endregion

        #region プロパティ (追加)
        private Color _baseBackColor = SystemColors.Control;
        /// <summary>
        /// 標準の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("標準の背景色です。")]
        [DefaultValue(typeof(Color), "Control")]
        public Color BaseBackColor
        {
            get
            {
                return _baseBackColor;
            }
            set
            {
                _baseBackColor = value;

                base.BackColor = _baseBackColor;
            }
        }

        private Color _activeBackColor = CColor.ActiveBackColor;
        /// <summary>
        /// アクティブ時の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("アクティブ時の背景色です。")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color ActiveBackColor
        {
            get
            {
                return _activeBackColor;
            }
            set
            {
                _activeBackColor = value;
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            base.BackColor = _activeBackColor;
        }

        /// <summary>
        /// Enabledプロパティ値が変更されたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            SetBackColor();
        }

        /// <summary>
        /// コントロールの検証が終了すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);

            SetBackColor();
        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// 背景色を設定します。
        /// </summary>
        /// <remarks>
        /// アクティブ時の背景色の設定は行いません。
        /// </remarks>
        public void SetBackColor()
        {
            base.BackColor = (this.Enabled) ? _baseBackColor : CColor.DisableBackColor;
        }
        #endregion
    }
}
