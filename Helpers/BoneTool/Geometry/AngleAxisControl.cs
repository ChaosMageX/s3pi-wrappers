using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.BoneTool.Geometry;

namespace s3piwrappers.BoneTool
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
            OnChanged(this, new EventArgs());
        }

        private void dbY_Validated(object sender, EventArgs e)
        {
            mValue.Axis.Y = dbY.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbZ_Validated(object sender, EventArgs e)
        {
            mValue.Axis.Z = dbZ.Value;
            OnChanged(this, new EventArgs());
        }

        private void dbAngle_Validated(object sender, EventArgs e)
        {
            mValue.Angle = dbAngle.Value;
            OnChanged(this, new EventArgs());
        }
    }
}

