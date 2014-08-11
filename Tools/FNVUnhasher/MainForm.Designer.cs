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
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyResultWordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyResultTimesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyResultItersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.elapsedTimeTXT = new System.Windows.Forms.TextBox();
            this.iterationsTXT = new System.Windows.Forms.TextBox();
            this.unHashingProgress = new System.Windows.Forms.ProgressBar();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.resultsLST = new System.Windows.Forms.ListBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.algorithmLBL = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxMatchesNUM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCharsNUM)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(509, 24);
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
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settings_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyResultsToolStripMenuItem,
            this.copyResultWordsToolStripMenuItem,
            this.copyResultTimesToolStripMenuItem,
            this.copyResultItersToolStripMenuItem,
            this.removeResultsToolStripMenuItem,
            this.clearResultsToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectAllResultsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyResultsToolStripMenuItem
            // 
            this.copyResultsToolStripMenuItem.Name = "copyResultsToolStripMenuItem";
            this.copyResultsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyResultsToolStripMenuItem.Text = "Copy Results";
            this.copyResultsToolStripMenuItem.Click += new System.EventHandler(this.copyResults_Click);
            // 
            // copyResultWordsToolStripMenuItem
            // 
            this.copyResultWordsToolStripMenuItem.Name = "copyResultWordsToolStripMenuItem";
            this.copyResultWordsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyResultWordsToolStripMenuItem.Text = "Copy Result Words";
            this.copyResultWordsToolStripMenuItem.Click += new System.EventHandler(this.copyResultWords_Click);
            // 
            // copyResultTimesToolStripMenuItem
            // 
            this.copyResultTimesToolStripMenuItem.Name = "copyResultTimesToolStripMenuItem";
            this.copyResultTimesToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyResultTimesToolStripMenuItem.Text = "Copy Result Times";
            this.copyResultTimesToolStripMenuItem.Click += new System.EventHandler(this.copyResultTimes_Click);
            // 
            // copyResultItersToolStripMenuItem
            // 
            this.copyResultItersToolStripMenuItem.Name = "copyResultItersToolStripMenuItem";
            this.copyResultItersToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyResultItersToolStripMenuItem.Text = "Copy Result Iters";
            this.copyResultItersToolStripMenuItem.Click += new System.EventHandler(this.copyResultIters_Click);
            // 
            // removeResultsToolStripMenuItem
            // 
            this.removeResultsToolStripMenuItem.Name = "removeResultsToolStripMenuItem";
            this.removeResultsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.removeResultsToolStripMenuItem.Text = "Remove Results";
            this.removeResultsToolStripMenuItem.Click += new System.EventHandler(this.removeResults_Click);
            // 
            // clearResultsToolStripMenuItem
            // 
            this.clearResultsToolStripMenuItem.Name = "clearResultsToolStripMenuItem";
            this.clearResultsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.clearResultsToolStripMenuItem.Text = "Clear Results";
            this.clearResultsToolStripMenuItem.Click += new System.EventHandler(this.clearResults_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // selectAllResultsToolStripMenuItem
            // 
            this.selectAllResultsToolStripMenuItem.Name = "selectAllResultsToolStripMenuItem";
            this.selectAllResultsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.selectAllResultsToolStripMenuItem.Text = "Select All Results";
            this.selectAllResultsToolStripMenuItem.Click += new System.EventHandler(this.selectAllResults_Click);
            // 
            // hashBtn
            // 
            this.hashBtn.Location = new System.Drawing.Point(13, 28);
            this.hashBtn.Name = "hashBtn";
            this.hashBtn.Size = new System.Drawing.Size(40, 23);
            this.hashBtn.TabIndex = 1;
            this.hashBtn.Text = "Hash";
            this.hashBtn.UseVisualStyleBackColor = true;
            this.hashBtn.Click += new System.EventHandler(this.hash_Click);
            // 
            // unhashBtn
            // 
            this.unhashBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unhashBtn.Location = new System.Drawing.Point(442, 28);
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
            this.inputTxt.Size = new System.Drawing.Size(377, 20);
            this.inputTxt.TabIndex = 3;
            // 
            // stopUnhashBtn
            // 
            this.stopUnhashBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopUnhashBtn.Location = new System.Drawing.Point(287, 57);
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
            this.unhashCmb.Location = new System.Drawing.Point(392, 59);
            this.unhashCmb.Name = "unhashCmb";
            this.unhashCmb.Size = new System.Drawing.Size(105, 21);
            this.unhashCmb.TabIndex = 5;
            // 
            // maxMatchesNUM
            // 
            this.maxMatchesNUM.Location = new System.Drawing.Point(226, 60);
            this.maxMatchesNUM.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.maxMatchesNUM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxMatchesNUM.Name = "maxMatchesNUM";
            this.maxMatchesNUM.Size = new System.Drawing.Size(55, 20);
            this.maxMatchesNUM.TabIndex = 13;
            this.maxMatchesNUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.maxCharsNUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maxCharsNUM.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // matchCountTXT
            // 
            this.matchCountTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.matchCountTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.matchCountTXT.Location = new System.Drawing.Point(12, 308);
            this.matchCountTXT.Name = "matchCountTXT";
            this.matchCountTXT.ReadOnly = true;
            this.matchCountTXT.Size = new System.Drawing.Size(80, 13);
            this.matchCountTXT.TabIndex = 17;
            this.matchCountTXT.Text = "Matches: ";
            // 
            // elapsedTimeTXT
            // 
            this.elapsedTimeTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.elapsedTimeTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.elapsedTimeTXT.Location = new System.Drawing.Point(102, 308);
            this.elapsedTimeTXT.Name = "elapsedTimeTXT";
            this.elapsedTimeTXT.ReadOnly = true;
            this.elapsedTimeTXT.Size = new System.Drawing.Size(185, 13);
            this.elapsedTimeTXT.TabIndex = 19;
            this.elapsedTimeTXT.Text = "Elapsed Time: ";
            // 
            // iterationsTXT
            // 
            this.iterationsTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iterationsTXT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.iterationsTXT.Location = new System.Drawing.Point(295, 308);
            this.iterationsTXT.Name = "iterationsTXT";
            this.iterationsTXT.ReadOnly = true;
            this.iterationsTXT.Size = new System.Drawing.Size(200, 13);
            this.iterationsTXT.TabIndex = 15;
            this.iterationsTXT.Text = "Iterations: ";
            // 
            // unHashingProgress
            // 
            this.unHashingProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unHashingProgress.Location = new System.Drawing.Point(12, 327);
            this.unHashingProgress.Maximum = 1000000;
            this.unHashingProgress.Name = "unHashingProgress";
            this.unHashingProgress.Size = new System.Drawing.Size(485, 23);
            this.unHashingProgress.Step = 1000;
            this.unHashingProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.unHashingProgress.TabIndex = 14;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // resultsLST
            // 
            this.resultsLST.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsLST.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsLST.FormattingEnabled = true;
            this.resultsLST.ItemHeight = 14;
            this.resultsLST.Location = new System.Drawing.Point(12, 88);
            this.resultsLST.Name = "resultsLST";
            this.resultsLST.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.resultsLST.Size = new System.Drawing.Size(485, 214);
            this.resultsLST.TabIndex = 18;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipTitle = "FNV Unhasher";
            this.notifyIcon.Text = "FNV Unhasher";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // algorithmLBL
            // 
            this.algorithmLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.algorithmLBL.AutoSize = true;
            this.algorithmLBL.Location = new System.Drawing.Point(333, 62);
            this.algorithmLBL.Name = "algorithmLBL";
            this.algorithmLBL.Size = new System.Drawing.Size(53, 13);
            this.algorithmLBL.TabIndex = 20;
            this.algorithmLBL.Text = "Algorithm:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 362);
            this.Controls.Add(this.algorithmLBL);
            this.Controls.Add(this.elapsedTimeTXT);
            this.Controls.Add(this.resultsLST);
            this.Controls.Add(this.matchCountTXT);
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
            this.MinimumSize = new System.Drawing.Size(510, 400);
            this.Name = "MainForm";
            this.Text = "FNV Unhasher";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxMatchesNUM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCharsNUM)).EndInit();
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
        private System.Windows.Forms.TextBox elapsedTimeTXT;
        private System.Windows.Forms.TextBox iterationsTXT;
        private System.Windows.Forms.ProgressBar unHashingProgress;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ListBox resultsLST;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyResultWordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyResultTimesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyResultItersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectAllResultsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label algorithmLBL;
    }
}

