namespace s3piwrappers.RigEditor
{
    partial class GrannyModelControl
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
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tcInitialPlacement = new s3piwrappers.RigEditor.GrannyTransformControl();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(6, 6);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(50, 3);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(340, 20);
            this.tbName.TabIndex = 1;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tcInitialPlacement
            // 
            this.tcInitialPlacement.Location = new System.Drawing.Point(0, 30);
            this.tcInitialPlacement.Name = "tcInitialPlacement";
            this.tcInitialPlacement.Size = new System.Drawing.Size(448, 277);
            this.tcInitialPlacement.TabIndex = 2;
            this.tcInitialPlacement.Title = "Initial Placement:";
            this.tcInitialPlacement.Value = null;
            // 
            // GrannyModelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcInitialPlacement);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Name = "GrannyModelControl";
            this.Size = new System.Drawing.Size(449, 306);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private GrannyTransformControl tcInitialPlacement;
    }
}
