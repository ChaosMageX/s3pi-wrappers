using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    partial class Matrix3Control
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
            this.dbM00 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM01 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM02 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM12 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM11 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM10 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM22 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM21 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.dbM20 = new s3piwrappers.RigEditor.Common.DoubleBox();
            this.SuspendLayout();
            // 
            // dbM11
            // 
            this.dbM00.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM00.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM00.Location = new System.Drawing.Point(0, 3);
            this.dbM00.Name = "dbM00";
            this.dbM00.Size = new System.Drawing.Size(100, 20);
            this.dbM00.TabIndex = 11;
            this.dbM00.Text = "0.000000";
            this.dbM00.Value = 0D;
            this.dbM00.Validated += new System.EventHandler(this.dbM00_Validated);
            // 
            // dbM12
            // 
            this.dbM01.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM01.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM01.Location = new System.Drawing.Point(106, 3);
            this.dbM01.Name = "dbM01";
            this.dbM01.Size = new System.Drawing.Size(100, 20);
            this.dbM01.TabIndex = 13;
            this.dbM01.Text = "0.000000";
            this.dbM01.Value = 0D;
            this.dbM01.Validated += new System.EventHandler(this.dbM01_Validated);
            // 
            // dbM13
            // 
            this.dbM02.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM02.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM02.Location = new System.Drawing.Point(212, 3);
            this.dbM02.Name = "dbM02";
            this.dbM02.Size = new System.Drawing.Size(100, 20);
            this.dbM02.TabIndex = 15;
            this.dbM02.Text = "0.000000";
            this.dbM02.Value = 0D;
            this.dbM02.Validated += new System.EventHandler(this.dbM02_Validated);
            // 
            // dbM23
            // 
            this.dbM12.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM12.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM12.Location = new System.Drawing.Point(212, 29);
            this.dbM12.Name = "dbM12";
            this.dbM12.Size = new System.Drawing.Size(100, 20);
            this.dbM12.TabIndex = 23;
            this.dbM12.Text = "0.000000";
            this.dbM12.Value = 0D;
            this.dbM12.Validated += new System.EventHandler(this.dbM12_Validated);
            // 
            // dbM22
            // 
            this.dbM11.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM11.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM11.Location = new System.Drawing.Point(106, 29);
            this.dbM11.Name = "dbM11";
            this.dbM11.Size = new System.Drawing.Size(100, 20);
            this.dbM11.TabIndex = 21;
            this.dbM11.Text = "0.000000";
            this.dbM11.Value = 0D;
            this.dbM11.Validated += new System.EventHandler(this.dbM11_Validated);
            // 
            // dbM21
            // 
            this.dbM10.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM10.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM10.Location = new System.Drawing.Point(0, 29);
            this.dbM10.Name = "dbM10";
            this.dbM10.Size = new System.Drawing.Size(100, 20);
            this.dbM10.TabIndex = 19;
            this.dbM10.Text = "0.000000";
            this.dbM10.Value = 0D;
            this.dbM10.Validated += new System.EventHandler(this.dbM10_Validated);
            // 
            // dbM33
            // 
            this.dbM22.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM22.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM22.Location = new System.Drawing.Point(212, 55);
            this.dbM22.Name = "dbM22";
            this.dbM22.Size = new System.Drawing.Size(100, 20);
            this.dbM22.TabIndex = 31;
            this.dbM22.Text = "0.000000";
            this.dbM22.Value = 0D;
            this.dbM22.Validated += new System.EventHandler(this.dbM22_Validated);
            // 
            // dbM32
            // 
            this.dbM21.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM21.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM21.Location = new System.Drawing.Point(106, 55);
            this.dbM21.Name = "dbM21";
            this.dbM21.Size = new System.Drawing.Size(100, 20);
            this.dbM21.TabIndex = 29;
            this.dbM21.Text = "0.000000";
            this.dbM21.Value = 0D;
            this.dbM21.Validated += new System.EventHandler(this.dbM21_Validated);
            // 
            // dbM31
            // 
            this.dbM20.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbM20.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbM20.Location = new System.Drawing.Point(0, 55);
            this.dbM20.Name = "dbM20";
            this.dbM20.Size = new System.Drawing.Size(100, 20);
            this.dbM20.TabIndex = 27;
            this.dbM20.Text = "0.000000";
            this.dbM20.Value = 0D;
            this.dbM20.Validated += new System.EventHandler(this.dbM20_Validated);
            // 
            // Matrix3Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbM22);
            this.Controls.Add(this.dbM21);
            this.Controls.Add(this.dbM20);
            this.Controls.Add(this.dbM12);
            this.Controls.Add(this.dbM11);
            this.Controls.Add(this.dbM10);
            this.Controls.Add(this.dbM02);
            this.Controls.Add(this.dbM01);
            this.Controls.Add(this.dbM00);
            this.Name = "Matrix3Control";
            this.Size = new System.Drawing.Size(314, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        private DoubleBox dbM00;
        private DoubleBox dbM01;
        private DoubleBox dbM02;
        private DoubleBox dbM12;
        private DoubleBox dbM11;
        private DoubleBox dbM10;
        private DoubleBox dbM22;
        private DoubleBox dbM21;
        private DoubleBox dbM20;
    }
}
