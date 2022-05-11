namespace ASViews
{
    partial class FormASViews_1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPgid = new System.Windows.Forms.Label();
            this.lblLibNo = new System.Windows.Forms.Label();
            this.ctbLibNo = new MKClass.MKControl.CTextBox();
            this.lblQuery = new System.Windows.Forms.Label();
            this.ctbQuery = new MKClass.MKControl.CTextBox();
            this.cdgvResult = new MKClass.MKControl.CDataGridView();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.cbtnF1 = new MKClass.MKControl.CButton();
            this.cbtnF11 = new MKClass.MKControl.CButton();
            this.cbtnF12 = new MKClass.MKControl.CButton();
            ((System.ComponentModel.ISupportInitialize)(this.cdgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("メイリオ", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(1150, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ASデータ参照";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPgid
            // 
            this.lblPgid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPgid.Font = new System.Drawing.Font("メイリオ", 11F);
            this.lblPgid.Location = new System.Drawing.Point(1001, 9);
            this.lblPgid.Name = "lblPgid";
            this.lblPgid.Size = new System.Drawing.Size(301, 45);
            this.lblPgid.TabIndex = 1;
            this.lblPgid.Text = "ASViews";
            this.lblPgid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLibNo
            // 
            this.lblLibNo.AutoSize = true;
            this.lblLibNo.Location = new System.Drawing.Point(18, 69);
            this.lblLibNo.Margin = new System.Windows.Forms.Padding(9, 0, 3, 0);
            this.lblLibNo.Name = "lblLibNo";
            this.lblLibNo.Size = new System.Drawing.Size(64, 23);
            this.lblLibNo.TabIndex = 10;
            this.lblLibNo.Text = "LIB番号";
            this.lblLibNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ctbLibNo
            // 
            this.ctbLibNo.BackColor = System.Drawing.SystemColors.Window;
            this.ctbLibNo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ctbLibNo.InputMode = MKClass.MKControl.TextBoxInputMode.UInt;
            this.ctbLibNo.Location = new System.Drawing.Point(88, 66);
            this.ctbLibNo.Margin = new System.Windows.Forms.Padding(3, 12, 3, 6);
            this.ctbLibNo.MaxLength = 1;
            this.ctbLibNo.Name = "ctbLibNo";
            this.ctbLibNo.Size = new System.Drawing.Size(25, 29);
            this.ctbLibNo.TabIndex = 11;
            this.ctbLibNo.Text = "9";
            this.ctbLibNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblQuery
            // 
            this.lblQuery.AutoSize = true;
            this.lblQuery.Location = new System.Drawing.Point(18, 104);
            this.lblQuery.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lblQuery.Name = "lblQuery";
            this.lblQuery.Size = new System.Drawing.Size(85, 23);
            this.lblQuery.TabIndex = 12;
            this.lblQuery.Text = "実行クエリ";
            this.lblQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ctbQuery
            // 
            this.ctbQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctbQuery.BackColor = System.Drawing.SystemColors.Window;
            this.ctbQuery.Font = new System.Drawing.Font("メイリオ", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ctbQuery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ctbQuery.IsSelectAll = false;
            this.ctbQuery.Location = new System.Drawing.Point(12, 133);
            this.ctbQuery.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.ctbQuery.Multiline = true;
            this.ctbQuery.Name = "ctbQuery";
            this.ctbQuery.Size = new System.Drawing.Size(1142, 131);
            this.ctbQuery.TabIndex = 13;
            this.ctbQuery.Text = "N\r\nN\r\nN\r\nN\r\nN";
            // 
            // cdgvResult
            // 
            this.cdgvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("メイリオ", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cdgvResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.cdgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cdgvResult.Location = new System.Drawing.Point(12, 276);
            this.cdgvResult.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cdgvResult.Name = "cdgvResult";
            this.cdgvResult.RowTemplate.Height = 21;
            this.cdgvResult.Size = new System.Drawing.Size(1290, 420);
            this.cdgvResult.TabIndex = 100;
            this.cdgvResult.TabStop = false;
            // 
            // lblResultCount
            // 
            this.lblResultCount.Location = new System.Drawing.Point(1163, 241);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(139, 23);
            this.lblResultCount.TabIndex = 101;
            this.lblResultCount.Text = "9,999件";
            this.lblResultCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbtnF1
            // 
            this.cbtnF1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbtnF1.Font = new System.Drawing.Font("メイリオ", 9.5F);
            this.cbtnF1.Location = new System.Drawing.Point(12, 708);
            this.cbtnF1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 9);
            this.cbtnF1.Name = "cbtnF1";
            this.cbtnF1.ShortcutKey = System.Windows.Forms.Keys.F1;
            this.cbtnF1.Size = new System.Drawing.Size(100, 35);
            this.cbtnF1.TabIndex = 1000;
            this.cbtnF1.Text = "F1:実行";
            this.cbtnF1.UseVisualStyleBackColor = true;
            // 
            // cbtnF11
            // 
            this.cbtnF11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbtnF11.Font = new System.Drawing.Font("メイリオ", 9.5F);
            this.cbtnF11.Location = new System.Drawing.Point(1054, 708);
            this.cbtnF11.Margin = new System.Windows.Forms.Padding(3, 6, 24, 9);
            this.cbtnF11.Name = "cbtnF11";
            this.cbtnF11.ShortcutKey = System.Windows.Forms.Keys.F11;
            this.cbtnF11.Size = new System.Drawing.Size(100, 35);
            this.cbtnF11.TabIndex = 1010;
            this.cbtnF11.Text = "F11:CSV";
            this.cbtnF11.UseVisualStyleBackColor = true;
            // 
            // cbtnF12
            // 
            this.cbtnF12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbtnF12.Font = new System.Drawing.Font("メイリオ", 9.5F);
            this.cbtnF12.Location = new System.Drawing.Point(1202, 708);
            this.cbtnF12.Margin = new System.Windows.Forms.Padding(24, 6, 3, 9);
            this.cbtnF12.Name = "cbtnF12";
            this.cbtnF12.ShortcutKey = System.Windows.Forms.Keys.F12;
            this.cbtnF12.Size = new System.Drawing.Size(100, 35);
            this.cbtnF12.TabIndex = 1011;
            this.cbtnF12.Text = "F12:閉じる";
            this.cbtnF12.UseVisualStyleBackColor = true;
            // 
            // FormASViews_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1314, 761);
            this.Controls.Add(this.cbtnF12);
            this.Controls.Add(this.cbtnF11);
            this.Controls.Add(this.cbtnF1);
            this.Controls.Add(this.lblResultCount);
            this.Controls.Add(this.cdgvResult);
            this.Controls.Add(this.ctbQuery);
            this.Controls.Add(this.lblQuery);
            this.Controls.Add(this.ctbLibNo);
            this.Controls.Add(this.lblLibNo);
            this.Controls.Add(this.lblPgid);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("メイリオ", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.Name = "FormASViews_1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASデータ参照";
            this.Load += new System.EventHandler(this.ASViews_1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cdgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblPgid;
        private System.Windows.Forms.Label lblLibNo;
        private MKClass.MKControl.CTextBox ctbLibNo;
        private System.Windows.Forms.Label lblQuery;
        private MKClass.MKControl.CTextBox ctbQuery;
        private MKClass.MKControl.CDataGridView cdgvResult;
        private System.Windows.Forms.Label lblResultCount;
        private MKClass.MKControl.CButton cbtnF1;
        private MKClass.MKControl.CButton cbtnF11;
        private MKClass.MKControl.CButton cbtnF12;
    }
}
