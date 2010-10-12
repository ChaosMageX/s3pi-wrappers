using System.Windows.Forms;

namespace s3piwrappers.RigEditor.Common
{
    public class FindAndReplaceDialog : Form
    {
        public FindAndReplaceDialog()
        {
            InitializeComponent();
        }

        public FindAndReplaceDialog(string caption)
            : this()
        {
            Text = caption;
        }
        private string mFindString;
        private TextBox tbFind;
        private TextBox tbReplace;
        private Label lbFind;
        private Label lbReplace;
        private Button btnOk;
        private Button btnCancel;
        private string mReplaceString;
        public string FindString
        {
            get { return mFindString; }
        }

        public string ReplaceString
        {
            get { return mReplaceString; }
        }

        private void InitializeComponent()
        {
            this.tbFind = new System.Windows.Forms.TextBox();
            this.tbReplace = new System.Windows.Forms.TextBox();
            this.lbFind = new System.Windows.Forms.Label();
            this.lbReplace = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFind
            // 
            this.tbFind.Location = new System.Drawing.Point(73, 12);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(214, 20);
            this.tbFind.TabIndex = 0;
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            // 
            // tbReplace
            // 
            this.tbReplace.Location = new System.Drawing.Point(73, 39);
            this.tbReplace.Name = "tbReplace";
            this.tbReplace.Size = new System.Drawing.Size(214, 20);
            this.tbReplace.TabIndex = 1;
            this.tbReplace.TextChanged += new System.EventHandler(this.tbReplace_TextChanged);
            // 
            // lbFind
            // 
            this.lbFind.AutoSize = true;
            this.lbFind.Location = new System.Drawing.Point(33, 15);
            this.lbFind.Name = "lbFind";
            this.lbFind.Size = new System.Drawing.Size(30, 13);
            this.lbFind.TabIndex = 2;
            this.lbFind.Text = "Find:";
            // 
            // lbReplace
            // 
            this.lbReplace.AutoSize = true;
            this.lbReplace.Location = new System.Drawing.Point(13, 42);
            this.lbReplace.Name = "lbReplace";
            this.lbReplace.Size = new System.Drawing.Size(50, 13);
            this.lbReplace.TabIndex = 3;
            this.lbReplace.Text = "Replace:";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(212, 65);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(131, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FindAndReplaceDialog
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(299, 100);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lbReplace);
            this.Controls.Add(this.lbFind);
            this.Controls.Add(this.tbReplace);
            this.Controls.Add(this.tbFind);
            this.Name = "FindAndReplaceDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void tbFind_TextChanged(object sender, System.EventArgs e)
        {
            mFindString = tbFind.Text;
        }

        private void tbReplace_TextChanged(object sender, System.EventArgs e)
        {
            mReplaceString = tbReplace.Text;

        }
    }
}