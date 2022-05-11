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
    /// Button拡張クラス
    /// </summary>
    public class CButton : Button
    {
        #region プロパティ (追加)
        private Keys _shortcutKey = Keys.None;
        /// <summary>
        /// 割り当てるショートカットキーを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("割り当てるショートカットキーを取得または設定します。")]
        [DefaultValue(Keys.None)]
        public Keys ShortcutKey
        {
            get
            {
                return _shortcutKey;
            }
            set
            {
                Keys keyModifiers = (value & Keys.Modifiers);
                if ((keyModifiers & Keys.Alt) > 0)
                {
                    throw new Exception("修飾子キーにAltは指定できません。");
                }

                _shortcutKey = value;
            }
        }

        private bool _processingEnabled = true;
        /// <summary>
        /// (イベント処理中) コントロールがユーザーとの対話に応答できるかどうかを示す値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("(イベント処理中) コントロールがユーザーとの対話に応答できるかどうかを示す値を取得または設定します。")]
        [DefaultValue(true)]
        public bool ProcessingEnabled
        {
            get
            {
                return _processingEnabled;
            }
            set
            {
                _processingEnabled = value;
            }
        }

        private bool _selectable = true;
        /// <summary>
        /// コントロールがフォーカスを受け取ることがどうかを示します。
        /// </summary>
        [Browsable(true)]
        [Category("(拡張)")]
        [Description("コントロールがフォーカスを受け取ることがどうかを示します。")]
        [DefaultValue(true)]
        public bool Selectable
        {
            get
            {
                return _selectable;
            }
            set
            {
                _selectable = value;

                if (_selectable)
                {
                    SetStyle(ControlStyles.Selectable, true);
                }
                else
                {
                    SetStyle(ControlStyles.Selectable, false);
                }
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールがクリックされたときに発生します。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        protected override void OnClick(EventArgs e)
        {
            if (CForm.GetForm(this).IsProcessing())
            {
                return;
            }

            if (!_processingEnabled)
            {
                this.Enabled = false;
            }

            try
            {
                base.OnClick(e);
            }
            finally
            {
                if (!_processingEnabled)
                {
                    this.Enabled = true;
                }
            }
        }
        #endregion

        #region メソッド (追加)
        /// <summary>
        /// ショートカットキーを押したときのイベントハンドラを割り当てます。
        /// </summary>
        public void SetShortcutKeyEvent()
        {
            if (_shortcutKey == Keys.None)
            {
                return;
            }

            Form form = CForm.GetForm(this);
            form.KeyDown += (sender_, e_) =>
            {
                if (e_.KeyData == _shortcutKey)
                {
                    if (!this.Enabled)
                    {
                        return;
                    }

                    Control ac = form.ActiveControl;

                    bool goEvent = true;
                    if (ac is TextBox)
                    {
                        if (((TextBox)ac).Multiline)
                        {
                            goEvent = false;
                        }
                        else if (ac is CTextBox)
                        {
                            goEvent = ((CTextBox)ac).DoEvent;
                        }
                        else if (ac is CTextBoxEditingControl)
                        {
                            goEvent = ((CTextBoxEditingControl)ac).DoEvent;
                        }
                        else if (ac is CComboBoxEditingControl)
                        {
                            goEvent = ((CComboBoxEditingControl)ac).DoEvent;
                        }
                    }

                    if (goEvent)
                    {
                        if (_selectable)
                        {
                            this.Focus();

                            if (!this.Focused)
                            {
                                // バリデーションエラーなどで、フォーカスが移動できなかった場合を考慮
                                goEvent = false;
                            }
                        }

                        if (goEvent)
                        {
                            this.OnClick(new EventArgs());
                        }
                    }

                    // ドロップダウンウィンドウの表示を制御
                    if (ac is ComboBox || ac is DateTimePicker)
                    {
                        if (e_.KeyCode == Keys.F4)
                        {
                            e_.Handled = true;
                        }
                    }

                    // Windowsメニューの表示を制御
                    if (e_.KeyCode == Keys.F10)
                    {
                        e_.Handled = true;
                    }
                }
            };
        }
        #endregion
    }
}
