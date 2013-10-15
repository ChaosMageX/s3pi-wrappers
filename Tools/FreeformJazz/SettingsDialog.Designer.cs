namespace s3piwrappers.FreeformJazz
{
    partial class SettingsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.graphVisualTabPage = new System.Windows.Forms.TabPage();
            this.graphVisualPropGrid = new System.Windows.Forms.PropertyGrid();
            this.graphVisualPanel = new System.Windows.Forms.Panel();
            this.previewFontChangesCHK = new System.Windows.Forms.CheckBox();
            this.previewColorChangesCHK = new System.Windows.Forms.CheckBox();
            this.stateGraphLayoutTabPage = new System.Windows.Forms.TabPage();
            this.mainButtonPanel = new System.Windows.Forms.Panel();
            this.applyBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.previewFloatChangesCHK = new System.Windows.Forms.CheckBox();
            this.mainTabControl.SuspendLayout();
            this.graphVisualTabPage.SuspendLayout();
            this.graphVisualPanel.SuspendLayout();
            this.mainButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.graphVisualTabPage);
            this.mainTabControl.Controls.Add(this.stateGraphLayoutTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(484, 428);
            this.mainTabControl.TabIndex = 0;
            // 
            // graphVisualTabPage
            // 
            this.graphVisualTabPage.Controls.Add(this.graphVisualPropGrid);
            this.graphVisualTabPage.Controls.Add(this.graphVisualPanel);
            this.graphVisualTabPage.Location = new System.Drawing.Point(4, 22);
            this.graphVisualTabPage.Name = "graphVisualTabPage";
            this.graphVisualTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.graphVisualTabPage.Size = new System.Drawing.Size(476, 402);
            this.graphVisualTabPage.TabIndex = 0;
            this.graphVisualTabPage.Text = "Graph Appearance";
            this.graphVisualTabPage.UseVisualStyleBackColor = true;
            // 
            // graphVisualPropGrid
            // 
            this.graphVisualPropGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphVisualPropGrid.Location = new System.Drawing.Point(3, 3);
            this.graphVisualPropGrid.Name = "graphVisualPropGrid";
            this.graphVisualPropGrid.Size = new System.Drawing.Size(470, 359);
            this.graphVisualPropGrid.TabIndex = 1;
            // 
            // graphVisualPanel
            // 
            this.graphVisualPanel.Controls.Add(this.previewFloatChangesCHK);
            this.graphVisualPanel.Controls.Add(this.previewFontChangesCHK);
            this.graphVisualPanel.Controls.Add(this.previewColorChangesCHK);
            this.graphVisualPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.graphVisualPanel.Location = new System.Drawing.Point(3, 368);
            this.graphVisualPanel.Name = "graphVisualPanel";
            this.graphVisualPanel.Size = new System.Drawing.Size(470, 31);
            this.graphVisualPanel.TabIndex = 0;
            // 
            // previewFontChangesCHK
            // 
            this.previewFontChangesCHK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previewFontChangesCHK.AutoSize = true;
            this.previewFontChangesCHK.Location = new System.Drawing.Point(311, 9);
            this.previewFontChangesCHK.Name = "previewFontChangesCHK";
            this.previewFontChangesCHK.Size = new System.Drawing.Size(133, 17);
            this.previewFontChangesCHK.TabIndex = 1;
            this.previewFontChangesCHK.Text = "Preview Font Changes";
            this.previewFontChangesCHK.UseVisualStyleBackColor = true;
            this.previewFontChangesCHK.CheckedChanged += new System.EventHandler(this.previewFontChangesCheckedChanged);
            // 
            // previewColorChangesCHK
            // 
            this.previewColorChangesCHK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previewColorChangesCHK.AutoSize = true;
            this.previewColorChangesCHK.Checked = true;
            this.previewColorChangesCHK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.previewColorChangesCHK.Location = new System.Drawing.Point(12, 9);
            this.previewColorChangesCHK.Name = "previewColorChangesCHK";
            this.previewColorChangesCHK.Size = new System.Drawing.Size(136, 17);
            this.previewColorChangesCHK.TabIndex = 0;
            this.previewColorChangesCHK.Text = "Preview Color Changes";
            this.previewColorChangesCHK.UseVisualStyleBackColor = true;
            this.previewColorChangesCHK.CheckedChanged += new System.EventHandler(this.previewColorChangesCheckedChanged);
            // 
            // stateGraphLayoutTabPage
            // 
            this.stateGraphLayoutTabPage.Location = new System.Drawing.Point(4, 22);
            this.stateGraphLayoutTabPage.Name = "stateGraphLayoutTabPage";
            this.stateGraphLayoutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stateGraphLayoutTabPage.Size = new System.Drawing.Size(476, 402);
            this.stateGraphLayoutTabPage.TabIndex = 1;
            this.stateGraphLayoutTabPage.Text = "State Graph Layout";
            this.stateGraphLayoutTabPage.UseVisualStyleBackColor = true;
            // 
            // mainButtonPanel
            // 
            this.mainButtonPanel.Controls.Add(this.applyBTN);
            this.mainButtonPanel.Controls.Add(this.okBTN);
            this.mainButtonPanel.Controls.Add(this.cancelBTN);
            this.mainButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainButtonPanel.Location = new System.Drawing.Point(0, 434);
            this.mainButtonPanel.Name = "mainButtonPanel";
            this.mainButtonPanel.Size = new System.Drawing.Size(484, 32);
            this.mainButtonPanel.TabIndex = 1;
            // 
            // applyBTN
            // 
            this.applyBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyBTN.Enabled = false;
            this.applyBTN.Location = new System.Drawing.Point(350, 6);
            this.applyBTN.Name = "applyBTN";
            this.applyBTN.Size = new System.Drawing.Size(41, 23);
            this.applyBTN.TabIndex = 2;
            this.applyBTN.Text = "Apply";
            this.applyBTN.UseVisualStyleBackColor = true;
            this.applyBTN.Click += new System.EventHandler(this.applyClick);
            // 
            // okBTN
            // 
            this.okBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBTN.Location = new System.Drawing.Point(397, 6);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(30, 23);
            this.okBTN.TabIndex = 1;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okClick);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBTN.Location = new System.Drawing.Point(433, 6);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(48, 23);
            this.cancelBTN.TabIndex = 0;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelClick);
            // 
            // previewFloatChangesCHK
            // 
            this.previewFloatChangesCHK.AutoSize = true;
            this.previewFloatChangesCHK.Location = new System.Drawing.Point(154, 9);
            this.previewFloatChangesCHK.Name = "previewFloatChangesCHK";
            this.previewFloatChangesCHK.Size = new System.Drawing.Size(151, 17);
            this.previewFloatChangesCHK.TabIndex = 2;
            this.previewFloatChangesCHK.Text = "Preview Numeric Changes";
            this.previewFloatChangesCHK.UseVisualStyleBackColor = true;
            this.previewFloatChangesCHK.CheckedChanged += new System.EventHandler(this.previewFloatChangesCheckedChanged);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 466);
            this.ControlBox = false;
            this.Controls.Add(this.mainButtonPanel);
            this.Controls.Add(this.mainTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SettingsDialog";
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.OnDialogLoad);
            this.Shown += new System.EventHandler(this.OnDialogShown);
            this.mainTabControl.ResumeLayout(false);
            this.graphVisualTabPage.ResumeLayout(false);
            this.graphVisualPanel.ResumeLayout(false);
            this.graphVisualPanel.PerformLayout();
            this.mainButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage graphVisualTabPage;
        private System.Windows.Forms.PropertyGrid graphVisualPropGrid;
        private System.Windows.Forms.Panel graphVisualPanel;
        private System.Windows.Forms.CheckBox previewFontChangesCHK;
        private System.Windows.Forms.CheckBox previewColorChangesCHK;
        private System.Windows.Forms.TabPage stateGraphLayoutTabPage;
        private System.Windows.Forms.Panel mainButtonPanel;
        private System.Windows.Forms.Button applyBTN;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.CheckBox previewFloatChangesCHK;
    }
}