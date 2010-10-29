using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace s3piwrappers
{
    public class SKIN : ARCOLBlock
    {
        public class BoneList : AResource.DependentList<Bone>
        {
            public BoneList(EventHandler handler)
                : base(handler)
            {
            }

            public BoneList(EventHandler handler, IList<Bone> ilt) : base(handler, ilt) {}

            public override void Add()
            {
                base.Add(new object[] { });
            }

            #region Unused
            protected override Bone CreateElement(Stream s)
            {
                throw new NotSupportedException();
            }

            protected override void WriteElement(Stream s, Bone element)
            {
                throw new NotSupportedException();
            } 
            #endregion
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
                mInverseBindPose = new Matrix43(0,handler,matrix43);
            }
            public Bone(int APIversion, EventHandler handler, Bone j)
                : this(APIversion, handler, j.mNameHash, j.mInverseBindPose)
            {
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name:\t0x{0:X8}\r\n", mNameHash);
                    sb.AppendFormat("InverseBindPose:\r\n{0}", mInverseBindPose.Value);
                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public uint NameHash
            {
                get { return mNameHash; }
                set { mNameHash = value; OnElementChanged(); }
            }

            [ElementPriority(2)]
            public Matrix43 InverseBindPose
            {
                get { return mInverseBindPose; }
                set { mInverseBindPose = value; OnElementChanged(); }
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
                return mNameHash.Equals(other.mNameHash);
            }
        }
        
        
        public SKIN(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 0x00000001;
            mBones = new BoneList(handler);
        }
        public SKIN(int APIversion, EventHandler handler, SKIN basis)
            : base(APIversion, handler, null)
        {
            mVersion = basis.mVersion;
            mBones = new BoneList(handler,basis.Bones.Select(x=> new Bone(0,handler,x)).ToList());
        }
        public SKIN(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnRCOLChanged(this, new EventArgs()); }
        }

        [ElementPriority(2)]
        public BoneList Bones
        {
            get { return mBones; }
            set { mBones = value; OnRCOLChanged(this,new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n",mVersion);
                if(mBones.Count >0)
                {
                    sb.AppendFormat("Bones:\n");
                    for (int i = 0; i < mBones.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]\n{1}\n", i, mBones[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private UInt32 mVersion;
        private BoneList mBones;
        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mBones = new BoneList(handler);
            int count = br.ReadInt32();
            uint[] names = new uint[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = br.ReadUInt32();
            }

            for (int i = 0; i < count; i++)
            {

                Matrix43 matrix43 = new Matrix43(0, handler, s);
                mBones.Add(new Bone(0, handler, names[i], matrix43));
            }
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

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}