﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers;

namespace s3piwrappers.ModelViewer
{
    struct Vertex
    {
        public float[] Position;
        public float[] Normal;
        public float[] UV;

    }
    class MLODUtil
    {
        private static VRTF DEFAULT_VRTF;
        static MLODUtil()
        {
            DEFAULT_VRTF = new VRTF(0,null);
            DEFAULT_VRTF.Stride = 16;
            DEFAULT_VRTF.Layouts.Add(new VRTF.VertexElementLayout(0, null, VRTF.VertexElementFormat.Short4, 0,
                                                                  VRTF.VertexElementUsage.Position, 0));
            DEFAULT_VRTF.Layouts.Add(new VRTF.VertexElementLayout(0, null, VRTF.VertexElementFormat.ColorUByte4, 0,
                                                                  VRTF.VertexElementUsage.Normal, 0));
            DEFAULT_VRTF.Layouts.Add(new VRTF.VertexElementLayout(0, null, VRTF.VertexElementFormat.Short2, 0,
                                                                  VRTF.VertexElementUsage.UV, 0));
        }
        static int IndexCountFromPrimitiveType(ModelPrimitiveType t)
        {
            switch (t)
            {
                case ModelPrimitiveType.TriangleList:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
        public static Int32[] GetIndices(MLOD.Mesh mesh, GenericRCOLResource rcol)
        {
            var ibuf = (IBUF)GetBlock(mesh.IndexBufferIndex, rcol);

            var s = new MemoryStream(ibuf.Buffer);
            var r = new BinaryReader(s);
            bool is32Bit = (ibuf.Flags & IBUF.IndexBufferFlags.Uses32BitIndices) != 0;
            Int32[] indices = new Int32[s.Length / (is32Bit? 4:2)];
            Int32 last = 0;
            for (int i = 0; i < indices.Length; i++)
            {
                Int32 cur = is32Bit ? r.ReadInt32() : r.ReadInt16();
                if ((ibuf.Flags & IBUF.IndexBufferFlags.DifferencedIndices) != 0)
                {
                    cur += last;
                }
                indices[i] = cur;
                last = cur;
            }
            
            Int32[] output = new int[mesh.PrimitiveCount * IndexCountFromPrimitiveType(mesh.PrimitiveType)];
            Array.Copy(indices,(int)mesh.StartIndex,output,0,output.Length);
            return output;
        }

        public static Vertex[] GetVertices(MLOD.Mesh mesh, GenericRCOLResource rcol)
        {
            var vbuf = (VBUF)GetBlock(mesh.VertexBufferIndex, rcol);
            var vrtf = (VRTF)GetBlock(mesh.VertexFormatIndex, rcol);
            if (vrtf == null) vrtf = DEFAULT_VRTF;
            var s = new MemoryStream(vbuf.Buffer);
            s.Seek(mesh.StreamOffset, SeekOrigin.Begin);
            var position = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.VertexElementUsage.Position);
            var normal = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.VertexElementUsage.Normal);

            var uv = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.VertexElementUsage.UV);
            Vertex[] verts = new Vertex[mesh.VertexCount];
            
            for (int i = 0; i < mesh.VertexCount;i++)
            {
                byte[] data = new byte[vrtf.Stride];
                s.Read(data, 0, (int)vrtf.Stride);
                Vertex v = new Vertex();
                float[] posPoints = new float[3];
                float[] normPoints = new float[3];
                float[] uvPoints = new float[2];
                ReadVertexData(data, position, ref posPoints);
                ReadVertexData(data, normal, ref normPoints);
                ReadVertexData(data, uv, ref uvPoints);
                verts[i] = new Vertex
                           {
                               Position = posPoints,
                               Normal = normPoints,
                               UV =uvPoints
                           };
            }
            return verts;
        }
        static int ElementSizeFromFormat(VRTF.VertexElementFormat f)
        {
            switch (f)
            {
                case VRTF.VertexElementFormat.Float1:
                case VRTF.VertexElementFormat.UByte4:
                case VRTF.VertexElementFormat.ColorUByte4:
                case VRTF.VertexElementFormat.UByte4N:
                case VRTF.VertexElementFormat.UShort2N:
                case VRTF.VertexElementFormat.Short2:
                    return 4;
                case VRTF.VertexElementFormat.UShort4N:
                case VRTF.VertexElementFormat.Float2:
                case VRTF.VertexElementFormat.Short4:
                case VRTF.VertexElementFormat.Short4N:
                    return 8;
                default:
                    throw new NotImplementedException();
            }
        }
        static void ReadVertexData(byte[] data, VRTF.VertexElementLayout layout, ref float[] output)
        {
            byte[] element = new byte[ElementSizeFromFormat(layout.Format)];
            Array.Copy(data, layout.Offset, element, 0, element.Length);
            float a, b, c, scalar;
            switch (layout.Format)
            {
                case VRTF.VertexElementFormat.Float1:
                    output[0] = BitConverter.ToSingle(element, 0);
                    break;
                case VRTF.VertexElementFormat.Float2:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[2] = BitConverter.ToSingle(element, 4);
                    break;
                case VRTF.VertexElementFormat.Float3:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[1] = BitConverter.ToSingle(element, 4);
                    output[2] = BitConverter.ToSingle(element, 8);
                    break;
                case VRTF.VertexElementFormat.Float4:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[1] = BitConverter.ToSingle(element, 4);
                    output[2] = BitConverter.ToSingle(element, 8);
                    output[3] = BitConverter.ToSingle(element, 12);
                    break;
                case VRTF.VertexElementFormat.ColorUByte4:
                    c = (SByte)element[0];
                    b = (SByte)element[1];
                    a = (SByte)element[2];
                    scalar = element[3];
                    if (scalar == 0) scalar = SByte.MaxValue;
                    scalar = 1f / scalar;
                    output[0] = scalar*(a < 0 ? a + 128 : a - 128);
                    output[1] = scalar * (b < 0 ? b + 128 : b - 128);
                    output[2] = scalar * (c < 0 ? c + 128 : c - 128);
                    break;
                case VRTF.VertexElementFormat.Short2:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    output[0] = a * (1f / short.MaxValue);
                    output[1] = b * (1f / short.MaxValue);
                    break;
                case VRTF.VertexElementFormat.Short4:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    c = BitConverter.ToInt16(element, 4);
                    scalar = BitConverter.ToUInt16(element, 6);
                    if (scalar == 0) scalar = ushort.MaxValue;
                    scalar = 1f / scalar;
                    output[0] = a * scalar;
                    output[1] = b * scalar;
                    output[2] = c * scalar;
                    break;
            }
        }
        public static IRCOLBlock GetBlock(UInt32 reference, GenericRCOLResource rcol)
        {
            if (reference == 0) return null;
            uint index = (reference & 0x00FFFFFF) - 1;
            if ((reference & 0x10000000) != 0)
            {
                index += rcol.DataType;
            }
            return rcol.ChunkEntries[(int)index].RCOLBlock;
        }

    }
}
