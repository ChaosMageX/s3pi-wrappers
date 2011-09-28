using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class EAxoidRig : AHandlerElement
    {
        private static Encoding sEncoding = Encoding.ASCII;

        public static string ReadPascalString(BinaryReader r)
        {
            int len = (int)r.ReadUInt32();
            byte[] buffer = new byte[len];
            char[] str = new char[len];
            r.Read(buffer, 0, len);
            sEncoding.GetChars(buffer, 0, len, str, 0);
            return new string(str);
        }

        public static void WritePascalString(BinaryWriter w, string str)
        {
            int len = str.Length;
            w.Write((uint)len);
            byte[] buffer = new byte[len];
            sEncoding.GetBytes(str, 0, len, buffer, 0);
            w.Write(buffer, 0, len);
        }

        public class Bone : AHandlerElement, IEquatable<Bone>
        {
            public Bone(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mPosition = new Granny2.Triple(apiVersion, handler);
                mOrientation = new Granny2.Quad(apiVersion, handler);
                mScale = new Granny2.Triple(apiVersion, handler);
                mName = "<New Bone>";
                mInt01 = 0;
                mParentIndex = -1;
                mNameHash = System.Security.Cryptography.FNV32.GetHash(mName);
                mInt02 = 0x23;
            }

            public Bone(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                this.Parse(s);
            }

            private const int kBufferSize = 60;// + mBoneName.Length

            private Granny2.Triple mPosition;
            private Granny2.Quad mOrientation;
            private Granny2.Triple mScale;
            private string mName;
            private UInt32 mInt01;
            private Int32 mParentIndex;
            private UInt32 mNameHash;
            private UInt32 mInt02;

            [ElementPriority(8)]
            public uint Int02
            {
                get { return mInt02; }
                set { if (mInt02 != value) { mInt02 = value; OnElementChanged(); } }
            }

            [ElementPriority(7)]
            public uint NameHash
            {
                get { return mNameHash; }
                set
                { 
                    if (mNameHash != value) { mNameHash = value; OnElementChanged(); } }
            }

            [ElementPriority(6)]
            public int ParentIndex
            {
                get { return mParentIndex; }
                set { if (mParentIndex != value) { mParentIndex = value; OnElementChanged(); } }
            }

            [ElementPriority(5)]
            public uint Int01
            {
                get { return mInt01; }
                set { if (mInt01 != value) { mInt01 = value; OnElementChanged(); } }
            }

            [ElementPriority(4)]
            public string Name
            {
                get { return mName; }
                set 
                { 
                    if (mName != value) 
                    { 
                        mName = value;
                        mNameHash = System.Security.Cryptography.FNV32.GetHash(value);
                        OnElementChanged(); 
                    } 
                }
            }

            [ElementPriority(3)]
            public Granny2.Triple Scale
            {
                get { return mScale; }
                set { if (!mScale.Equals(value)) { mScale = value; OnElementChanged(); } }
            }

            [ElementPriority(2)]
            public Granny2.Quad Orientation
            {
                get { return mOrientation; }
                set { if (!mOrientation.Equals(value)) { mOrientation = value; OnElementChanged(); } }
            }

            [ElementPriority(1)]
            public Granny2.Triple Position
            {
                get { return mPosition; }
                set { if (!mPosition.Equals(value)) { mPosition = value; OnElementChanged(); } }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                MemoryStream ms = new MemoryStream(kBufferSize + mName.Length);
                this.UnParse(ms);
                return new Bone(requestedApiVersion, handler, ms);
            }

            public override List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(base.requestedApiVersion, typeof(Bone)); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public string Value
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("Name:\t{0}\n", mName);

                    builder.AppendFormat("Position:\n{0}\n", mPosition);
                    builder.AppendFormat("Orientation:\n{0}\n", mOrientation);
                    builder.AppendFormat("Scale:\n{0}\n", mScale);
                    
                    builder.AppendFormat("Int 01:\t0x{0:X8}\t({0})\n", mInt01);
                    builder.AppendFormat("Parent Index:\t{0}\n", mParentIndex);
                    builder.AppendFormat("Name Hash:\t0x{0:X8}\n", mNameHash);
                    builder.AppendFormat("Int 02:\t0x{0:X8}\t({0})\n", mInt02);
                    return builder.ToString();
                }
            }

            private void Parse(Stream s)
            {
                BinaryReader r = new BinaryReader(s);
                mPosition = new Granny2.Triple(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mOrientation = new Granny2.Quad(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mScale = new Granny2.Triple(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mName = ReadPascalString(r);
                mInt01 = r.ReadUInt32();
                mParentIndex = r.ReadInt32();
                mNameHash = r.ReadUInt32();
                mInt02 = r.ReadUInt32();
            }

            public void UnParse(Stream s)
            {
                BinaryWriter w = new BinaryWriter(s);
                w.Write(mPosition.X);
                w.Write(mPosition.Y);
                w.Write(mPosition.Z);
                w.Write(mOrientation.X);
                w.Write(mOrientation.Y);
                w.Write(mOrientation.Z);
                w.Write(mOrientation.W);
                w.Write(mScale.X);
                w.Write(mScale.Y);
                w.Write(mScale.Z);
                WritePascalString(w, mName);
                w.Write(mInt01);
                w.Write(mParentIndex);
                w.Write(mNameHash);
                w.Write(mInt02);
            }

            public override string ToString()
            {
                return mName;
            }

            public bool Equals(Bone other)
            {
                return
                    this.mPosition.Equals(other.mPosition) &&
                    this.mOrientation.Equals(other.mOrientation) &&
                    this.mScale.Equals(other.mScale) &&
                    this.mName == other.mName &&
                    this.mInt01 == other.mInt01 &&
                    this.mParentIndex == other.mParentIndex &&
                    this.mNameHash == other.mNameHash &&
                    this.mInt02 == other.mInt02;
            }
        }

        public class BoneList : DependentList<Bone>
        {
            public BoneList(EventHandler handler, long size = -1L)
                : base(handler, size) { }

            public BoneList(EventHandler handler, IEnumerable<Bone> ilt, long size = -1L)
                : base(handler, ilt, size) { }

            public BoneList(EventHandler handler, Stream s, long size = -1L)
                : base(handler, s, size) { }

            public override void Add()
            {
                base.Add(new Bone(0, base.elementHandler));
            }

            protected override Bone CreateElement(Stream s)
            {
                return new Bone(0, base.elementHandler, s);
            }

            protected override void WriteElement(Stream s, Bone element)
            {
                element.UnParse(s);
            }
        }

        public EAxoidRig(int apiVersion, EventHandler handler)
            : base(apiVersion, handler) { }

        public EAxoidRig(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler)
        {
            Parse(s);
        }

        private UInt32 mInt01;
        private UInt32 mInt02;
        private BoneList mBones;
        private string mName;
        private SimpleList<UInt32> mUnknown;

        [ElementPriority(5)]
        public SimpleList<UInt32> Unknown
        {
            get { return mUnknown; }
            set { mUnknown = value; OnElementChanged(); }
        }

        [ElementPriority(4)]
        public string Name
        {
            get { return mName; }
            set { mName = value; OnElementChanged(); }
        }

        [ElementPriority(3)]
        public BoneList Bones
        {
            get { return mBones; }
            set { mBones = value; OnElementChanged(); }
        }

        [ElementPriority(2)]
        public uint Int02
        {
            get { return mInt02; }
            set { mInt02 = value; OnElementChanged(); }
        }

        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }

        public string Value
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("Int01:\t{0}\nInt02:\t{1}\n", mInt01, mInt02);
                builder.AppendFormat("Name:\t{0}\n", mName);
                builder.Append("Bones:\n");
                for (int i = 0; i < mBones.Count; i++)
                {
                    builder.AppendFormat("==Bone[{0}]==\n{1}\n", i, mBones[i].Value);
                }
                builder.Append("Unknown:\n");
                int lineCount = mUnknown.Count / 4;
                int j, k, index = 0;
                for (j = 0; j < lineCount; j++)
                {
                    for (k = 0; k < 4; k++)
                    {
                        builder.AppendFormat("{0:X8}\t", mUnknown[index]);
                        index++;
                    }
                    builder.Append("\n");
                }
                for (; index < mUnknown.Count; index++)
                {
                    builder.AppendFormat("{0:X8}\t", mUnknown[index]);
                }
                return builder.ToString();
            }
        }

        private void Parse(Stream s)
        {
            s.Position = 0L;
            BinaryReader r = new BinaryReader(s);
            mInt01 = r.ReadUInt32();
            mInt02 = r.ReadUInt32();
            mBones = new BoneList(handler, s);
            mName = ReadPascalString(r);
            long unkCount = (s.Length - s.Position) / 4L;
            mUnknown = new SimpleList<uint>(handler);
            for (int i = 0; i < unkCount; i++)
            {
                mUnknown.Add(r.ReadUInt32());
            }
        }

        public void UnParse(Stream s)
        {
            BinaryWriter w = new BinaryWriter(s);
            w.Write(mInt01);
            w.Write(mInt02);
            mBones.UnParse(s);
            WritePascalString(w, mName);
            int unkCount = mUnknown.Count;
            for (int i = 0; i < unkCount; i++)
            {
                w.Write(mUnknown[i]);
            }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            MemoryStream ms = new MemoryStream(16);
            UnParse(ms);
            return new EAxoidRig(requestedApiVersion, handler, ms);
        }

        public override List<string> ContentFields
        {
            get { return AApiVersionedFields.GetContentFields(requestedApiVersion, typeof(EAxoidRig)); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}
