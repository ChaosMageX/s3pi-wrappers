using System;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannyModelControl : ValueControl
    {
        public GrannyModelControl()
        {
            InitializeComponent();
        }

        private Model mValue;

        public Model Value
        {
            get { return mValue; }
            set { mValue = value;if(mValue!=null)UpdateView(); }
        }

        protected override void UpdateView()
        {
            tbName.Text = mValue.Name;
            tcInitialPlacement.Value = mValue.InitialPlacement;
        }
        private void tbName_TextChanged(object sender, EventArgs e)
        {
            mValue.Name = tbName.Text;
        }
    }
}
