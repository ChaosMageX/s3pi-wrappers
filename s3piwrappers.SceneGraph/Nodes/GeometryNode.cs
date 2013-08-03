using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class GeometryNode : DefaultNode
    {
        public const uint GEOM_TID = 0x015A1849;

        public override string GetContentPathRootName()
        {
            if (base.originalKey.ResourceType == GEOM_TID)
                return "geom";
            return base.GetContentPathRootName();
        }

        private static bool IsNormalMap(ShaderData sd)
        {
            return sd.Field == FieldType.NormalMap;
        }

        private IResourceConnection GEOM_getNormalMap()
        {
            Diagnostics.Log("GEOM_getNormalMap");
            //if (geomItem == null) { Diagnostics.Show("GEOM reference not found"); return; }
            if (base.resource == null) { Diagnostics.Show("GEOM resource not found"); return null; }
            if (!(base.resource is meshExpImp.ModelBlocks.GeometryResource)) { Diagnostics.Show("GEOM resource is not a GeometryResource"); return null; }
            meshExpImp.ModelBlocks.GeometryResource geomResource = base.resource as meshExpImp.ModelBlocks.GeometryResource;
            if (geomResource.ChunkEntries.Count != 1) { Diagnostics.Show("GeometryResource should have 1 Chunk, found " + geomResource.ChunkEntries.Count); return null; }
            meshExpImp.ModelBlocks.GEOM geom = geomResource.ChunkEntries[0].RCOLBlock as meshExpImp.ModelBlocks.GEOM;
            if (geom == null) { Diagnostics.Show("GEOM RCOL block not found"); return null; }
            if (geom.Shader == 0) return null;
            MTNF mtnf = geom.Mtnf;
            if (mtnf == null) { Diagnostics.Show("MTNF expected but was null"); return null; }
            ShaderData sd = mtnf.SData.Find(IsNormalMap);
            if (sd == null) { Diagnostics.Show("NormalMap not found"); return null; }
            ElementTextureRef tr = sd as ElementTextureRef;
            if (tr == null) { Diagnostics.Show("NormalMap not an ElementTextureRef"); return null; }
            //This will break when Data stops being a ChunkRef and turns into a plain int...
            int index = -2;
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Data.UnParse(ms);
                ms.Position = 0;
                index = new BinaryReader(ms).ReadInt32();
            }
            if (index < 0) { Diagnostics.Show(String.Format("NormalMap index read {0}, expected >= 0", index)); return null; }
            if (index >= geom.TGIBlocks.Count) { Diagnostics.Show(String.Format("NormalMap index read {0}, expected < {1}", index, geom.TGIBlocks.Count)); return null; }
            //if (rkLookup.ContainsValue(geom.TGIBlocks[index])) { Diagnostics.Show("Already seen NormalMap " + geom.TGIBlocks[index]); }
            //Add("casp.GEOM.NormalMap", geom.TGIBlocks[index]);
            return new DefaultConnection(geom.TGIBlocks[index], geom.TGIBlocks[index], ResourceDataActions.FindWrite,
                "geom.ChunkEntries[0].RCOLBlock.TGIBlocks[" + index + "]");
        }

        private void SlurpKinRCOLTGIBlocks()
        {
            GenericRCOLResource rcol = base.resource as GenericRCOLResource;
            if (rcol != null)
            {
                for (int i = 0; i < rcol.ChunkEntries.Count; i++)
                {
                    if (this.GetChunkType(rcol.ChunkEntries[i].TGIBlock) == ChunkEntryType.Kindred)
                    {
                        base.kindredRCOLChunkKeys.Add(rcol.ChunkEntries[i].TGIBlock);
                    }
                }
            }
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            if (base.includeDDSes)
                return base.SlurpAllRKs();

            this.SlurpKinRCOLTGIBlocks();
            List<IResourceConnection> results = new List<IResourceConnection>();
            IResourceConnection normalMap = GEOM_getNormalMap();
            if (normalMap != null)
                results.Add(normalMap);
            return results;
        }

        public GeometryNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
