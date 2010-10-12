using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class AngleAxisControl : ValueControl
    {
        public AngleAxisControl()
        {
            InitializeComponent();
        }

        private AngleAxis mValue;

        public AngleAxis Value
        {
            get { return mValue; }
            set 
            { 
                mValue = value;
                UpdateView();
            }
        }

        protected override void UpdateView()
        {
            dbX.Value = mValue.Axis.X;
            dbY.Value = mValue.Axis.Y;
            dbZ.Value = mValue.Axis.Z;
            dbAngle.Value = mValue.Angle;
        }

        private void dbX_Validated(object sender, EventArgs e)
        {
            mValue.Axis.X = dbX.Value;
        }

        private void dbY_Validated(object sender, EventArgs e)
        {
            mValue.Axis.Y = dbY.Value;
        }

        private void dbZ_Validated(object sender, EventArgs e)
        {
            mValue.Axis.Z = dbZ.Value;
        }

        private void dbAngle_Validated(object sender, EventArgs e)
        {
            mValue.Angle = dbAngle.Value;
        }
        protected override void OnValidated(EventArgs e)
        {
            if(mValue.Axis.Magnitude() > 1)
            {
                mValue.Axis.Normalize();
            }
            UpdateView();
            OnChanged(this, new EventArgs());
        }
    }
}

