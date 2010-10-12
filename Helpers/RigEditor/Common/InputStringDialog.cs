using System.Windows.Forms;

namespace s3piwrappers.RigEditor.Common
{
    public class InputStringDialog : Form
    {
        public InputStringDialog()
        {
            InitializeComponent();
        }

        public InputStringDialog(string caption):this()
        {
            Text = caption;
        }
        public InputStringDialog(string caption,string initText)
            : this(caption)
        {
            tbInput.Text = initText;
        }
        private TextBox tbInput;
        private Button btnOk;
        private Button btnCancel;
        private string mInputString;
        public string InputString
        {
            get { return mInputString; }
        }


        private void InitializeComponent()
        {
            this.tbInput = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(12, 12);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(275, 20);
            this.tbInput.TabIndex = 0;
            this.tbInput.TextChanged += new System.EventHandler(this.tbInput_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(212, 38);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(131, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InputStringDialog
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(299, 72);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbInput);
            this.Name = "InputStringDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void tbInput_TextChanged(object sender, System.EventArgs e)
        {
            mInputString = tbInput.Text;
        }

    }
}