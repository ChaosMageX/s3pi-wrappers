namespace FNVUnhasher
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.setPrefixBtn = new System.Windows.Forms.Button();
            this.prefixInputTxt = new System.Windows.Forms.TextBox();
            this.prefixOutputTxt = new System.Windows.Forms.TextBox();
            this.setSuffixBtn = new System.Windows.Forms.Button();
            this.suffixInputTxt = new System.Windows.Forms.TextBox();
            this.suffixOutputTxt = new System.Windows.Forms.TextBox();
            this.searchTableInputTxt = new System.Windows.Forms.TextBox();
            this.removeCharsBtn = new System.Windows.Forms.Button();
            this.addCharsBtn = new System.Windows.Forms.Button();
            this.searchTableOutputTxt = new System.Windows.Forms.TextBox();
            this.defSearchTablesBtn = new System.Windows.Forms.Button();
            this.defSearchTablesCmb = new System.Windows.Forms.ComboBox();
            this.defSearchTablesTxt = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.filter32Lbl = new System.Windows.Forms.Label();
            this.filter32Txt = new System.Windows.Forms.TextBox();
            this.filter64Lbl = new System.Windows.Forms.Label();
            this.filter64Txt = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Controls.Add(this.searchTableInputTxt);
            this.groupBox1.Controls.Add(this.removeCharsBtn);
            this.groupBox1.Controls.Add(this.addCharsBtn);
            this.groupBox1.Controls.Add(this.searchTableOutputTxt);
            this.groupBox1.Controls.Add(this.defSearchTablesBtn);
            this.groupBox1.Controls.Add(this.defSearchTablesCmb);
            this.groupBox1.Controls.Add(this.defSearchTablesTxt);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 195);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Table";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(10, 100);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.setPrefixBtn);
            this.splitContainer1.Panel1.Controls.Add(this.prefixInputTxt);
            this.splitContainer1.Panel1.Controls.Add(this.prefixOutputTxt);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.setSuffixBtn);
            this.splitContainer1.Panel2.Controls.Add(this.suffixInputTxt);
            this.splitContainer1.Panel2.Controls.Add(this.suffixOutputTxt);
            this.splitContainer1.Size = new System.Drawing.Size(342, 85);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 3;
            // 
            // setPrefixBtn
            // 
            this.setPrefixBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setPrefixBtn.Location = new System.Drawing.Point(4, 57);
            this.setPrefixBtn.Name = "setPrefixBtn";
            this.setPrefixBtn.Size = new System.Drawing.Size(164, 23);
            this.setPrefixBtn.TabIndex = 2;
            this.setPrefixBtn.Text = "Set Prefix";
            this.setPrefixBtn.UseVisualStyleBackColor = true;
            this.setPrefixBtn.Click += new System.EventHandler(this.setPrefix_Click);
            // 
            // prefixInputTxt
            // 
            this.prefixInputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prefixInputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixInputTxt.Location = new System.Drawing.Point(4, 30);
            this.prefixInputTxt.Name = "prefixInputTxt";
            this.prefixInputTxt.Size = new System.Drawing.Size(164, 20);
            this.prefixInputTxt.TabIndex = 1;
            // 
            // prefixOutputTxt
            // 
            this.prefixOutputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prefixOutputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixOutputTxt.Location = new System.Drawing.Point(3, 3);
            this.prefixOutputTxt.Name = "prefixOutputTxt";
            this.prefixOutputTxt.ReadOnly = true;
            this.prefixOutputTxt.Size = new System.Drawing.Size(165, 20);
            this.prefixOutputTxt.TabIndex = 0;
            // 
            // setSuffixBtn
            // 
            this.setSuffixBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setSuffixBtn.Location = new System.Drawing.Point(4, 57);
            this.setSuffixBtn.Name = "setSuffixBtn";
            this.setSuffixBtn.Size = new System.Drawing.Size(160, 23);
            this.setSuffixBtn.TabIndex = 2;
            this.setSuffixBtn.Text = "Set Suffix";
            this.setSuffixBtn.UseVisualStyleBackColor = true;
            this.setSuffixBtn.Click += new System.EventHandler(this.setSuffix_Click);
            // 
            // suffixInputTxt
            // 
            this.suffixInputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suffixInputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suffixInputTxt.Location = new System.Drawing.Point(4, 30);
            this.suffixInputTxt.Name = "suffixInputTxt";
            this.suffixInputTxt.Size = new System.Drawing.Size(160, 20);
            this.suffixInputTxt.TabIndex = 1;
            // 
            // suffixOutputTxt
            // 
            this.suffixOutputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suffixOutputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suffixOutputTxt.Location = new System.Drawing.Point(4, 3);
            this.suffixOutputTxt.Name = "suffixOutputTxt";
            this.suffixOutputTxt.ReadOnly = true;
            this.suffixOutputTxt.Size = new System.Drawing.Size(160, 20);
            this.suffixOutputTxt.TabIndex = 0;
            // 
            // searchTableInputTxt
            // 
            this.searchTableInputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTableInputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTableInputTxt.Location = new System.Drawing.Point(81, 73);
            this.searchTableInputTxt.Name = "searchTableInputTxt";
            this.searchTableInputTxt.Size = new System.Drawing.Size(169, 20);
            this.searchTableInputTxt.TabIndex = 6;
            // 
            // removeCharsBtn
            // 
            this.removeCharsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeCharsBtn.Location = new System.Drawing.Point(256, 71);
            this.removeCharsBtn.Name = "removeCharsBtn";
            this.removeCharsBtn.Size = new System.Drawing.Size(96, 23);
            this.removeCharsBtn.TabIndex = 5;
            this.removeCharsBtn.Text = "Remove Chars";
            this.removeCharsBtn.UseVisualStyleBackColor = true;
            this.removeCharsBtn.Click += new System.EventHandler(this.removeChars_Click);
            // 
            // addCharsBtn
            // 
            this.addCharsBtn.Location = new System.Drawing.Point(10, 71);
            this.addCharsBtn.Name = "addCharsBtn";
            this.addCharsBtn.Size = new System.Drawing.Size(65, 23);
            this.addCharsBtn.TabIndex = 4;
            this.addCharsBtn.Text = "Add Chars";
            this.addCharsBtn.UseVisualStyleBackColor = true;
            this.addCharsBtn.Click += new System.EventHandler(this.addChars_Click);
            // 
            // searchTableOutputTxt
            // 
            this.searchTableOutputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTableOutputTxt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTableOutputTxt.Location = new System.Drawing.Point(10, 44);
            this.searchTableOutputTxt.Name = "searchTableOutputTxt";
            this.searchTableOutputTxt.ReadOnly = true;
            this.searchTableOutputTxt.Size = new System.Drawing.Size(343, 20);
            this.searchTableOutputTxt.TabIndex = 3;
            // 
            // defSearchTablesBtn
            // 
            this.defSearchTablesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defSearchTablesBtn.Location = new System.Drawing.Point(256, 15);
            this.defSearchTablesBtn.Name = "defSearchTablesBtn";
            this.defSearchTablesBtn.Size = new System.Drawing.Size(97, 23);
            this.defSearchTablesBtn.TabIndex = 2;
            this.defSearchTablesBtn.Text = "Set to Default";
            this.defSearchTablesBtn.UseVisualStyleBackColor = true;
            this.defSearchTablesBtn.Click += new System.EventHandler(this.defSearchTables_Click);
            // 
            // defSearchTablesCmb
            // 
            this.defSearchTablesCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.defSearchTablesCmb.FormattingEnabled = true;
            this.defSearchTablesCmb.Items.AddRange(new object[] {
            "All ASCII",
            "All Printable",
            "English Alphanumeric",
            "English Alphabet",
            "Numeric"});
            this.defSearchTablesCmb.Location = new System.Drawing.Point(129, 17);
            this.defSearchTablesCmb.Name = "defSearchTablesCmb";
            this.defSearchTablesCmb.Size = new System.Drawing.Size(121, 21);
            this.defSearchTablesCmb.TabIndex = 1;
            // 
            // defSearchTablesTxt
            // 
            this.defSearchTablesTxt.AutoSize = true;
            this.defSearchTablesTxt.Location = new System.Drawing.Point(7, 20);
            this.defSearchTablesTxt.Name = "defSearchTablesTxt";
            this.defSearchTablesTxt.Size = new System.Drawing.Size(116, 13);
            this.defSearchTablesTxt.TabIndex = 0;
            this.defSearchTablesTxt.Text = "Default Search Tables:";
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(216, 251);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(297, 251);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // filter32Lbl
            // 
            this.filter32Lbl.AutoSize = true;
            this.filter32Lbl.Location = new System.Drawing.Point(13, 215);
            this.filter32Lbl.Name = "filter32Lbl";
            this.filter32Lbl.Size = new System.Drawing.Size(61, 13);
            this.filter32Lbl.TabIndex = 3;
            this.filter32Lbl.Text = "32-bit Filter:";
            // 
            // filter32Txt
            // 
            this.filter32Txt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filter32Txt.Location = new System.Drawing.Point(80, 212);
            this.filter32Txt.Name = "filter32Txt";
            this.filter32Txt.Size = new System.Drawing.Size(80, 20);
            this.filter32Txt.TabIndex = 4;
            this.filter32Txt.Text = "0xFFFFFFFF";
            // 
            // filter64Lbl
            // 
            this.filter64Lbl.AutoSize = true;
            this.filter64Lbl.Location = new System.Drawing.Point(167, 215);
            this.filter64Lbl.Name = "filter64Lbl";
            this.filter64Lbl.Size = new System.Drawing.Size(61, 13);
            this.filter64Lbl.TabIndex = 5;
            this.filter64Lbl.Text = "64-bit Filter:";
            // 
            // filter64Txt
            // 
            this.filter64Txt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filter64Txt.Location = new System.Drawing.Point(234, 212);
            this.filter64Txt.Name = "filter64Txt";
            this.filter64Txt.Size = new System.Drawing.Size(135, 20);
            this.filter64Txt.TabIndex = 6;
            this.filter64Txt.Text = "0xFFFFFFFFFFFFFFFF";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 286);
            this.ControlBox = false;
            this.Controls.Add(this.filter64Txt);
            this.Controls.Add(this.filter64Lbl);
            this.Controls.Add(this.filter32Txt);
            this.Controls.Add(this.filter32Lbl);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 280);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox defSearchTablesCmb;
        private System.Windows.Forms.Label defSearchTablesTxt;
        private System.Windows.Forms.Button defSearchTablesBtn;
        private System.Windows.Forms.TextBox searchTableInputTxt;
        private System.Windows.Forms.Button removeCharsBtn;
        private System.Windows.Forms.Button addCharsBtn;
        private System.Windows.Forms.TextBox searchTableOutputTxt;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button setPrefixBtn;
        private System.Windows.Forms.TextBox prefixInputTxt;
        private System.Windows.Forms.TextBox prefixOutputTxt;
        private System.Windows.Forms.Button setSuffixBtn;
        private System.Windows.Forms.TextBox suffixInputTxt;
        private System.Windows.Forms.TextBox suffixOutputTxt;
        private System.Windows.Forms.Label filter32Lbl;
        private System.Windows.Forms.TextBox filter32Txt;
        private System.Windows.Forms.Label filter64Lbl;
        private System.Windows.Forms.TextBox filter64Txt;
    }
}