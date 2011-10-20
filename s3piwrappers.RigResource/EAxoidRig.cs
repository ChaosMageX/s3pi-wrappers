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
                return new Bone(base.requestedApiVersion, handler, ms);
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

        public class IKChain : AHandlerElement, IEquatable<IKChain>
        {
            private EAxoidRig mParent;

            public IKChain(int apiVersion, EventHandler handler, EAxoidRig parent)
                : base(apiVersion, handler)
            {
                mParent = parent;
            }

            public IKChain(int apiVersion, EventHandler handler, EAxoidRig parent, Stream s)
                : base(apiVersion, handler)
            {
                mParent = parent;
                this.Parse(s);
            }

            private const int kBufferSize = 64;//+ 4 * mBoneIndices.Count

            private IntList mBoneIndices;

            private Int32 mInfo01;
            private Int32 mInfo02;
            private Int32 mInfo03;
            private Int32 mInfo04;
            private Int32 mInfo05;
            private Int32 mInfo06;
            private Int32 mInfo07;
            private Int32 mInfo08;
            private Int32 mInfo09;
            private Int32 mInfo10;
            private Int32 mInfo11;

            private Int32 mPole;
            private Int32 mSlotInfo;
            private Int32 mSlotOffset;
            private Int32 mRoot;

            [ElementPriority(16)]
            public int Root
            {
                get { return mRoot; }
                set { if (mRoot != value) { mRoot = value; OnElementChanged(); } }
            }

            [ElementPriority(15)]
            public int SlotOffset
            {
                get { return mSlotOffset; }
                set { if (mSlotOffset != value) { mSlotOffset = value; OnElementChanged(); } }
            }

            [ElementPriority(14)]
            public int SlotInfo
            {
                get { return mSlotInfo; }
                set { if (mSlotInfo != value) { mSlotInfo = value; OnElementChanged(); } }
            }

            [ElementPriority(13)]
            public int Pole
            {
                get { return mPole; }
                set { if (mPole != value) { mPole = value; OnElementChanged(); } }
            }

            [ElementPriority(12)]
            public int Info11
            {
                get { return mInfo11; }
                set { if (mInfo11 != value) { mInfo11 = value; OnElementChanged(); } }
            }

            [ElementPriority(11)]
            public int Info10
            {
                get { return mInfo10; }
                set { if (mInfo10 != value) { mInfo10 = value; OnElementChanged(); } }
            }

            [ElementPriority(10)]
            public int Info09
            {
                get { return mInfo09; }
                set { if (mInfo09 != value) { mInfo09 = value; OnElementChanged(); } }
            }

            [ElementPriority(9)]
            public int Info08
            {
                get { return mInfo08; }
                set { if (mInfo08 != value) { mInfo08 = value; OnElementChanged(); } }
            }

            [ElementPriority(8)]
            public int Info07
            {
                get { return mInfo07; }
                set { if (mInfo07 != value) { mInfo07 = value; OnElementChanged(); } }
            }

            [ElementPriority(7)]
            public int Info06
            {
                get { return mInfo06; }
                set { if (mInfo06 != value) { mInfo06 = value; OnElementChanged(); } }
            }

            [ElementPriority(6)]
            public int Info05
            {
                get { return mInfo05; }
                set { if (mInfo05 != value) { mInfo05 = value; OnElementChanged(); } }
            }

            [ElementPriority(5)]
            public int Info04
            {
                get { return mInfo04; }
                set { if (mInfo04 != value) { mInfo04 = value; OnElementChanged(); } }
            }

            [ElementPriority(4)]
            public int Info03
            {
                get { return mInfo03; }
                set { if (mInfo03 != value) { mInfo03 = value; OnElementChanged(); } }
            }

            [ElementPriority(3)]
            public int Info02
            {
                get { return mInfo02; }
                set { if (mInfo02 != value) { mInfo02 = value; OnElementChanged(); } }
            }

            [ElementPriority(2)]
            public int Info01
            {
                get { return mInfo01; }
                set { if (mInfo01 != value) { mInfo01 = value; OnElementChanged(); } }
            }

            [ElementPriority(1)]
            public IntList BoneIndices
            {
                get { return mBoneIndices; }
                set { mBoneIndices = value; OnElementChanged(); }
            }

            internal void SetParent(EAxoidRig parent)
            {
                mParent = parent;
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                MemoryStream ms = new MemoryStream(kBufferSize + 4 * mBoneIndices.Count);
                this.UnParse(ms);
                return new IKChain(base.requestedApiVersion, handler, this.mParent, ms);
            }

            public override List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(base.requestedApiVersion, typeof(IKChain)); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            private string GetBoneName(int index)
            {
                if (mParent == null || index < 0 || index >= mParent.mBones.Count)
                    return index.ToString();
                else
                    return mParent.mBones[index].ToString();
            }

            public string Value
            {
                get
                {
                    StringBuilder builder = new StringBuilder("Bone Indices:\n");
                    int length = mBoneIndices.Count;
                    for (int i = 0; i < length; i++)
                        builder.AppendFormat("[{0}]:\t{1}\n", i, GetBoneName(mBoneIndices[i]));

                    builder.AppendLine(string.Concat("Info01:\t", GetBoneName(mInfo01)));
                    builder.AppendLine(string.Concat("Info02:\t", GetBoneName(mInfo02)));
                    builder.AppendLine(string.Concat("Info03:\t", GetBoneName(mInfo03)));
                    builder.AppendLine(string.Concat("Info04:\t", GetBoneName(mInfo04)));
                    builder.AppendLine(string.Concat("Info05:\t", GetBoneName(mInfo05)));
                    builder.AppendLine(string.Concat("Info06:\t", GetBoneName(mInfo06)));
                    builder.AppendLine(string.Concat("Info07:\t", GetBoneName(mInfo07)));
                    builder.AppendLine(string.Concat("Info08:\t", GetBoneName(mInfo08)));
                    builder.AppendLine(string.Concat("Info09:\t", GetBoneName(mInfo09)));
                    builder.AppendLine(string.Concat("Info10:\t", GetBoneName(mInfo10)));
                    builder.AppendLine(string.Concat("Info11:\t", GetBoneName(mInfo11)));

                    builder.AppendLine(string.Concat("Pole:\t", GetBoneName(mPole)));
                    builder.AppendLine(string.Concat("SlotInfo:\t", GetBoneName(mSlotInfo)));
                    builder.AppendLine(string.Concat("SlotOffset:\t", GetBoneName(mSlotOffset)));
                    builder.AppendLine(string.Concat("Root:\t", GetBoneName(mRoot)));

                    return builder.ToString();
                }
            }

            private void Parse(Stream s)
            {
                mBoneIndices = new IntList(this.handler, s);
                BinaryReader r = new BinaryReader(s);
                mInfo01 = r.ReadInt32();
                mInfo02 = r.ReadInt32();
                mInfo03 = r.ReadInt32();
                mInfo04 = r.ReadInt32();
                mInfo05 = r.ReadInt32();
                mInfo06 = r.ReadInt32();
                mInfo07 = r.ReadInt32();
                mInfo08 = r.ReadInt32();
                mInfo09 = r.ReadInt32();
                mInfo10 = r.ReadInt32();
                mInfo11 = r.ReadInt32();

                mPole = r.ReadInt32();
                mSlotInfo = r.ReadInt32();
                mSlotOffset = r.ReadInt32();
                mRoot = r.ReadInt32();
            }

            public void UnParse(Stream s)
            {
                mBoneIndices.UnParse(s);
                BinaryWriter w = new BinaryWriter(s);
                w.Write(mInfo01);
                w.Write(mInfo02);
                w.Write(mInfo03);
                w.Write(mInfo04);
                w.Write(mInfo05);
                w.Write(mInfo06);
                w.Write(mInfo07);
                w.Write(mInfo08);
                w.Write(mInfo09);
                w.Write(mInfo10);
                w.Write(mInfo11);

                w.Write(mPole);
                w.Write(mSlotInfo);
                w.Write(mSlotOffset);
                w.Write(mRoot);
            }

            public bool Equals(IKChain other)
            {
                bool flag =
                    this.mInfo01 == other.mInfo01 &&
                    this.mInfo02 == other.mInfo02 &&
                    this.mInfo03 == other.mInfo03 &&
                    this.mInfo04 == other.mInfo04 &&
                    this.mInfo05 == other.mInfo05 &&
                    this.mInfo06 == other.mInfo06 &&
                    this.mInfo07 == other.mInfo07 &&
                    this.mInfo08 == other.mInfo08 &&
                    this.mInfo09 == other.mInfo09 &&
                    this.mInfo10 == other.mInfo10 &&
                    this.mInfo11 == other.mInfo11 &&
                    this.mPole == other.mPole &&
                    this.mSlotInfo == other.mSlotInfo &&
                    this.mSlotOffset == other.mSlotOffset &&
                    this.mRoot == other.mRoot;
                if (flag)
                {
                    int length = this.mBoneIndices.Count;
                    if (length != other.mBoneIndices.Count)
                        return false;
                    for (int i = 0; i < length && flag; i++)
                    {
                        flag = this.mBoneIndices[i] == other.mBoneIndices[i];
                    }
                    return flag;
                }
                return false;
            }
        }

        public class IKChainList : DependentList<IKChain>
        {
            private EAxoidRig mParent;

            public IKChainList(EventHandler handler, EAxoidRig parent, long size = -1L)
                : base(handler, size) { SetParent(parent); }

            public IKChainList(EventHandler handler, EAxoidRig parent, IEnumerable<IKChain> ilt, long size = -1L)
                : base(handler, ilt, size) { SetParent(parent); }

            public IKChainList(EventHandler handler, EAxoidRig parent, Stream s, long size = -1L)
                : base(handler, s, size) { SetParent(parent); }

            private void SetParent(EAxoidRig parent)
            {
                this.mParent = parent;
                int length = base.Count;
                for (int i = 0; i < length; i++)
                {
                    base[i].SetParent(parent);
                }
            }

            public override void Add()
            {
                base.Add(new IKChain(0, base.elementHandler, this.mParent));
            }

            protected override IKChain CreateElement(Stream s)
            {
                return new IKChain(0, base.elementHandler, this.mParent, s);
            }

            protected override void WriteElement(Stream s, IKChain element)
            {
                element.UnParse(s);
            }
        }

        public EAxoidRig(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
            mBones = new BoneList(handler);
            mName = "";
            mIKChains = new IKChainList(handler, this);
        }

        public EAxoidRig(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler)
        {
            Parse(s);
        }

        private UInt32 mMajor = 0x00000004;
        private UInt32 mMinor = 0x00000002;
        private BoneList mBones;
        private string mName;
        private IKChainList mIKChains;

        [ElementPriority(5)]
        public IKChainList IKChains
        {
            get { return mIKChains; }
            set { mIKChains = value; OnElementChanged(); }
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
        public uint MinorVersion
        {
            get { return mMinor; }
            set { mMinor = value; OnElementChanged(); }
        }

        [ElementPriority(1)]
        public uint MajorVersion
        {
            get { return mMajor; }
            set { mMajor = value; OnElementChanged(); }
        }

        public string GetBoneName(int index)
        {
            if (index < 0 || index >= mBones.Count)
                return index.ToString();
            else
                return mBones[index].ToString();
        }

        public string Value
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("Major:\t{0}\nMinor:\t{1}\n", mMajor, mMinor);
                builder.AppendFormat("Name:\t{0}\n", mName);
                builder.Append("Bones:\n");
                int i, count = mBones.Count;
                for (i = 0; i < count; i++)
                {
                    builder.AppendFormat("==Bone[{0}]==\n{1}\n", i, mBones[i].Value);
                }
                if (mIKChains.Count > 0)
                {
                    builder.Append("IK Chains:\n");
                    count = mIKChains.Count;
                    for (i = 0; i < count; i++)
                    {
                        builder.AppendFormat("==IK Chain[{0}]==\n{1}\n", i, mIKChains[i].Value);
                    }
                }
                return builder.ToString();
            }
        }

        private void Parse(Stream s)
        {
            s.Position = 0L;
            BinaryReader r = new BinaryReader(s);
            mMajor = r.ReadUInt32();
            mMinor = r.ReadUInt32();
            mBones = new BoneList(handler, s);
            mName = ReadPascalString(r);
            long unkCount = (s.Length - s.Position) / 4L;
            mIKChains = new IKChainList(handler, this, s);
        }

        public void UnParse(Stream s)
        {
            BinaryWriter w = new BinaryWriter(s);
            w.Write(mMajor);
            w.Write(mMinor);
            mBones.UnParse(s);
            WritePascalString(w, mName);
            mIKChains.UnParse(s);
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
