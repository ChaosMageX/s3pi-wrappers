using System;
using System.Windows.Forms;
using s3pi.DemoPlugins;
using System.IO;

namespace s3piwrappers.BoneTool
{
    public partial class MainForm : Form,IRunHelper
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private byte[] mResult;
        private RigResource mRig;
        private const string kFormName= "Bone Tool(#)";
        public MainForm(Stream s)
        {
            InitializeComponent();
            mRig = new RigResource(0,s);
            mResult = mRig.AsBytes;
            var grd = mRig.Rig.GrannyData as WrappedGrannyData;
            if(grd == null)
            {
                throw new Exception("Could not read Granny2 data.  Ensure that the correct granny2.dll is installed.");
            }
            boneTreeView.Bones = grd.FileInfo.Skeleton.Bones;
            Text = kFormName.Replace("#", grd.FileInfo.Skeleton.Name);
        }
        private void btnCommit_Click(object sender, System.EventArgs e)
        {
            mResult = mRig.AsBytes;
            Environment.ExitCode = 0;
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        public byte[] Result
        {
            get { return mResult; }
        }
    }
}
