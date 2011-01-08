using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.RigEditor.Geometry
{
    public partial class MatrixInputDialog : Form
    {
        private Matrix mValue;
        public MatrixInputDialog():this(Matrix.Identity)
        {
        }
        public MatrixInputDialog(Matrix matrix)
        {
            mValue = matrix;
            InitializeComponent();
            SetFields();
        }
        private void SetFields()
        {
            dbM00.Value = mValue.M00;
            dbM01.Value = mValue.M01;
            dbM02.Value = mValue.M02;

            dbM10.Value = mValue.M10;
            dbM11.Value = mValue.M11;
            dbM12.Value = mValue.M12;

            dbM20.Value = mValue.M20;
            dbM21.Value = mValue.M21;
            dbM22.Value = mValue.M22;
        }
        public Matrix Value
        {
            get { return mValue; }
        }

        private void dbM00_Validated(object sender, EventArgs e)
        {
            mValue.M00 = dbM00.Value;
        }

        private void dbM01_Validated(object sender, EventArgs e)
        {
            mValue.M01 = dbM01.Value;
        }

        private void dbM02_Validated(object sender, EventArgs e)
        {
            mValue.M02 = dbM02.Value;
        }


        private void dbM10_Validated(object sender, EventArgs e)
        {
            mValue.M10 = dbM10.Value;
        }

        private void dbM11_Validated(object sender, EventArgs e)
        {
            mValue.M11 = dbM11.Value;
        }

        private void dbM12_Validated(object sender, EventArgs e)
        {
            mValue.M12 = dbM12.Value;
        }


        private void dbM20_Validated(object sender, EventArgs e)
        {
            mValue.M20 = dbM20.Value;
        }

        private void dbM21_Validated(object sender, EventArgs e)
        {
            mValue.M21 = dbM21.Value;
        }

        private void dbM22_Validated(object sender, EventArgs e)
        {
            mValue.M22 = dbM22.Value;
        }


    }
}
