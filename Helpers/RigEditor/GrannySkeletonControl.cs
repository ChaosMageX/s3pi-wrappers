using System;
using System.Windows.Forms;
using s3piwrappers.Granny2;
using System.Text;
using System.Collections.Generic;
using s3piwrappers.RigEditor.Geometry;
using System.Security.Cryptography;
using s3piwrappers.RigEditor.Common;

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
            llbMatrixInfo.Visible = false;
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

        private Matrix GetAbsoluteTransform(Bone b)
        {
            List<Matrix> transforms = new List<Matrix>();
            while (b != null)
            {
                var q = b.LocalTransform.Orientation;
                var p = b.LocalTransform.Position;
                Matrix t = new Matrix(new Quaternion(q.X, q.Y, q.Z, q.W));
                t.Translation = new Vector3(p.X, p.Y, p.Z);
                transforms.Add(t);
                b = BoneManager.GetParent(b);
            }
            transforms.Reverse();
            Matrix absolute = transforms[0];
            transforms.RemoveAt(0);
            while (transforms.Count > 0)
            {
                Matrix m = transforms[0];
                transforms.RemoveAt(0);
                absolute = absolute * m;
            }
            return absolute;
        }
        private void ShowMatrixInfo()
        {
            var sb = new StringBuilder();
            foreach (var b in BoneManager.Bones)
            {
                GetInfo(b, sb);
            }
            InfoDialog.Show("Matrix Info", sb.ToString());
        }
        private void GetInfo(Bone bone, StringBuilder sb)
        {
            var t = GetAbsoluteTransform(bone);
            var ti = t.GetInverse();
            sb.AppendLine("============================");
            sb.AppendFormat("Name:\t{0}\r\n", bone.Name);
            sb.AppendFormat("Hashed:\t0x{0:X8}\r\n", FNV32.GetHash(bone.Name));
            sb.AppendLine("Absolute Transform (RSLT):");
            sb.AppendLine(t.ToString());
            sb.AppendLine("Absolute Transform Inverse (SKIN):");
            sb.AppendLine(ti.ToString());


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

        private void llbMatrixInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //ShowMatrixInfo();
        }
    }
}
