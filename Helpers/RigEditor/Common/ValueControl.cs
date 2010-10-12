using System;
using System.Windows.Forms;

namespace s3piwrappers.RigEditor.Common
{
    internal class ValueControl : UserControl
    {
        public event EventHandler ValueChanged;
        protected virtual void UpdateView(){}
        protected virtual void OnChanged(object sender,EventArgs e)
        {
            if(ValueChanged!=null)
                ValueChanged(sender, e);
        }
    }
}
