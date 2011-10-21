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
        public const RigType EAxoidRigType = (RigType)(-1);

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
        private EAxoidRig mEaRig;

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
            set { if (mRig != value) { mRig = value; OnResourceChanged(this, new EventArgs()); } }
        }
        [ElementPriority(3)]
        public EAxoidRig EaRig
        {
            get { return mEaRig; }
            set { if (mEaRig != value) { mEaRig = value; OnResourceChanged(this, new EventArgs()); } }
        }
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            UInt32 type = br.ReadUInt32();
            if (type < 0x0100)
            {
                mType = EAxoidRigType;
                mEaRig = new EAxoidRig(0, new EventHandler(OnResourceChanged), s);
            }
            else
            {
                mType = type == 0x8EAF13DE ? RigType.Body : RigType.Object;
                mRig = AbstractRig.CreateRig(mType, 0, new EventHandler(OnResourceChanged), s);
            }
        }
        protected override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            if (mType == EAxoidRigType)
            {
                if (mEaRig == null) mEaRig = new EAxoidRig(0, new EventHandler(OnResourceChanged));
                mEaRig.UnParse(s);
            }
            else
            {
                if (mRig == null) mRig = AbstractRig.CreateRig(mType, 0, new EventHandler(OnResourceChanged));
                if (mType == RigType.Body)
                {
                    BinaryWriter bw = new BinaryWriter(s);
                    bw.Write(0x8EAF13DE);
                }
                mRig.UnParse(s);
            }
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
                List<string> results = GetContentFields(base.requestedApiVersion, GetType());
                if (mType == EAxoidRigType)
                {
                    results.Remove("Type");
                    results.Remove("Rig");
                }
                else
                {
                    results.Remove("EaRig");
                }
                return results;
            }
        }
        public string Value
        {
            get
            {
                if (mType == EAxoidRigType)
                    return string.Format("===EAxoid Rig===\n{0}\n", mEaRig.Value);
                else
                    return string.Format("==={0} Rig===\n{1}\n",mType, mRig.Value); 
            }
        }

        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;

    }
}
