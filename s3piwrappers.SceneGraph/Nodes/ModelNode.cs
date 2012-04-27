using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class ModelNode : DefaultNode
    {
        public const uint MODL_TID = 0x01661233;
        public const uint MLOD_TID = 0x01D10F34;

        private static bool IsWantedImage(MATD.FieldType fieldType)
        {
            return fieldType == MATD.FieldType.DiffuseMap ||
                   fieldType == MATD.FieldType.SpecularMap;
        }

        public override string GetContentPathRootName()
        {
            switch (base.originalKey.ResourceType)
            {
                case MODL_TID:
                    return "modl";
                case MLOD_TID:
                    return "mlod";
                default:
                    return base.GetContentPathRootName();
            }
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            base.includeDDSes = true;

            if (base.includeDDSes)
                return base.SlurpConnections(constraints);

            string rootStr = this.GetContentPathRootName();
            base.includeDDSes = false;
            List<IResourceConnection> results = base.SlurpConnections(constraints);
            GenericRCOLResource rcol = base.Resource as GenericRCOLResource;
            if (rcol == null)
                return results;
            string absolutePath, absoluteName;
            for (int i = 0; i < rcol.ChunkEntries.Count; i++)
            {
                MATD matd = rcol.ChunkEntries[i].RCOLBlock as MATD;
                if (matd == null) continue;

                MATD.ShaderDataList sdataList;
                if (matd.ContentFields.Contains("Mtnf"))
                {
                    sdataList = matd.Mtnf.SData;
                    absolutePath = rootStr + ".ChunkEntries[" + i + "].RCOLBlock.Mtnf.SData[";
                }
                else
                {
                    sdataList = matd.Mtrl.SData;
                    absolutePath = rootStr + ".ChunkEntries[" + i + "].RCOLBlock.Mtrl.SData[";
                }

                for (int j = 0; j < sdataList.Count; j++)
                {
                    MATD.ElementTextureRef texRef = sdataList[i] as MATD.ElementTextureRef;
                    if (texRef != null && IsWantedImage(texRef.Field))
                    {
                        TGIBlock tgiblock = rcol.Resources[texRef.Data.TGIBlockIndex];
                        if (ResourceGraph.IsDDS(tgiblock.ResourceType))
                        {
                            absoluteName = rootStr + ".Resources[" + texRef.Data.TGIBlockIndex + "]";
                            results.Add(new DefaultConnection(tgiblock, tgiblock, false, absoluteName));
                        }
                    }
                    else
                    {
                        MATD.ElementTextureKey texKey = sdataList[i] as MATD.ElementTextureKey;
                        if (texKey != null && IsWantedImage(texKey.Field))
                        {
                            if (ResourceGraph.IsDDS(texKey.Data.ResourceType))
                            {
                                absoluteName = absolutePath + j + "].Data";
                                results.Add(new DefaultConnection(texKey.Data, 
                                    texKey, false, absoluteName, rootStr + ".Data"));
                            }
                        }
                    }
                }
            }

            return results;
        }

        public ModelNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
