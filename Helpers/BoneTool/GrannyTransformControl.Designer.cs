namespace s3piwrappers.BoneTool
{
    partial class GrannyTransformControl
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
            this.rotationControl = new s3piwrappers.BoneTool.Geometry.RotationControl();
            this.lbPosition = new System.Windows.Forms.Label();
            this.positionControl = new s3piwrappers.BoneTool.Geometry.Vector3Control();
            this.lbOrientation = new System.Windows.Forms.Label();
            this.cbOrientationEnabled = new System.Windows.Forms.CheckBox();
            this.cbPositionEnabled = new System.Windows.Forms.CheckBox();
            this.cbScaleEnabled = new System.Windows.Forms.CheckBox();
            this.scaleControl = new s3piwrappers.BoneTool.Geometry.Vector3Control();
            this.lbScale = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rotationControl
            // 
            this.rotationControl.Location = new System.Drawing.Point(-1, 77);
            this.rotationControl.Name = "rotationControl";
            this.rotationControl.Size = new System.Drawing.Size(438, 138);
            this.rotationControl.TabIndex = 0;
            // 
            // lbPosition
            // 
            this.lbPosition.AutoSize = true;
            this.lbPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPosition.Location = new System.Drawing.Point(8, 9);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(47, 13);
            this.lbPosition.TabIndex = 1;
            this.lbPosition.Text = "Position:";
            // 
            // positionControl
            // 
            this.positionControl.Location = new System.Drawing.Point(5, 26);
            this.positionControl.Name = "positionControl";
            this.positionControl.Size = new System.Drawing.Size(365, 20);
            this.positionControl.TabIndex = 2;
            // 
            // lbOrientation
            // 
            this.lbOrientation.AutoSize = true;
            this.lbOrientation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOrientation.Location = new System.Drawing.Point(8, 61);
            this.lbOrientation.Name = "lbOrientation";
            this.lbOrientation.Size = new System.Drawing.Size(61, 13);
            this.lbOrientation.TabIndex = 3;
            this.lbOrientation.Text = "Orientation:";
            // 
            // cbOrientationEnabled
            // 
            this.cbOrientationEnabled.AutoSize = true;
            this.cbOrientationEnabled.Location = new System.Drawing.Point(75, 61);
            this.cbOrientationEnabled.Name = "cbOrientationEnabled";
            this.cbOrientationEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbOrientationEnabled.TabIndex = 4;
            this.cbOrientationEnabled.Text = "Enabled";
            this.cbOrientationEnabled.UseVisualStyleBackColor = true;
            this.cbOrientationEnabled.CheckedChanged += new System.EventHandler(this.cbOrientationEnabled_CheckedChanged);
            // 
            // cbPositionEnabled
            // 
            this.cbPositionEnabled.AutoSize = true;
            this.cbPositionEnabled.Location = new System.Drawing.Point(75, 9);
            this.cbPositionEnabled.Name = "cbPositionEnabled";
            this.cbPositionEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbPositionEnabled.TabIndex = 5;
            this.cbPositionEnabled.Text = "Enabled";
            this.cbPositionEnabled.UseVisualStyleBackColor = true;
            this.cbPositionEnabled.CheckedChanged += new System.EventHandler(this.cbPositionEnabled_CheckedChanged);
            // 
            // cbScaleEnabled
            // 
            this.cbScaleEnabled.AutoSize = true;
            this.cbScaleEnabled.Location = new System.Drawing.Point(75, 220);
            this.cbScaleEnabled.Name = "cbScaleEnabled";
            this.cbScaleEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbScaleEnabled.TabIndex = 8;
            this.cbScaleEnabled.Text = "Enabled";
            this.cbScaleEnabled.UseVisualStyleBackColor = true;
            this.cbScaleEnabled.CheckedChanged += new System.EventHandler(this.cbScaleEnabled_CheckedChanged);
            // 
            // scaleControl
            // 
            this.scaleControl.Location = new System.Drawing.Point(5, 237);
            this.scaleControl.Name = "scaleControl";
            this.scaleControl.Size = new System.Drawing.Size(365, 20);
            this.scaleControl.TabIndex = 7;
            // 
            // lbScale
            // 
            this.lbScale.AutoSize = true;
            this.lbScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScale.Location = new System.Drawing.Point(8, 220);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new System.Drawing.Size(37, 13);
            this.lbScale.TabIndex = 6;
            this.lbScale.Text = "Scale:";
            // 
            // GrannyTransformControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbScaleEnabled);
            this.Controls.Add(this.scaleControl);
            this.Controls.Add(this.lbScale);
            this.Controls.Add(this.cbPositionEnabled);
            this.Controls.Add(this.cbOrientationEnabled);
            this.Controls.Add(this.lbOrientation);
            this.Controls.Add(this.positionControl);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.rotationControl);
            this.Name = "GrannyTransformControl";
            this.Size = new System.Drawing.Size(435, 275);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geometry.RotationControl rotationControl;
        private System.Windows.Forms.Label lbPosition;
        private Geometry.Vector3Control positionControl;
        private System.Windows.Forms.Label lbOrientation;
        private System.Windows.Forms.CheckBox cbOrientationEnabled;
        private System.Windows.Forms.CheckBox cbPositionEnabled;
        private System.Windows.Forms.CheckBox cbScaleEnabled;
        private Geometry.Vector3Control scaleControl;
        private System.Windows.Forms.Label lbScale;

    }
}
