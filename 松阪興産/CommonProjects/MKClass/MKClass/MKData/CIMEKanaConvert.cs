using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using MKClass.MKControl;

namespace MKClass.MKData
{
    /// <summary>
    /// IME変換イベントのデータを提供します。
    /// </summary>
    public class IMECompositionEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>読み仮名を示します。</summary>
        public string Kana { get; private set; }
        /// <summary>Singleバイト (半角) かどうかを示します。</summary>
        public bool IsSByte { get; private set; }
        /// <summary>読み仮名の文字数を示します。</summary>
        public int KanaLength { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kana">読み仮名</param>
        /// <param name="isSByte">Singleバイト (半角) かどうか</param>
        /// <param name="kanaLength">読み仮名の文字数</param>
        public IMECompositionEventArgs(string kana, bool isSByte, int kanaLength)
        {
            this.Kana = kana;
            this.IsSByte = isSByte;
            this.KanaLength = kanaLength;
        }
        #endregion
    }

    /// <summary>
    /// 読み仮名変換クラス
    /// </summary>
    public class CIMEKanaConvert
    {
        private const int WM_CHAR = 0x102;

        private const int WM_IME_STARTCOMPOSITION = 0x10D;
        private const int WM_IME_ENDCOMPOSITION = 0x10E;
        private const int WM_IME_COMPOSITION = 0x10F;
        private const int GCS_COMPREADSTR = 0x0001;
        private const int GCS_COMPREADATTR = 0x0002;
        private const int GCS_COMPREADCLAUSE = 0x0004;
        private const int GCS_COMPSTR = 0x0008;
        private const int GCS_COMPATTR = 0x0010;
        private const int GCS_COMPCLAUSE = 0x0020;
        private const int GCS_CURSORPOS = 0x0080;
        private const int GCS_DELTASTART = 0x0100;
        private const int GCS_RESULTREADSTR = 0x0200;
        private const int GCS_RESULTREADCLAUSE = 0x0400;
        private const int GCS_RESULTSTR = 0x0800;
        private const int GCS_RESULTCLAUSE = 0x1000;

        private const int IMM_ERROR_NODATA = -1;
        private const int IMM_ERROR_GENERAL = -2;

        private Control _baseControl = null;
        private Control _targetControl = null;

        private VbStrConv _convertType = VbStrConv.Hiragana;

        private WndMsgListener _wndMsgListener;

        // コンテキストハンドルの取得
        [DllImport("Imm32.dll")]
        private static extern int ImmGetContext(int hWnd);

        // コンテキストハンドルの開放
        [DllImport("Imm32.dll")]
        private static extern int ImmReleaseContext(int hWnd, int hIMC);

        // 変換中の文字列の情報を取得
        [DllImport("Imm32.dll")]
        private static extern int ImmGetCompositionString(int hIMC, int dwIndex, StringBuilder lpBuf, int dwBufLen);

        // IMEのオープン状態を取得
        [DllImport("Imm32.dll")]
        private static extern int ImmGetOpenStatus(int hIMC);

        #region プロパティ
        /// <summary>
        /// メッセージ監視が有効かどうかを示します。
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _wndMsgListener.Enabled;
            }
            set
            {
                _wndMsgListener.Enabled = value;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="baseControl">変換元コントロール</param>
        /// <param name="targetControl">変換先コントロール</param>
        /// <param name="convertType">変換の種類</param>
        public CIMEKanaConvert(Control baseControl, Control targetControl, VbStrConv convertType)
        {
            _baseControl = baseControl;
            _targetControl = targetControl;
            _convertType = convertType;

            _wndMsgListener = new WndMsgListener(_baseControl);
            _wndMsgListener.OnConverted += new WndMsgListener.Converted(Converted);
        }
        #endregion

        #region 文字変換・セット
        /// <summary>
        /// 文字を変換し、変換先コントロールにセットします。
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
        /// <param name="e">イベントデータ</param>
        private void Converted(object sender, IMECompositionEventArgs e)
        {
            string convertedText = _targetControl.Text;
            if (_baseControl.Text.Length == 0)
            {
                convertedText = "";
            }

            string ret = "";
            if (e.IsSByte)
            {
                ret = e.Kana;
            }
            else
            {
                ret = Strings.StrConv(e.Kana, VbStrConv.Wide, 0x411);

                ret = Strings.StrConv(ret, _convertType, 0);

                if (_convertType == VbStrConv.Katakana)
                {
                    if (_convertType == VbStrConv.Wide)
                    {
                        ret = CConvert.ConvertToKatakana(ret);
                    }
                    else
                    {
                        ret = CConvert.ConvertToKatakanaHalf(ret);
                    }
                }
            }
            convertedText += ret;

            // CTextBox からの派生型
            if (_targetControl is CTextBox)
            {
                CTextBox ctb = (CTextBox)_targetControl;
                ctb.Text = (ctb.IsLengthInOfRange(convertedText)) ? convertedText : convertedText.Substring(0, ctb.MaxLength);
            }
            // TextBoxBase からの派生型
            else if (_targetControl is TextBoxBase)
            {
                TextBoxBase tbb = (TextBoxBase)_targetControl;
                tbb.Text = (convertedText.Length <= tbb.MaxLength) ? convertedText : convertedText.Substring(0, tbb.MaxLength);
            }
            else
            {
                _targetControl.Text = convertedText;
            }
        }
        #endregion

        #region Convertedイベント (WndMsgListener)
        /// <summary>
        /// Windowメッセージリスナークラス
        /// </summary>
        /// <remarks>
        /// 漢字入力のコントロールに送られるメッセージをチェックします。
        /// </remarks>
        private class WndMsgListener : NativeWindow
        {
            /// <summary>メッセージ監視が有効かどうかを示します。</summary>
            public bool Enabled { get; set; }

            /// <summary>
            /// Convertedイベントハンドラ
            /// </summary>
            /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
            /// <param name="e">イベントデータ</param>
            public delegate void Converted(Object sender, IMECompositionEventArgs e);

            /// <summary>
            /// 文字変換時に発生します。
            /// </summary>
            public event Converted OnConverted;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="c">コントロール</param>
            public WndMsgListener(Control c)
            {
                AssignHandle(c.Handle);

                c.HandleDestroyed += new EventHandler(OnHandleDestroyed);
            }

            /// <summary>
            /// コントロールのハンドルが破棄されているときに発生します。
            /// </summary>
            /// <param name="sender">イベントを発生させたオブジェクトへの参照</param>
            /// <param name="e">イベントデータ</param>
            internal void OnHandleDestroyed(object sender, EventArgs e)
            {
                ReleaseHandle();
            }

            /// <summary>
            /// Windowsメッセージを処理します。
            /// </summary>
            /// <param name="m">処理するWindowsメッセージ</param>
            protected override void WndProc(ref Message m)
            {
                int hIMC;

                if (this.Enabled)
                {
                    switch (m.Msg)
                    {
                        case WM_CHAR:
                            // 文字が入力された場合
                            hIMC = ImmGetContext(this.Handle.ToInt32());
                            if (ImmGetOpenStatus(hIMC) == 0)
                            {
                                if (m.WParam.ToInt32() >= 32)
                                {
                                    IMECompositionEventArgs e = new IMECompositionEventArgs(Convert.ToString((char)(m.WParam.ToInt32() & 0xff)), true, 1);

                                    OnConverted(this, e);
                                }
                            }

                            ImmReleaseContext(this.Handle.ToInt32(), hIMC);

                            break;
                        case WM_IME_COMPOSITION:
                            // IMEから未確定文字列や確定された文字列がやってきたとき
                            if (((uint)m.LParam & (uint)GCS_RESULTREADSTR) != 0)
                            {
                                hIMC = ImmGetContext(this.Handle.ToInt32());

                                int size = ImmGetCompositionString(hIMC, GCS_RESULTREADSTR, null, 0);
                                if (size > 0)
                                {
                                    StringBuilder sb = new StringBuilder(size);
                                    ImmGetCompositionString(hIMC, GCS_RESULTREADSTR, sb, size);
                                    string kana = sb.ToString();
                                    if (kana.Length > size)
                                    {
                                        kana = kana.Substring(0, size);
                                    }

                                    IMECompositionEventArgs e = new IMECompositionEventArgs(kana, false, size);

                                    OnConverted(this, e);
                                }

                                ImmReleaseContext(this.Handle.ToInt32(), hIMC);
                            }

                            break;
                    }
                }

                base.WndProc(ref m);
            }
        }
        #endregion
    }
}
