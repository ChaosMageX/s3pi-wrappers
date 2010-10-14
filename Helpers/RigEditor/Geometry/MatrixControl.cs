using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal partial class MatrixControl : ValueControl
    {
        public MatrixControl()
        {
            InitializeComponent();
        }
        private Matrix mValue;

        public Matrix Value
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
            dbM11.Value = mValue.M00;
            dbM12.Value = mValue.M01;
            dbM13.Value = mValue.M02;
            dbM14.Value = mValue.M03;

            dbM21.Value = mValue.M10;
            dbM22.Value = mValue.M11;
            dbM23.Value = mValue.M12;
            dbM24.Value = mValue.M13;

            dbM31.Value = mValue.M20;
            dbM32.Value = mValue.M21;
            dbM33.Value = mValue.M22;
            dbM34.Value = mValue.M23;

            dbM41.Value = mValue.M30;
            dbM42.Value = mValue.M31;
            dbM43.Value = mValue.M32;
            dbM44.Value = mValue.M33;
        }

        private void dbM11_Validated(object sender, System.EventArgs e)
        {
            mValue.M00 = dbM11.Value;
        }

        private void dbM12_Validated(object sender, System.EventArgs e)
        {
            mValue.M01 = dbM12.Value;
        }

        private void dbM13_Validated(object sender, System.EventArgs e)
        {
            mValue.M02 = dbM13.Value;
        }

        private void dbM14_Validated(object sender, System.EventArgs e)
        {
            mValue.M03 = dbM14.Value;
        }

        private void dbM21_Validated(object sender, System.EventArgs e)
        {
            mValue.M10 = dbM21.Value;
        }

        private void dbM22_Validated(object sender, System.EventArgs e)
        {
            mValue.M11 = dbM22.Value;
        }

        private void dbM23_Validated(object sender, System.EventArgs e)
        {
            mValue.M12 = dbM23.Value;
        }

        private void dbM24_Validated(object sender, System.EventArgs e)
        {
            mValue.M13 = dbM24.Value;
        }

        private void dbM31_Validated(object sender, System.EventArgs e)
        {
            mValue.M20 = dbM31.Value;
        }

        private void dbM32_Validated(object sender, System.EventArgs e)
        {
            mValue.M21 = dbM32.Value;
        }

        private void dbM33_Validated(object sender, System.EventArgs e)
        {
            mValue.M22 = dbM33.Value;
        }

        private void dbM34_Validated(object sender, System.EventArgs e)
        {
            mValue.M23 = dbM34.Value;
        }

        private void dbM41_Validated(object sender, System.EventArgs e)
        {
            mValue.M30 = dbM41.Value;
        }

        private void dbM42_Validated(object sender, System.EventArgs e)
        {
            mValue.M31 = dbM42.Value;
        }

        private void dbM43_Validated(object sender, System.EventArgs e)
        {
            mValue.M32 = dbM43.Value;
        }

        private void dbM44_Validated(object sender, System.EventArgs e)
        {
            mValue.M33 = dbM44.Value;
        }
    }
}
