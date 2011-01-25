using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;
namespace s3piwrappers
{
    public class IBUF2 : IBUF
    {
        public IBUF2(int apiVersion, EventHandler handler, IBUF basis)
            : base(apiVersion, handler, basis)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public override uint ResourceType
        {
            get { return 0x0229684F; }
        }
    }
    public class IBUF : ARCOLBlock
    {
        public int AddIndices(Int32[] indices)
        {
            int starts = mBuffer.Length;
            Int32[] newBuffer = new int[indices.Length+mBuffer.Length];
            Array.Copy(mBuffer, 0, newBuffer, 0, mBuffer.Length);
            Array.Copy(indices, 0, newBuffer, 0, indices.Length);
            mBuffer = newBuffer;
            return starts;
        }
        public Int32[] GetIndices(MLOD.Mesh mesh)
        {
            return GetIndices(mesh.PrimitiveType, mesh.StartIndex, mesh.PrimitiveCount);
        }
        public Int32[] GetIndices(MLOD.Mesh mesh,VRTF vrtf, int geoStateIndex)
        {
            MLOD.GeometryState geometryState = mesh.GeometryStates[geoStateIndex];
            return GetIndices(mesh,vrtf, geometryState);
        }
        public Int32[] GetIndices(MLOD.Mesh mesh,VRTF vrtf, MLOD.GeometryState geometryState)
        {
            return GetIndices(mesh.PrimitiveType, geometryState.StartIndex, geometryState.PrimitiveCount)
                .Select(x=>x-geometryState.MinVertexIndex).ToArray();
        }
        public Int32[] GetIndices(ModelPrimitiveType type, Int32 startIndex, Int32 count)
        {
            return GetIndices(MLOD.IndexCountFromPrimitiveType(type),startIndex,count);
        }
        public Int32[] GetIndices(int sizePerPrimitive, Int32 startIndex, Int32 count)
        {
            Int32[] output = new int[count * sizePerPrimitive];
            Array.Copy(mBuffer, startIndex, output, 0, output.Length);
            return output;
        }

        [Flags]
        public enum FormatFlags : uint
        {
            DifferencedIndices = 0x1,
            Uses32BitIndices = 0x2,
            IsDisplayList = 0x4
        }
        private UInt32 mVersion;
        private FormatFlags mFlags;
        private UInt32 mDisplayListUsage;
        private Int32[] mBuffer;
        public IBUF(int apiVersion, EventHandler handler) : base(apiVersion, handler, null) { }
        public IBUF(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
        public IBUF(int apiVersion, EventHandler handler, IBUF basis) : this(apiVersion, handler, basis.Version, basis.Flags, basis.DisplayListUsage, basis.Buffer) { }
        public IBUF(int APIversion, EventHandler handler, uint version, FormatFlags flags, uint displayListUsage, Int32[] buffer)
            : base(APIversion, handler, null)
        {
            mVersion = version;
            mFlags = flags;
            mDisplayListUsage = displayListUsage;
            mBuffer = buffer;
        }

        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(2)]
        public FormatFlags Flags { get { return mFlags; } set { if (mFlags != value) { mFlags = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(3)]
        public UInt32 DisplayListUsage { get { return mDisplayListUsage; } set { if (mDisplayListUsage != value) { mDisplayListUsage = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(4)]
        public Int32[] Buffer { get { return mBuffer; } set { if (mBuffer != value) { mBuffer = value; OnRCOLChanged(this, new EventArgs()); } } }

        public string Value
        {
            get
            {
                return ValueBuilder;
                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("DisplayListUsage:\t0x{0:X8}\n", mDisplayListUsage);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
                /**/
            }
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new IBUF(0, handler, this);
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{2:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mFlags = (FormatFlags)br.ReadUInt32();
            mDisplayListUsage = br.ReadUInt32();

            bool is32Bit = (mFlags & FormatFlags.Uses32BitIndices) != 0;
            mBuffer = new Int32[(s.Length-s.Position) / (is32Bit ? 4 : 2)];
            Int32 last = 0;
            for (int i = 0; i < mBuffer.Length; i++)
            {
                Int32 cur = is32Bit ? br.ReadInt32() : br.ReadInt16();
                if ((mFlags & FormatFlags.DifferencedIndices) != 0)
                {
                    cur += last;
                }
                mBuffer[i] = cur;
                last = cur;
            }


        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            if (mBuffer == null) mBuffer = new Int32[0];
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write((UInt32)mFlags);
            bw.Write(mDisplayListUsage);

            bool is32Bit = (mFlags & FormatFlags.Uses32BitIndices) != 0;
            bool isDifferenced = (mFlags & FormatFlags.DifferencedIndices) != 0;
            Int32 last = 0;
            for (int i = 0; i < mBuffer.Length; i++)
            {
                Int32 cur = mBuffer[i];
                if (isDifferenced)
                {
                    cur -= last;
                    last = mBuffer[i];
                }
                if (is32Bit) bw.Write(cur); else bw.Write((UInt16)cur);
            }
            return s;
        }

        public override uint ResourceType
        {
            get { return 0x01D0E70F; }
        }

        public override string Tag
        {
            get { return "IBUF"; }
        }
        static bool checking = Settings.Checking;
    }
}
