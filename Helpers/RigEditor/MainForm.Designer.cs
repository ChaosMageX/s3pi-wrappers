﻿namespace s3piwrappers.RigEditor
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
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grannyFileInfoControl1 = new s3piwrappers.RigEditor.GrannyFileInfoControl();
            this.SuspendLayout();
            // 
            // btnCommit
            // 
            this.btnCommit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCommit.Location = new System.Drawing.Point(587, 463);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(75, 23);
            this.btnCommit.TabIndex = 1;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(506, 463);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grannyFileInfoControl1
            // 
            this.grannyFileInfoControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grannyFileInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.grannyFileInfoControl1.Name = "grannyFileInfoControl1";
            this.grannyFileInfoControl1.Size = new System.Drawing.Size(671, 461);
            this.grannyFileInfoControl1.TabIndex = 0;
            this.grannyFileInfoControl1.Value = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 489);
            this.Controls.Add(this.grannyFileInfoControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCommit);
            this.MaximumSize = new System.Drawing.Size(680, 530);
            this.MinimumSize = new System.Drawing.Size(680, 525);
            this.Name = "MainForm";
            this.Text = "Rig Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnCancel;
        private GrannyFileInfoControl grannyFileInfoControl1;

    }
}