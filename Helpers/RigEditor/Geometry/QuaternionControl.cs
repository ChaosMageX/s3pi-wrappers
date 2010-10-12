using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class QuaternionControl : ValueControl
    {
        public QuaternionControl()
        {
            InitializeComponent();
        }

        private Quaternion mValue;
        public Quaternion Value
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
            dbX.Value = mValue.X;
            dbY.Value = mValue.Y;
            dbZ.Value = mValue.Z;
            dbW.Value = mValue.W;
        }

        private void dbX_Validated(object sender, EventArgs e)
        {
            mValue.X = dbX.Value;
        }

        private void dbY_Validated(object sender, EventArgs e)
        {
            mValue.Y = dbY.Value;
        }

        private void dbZ_Validated(object sender, EventArgs e)
        {
            mValue.Z = dbZ.Value;
        }

        private void dbW_Validated(object sender, EventArgs e)
        {
            mValue.W = dbW.Value;
        }
        protected override void OnValidated(EventArgs e)
        {
            if(mValue.Magnitude() > 1d)
            {
                mValue.Normalize();
            }
            UpdateView();
            base.OnValidated(e);
            OnChanged(this, new EventArgs());
        }
    }
}
