using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.Settings;
namespace s3piwrappers
{

    public struct Vertex
    {
        public float[] Position;
        public float[] Normal;
        public float[][] UV;
        public byte[] BlendIndices;
        public float[] BlendWeights;
        public float[] Tangents;
        public float[] Color;

    }
    public class VBUF2 : VBUF
    {
        public VBUF2(int apiVersion, EventHandler handler, VBUF basis)
            : base(apiVersion, handler, basis)
        {
        }

        public VBUF2(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public VBUF2(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public override uint ResourceType
        {
            get
            {
                return 0x0229684B;
            }
        }
    }
    public class VBUF : ARCOLBlock
    {
        [Flags]
        public enum FormatFlags : uint
        {
            Collapsed = 0x4,
            DifferencedVertices = 0x2,
            Dynamic = 0x1,
            None = 0x0
        }
        
        private UInt32 mVersion = 0x00000101;
        private FormatFlags mFlags;
        private UInt32 mSwizzleInfo;
        private Byte[] mBuffer;
        public VBUF(int apiVersion, EventHandler handler, VBUF basis): this(apiVersion, handler, basis.Version, basis.Flags, basis.SwizzleInfo, (Byte[])basis.Buffer.Clone()){}
        public VBUF(int apiVersion, EventHandler handler): base(apiVersion, handler, null){}
        public VBUF(int apiVersion, EventHandler handler, Stream s): base(apiVersion, handler, s){}
        public VBUF(int APIversion, EventHandler handler, uint version, FormatFlags flags, uint swizzleInfo, byte[] buffer) : this(APIversion, handler)
        {
            mVersion = version;
            mFlags = flags;
            mSwizzleInfo = swizzleInfo;
            mBuffer = buffer;
        }

        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(2)]
        public FormatFlags Flags { get { return mFlags; } set { if(mFlags!=value){mFlags = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(3)]
        public UInt32 SwizzleInfo { get { return mSwizzleInfo; } set { if(mSwizzleInfo!=value){mSwizzleInfo = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(4)]
        public Byte[] Buffer { get { return mBuffer; } set { if(mBuffer!=value){mBuffer = value; OnRCOLChanged(this, new EventArgs());} } }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("Swizzle Info:\t0x{0:X8}\n", mSwizzleInfo);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
            }
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mFlags = (FormatFlags)br.ReadUInt32();
            mSwizzleInfo = br.ReadUInt32();
            mBuffer = new Byte[s.Length - s.Position];
            s.Read(mBuffer, 0, mBuffer.Length);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write((UInt32)mFlags);
            bw.Write(mSwizzleInfo);
            if (mBuffer == null) mBuffer = new byte[0];
            bw.Write(mBuffer);
            return s;
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VBUF(0, handler, this);
        }
        public override uint ResourceType
        {
            get { return 0x01D0E6FB; }
        }

        public override string Tag
        {
            get { return "VBUF"; }
        }
        static bool checking = Settings.Checking;

        // TODO: does not handle all data yet
        public Vertex[] GetVertices(VRTF vrtf, long offset, int count)
        {
            long streamOffset = offset;
            Stream s = new MemoryStream(mBuffer);
            s.Seek(streamOffset, SeekOrigin.Begin);

            var position = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Position);
            var normal = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Normal);

            var uv = vrtf.Layouts
                .Where(x => x.Usage == VRTF.ElementUsage.UV)
                .ToArray();
            Vertex[] verts = new Vertex[count];

            for (int i = 0; i < count; i++)
            {
                Vertex v = new Vertex();
                byte[] data = new byte[vrtf.Stride];
                s.Read(data, 0, vrtf.Stride);
                if (position != null)
                {
                    float[] posPoints = new float[3];
                    ReadFloatData(data, position, ref posPoints);
                    v.Position = posPoints;
                }
                if (normal != null)
                {
                    float[] normPoints = new float[3];
                    ReadFloatData(data, normal, ref normPoints);
                    v.Normal = normPoints;
                }
                v.UV = new float[uv.Length][];
                for (int j = 0; j < uv.Length; j++)
                {
                    var u = uv[j];
                    float[] uvPoints = new float[(u.Format == VRTF.ElementFormat.Float4 || u.Format == VRTF.ElementFormat.UShort4N) ? 4 : 2];
                    ReadFloatData(data, u, ref uvPoints);
                    v.UV[j] = uvPoints;
                }
                verts[i] = v;
            }
            return verts;
        }
        public static void ReadFloatData(byte[] data, VRTF.ElementLayout layout, ref float[] output)
        {
            byte[] element = new byte[VRTF.ElementSizeFromFormat(layout.Format)];
            Array.Copy(data, layout.Offset, element, 0, element.Length);
            float a, b, c, scalar;

            switch (layout.Format)
            {
                case VRTF.ElementFormat.Float1:
                    output[0] += BitConverter.ToSingle(element, 0);
                    break;
                case VRTF.ElementFormat.Float2:
                    output[0] += BitConverter.ToSingle(element, 0);
                    output[1] += BitConverter.ToSingle(element, 4);
                    break;
                case VRTF.ElementFormat.Float3:
                    output[0] += BitConverter.ToSingle(element, 0);
                    output[1] += BitConverter.ToSingle(element, 4);
                    output[2] += BitConverter.ToSingle(element, 8);
                    break;
                case VRTF.ElementFormat.Float4:
                    output[0] += BitConverter.ToSingle(element, 0);
                    output[1] += BitConverter.ToSingle(element, 4);
                    output[2] += BitConverter.ToSingle(element, 8);
                    output[3] += BitConverter.ToSingle(element, 12);
                    break;
                case VRTF.ElementFormat.ColorUByte4:
                    a = (SByte)element[0];
                    b = (SByte)element[1];
                    c = (SByte)element[2];
                    scalar = element[3];
                    if (scalar == 0) scalar = SByte.MaxValue;
                    scalar = 1f / scalar;
                    output[0] += scalar * (a < 0 ? a + 128 : a - 128);
                    output[1] += scalar * (b < 0 ? b + 128 : b - 128);
                    output[2] += scalar * (c < 0 ? c + 128 : c - 128);
                    break;
                case VRTF.ElementFormat.Short2:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    output[0] += a * (1f / short.MaxValue);
                    output[1] += b * (1f / short.MaxValue);
                    break;
                case VRTF.ElementFormat.Short4:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    c = BitConverter.ToInt16(element, 4);
                    scalar = BitConverter.ToUInt16(element, 6);
                    if (scalar == 0) scalar = short.MaxValue;
                    scalar = 1f / scalar;
                    output[0] += a * scalar;
                    output[1] += b * scalar;
                    output[2] += c * scalar;
                    break;
                case VRTF.ElementFormat.UShort4N:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    c = BitConverter.ToInt16(element, 4);
                    scalar = BitConverter.ToUInt16(element, 6);
                    if (scalar == 0) scalar = 512;
                    scalar = 1f / scalar;
                    output[0] += a * scalar;
                    output[1] += b * scalar;
                    output[2] += c * scalar;
                    break;
            }
        }
    }
}
