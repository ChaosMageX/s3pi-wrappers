namespace s3piwrappers.CustomForms
{
    partial class SelectEditRIEDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.messageLBL = new System.Windows.Forms.Label();
            this.resDGV = new System.Windows.Forms.DataGridView();
            this.resTagColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.resTIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resGIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resIIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resCompColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.resNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.okBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.fnvClipBTN = new System.Windows.Forms.Button();
            this.fnv64BTN = new System.Windows.Forms.Button();
            this.fnv32BTN = new System.Windows.Forms.Button();
            this.copyRKsBTN = new System.Windows.Forms.Button();
            this.pasteRKsBTN = new System.Windows.Forms.Button();
            this.compressAllCHK = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resDGV)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.messageLBL, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.resDGV, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(560, 263);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // messageLBL
            // 
            this.messageLBL.AutoSize = true;
            this.messageLBL.Location = new System.Drawing.Point(3, 0);
            this.messageLBL.Name = "messageLBL";
            this.messageLBL.Size = new System.Drawing.Size(105, 13);
            this.messageLBL.TabIndex = 0;
            this.messageLBL.Text = "Insert Message Here";
            // 
            // resDGV
            // 
            this.resDGV.AllowUserToAddRows = false;
            this.resDGV.AllowUserToDeleteRows = false;
            this.resDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resTagColumn,
            this.resTIDColumn,
            this.resGIDColumn,
            this.resIIDColumn,
            this.resCompColumn,
            this.resNameColumn});
            this.resDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resDGV.Location = new System.Drawing.Point(3, 21);
            this.resDGV.Name = "resDGV";
            this.resDGV.Size = new System.Drawing.Size(554, 204);
            this.resDGV.TabIndex = 1;
            this.resDGV.VirtualMode = true;
            this.resDGV.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.resCellValidating);
            this.resDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.resCellValueNeeded);
            this.resDGV.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.resCellValuePushed);
            this.resDGV.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.resColumnHeaderMouseClick);
            this.resDGV.SelectionChanged += new System.EventHandler(this.resSelectionChanged);
            // 
            // resTagColumn
            // 
            this.resTagColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.resTagColumn.HeaderText = "Tag";
            this.resTagColumn.MinimumWidth = 50;
            this.resTagColumn.Name = "resTagColumn";
            this.resTagColumn.Width = 50;
            // 
            // resTIDColumn
            // 
            this.resTIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resTIDColumn.HeaderText = "Type ID";
            this.resTIDColumn.Name = "resTIDColumn";
            this.resTIDColumn.Width = 70;
            // 
            // resGIDColumn
            // 
            this.resGIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resGIDColumn.HeaderText = "Group ID";
            this.resGIDColumn.Name = "resGIDColumn";
            this.resGIDColumn.Width = 75;
            // 
            // resIIDColumn
            // 
            this.resIIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resIIDColumn.HeaderText = "Instance ID";
            this.resIIDColumn.Name = "resIIDColumn";
            this.resIIDColumn.Width = 87;
            // 
            // resCompColumn
            // 
            this.resCompColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resCompColumn.HeaderText = "Comp";
            this.resCompColumn.Name = "resCompColumn";
            this.resCompColumn.Width = 40;
            // 
            // resNameColumn
            // 
            this.resNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resNameColumn.HeaderText = "Name";
            this.resNameColumn.Name = "resNameColumn";
            // 
            // okBTN
            // 
            this.okBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBTN.Location = new System.Drawing.Point(416, 281);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(75, 23);
            this.okBTN.TabIndex = 1;
            this.okBTN.Text = "OK";
            this.okBTN.UseVisualStyleBackColor = true;
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBTN.Location = new System.Drawing.Point(497, 281);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(75, 23);
            this.cancelBTN.TabIndex = 2;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.Controls.Add(this.fnvClipBTN, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.fnv64BTN, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.fnv32BTN, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.copyRKsBTN, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pasteRKsBTN, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.compressAllCHK, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 231);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(554, 29);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // fnvClipBTN
            // 
            this.fnvClipBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fnvClipBTN.Location = new System.Drawing.Point(487, 3);
            this.fnvClipBTN.Name = "fnvClipBTN";
            this.fnvClipBTN.Size = new System.Drawing.Size(64, 23);
            this.fnvClipBTN.TabIndex = 0;
            this.fnvClipBTN.Text = "FNVCLIP";
            this.fnvClipBTN.UseVisualStyleBackColor = true;
            this.fnvClipBTN.Click += new System.EventHandler(this.fnvClip_Click);
            // 
            // fnv64BTN
            // 
            this.fnv64BTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fnv64BTN.Location = new System.Drawing.Point(417, 3);
            this.fnv64BTN.Name = "fnv64BTN";
            this.fnv64BTN.Size = new System.Drawing.Size(64, 23);
            this.fnv64BTN.TabIndex = 1;
            this.fnv64BTN.Text = "FNV64";
            this.fnv64BTN.UseVisualStyleBackColor = true;
            this.fnv64BTN.Click += new System.EventHandler(this.fnv64_Click);
            // 
            // fnv32BTN
            // 
            this.fnv32BTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fnv32BTN.Location = new System.Drawing.Point(347, 3);
            this.fnv32BTN.Name = "fnv32BTN";
            this.fnv32BTN.Size = new System.Drawing.Size(64, 23);
            this.fnv32BTN.TabIndex = 2;
            this.fnv32BTN.Text = "FNV32";
            this.fnv32BTN.UseVisualStyleBackColor = true;
            this.fnv32BTN.Click += new System.EventHandler(this.fnv32_Click);
            // 
            // copyRKsBTN
            // 
            this.copyRKsBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyRKsBTN.Location = new System.Drawing.Point(3, 3);
            this.copyRKsBTN.Name = "copyRKsBTN";
            this.copyRKsBTN.Size = new System.Drawing.Size(74, 23);
            this.copyRKsBTN.TabIndex = 3;
            this.copyRKsBTN.Text = "Copy RK(s)";
            this.copyRKsBTN.UseVisualStyleBackColor = true;
            this.copyRKsBTN.Click += new System.EventHandler(this.copyRKs_Click);
            // 
            // pasteRKsBTN
            // 
            this.pasteRKsBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pasteRKsBTN.Location = new System.Drawing.Point(83, 3);
            this.pasteRKsBTN.Name = "pasteRKsBTN";
            this.pasteRKsBTN.Size = new System.Drawing.Size(74, 23);
            this.pasteRKsBTN.TabIndex = 4;
            this.pasteRKsBTN.Text = "Paste RK(s)";
            this.pasteRKsBTN.UseVisualStyleBackColor = true;
            this.pasteRKsBTN.Click += new System.EventHandler(this.pasteRKs_Click);
            // 
            // compressAllCHK
            // 
            this.compressAllCHK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.compressAllCHK.AutoSize = true;
            this.compressAllCHK.Location = new System.Drawing.Point(252, 6);
            this.compressAllCHK.Name = "compressAllCHK";
            this.compressAllCHK.Size = new System.Drawing.Size(86, 17);
            this.compressAllCHK.TabIndex = 5;
            this.compressAllCHK.Text = "Compress All";
            this.compressAllCHK.ThreeState = true;
            this.compressAllCHK.UseVisualStyleBackColor = true;
            this.compressAllCHK.Click += new System.EventHandler(this.compressAll_Click);
            // 
            // SelectEditRIEDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 316);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SelectEditRIEDialog";
            this.Text = "Select and Edit Resource Index Entries";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resDGV)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label messageLBL;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.DataGridView resDGV;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.DataGridViewComboBoxColumn resTagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resTIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resGIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resIIDColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn resCompColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resNameColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button fnvClipBTN;
        private System.Windows.Forms.Button fnv64BTN;
        private System.Windows.Forms.Button fnv32BTN;
        private System.Windows.Forms.Button copyRKsBTN;
        private System.Windows.Forms.Button pasteRKsBTN;
        private System.Windows.Forms.CheckBox compressAllCHK;
    }
}