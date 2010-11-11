using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace LightResource
{
    [DataGridExpandable(true)]
    public class Colour3f : AHandlerElement
    {
        private float mRed, mGreen, mBlue;
        public Colour3f(int APIversion, EventHandler handler, float red, float green, float blue)
            : base(APIversion, handler)
        {
            mRed = red;
            mGreen = green;
            mBlue = blue;
        }

        public Colour3f(int APIversion, EventHandler handler) : base(APIversion, handler) { }
        public Colour3f(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }
        public Colour3f(int APIversion, EventHandler handler, Colour3f basis) : this(APIversion, handler, basis.mRed, basis.mGreen, basis.mBlue) { }
        private void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            mRed = br.ReadSingle();
            mGreen = br.ReadSingle();
            mBlue = br.ReadSingle();
        }
        public void UnParse(Stream s)
        {
            var bw = new BinaryWriter(s);
            bw.Write(mRed);
            bw.Write(mGreen);
            bw.Write(mBlue);
        }
        [ElementPriority(1)]
        public float Red
        {
            get { return mRed; }
            set { if(mRed!=value){mRed = value; OnElementChanged();} }
        }
        [ElementPriority(2)]
        public float Green
        {
            get { return mGreen; }
            set { if(mGreen!=value){mGreen = value; OnElementChanged();} }
        }
        [ElementPriority(3)]
        public float Blue
        {
            get { return mBlue; }
            set { if(mBlue!=value){mBlue = value; OnElementChanged();} }
        }

        public override string ToString()
        {
            return String.Format("[R:{0:0.000000},G:{1:0.000000},B:{2:0.000000}]", mRed, mGreen, mBlue);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new Colour3f(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}