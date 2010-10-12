using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class EulerControl : ValueControl
    {
        public EulerControl()
        {
            InitializeComponent();
        }

        private EulerAngle mValue;

        public EulerAngle Value
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
            dbYaw.Value = mValue.Yaw;
            dbPitch.Value = mValue.Pitch;
            dbRoll.Value = mValue.Roll;
        }

        private void dbYaw_Validated(object sender, EventArgs e)
        {
            mValue.Yaw = dbYaw.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbPitch_Validated(object sender, EventArgs e)
        {
            mValue.Pitch = dbPitch.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbRoll_Validated(object sender, EventArgs e)
        {
            mValue.Roll = dbRoll.Value;
            OnChanged(this, new EventArgs());
        }
    }
}
