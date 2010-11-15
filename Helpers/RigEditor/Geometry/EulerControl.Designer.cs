namespace s3piwrappers.RigEditor.Geometry
{
    partial class EulerControl
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
            this.dbRoll = new s3piwrappers.RigEditor.Geometry.AngleBox();
            this.dbPitch = new s3piwrappers.RigEditor.Geometry.AngleBox();
            this.dbYaw = new s3piwrappers.RigEditor.Geometry.AngleBox();
            this.lbRoll = new System.Windows.Forms.Label();
            this.lbPitch = new System.Windows.Forms.Label();
            this.lbYaw = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dbRoll
            // 
            this.dbRoll.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbRoll.DisplayUnits = s3piwrappers.RigEditor.Geometry.AngleBox.RotationUnit.Degrees;
            this.dbRoll.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbRoll.Location = new System.Drawing.Point(17, 0);
            this.dbRoll.Name = "dbRoll";
            this.dbRoll.Size = new System.Drawing.Size(100, 20);
            this.dbRoll.TabIndex = 0;
            this.dbRoll.Text = "0.000000";
            this.dbRoll.Value = 0D;
            this.dbRoll.Validated += new System.EventHandler(this.dbRoll_Validated);
            // 
            // dbPitch
            // 
            this.dbPitch.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbPitch.DisplayUnits = s3piwrappers.RigEditor.Geometry.AngleBox.RotationUnit.Degrees;
            this.dbPitch.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbPitch.Location = new System.Drawing.Point(261, 0);
            this.dbPitch.Name = "dbPitch";
            this.dbPitch.Size = new System.Drawing.Size(100, 20);
            this.dbPitch.TabIndex = 2;
            this.dbPitch.Text = "0.000000";
            this.dbPitch.Value = 0D;
            this.dbPitch.Validated += new System.EventHandler(this.dbPitch_Validated);
            // 
            // dbYaw
            // 
            this.dbYaw.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dbYaw.DisplayUnits = s3piwrappers.RigEditor.Geometry.AngleBox.RotationUnit.Degrees;
            this.dbYaw.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dbYaw.Location = new System.Drawing.Point(139, 0);
            this.dbYaw.Name = "dbYaw";
            this.dbYaw.Size = new System.Drawing.Size(100, 20);
            this.dbYaw.TabIndex = 1;
            this.dbYaw.Text = "0.000000";
            this.dbYaw.Value = 0D;
            this.dbYaw.Validated += new System.EventHandler(this.dbYaw_Validated);
            // 
            // lbRoll
            // 
            this.lbRoll.AutoSize = true;
            this.lbRoll.Location = new System.Drawing.Point(1, 3);
            this.lbRoll.Name = "lbRoll";
            this.lbRoll.Size = new System.Drawing.Size(17, 13);
            this.lbRoll.TabIndex = 3;
            this.lbRoll.Text = "X:";
            // 
            // lbPitch
            // 
            this.lbPitch.AutoSize = true;
            this.lbPitch.Location = new System.Drawing.Point(245, 3);
            this.lbPitch.Name = "lbPitch";
            this.lbPitch.Size = new System.Drawing.Size(17, 13);
            this.lbPitch.TabIndex = 5;
            this.lbPitch.Text = "Z:";
            // 
            // lbYaw
            // 
            this.lbYaw.AutoSize = true;
            this.lbYaw.Location = new System.Drawing.Point(123, 3);
            this.lbYaw.Name = "lbYaw";
            this.lbYaw.Size = new System.Drawing.Size(17, 13);
            this.lbYaw.TabIndex = 4;
            this.lbYaw.Text = "Y:";
            // 
            // EulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbRoll);
            this.Controls.Add(this.dbPitch);
            this.Controls.Add(this.dbYaw);
            this.Controls.Add(this.lbRoll);
            this.Controls.Add(this.lbPitch);
            this.Controls.Add(this.lbYaw);
            this.Name = "EulerControl";
            this.Size = new System.Drawing.Size(364, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        private AngleBox dbRoll;
        private AngleBox dbPitch;
        private AngleBox dbYaw;
        private System.Windows.Forms.Label lbRoll;
        private System.Windows.Forms.Label lbPitch;
        private System.Windows.Forms.Label lbYaw;
    }
}
