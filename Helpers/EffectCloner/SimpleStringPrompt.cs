using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.EffectCloner
{
    public partial class SimpleStringPrompt : Form
    {
        public SimpleStringPrompt(string title, string defaultValue = "")
        {
            this.InitializeComponent();
            this.Text = title;
            this.inputTXT.Text = defaultValue;
        }

        private void inputTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.inputTXT.Text))
                this.btnOK.Enabled = false;
            else
                this.btnOK.Enabled = true;
        }

        public string Result
        {
            get { return this.inputTXT.Text; }
        }
    }
}
