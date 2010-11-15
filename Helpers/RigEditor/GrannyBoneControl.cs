using System;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;
using s3piwrappers.RigEditor.Geometry;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannyBoneControl : ValueControl
    {
        public GrannyBoneControl()
        {
            InitializeComponent();
            tbName.TextChanged += new EventHandler(tbName_TextChanged);
            tcLocalTransform.ValueChanged += new EventHandler(tcLocalTransform_Changed);
            Visible = false;
        }
        private Bone mValue;
        public Bone Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                if (mValue != null) UpdateView();
            }
        }
        protected override void UpdateView()
        {
            Enabled = true;
            tbName.Text = mValue.Name;
            tcLocalTransform.Value = mValue.LocalTransform;
            tbHashedName.Text = String.Format("0x{0:X8}", FNV32.GetHash(mValue.Name));

        }
        private void UpdateInverseWorld()
        {
            var q = new Quaternion(mValue.LocalTransform.Orientation.X, mValue.LocalTransform.Orientation.Y, mValue.LocalTransform.Orientation.Z, mValue.LocalTransform.Orientation.W);
            var p = new Vector3(mValue.LocalTransform.Position.X, mValue.LocalTransform.Position.Y, mValue.LocalTransform.Position.Z);
            var s = new Vector3(mValue.LocalTransform.ScaleShearX.X, mValue.LocalTransform.ScaleShearY.Y,
                                mValue.LocalTransform.ScaleShearZ.Z);
            Matrix mi = Matrix.CreateTransformMatrix(q, s, p).GetInverse();
            Matrix4x4 g = mValue.InverseWorld4X4;
            g.M0.X = (float)mi.M00;
            g.M0.Y = (float)mi.M10;
            g.M0.Z = (float)mi.M20;
            g.M0.W = (float)mi.M30;

            g.M1.X = (float)mi.M01;
            g.M1.Y = (float)mi.M11;
            g.M1.Z = (float)mi.M21;
            g.M1.W = (float)mi.M31;

            g.M2.X = (float)mi.M02;
            g.M2.Y = (float)mi.M12;
            g.M2.Z = (float)mi.M22;
            g.M2.W = (float)mi.M32;

            g.M3.X = (float)mi.M03;
            g.M3.Y = (float)mi.M13;
            g.M3.Z = (float)mi.M23;
            g.M3.W = (float)mi.M33;

        }
        void tcLocalTransform_Changed(object sender, EventArgs e)
        {
            UpdateInverseWorld();
            OnChanged(this, new EventArgs());
        }
        void tbName_TextChanged(object sender, EventArgs e)
        {
            mValue.Name = tbName.Text;
            tbHashedName.Text = String.Format("0x{0:X8}", FNV32.GetHash(tbName.Text));
            OnChanged(this, new EventArgs());
        }

    }
}
