namespace FNVUnhasher
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hashBtn = new System.Windows.Forms.Button();
            this.unhashBtn = new System.Windows.Forms.Button();
            this.inputTxt = new System.Windows.Forms.TextBox();
            this.stopUnhashBtn = new System.Windows.Forms.Button();
            this.unhashCmb = new System.Windows.Forms.ComboBox();
            this.maxMatchesNUM = new System.Windows.Forms.NumericUpDown();
            this.maxMatchesLBL = new System.Windows.Forms.Label();
            this.maxCharsLBL = new System.Windows.Forms.Label();
            this.maxCharsNUM = new System.Windows.Forms.NumericUpDown();
            this.matchCountTXT = new System.Windows.Forms.TextBox();
            this.outputPanel = new System.Windows.Forms.Panel();
            this.endTimesTXT = new System.Windows.Forms.TextBox();
            this.resultsTXT = new System.Windows.Forms.TextBox();
            this.iterationsTXT = new System.Windows.Forms.TextBox();
            this.unHashingProgress = new System.Windows.Forms.ProgressBar();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxMatchesNUM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCharsNUM)).BeginInit();
            this.outputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(434, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settings_Click);
            // 
            // hashBtn
            // 
            this.hashBtn.Location = new System.Drawing.Point(13, 28);
            this.hashBtn.Name = "hashBtn";
            this.hashBtn.Size = new System.Drawing.Size(40, 23);
            this.hashBtn.TabIndex = 1;
            this.hashBtn.Text = "Hash";
            this.hashBtn.UseVisualStyleBackColor = true;
            // 
            // unhashBtn
            // 
            this.unhashBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unhashBtn.Location = new System.Drawing.Point(367, 28);
            this.unhashBtn.Name = "unhashBtn";
            this.unhashBtn.Size = new System.Drawing.Size(55, 23);
            this.unhashBtn.TabIndex = 2;
            this.unhashBtn.Text = "Unhash";
            this.unhashBtn.UseVisualStyleBackColor = true;
            this.unhashBtn.Click += new System.EventHandler(this.unhash_Click);
            // 
            // inputTxt
            // 
            this.inputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTxt.Location = new System.Drawing.Point(59, 30);
            this.inputTxt.Name = "inputTxt";
            this.inputTxt.Size = new System.Drawing.Size(302, 20);
            this.inputTxt.TabIndex = 3;
            // 
            // stopUnhashBtn
            // 
            this.stopUnhashBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopUnhashBtn.Location = new System.Drawing.Point(286, 57);
            this.stopUnhashBtn.Name = "stopUnhashBtn";
            this.stopUnhashBtn.Size = new System.Drawing.Size(40, 23);
            this.stopUnhashBtn.TabIndex = 4;
            this.stopUnhashBtn.Text = "Stop";
            this.stopUnhashBtn.UseVisualStyleBackColor = true;
            this.stopUnhashBtn.Click += new System.EventHandler(this.stopUnhash_Click);
            // 
            // unhashCmb
            // 
            this.unhashCmb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unhashCmb.FormattingEnabled = true;
            this.unhashCmb.Items.AddRange(new object[] {
            "32",
            "64"});
            this.unhashCmb.Location = new System.Drawing.Point(332, 59);
            this.unhashCmb.Name = "unhashCmb";
            this.unhashCmb.Size = new System.Drawing.Size(90, 21);
            this.unhashCmb.TabIndex = 5;
            // 
            // maxMatchesNUM
            // 
            this.maxMatchesNUM.Location = new System.Drawing.Point(226, 60);
            this.maxMatchesNUM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxMatchesNUM.Name = "maxMatchesNUM";
            this.maxMatchesNUM.Size = new System.Drawing.Size(47, 20);
            this.maxMatchesNUM.TabIndex = 13;
            this.maxMatchesNUM.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // maxMatchesLBL
            // 
            this.maxMatchesLBL.AutoSize = true;
            this.maxMatchesLBL.Location = new System.Drawing.Point(146, 62);
            this.maxMatchesLBL.Name = "maxMatchesLBL";
            this.maxMatchesLBL.Size = new System.Drawing.Size(74, 13);
            this.maxMatchesLBL.TabIndex = 12;
            this.maxMatchesLBL.Text = "Max Matches:";
            // 
            // maxCharsLBL
            // 
            this.maxCharsLBL.AutoSize = true;
            this.maxCharsLBL.Location = new System.Drawing.Point(12, 62);
            this.maxCharsLBL.Name = "maxCharsLBL";
            this.maxCharsLBL.Size = new System.Drawing.Size(84, 13);
            this.maxCharsLBL.TabIndex = 11;
            this.maxCharsLBL.Text = "Max Characters:";
            // 
            // maxCharsNUM
            // 
            this.maxCharsNUM.Location = new System.Drawing.Point(102, 60);
            this.maxCharsNUM.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.maxCharsNUM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxCharsNUM.Name = "maxCharsNUM";
            this.maxCharsNUM.Size = new System.Drawing.Size(38, 20);
            this.maxCharsNUM.TabIndex = 10;
            this.maxCharsNUM.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // matchCountTXT
            // 
            this.matchCountTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.matchCountTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.matchCountTXT.Location = new System.Drawing.Point(347, 308);
            this.matchCountTXT.Name = "matchCountTXT";
            this.matchCountTXT.ReadOnly = true;
            this.matchCountTXT.Size = new System.Drawing.Size(75, 13);
            this.matchCountTXT.TabIndex = 17;
            this.matchCountTXT.Text = "Matches:";
            // 
            // outputPanel
            // 
            this.outputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputPanel.Controls.Add(this.endTimesTXT);
            this.outputPanel.Controls.Add(this.resultsTXT);
            this.outputPanel.Location = new System.Drawing.Point(12, 88);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(410, 214);
            this.outputPanel.TabIndex = 16;
            // 
            // endTimesTXT
            // 
            this.endTimesTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.endTimesTXT.Dock = System.Windows.Forms.DockStyle.Right;
            this.endTimesTXT.Location = new System.Drawing.Point(308, 0);
            this.endTimesTXT.Multiline = true;
            this.endTimesTXT.Name = "endTimesTXT";
            this.endTimesTXT.ReadOnly = true;
            this.endTimesTXT.Size = new System.Drawing.Size(100, 212);
            this.endTimesTXT.TabIndex = 1;
            // 
            // resultsTXT
            // 
            this.resultsTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsTXT.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsTXT.Location = new System.Drawing.Point(0, 0);
            this.resultsTXT.Multiline = true;
            this.resultsTXT.Name = "resultsTXT";
            this.resultsTXT.ReadOnly = true;
            this.resultsTXT.Size = new System.Drawing.Size(302, 212);
            this.resultsTXT.TabIndex = 0;
            // 
            // iterationsTXT
            // 
            this.iterationsTXT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iterationsTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.iterationsTXT.Location = new System.Drawing.Point(12, 308);
            this.iterationsTXT.Name = "iterationsTXT";
            this.iterationsTXT.ReadOnly = true;
            this.iterationsTXT.Size = new System.Drawing.Size(329, 13);
            this.iterationsTXT.TabIndex = 15;
            this.iterationsTXT.Text = "Iterations:";
            // 
            // unHashingProgress
            // 
            this.unHashingProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unHashingProgress.Location = new System.Drawing.Point(12, 327);
            this.unHashingProgress.Maximum = 1000000;
            this.unHashingProgress.Name = "unHashingProgress";
            this.unHashingProgress.Size = new System.Drawing.Size(410, 23);
            this.unHashingProgress.Step = 1000;
            this.unHashingProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.unHashingProgress.TabIndex = 14;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 362);
            this.Controls.Add(this.matchCountTXT);
            this.Controls.Add(this.outputPanel);
            this.Controls.Add(this.iterationsTXT);
            this.Controls.Add(this.unHashingProgress);
            this.Controls.Add(this.maxMatchesNUM);
            this.Controls.Add(this.maxMatchesLBL);
            this.Controls.Add(this.maxCharsLBL);
            this.Controls.Add(this.maxCharsNUM);
            this.Controls.Add(this.unhashCmb);
            this.Controls.Add(this.stopUnhashBtn);
            this.Controls.Add(this.inputTxt);
            this.Controls.Add(this.unhashBtn);
            this.Controls.Add(this.hashBtn);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "MainForm";
            this.Text = "FNV Unhasher";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxMatchesNUM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCharsNUM)).EndInit();
            this.outputPanel.ResumeLayout(false);
            this.outputPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button hashBtn;
        private System.Windows.Forms.Button unhashBtn;
        private System.Windows.Forms.TextBox inputTxt;
        private System.Windows.Forms.Button stopUnhashBtn;
        private System.Windows.Forms.ComboBox unhashCmb;
        private System.Windows.Forms.NumericUpDown maxMatchesNUM;
        private System.Windows.Forms.Label maxMatchesLBL;
        private System.Windows.Forms.Label maxCharsLBL;
        private System.Windows.Forms.NumericUpDown maxCharsNUM;
        private System.Windows.Forms.TextBox matchCountTXT;
        private System.Windows.Forms.Panel outputPanel;
        private System.Windows.Forms.TextBox endTimesTXT;
        private System.Windows.Forms.TextBox resultsTXT;
        private System.Windows.Forms.TextBox iterationsTXT;
        private System.Windows.Forms.ProgressBar unHashingProgress;
        private System.Windows.Forms.Timer updateTimer;
    }
}

