namespace s3piwrappers.BoneTool.Geometry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbQuat = new System.Windows.Forms.GroupBox();
            this.gbAngleAxis = new System.Windows.Forms.GroupBox();
            this.gbEuler = new System.Windows.Forms.GroupBox();
            this.quaternionControl = new s3piwrappers.BoneTool.Geometry.QuaternionControl();
            this.angleAxisControl = new s3piwrappers.BoneTool.AngleAxisControl();
            this.eulerControl = new s3piwrappers.BoneTool.EulerControl();
            this.gbQuat.SuspendLayout();
            this.gbAngleAxis.SuspendLayout();
            this.gbEuler.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbQuat
            // 
            this.gbQuat.Controls.Add(this.quaternionControl);
            this.gbQuat.Location = new System.Drawing.Point(3, 3);
            this.gbQuat.Name = "gbQuat";
            this.gbQuat.Size = new System.Drawing.Size(132, 131);
            this.gbQuat.TabIndex = 3;
            this.gbQuat.TabStop = false;
            this.gbQuat.Text = "Quaternion";
            // 
            // gbAngleAxis
            // 
            this.gbAngleAxis.Controls.Add(this.angleAxisControl);
            this.gbAngleAxis.Location = new System.Drawing.Point(141, 3);
            this.gbAngleAxis.Name = "gbAngleAxis";
            this.gbAngleAxis.Size = new System.Drawing.Size(132, 131);
            this.gbAngleAxis.TabIndex = 4;
            this.gbAngleAxis.TabStop = false;
            this.gbAngleAxis.Text = "Angle Axis";
            // 
            // gbEuler
            // 
            this.gbEuler.Controls.Add(this.eulerControl);
            this.gbEuler.Location = new System.Drawing.Point(279, 3);
            this.gbEuler.Name = "gbEuler";
            this.gbEuler.Size = new System.Drawing.Size(156, 131);
            this.gbEuler.TabIndex = 4;
            this.gbEuler.TabStop = false;
            this.gbEuler.Text = "Euler";
            // 
            // quaternionControl1
            // 
            this.quaternionControl.Location = new System.Drawing.Point(2, 17);
            this.quaternionControl.Name = "quaternionControl";
            this.quaternionControl.Size = new System.Drawing.Size(124, 108);
            this.quaternionControl.TabIndex = 0;
            // 
            // angleAxisControl1
            // 
            this.angleAxisControl.Location = new System.Drawing.Point(6, 17);
            this.angleAxisControl.Name = "angleAxisControl";
            this.angleAxisControl.Size = new System.Drawing.Size(124, 108);
            this.angleAxisControl.TabIndex = 0;
            // 
            // eulerControl1
            // 
            this.eulerControl.Location = new System.Drawing.Point(6, 17);
            this.eulerControl.Name = "eulerControl";
            this.eulerControl.Size = new System.Drawing.Size(144, 78);
            this.eulerControl.TabIndex = 0;
            // 
            // RotationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbEuler);
            this.Controls.Add(this.gbAngleAxis);
            this.Controls.Add(this.gbQuat);
            this.Name = "RotationControl";
            this.Size = new System.Drawing.Size(438, 138);
            this.gbQuat.ResumeLayout(false);
            this.gbAngleAxis.ResumeLayout(false);
            this.gbEuler.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbQuat;
        private System.Windows.Forms.GroupBox gbAngleAxis;
        private System.Windows.Forms.GroupBox gbEuler;
        private QuaternionControl quaternionControl;
        private AngleAxisControl angleAxisControl;
        private EulerControl eulerControl;
    }
}
