using System;
using System.ComponentModel;
using System.Windows.Forms;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannyTransformControl : ValueControl
    {
        [Browsable(true)]
        public string Title
        {
            get { return gbMain.Text; }
            set { gbMain.Text = value; }
        }
        public GrannyTransformControl()
        {
            InitializeComponent();
            positionControl.ValueChanged += new EventHandler(positionControl_Changed);
            rotationControl.ValueChanged += new EventHandler(rotationControl_Changed);
            scaleControl.ValueChanged += new EventHandler(scaleControl_Changed);
        }

        void scaleControl_Changed(object sender, EventArgs e)
        {
            mValue.ScaleShearX.X = (float)scaleControl.Value.X;
            mValue.ScaleShearY.Y = (float)scaleControl.Value.Y;
            mValue.ScaleShearZ.Z = (float)scaleControl.Value.Z;
            OnChanged(this, new EventArgs());
        }

        void rotationControl_Changed(object sender, EventArgs e)
        {
            var quat = new Quaternion(rotationControl.Value);
            mValue.Orientation.X = (float)quat.X;
            mValue.Orientation.Y = (float)quat.Y;
            mValue.Orientation.Z = (float)quat.Z;
            mValue.Orientation.W = (float)quat.W;
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
        public EulerAngle Rotation
        {
            get { return rotationControl.Value; }
            set { rotationControl.Value = value; }
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
            Rotation = new EulerAngle(new Quaternion(mValue.Orientation.X, mValue.Orientation.Y, mValue.Orientation.Z, mValue.Orientation.W));
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
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.Position;
                mValue.Flags &= mask;
            }
        }

        private void cbRotationEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOrientationEnabled.Checked)
            {
                mValue.Flags |= TransformFlags.Orientation;
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.Orientation;
                mValue.Flags &= mask;
            }

        }

        private void cbScaleEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbScaleEnabled.Checked)
            {
                mValue.Flags |= TransformFlags.ScaleShear;
            }
            else
            {
                var mask = (TransformFlags)0xFFFFFFFF ^ TransformFlags.ScaleShear;
                mValue.Flags &= mask;
            }

        }

        private void llbRotation_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            MatrixInputDialog d = new MatrixInputDialog(new Matrix(new Quaternion(Value.Orientation.X,Value.Orientation.Y,Value.Orientation.Z,Value.Orientation.W)));
            DialogResult result = d.ShowDialog(this);
            if(result == DialogResult.OK)
            {
                Quaternion q = new Quaternion(d.Value);
                this.Value.Orientation.X = (float)q.X;
                this.Value.Orientation.Y = (float)q.Y;
                this.Value.Orientation.Z = (float)q.Z;
                this.Value.Orientation.W = (float)q.W;
                this.UpdateView();
            }
        }

    }
}
