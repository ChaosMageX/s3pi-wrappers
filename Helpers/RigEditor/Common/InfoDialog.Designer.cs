namespace s3piwrappers.RigEditor.Common
{
    partial class InfoDialog
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
            this.btnOk = new System.Windows.Forms.Button();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lbInfo = new System.Windows.Forms.TextBox();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(438, 449);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Close";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panelInfo
            // 
            this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInfo.Controls.Add(this.lbInfo);
            this.panelInfo.Location = new System.Drawing.Point(5, 8);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(508, 429);
            this.panelInfo.TabIndex = 1;
            // 
            // lbInfo
            // 
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInfo.Location = new System.Drawing.Point(0, 0);
            this.lbInfo.Multiline = true;
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.ReadOnly = true;
            this.lbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lbInfo.Size = new System.Drawing.Size(508, 429);
            this.lbInfo.TabIndex = 0;
            // 
            // InfoDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 484);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.btnOk);
            this.Name = "InfoDialog";
            this.Text = "InfoDialog";
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.TextBox lbInfo;
    }
}