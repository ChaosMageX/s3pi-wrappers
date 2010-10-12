using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    partial class Vector3Control
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
            this.lbX = new System.Windows.Forms.Label();
            this.lbY = new System.Windows.Forms.Label();
            this.lbZ = new System.Windows.Forms.Label();
            this.dbX = new DoubleBox();
            this.dbY = new DoubleBox();
            this.dbZ = new DoubleBox();
            this.SuspendLayout();
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Location = new System.Drawing.Point(1, 3);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(17, 13);
            this.lbX.TabIndex = 0;
            this.lbX.Text = "X:";
            // 
            // lbY
            // 
            this.lbY.AutoSize = true;
            this.lbY.Location = new System.Drawing.Point(123, 3);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(17, 13);
            this.lbY.TabIndex = 1;
            this.lbY.Text = "Y:";
            // 
            // lbZ
            // 
            this.lbZ.AutoSize = true;
            this.lbZ.Location = new System.Drawing.Point(245, 3);
            this.lbZ.Name = "lbZ";
            this.lbZ.Size = new System.Drawing.Size(17, 13);
            this.lbZ.TabIndex = 2;
            this.lbZ.Text = "Z:";
            // 
            // dbX
            // 
            this.dbX.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbX.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbX.Location = new System.Drawing.Point(17, 0);
            this.dbX.Name = "dbX";
            this.dbX.Size = new System.Drawing.Size(100, 20);
            this.dbX.TabIndex = 3;
            this.dbX.Text = "0.000000";
            this.dbX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbX.Value = 0D;
            this.dbX.Validated += new System.EventHandler(this.dbX_Validated);
            // 
            // dbY
            // 
            this.dbY.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbY.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbY.Location = new System.Drawing.Point(139, 0);
            this.dbY.Name = "dbY";
            this.dbY.Size = new System.Drawing.Size(100, 20);
            this.dbY.TabIndex = 4;
            this.dbY.Text = "0.000000";
            this.dbY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbY.Value = 0D;
            this.dbY.Validated += new System.EventHandler(this.dbY_Validated);
            // 
            // dbZ
            // 
            this.dbZ.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbZ.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbZ.Location = new System.Drawing.Point(261, 0);
            this.dbZ.Name = "dbZ";
            this.dbZ.Size = new System.Drawing.Size(100, 20);
            this.dbZ.TabIndex = 5;
            this.dbZ.Text = "0.000000";
            this.dbZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dbZ.Value = 0D;
            this.dbZ.Validated += new System.EventHandler(this.dbZ_Validated);
            // 
            // Vector3Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbZ);
            this.Controls.Add(this.dbY);
            this.Controls.Add(this.dbX);
            this.Controls.Add(this.lbZ);
            this.Controls.Add(this.lbY);
            this.Controls.Add(this.lbX);
            this.Name = "Vector3Control";
            this.Size = new System.Drawing.Size(365, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.Label lbZ;
        private DoubleBox dbX;
        private DoubleBox dbY;
        private DoubleBox dbZ;
    }
}
