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
    internal partial class GrannyTransformControl : ValueControl
    {
        public GrannyTransformControl()
        {
            InitializeComponent();
            positionControl.Changed += new EventHandler(positionControl_Changed);
            rotationControl.Changed += new EventHandler(orientationControl_Changed);
            scaleControl.Changed += new EventHandler(scaleControl_Changed);
        }

        void scaleControl_Changed(object sender, EventArgs e)
        {
            mValue.ScaleShearX.X = (float)scaleControl.Value.X;
            mValue.ScaleShearY.Y = (float)scaleControl.Value.Y;
            mValue.ScaleShearZ.Z = (float)scaleControl.Value.Z;
            OnChanged(this, new EventArgs());
        }

        void orientationControl_Changed(object sender, EventArgs e)
        {
            mValue.Orientation.X = (float)rotationControl.Quaternion.X;
            mValue.Orientation.Y = (float)rotationControl.Quaternion.Y;
            mValue.Orientation.Z = (float)rotationControl.Quaternion.Z;
            mValue.Orientation.W = (float)rotationControl.Quaternion.W;
            OnChanged(this, new EventArgs());
        }

        void positionControl_Changed(object sender, EventArgs e)
        {
            mValue.Position.X = (float)positionControl.Value.X;
            mValue.Position.Y = (float)positionControl.Value.Y;
            mValue.Position.Z = (float)positionControl.Value.Z;
            OnChanged(this, new EventArgs());
        }

        private Transform mValue;
        public Vector3 Position
        {
            get { return positionControl.Value; }
            set { positionControl.Value = value; }
        }
        public Quaternion Orientation
        {
            get { return rotationControl.Quaternion; }
            set { rotationControl.Quaternion = value; }
        }
        public Vector3 ScaleXYZ
        {
            get { return scaleControl.Value; }
            set { scaleControl.Value = value; }
        }
        public Transform Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                if (value != null) UpdateView();
            }
        }

        protected override void UpdateView()
        {
            Position = new Vector3(mValue.Position.X, mValue.Position.Y, mValue.Position.Z);
            Orientation = new Quaternion(mValue.Orientation.X, mValue.Orientation.Y, mValue.Orientation.Z, mValue.Orientation.W);
            ScaleXYZ = new Vector3(mValue.ScaleShearX.X,mValue.ScaleShearY.Y,mValue.ScaleShearZ.Z);
            cbPositionEnabled.Checked = (mValue.Flags & TransformFlags.Position) > 0;
            cbOrientationEnabled.Checked = cbPositionEnabled.Checked = (mValue.Flags & TransformFlags.Orientation) > 0;
            cbScaleEnabled.Checked = (mValue.Flags & TransformFlags.ScaleShear) > 0;
        }

        private void cbPositionEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPositionEnabled.Checked)
            {
                mValue.Flags |= TransformFlags.Position;
                positionControl.Enabled = true;
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.Position;
                mValue.Flags &= mask;
                positionControl.Enabled = false;
            }
        }

        private void cbOrientationEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOrientationEnabled.Checked)
            {
                mValue.Flags |= TransformFlags.Orientation;
                rotationControl.Enabled = true;
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.Orientation;
                mValue.Flags &= mask;
                rotationControl.Enabled = false;
            }

        }

        private void cbScaleEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbScaleEnabled.Checked)
            {
                mValue.Flags |= TransformFlags.ScaleShear;
                scaleControl.Enabled = true;
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.ScaleShear;
                mValue.Flags &= mask;
                scaleControl.Enabled = false;
            }

        }

    }
}
