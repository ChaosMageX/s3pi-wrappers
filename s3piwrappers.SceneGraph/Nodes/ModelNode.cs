using System.Collections.Generic;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class ModelNode : DefaultNode
    {
        public const uint MODL_TID = 0x01661233;
        public const uint MLOD_TID = 0x01D10F34;

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

        private static bool IsWantedImage(FieldType fieldType)
        {
            return fieldType == FieldType.DiffuseMap ||
                fieldType == FieldType.SpecularMap;
        }

        private List<IResourceConnection> MATD_GetDiffuseSpecular()
        {
            string rootStr = GetContentPathRootName();
            var results = new List<IResourceConnection>();
            var rcol = base.Resource as GenericRCOLResource;
            if (rcol == null)
                return results;
            string absolutePath, absoluteName;
            Diagnostics.Log("MATD_GetDiffuseSpecular: " +
                ResourceGraph.PrintRKTag(base.originalKey, rootStr));
            for (int i = 0; i < rcol.ChunkEntries.Count; i++)
            {
                var matd = rcol.ChunkEntries[i].RCOLBlock as MATD;
                if (matd == null) continue;

                ShaderDataList sdataList;
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
                    var texRef = sdataList[i] as ElementTextureRef;
                    if (texRef != null && IsWantedImage(texRef.Field))
                    {
                        TGIBlock tgiblock = rcol.Resources[texRef.Data.TGIBlockIndex];
                        if (ResourceGraph.IsDDS(tgiblock.ResourceType))
                        {
                            absoluteName = rootStr + ".Resources[" + texRef.Data.TGIBlockIndex + "]";
                            results.Add(new DefaultConnection(tgiblock, tgiblock,
                                                              ResourceDataActions.FindWrite, absoluteName));
                        }
                    }
                    else
                    {
                        var texKey = sdataList[i] as ElementTextureKey;
                        if (texKey != null && IsWantedImage(texKey.Field))
                        {
                            if (ResourceGraph.IsDDS(texKey.Data.ResourceType))
                            {
                                absoluteName = absolutePath + j + "].Data";
                                results.Add(new DefaultConnection(texKey.Data, texKey,
                                                                  ResourceDataActions.FindWrite, absoluteName, rootStr + ".Data"));
                            }
                        }
                    }
                }
            }
            return results;
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            List<IResourceConnection> results = base.SlurpConnections(constraints);
            if (!base.includeDDSes)
                results.AddRange(MATD_GetDiffuseSpecular());
            return results;
        }

        public ModelNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
