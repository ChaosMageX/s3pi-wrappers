using System.Windows.Forms;

namespace s3piwrappers.BoneTool
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
            this.tbInput.Validating += new System.ComponentModel.CancelEventHandler(this.tbInput_Validating);
            this.tbInput.Validated += new System.EventHandler(this.tbInput_Validated);
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
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // InputStringDialog
            // 
            this.ClientSize = new System.Drawing.Size(299, 72);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbInput);
            this.Name = "InputStringDialog";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputStringDialog_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                MessageBox.Show(this, "Field cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Close();
        }
        
        private void tbInput_Validated(object sender, System.EventArgs e)
        {
            mInputString = tbInput.Text;
        }
        private void tbInput_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                e.Cancel = true;
                MessageBox.Show(this, "Field cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void InputStringDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnOk_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}