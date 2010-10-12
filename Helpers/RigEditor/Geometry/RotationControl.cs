using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class RotationControl : ValueControl
    {
        public EulerAngle Euler
        {
            get { return eulerControl.Value; }
            set
            {
                eulerControl.Value = value;
                quaternionControl.Value = new Quaternion(eulerControl.Value);
                angleAxisControl.Value = new AngleAxis(eulerControl.Value);
            }
        }
        public Quaternion Quaternion
        {
            get { return quaternionControl.Value; }
            set
            {
                quaternionControl.Value = value;
                eulerControl.Value = new EulerAngle(quaternionControl.Value);
                angleAxisControl.Value = new AngleAxis(quaternionControl.Value);
            }
        }
        public AngleAxis AngleAxis
        {
            get { return angleAxisControl.Value; }
            set
            {
                angleAxisControl.Value = value;
                quaternionControl.Value = new Quaternion(angleAxisControl.Value);
                eulerControl.Value = new EulerAngle(angleAxisControl.Value);
            }
        }
        public RotationControl()
        {
            InitializeComponent();
            quaternionControl.ValueChanged += new EventHandler(quaternionControl_Changed);
            angleAxisControl.ValueChanged += new EventHandler(angleAxisControl_Changed);
            eulerControl.ValueChanged += new EventHandler(eulerControl_Changed);
        }

        void eulerControl_Changed(object sender, EventArgs e)
        {
            quaternionControl.Value = new Quaternion(eulerControl.Value);
            angleAxisControl.Value= new AngleAxis(eulerControl.Value);
            OnChanged(this, new EventArgs());
        }

        void angleAxisControl_Changed(object sender, EventArgs e)
        {
            quaternionControl.Value = new Quaternion(angleAxisControl.Value);
            eulerControl.Value = new EulerAngle(angleAxisControl.Value);
            OnChanged(this, new EventArgs());
        }

        void quaternionControl_Changed(object sender, EventArgs e)
        {
            eulerControl.Value = new EulerAngle(quaternionControl.Value);
            angleAxisControl.Value = new AngleAxis(quaternionControl.Value);
            OnChanged(this, new EventArgs());
        }
    }
}
