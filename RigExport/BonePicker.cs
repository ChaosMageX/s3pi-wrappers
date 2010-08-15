using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers;
using System.IO;
using s3pi.DemoPlugins;

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
            GrannyRigData grd = mRig.RigData as GrannyRigData;
            
            if (grd == null)
            {
                MessageBox.Show("Could not find Granny2 data.  Make sure you have installed the granny2.dll",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            var bones = grd.Skeleton.Bones;
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

                GrannyRigData grd = mRig.RigData as GrannyRigData;
                
                var result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < clbBones.Items.Count;i++)
                    {
                        if (clbBones.GetItemChecked(i))
                        {
                            var bone = (GrannyRigData.Bone) clbBones.Items[i];
                            sb.AppendFormat("\"{0}\" \"{1}\" {2,8:0.000000} {3,8:0.000000} {4,8:0.000000} {5}\n", bone.Name,
                                            bone.ParentIndex == -1
                                                ? "unparented"
                                                : grd.Skeleton.Bones[bone.ParentIndex].Name,
                                            bone.LocalTransform.Position.X,
                                            bone.LocalTransform.Position.Y, bone.LocalTransform.Position.Z,
                                            ToEuler(bone.LocalTransform.Orientation));
                        }
                    }
                    using (var s = new StreamWriter(File.Create(saveFileDialog1.FileName))) s.Write(sb.ToString());
                }
            }
            catch
            {
                throw;
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
        public static string ToEuler(GrannyRigData.Quad q)
        {
            double sqX, sqY, sqZ, sqW;
            double x, y, z;
            sqX = Math.Pow(q.X, 2D);
            sqY = Math.Pow(q.Y, 2D);
            sqZ = Math.Pow(q.Z, 2D);
            sqW = Math.Pow(q.W, 2D);
            double poleTest = q.X * q.Y + q.Z * q.W;
            x = Math.Atan2(2D * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * sqX - 2 * sqZ);
            y = Math.Asin(2 * poleTest);
            z = Math.Atan2(2D * q.Y * q.W - 2 * q.X * q.Z, 1 - 2 * sqY - 2 * sqZ);
            return string.Format("{0,8:0.000000} {1,8:0.000000} {2,8:0.000000}", x, y, z);

        }
    }
}
