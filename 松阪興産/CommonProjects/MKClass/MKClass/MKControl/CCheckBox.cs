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
    /// CheckBox拡張クラス
    /// </summary>
    public class CCheckBox : CheckBox, IControlChanged
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

        private bool _isChanged = false;
        /// <summary>
        /// 変更状態を取得します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        bool IControlChanged.IsChanged
        {
            get
            {
                return _isChanged;
            }
        }

        private bool _isTempChanged = false;
        /// <summary>
        /// Validatedが発生するまでの一時的な変更状態を取得します。
        /// </summary>
        /// <remarks>
        /// デザイナー上のプロパティには表示しません。
        /// </remarks>
        [Browsable(false)]
        bool IControlChanged.IsTempChanged
        {
            get
            {
                return _isTempChanged;
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
        /// CheckStateプロパティの値が変化したときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnCheckStateChanged(EventArgs e)
        {
            base.OnCheckStateChanged(e);

            _isChanged = true;
            _isTempChanged = true;
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

            _isTempChanged = false;
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

        /// <summary>
        /// 変更状態をクリアします。
        /// </summary>
        void IControlChanged.Clear()
        {
            _isChanged = false;
            _isTempChanged = false;
        }
        #endregion
    }
}
