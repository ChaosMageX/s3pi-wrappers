using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.BoneTool
{
    internal class ValueControl : UserControl
    {
        public event EventHandler Changed;
        protected virtual void UpdateView(){}
        protected virtual void OnChanged(object sender,EventArgs e)
        {
            if(Changed!=null)
                Changed(sender, e);
        }
    }
}
