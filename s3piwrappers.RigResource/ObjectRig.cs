using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class ObjectRig : AbstractRig
    {
        public ObjectRig(int APIversion, EventHandler handler) : base(APIversion, handler) {}
        public ObjectRig(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) {}
        public ObjectRig(int APIversion, EventHandler handler, GrannyData grannyData) : base(APIversion, handler, grannyData) {}

        protected override void Parse(Stream s)
        {
            s.Position = 0L;
            mGrannyData = GrannyData.CreateInstance(0, handler, s);
        }

        public override void UnParse(Stream s)
        {
            var gr2 = mGrannyData.UnParse();
            var buffer = new byte[gr2.Length];
            gr2.Read(buffer, 0, buffer.Length);
            s.Write(buffer,0,buffer.Length);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new ObjectRig(0, handler, mGrannyData);
        }
    }
}
