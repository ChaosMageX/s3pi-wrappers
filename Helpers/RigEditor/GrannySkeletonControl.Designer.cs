namespace s3piwrappers.RigEditor
{
    partial class GrannySkeletonControl
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

        
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbBones = new System.Windows.Forms.GroupBox();
            this.llbMatrixInfo = new System.Windows.Forms.LinkLabel();
            this.boneTreeView = new s3piwrappers.RigEditor.BoneTreeView();
            this.grannyBoneControl = new s3piwrappers.RigEditor.GrannyBoneControl();
            this.gbBones.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(9, 12);
            this.lbName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(49, 17);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(58, 9);
            this.tbName.Margin = new System.Windows.Forms.Padding(4);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(293, 22);
            this.tbName.TabIndex = 1;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // gbBones
            // 
            this.gbBones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBones.Controls.Add(this.boneTreeView);
            this.gbBones.Controls.Add(this.grannyBoneControl);
            this.gbBones.Location = new System.Drawing.Point(4, 37);
            this.gbBones.Margin = new System.Windows.Forms.Padding(4);
            this.gbBones.Name = "gbBones";
            this.gbBones.Padding = new System.Windows.Forms.Padding(4);
            this.gbBones.Size = new System.Drawing.Size(863, 452);
            this.gbBones.TabIndex = 2;
            this.gbBones.TabStop = false;
            this.gbBones.Text = "Bones";
            // 
            // llbMatrixInfo
            // 
            this.llbMatrixInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llbMatrixInfo.AutoSize = true;
            this.llbMatrixInfo.Location = new System.Drawing.Point(768, 12);
            this.llbMatrixInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llbMatrixInfo.Name = "llbMatrixInfo";
            this.llbMatrixInfo.Size = new System.Drawing.Size(99, 17);
            this.llbMatrixInfo.TabIndex = 4;
            this.llbMatrixInfo.TabStop = true;
            this.llbMatrixInfo.Text = "Get Matrix Info";
            this.llbMatrixInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbMatrixInfo_LinkClicked);
            // 
            // boneTreeView
            // 
            this.boneTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.boneTreeView.BoneManager = null;
            this.boneTreeView.HideSelection = false;
            this.boneTreeView.Location = new System.Drawing.Point(8, 22);
            this.boneTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.boneTreeView.Name = "boneTreeView";
            this.boneTreeView.Size = new System.Drawing.Size(339, 421);
            this.boneTreeView.TabIndex = 1;
            this.boneTreeView.TabStop = false;
            this.boneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.boneTreeView_AfterSelect);
            // 
            // grannyBoneControl
            // 
            this.grannyBoneControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grannyBoneControl.Enabled = false;
            this.grannyBoneControl.Location = new System.Drawing.Point(356, 22);
            this.grannyBoneControl.Margin = new System.Windows.Forms.Padding(5);
            this.grannyBoneControl.Name = "grannyBoneControl";
            this.grannyBoneControl.Size = new System.Drawing.Size(499, 233);
            this.grannyBoneControl.TabIndex = 2;
            this.grannyBoneControl.Value = null;
            this.grannyBoneControl.Visible = false;
            // 
            // GrannySkeletonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.llbMatrixInfo);
            this.Controls.Add(this.gbBones);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GrannySkeletonControl";
            this.Size = new System.Drawing.Size(873, 492);
            this.gbBones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox gbBones;
        private BoneTreeView boneTreeView;
        private GrannyBoneControl grannyBoneControl;
        private System.Windows.Forms.LinkLabel llbMatrixInfo;
    }
}
