namespace s3piwrappers.BoneTool
{
    partial class AngleAxisControl
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
            this.dbAngle = new s3piwrappers.BoneTool.AngleBox();
            this.lbAngle = new System.Windows.Forms.Label();
            this.dbZ = new s3piwrappers.BoneTool.DoubleBox();
            this.dbY = new s3piwrappers.BoneTool.DoubleBox();
            this.dbX = new s3piwrappers.BoneTool.DoubleBox();
            this.lbZ = new System.Windows.Forms.Label();
            this.lbY = new System.Windows.Forms.Label();
            this.lbX = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dbAngle
            // 
            this.dbAngle.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbAngle.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbAngle.Location = new System.Drawing.Point(20, 83);
            this.dbAngle.Name = "dbAngle";
            this.dbAngle.Size = new System.Drawing.Size(100, 20);
            this.dbAngle.TabIndex = 15;
            this.dbAngle.Text = "0.000000";
            this.dbAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbAngle.Value = 0D;
            this.dbAngle.Validated += new System.EventHandler(this.dbAngle_Validated);
            // 
            // lbAngle
            // 
            this.lbAngle.AutoSize = true;
            this.lbAngle.Location = new System.Drawing.Point(4, 87);
            this.lbAngle.Name = "lbAngle";
            this.lbAngle.Size = new System.Drawing.Size(17, 13);
            this.lbAngle.TabIndex = 14;
            this.lbAngle.Text = "Θ:";
            // 
            // dbZ
            // 
            this.dbZ.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbZ.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbZ.Location = new System.Drawing.Point(20, 57);
            this.dbZ.Name = "dbZ";
            this.dbZ.Size = new System.Drawing.Size(100, 20);
            this.dbZ.TabIndex = 13;
            this.dbZ.Text = "0.000000";
            this.dbZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbZ.Value = 0D;
            this.dbZ.Validated += new System.EventHandler(this.dbZ_Validated);
            // 
            // dbY
            // 
            this.dbY.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbY.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbY.Location = new System.Drawing.Point(20, 31);
            this.dbY.Name = "dbY";
            this.dbY.Size = new System.Drawing.Size(100, 20);
            this.dbY.TabIndex = 12;
            this.dbY.Text = "0.000000";
            this.dbY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbY.Value = 0D;
            this.dbY.Validated += new System.EventHandler(this.dbY_Validated);
            // 
            // dbX
            // 
            this.dbX.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbX.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbX.Location = new System.Drawing.Point(20, 5);
            this.dbX.Name = "dbX";
            this.dbX.Size = new System.Drawing.Size(100, 20);
            this.dbX.TabIndex = 11;
            this.dbX.Text = "0.000000";
            this.dbX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbX.Value = 0D;
            this.dbX.Validated += new System.EventHandler(this.dbX_Validated);
            // 
            // lbZ
            // 
            this.lbZ.AutoSize = true;
            this.lbZ.Location = new System.Drawing.Point(4, 61);
            this.lbZ.Name = "lbZ";
            this.lbZ.Size = new System.Drawing.Size(17, 13);
            this.lbZ.TabIndex = 10;
            this.lbZ.Text = "Z:";
            // 
            // lbY
            // 
            this.lbY.AutoSize = true;
            this.lbY.Location = new System.Drawing.Point(4, 35);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(17, 13);
            this.lbY.TabIndex = 9;
            this.lbY.Text = "Y:";
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Location = new System.Drawing.Point(4, 9);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(17, 13);
            this.lbX.TabIndex = 8;
            this.lbX.Text = "X:";
            // 
            // AngleAxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbAngle);
            this.Controls.Add(this.lbAngle);
            this.Controls.Add(this.dbZ);
            this.Controls.Add(this.dbY);
            this.Controls.Add(this.dbX);
            this.Controls.Add(this.lbZ);
            this.Controls.Add(this.lbY);
            this.Controls.Add(this.lbX);
            this.Name = "AngleAxisControl";
            this.Size = new System.Drawing.Size(124, 108);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AngleBox dbAngle;
        private System.Windows.Forms.Label lbAngle;
        private DoubleBox dbZ;
        private DoubleBox dbY;
        private DoubleBox dbX;
        private System.Windows.Forms.Label lbZ;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.Label lbX;
    }
}
