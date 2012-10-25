using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class CatalogObjectNode : CatalogNode
    {
        public static readonly uint[] objectTIDs = new[]
            {
                (uint) CatalogType.CatalogFence,
                (uint) CatalogType.CatalogStairs,
                (uint) CatalogType.CatalogRailing,
                (uint) CatalogType.CatalogRoofPattern,
                (uint) CatalogType.CatalogObject,
                (uint) CatalogType.CatalogWallFloorPattern
            };

        protected bool isFix = false;

        public CatalogObjectNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        protected override bool ICanSlurpRK(IResourceKey key)
        {
            // From Catlg_removeRefdCatlgs
            return (!Enum.IsDefined(typeof (CatalogType), key.ResourceType) ||
                key.Instance == base.originalKey.Instance) &&
                (base.includeDDSes || !ResourceGraph.IsDDS(key.ResourceType));
        }

        private List<IResourceConnection> Catlg_getVPXYs()
        {
            Diagnostics.Log("Catlg_getVPXYs");
            string rootName = GetContentPathRootName();
            var results = new List<IResourceConnection>();
            var ltgi = (TGIBlockList) base.Resource["TGIBlocks"].Value;
            TGIBlock tgi;
            var builder = new StringBuilder();
            bool addVPXY;
            int i, found = 0;
            for (i = 0; i < ltgi.Count; i++)
            {
                tgi = ltgi[i];
                if (tgi.ResourceType != VisualProxyNode.VPXY_TID) continue;
                addVPXY = true;
                if (preTestResources)
                {
                    var vpxy = new SpecificResource(FileTable.GameContent, tgi);
                    if (vpxy.Resource == null)
                    {
                        builder.AppendFormat("Catalog Resource {0} -> RK {1}: not found\n", base.originalKey, tgi);
                        addVPXY = false;
                    }
                }
                if (addVPXY)
                {
                    results.Add(new DefaultConnection(tgi, tgi, ResourceDataActions.FindWrite,
                                                      rootName + ".TGIBlocks[" + i + "]"));
                    found++;
                }
            }
            if (preTestResources)
                Diagnostics.Show(builder.ToString(), "Missing VPXYs");
            if (found == 0)
            {
                Diagnostics.Show(String.Format("Catalog Resource {0} has no VPXY items", base.originalKey), "No VPXY items");
                return null;
            }
            return results;
        }

        #region OBJD Steps

        private IResourceConnection OBJD_setFallback()
        {
            Diagnostics.Log("OBJD_setFallback");
            if ((base.originalKey.ResourceGroup >> 27) > 0) return null; // Only base game objects

            var fallbackIndex = (int) (uint) base.Resource["FallbackIndex"].Value;
            var tgiBlocks = base.Resource["TGIBlocks"].Value as TGIBlockList;
            TGIBlock tgi = tgiBlocks[fallbackIndex];
            if (tgi.Equals(RK.NULL))
            {
                fallbackIndex = tgiBlocks.Count;
                base.Resource["FallbackIndex"] = new TypedValue(typeof (uint), (uint) fallbackIndex, "X");
                tgiBlocks.Add(new TGIBlock(0, null, base.originalKey.ResourceType,
                                           base.originalKey.ResourceGroup, base.originalKey.Instance));
                tgi = tgiBlocks[fallbackIndex];
                Diagnostics.Log("OBJD_setFallback: FallbackIndex: 0x" + fallbackIndex.ToString("X2") + ", Resourcekey: " + tgi);
            }
            return new DefaultConnection(tgi, tgi, ResourceDataActions.FindWrite, "objd.TGIBlocks[" + fallbackIndex + "]");
        }

        private IResourceConnection OBJD_getOBJK()
        {
            Diagnostics.Log("OBJD_getOBJK");
            var index = (uint) base.Resource["OBJKIndex"].Value;
            var ltgi = (IList<TGIBlock>) base.Resource["TGIBlocks"].Value;
            TGIBlock objkTGI = ltgi[(int) index];
            if (preTestResources)
            {
                var objkItem = new SpecificResource(FileTable.GameContent, objkTGI);
                if (objkItem == null || objkItem.ResourceIndexEntry == null)
                {
                    Diagnostics.Show(String.Format("OBJK {0} -> OBJK {1}: not found\n", base.originalKey, objkTGI),
                                     "Missing OBJK");
                    return null;
                }
                else
                {
                    Diagnostics.Log(String.Format("OBJD_getOBJK: Found {0}", objkItem.LongName));
                }
            }
            return new DefaultConnection(objkTGI, objkTGI, ResourceDataActions.FindWrite,
                                         "objd.TGIBlocks[" + (int) index + "]");
        }

        private List<IResourceConnection> OBJD_SlurpDDSes()
        {
            Diagnostics.Log("OBJD_SlurpDDSes");
            string rootStr = GetContentPathRootName();
            var results = new List<IResourceConnection>();
            var ltgi = (IList<TGIBlock>) base.Resource["TGIBlocks"].Value;
            var mtdoors = (IList) base.Resource["MTDoors"].Value;
            AApiVersionedFields mtdoor;
            int i, index;
            for (i = 0; i < mtdoors.Count; i++)
            {
                mtdoor = mtdoors[i] as AApiVersionedFields;
                if (mtdoor != null)
                {
                    index = (int) (uint) mtdoor["WallMaskIndex"].Value;
                    results.Add(new DefaultConnection(ltgi[index], ltgi[index],
                                                      ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));
                }
            }
            index = (int) (uint) base.Resource["SurfaceCutoutDDSIndex"].Value; //sinkmask
            results.Add(new DefaultConnection(ltgi[index], ltgi[index],
                                              ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));

            if (base.Resource.ContentFields.Contains("FloorCutoutDDSIndex"))
            {
                index = (int) (uint) base.Resource["FloorCutoutDDSIndex"].Value; //tubmask
                results.Add(new DefaultConnection(ltgi[index], ltgi[index],
                                                  ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));
            }
            return results;
        }

        #endregion

        private static readonly string[] complateOverrideVariables = new[]
            {
                "Multiplier", "Mask", "Specular", "Overlay",
                "Stencil A", "Stencil B", "Stencil C", "Stencil D",
            };

        private List<IResourceConnection> Catlg_IncludePresets()
        {
            Diagnostics.Log("Catlg_IncludePresets");
            string rootStr = GetContentPathRootName();
            var results = new List<IResourceConnection>();
            if (!base.Resource.ContentFields.Contains("Materials"))
            {
                Diagnostics.Log("Catlg_IncludePresets - field not found");
                return results;
            }
            int i = 0;
            int j, tgiIndex, count = complateOverrideVariables.Length;
            var materials = (IEnumerable) base.Resource["Materials"].Value;
            foreach (CatalogResource.CatalogResource.Material material in materials)
            {
                foreach (CatalogResource.CatalogResource.ComplateElement complateOverride in material.MaterialBlock.ComplateOverrides)
                {
                    tgiIndex = -1;
                    for (j = 0; j < count && tgiIndex < 0; j++)
                    {
                        if (complateOverride.VariableName == complateOverrideVariables[j])
                        {
                            tgiIndex = (Byte) complateOverride["TGIIndex"].Value;
                            TGIBlock rk = material.TGIBlocks[tgiIndex];
                            results.Add(new DefaultConnection(rk, rk, ResourceDataActions.FindWrite,
                                                              rootStr + ".Materials[" + i + "].TGIBlocks[" + tgiIndex + "]"));
                        }
                    }
                }
                i++;
            }
            return results;
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            List<IResourceConnection> results = null;
            if (!base.justSelf)
            {
                if (base.isDeepClone)
                {
                    results = base.SlurpAllRKs();
                }
                else
                {
                    results = Catlg_IncludePresets();
                    if (base.originalKey.ResourceType != OBJD_TID)
                    {
                        List<IResourceConnection> vpxys = Catlg_getVPXYs();
                        if (vpxys != null)
                            results.AddRange(vpxys);
                    }
                    else
                    {
                        if (!isFix && base.mode == Mode.FromGame)
                        {
                            results.Add(OBJD_setFallback());
                        }
                        IResourceConnection objk = OBJD_getOBJK();
                        if (objk != null)
                        {
                            results.Add(objk);
                            results.AddRange(OBJD_SlurpDDSes());
                        }
                    }
                }
            }
            return results;
        }
    }
}
