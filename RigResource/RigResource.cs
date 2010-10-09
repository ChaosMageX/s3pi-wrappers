using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Settings;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    public class RigResource : AResource
    {
        public RigResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }
        private RigType mType;
        private AbstractRig mRig;



        [ElementPriority(1)]
        public RigType Type
        {
            get { return mType; }
            set
            {
                if (mType != value)
                {
                    mType = value;
                    mRig = AbstractRig.CreateRig(mType, 0, new EventHandler(OnResourceChanged), mRig.GrannyData);
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }
        [ElementPriority(2)]
        public AbstractRig Rig
        {
            get { return mRig; }
            set { mRig = value; OnResourceChanged(this, new EventArgs()); }
        }



        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            UInt32 type = br.ReadUInt32();
            mType = type == 0x8EAF13DE ? RigType.Body : RigType.Object;
            mRig = AbstractRig.CreateRig(mType, 0, new EventHandler(OnResourceChanged), s);
        }
        protected override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            if (mRig == null) mRig = AbstractRig.CreateRig(mType, 0, new EventHandler(OnResourceChanged));

            if (mType == RigType.Body)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(0x8EAF13DE);
            }
            mRig.UnParse(s);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }
        public override List<string> ContentFields
        {
            get
            {
                return GetContentFields(base.requestedApiVersion, GetType());
            }
        }
        public string Value
        {
            get
            {
                return string.Format("==={0} Rig===\n{1}\n",mType, mRig.Value);
            }
        }

        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;

    }
}
