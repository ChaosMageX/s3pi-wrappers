using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Granny2;
using s3piwrappers.BoneTool.Geometry;

namespace s3piwrappers.BoneTool
{
    public partial class GrannyBoneControl : UserControl
    {
        public GrannyBoneControl()
        {
            InitializeComponent();
            Enabled = false;
            tbName.TextChanged += new EventHandler(tbName_TextChanged);
            tcLocalTransform.Changed += new EventHandler(tcLocalTransform_Changed);
        }
        private Bone mValue;
        public Bone Value
        {
            get { return mValue; }
            set 
            { 
                mValue = value; 
                if(mValue!=null) UpdateView();
            }
        }
        private void UpdateView()
        {
            Enabled = true;
            tbName.Text = mValue.Name;
            tcLocalTransform.Value = mValue.LocalTransform;

        }
        private void UpdateInverseWorld()
        {
            Matrix m = new Matrix(tcLocalTransform.Orientation);
            m.Translation = tcLocalTransform.Position;
            Matrix mi = m.GetInverse();
            Matrix4x4 g = mValue.InverseWorld4X4;
            g.M0.X = (float)mi.M11;
            g.M0.Y = (float)mi.M12;
            g.M0.Z = (float)mi.M13;
            g.M0.W = (float)mi.M14;

            g.M1.X = (float)mi.M21;
            g.M1.Y = (float)mi.M22;
            g.M1.Z = (float)mi.M23;
            g.M1.W = (float)mi.M24;

            g.M2.X = (float)mi.M31;
            g.M2.Y = (float)mi.M32;
            g.M2.Z = (float)mi.M33;
            g.M2.W = (float)mi.M34;

            g.M3.X = (float)mi.M41;
            g.M3.Y = (float)mi.M42;
            g.M3.Z = (float)mi.M43;
            g.M3.W = (float)mi.M44;
            
        }
        void tcLocalTransform_Changed(object sender, EventArgs e)
        {
            UpdateInverseWorld();
        }
        void tbName_TextChanged(object sender, EventArgs e)
        {
            mValue.Name = tbName.Text;
        }

    }
}
