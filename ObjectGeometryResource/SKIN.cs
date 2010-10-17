using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;

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
        }
        public class Bone : AHandlerElement, IEquatable<Bone>
        {
            private UInt32 mNameHash;
            private Transformation mTransformation;
            public Bone(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mTransformation = new Transformation(0, handler);
            }
            public Bone(int APIversion, EventHandler handler, UInt32 Bone, Transformation transformation)
                : base(APIversion, handler)
            {
                mNameHash = Bone;
                mTransformation = new Transformation(0,handler,transformation);
            }
            public Bone(int APIversion, EventHandler handler, Bone j)
                : this(APIversion, handler, j.mNameHash, j.mTransformation)
            {
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("BoneNameHash:\t0x{0:X8}\r\n", mNameHash);
                    sb.AppendFormat("Transformation:\r\n{0}", mTransformation.Value);
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
            public Transformation Transformation
            {
                get { return mTransformation; }
                set { mTransformation = value; OnElementChanged(); }
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
        [DataGridExpandable(true)]
        public class Vector3 : AHandlerElement
        {
            private float mX, mY,mZ;
            public Vector3(int APIversion, EventHandler handler, float x, float y, float z) : base(APIversion, handler)
            {
                mX = x;
                mY = y;
                mZ = z;
            }

            public Vector3(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public Vector3(int APIversion, EventHandler handler,Vector3 basis) : this(APIversion, handler,basis.mX,basis.mY,basis.mZ) { }
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value;OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value;OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public override string ToString()
            {
                return String.Format("[{0:0.000000},{1:0.000000},{2:0.000000}]", X, Y, Z);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Vector3(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class Transformation : AHandlerElement
        {

            private Vector3 mRight, mUp, mBack, mTranslate;

            public Transformation(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
                mBack = new Vector3(0,handler,0,0,1);
                mRight = new Vector3(0, handler, 1, 0, 0);
                mTranslate = new Vector3(0, handler, 0, 0, 0);
                mUp = new Vector3(0, handler, 0, 1, 0);
            }
            public Transformation(int APIversion, EventHandler handler,Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }

            public Transformation(int APIversion, EventHandler handler, Vector3 back, Vector3 right, Vector3 translate, Vector3 up) : base(APIversion, handler)
            {
                mBack = new Vector3(0,handler,back);
                mRight = new Vector3(0,handler,right);
                mTranslate = new Vector3(0,handler,translate);
                mUp = new Vector3(0,handler,up);
            }
            public Transformation(int APIversion, EventHandler handler,Transformation basis)
                : this(APIversion, handler,basis.mBack,basis.mRight,basis.mTranslate,basis.mUp)
            {
            }
            [ElementPriority(1)]
            public Vector3 Right
            {
                get { return mRight; }
                set { mRight = value;OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Vector3 Up
            {
                get { return mUp; }
                set { mUp = value;OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Vector3 Back
            {
                get { return mBack; }
                set { mBack = value;OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Vector3 Translate
            {
                get { return mTranslate; }
                set { mTranslate = value;OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                float m00, m01, m02, m03;
                float m10, m11, m12, m13;
                float m20, m21, m22, m23;
                var br = new BinaryReader(s);
                m00 = br.ReadSingle();
                m01 = br.ReadSingle();
                m02 = br.ReadSingle();
                m03 = br.ReadSingle();

                m10 = br.ReadSingle();
                m11 = br.ReadSingle();
                m12 = br.ReadSingle();
                m13 = br.ReadSingle();

                m20 = br.ReadSingle();
                m21 = br.ReadSingle();
                m22 = br.ReadSingle();
                m23 = br.ReadSingle();

                mRight=new Vector3(0,handler,m00,m01,m02);
                mUp = new Vector3(0, handler, m10, m11, m12);
                mBack = new Vector3(0, handler, m20, m21, m22);
                mTranslate = new Vector3(0, handler, m03, m13, m23);
            }
            public void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                bw.Write(mRight.X);
                bw.Write(mRight.Y);
                bw.Write(mRight.Z);
                bw.Write(mTranslate.X);

                bw.Write(mUp.X);
                bw.Write(mUp.Y);
                bw.Write(mUp.Z);
                bw.Write(mTranslate.Y);

                bw.Write(mBack.X);
                bw.Write(mBack.Y);
                bw.Write(mBack.Z);
                bw.Write(mTranslate.Z);
            }
            public string Value
            {
                get { return String.Format("{0}\r\n{1}\r\n{2}\r\n{3}",Right,Up,Back,Translate); }
            }
            public override string ToString()
            {
                return String.Format("{0},{1},{2},{3}", Right, Up, Back, Translate);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Transformation(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
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
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
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

                Transformation transformation = new Transformation(0, handler, s);
                mBones.Add(new Bone(0, handler, names[i], transformation));
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
            foreach (var j in mBones) j.Transformation.UnParse(s);
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