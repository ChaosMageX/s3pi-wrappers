using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.RigEditor.Common
{
    public partial class InfoDialog : Form
    {
        public InfoDialog()
        {
            InitializeComponent();
        }
        public InfoDialog(string caption,string info):this()
        {
            Text = caption;
            lbInfo.Text = info;
        }
        public static void Show(string caption,string info)
        {
            var d = new InfoDialog(caption, info);
            d.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
