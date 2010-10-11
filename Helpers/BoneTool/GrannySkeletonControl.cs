using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool
{
    internal partial class GrannySkeletonControl : UserControl
    {
        public GrannySkeletonControl()
        {
            InitializeComponent();
            mBoneManager = new BoneManager();
            boneTreeView.BoneManager = mBoneManager;
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

        void boneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                var b = e.Node.Tag as Bone;
                grannyBoneControl.Value = b;
                grannyBoneControl.Enabled = true;
            }
            else
            {
                grannyBoneControl.Enabled = false;
            }
        }
    }
}
