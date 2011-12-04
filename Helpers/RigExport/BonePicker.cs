using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using s3piwrappers.RigEditor.Geometry;

namespace RigExport
{
    public partial class BonePicker : Form
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
        private readonly RigResource.RigResource mRig;
        public BonePicker()
        {
            InitializeComponent();
        }

        public BonePicker(Stream s)
            : this()
        {
            s.Position = 0L;
            mRig = new RigResource.RigResource(0, s);
            var bones = mRig.Bones;

            clbBones.ItemCheck += new ItemCheckEventHandler(clbBones_ItemCheck);
            for (int i = 0; i < bones.Count; i++)
            {
                var b = bones[i];
                clbBones.Items.Add(new BoneListItem(b));
                if (!b.Name.Contains("_slot") && !b.Name.Contains("_compress"))
                {
                    clbBones.SetItemChecked(i,true);
                }
            }
            
           

        }

        void clbBones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int total = clbBones.CheckedItems.Count;
            if(e.NewValue == CheckState.Checked)
            {
                total += 1;
            }else
            {
                total -= 1;
            }

            lbBonesSelectedCount.Text = total.ToString();
        }

        
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                var grd = mRig;

                saveFileDialog1.FileName = "rigfile.txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
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
                            sb.AppendFormat("\"{0}\" \"{1}\" {2} {3:F10} {4:F10} {5:F10}\r\n", cbHashBones.Checked? String.Format("0x{0:X8}",FNV32.GetHash(bone.Name)): bone.Name,
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
            return String.Format("{0:F10} {1:F10} {2:F10}",euler.Roll, euler.Yaw, euler.Pitch);

            

        }
    }
}
