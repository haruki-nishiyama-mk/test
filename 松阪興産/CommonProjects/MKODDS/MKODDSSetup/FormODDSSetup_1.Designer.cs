namespace MKODDS.Setup
{
    partial class FormODDSSetup_1
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
            this.cbtnESC = new MKClass.MKControl.CButton();
            this.gbHB = new System.Windows.Forms.GroupBox();
            this.cbtnHBSetERPMenu = new MKClass.MKControl.CButton();
            this.cbtnHBSetERPScript = new MKClass.MKControl.CButton();
            this.cbtnHBSetTables = new MKClass.MKControl.CButton();
            this.gbHB.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbtnESC
            // 
            this.cbtnESC.Location = new System.Drawing.Point(12, 12);
            this.cbtnESC.Name = "cbtnESC";
            this.cbtnESC.ShortcutKey = System.Windows.Forms.Keys.Escape;
            this.cbtnESC.Size = new System.Drawing.Size(105, 45);
            this.cbtnESC.TabIndex = 1000;
            this.cbtnESC.Text = "ESC:閉じる";
            this.cbtnESC.UseVisualStyleBackColor = true;
            // 
            // gbHB
            // 
            this.gbHB.Controls.Add(this.cbtnHBSetERPMenu);
            this.gbHB.Controls.Add(this.cbtnHBSetERPScript);
            this.gbHB.Controls.Add(this.cbtnHBSetTables);
            this.gbHB.Location = new System.Drawing.Point(12, 66);
            this.gbHB.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.gbHB.Name = "gbHB";
            this.gbHB.Size = new System.Drawing.Size(230, 219);
            this.gbHB.TabIndex = 0;
            this.gbHB.TabStop = false;
            this.gbHB.Text = "販売大臣";
            // 
            // cbtnHBSetERPMenu
            // 
            this.cbtnHBSetERPMenu.Font = new System.Drawing.Font("メイリオ", 10F);
            this.cbtnHBSetERPMenu.Location = new System.Drawing.Point(12, 160);
            this.cbtnHBSetERPMenu.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.cbtnHBSetERPMenu.Name = "cbtnHBSetERPMenu";
            this.cbtnHBSetERPMenu.ShortcutKey = System.Windows.Forms.Keys.F1;
            this.cbtnHBSetERPMenu.Size = new System.Drawing.Size(200, 45);
            this.cbtnHBSetERPMenu.TabIndex = 3;
            this.cbtnHBSetERPMenu.Text = "ERPメニューの設定";
            this.cbtnHBSetERPMenu.UseVisualStyleBackColor = true;
            // 
            // cbtnHBSetERPScript
            // 
            this.cbtnHBSetERPScript.Font = new System.Drawing.Font("メイリオ", 10F);
            this.cbtnHBSetERPScript.Location = new System.Drawing.Point(12, 97);
            this.cbtnHBSetERPScript.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.cbtnHBSetERPScript.Name = "cbtnHBSetERPScript";
            this.cbtnHBSetERPScript.ShortcutKey = System.Windows.Forms.Keys.F1;
            this.cbtnHBSetERPScript.Size = new System.Drawing.Size(200, 45);
            this.cbtnHBSetERPScript.TabIndex = 2;
            this.cbtnHBSetERPScript.Text = "ERP拡張スクリプトの設定";
            this.cbtnHBSetERPScript.UseVisualStyleBackColor = true;
            // 
            // cbtnHBSetTables
            // 
            this.cbtnHBSetTables.Font = new System.Drawing.Font("メイリオ", 10F);
            this.cbtnHBSetTables.Location = new System.Drawing.Point(12, 34);
            this.cbtnHBSetTables.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.cbtnHBSetTables.Name = "cbtnHBSetTables";
            this.cbtnHBSetTables.ShortcutKey = System.Windows.Forms.Keys.F1;
            this.cbtnHBSetTables.Size = new System.Drawing.Size(200, 45);
            this.cbtnHBSetTables.TabIndex = 1;
            this.cbtnHBSetTables.Text = "テーブル設定";
            this.cbtnHBSetTables.UseVisualStyleBackColor = true;
            // 
            // FormODDSSetup_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 300);
            this.Controls.Add(this.gbHB);
            this.Controls.Add(this.cbtnESC);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormODDSSetup_1";
            this.Text = "ODDSセットアップ";
            this.Load += new System.EventHandler(this.FormSetup_1_Load);
            this.Shown += new System.EventHandler(this.FormODDSSetup_1_Shown);
            this.gbHB.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public MKClass.MKControl.CButton cbtnESC;
        private System.Windows.Forms.GroupBox gbHB;
        public MKClass.MKControl.CButton cbtnHBSetTables;
        public MKClass.MKControl.CButton cbtnHBSetERPScript;
        public MKClass.MKControl.CButton cbtnHBSetERPMenu;
    }
}

