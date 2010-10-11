using System;
using System.Windows.Forms;
using s3pi.DemoPlugins;
using System.IO;
using s3piwrappers.BoneTool.Geometry;
using s3piwrappers.Granny2;

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
            grannySkeletonControl1.Value = grd.FileInfo.Skeleton;

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
