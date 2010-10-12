using System;
using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannySkeletonControl : UserControl
    {
        public GrannySkeletonControl()
        {
            InitializeComponent();
            mBoneManager = new BoneManager();
            boneTreeView.BoneManager = mBoneManager;
            grannyBoneControl.ValueChanged += new EventHandler(grannyBoneControl_Changed);
        }


        private Skeleton mValue;
        private BoneManager mBoneManager;
        public Skeleton Value
        {
            get { return mValue; }
            set { mValue = value;if(mValue!=null)UpdateView(); }
        }

        public BoneManager BoneManager
        {
            get { return mBoneManager; }
        }

        private void UpdateView()
        {
            mBoneManager.Bones = mValue.Bones;
            tbName.Text = mValue.Name;
        }
        private void tbName_TextChanged(object sender, EventArgs e)
        {
            mValue.Name = tbName.Text;
        }
        void grannyBoneControl_Changed(object sender, EventArgs e)
        {
            mBoneManager.UpdateBone(grannyBoneControl.Value);
        }

        void boneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                var b = e.Node.Tag as Bone;
                grannyBoneControl.Value = b;
                grannyBoneControl.Visible = true;
            }
            else
            {
                grannyBoneControl.Visible = false;
            }
        }
    }
}
