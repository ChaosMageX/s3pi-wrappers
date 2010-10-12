using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class Vector3Control : ValueControl
    {
        public Vector3Control()
        {
            InitializeComponent();
        }

        private Vector3 mValue;
        public Vector3 Value
        {
            get { return mValue; }
            set 
            {
                mValue = value;
                UpdateView();
                OnChanged(this, new EventArgs());
            }
        }

        protected override void UpdateView()
        {
            dbX.Value = mValue.X;
            dbY.Value = mValue.Y;
            dbZ.Value = mValue.Z;
        }

        private void dbX_Validated(object sender, EventArgs e)
        {
            mValue.X = dbX.Value;
            OnChanged(this,new EventArgs());
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
    }
}
