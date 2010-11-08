using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class MaterialDefinition : ARCOLBlock
    {
        private UInt32 mVersion;
        private UInt32 mName;
        private UInt32 mShaderName;
        private MTNF mMaterialBlock;
        private bool mIsVideoSurface;
        private bool mIsPaintingSurface;
        public MATD(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) {}

        public override AHandlerElement Clone(EventHandler handler)
        {
            throw new NotImplementedException();
        }

        protected override void Parse(Stream s)
        {
            throw new NotImplementedException();
        }

        public override Stream UnParse()
        {
            throw new NotImplementedException();
        }

        public override uint ResourceType
        {
            get { return 0x01D0E75D; }
        }

        public override string Tag
        {
            get { return "MATD"; }
        }

    }
}
