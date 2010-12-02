using System;
using System.Windows.Forms;
using System.IO;
using s3pi.Helpers;

namespace s3piwrappers.RigEditor
{
    public partial class MainForm : Form,IRunHelper
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private byte[] mResult;
        private readonly RigResource mRig;
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
            this.grannySkeletonControl1.Value = grd.FileInfo.Skeleton;
            
        }


        private void btnCommit_Click(object sender, System.EventArgs e)
        {
            var grd = mRig.Rig.GrannyData as WrappedGrannyData;
            if (!grd.FileInfo.Skeleton.Name.Equals(grd.FileInfo.Model.Name)) //sync names since model name is no longer displayed
                grd.FileInfo.Model.Name = grd.FileInfo.Skeleton.Name;
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
