using System;
using System.IO;
using s3pi.Interfaces;
using System.Collections.Generic;
namespace s3piwrappers
{
    public class RawRigData : RigData
    {
        private byte[] mData;

        public RawRigData(int APIversion, EventHandler handler) : base(APIversion, handler)
        {
        }

        public RawRigData(int APIversion, EventHandler handler, RawRigData basis)
            : base(APIversion, handler, basis)
        {
        }

        public RawRigData(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s)
        {
        }

        protected override void Parse(Stream s)
        {
            mData = new byte[s.Length];
            s.Read(mData, 0, mData.Length);
        }

        public override Stream UnParse()
        {
            Stream ms = new MemoryStream();
            ms.Write(mData,0,mData.Length);
            return ms;
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new RawRigData(0, handler, this);
        }

        public override System.Collections.Generic.List<string> ContentFields
        {
            get { return GetContentFields(base.requestedApiVersion,GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }

        public override string Value
        {
            get { return String.Format("Raw RigData[0x{0:X8}]",mData.Length); }
        }
    }
}