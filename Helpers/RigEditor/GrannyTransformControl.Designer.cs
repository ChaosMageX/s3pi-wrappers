using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
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

        
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.llbRotation = new System.Windows.Forms.LinkLabel();
            this.rotationControl = new s3piwrappers.RigEditor.Geometry.EulerControl();
            this.lbPosition = new System.Windows.Forms.Label();
            this.cbScaleEnabled = new System.Windows.Forms.CheckBox();
            this.scaleControl = new s3piwrappers.RigEditor.Geometry.Vector3Control();
            this.positionControl = new s3piwrappers.RigEditor.Geometry.Vector3Control();
            this.lbScale = new System.Windows.Forms.Label();
            this.cbPositionEnabled = new System.Windows.Forms.CheckBox();
            this.cbOrientationEnabled = new System.Windows.Forms.CheckBox();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.llbRotation);
            this.gbMain.Controls.Add(this.rotationControl);
            this.gbMain.Controls.Add(this.lbPosition);
            this.gbMain.Controls.Add(this.cbScaleEnabled);
            this.gbMain.Controls.Add(this.scaleControl);
            this.gbMain.Controls.Add(this.positionControl);
            this.gbMain.Controls.Add(this.lbScale);
            this.gbMain.Controls.Add(this.cbPositionEnabled);
            this.gbMain.Controls.Add(this.cbOrientationEnabled);
            this.gbMain.Location = new System.Drawing.Point(0, 0);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(370, 143);
            this.gbMain.TabIndex = 9;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Transform";
            // 
            // llbRotation
            // 
            this.llbRotation.AutoSize = true;
            this.llbRotation.Location = new System.Drawing.Point(5, 57);
            this.llbRotation.Name = "llbRotation";
            this.llbRotation.Size = new System.Drawing.Size(50, 13);
            this.llbRotation.TabIndex = 9;
            this.llbRotation.TabStop = true;
            this.llbRotation.Text = "Rotation:";
            this.llbRotation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbRotation_LinkClicked);
            // 
            // rotationControl
            // 
            this.rotationControl.Location = new System.Drawing.Point(3, 74);
            this.rotationControl.Name = "rotationControl";
            this.rotationControl.Size = new System.Drawing.Size(364, 21);
            this.rotationControl.TabIndex = 1;
            // 
            // lbPosition
            // 
            this.lbPosition.AutoSize = true;
            this.lbPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPosition.Location = new System.Drawing.Point(5, 16);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(47, 13);
            this.lbPosition.TabIndex = 1;
            this.lbPosition.Text = "Position:";
            // 
            // cbScaleEnabled
            // 
            this.cbScaleEnabled.AutoSize = true;
            this.cbScaleEnabled.Location = new System.Drawing.Point(75, 99);
            this.cbScaleEnabled.Name = "cbScaleEnabled";
            this.cbScaleEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbScaleEnabled.TabIndex = 8;
            this.cbScaleEnabled.TabStop = false;
            this.cbScaleEnabled.Text = "Enabled";
            this.cbScaleEnabled.UseVisualStyleBackColor = true;
            this.cbScaleEnabled.CheckedChanged += new System.EventHandler(this.cbScaleEnabled_CheckedChanged);
            // 
            // scaleControl
            // 
            this.scaleControl.Location = new System.Drawing.Point(3, 116);
            this.scaleControl.Name = "scaleControl";
            this.scaleControl.Size = new System.Drawing.Size(365, 20);
            this.scaleControl.TabIndex = 2;
            // 
            // positionControl
            // 
            this.positionControl.Location = new System.Drawing.Point(3, 33);
            this.positionControl.Name = "positionControl";
            this.positionControl.Size = new System.Drawing.Size(365, 20);
            this.positionControl.TabIndex = 0;
            // 
            // lbScale
            // 
            this.lbScale.AutoSize = true;
            this.lbScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScale.Location = new System.Drawing.Point(5, 99);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new System.Drawing.Size(37, 13);
            this.lbScale.TabIndex = 6;
            this.lbScale.Text = "Scale:";
            // 
            // cbPositionEnabled
            // 
            this.cbPositionEnabled.AutoSize = true;
            this.cbPositionEnabled.Location = new System.Drawing.Point(75, 16);
            this.cbPositionEnabled.Name = "cbPositionEnabled";
            this.cbPositionEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbPositionEnabled.TabIndex = 5;
            this.cbPositionEnabled.TabStop = false;
            this.cbPositionEnabled.Text = "Enabled";
            this.cbPositionEnabled.UseVisualStyleBackColor = true;
            this.cbPositionEnabled.CheckedChanged += new System.EventHandler(this.cbPositionEnabled_CheckedChanged);
            // 
            // cbOrientationEnabled
            // 
            this.cbOrientationEnabled.AutoSize = true;
            this.cbOrientationEnabled.Location = new System.Drawing.Point(75, 57);
            this.cbOrientationEnabled.Name = "cbOrientationEnabled";
            this.cbOrientationEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbOrientationEnabled.TabIndex = 4;
            this.cbOrientationEnabled.TabStop = false;
            this.cbOrientationEnabled.Text = "Enabled";
            this.cbOrientationEnabled.UseVisualStyleBackColor = true;
            this.cbOrientationEnabled.CheckedChanged += new System.EventHandler(this.cbRotationEnabled_CheckedChanged);
            // 
            // GrannyTransformControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMain);
            this.Name = "GrannyTransformControl";
            this.Size = new System.Drawing.Size(371, 146);
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Label lbPosition;
        private Geometry.Vector3Control positionControl;
        private System.Windows.Forms.CheckBox cbOrientationEnabled;
        private System.Windows.Forms.CheckBox cbPositionEnabled;
        private System.Windows.Forms.CheckBox cbScaleEnabled;
        private Geometry.Vector3Control scaleControl;
        private System.Windows.Forms.Label lbScale;
        private System.Windows.Forms.GroupBox gbMain;
        private EulerControl rotationControl;
        private System.Windows.Forms.LinkLabel llbRotation;

    }
}
