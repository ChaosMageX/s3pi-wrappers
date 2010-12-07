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
        private static readonly VRTF DEFAULT_VRTF;
        static MLODUtil()
        {
            DEFAULT_VRTF = new VRTF(0,null);
            DEFAULT_VRTF.Stride = 16;
            DEFAULT_VRTF.Layouts.Add(new VRTF.ElementLayout(0, null, VRTF.ElementFormat.Short4, 0,
                                                                  VRTF.ElementUsage.Position, 0));
            //DEFAULT_VRTF.Layouts.Add(new VRTF.ElementLayout(0, null, VRTF.ElementFormat.ColorUByte4, 0,
            //                                                      VRTF.ElementUsage.Normal, 0));
            //DEFAULT_VRTF.Layouts.Add(new VRTF.ElementLayout(0, null, VRTF.ElementFormat.UShort4N, 0,
            //                                                      VRTF.ElementUsage.UV, 0));
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
            Int32[] output = new int[mesh.PrimitiveCount * IndexCountFromPrimitiveType(mesh.PrimitiveType)];
            Array.Copy(ibuf.Buffer,(int)mesh.StartIndex,output,0,output.Length);
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
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Position);
            var normal = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.Normal);

            var uv = vrtf.Layouts
                .FirstOrDefault(x => x.Usage == VRTF.ElementUsage.UV);
            Vertex[] verts = new Vertex[mesh.VertexCount];
            
            for (int i = 0; i < mesh.VertexCount;i++)
            {
                Vertex v = new Vertex();
                byte[] data = new byte[vrtf.Stride];
                s.Read(data, 0, (int)vrtf.Stride);
                if(position!=null)
                {
                    float[] posPoints = new float[3];
                    ReadVertexData(data, position, ref posPoints);
                    v.Position = posPoints;
                }
                if(normal !=null)
                {
                    float[] normPoints = new float[3];
                    ReadVertexData(data, normal, ref normPoints);
                    v.Normal = normPoints;
                }
                if(uv!=null)
                {
                    float[] uvPoints = new float[uv.Format == VRTF.ElementFormat.Float4 ? 4 : 2];
                    ReadVertexData(data, uv, ref uvPoints);
                    v.UV = uvPoints;
                }
                verts[i] = v;
            }
            return verts;
        }
        static void ReadVertexData(byte[] data, VRTF.ElementLayout layout, ref float[] output)
        {
            byte[] element = new byte[VRTF.ElementSizeFromFormat(layout.Format)];
            Array.Copy(data, layout.Offset, element, 0, element.Length);
            float a, b, c, scalar;
            switch (layout.Format)
            {
                case VRTF.ElementFormat.Float1:
                    output[0] = BitConverter.ToSingle(element, 0);
                    break;
                case VRTF.ElementFormat.Float2:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[1] = BitConverter.ToSingle(element, 4);
                    break;
                case VRTF.ElementFormat.Float3:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[1] = BitConverter.ToSingle(element, 4);
                    output[2] = BitConverter.ToSingle(element, 8);
                    break;
                case VRTF.ElementFormat.Float4:
                    output[0] = BitConverter.ToSingle(element, 0);
                    output[1] = BitConverter.ToSingle(element, 4);
                    output[2] = BitConverter.ToSingle(element, 8);
                    output[3] = BitConverter.ToSingle(element, 12);
                    break;
                case VRTF.ElementFormat.ColorUByte4:
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
                case VRTF.ElementFormat.Short2:
                    a = BitConverter.ToInt16(element, 0);
                    b = BitConverter.ToInt16(element, 2);
                    output[0] = a * (1f / short.MaxValue);
                    output[1] = b * (1f / short.MaxValue);
                    break;
                case VRTF.ElementFormat.Short4:
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
