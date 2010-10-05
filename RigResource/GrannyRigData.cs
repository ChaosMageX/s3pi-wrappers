using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using s3pi.Interfaces;
using s3piwrappers.Granny2;

namespace s3piwrappers
{
    public class GrannyRigData : RigData
    {
        public GrannyRigData(int APIversion, EventHandler handler) 
            : base(APIversion, handler)
        {
            mGrannyFileInfo = new GrannyFileInfo(0,handler);
        }
        public GrannyRigData(int APIversion, EventHandler handler, GrannyRigData basis) : this(APIversion, handler,basis.mGrannyFileInfo) { }
        public GrannyRigData(int APIversion, EventHandler handler, GrannyFileInfo grannyFileInfo) : base(APIversion, handler)
        {
            mGrannyFileInfo = new GrannyFileInfo(0, handler, grannyFileInfo);
        }
        public GrannyRigData(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

        private GrannyFileInfo mGrannyFileInfo;

        [ElementPriority(1)]
        public GrannyFileInfo FileInfo
        {
            get { return mGrannyFileInfo; }
            set { mGrannyFileInfo = value; OnElementChanged(); }
        }
        public override string Value
        {
            get { return mGrannyFileInfo.Value; }
        }

        protected override void Parse(Stream s)
        {
            mGrannyFileInfo = new GrannyFileInfo(0, handler, IO.LoadFile(0, handler, s));
        }
        public override Stream UnParse()
        {
            string tempPath = Path.GetTempFileName();
            IO.SaveFile(mGrannyFileInfo, tempPath, CompressionType.Oodle1);
            byte[] buffer;
            using (var s = File.OpenRead(tempPath))
            {
                buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
            }
            File.Delete(tempPath);
            return new MemoryStream(buffer);
        }


        public override AHandlerElement Clone(EventHandler handler)
        {
            throw new NotImplementedException();
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