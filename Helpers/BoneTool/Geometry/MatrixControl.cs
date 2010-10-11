using System.Windows.Forms;
using System;

namespace s3piwrappers.BoneTool.Geometry
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
            dbM11.Value = mValue.M11;
            dbM12.Value = mValue.M12;
            dbM13.Value = mValue.M13;
            dbM14.Value = mValue.M14;

            dbM21.Value = mValue.M21;
            dbM22.Value = mValue.M22;
            dbM23.Value = mValue.M23;
            dbM24.Value = mValue.M24;

            dbM31.Value = mValue.M31;
            dbM32.Value = mValue.M32;
            dbM33.Value = mValue.M33;
            dbM34.Value = mValue.M34;

            dbM41.Value = mValue.M41;
            dbM42.Value = mValue.M42;
            dbM43.Value = mValue.M43;
            dbM44.Value = mValue.M44;
        }

        private void dbM11_Validated(object sender, System.EventArgs e)
        {
            mValue.M11 = dbM11.Value;
        }

        private void dbM12_Validated(object sender, System.EventArgs e)
        {
            mValue.M12 = dbM12.Value;
        }

        private void dbM13_Validated(object sender, System.EventArgs e)
        {
            mValue.M13 = dbM13.Value;
        }

        private void dbM14_Validated(object sender, System.EventArgs e)
        {
            mValue.M14 = dbM14.Value;
        }

        private void dbM21_Validated(object sender, System.EventArgs e)
        {
            mValue.M21 = dbM21.Value;
        }

        private void dbM22_Validated(object sender, System.EventArgs e)
        {
            mValue.M22 = dbM22.Value;
        }

        private void dbM23_Validated(object sender, System.EventArgs e)
        {
            mValue.M23 = dbM23.Value;
        }

        private void dbM24_Validated(object sender, System.EventArgs e)
        {
            mValue.M24 = dbM24.Value;
        }

        private void dbM31_Validated(object sender, System.EventArgs e)
        {
            mValue.M31 = dbM31.Value;
        }

        private void dbM32_Validated(object sender, System.EventArgs e)
        {
            mValue.M32 = dbM32.Value;
        }

        private void dbM33_Validated(object sender, System.EventArgs e)
        {
            mValue.M33 = dbM33.Value;
        }

        private void dbM34_Validated(object sender, System.EventArgs e)
        {
            mValue.M34 = dbM34.Value;
        }

        private void dbM41_Validated(object sender, System.EventArgs e)
        {
            mValue.M41 = dbM41.Value;
        }

        private void dbM42_Validated(object sender, System.EventArgs e)
        {
            mValue.M42 = dbM42.Value;
        }

        private void dbM43_Validated(object sender, System.EventArgs e)
        {
            mValue.M43 = dbM43.Value;
        }

        private void dbM44_Validated(object sender, System.EventArgs e)
        {
            mValue.M44 = dbM44.Value;
        }
    }
}
