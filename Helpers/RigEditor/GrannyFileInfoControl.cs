using System;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannyFileInfoControl : ValueControl
    {
        public GrannyFileInfoControl()
        {
            InitializeComponent();
        }

        private GrannyFileInfo mValue;

        public GrannyFileInfo Value
        {
            get { return mValue; }
            set { mValue = value;if(mValue!=null)UpdateView(); }
        }
        protected override void UpdateView()
        {
            grannySkeletonControl1.Value = mValue.Skeleton;
            grannyModelControl1.Value = mValue.Model;
            grannyArtToolInfoControl1.Value = mValue.ArtToolInfo;
            tbFromFileName.Text = mValue.FromFileName;
        }
        private void tbFromFileName_TextChanged(object sender, EventArgs e)
        {
            mValue.FromFileName = tbFromFileName.Text;
        }
    }
}
