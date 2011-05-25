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
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Geometry;

namespace RigExport
{
    public partial class BonePicker : Form,IRunHelper
    {
        private byte[] mResult;
        private RigResource mRig;
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
            mRig = new RigResource(0, s);
            WrappedGrannyData grd = mRig.Rig.GrannyData as WrappedGrannyData;
            
            if (grd == null)
            {
                MessageBox.Show("Could not find Granny2 data.  Make sure you have installed the granny2.dll",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            var bones = grd.FileInfo.Skeleton.Bones;
            for (int i = 0; i < bones.Count; i++)
            {
                clbBones.Items.Add(bones[i]);
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
                var grd = (WrappedGrannyData)mRig.Rig.GrannyData;

                saveFileDialog1.FileName = "rigfile.txt";
                var result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < clbBones.Items.Count;i++)
                    {
                        if (clbBones.GetItemChecked(i))
                        {
                            var bone = (Bone) clbBones.Items[i];
                            bool flip = bone.ParentIndex == -1;
                            sb.AppendFormat("\"{0}\" \"{1}\" {2} {3} {4} {5}\r\n", cbHashBones.Checked? String.Format("0x{0:X8}",FNV32.GetHash(bone.Name)): bone.Name,
                                            bone.ParentIndex == -1
                                                ? "unparented"
                                                : cbHashBones.Checked? String.Format("0x{0:X8}",FNV32.GetHash(grd.FileInfo.Skeleton.Bones[bone.ParentIndex].Name)):grd.FileInfo.Skeleton.Bones[bone.ParentIndex].Name,
                                            bone.LocalTransform.Position.X,
                                            bone.LocalTransform.Position.Y,
                                            bone.LocalTransform.Position.Z,
                                            ToEuler(bone.LocalTransform.Orientation,flip));
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
        public static string ToEuler(Quad q,bool flip)
        {
            var matrix = new Matrix(new Quaternion(q.X,q.Y,q.Z,q.W));
            var euler = new EulerAngle(matrix);
            if (flip)
            {
                euler = new EulerAngle(0,0,Math.PI/2);
            }
            return String.Format("{0} {1} {2}",euler.Roll, euler.Yaw, euler.Pitch);

            

        }
    }
}
