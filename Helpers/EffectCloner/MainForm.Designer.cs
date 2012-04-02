namespace s3piwrappers.EffectCloner
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.printInputBTN = new System.Windows.Forms.Button();
            this.cloneEffectBTN = new System.Windows.Forms.Button();
            this.openInputBTN = new System.Windows.Forms.Button();
            this.inputEffectLST = new System.Windows.Forms.ListBox();
            this.printOutputBTN = new System.Windows.Forms.Button();
            this.saveOutputBTN = new System.Windows.Forms.Button();
            this.removeEffectBTN = new System.Windows.Forms.Button();
            this.outputEffectLST = new System.Windows.Forms.ListBox();
            this.outputCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setEffectNameTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.inputCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showItemBStringsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.outputCMS.SuspendLayout();
            this.inputCMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.printInputBTN);
            this.splitContainer1.Panel1.Controls.Add(this.cloneEffectBTN);
            this.splitContainer1.Panel1.Controls.Add(this.openInputBTN);
            this.splitContainer1.Panel1.Controls.Add(this.inputEffectLST);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.printOutputBTN);
            this.splitContainer1.Panel2.Controls.Add(this.saveOutputBTN);
            this.splitContainer1.Panel2.Controls.Add(this.removeEffectBTN);
            this.splitContainer1.Panel2.Controls.Add(this.outputEffectLST);
            this.splitContainer1.Size = new System.Drawing.Size(684, 262);
            this.splitContainer1.SplitterDistance = 342;
            this.splitContainer1.TabIndex = 0;
            // 
            // printInputBTN
            // 
            this.printInputBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printInputBTN.Enabled = false;
            this.printInputBTN.Location = new System.Drawing.Point(147, 235);
            this.printInputBTN.Name = "printInputBTN";
            this.printInputBTN.Size = new System.Drawing.Size(40, 23);
            this.printInputBTN.TabIndex = 3;
            this.printInputBTN.Text = "Print";
            this.printInputBTN.UseVisualStyleBackColor = true;
            this.printInputBTN.Click += new System.EventHandler(this.printInput_Click);
            // 
            // cloneEffectBTN
            // 
            this.cloneEffectBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cloneEffectBTN.Location = new System.Drawing.Point(193, 235);
            this.cloneEffectBTN.Name = "cloneEffectBTN";
            this.cloneEffectBTN.Size = new System.Drawing.Size(134, 23);
            this.cloneEffectBTN.TabIndex = 2;
            this.cloneEffectBTN.Text = "Clone Selected Effect";
            this.cloneEffectBTN.UseVisualStyleBackColor = true;
            this.cloneEffectBTN.Click += new System.EventHandler(this.cloneEffect_Click);
            // 
            // openInputBTN
            // 
            this.openInputBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openInputBTN.Location = new System.Drawing.Point(13, 235);
            this.openInputBTN.Name = "openInputBTN";
            this.openInputBTN.Size = new System.Drawing.Size(128, 23);
            this.openInputBTN.TabIndex = 1;
            this.openInputBTN.Text = "Open Effect File";
            this.openInputBTN.UseVisualStyleBackColor = true;
            this.openInputBTN.Click += new System.EventHandler(this.openInput_Click);
            // 
            // inputEffectLST
            // 
            this.inputEffectLST.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputEffectLST.ContextMenuStrip = this.inputCMS;
            this.inputEffectLST.FormattingEnabled = true;
            this.inputEffectLST.Location = new System.Drawing.Point(13, 13);
            this.inputEffectLST.Name = "inputEffectLST";
            this.inputEffectLST.Size = new System.Drawing.Size(314, 212);
            this.inputEffectLST.TabIndex = 0;
            // 
            // printOutputBTN
            // 
            this.printOutputBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printOutputBTN.Enabled = false;
            this.printOutputBTN.Location = new System.Drawing.Point(153, 235);
            this.printOutputBTN.Name = "printOutputBTN";
            this.printOutputBTN.Size = new System.Drawing.Size(39, 23);
            this.printOutputBTN.TabIndex = 3;
            this.printOutputBTN.Text = "Print";
            this.printOutputBTN.UseVisualStyleBackColor = true;
            this.printOutputBTN.Click += new System.EventHandler(this.printOutput_Click);
            // 
            // saveOutputBTN
            // 
            this.saveOutputBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveOutputBTN.Enabled = false;
            this.saveOutputBTN.Location = new System.Drawing.Point(198, 235);
            this.saveOutputBTN.Name = "saveOutputBTN";
            this.saveOutputBTN.Size = new System.Drawing.Size(128, 23);
            this.saveOutputBTN.TabIndex = 2;
            this.saveOutputBTN.Text = "Save Effect File";
            this.saveOutputBTN.UseVisualStyleBackColor = true;
            this.saveOutputBTN.Click += new System.EventHandler(this.saveOutput_Click);
            // 
            // removeEffectBTN
            // 
            this.removeEffectBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeEffectBTN.Location = new System.Drawing.Point(13, 235);
            this.removeEffectBTN.Name = "removeEffectBTN";
            this.removeEffectBTN.Size = new System.Drawing.Size(134, 23);
            this.removeEffectBTN.TabIndex = 1;
            this.removeEffectBTN.Text = "Remove Selected Effect";
            this.removeEffectBTN.UseVisualStyleBackColor = true;
            this.removeEffectBTN.Click += new System.EventHandler(this.removeEffect_Click);
            // 
            // outputEffectLST
            // 
            this.outputEffectLST.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputEffectLST.ContextMenuStrip = this.outputCMS;
            this.outputEffectLST.FormattingEnabled = true;
            this.outputEffectLST.Location = new System.Drawing.Point(13, 13);
            this.outputEffectLST.Name = "outputEffectLST";
            this.outputEffectLST.Size = new System.Drawing.Size(313, 212);
            this.outputEffectLST.TabIndex = 0;
            // 
            // outputCMS
            // 
            this.outputCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setEffectNameTSMI});
            this.outputCMS.Name = "effectHandleCMS";
            this.outputCMS.Size = new System.Drawing.Size(159, 26);
            // 
            // setEffectNameTSMI
            // 
            this.setEffectNameTSMI.Name = "setEffectNameTSMI";
            this.setEffectNameTSMI.Size = new System.Drawing.Size(158, 22);
            this.setEffectNameTSMI.Text = "Set Effect Name";
            this.setEffectNameTSMI.Click += new System.EventHandler(this.setEffectNameTSMI_Click);
            // 
            // inputCMS
            // 
            this.inputCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showItemBStringsTSMI});
            this.inputCMS.Name = "inputCMS";
            this.inputCMS.Size = new System.Drawing.Size(177, 26);
            // 
            // showItemBStringsTSMI
            // 
            this.showItemBStringsTSMI.Name = "showItemBStringsTSMI";
            this.showItemBStringsTSMI.Size = new System.Drawing.Size(176, 22);
            this.showItemBStringsTSMI.Text = "Show ItemB Strings";
            this.showItemBStringsTSMI.Click += new System.EventHandler(this.showItemBStringsTSMI_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 262);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Effect Cloner";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.outputCMS.ResumeLayout(false);
            this.inputCMS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button cloneEffectBTN;
        private System.Windows.Forms.Button openInputBTN;
        private System.Windows.Forms.ListBox inputEffectLST;
        private System.Windows.Forms.Button saveOutputBTN;
        private System.Windows.Forms.Button removeEffectBTN;
        private System.Windows.Forms.ListBox outputEffectLST;
        private System.Windows.Forms.Button printInputBTN;
        private System.Windows.Forms.ContextMenuStrip outputCMS;
        private System.Windows.Forms.ToolStripMenuItem setEffectNameTSMI;
        private System.Windows.Forms.Button printOutputBTN;
        private System.Windows.Forms.ContextMenuStrip inputCMS;
        private System.Windows.Forms.ToolStripMenuItem showItemBStringsTSMI;

    }
}

