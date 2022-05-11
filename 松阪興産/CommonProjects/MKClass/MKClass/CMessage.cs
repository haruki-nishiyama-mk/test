using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKClass
{
    /// <summary>
    /// メッセージクラス (static)
    /// </summary>
    public static class CMessage
    {
        #region メッセージ表示
        /// <summary>
        /// インフォメーションを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowInformationMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// 質問メッセージを表示します。 (OK選択)
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        /// <param name="defaultButton">既定のボタン</param>
        /// <returns>
        /// true : ダイアログボックスの戻り値に対して一致 (OK選択)
        /// false: それ以外
        /// </returns>
        public static bool ShowQuestionMessageOK(string text,
                                                 string caption = "",
                                                 MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            if (MessageBox.Show(text,
                                caption,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question,
                                defaultButton) == DialogResult.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 質問メッセージを表示します。 (キャンセル選択)
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        /// <param name="defaultButton">既定のボタン</param>
        /// <returns>
        /// true : ダイアログボックスの戻り値に対して一致
        /// false: それ以外
        /// </returns>
        public static bool ShowQuestionMessageCancel(string text,
                                                     string caption = "",
                                                     MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            if (MessageBox.Show(text,
                                caption,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question,
                                defaultButton) == DialogResult.Cancel)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowWarningMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowErrorMessage(string text, string caption = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            MessageBox.Show(text,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
        #endregion
    }
}
