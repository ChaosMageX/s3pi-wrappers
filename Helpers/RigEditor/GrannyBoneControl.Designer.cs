﻿namespace s3piwrappers.RigEditor
{
    partial class GrannyBoneControl
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
            this.tcLocalTransform = new s3piwrappers.RigEditor.GrannyTransformControl();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tcLocalTransform
            // 
            this.tcLocalTransform.Location = new System.Drawing.Point(0, 30);
            this.tcLocalTransform.Name = "tcLocalTransform";
            this.tcLocalTransform.Size = new System.Drawing.Size(451, 278);
            this.tcLocalTransform.TabIndex = 2;
            this.tcLocalTransform.Title = "Local Transform:";
            this.tcLocalTransform.Value = null;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(45, 5);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(398, 20);
            this.tbName.TabIndex = 1;
            this.tbName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(2, 8);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // GrannyBoneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcLocalTransform);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Name = "GrannyBoneControl";
            this.Size = new System.Drawing.Size(450, 303);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private GrannyTransformControl tcLocalTransform;
    }
}
