using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.FreeformJazz
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        public Splash(string largeMessage)
            : this()
        {
            this.largeMessageLBL.Text = largeMessage;
            this.smallMessageLBL.Text = "";
        }

        public string LargeMessage
        {
            get { return this.largeMessageLBL.Text; }
            set
            {
                this.largeMessageLBL.Text = value;
                this.smallMessageLBL.Text = "";
            }
        }

        public string SmallMessage
        {
            get { return this.smallMessageLBL.Text; }
            set { this.smallMessageLBL.Text = value; }
        }
    }
}
