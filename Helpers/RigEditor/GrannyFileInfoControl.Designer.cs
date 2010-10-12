namespace s3piwrappers.RigEditor
{
    partial class GrannyFileInfoControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grannyArtToolInfoControl1 = new s3piwrappers.RigEditor.GrannyArtToolInfoControl();
            this.grannyModelControl1 = new s3piwrappers.RigEditor.GrannyModelControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSkeleton = new System.Windows.Forms.TabPage();
            this.grannySkeletonControl1 = new s3piwrappers.RigEditor.GrannySkeletonControl();
            this.tpModel = new System.Windows.Forms.TabPage();
            this.tpArtToolInfo = new System.Windows.Forms.TabPage();
            this.lbFromFileName = new System.Windows.Forms.Label();
            this.tbFromFileName = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tpSkeleton.SuspendLayout();
            this.tpModel.SuspendLayout();
            this.tpArtToolInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // grannyArtToolInfoControl1
            // 
            this.grannyArtToolInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.grannyArtToolInfoControl1.Name = "grannyArtToolInfoControl1";
            this.grannyArtToolInfoControl1.Size = new System.Drawing.Size(388, 256);
            this.grannyArtToolInfoControl1.TabIndex = 0;
            this.grannyArtToolInfoControl1.Value = null;
            // 
            // grannyModelControl1
            // 
            this.grannyModelControl1.Location = new System.Drawing.Point(0, 0);
            this.grannyModelControl1.Name = "grannyModelControl1";
            this.grannyModelControl1.Size = new System.Drawing.Size(451, 307);
            this.grannyModelControl1.TabIndex = 0;
            this.grannyModelControl1.Value = null;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpSkeleton);
            this.tabControl1.Controls.Add(this.tpModel);
            this.tabControl1.Controls.Add(this.tpArtToolInfo);
            this.tabControl1.Location = new System.Drawing.Point(3, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(665, 430);
            this.tabControl1.TabIndex = 2;
            // 
            // tpSkeleton
            // 
            this.tpSkeleton.Controls.Add(this.grannySkeletonControl1);
            this.tpSkeleton.Location = new System.Drawing.Point(4, 22);
            this.tpSkeleton.Name = "tpSkeleton";
            this.tpSkeleton.Size = new System.Drawing.Size(657, 404);
            this.tpSkeleton.TabIndex = 2;
            this.tpSkeleton.Text = "Skeleton";
            this.tpSkeleton.UseVisualStyleBackColor = true;
            // 
            // grannySkeletonControl1
            // 
            this.grannySkeletonControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grannySkeletonControl1.Location = new System.Drawing.Point(0, 0);
            this.grannySkeletonControl1.Name = "grannySkeletonControl1";
            this.grannySkeletonControl1.Size = new System.Drawing.Size(657, 404);
            this.grannySkeletonControl1.TabIndex = 0;
            this.grannySkeletonControl1.Value = null;
            // 
            // tpModel
            // 
            this.tpModel.Controls.Add(this.grannyModelControl1);
            this.tpModel.Location = new System.Drawing.Point(4, 22);
            this.tpModel.Name = "tpModel";
            this.tpModel.Padding = new System.Windows.Forms.Padding(3);
            this.tpModel.Size = new System.Drawing.Size(657, 404);
            this.tpModel.TabIndex = 1;
            this.tpModel.Text = "Model";
            this.tpModel.UseVisualStyleBackColor = true;
            // 
            // tpArtToolInfo
            // 
            this.tpArtToolInfo.Controls.Add(this.grannyArtToolInfoControl1);
            this.tpArtToolInfo.Location = new System.Drawing.Point(4, 22);
            this.tpArtToolInfo.Name = "tpArtToolInfo";
            this.tpArtToolInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpArtToolInfo.Size = new System.Drawing.Size(657, 404);
            this.tpArtToolInfo.TabIndex = 0;
            this.tpArtToolInfo.Text = "ArtToolInfo";
            this.tpArtToolInfo.UseVisualStyleBackColor = true;
            // 
            // lbFromFileName
            // 
            this.lbFromFileName.AutoSize = true;
            this.lbFromFileName.Location = new System.Drawing.Point(4, 7);
            this.lbFromFileName.Name = "lbFromFileName";
            this.lbFromFileName.Size = new System.Drawing.Size(77, 13);
            this.lbFromFileName.TabIndex = 0;
            this.lbFromFileName.Text = "FromFileName:";
            // 
            // tbFromFileName
            // 
            this.tbFromFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFromFileName.Location = new System.Drawing.Point(85, 3);
            this.tbFromFileName.Name = "tbFromFileName";
            this.tbFromFileName.Size = new System.Drawing.Size(570, 20);
            this.tbFromFileName.TabIndex = 1;
            this.tbFromFileName.TextChanged += new System.EventHandler(this.tbFromFileName_TextChanged);
            // 
            // GrannyFileInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbFromFileName);
            this.Controls.Add(this.lbFromFileName);
            this.Controls.Add(this.tabControl1);
            this.Name = "GrannyFileInfoControl";
            this.Size = new System.Drawing.Size(670, 461);
            this.tabControl1.ResumeLayout(false);
            this.tpSkeleton.ResumeLayout(false);
            this.tpModel.ResumeLayout(false);
            this.tpArtToolInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrannyArtToolInfoControl grannyArtToolInfoControl1;
        private GrannyModelControl grannyModelControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSkeleton;
        private GrannySkeletonControl grannySkeletonControl1;
        private System.Windows.Forms.TabPage tpModel;
        private System.Windows.Forms.TabPage tpArtToolInfo;
        private System.Windows.Forms.Label lbFromFileName;
        private System.Windows.Forms.TextBox tbFromFileName;

    }
}
