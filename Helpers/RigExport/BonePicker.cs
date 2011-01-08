using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers;
using System.IO;
using s3pi.Helpers;
using System.Globalization;
using System.Threading;
using s3piwrappers.Granny2;

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
                WrappedGrannyData grd = mRig.Rig.GrannyData as WrappedGrannyData;
                
                var result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < clbBones.Items.Count;i++)
                    {
                        if (clbBones.GetItemChecked(i))
                        {
                            var bone = (Bone) clbBones.Items[i];
                            sb.AppendFormat("\"{0}\" \"{1}\" {2} {3} {4} {5}\r\n", bone.Name,
                                            bone.ParentIndex == -1
                                                ? "unparented"
                                                : grd.FileInfo.Skeleton.Bones[bone.ParentIndex].Name,
                                            bone.LocalTransform.Position.X.ToString("0.00000",CultureInfo.InvariantCulture),
                                            bone.LocalTransform.Position.Y.ToString("0.00000", CultureInfo.InvariantCulture),
                                            bone.LocalTransform.Position.Z.ToString("0.00000", CultureInfo.InvariantCulture),
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
        public static string ToEuler(Quad q)
        {
            double sqX, sqY, sqZ, sqW;
            double x, y, z;
            sqX = Math.Pow(q.X, 2D);
            sqY = Math.Pow(q.Y, 2D);
            sqZ = Math.Pow(q.Z, 2D);
            sqW = Math.Pow(q.W, 2D);
            double poleTest = q.X * q.Y + q.Z * q.W;
            if (poleTest > 0.49999)
            {
                y = 2 * Math.Atan2(q.X, q.W);
                z = Math.PI / 2;
                x = 0;
            }
            else if (poleTest < -0.49999)
            {
                y = -2 * Math.Atan2(q.X, q.W);
                z = -Math.PI / 2;
                x = 0;
            }
            else
            {

                x = Math.Atan2(2D * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * sqX - 2 * sqZ);
                y = Math.Asin(2 * poleTest);
                z = Math.Atan2(2D * q.Y * q.W - 2 * q.X * q.Z, 1 - 2 * sqY - 2 * sqZ);
            }

            return string.Format("{0} {1} {2}", x.ToString("0.00000", CultureInfo.InvariantCulture), y.ToString("0.00000", CultureInfo.InvariantCulture), z.ToString("0.00000", CultureInfo.InvariantCulture));

        }
    }
}
