namespace s3piwrappers.FreeformJazz
{
    partial class Splash
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.smallMessageLBL = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.largeMessagePNL = new System.Windows.Forms.Panel();
            this.largeMessageLBL = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.largeMessagePNL.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.smallMessageLBL, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 175);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.iconPictureBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.largeMessagePNL, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(494, 125);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // smallMessageLBL
            // 
            this.smallMessageLBL.AutoSize = true;
            this.smallMessageLBL.Location = new System.Drawing.Point(3, 131);
            this.smallMessageLBL.Name = "smallMessageLBL";
            this.smallMessageLBL.Size = new System.Drawing.Size(87, 13);
            this.smallMessageLBL.TabIndex = 1;
            this.smallMessageLBL.Text = "Small Message...";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Location = new System.Drawing.Point(3, 3);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(64, 64);
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // largeMessagePNL
            // 
            this.largeMessagePNL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.largeMessagePNL.Controls.Add(this.largeMessageLBL);
            this.largeMessagePNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.largeMessagePNL.Location = new System.Drawing.Point(77, 7);
            this.largeMessagePNL.Margin = new System.Windows.Forms.Padding(7);
            this.largeMessagePNL.Name = "largeMessagePNL";
            this.largeMessagePNL.Size = new System.Drawing.Size(410, 111);
            this.largeMessagePNL.TabIndex = 1;
            // 
            // largeMessageLBL
            // 
            this.largeMessageLBL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.largeMessageLBL.AutoSize = true;
            this.largeMessageLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.largeMessageLBL.Location = new System.Drawing.Point(3, 41);
            this.largeMessageLBL.Name = "largeMessageLBL";
            this.largeMessageLBL.Size = new System.Drawing.Size(198, 29);
            this.largeMessageLBL.TabIndex = 0;
            this.largeMessageLBL.Text = "Large Message...";
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(500, 175);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.largeMessagePNL.ResumeLayout(false);
            this.largeMessagePNL.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Panel largeMessagePNL;
        private System.Windows.Forms.Label largeMessageLBL;
        private System.Windows.Forms.Label smallMessageLBL;
    }
}