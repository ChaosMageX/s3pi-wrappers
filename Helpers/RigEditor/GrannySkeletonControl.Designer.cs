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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbBones = new System.Windows.Forms.GroupBox();
            this.boneTreeView = new BoneTreeView();
            this.grannyBoneControl = new GrannyBoneControl();
            this.gbBones.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(10, 13);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(54, 10);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(340, 20);
            this.tbName.TabIndex = 1;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // gbBones
            // 
            this.gbBones.Controls.Add(this.boneTreeView);
            this.gbBones.Controls.Add(this.grannyBoneControl);
            this.gbBones.Location = new System.Drawing.Point(4, 38);
            this.gbBones.Name = "gbBones";
            this.gbBones.Size = new System.Drawing.Size(640, 334);
            this.gbBones.TabIndex = 2;
            this.gbBones.TabStop = false;
            this.gbBones.Text = "Bones";
            // 
            // boneTreeView
            // 
            this.boneTreeView.BoneManager = null;
            this.boneTreeView.HideSelection = false;
            this.boneTreeView.Location = new System.Drawing.Point(6, 19);
            this.boneTreeView.Name = "boneTreeView";
            this.boneTreeView.Size = new System.Drawing.Size(170, 310);
            this.boneTreeView.TabIndex = 1;
            this.boneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.boneTreeView_AfterSelect);
            // 
            // grannyBoneControl
            // 
            this.grannyBoneControl.Enabled = false;
            this.grannyBoneControl.Location = new System.Drawing.Point(182, 19);
            this.grannyBoneControl.Name = "grannyBoneControl";
            this.grannyBoneControl.Size = new System.Drawing.Size(455, 310);
            this.grannyBoneControl.TabIndex = 0;
            this.grannyBoneControl.Value = null;
            // 
            // GrannySkeletonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbBones);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Name = "GrannySkeletonControl";
            this.Size = new System.Drawing.Size(648, 378);
            this.gbBones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox gbBones;
        private BoneTreeView boneTreeView;
        private GrannyBoneControl grannyBoneControl;
    }
}
