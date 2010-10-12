using s3piwrappers.RigEditor.Common;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
{
    partial class GrannyArtToolInfoControl
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
            this.lbFromArtToolName = new System.Windows.Forms.Label();
            this.tbFromArtToolName = new System.Windows.Forms.TextBox();
            this.lbFromArtToolMajorRevision = new System.Windows.Forms.Label();
            this.tbArtToolMajorRevision = new IntBox();
            this.tbArtToolMinorRevision = new IntBox();
            this.lbFromArtToolMinorRevision = new System.Windows.Forms.Label();
            this.lbUnitsPerMeter = new System.Windows.Forms.Label();
            this.tbUnitsPerMeter = new DoubleBox();
            this.lbOrigin = new System.Windows.Forms.Label();
            this.v3cOrigin = new Vector3Control();
            this.v3cRightVector = new Vector3Control();
            this.lbRightVector = new System.Windows.Forms.Label();
            this.v3cUpVector = new Vector3Control();
            this.lbUpVector = new System.Windows.Forms.Label();
            this.v3cBackVector = new Vector3Control();
            this.lbBackVector = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbFromArtToolName
            // 
            this.lbFromArtToolName.AutoSize = true;
            this.lbFromArtToolName.Location = new System.Drawing.Point(3, 4);
            this.lbFromArtToolName.Name = "lbFromArtToolName";
            this.lbFromArtToolName.Size = new System.Drawing.Size(98, 13);
            this.lbFromArtToolName.TabIndex = 0;
            this.lbFromArtToolName.Text = "FromArtTool Name:";
            // 
            // tbFromArtToolName
            // 
            this.tbFromArtToolName.Location = new System.Drawing.Point(149, 1);
            this.tbFromArtToolName.Name = "tbFromArtToolName";
            this.tbFromArtToolName.Size = new System.Drawing.Size(125, 20);
            this.tbFromArtToolName.TabIndex = 1;
            this.tbFromArtToolName.TextChanged += new System.EventHandler(this.tbFromArtToolName_TextChanged);
            // 
            // lbFromArtToolMajorRevision
            // 
            this.lbFromArtToolMajorRevision.AutoSize = true;
            this.lbFromArtToolMajorRevision.Location = new System.Drawing.Point(3, 27);
            this.lbFromArtToolMajorRevision.Name = "lbFromArtToolMajorRevision";
            this.lbFromArtToolMajorRevision.Size = new System.Drawing.Size(140, 13);
            this.lbFromArtToolMajorRevision.TabIndex = 2;
            this.lbFromArtToolMajorRevision.Text = "FromArtTool Major Revision:";
            // 
            // tbFromArtToolMajorRevision
            // 
            this.tbArtToolMajorRevision.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tbArtToolMajorRevision.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbArtToolMajorRevision.Location = new System.Drawing.Point(149, 24);
            this.tbArtToolMajorRevision.Name = "tbArtToolMajorRevision";
            this.tbArtToolMajorRevision.Size = new System.Drawing.Size(125, 20);
            this.tbArtToolMajorRevision.TabIndex = 3;
            this.tbArtToolMajorRevision.Text = "0";
            this.tbArtToolMajorRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbArtToolMajorRevision.Value = 0;
            this.tbArtToolMajorRevision.Validated += new System.EventHandler(this.tbFromArtToolMajorRevision_Validated);
            // 
            // tbFromArtToolMinorRevision
            // 
            this.tbArtToolMinorRevision.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tbArtToolMinorRevision.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbArtToolMinorRevision.Location = new System.Drawing.Point(149, 47);
            this.tbArtToolMinorRevision.Name = "tbArtToolMinorRevision";
            this.tbArtToolMinorRevision.Size = new System.Drawing.Size(125, 20);
            this.tbArtToolMinorRevision.TabIndex = 5;
            this.tbArtToolMinorRevision.Text = "0";
            this.tbArtToolMinorRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbArtToolMinorRevision.Value = 0;
            this.tbArtToolMinorRevision.Validated += new System.EventHandler(this.tbFromArtToolMinorRevision_Validated);
            // 
            // lbFromArtToolMinorRevision
            // 
            this.lbFromArtToolMinorRevision.AutoSize = true;
            this.lbFromArtToolMinorRevision.Location = new System.Drawing.Point(3, 50);
            this.lbFromArtToolMinorRevision.Name = "lbFromArtToolMinorRevision";
            this.lbFromArtToolMinorRevision.Size = new System.Drawing.Size(140, 13);
            this.lbFromArtToolMinorRevision.TabIndex = 4;
            this.lbFromArtToolMinorRevision.Text = "FromArtTool Minor Revision:";
            // 
            // lbUnitsPerMeter
            // 
            this.lbUnitsPerMeter.AutoSize = true;
            this.lbUnitsPerMeter.Location = new System.Drawing.Point(3, 72);
            this.lbUnitsPerMeter.Name = "lbUnitsPerMeter";
            this.lbUnitsPerMeter.Size = new System.Drawing.Size(83, 13);
            this.lbUnitsPerMeter.TabIndex = 6;
            this.lbUnitsPerMeter.Text = "Units Per Meter:";
            // 
            // tbUnitsPerMeter
            // 
            this.tbUnitsPerMeter.DefaultColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tbUnitsPerMeter.ErrorColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbUnitsPerMeter.Location = new System.Drawing.Point(149, 72);
            this.tbUnitsPerMeter.Name = "tbUnitsPerMeter";
            this.tbUnitsPerMeter.Size = new System.Drawing.Size(125, 20);
            this.tbUnitsPerMeter.TabIndex = 7;
            this.tbUnitsPerMeter.Text = "0.000000";
            this.tbUnitsPerMeter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbUnitsPerMeter.Value = 0D;
            this.tbUnitsPerMeter.Validated += new System.EventHandler(this.tbUnitsPerMeter_Validated);
            // 
            // lbOrigin
            // 
            this.lbOrigin.AutoSize = true;
            this.lbOrigin.Location = new System.Drawing.Point(3, 98);
            this.lbOrigin.Name = "lbOrigin";
            this.lbOrigin.Size = new System.Drawing.Size(37, 13);
            this.lbOrigin.TabIndex = 8;
            this.lbOrigin.Text = "Origin:";
            // 
            // v3cOrigin
            // 
            this.v3cOrigin.Location = new System.Drawing.Point(6, 114);
            this.v3cOrigin.Name = "v3cOrigin";
            this.v3cOrigin.Size = new System.Drawing.Size(365, 20);
            this.v3cOrigin.TabIndex = 9;
            // 
            // v3RightVector
            // 
            this.v3cRightVector.Location = new System.Drawing.Point(6, 151);
            this.v3cRightVector.Name = "v3cRightVector";
            this.v3cRightVector.Size = new System.Drawing.Size(365, 20);
            this.v3cRightVector.TabIndex = 11;
            // 
            // lbRightVector
            // 
            this.lbRightVector.AutoSize = true;
            this.lbRightVector.Location = new System.Drawing.Point(3, 135);
            this.lbRightVector.Name = "lbRightVector";
            this.lbRightVector.Size = new System.Drawing.Size(69, 13);
            this.lbRightVector.TabIndex = 10;
            this.lbRightVector.Text = "Right Vector:";
            // 
            // v3UpVector
            // 
            this.v3cUpVector.Location = new System.Drawing.Point(6, 190);
            this.v3cUpVector.Name = "v3cUpVector";
            this.v3cUpVector.Size = new System.Drawing.Size(365, 20);
            this.v3cUpVector.TabIndex = 13;
            // 
            // lbUpVector
            // 
            this.lbUpVector.AutoSize = true;
            this.lbUpVector.Location = new System.Drawing.Point(3, 174);
            this.lbUpVector.Name = "lbUpVector";
            this.lbUpVector.Size = new System.Drawing.Size(58, 13);
            this.lbUpVector.TabIndex = 12;
            this.lbUpVector.Text = "Up Vector:";
            // 
            // v3BackVector
            // 
            this.v3cBackVector.Location = new System.Drawing.Point(6, 227);
            this.v3cBackVector.Name = "v3cBackVector";
            this.v3cBackVector.Size = new System.Drawing.Size(365, 20);
            this.v3cBackVector.TabIndex = 15;
            // 
            // lbBackVector
            // 
            this.lbBackVector.AutoSize = true;
            this.lbBackVector.Location = new System.Drawing.Point(3, 211);
            this.lbBackVector.Name = "lbBackVector";
            this.lbBackVector.Size = new System.Drawing.Size(69, 13);
            this.lbBackVector.TabIndex = 14;
            this.lbBackVector.Text = "Back Vector:";
            // 
            // GrannyArtToolInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.v3cBackVector);
            this.Controls.Add(this.lbBackVector);
            this.Controls.Add(this.v3cUpVector);
            this.Controls.Add(this.lbUpVector);
            this.Controls.Add(this.v3cRightVector);
            this.Controls.Add(this.lbRightVector);
            this.Controls.Add(this.v3cOrigin);
            this.Controls.Add(this.lbOrigin);
            this.Controls.Add(this.tbUnitsPerMeter);
            this.Controls.Add(this.lbUnitsPerMeter);
            this.Controls.Add(this.tbArtToolMinorRevision);
            this.Controls.Add(this.lbFromArtToolMinorRevision);
            this.Controls.Add(this.tbArtToolMajorRevision);
            this.Controls.Add(this.lbFromArtToolMajorRevision);
            this.Controls.Add(this.tbFromArtToolName);
            this.Controls.Add(this.lbFromArtToolName);
            this.Name = "GrannyArtToolInfoControl";
            this.Size = new System.Drawing.Size(368, 247);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFromArtToolName;
        private System.Windows.Forms.TextBox tbFromArtToolName;
        private System.Windows.Forms.Label lbFromArtToolMajorRevision;
        private IntBox tbArtToolMajorRevision;
        private IntBox tbArtToolMinorRevision;
        private System.Windows.Forms.Label lbFromArtToolMinorRevision;
        private System.Windows.Forms.Label lbUnitsPerMeter;
        private DoubleBox tbUnitsPerMeter;
        private System.Windows.Forms.Label lbOrigin;
        private Geometry.Vector3Control v3cOrigin;
        private Geometry.Vector3Control v3cRightVector;
        private System.Windows.Forms.Label lbRightVector;
        private Geometry.Vector3Control v3cUpVector;
        private System.Windows.Forms.Label lbUpVector;
        private Geometry.Vector3Control v3cBackVector;
        private System.Windows.Forms.Label lbBackVector;

    }
}
