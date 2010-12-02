using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    public class SkinControllerResource : AResource
    {
        public class BoneList : DependentList<Bone>
        {
            public BoneList(EventHandler handler) : base(handler) { }

            public BoneList(EventHandler handler, IEnumerable<Bone> ilt) : base(handler, ilt) {}

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override Bone CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, Bone element)
            {
                throw new NotImplementedException();
            }
        }
        public class Bone : AHandlerElement, IEquatable<Bone>
        {
            private string mName;
            private Matrix43 mInverseBindPose;
            public Bone(int APIversion, EventHandler handler, Bone basis)
                : this(APIversion, handler, basis.mName, new Matrix43(0,handler,basis.mInverseBindPose)) { }
            public Bone(int APIversion, EventHandler handler) : base(APIversion, handler) {mInverseBindPose=new Matrix43(0,handler); }
            public Bone(int APIversion, EventHandler handler, String name, Matrix43 inverseBindPose)
                : base(APIversion, handler)
            {
                mName = name;
                mInverseBindPose = inverseBindPose;
            }
            public string Value
            {
                get 
                { 
                    StringBuilder s = new StringBuilder();
                    s.AppendFormat("{0}\n", mName);
                    s.AppendLine(mInverseBindPose.Value);
                    return s.ToString();
                }
            }
            [ElementPriority(1)]
            public string Name
            {
                get { return mName; }
                set { if(mName!=value){mName = value; OnElementChanged();} }
            }

            [ElementPriority(2)]
            public Matrix43 InverseBindPose
            {
                get { return mInverseBindPose; }
                set { if(mInverseBindPose!=value){mInverseBindPose = value; OnElementChanged();} }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Bone(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Bone other)
            {
                return mName.Equals(other.mName);
            }
        }
        private UInt32 mVersion;
        private BoneList mBones;
        public SkinControllerResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                OnResourceChanged(this,new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }

        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                sb.AppendFormat("\nBones:\n");
                for (int i = 0; i < mBones.Count; i++) sb.AppendFormat("==Bone[{0}]==\n{1}\n", i, mBones[i].Value);
                return sb.ToString();
            }
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnResourceChanged(this, new EventArgs());} }
        }

        [ElementPriority(2)]
        public BoneList Bones
        {
            get { return mBones; }
            set { if(mBones!=value){mBones = value; OnResourceChanged(this, new EventArgs());} }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s, Encoding.BigEndianUnicode);
            List<string> names = new List<string>();
            List<Bone> bones = new List<Bone>();
            mVersion = br.ReadUInt32();
            int count1 = br.ReadInt32();
            for (int i = 0; i < count1; i++) names.Add(br.ReadString());
            int count2 = br.ReadInt32();
            if (checking && count2 != count1) 
                throw new Exception("Expected name count and matrix to be equal.");
            for (int i = 0; i < count2; i++)
            {
                bones.Add(new Bone(0, this.OnResourceChanged, names[i], new Matrix43(0, OnResourceChanged, s)));
            }
            mBones = new BoneList(OnResourceChanged,bones);
        }
        protected override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            if(mBones!=null)mBones=new BoneList(OnResourceChanged);
            bw.Write(mBones.Count);
            foreach (var entry in mBones) SevenBitString.Write(s, Encoding.BigEndianUnicode, entry.Name);
            bw.Write(mBones.Count);
            foreach (var entry in mBones) entry.InverseBindPose.UnParse(s);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}
