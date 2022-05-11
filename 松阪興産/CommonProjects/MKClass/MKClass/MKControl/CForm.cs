using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MKClass.MKControl
{
    #region Form拡張クラス (static)
    /// <summary>
    /// Form拡張クラス (static)
    /// </summary>
    public static class CForm
    {
        #region プロパティ
        /// <summary>入力コントロールの検証状態</summary>
        public static ControlValidationStatus ValidationStatus { get; private set; }
        #endregion

        #region フォーム関連
        /// <summary>
        /// フォーム共通の設定を行います。
        /// </summary>
        /// <param name="self">対象フォーム</param>
        /// <remarks>
        /// Form.Loadイベントでのコールを推奨します。
        /// </remarks>
        public static void CustomizeForm(this Form self)
        {
            try
            {
                // フォームが処理中かどうかを検出できるようにする
                self.ActivateProcessingDetection();

                ////// プロパティ
                self.KeyPreview = true;
                self.StartPosition = FormStartPosition.CenterScreen;

                ////// イベント
                //// Enterキーでフォーカスを移動する
                self.KeyDown += (sender, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        if (self.ActiveControl is TextBox)
                        {
                            TextBox activeTB = (TextBox)self.ActiveControl;
                            if (activeTB.Multiline)
                            {
                                if (activeTB.AcceptsReturn)
                                {
                                    return;
                                }
                                else if (!activeTB.AcceptsReturn && e.Modifiers == Keys.Control)
                                {
                                    return;
                                }
                            }
                        }
                        else if (self.ActiveControl is CDataGridView)
                        {
                            // CDataGridViewの実装と重複するため、何もしない
                            return;
                        }

                        if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
                        {
                            bool forward = (e.Modifiers != Keys.Shift);
                            self.SelectNextControl(self.ActiveControl, forward, true, true, false);
                        }
                    }
                };

                SetButtonShortcutKeyEvent(self);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// フォームをアクティブ化します。
        /// </summary>
        /// <param name="self">対象フォーム</param>
        public static void SetActiveWindow(this Form self)
        {
            try
            {
                self.Activate();

                self.TopMost = true;
                self.TopMost = false;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 指定したコントロールのフォームを取得します。
        /// </summary>
        /// <param name="control">指定のコントロール</param>
        /// <returns>フォーム</returns>
        public static Form GetForm(Control control)
        {
            try
            {
                return (control.Parent) is Form ? (Form)control.Parent : GetForm(control.Parent);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region コントロールリスト取得
        /// <summary>
        /// 全てのジェネリック型Tコントロールを取得します。
        /// </summary>
        /// <typeparam name="T">取得したいコントロール</typeparam>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <returns>コントロールリスト</returns>
        public static List<T> GetAllControls<T>(Control hParent) where T : Control
        {
            try
            {
                List<T> buf = new List<T>();
                foreach (Control c in hParent.Controls.Cast<Control>().OrderBy(c => c.TabIndex).ToList())
                {
                    if (c is T)
                    {
                        buf.Add((T)c);
                    }

                    buf.AddRange(GetAllControls<T>(c));
                }

                return buf;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region コントロール取得
        /// <summary>
        /// コントロールを取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="name">コントロール名</param>
        /// <returns>コントロール</returns>
        public static Control GetControl(this Control hParent, string name)
        {
            try
            {
                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    if (c.Name == name)
                    {
                        return c;
                    }
                }

                return null;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region イベント関連
        /// <summary>
        /// ショートカットキーが設定されているCButtonに対して、イベントハンドラを割り当てます。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <remarks>
        /// CustomizeFormメソッドをコールした場合、使用しないでください。
        /// Form.Loadイベントでのコールを推奨します。
        /// </remarks>
        public static void SetButtonShortcutKeyEvent(Control hParent)
        {
            try
            {
                foreach (CButton c in GetAllControls<CButton>(hParent))
                {
                    c.SetShortcutKeyEvent();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 指定したコントロールに対して、イベントハンドラを割り当てます。
        /// </summary>
        /// <typeparam name="T">イベントハンドラを割り当てたいコントロール</typeparam>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="method">メソッド</param>
        public static void SetControlEvent<T>(Control hParent, Action<T> method) where T : Control
        {
            try
            {
                foreach (T c in GetAllControls<T>(hParent))
                {
                    method(c);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 処理中検出
        private static bool _enableProcessingDetection = false;
        private static bool _isProcessing = false;

        /// <summary>
        /// フォームが処理中かどうかを検出できるようにします。
        /// </summary>
        /// <param name="self">対象フォーム</param>
        /// <remarks>
        /// CustomizeFormメソッドをコールした場合、使用しないでください。
        /// Form.Loadイベントでのコールを推奨します。
        /// </remarks>
        public static void ActivateProcessingDetection(this Form self)
        {
            _enableProcessingDetection = true;

            Application.Idle += Application_Idle;

            self.Disposed += (s, e) => Application.Idle -= Application_Idle;
        }

        /// <summary>
        /// アプリケーションが処理を完了し、アイドル状態に入ろうとすると発生します。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private static void Application_Idle(object sender, EventArgs e)
        {
            _isProcessing = false;
        }

        /// <summary>
        /// フォームが処理中かどうかを取得します。
        /// </summary>
        /// <param name="self">対象フォーム</param>
        /// <returns>
        /// true : 処理中
        /// false: それ以外
        /// </returns>
        public static bool IsProcessing(this Form self)
        {
            if (_enableProcessingDetection)
            {
                if (_isProcessing)
                {
                    return true;
                }

                _isProcessing = true;
            }

            return false;
        }
        #endregion

        #region 必須コントロールの取得
        /// <summary>
        /// 必須コントロールの配列を取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <returns>必須コントロールの配列</returns>
        /// <remarks>
        /// IsRequiredプロパティを実装したコントールのみ、使用できます。
        /// </remarks>
        public static Control[] GetRequiredControls(Control hParent)
        {
            try
            {
                ArrayList buf = new ArrayList();

                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    PropertyInfo pi = c.GetType().GetProperty("IsRequired");
                    if (pi != null)
                    {
                        if ((bool)pi.GetValue(c))
                        {
                            buf.Add(c);
                        }
                    }
                }

                return (Control[])buf.ToArray(typeof(Control));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region BackColorの初期化
        /// <summary>
        /// コントロールのBackColorプロパティを初期化します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        public static void InitBackColor(Control hParent)
        {
            try
            {
                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    if (c is CTextBox)
                    {
                        ((CTextBox)c).SetBackColor();
                    }
                    else if (c is CMaskedTextBox)
                    {
                        ((CMaskedTextBox)c).SetBackColor();
                    }
                    else if (c is TextBoxBase)
                    {
                        c.BackColor = SystemColors.Window;
                    }
                    else if (c is CComboBox)
                    {
                        ((CComboBox)c).SetBackColor();
                    }
                    else if (c is ComboBox)
                    {
                        c.BackColor = SystemColors.Window;
                    }
                    else if (c is CCheckBox)
                    {
                        ((CCheckBox)c).SetBackColor();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region クリア処理
        /// <summary>
        /// 入力コントロールをクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="ignoreControls">クリア対象外のコントロール名リスト</param>
        public static void ClearControl(Control hParent, List<string> ignoreControls = null)
        {
            try
            {
                ClearControl(hParent,
                                name => { return (ignoreControls != null && ignoreControls.Count > 0) ? ignoreControls.Contains(name) : false; });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 入力コントロールをクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="pred">クリア対象外条件</param>
        private static void ClearControl(Control hParent, Func<string, bool> pred)
        {
            try
            {
                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    if (pred(c.Name))
                    {
                        continue;
                    }

                    // TextBoxBase からの派生型
                    if (c is TextBoxBase)
                    {
                        c.Text = "";
                    }
                    // ComboBox からの派生型
                    else if (c is ComboBox)
                    {
                        ((ComboBox)c).SelectedIndex = -1;
                        c.Text = "";
                    }
                    // CheckBox からの派生型
                    else if (c is CheckBox)
                    {
                        ((CheckBox)c).Checked = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ラベル (コンテンツ) をクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="ignoreControls">クリア対象外のコントロール名リスト</param>
        /// <remarks>
        /// キー (IDやコードなど) に関連する値を格納するラベルを示します。
        /// 開発規約の命名規則に従っていることが前提です。
        /// Control.Nameプロパティの末尾がアンダーライン(_)であることが前提です。
        /// </remarks>
        public static void ClearContentLabel(Control hParent, List<string> ignoreControls = null)
        {
            try
            {
                ClearContentLabel(hParent,
                                    name => { return (ignoreControls != null && ignoreControls.Count > 0) ? ignoreControls.Contains(name) : false; });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ラベル (コンテンツ) をクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="pred">クリア対象外条件</param>
        /// <remarks>
        /// キー (IDやコードなど) に関連する値を格納するラベルを示します。
        /// 開発規約の命名規則に従っていることが前提です。
        /// Control.Nameプロパティの末尾がアンダーライン(_)であることが前提です。
        /// </remarks>
        private static void ClearContentLabel(Control hParent, Func<string, bool> pred)
        {
            try
            {
                foreach (Label lbl in CForm.GetAllControls<Label>(hParent))
                {
                    if (pred(lbl.Name))
                    {
                        continue;
                    }

                    // ex. lblSample_
                    if (Regex.IsMatch(lbl.Name, @"^lbl.*_$"))
                    {
                        lbl.Text = "";
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region コントロールに関連するラベル情報を取得
        /// <summary>
        /// コントロールに関連するラベル (タイトル) の名称を取得します。
        /// </summary>
        /// <param name="control">取得したいラベル (タイトル) に関連するコントロール</param>
        /// <returns>ラベル (タイトル) の名称</returns>
        /// <remarks>
        /// 開発規約の命名規則に従っていることが前提です。
        /// </remarks>
        public static string GetCaptionLabelName(Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample
                return Regex.Replace(control.Name, "^[a-z]+", "lbl");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールに関連するラベル (タイトル) コントロールを取得します。
        /// </summary>
        /// <param name="self">対象コントロール</param>
        /// <param name="control">取得したいラベル (タイトル) に関連するコントロール</param>
        /// <returns>ラベル (タイトル) コントロール</returns>
        /// <remarks>
        /// 開発規約の命名規則に従っていることが前提です。
        /// </remarks>
        public static Label GetCaptionLabel(this Control self, Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample
                Label label = self.GetControl(GetCaptionLabelName(control)) as Label;

                return (label != null) ? label : null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールに関連するラベル (タイトル) のTextプロパティを取得します。
        /// </summary>
        /// <param name="self">対象コントロール</param>
        /// <param name="control">取得したいラベル (タイトル) に関連するコントロール</param>
        /// <returns>ラベル (タイトル) のTextプロパティ</returns>
        /// <remarks>
        /// 開発規約の命名規則に従っていることが前提です。
        /// </remarks>
        public static string GetCaptionLabelText(this Control self, Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample
                Label label = self.GetCaptionLabel(control);

                return (label != null) ? label.Text : "";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールに関連するラベル (コンテンツ) の名称を取得します。
        /// </summary>
        /// <param name="control">取得したいラベル (コンテンツ) に関連するコントロール</param>
        /// <returns>ラベル (コンテンツ) の名称</returns>
        /// <remarks>
        /// キー (IDやコードなど) に関連する値を格納するラベルを示します。
        /// 開発規約の命名規則に従っていることが前提です。
        /// Control.Nameプロパティの末尾がアンダーライン(_)であることが前提です。
        /// </remarks>
        public static string GetContentLabelName(Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample_
                return Regex.Replace(control.Name, "^[a-z]+", "lbl") + "_";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールに関連するラベル (コンテンツ) コントロールを取得します。
        /// </summary>
        /// <param name="self">対象コントロール</param>
        /// <param name="control">取得したいラベル (コンテンツ) に関連するコントロール</param>
        /// <returns>ラベル (コンテンツ) コントロール</returns>
        /// <remarks>
        /// キー (IDやコードなど) に関連する値を格納するラベルを示します。
        /// 開発規約の命名規則に従っていることが前提です。
        /// Control.Nameプロパティの末尾がアンダーライン(_)であることが前提です。
        /// </remarks>
        public static Label GetContentLabel(this Control self, Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample_
                Label label = self.GetControl(GetContentLabelName(control)) as Label;

                return (label != null) ? label : null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// コントロールに関連するラベル (コンテンツ) のTextプロパティを取得します。
        /// </summary>
        /// <param name="self">対象コントロール</param>
        /// <param name="control">取得したいラベル (コンテンツ) に関連するコントロール</param>
        /// <returns>ラベル (コンテンツ) のTextプロパティ</returns>
        /// <remarks>
        /// キー (IDやコードなど) に関連する値を格納するラベルを示します。
        /// 開発規約の命名規則に従っていることが前提です。
        /// Control.Nameプロパティの末尾がアンダーライン(_)であることが前提です。
        /// </remarks>
        public static string GetContentLabelText(this Control self, Control control)
        {
            try
            {
                // ex. ctbSample -> lblSample_
                Label label = self.GetContentLabel(control);

                return (label != null) ? label.Text : "";
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 変更管理
        /// <summary>
        /// 入力コントロールの変更状態をクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="ignoreControls">クリア対象外のコントロール名リスト</param>
        public static void ClearControlChanged(Control hParent, List<string> ignoreControls = null)
        {
            try
            {
                ClearControlChanged(hParent,
                                name => { return (ignoreControls != null && ignoreControls.Count > 0) ? ignoreControls.Contains(name) : false; });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 入力コントロールの変更状態をクリアします。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="pred">クリア対象外条件</param>
        private static void ClearControlChanged(Control hParent, Func<string, bool> pred)
        {
            try
            {
                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    if (pred(c.Name))
                    {
                        continue;
                    }

                    if (c is IControlChanged)
                    {
                        ((IControlChanged)c).Clear();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 入力コントロールの変更状態を取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="ignoreControls">取得対象外のコントロール名リスト</param>
        public static bool GetControlChanged(Control hParent, List<string> ignoreControls = null)
        {
            try
            {
                return GetControlChanged(hParent,
                                name => { return (ignoreControls != null && ignoreControls.Count > 0) ? ignoreControls.Contains(name) : false; });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 入力コントロールの変更状態を取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="pred">取得対象外条件</param>
        /// <returns>
        /// true : 変更あり
        /// false: それ以外
        /// </returns>
        private static bool GetControlChanged(Control hParent, Func<string, bool> pred)
        {
            try
            {
                foreach (Control c in GetAllControls<Control>(hParent))
                {
                    if (pred(c.Name))
                    {
                        continue;
                    }

                    if (c is IControlChanged)
                    {
                        if (((IControlChanged)c).IsChanged)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region バリデーション
        /// <summary>
        /// 入力コントロールで、バリデーションエラーのコントロールの配列を取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="ignoreControls">検証対象外のコントロール名リスト</param>
        /// <returns>バリデーションエラーのコントロールの配列</returns>
        public static IEnumerable<Control> GetValidationErrorContols(Control hParent, List<string> ignoreControls = null)
        {
            return GetValidationErrorContols(hParent,
                            name => { return (ignoreControls != null && ignoreControls.Count > 0) ? ignoreControls.Contains(name) : false; });
        }

        /// <summary>
        /// 入力コントロールで、バリデーションエラーのコントロールの配列を取得します。
        /// </summary>
        /// <param name="hParent">走査対象となる親コントロール</param>
        /// <param name="pred">検証対象外条件</param>
        /// <returns>バリデーションエラーのコントロールの配列</returns>
        /// <remarks>
        /// エラーの原因をValidationStatusプロパティに設定します。
        /// </remarks>
        private static IEnumerable<Control> GetValidationErrorContols(Control hParent, Func<string, bool> pred)
        {
            foreach (Control c in GetAllControls<Control>(hParent))
            {
                ValidationStatus = ControlValidationStatus.Normal;
                if (pred(c.Name))
                {
                    continue;
                }

                if (c is CTextBox)
                {
                    ValidationStatus = ((CTextBox)c).Validation();
                    if (ValidationStatus != ControlValidationStatus.Normal)
                    {
                        yield return c;
                    }
                }
                else if (c is CComboBox)
                {
                    ValidationStatus = ((CComboBox)c).Validation();
                    if (ValidationStatus != ControlValidationStatus.Normal)
                    {
                        yield return c;
                    }
                }
            }

            yield break;
        }
        #endregion
    }
    #endregion

    #region 画面描画制御クラス
    /// <summary>
    /// 画面描画制御クラス
    /// </summary>
    /// <remarks>
    /// フォームの画面描画抑制を制御するオブジェクトを提供するクラスです。
    /// </remarks>
    public class FormRedrawSuspension : IDisposable
    {
        /// <summary>対象フォーム</summary>
        private Form _form;

        /// <summary>
        /// オブジェクトを構築し、画面描画を抑制します。
        /// オブジェクトを破棄すると画面描画抑制が解除されます。
        /// </summary>
        /// <param name="form">対象フォーム</param>
        public FormRedrawSuspension(Form form)
        {
            _form = form;

            SendSetRedrawMessage(false);
        }

        /// <summary>
        /// 画面描画抑制を解除してフォームを再描画させ、オブジェクトを破棄します。
        /// </summary>
        public void Dispose()
        {
            SendSetRedrawMessage(true);

            _form.Refresh();
        }

        private void SendSetRedrawMessage(bool enableRedraw)
        {
            NativeMethods.SendMessage(_form.Handle, NativeMethods.WM_SETREDRAW, enableRedraw ? new IntPtr(1) : IntPtr.Zero, IntPtr.Zero);
        }

        private class NativeMethods
        {
            // https://docs.microsoft.com/ja-jp/visualstudio/code-quality/ca1060-move-p-invokes-to-nativemethods-class
            private NativeMethods() { }

            [DllImport("user32.dll")]
            internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 dwMsg, IntPtr wParam, IntPtr lParam);

            internal const UInt32 WM_SETREDRAW = 11;
        }
    }
    #endregion
}
