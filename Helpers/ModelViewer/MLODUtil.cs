using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers;

namespace s3piwrappers.ModelViewer
{
    class MLODUtil
    {
        public static Int32[] GetIndices(MLOD.Mesh mesh, MLOD.GeometryState geostate, GenericRCOLResource rcol)
        {            
            var ibuf = (IBUF)GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.IndexBufferIndex);
            return ibuf.GetIndices(mesh, geostate);

        }
        public static Int32[] GetIndices(MLOD.Mesh mesh,  GenericRCOLResource rcol)
        {
            var ibuf = (IBUF)GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.IndexBufferIndex);
            return ibuf.GetIndices(mesh);
        }
        public static Vertex[] GetVertices(MLOD.Mesh mesh, MLOD.GeometryState geo, GenericRCOLResource rcol)
        {
            return GetVertices(mesh, mesh.StreamOffset, geo.VertexCount + geo.MinVertexIndex, rcol);
        }
        public static Vertex[] GetVertices(MLOD.Mesh mesh, GenericRCOLResource rcol)
        {
            return GetVertices(mesh, mesh.StreamOffset, mesh.VertexCount, rcol);
        }

        public static Vertex[] GetVertices(MLOD.Mesh mesh, long offset, int count, GenericRCOLResource rcol)
        {
            VBUF vbuf = (VBUF)GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexBufferIndex);
            VRTF vrtf = (VRTF)GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexFormatIndex);

            if (vrtf == null)
            {
                vrtf = mesh.IsShadowCaster ? VRTF.CreateDefaultForSunShadow() : VRTF.CreateDefaultForDropShadow();
            }
            return vbuf.GetVertices(vrtf,offset,count);
        }

    }
}
