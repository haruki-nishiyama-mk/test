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
    /// MaskedTextBox拡張クラス
    /// </summary>
    public class CMaskedTextBox : MaskedTextBox, IControlChanged
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
        private Color _baseBackColor = SystemColors.Window;
        /// <summary>
        /// 標準の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("標準の背景色です。")]
        [DefaultValue(typeof(Color), "Window")]
        public Color BaseBackColor
        {
            get
            {
                return _baseBackColor;
            }
            set
            {
                _baseBackColor = value;

                if (!base.ReadOnly && !_isRequired)
                {
                    base.BackColor = _baseBackColor;
                }
            }
        }

        private Color _readOnlyBackColor = CColor.ReadOnlyBackColor;
        /// <summary>
        /// 読み取り専用項目の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("読み取り専用項目の背景色です。")]
        [DefaultValue(typeof(Color), "Info")]
        public Color ReadOnlyBackColor
        {
            get
            {
                return _readOnlyBackColor;
            }
            set
            {
                _readOnlyBackColor = value;

                if (base.ReadOnly)
                {
                    base.BackColor = _readOnlyBackColor;
                }
            }
        }

        private Color _requiredBackColor = CColor.RequiredBackColor;
        /// <summary>
        /// 必須項目の背景色です。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("必須項目の背景色です。")]
        [DefaultValue(typeof(Color), "LightCyan")]
        public Color RequiredBackColor
        {
            get
            {
                return _requiredBackColor;
            }
            set
            {
                _requiredBackColor = value;

                if (!base.ReadOnly && _isRequired)
                {
                    base.BackColor = _requiredBackColor;
                }
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

        private bool _selectAll = true;
        private bool _isSelectAll = true;
        /// <summary>
        /// テキストボックスの全てのテキストを選択するかどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("テキストボックスの全てのテキストを選択するかどうかを示します。")]
        [DefaultValue(true)]
        public bool IsSelectAll
        {
            get
            {
                return _isSelectAll;
            }
            set
            {
                _isSelectAll = value;
            }
        }

        private bool _isRequired = false;
        /// <summary>
        /// 必須かどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("必須かどうかを示します。")]
        [DefaultValue(false)]
        public bool IsRequired
        {
            get
            {
                return _isRequired;
            }
            set
            {
                _isRequired = value;

                base.BackColor = (base.ReadOnly) ? _readOnlyBackColor : ((_isRequired) ? _requiredBackColor : _baseBackColor);
            }
        }

        private ToolStripLabel _toolStripLabel = null;
        /// <summary>
        /// ツールバーに使用するToolStripLabelを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("ツールバーに使用するToolStripLabelを示します。")]
        [DefaultValue(null)]
        public ToolStripLabel ToolStripLabel
        {
            get
            {
                return _toolStripLabel;
            }
            set
            {
                _toolStripLabel = value;
            }
        }

        private string _toolStripLabelText = "";
        /// <summary>
        /// ToolStripLabelに関連付けられたテキストです。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("ToolStripLabelに関連付けられたテキストです。")]
        [DefaultValue("")]
        public string ToolStripLabelText
        {
            get
            {
                return _toolStripLabelText;
            }
            set
            {
                _toolStripLabelText = value;
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

            if (_isSelectAll)
            {
                SelectAll();
            }
            else
            {
                this.Select(this.Text.Length, 0);
            }

            if (!this.ReadOnly)
            {
                base.BackColor = _activeBackColor;
            }

            if (_toolStripLabel != null)
            {
                _toolStripLabel.Text = _toolStripLabelText;
            }
        }

        /// <summary>
        /// マウスポインタによってコントロールが入力されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!this.Focused)
                _selectAll = true;
            else
                _selectAll = false;
        }

        /// <summary>
        /// マウスポインタがコントロール上にある状態でマウスボタンが離されると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isSelectAll && _selectAll)
            {
                SelectAll();
            }

            _selectAll = false;
        }

        /// <summary>
        /// Textプロパティの値が変化すると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _isChanged = true;
            _isTempChanged = true;
        }

        /// <summary>
        /// 入力フォーカスがコントロールを離れると発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            _selectAll = true;
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
        /// ReadOnlyプロパティの値が変更されたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);

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

            if (_toolStripLabel != null)
            {
                _toolStripLabel.Text = "";
            }

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
            if (!this.Enabled)
            {
                base.BackColor = CColor.DisableBackColor;
            }
            else if (this.ReadOnly)
            {
                base.BackColor = _readOnlyBackColor;
            }
            else
            {
                base.BackColor = (_isRequired) ? _requiredBackColor : _baseBackColor;
            }
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
