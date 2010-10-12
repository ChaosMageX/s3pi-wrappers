using System;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
{
    internal partial class GrannyArtToolInfoControl : ValueControl
    {
        public GrannyArtToolInfoControl()
        {
            InitializeComponent();
            v3cOrigin.ValueChanged += new EventHandler(v3cOrigin_ValueChanged);
            v3cRightVector.ValueChanged += new EventHandler(v3cRightVector_ValueChanged);
            v3cBackVector.ValueChanged += new EventHandler(v3cBackVector_ValueChanged);
            v3cUpVector.ValueChanged += new EventHandler(v3cUpVector_ValueChanged);
        }


        private ArtToolInfo mValue;

        public ArtToolInfo Value
        {
            get { return mValue; }
            set { mValue = value; if(mValue!=null)UpdateView();}
        }
        protected override void UpdateView()
        {
            tbFromArtToolName.Text = mValue.FromArtToolName;
            tbArtToolMajorRevision.Value = mValue.ArtToolMajorRevision;
            tbArtToolMinorRevision.Value = mValue.ArtToolMinorRevision;
            tbUnitsPerMeter.Value = mValue.UnitsPerMeter;
            v3cOrigin.Value = new Vector3(mValue.Origin.X, mValue.Origin.Y, mValue.Origin.Z);
            v3cUpVector.Value = new Vector3(mValue.UpVector.X, mValue.UpVector.Y, mValue.UpVector.Z);
            v3cRightVector.Value = new Vector3(mValue.RightVector.X, mValue.RightVector.Y, mValue.RightVector.Z);
            v3cBackVector.Value = new Vector3(mValue.BackVector.X, mValue.BackVector.Y, mValue.BackVector.Z);

        }
        void tbFromArtToolName_TextChanged(object sender, EventArgs e)
        {
            mValue.FromArtToolName = tbFromArtToolName.Text;
        }

        void tbFromArtToolMajorRevision_Validated(object sender, EventArgs e)
        {
            mValue.ArtToolMajorRevision = tbArtToolMajorRevision.Value;
        }

        void tbFromArtToolMinorRevision_Validated(object sender, EventArgs e)
        {
            mValue.ArtToolMinorRevision = tbArtToolMinorRevision.Value;
        }

        void tbUnitsPerMeter_Validated(object sender, EventArgs e)
        {
            mValue.UnitsPerMeter = (float)tbUnitsPerMeter.Value;
        }
        void v3cUpVector_ValueChanged(object sender, EventArgs e)
        {
            mValue.UpVector.X = (float)v3cUpVector.Value.X;
            mValue.UpVector.Y = (float)v3cUpVector.Value.Y;
            mValue.UpVector.Z = (float)v3cUpVector.Value.Z;
        }

        void v3cBackVector_ValueChanged(object sender, EventArgs e)
        {
            mValue.BackVector.X = (float)v3cBackVector.Value.X;
            mValue.BackVector.Y = (float)v3cBackVector.Value.Y;
            mValue.BackVector.Z = (float)v3cBackVector.Value.Z;
        }

        void v3cRightVector_ValueChanged(object sender, EventArgs e)
        {
            mValue.RightVector.X = (float)v3cRightVector.Value.X;
            mValue.RightVector.Y = (float)v3cRightVector.Value.Y;
            mValue.RightVector.Z = (float)v3cRightVector.Value.Z;
        }

        void v3cOrigin_ValueChanged(object sender, EventArgs e)
        {
            mValue.Origin.X = (float)v3cOrigin.Value.X;
            mValue.Origin.Y = (float)v3cOrigin.Value.Y;
            mValue.Origin.Z = (float)v3cOrigin.Value.Z;
        }
    }
}
