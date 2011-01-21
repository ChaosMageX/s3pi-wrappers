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
        private GenericRCOLResource.ChunkReference mSwizzleInfo;
        private Byte[] mBuffer;
        public VBUF(int apiVersion, EventHandler handler, VBUF basis) : this(apiVersion, handler, basis.Version, basis.Flags, basis.SwizzleInfo, (Byte[])basis.Buffer.Clone()) { }
        public VBUF(int apiVersion, EventHandler handler) : base(apiVersion, handler, null) { }
        public VBUF(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
        public VBUF(int APIversion, EventHandler handler, uint version, FormatFlags flags, GenericRCOLResource.ChunkReference swizzleInfo, byte[] buffer)
            : this(APIversion, handler)
        {
            mVersion = version;
            mFlags = flags;
            mSwizzleInfo = swizzleInfo;
            mBuffer = buffer;
        }

        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(2)]
        public FormatFlags Flags { get { return mFlags; } set { if (mFlags != value) { mFlags = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(3)]
        public GenericRCOLResource.ChunkReference SwizzleInfo { get { return mSwizzleInfo; } set { if (mSwizzleInfo != value) { mSwizzleInfo = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(4)]
        public Byte[] Buffer { get { return mBuffer; } set { if (mBuffer != value) { mBuffer = value; OnRCOLChanged(this, new EventArgs()); } } }
        public string Value
        {
            get
            {
                return ValueBuilder;
                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("Swizzle Info:\t0x{0:X8}\n", mSwizzleInfo);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
                /**/
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
            mSwizzleInfo = new GenericRCOLResource.ChunkReference(0, handler, s);
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
            if (mSwizzleInfo == null) mSwizzleInfo = new GenericRCOLResource.ChunkReference(0, handler, 0);
            mSwizzleInfo.UnParse(s);
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
            var blendIndices = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.BlendIndex);
            var blendWeights = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.BlendWeight);
            var tangents = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Tangent);
            var color = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Colour);

            Vertex[] verts = new Vertex[count];

            for (int i = 0; i < count; i++)
            {
                Vertex v = new Vertex();
                byte[] data = new byte[vrtf.Stride];
                s.Read(data, 0, vrtf.Stride);
                if (position != null)
                {
                    float[] posPoints = new float[VRTF.FloatCountFromFormat(position.Format)];
                    ReadFloatData(data, position, ref posPoints);
                    v.Position = posPoints;
                }
                if (normal != null)
                {
                    float[] normPoints = new float[VRTF.FloatCountFromFormat(normal.Format)];
                    ReadFloatData(data, normal, ref normPoints);
                    v.Normal = normPoints;
                }
                v.UV = new float[uv.Length][];
                for (int j = 0; j < uv.Length; j++)
                {
                    var u = uv[j];
                    float[] uvPoints = new float[VRTF.FloatCountFromFormat(u.Format)];
                    ReadFloatData(data, u, ref uvPoints);
                    v.UV[j] = uvPoints;
                }
                if (blendIndices != null)
                {
                    byte[] blendIPoints = new byte[VRTF.ByteSizeFromFormat(blendWeights.Format)];
                    Array.Copy(data, blendIndices.Offset, blendIPoints, 0, blendIPoints.Length);
                    v.BlendIndices = blendIPoints;
                }
                if (blendWeights != null)
                {
                    float[] blendWPoints = new float[VRTF.FloatCountFromFormat(blendWeights.Format)];
                    ReadFloatData(data, blendWeights, ref blendWPoints);
                    v.BlendWeights = blendWPoints;
                }
                if (tangents != null)
                {
                    float[] tangentPoints = new float[VRTF.FloatCountFromFormat(tangents.Format)];
                    ReadFloatData(data, tangents, ref tangentPoints);
                    v.Tangents = tangentPoints;
                }
                if (color != null)
                {
                    float[] colorPoints = new float[VRTF.FloatCountFromFormat(color.Format)];
                    ReadFloatData(data, color, ref colorPoints);
                    v.Color = colorPoints;
                }
                verts[i] = v;
            }
            return verts;
        }
        //Currently not supported:
        //UByte4N,
        //Short2N, Short4N, UShort2N,
        //Dec3N, UDec3N,
        //Float16_2, Float16_4
        public static void ReadFloatData(byte[] data, VRTF.ElementLayout layout, ref float[] output)
        {
            byte[] element = new byte[VRTF.ByteSizeFromFormat(layout.Format)];
            Array.Copy(data, layout.Offset, element, 0, element.Length);
            float scalar;

            switch (layout.Format)
            {
                case VRTF.ElementFormat.Float1:
                case VRTF.ElementFormat.Float2:
                case VRTF.ElementFormat.Float3:
                case VRTF.ElementFormat.Float4:
                    for (int i = 0; i < output.Length; i++)
                        output[i] += BitConverter.ToSingle(element, i * sizeof(float));
                    break;
                case VRTF.ElementFormat.ColorUByte4:
                    /* This doesn't match Wes Howe's result
                    a = (SByte)element[0];
                    b = (SByte)element[1];
                    c = (SByte)element[2];
                    scalar = element[3];
                    if (scalar == 0) scalar = SByte.MaxValue;
                    output[0] += (a < 0 ? a + 128 : a - 128) / scalar;
                    output[1] += (b < 0 ? b + 128 : b - 128) / scalar;
                    output[2] += (c < 0 ? c + 128 : c - 128) / scalar;
                    /**/
                    scalar = Byte.MaxValue - element[3];
                    if (layout.Usage != VRTF.ElementUsage.BlendWeight)
                    {
                        if (scalar == 0) scalar = 128;
                        for (int i = 0; i < output.Length; i++)
                            output[i] += (element[2 - i] - 128) / scalar;
                    }
                    else
                    {
                        if (scalar == 0) scalar = 256;
                        for (int i = 0; i < output.Length; i++)
                            output[i] += element[2 - i] / scalar;
                    }
                    break;
                case VRTF.ElementFormat.Short2:
                    for (int i = 0; i < output.Length; i++)
                        output[i] += BitConverter.ToInt16(element, i * sizeof(short)) / (Single)short.MaxValue;
                    break;
                case VRTF.ElementFormat.Short4:
                    scalar = BitConverter.ToUInt16(element, 3 * sizeof(short));
                    if (scalar == 0) scalar = ushort.MaxValue;
                    for (int i = 0; i < output.Length; i++)
                        output[i] += BitConverter.ToInt16(element, i * sizeof(short)) / scalar;
                    break;
                case VRTF.ElementFormat.UShort4N:
                    scalar = BitConverter.ToUInt16(element, 3 * sizeof(short));
                    if (scalar == 0) scalar = 512;
                    for (int i = 0; i < output.Length; i++)
                        output[i] += BitConverter.ToInt16(element, i * sizeof(short)) / scalar;
                    break;
            }
        }
    }
}
