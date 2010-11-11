namespace s3piwrappers.RigEditor.Geometry
{
    partial class RotationControl
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
            this.gbQuat = new System.Windows.Forms.GroupBox();
            this.quaternionControl = new s3piwrappers.RigEditor.Geometry.QuaternionControl();
            this.gbAngleAxis = new System.Windows.Forms.GroupBox();
            this.angleAxisControl = new s3piwrappers.RigEditor.Geometry.AngleAxisControl();
            this.gbEuler = new System.Windows.Forms.GroupBox();
            this.eulerControl = new s3piwrappers.RigEditor.Geometry.EulerControl();
            this.gbQuat.SuspendLayout();
            this.gbAngleAxis.SuspendLayout();
            this.gbEuler.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbQuat
            // 
            this.gbQuat.Controls.Add(this.quaternionControl);
            this.gbQuat.Location = new System.Drawing.Point(0, 0);
            this.gbQuat.Name = "gbQuat";
            this.gbQuat.Size = new System.Drawing.Size(132, 132);
            this.gbQuat.TabIndex = 0;
            this.gbQuat.TabStop = false;
            this.gbQuat.Text = "Quaternion";
            // 
            // quaternionControl
            // 
            this.quaternionControl.Location = new System.Drawing.Point(6, 15);
            this.quaternionControl.Name = "quaternionControl";
            this.quaternionControl.Size = new System.Drawing.Size(124, 108);
            this.quaternionControl.TabIndex = 1;
            // 
            // gbAngleAxis
            // 
            this.gbAngleAxis.Controls.Add(this.angleAxisControl);
            this.gbAngleAxis.Location = new System.Drawing.Point(140, 0);
            this.gbAngleAxis.Name = "gbAngleAxis";
            this.gbAngleAxis.Size = new System.Drawing.Size(132, 132);
            this.gbAngleAxis.TabIndex = 2;
            this.gbAngleAxis.TabStop = false;
            this.gbAngleAxis.Text = "Angle Axis";
            // 
            // angleAxisControl
            // 
            this.angleAxisControl.Location = new System.Drawing.Point(6, 15);
            this.angleAxisControl.Name = "angleAxisControl";
            this.angleAxisControl.Size = new System.Drawing.Size(124, 108);
            this.angleAxisControl.TabIndex = 3;
            // 
            // gbEuler
            // 
            this.gbEuler.Controls.Add(this.eulerControl);
            this.gbEuler.Location = new System.Drawing.Point(280, 0);
            this.gbEuler.Name = "gbEuler";
            this.gbEuler.Size = new System.Drawing.Size(150, 132);
            this.gbEuler.TabIndex = 4;
            this.gbEuler.TabStop = false;
            this.gbEuler.Text = "Euler";
            // 
            // eulerControl
            // 
            this.eulerControl.Location = new System.Drawing.Point(5, 15);
            this.eulerControl.Name = "eulerControl";
            this.eulerControl.Size = new System.Drawing.Size(144, 78);
            this.eulerControl.TabIndex = 5;
            // 
            // RotationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbEuler);
            this.Controls.Add(this.gbAngleAxis);
            this.Controls.Add(this.gbQuat);
            this.Name = "RotationControl";
            this.Size = new System.Drawing.Size(432, 135);
            this.gbQuat.ResumeLayout(false);
            this.gbAngleAxis.ResumeLayout(false);
            this.gbEuler.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        

        private System.Windows.Forms.GroupBox gbQuat;
        private System.Windows.Forms.GroupBox gbAngleAxis;
        private System.Windows.Forms.GroupBox gbEuler;
        private QuaternionControl quaternionControl;
        private AngleAxisControl angleAxisControl;
        private EulerControl eulerControl;
    }
}
