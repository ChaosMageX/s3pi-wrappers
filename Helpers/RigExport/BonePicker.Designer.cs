namespace RigExport
{
    partial class BonePicker
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
            this.clbBones = new System.Windows.Forms.CheckedListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnSelNone = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cbHashBones = new System.Windows.Forms.CheckBox();
            this.lbBonesSelected = new System.Windows.Forms.Label();
            this.lbBonesSelectedCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // clbBones
            // 
            this.clbBones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbBones.CheckOnClick = true;
            this.clbBones.FormattingEnabled = true;
            this.clbBones.Location = new System.Drawing.Point(12, 12);
            this.clbBones.Name = "clbBones";
            this.clbBones.Size = new System.Drawing.Size(600, 364);
            this.clbBones.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(537, 416);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelAll.Location = new System.Drawing.Point(324, 416);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelAll.TabIndex = 2;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSelNone
            // 
            this.btnSelNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelNone.Location = new System.Drawing.Point(431, 416);
            this.btnSelNone.Name = "btnSelNone";
            this.btnSelNone.Size = new System.Drawing.Size(75, 23);
            this.btnSelNone.TabIndex = 3;
            this.btnSelNone.Text = "Select None";
            this.btnSelNone.UseVisualStyleBackColor = true;
            this.btnSelNone.Click += new System.EventHandler(this.btnSelNone_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.Title = "Save";
            // 
            // cbHashBones
            // 
            this.cbHashBones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbHashBones.AutoSize = true;
            this.cbHashBones.Location = new System.Drawing.Point(183, 420);
            this.cbHashBones.Name = "cbHashBones";
            this.cbHashBones.Size = new System.Drawing.Size(115, 17);
            this.cbHashBones.TabIndex = 4;
            this.cbHashBones.Text = "Hash Bone Names";
            this.cbHashBones.UseVisualStyleBackColor = true;
            // 
            // lbBonesSelected
            // 
            this.lbBonesSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBonesSelected.AutoSize = true;
            this.lbBonesSelected.Location = new System.Drawing.Point(16, 421);
            this.lbBonesSelected.Name = "lbBonesSelected";
            this.lbBonesSelected.Size = new System.Drawing.Size(85, 13);
            this.lbBonesSelected.TabIndex = 5;
            this.lbBonesSelected.Text = "Bones Selected:";
            // 
            // lbBonesSelectedCount
            // 
            this.lbBonesSelectedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBonesSelectedCount.AutoSize = true;
            this.lbBonesSelectedCount.Location = new System.Drawing.Point(127, 420);
            this.lbBonesSelectedCount.Name = "lbBonesSelectedCount";
            this.lbBonesSelectedCount.Size = new System.Drawing.Size(10, 13);
            this.lbBonesSelectedCount.TabIndex = 6;
            this.lbBonesSelectedCount.Text = ".";
            // 
            // BonePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.lbBonesSelectedCount);
            this.Controls.Add(this.lbBonesSelected);
            this.Controls.Add(this.cbHashBones);
            this.Controls.Add(this.btnSelNone);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.clbBones);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "BonePicker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Bones";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbBones;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnSelNone;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox cbHashBones;
        private System.Windows.Forms.Label lbBonesSelected;
        private System.Windows.Forms.Label lbBonesSelectedCount;
    }
}