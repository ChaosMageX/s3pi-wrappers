using System;
using s3pi.Interfaces;
using System.Text;
using System.Reflection;

namespace s3piwrappers.Granny2
{
    public class ArtToolInfo : GrannyElement
    {

        private String mFromArtToolName;
        private Int32 mArtToolMajorRevision;
        private Int32 mArtToolMinorRevision;
        private Single mUnitsPerMeter;
        private Triple mOrigin;
        private Triple mRightVector;
        private Triple mUpVector;
        private Triple mBackVector;

        public ArtToolInfo(int APIversion, EventHandler handler) : base(APIversion, handler)
        {
            var a = Assembly.GetEntryAssembly().GetName();
            mFromArtToolName = a.Name;
            mArtToolMajorRevision = a.Version.MajorRevision;
            mArtToolMinorRevision = a.Version.MinorRevision;
            mUnitsPerMeter = 1f;
            mArtToolMajorRevision = RecommendedApiVersion;
            mOrigin = new Triple(0, handler);
            mRightVector = new Triple(0, handler, 1, 0, 0);
            mUpVector = new Triple(0, handler, 0, 1, 0);
            mBackVector = new Triple(0, handler, 0, 0, 1);
            
        }

        public ArtToolInfo(int APIversion, EventHandler handler, string fromArtToolName, int artToolMajorRevision, int artToolMinorRevision, float unitsPerMeter, Triple origin, Triple rightVector, Triple upVector, Triple backVector)
            : base(APIversion, handler)
        {
            mFromArtToolName = fromArtToolName;
            mArtToolMajorRevision = artToolMajorRevision;
            mArtToolMinorRevision = artToolMinorRevision;
            mUnitsPerMeter = unitsPerMeter;
            mOrigin = new Triple(0, handler, origin);
            mRightVector = new Triple(0, handler, rightVector);
            mUpVector = new Triple(0, handler, upVector);
            mBackVector = new Triple(0, handler, backVector);
        }
        public ArtToolInfo(int APIversion, EventHandler handler, ArtToolInfo a)
            : this(APIversion, handler, a.mFromArtToolName, a.mArtToolMajorRevision, a.mArtToolMinorRevision, a.mUnitsPerMeter, a.mOrigin, a.mRightVector, a.mUpVector, a.mBackVector) { }
        internal ArtToolInfo(int APIversion, EventHandler handler, _ArtToolInfo a) : base(APIversion, handler) { FromStruct(a); }

        [ElementPriority(1)]
        public string FromArtToolName
        {
            get { return mFromArtToolName; }
            set { mFromArtToolName = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public int ArtToolMajorRevision
        {
            get { return mArtToolMajorRevision; }
            set { mArtToolMajorRevision = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public int ArtToolMinorRevision
        {
            get { return mArtToolMinorRevision; }
            set { mArtToolMinorRevision = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float UnitsPerMeter
        {
            get { return mUnitsPerMeter; }
            set { mUnitsPerMeter = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public Triple Origin
        {
            get { return mOrigin; }
            set { mOrigin = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public Triple RightVector
        {
            get { return mRightVector; }
            set { mRightVector = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public Triple UpVector
        {
            get { return mUpVector; }
            set { mUpVector = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public Triple BackVector
        {
            get { return mBackVector; }
            set { mBackVector = value; OnElementChanged(); }
        }
        internal void FromStruct(_ArtToolInfo data)
        {
            FromArtToolName = data.FromArtToolName;
            ArtToolMajorRevision = data.ArtToolMajorRevision;
            ArtToolMinorRevision = data.ArtToolMinorRevision;
            UnitsPerMeter = data.UnitsPerMeter;
            Origin = new Triple(0, handler, data.Origin);
            RightVector = new Triple(0, handler, data.RightVector);
            UpVector = new Triple(0, handler, data.UpVector);
            BackVector = new Triple(0, handler, data.BackVector);

        }

        internal _ArtToolInfo ToStruct()
        {
            var art = new _ArtToolInfo();
            art.FromArtToolName = FromArtToolName;
            art.ArtToolMajorRevision = ArtToolMajorRevision;
            art.ArtToolMinorRevision = ArtToolMinorRevision;
            art.UnitsPerMeter = UnitsPerMeter;
            art.Origin = Origin.ToStruct();
            art.RightVector = RightVector.ToStruct();
            art.UpVector = UpVector.ToStruct();
            art.BackVector = BackVector.ToStruct();
            return art;
        }
        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("FromArtToolName:\t{0}\n", mFromArtToolName);
                sb.AppendFormat("ArtToolMajorRevision:\t{0}\n", mArtToolMajorRevision);
                sb.AppendFormat("ArtToolMinorRevision:\t{0}\n", mArtToolMinorRevision);
                sb.AppendFormat("UnitsPerMeter:\t{0,8:0.00000}\n", mUnitsPerMeter);
                sb.AppendFormat("Origin:\t{0}\n", mOrigin);
                sb.AppendFormat("RightVector:\t{0}\n", mRightVector);
                sb.AppendFormat("UpVector:\t{0}\n", mUpVector);
                sb.AppendFormat("BackVector:\t{0}\n", mBackVector);
                return sb.ToString();
            }
        }
        public override string ToString()
        {
            return mFromArtToolName.ToString();
        }

    }
}