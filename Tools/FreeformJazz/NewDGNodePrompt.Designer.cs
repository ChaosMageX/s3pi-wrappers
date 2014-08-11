namespace s3piwrappers.FreeformJazz
{
    partial class NewDGNodePrompt
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
            this.categoryLBL = new System.Windows.Forms.Label();
            this.categoryCMB = new System.Windows.Forms.ComboBox();
            this.nodeTypeLBL = new System.Windows.Forms.Label();
            this.nodeTypeCMB = new System.Windows.Forms.ComboBox();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // categoryLBL
            // 
            this.categoryLBL.AutoSize = true;
            this.categoryLBL.Location = new System.Drawing.Point(12, 16);
            this.categoryLBL.Name = "categoryLBL";
            this.categoryLBL.Size = new System.Drawing.Size(52, 13);
            this.categoryLBL.TabIndex = 0;
            this.categoryLBL.Text = "Category:";
            // 
            // categoryCMB
            // 
            this.categoryCMB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryCMB.FormattingEnabled = true;
            this.categoryCMB.Location = new System.Drawing.Point(72, 13);
            this.categoryCMB.Name = "categoryCMB";
            this.categoryCMB.Size = new System.Drawing.Size(210, 21);
            this.categoryCMB.TabIndex = 1;
            this.categoryCMB.SelectedIndexChanged += new System.EventHandler(this.categorySelectedIndexChanged);
            // 
            // nodeTypeLBL
            // 
            this.nodeTypeLBL.AutoSize = true;
            this.nodeTypeLBL.Location = new System.Drawing.Point(12, 43);
            this.nodeTypeLBL.Name = "nodeTypeLBL";
            this.nodeTypeLBL.Size = new System.Drawing.Size(34, 13);
            this.nodeTypeLBL.TabIndex = 2;
            this.nodeTypeLBL.Text = "Type:";
            // 
            // nodeTypeCMB
            // 
            this.nodeTypeCMB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeTypeCMB.FormattingEnabled = true;
            this.nodeTypeCMB.Location = new System.Drawing.Point(53, 40);
            this.nodeTypeCMB.Name = "nodeTypeCMB";
            this.nodeTypeCMB.Size = new System.Drawing.Size(229, 21);
            this.nodeTypeCMB.TabIndex = 3;
            this.nodeTypeCMB.SelectedIndexChanged += new System.EventHandler(this.nodeTypeSelectedIndexChanged);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBTN.Location = new System.Drawing.Point(206, 68);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(75, 23);
            this.cancelBTN.TabIndex = 4;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            // 
            // okBTN
            // 
            this.okBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBTN.Enabled = false;
            this.okBTN.Location = new System.Drawing.Point(125, 68);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(75, 23);
            this.okBTN.TabIndex = 5;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            // 
            // NewDGNodePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 101);
            this.ControlBox = false;
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.nodeTypeCMB);
            this.Controls.Add(this.nodeTypeLBL);
            this.Controls.Add(this.categoryCMB);
            this.Controls.Add(this.categoryLBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewDGNodePrompt";
            this.Text = "Freeform Jazz: Create New DG Node";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label categoryLBL;
        private System.Windows.Forms.ComboBox categoryCMB;
        private System.Windows.Forms.Label nodeTypeLBL;
        private System.Windows.Forms.ComboBox nodeTypeCMB;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button okBTN;
    }
}