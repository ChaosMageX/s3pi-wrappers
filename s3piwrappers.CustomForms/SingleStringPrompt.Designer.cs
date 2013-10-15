namespace s3piwrappers.CustomForms
{
    partial class SingleStringPrompt
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
            this.cancelBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.promptLBL = new System.Windows.Forms.Label();
            this.responseTXT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBTN.Location = new System.Drawing.Point(307, 66);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(75, 23);
            this.cancelBTN.TabIndex = 0;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            // 
            // okBTN
            // 
            this.okBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBTN.Enabled = false;
            this.okBTN.Location = new System.Drawing.Point(227, 66);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(75, 23);
            this.okBTN.TabIndex = 1;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            // 
            // promptLBL
            // 
            this.promptLBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptLBL.Location = new System.Drawing.Point(13, 9);
            this.promptLBL.Name = "promptLBL";
            this.promptLBL.Size = new System.Drawing.Size(370, 20);
            this.promptLBL.TabIndex = 2;
            this.promptLBL.Text = "Insert Prompt Text Here...";
            // 
            // responseTXT
            // 
            this.responseTXT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.responseTXT.Location = new System.Drawing.Point(13, 32);
            this.responseTXT.Name = "responseTXT";
            this.responseTXT.Size = new System.Drawing.Size(370, 20);
            this.responseTXT.TabIndex = 3;
            this.responseTXT.TextChanged += new System.EventHandler(this.responseTextChanged);
            // 
            // SingleStringPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 101);
            this.ControlBox = false;
            this.Controls.Add(this.responseTXT);
            this.Controls.Add(this.promptLBL);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.cancelBTN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 125);
            this.Name = "SingleStringPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Single String Prompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Label promptLBL;
        private System.Windows.Forms.TextBox responseTXT;
    }
}