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
            OnChanged(this, new EventArgs());
        }

        private void dbY_Validated(object sender, EventArgs e)
        {
            mValue.Y = dbY.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbZ_Validated(object sender, EventArgs e)
        {
            mValue.Z = dbZ.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbW_Validated(object sender, EventArgs e)
        {
            mValue.W = dbW.Value;
            OnChanged(this, new EventArgs());
        }
    }
}
