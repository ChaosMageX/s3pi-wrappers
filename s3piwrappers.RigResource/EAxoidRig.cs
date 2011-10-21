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
                mMirrorIndex = 0;
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
            private Int32 mMirrorIndex;
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
            public int MirrorIndex
            {
                get { return mMirrorIndex; }
                set { if (mMirrorIndex != value) { mMirrorIndex = value; OnElementChanged(); } }
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

            private void Parse(Stream s)
            {
                BinaryReader r = new BinaryReader(s);
                mPosition = new Granny2.Triple(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mOrientation = new Granny2.Quad(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mScale = new Granny2.Triple(0, handler, r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                mName = ReadPascalString(r);
                mMirrorIndex = r.ReadInt32();
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
                w.Write(mMirrorIndex);
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
                    this.mMirrorIndex == other.mMirrorIndex &&
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
            public IKChain(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                this.mIkLinks = new IntList(handler);
            }

            public IKChain(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                this.Parse(s);
            }

            private const int kBufferSize = 64;//+ 4 * mBoneIndices.Count

            private IntList mIkLinks;

            private Int32[] mInfoNodes = new Int32[11];

            private Int32 mIkPole;
            private Int32 mSlotInfo;
            private Int32 mSlotOffset;
            private Int32 mRoot;

            [ElementPriority(6)]
            public int Root
            {
                get { return mRoot; }
                set { if (mRoot != value) { mRoot = value; OnElementChanged(); } }
            }

            [ElementPriority(5)]
            public int SlotOffset
            {
                get { return mSlotOffset; }
                set { if (mSlotOffset != value) { mSlotOffset = value; OnElementChanged(); } }
            }

            [ElementPriority(4)]
            public int SlotInfo
            {
                get { return mSlotInfo; }
                set { if (mSlotInfo != value) { mSlotInfo = value; OnElementChanged(); } }
            }

            [ElementPriority(3)]
            public int IkPole
            {
                get { return mIkPole; }
                set { if (mIkPole != value) { mIkPole = value; OnElementChanged(); } }
            }

            [ElementPriority(2)]
            public int[] InfoNodes
            {
                get { return mInfoNodes; }
                set { if (mInfoNodes != value) { mInfoNodes = value; OnElementChanged(); } }
            }

            [ElementPriority(1)]
            public IntList IkLinks
            {
                get { return mIkLinks; }
                set { if (mIkLinks != value) { mIkLinks = value; OnElementChanged(); } }
            }

            public int GetInfoNode(int index)
            {
                return mInfoNodes[index];
            }

            public void SetInfoNode(int index, int value)
            {
                mInfoNodes[index] = value;
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                MemoryStream ms = new MemoryStream(kBufferSize + 4 * mIkLinks.Count);
                this.UnParse(ms);
                return new IKChain(base.requestedApiVersion, handler, ms);
            }

            public override List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(base.requestedApiVersion, typeof(IKChain)); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            private void Parse(Stream s)
            {
                mIkLinks = new IntList(this.handler, s);
                BinaryReader r = new BinaryReader(s);
                for (int i = 0; i < 11; i++)
                    mInfoNodes[i] = r.ReadInt32();

                mIkPole = r.ReadInt32();
                mSlotInfo = r.ReadInt32();
                mSlotOffset = r.ReadInt32();
                mRoot = r.ReadInt32();
            }

            public void UnParse(Stream s)
            {
                mIkLinks.UnParse(s);
                BinaryWriter w = new BinaryWriter(s);
                for (int i = 0; i < 11; i++)
                    w.Write(mInfoNodes[i]);

                w.Write(mIkPole);
                w.Write(mSlotInfo);
                w.Write(mSlotOffset);
                w.Write(mRoot);
            }

            public bool Equals(IKChain other)
            {
                bool flag =
                    this.mIkPole == other.mIkPole &&
                    this.mSlotInfo == other.mSlotInfo &&
                    this.mSlotOffset == other.mSlotOffset &&
                    this.mRoot == other.mRoot;
                if (flag)
                {
                    for (int j = 0; j < 11 && flag; j++)
                    {
                        flag = this.mInfoNodes[j] == other.mInfoNodes[j];
                    }
                }
                if (flag)
                {
                    int length = this.mIkLinks.Count;
                    if (length != other.mIkLinks.Count)
                        return false;
                    for (int i = 0; i < length && flag; i++)
                    {
                        flag = this.mIkLinks[i] == other.mIkLinks[i];
                    }
                    return flag;
                }
                return false;
            }
        }

        public class IKChainList : DependentList<IKChain>
        {
            public IKChainList(EventHandler handler, long size = -1L)
                : base(handler, size) { }

            public IKChainList(EventHandler handler, IEnumerable<IKChain> ilt, long size = -1L)
                : base(handler, ilt, size) { }

            public IKChainList(EventHandler handler, Stream s, long size = -1L)
                : base(handler, s, size) { }

            public override void Add()
            {
                base.Add(new IKChain(0, base.elementHandler));
            }

            protected override IKChain CreateElement(Stream s)
            {
                return new IKChain(0, base.elementHandler, s);
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
            mIKChains = new IKChainList(handler);
        }

        public EAxoidRig(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler)
        {
            Parse(s);
        }

        private UInt32 mMajor = 0x04;
        private UInt32 mMinor = 0x02;
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

        public int GetBoneParent(int index)
        {
            return mBones[index].ParentIndex;
        }

        public string GetBoneName(int index)
        {
            if (index < 0 || index >= mBones.Count)
                return index.ToString();
            else
                return string.Concat(index.ToString(), "\t", mBones[index].ToString());
        }

        private string BoneValue(Bone bone)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Name:\t{0}\n", bone.Name);

            builder.AppendFormat("Position:\n{0}\n", bone.Position);
            builder.AppendFormat("Orientation:\n{0}\n", bone.Orientation);
            builder.AppendFormat("Scale:\n{0}\n", bone.Scale);

            builder.AppendFormat("Mirror Index:\t{0}\n", GetBoneName(bone.MirrorIndex));
            builder.AppendFormat("Parent Index:\t{0}\n", GetBoneName(bone.ParentIndex));
            builder.AppendFormat("Name Hash:\t0x{0:X8}\n", bone.NameHash);
            builder.AppendFormat("Int 02:\t0x{0:X8}\t({0})\n", bone.Int02);

            return builder.ToString();
        }

        private string IKChainValue(IKChain ikChain)
        {
            StringBuilder builder = new StringBuilder("IK Links:\n");
            int length = ikChain.IkLinks.Count;
            for (int i = 0; i < length; i++)
            {
                builder.AppendFormat("[{0}]:\t{1}\n", i, GetBoneName(ikChain.IkLinks[i]));
            }
            for (int j = 0; j < 11; j++)
            {
                builder.AppendFormat("Info{0:00}:\t{1}\n", j, GetBoneName(ikChain.InfoNodes[j]));
            }
            builder.AppendLine(string.Concat("IkPole:\t", GetBoneName(ikChain.IkPole)));
            builder.AppendLine(string.Concat("SlotInfo:\t", GetBoneName(ikChain.SlotInfo)));
            builder.AppendLine(string.Concat("SlotOffset:\t", GetBoneName(ikChain.SlotOffset)));
            builder.AppendLine(string.Concat("Root:\t", GetBoneName(ikChain.Root)));

            return builder.ToString();
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
                    builder.AppendFormat("==Bone[{0}]==\n{1}\n", i, BoneValue(mBones[i]));
                }
                if (mIKChains.Count > 0)
                {
                    builder.Append("IK Chains:\n");
                    count = mIKChains.Count;
                    for (i = 0; i < count; i++)
                    {
                        builder.AppendFormat("==IK Chain[{0}]==\n{1}\n", i, IKChainValue(mIKChains[i]));
                    }
                }
#if DEBUG
                SortedDictionary<uint, List<int>> unknownSort = new SortedDictionary<uint, List<int>>();
                List<int> indices;
                count = mBones.Count;
                uint unk;
                for (i = 0; i < count; i++)
                {
                    unk = mBones[i].Int02;
                    if (!unknownSort.ContainsKey(unk))
                    {
                        indices = new List<int>(count / 4);
                        indices.Add(i);
                        unknownSort.Add(unk, indices);
                    }
                    else
                    {
                        unknownSort[unk].Add(i);
                    }
                }
                builder.Append("Int02 Groups:\n");
                foreach (KeyValuePair<uint, List<int>> pair in unknownSort)
                {
                    builder.AppendFormat("==Group 0x{0:X8} ({0})==\n", pair.Key);
                    for (i = 0; i < pair.Value.Count; i++)
                    {
                        builder.AppendFormat("[{0}]:\t{1}\n", i, GetBoneName(pair.Value[i]));
                    }
                    builder.AppendLine();
                }
#endif
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
            mIKChains = new IKChainList(handler, s);
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
