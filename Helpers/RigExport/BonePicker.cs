using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using s3piwrappers;
using System.IO;
using s3pi.Helpers;
using System.Globalization;
using System.Threading;
using s3piwrappers.RigEditor.Geometry;

namespace RigExport
{
    public partial class BonePicker : Form,IRunHelper
    {
        class BoneListItem
        {
            public BoneListItem(RigResource.RigResource.Bone bone)
            {
                this.Bone = bone;
            }
            public RigResource.RigResource.Bone Bone { get; private set; }
            public override string ToString()
            {
                return Bone.Name;
            }
        }
        private byte[] mResult;
        private RigResource.RigResource mRig;
        public BonePicker()
        {
            InitializeComponent();
        }

        public BonePicker(Stream s)
            : this()
        {
            mResult = new byte[s.Length];
            s.Read(mResult, 0, mResult.Length);
            s.Position = 0L;
            mRig = new RigResource.RigResource(0, s);
            var bones = mRig.Bones;
            for (int i = 0; i < bones.Count; i++)
            {
                var b = bones[i];
                clbBones.Items.Add(b);
                if (!b.Name.Contains("_slot") && !b.Name.Contains("_compress"))
                {
                    clbBones.SetItemChecked(i,true);
                }
            }
           

        }

        public byte[] Result
        {
            get { return mResult; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                var grd = mRig;

                saveFileDialog1.FileName = "rigfile.txt";
                var result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < clbBones.Items.Count;i++)
                    {
                        if (clbBones.GetItemChecked(i))
                        {
                            var bone = ((BoneListItem) clbBones.Items[i]).Bone;
                            bool flip = bone.ParentBoneIndex == -1;
                            sb.AppendFormat("\"{0}\" \"{1}\" {2} {3} {4} {5}\r\n", cbHashBones.Checked? String.Format("0x{0:X8}",FNV32.GetHash(bone.Name)): bone.Name,
                                            bone.ParentBoneIndex == -1
                                                ? "unparented"
                                                : cbHashBones.Checked ? String.Format("0x{0:X8}", FNV32.GetHash(grd.Bones[(int)bone.ParentBoneIndex].Name)) : grd.Bones[(int)bone.ParentBoneIndex].Name,
                                            bone.Position.X,
                                            bone.Position.Y,
                                            bone.Position.Z,
                                            ToEuler(bone.Orientation,flip));
                        }
                    }
                    using (var s = new StreamWriter(File.Create(saveFileDialog1.FileName))) s.Write(sb.ToString());
                }
            }
            finally
            {
                Close();
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBones.Items.Count; i++)
            {
                clbBones.SetItemChecked(i,true);
            }
        }

        private void btnSelNone_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < clbBones.Items.Count; i++)
            {
                clbBones.SetItemChecked(i, false);
            }
        }
        public static string ToEuler(s3pi.Interfaces.Quaternion q,bool flip)
        {
            var matrix = new Matrix(new Quaternion(q.A,q.B,q.C,q.D));
            var euler = new EulerAngle(matrix);
            if (flip)
            {
                euler = new EulerAngle(0,0,Math.PI/2);
            }
            return String.Format("{0} {1} {2}",euler.Roll, euler.Yaw, euler.Pitch);

            

        }
    }
}
