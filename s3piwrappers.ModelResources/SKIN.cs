using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using s3pi.Settings;

namespace s3piwrappers
{
    public class SKIN : ARCOLBlock
    {
        public class BoneList : DependentList<Bone>
        {
            public BoneList(EventHandler handler) : base(handler) { }
            public BoneList(EventHandler handler, IEnumerable<Bone> ilt) : base(handler, ilt) { }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override Bone CreateElement(Stream s)
            {
                throw new NotSupportedException();
            }

            protected override void WriteElement(Stream s, Bone element)
            {
                throw new NotSupportedException();
            }
            public Bone this[UInt32 nameHash]
            {
                get { return this.FirstOrDefault(x=>x.NameHash.Equals(nameHash)); }
            }

        }
        public class Bone : AHandlerElement, IEquatable<Bone>
        {
            private UInt32 mNameHash;
            private Matrix43 mInverseBindPose;
            public Bone(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mInverseBindPose = new Matrix43(0, handler);
            }
            public Bone(int APIversion, EventHandler handler, UInt32 Bone, Matrix43 matrix43)
                : base(APIversion, handler)
            {
                mNameHash = Bone;
                mInverseBindPose = new Matrix43(0, handler, matrix43);
            }
            public Bone(int APIversion, EventHandler handler, Bone j)
                : this(APIversion, handler, j.mNameHash, j.mInverseBindPose)
            {
            }
            public string Value
            {
                get
                {
                    return ValueBuilder;
                    /*
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name:\t0x{0:X8}\r\n", mNameHash);
                    sb.AppendFormat("InverseBindPose:\r\n{0}", mInverseBindPose.Value);
                    return sb.ToString();
                    /**/
                }
            }
            [ElementPriority(1)]
            public uint NameHash
            {
                get { return mNameHash; }
                set { if(mNameHash!=value){mNameHash = value; OnElementChanged();} }
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
                return mNameHash.Equals(other.mNameHash)
                    && mInverseBindPose.Equals(other.mInverseBindPose);
            }
            public override bool Equals(object obj)
            {
                return obj as Bone != null ? this.Equals(obj as Bone) : false;
            }
            public override int GetHashCode()
            {
                return mNameHash.GetHashCode()
                    ^ mInverseBindPose.GetHashCode();
            }

        }

        private UInt32 mVersion;
        private BoneList mBones;

        public SKIN(int APIversion, EventHandler handler): this(APIversion, handler, new BoneList(handler),0x00000001){}
        public SKIN(int APIversion, EventHandler handler, SKIN basis) : this(APIversion, handler, new BoneList(handler, basis.Bones), basis.Version) { }
        public SKIN(int APIversion, EventHandler handler, Stream s): base(APIversion, handler, s){}
        public SKIN(int APIversion, EventHandler handler, BoneList bones, uint version) : base(APIversion, handler, null)
        {
            mBones = bones;
            mVersion = version;
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} }
        }

        [ElementPriority(2)]
        public BoneList Bones
        {
            get { return mBones; }
            set { if(mBones!=value){mBones = value; OnRCOLChanged(this, new EventArgs());} }
        }

        public string Value
        {
            get
            {
                return ValueBuilder;
                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                if (mBones.Count > 0)
                {
                    sb.AppendFormat("Bones:\n");
                    for (int i = 0; i < mBones.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]\n{1}\n", i, mBones[i].Value);
                    }
                }
                return sb.ToString();
                /**/
            }
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (Settings.Checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mBones = new BoneList(handler);
            int count = br.ReadInt32();
            uint[] names = new uint[count];
            for (int i = 0; i < count; i++)names[i] = br.ReadUInt32();
            for (int i = 0; i < count; i++)mBones.Add(new Bone(0, handler, names[i], new Matrix43(0, handler, s)));
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            if (mBones == null) mBones = new BoneList(handler);
            bw.Write(mBones.Count);
            foreach (var j in mBones) bw.Write(j.NameHash);
            foreach (var j in mBones) j.InverseBindPose.UnParse(s);
            return s;
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new SKIN(0, handler, this);
        }

        public override string Tag
        {
            get { return "SKIN"; }
        }

        public override uint ResourceType
        {
            get { return 0x01D0E76B; }
        }

        private const int kRecommendedApiVersion = 1;
    }
}