namespace s3piwrappers.CustomForms
{
    partial class SelectResourceDialog
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
            this.packageLBL = new System.Windows.Forms.Label();
            this.packageCMB = new System.Windows.Forms.ComboBox();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.okBTN = new System.Windows.Forms.Button();
            this.resDGV = new System.Windows.Forms.DataGridView();
            this.categoryLBL = new System.Windows.Forms.Label();
            this.categoryCMB = new System.Windows.Forms.ComboBox();
            this.jazzIIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jazzGIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jazzNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.resDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // packageLBL
            // 
            this.packageLBL.AutoSize = true;
            this.packageLBL.Location = new System.Drawing.Point(178, 16);
            this.packageLBL.Name = "packageLBL";
            this.packageLBL.Size = new System.Drawing.Size(53, 13);
            this.packageLBL.TabIndex = 0;
            this.packageLBL.Text = "Package:";
            // 
            // packageCMB
            // 
            this.packageCMB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageCMB.FormattingEnabled = true;
            this.packageCMB.Location = new System.Drawing.Point(237, 13);
            this.packageCMB.Name = "packageCMB";
            this.packageCMB.Size = new System.Drawing.Size(335, 21);
            this.packageCMB.TabIndex = 1;
            this.packageCMB.SelectionChangeCommitted += new System.EventHandler(this.packageSelectChangeCommitted);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBTN.Location = new System.Drawing.Point(522, 231);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(50, 23);
            this.cancelBTN.TabIndex = 2;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            // 
            // okBTN
            // 
            this.okBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBTN.Enabled = false;
            this.okBTN.Location = new System.Drawing.Point(485, 231);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(31, 23);
            this.okBTN.TabIndex = 3;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            // 
            // resDGV
            // 
            this.resDGV.AllowUserToAddRows = false;
            this.resDGV.AllowUserToDeleteRows = false;
            this.resDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.jazzIIDColumn,
            this.jazzGIDColumn,
            this.jazzNameColumn});
            this.resDGV.Location = new System.Drawing.Point(15, 40);
            this.resDGV.MultiSelect = false;
            this.resDGV.Name = "resDGV";
            this.resDGV.ReadOnly = true;
            this.resDGV.RowHeadersVisible = false;
            this.resDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resDGV.Size = new System.Drawing.Size(557, 185);
            this.resDGV.TabIndex = 4;
            this.resDGV.VirtualMode = true;
            this.resDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resCellContentClick);
            this.resDGV.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resCellContentDoubleClick);
            this.resDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.resCellValueNeeded);
            this.resDGV.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.resColumnHeaderMouseClick);
            // 
            // categoryLBL
            // 
            this.categoryLBL.AutoSize = true;
            this.categoryLBL.Location = new System.Drawing.Point(12, 16);
            this.categoryLBL.Name = "categoryLBL";
            this.categoryLBL.Size = new System.Drawing.Size(52, 13);
            this.categoryLBL.TabIndex = 5;
            this.categoryLBL.Text = "Category:";
            // 
            // categoryCMB
            // 
            this.categoryCMB.FormattingEnabled = true;
            this.categoryCMB.Location = new System.Drawing.Point(71, 13);
            this.categoryCMB.Name = "categoryCMB";
            this.categoryCMB.Size = new System.Drawing.Size(101, 21);
            this.categoryCMB.TabIndex = 6;
            this.categoryCMB.SelectionChangeCommitted += new System.EventHandler(this.categorySelectChangeCommitted);
            this.categoryCMB.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.categoryFormat);
            // 
            // jazzIIDColumn
            // 
            this.jazzIIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.jazzIIDColumn.HeaderText = "Instance ID";
            this.jazzIIDColumn.Name = "jazzIIDColumn";
            this.jazzIIDColumn.ReadOnly = true;
            this.jazzIIDColumn.Width = 87;
            // 
            // jazzGIDColumn
            // 
            this.jazzGIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.jazzGIDColumn.HeaderText = "Group ID";
            this.jazzGIDColumn.Name = "jazzGIDColumn";
            this.jazzGIDColumn.ReadOnly = true;
            this.jazzGIDColumn.Width = 75;
            // 
            // jazzNameColumn
            // 
            this.jazzNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.jazzNameColumn.HeaderText = "Name";
            this.jazzNameColumn.Name = "jazzNameColumn";
            this.jazzNameColumn.ReadOnly = true;
            // 
            // SelectResourceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 266);
            this.ControlBox = false;
            this.Controls.Add(this.categoryCMB);
            this.Controls.Add(this.categoryLBL);
            this.Controls.Add(this.resDGV);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.packageCMB);
            this.Controls.Add(this.packageLBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SelectResourceDialog";
            this.Text = "Select Resource";
            ((System.ComponentModel.ISupportInitialize)(this.resDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label packageLBL;
        private System.Windows.Forms.ComboBox packageCMB;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.DataGridView resDGV;
        private System.Windows.Forms.Label categoryLBL;
        private System.Windows.Forms.ComboBox categoryCMB;
        private System.Windows.Forms.DataGridViewTextBoxColumn jazzIIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jazzGIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jazzNameColumn;
    }
}