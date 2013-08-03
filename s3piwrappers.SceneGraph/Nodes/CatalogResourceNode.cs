using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using s3pi.Filetable;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph.Nodes
{
    // TODO: Ask: What is the point of OBJD_SlurpDDSes()?
    // Wouldn't all DDSes already be slurped up by Catlg_SlurpRKs(),
    // since it calls SlurpRKsFromField with includeDDSes = true ?
    public class CatalogResourceNode : DefaultNode
    {
        public const uint OBJD_TID = 0x319E4F1D;

        public enum Mode
        {
            None = 0,
            FromGame,
            FromUser,
        }
        private Mode mode = Mode.None;

        private bool preTestResources = false;
        private bool isFix = false;
        private bool isDeepClone = false;
        private bool justSelf = false;
        private bool wantThumbs = false;

        #region Step Lists
        private List<IResourceConnection> SetStepList()
        {
            switch ((CatalogType)base.originalKey.ResourceType)
            {
                case CatalogType.CatalogProxyProduct:
                case CatalogType.CatalogFountainPool:
                case CatalogType.CatalogFoundation:
                case CatalogType.CatalogWallStyle:
                case CatalogType.CatalogRoofStyle:
                    return ThumbnailsOnly_Steps();
                case CatalogType.CatalogTerrainGeometryBrush:
                case CatalogType.CatalogTerrainWaterBrush:
                    return brush_Steps();
                case CatalogType.CatalogTerrainPaintBrush:
                    return CTPT_Steps();

                case CatalogType.CatalogFence:
                case CatalogType.CatalogStairs:
                case CatalogType.CatalogRailing:
                case CatalogType.CatalogRoofPattern:
                    return CatlgHasVPXY_Steps();
                case CatalogType.CatalogObject:
                    return OBJD_Steps();
                case CatalogType.CatalogWallFloorPattern:
                    return CWAL_Steps();

                case CatalogType.CatalogFireplace:
                    return CFIR_Steps();
                case CatalogType.ModularResource:
                    return MDLR_Steps();

                case CatalogType.CAS_Part:
                    return CASP_Steps();

                default:
                    return null;
            }
        }

        private List<IResourceConnection> ThumbnailsOnly_Steps()
        {
            Diagnostics.Log("ThumbnailsOnly_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            if (!justSelf)
            {
                //if (wantThumbs)
                //    results.AddRange(SlurpThumbnails());
            }
            return results;
        }

        private List<IResourceConnection> brush_Steps()
        {
            Diagnostics.Log("brush_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            if (!justSelf)
            {
                if (isDeepClone)
                {
                    results.Add(brush_addBrushShape());
                }
                else
                {
                }
            }
            results.AddRange(ThumbnailsOnly_Steps());
            return results;
        }

        private List<IResourceConnection> CTPT_Steps()
        {
            Diagnostics.Log("CTPT_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            if (!justSelf)
            {
                results.Add(CTPT_addPair());
                results.Add(CTPT_addBrushTexture());
            }
            results.AddRange(brush_Steps());
            return results;
        }

        private List<IResourceConnection> Catlg_Steps()
        {
            List<IResourceConnection> results = new List<IResourceConnection>();
            if (!justSelf)
            {
                if (!isDeepClone)
                    results.AddRange(Catlg_IncludePresets());
                else
                {
                    results.AddRange(base.SlurpAllRKs());
                    // ICanSlurpRK predicate takes care of this
                    //Catlg_removeRefdCatlgs(results);
                }
            }
            return results;
        }

        private List<IResourceConnection> CatlgHasVPXY_Steps()
        {
            Diagnostics.Log("CatlgHasVPXY_Steps");
            List<IResourceConnection> subResults, results = new List<IResourceConnection>();
            results.AddRange(Catlg_Steps());
            if (!justSelf)
            {
                subResults = Catlg_getVPXYs();
                if (subResults != null)
                {
                    results.AddRange(subResults);
                    //if (wantThumbs)
                    //    results.AddRange(SlurpThumbnails());
                }
            }
            return results;
        }

        private List<IResourceConnection> OBJD_Steps()
        {
            Diagnostics.Log("OBJD_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            IResourceConnection subResult;
            results.AddRange(Catlg_Steps());
            if (!justSelf)
            {
                if (!isFix && mode == Mode.FromGame)
                {
                    results.Add(OBJD_setFallback());
                }
                subResult = OBJD_getOBJK();
                if (subResult != null)
                {
                    results.Add(subResult);
                    if (isDeepClone)
                    {
                        // WARNING: S3OC apparently only clones the OBJK on deep clones,
                        // but this library will always end up cloning it.
                        // If it's a big deal, then the OBJK node 
                        // could be treated as an RCOL block to prevent it from being
                        // written to the final package.
                    }
                    else
                    {
                        results.AddRange(OBJD_SlurpDDSes());
                    }
                    //if (wantThumbs)
                    //    results.AddRange(SlurpThumbnails());
                }
            }
            return results;
        }

        private List<IResourceConnection> CWAL_Steps()
        {
            Diagnostics.Log("CWAL_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            results.AddRange(Catlg_Steps());
            if (!justSelf)
            {
                results.AddRange(Catlg_getVPXYs());
                if (isDeepClone)
                {
                }
                else
                {
                }
                //if (wantThumbs)
                //    results.AddRange(CWAL_SlurpThumbnails());
            }
            return results;
        }

        private List<IResourceConnection> CFIR_Steps()
        {
            Diagnostics.Log("CFIR_Steps");
            List<IResourceConnection> results = new List<IResourceConnection>();
            results.AddRange(Item_findObjds());
            //if (wantThumbs)
            //    results.AddRange(SlurpThumbnails());//For the CFIR itself
            return results;
        }

        private List<IResourceConnection> MDLR_Steps()
        {
            Diagnostics.Log("MDLR_Steps");
            return Item_findObjds();
        }

        private List<IResourceConnection> CASP_Steps()
        {
            List<IResourceConnection> results = new List<IResourceConnection>();

            return results;
        }
        #endregion

        protected override bool ICanSlurpRK(IResourceKey key)
        {
            // From Catlg_removeRefdCatlgs
            return (!Enum.IsDefined(typeof(CatalogType), key.ResourceType) ||
                key.Instance == base.originalKey.Instance) &&
                (base.includeDDSes || !ResourceGraph.IsDDS(key.ResourceType));
        }

        // Any interdependency between catalog resource is handled like CFIR (for complex ones) or CTPT (for simple ones)
        private void Catlg_removeRefdCatlgs(List<IResourceConnection> connections)
        {
            Diagnostics.Log("Catlg_removeRefdCatlgs");
            IList<TGIBlock> ltgi = (IList<TGIBlock>)base.Resource["TGIBlocks"].Value;
            AResourceKey rk;
            int i, j, index, count = ltgi.Count;
            for (i = 0; i < count; i++)
            {
                rk = ltgi[i];
                if (Enum.IsDefined(typeof(CatalogType), rk.ResourceType) && rk.Instance != base.originalKey.Instance)
                {
                    index = -1;
                    for (j = 0; j < connections.Count && index < 0; j++)
                    {
                        if (connections[j].OriginalChildKey.Equals(rk))
                        {
                            index = j;
                        }
                    }
                    if (index >= 0)
                    {
                        string key = ResourceGraph.PrintRKRef(connections[index].OriginalChildKey, 
                            connections[index].AbsolutePath);
                        Diagnostics.Log("Catlg_removeRefdCatlgs: Removed " + key);
                        connections.RemoveAt(index);
                    }
                }
            }
        }

        private List<IResourceConnection> Catlg_getVPXYs()
        {
            Diagnostics.Log("Catlg_getVPXYs");
            List<IResourceConnection> results = new List<IResourceConnection>();
            TGIBlockList ltgi = (TGIBlockList)base.Resource["TGIBlocks"].Value;
            TGIBlock tgi;
            StringBuilder builder = new StringBuilder();
            bool addVPXY;
            int i, found = 0;
            for (i = 0; i < ltgi.Count; i++)
            {
                tgi = ltgi[i];
                if (tgi.ResourceType != VisualProxyNode.VPXY_TID) continue;
                addVPXY = true;
                if (preTestResources)
                {
                    SpecificResource vpxy = new SpecificResource(FileTable.GameContent, tgi);
                    if (vpxy.Resource == null)
                    {
                        builder.AppendFormat("Catalog Resource {0} -> RK {1}: not found\n", base.originalKey, tgi);
                        addVPXY = false;
                    }
                }
                if (addVPXY)
                {
                    results.Add(new DefaultConnection(tgi, tgi, ResourceDataActions.FindWrite, "root.TGIBlocks[" + i + "]"));
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
            if ((base.originalKey.ResourceGroup >> 27) > 0) return null;// Only base game objects

            int fallbackIndex = (int)(uint)base.Resource["FallbackIndex"].Value;
            TGIBlockList tgiBlocks = base.Resource["TGIBlocks"].Value as TGIBlockList;
            TGIBlock tgi = tgiBlocks[fallbackIndex];
            if (tgi.Equals(s3pi.Filetable.RK.NULL))
            {
                fallbackIndex = tgiBlocks.Count;
                base.Resource["FallbackIndex"] = new TypedValue(typeof(uint), (uint)fallbackIndex, "X");
                tgiBlocks.Add(new TGIBlock(0, null, "TGI", base.originalKey.ResourceType, base.originalKey.ResourceGroup, base.originalKey.Instance));
                tgi = tgiBlocks[fallbackIndex];
                Diagnostics.Log("OBJD_setFallback: FallbackIndex: 0x" + fallbackIndex.ToString("X2") + ", Resourcekey: " + tgi);
            }
            return new DefaultConnection(tgi, tgi, ResourceDataActions.FindWrite, "root.TGIBlocks[" + fallbackIndex + "]");
        }
        private IResourceConnection OBJD_getOBJK()
        {
            Diagnostics.Log("OBJD_getOBJK");
            uint index = (uint)base.Resource["OBJKIndex"].Value;
            IList<TGIBlock> ltgi = (IList<TGIBlock>)base.Resource["TGIBlocks"].Value;
            TGIBlock objkTGI = ltgi[(int)index];
            if (preTestResources)
            {
                SpecificResource objkItem = new SpecificResource(FileTable.GameContent, objkTGI);
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
                "root.TGIBlocks[" + (int)index + "]");
        }
        private List<IResourceConnection> OBJD_SlurpDDSes()
        {
            Diagnostics.Log("OBJD_SlurpDDSes");
            string rootStr = "root";
            List<IResourceConnection> results = new List<IResourceConnection>();
            IList<TGIBlock> ltgi = (IList<TGIBlock>)base.Resource["TGIBlocks"].Value;
            IList mtdoors = (IList)base.Resource["MTDoors"].Value;
            AApiVersionedFields mtdoor;
            int i, index;
            for (i = 0; i < mtdoors.Count; i++)
            {
                mtdoor = mtdoors[i] as AApiVersionedFields;
                if (mtdoor != null)
                {
                    index = (int)(uint)mtdoor["WallMaskIndex"].Value;
                    results.Add(new DefaultConnection(ltgi[index], ltgi[index], 
                        ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));
                }
            }
            index = (int)(uint)base.Resource["SurfaceCutoutDDSIndex"].Value;//sinkmask
            results.Add(new DefaultConnection(ltgi[index], ltgi[index], 
                ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));
            
            if (base.Resource.ContentFields.Contains("FloorCutoutDDSIndex"))
            {
                index = (int)(uint)base.Resource["FloorCutoutDDSIndex"].Value;//tubmask
                results.Add(new DefaultConnection(ltgi[index], ltgi[index], 
                    ResourceDataActions.FindWrite, rootStr + ".TGIBlocks[" + index + "]"));
            }
            return results;
        }
        #endregion

        private static Dictionary<ulong, SpecificResource> CTPTBrushIndexToPair
            = new Dictionary<ulong, SpecificResource>();

        private static bool IsCTPT(IResourceIndexEntry rie)
        {
            return (CatalogType)rie.ResourceType == CatalogType.CatalogTerrainPaintBrush;
        }

        private static void FindAllBrushPairs()
        {
            List<PathPackageTuple> ppts = FileTable.GameContent;
            PathPackageTuple ppt;
            int i, j;
            for (i = 0; i < ppts.Count; i++)
            {
                ppt = ppts[i];
                List<IResourceIndexEntry> lrs = ppt.Package.FindAll(IsCTPT);
                for (j = 0; j < lrs.Count; j++)
                {
                    SpecificResource sr = new SpecificResource(ppt, lrs[j]);
                    if (sr.Resource != null)
                    {
                        byte status = (byte)sr.Resource["CommonBlock.BuildBuyProductStatusFlags"].Value;
                        if ((status & 0x01) == 0) // do not list
                        {
                            UInt64 brushIndex = (UInt64)sr.Resource["CommonBlock.NameGUID"].Value;
                            if (!CTPTBrushIndexToPair.ContainsKey(brushIndex))//Try to leave one behind...
                            {
                                CTPTBrushIndexToPair.Add(brushIndex, sr);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private IResourceConnection CTPT_addPair()
        {
            Diagnostics.Log("CTPT_addPair");
            //int brushIndex = ("" + base.Resource["BrushTexture"]).GetHashCode();
            UInt64 brushIndex = (UInt64)base.Resource["CommonBlock.NameGUID"].Value;
            if (CTPTBrushIndexToPair.ContainsKey(brushIndex))
            {
                //Add("ctpt_pair", CTPTBrushIndexToPair[brushIndex].RequestedRK);
            }
            else
            {
                Diagnostics.Show(String.Format("CTPT {0} BrushIndex {1} not found", base.originalKey, brushIndex), "No ctpt_pair item");
            }
            return null;
        }/**/

        private IResourceConnection CTPT_addBrushTexture()
        {
            Diagnostics.Log("CTPT_addBrushTexture");
            //Add("ctpt_BrushTexture", (TGIBlock)base.Resource["BrushTexture"].Value); 
            TGIBlock bt = (TGIBlock)base.Resource["BrushTexture"].Value;
            return new DefaultConnection(bt, bt, ResourceDataActions.FindWrite, "root.BrushTexture");
        }
        private IResourceConnection brush_addBrushShape()
        {
            Diagnostics.Log("brush_addBrushShape");
            //Add("brush_ProfileTexture", (TGIBlock)base.Resource["ProfileTexture"].Value); 
            TGIBlock pt = (TGIBlock)base.Resource["ProfileTexture"].Value;
            return new DefaultConnection(pt, pt, ResourceDataActions.FindWrite, "root.ProfileTexture");
        }

        private static string[] complateOverrideVariables = new string[] {
            "Multiplier", "Mask", "Specular", "Overlay",
            "Stencil A", "Stencil B", "Stencil C", "Stencil D",
        };
        private List<IResourceConnection> Catlg_IncludePresets()
        {
            Diagnostics.Log("Catlg_IncludePresets");
            List<IResourceConnection> results = new List<IResourceConnection>();
            if (!base.Resource.ContentFields.Contains("Materials"))
            {
                Diagnostics.Log("Catlg_IncludePresets - field not found");
                return results;
            }
            int i = 0;
            int j, tgiIndex, count = complateOverrideVariables.Length;
            IEnumerable materials = (IEnumerable)base.Resource["Materials"].Value;
            foreach (CatalogResource.CatalogResource.Material material in materials)
            {
                foreach (var complateOverride in material.MaterialBlock.ComplateOverrides)
                {
                    tgiIndex = -1;
                    for (j = 0; j < count && tgiIndex < 0; j++)
                    {
                        if (complateOverride.VariableName == complateOverrideVariables[j])
                        {
                            tgiIndex = (int)(Byte)complateOverride["TGIIndex"].Value;
                            TGIBlock rk = material.TGIBlocks[tgiIndex];
                            results.Add(new DefaultConnection(rk, rk, ResourceDataActions.FindWrite, 
                                "root.Materials[" + i + "].TGIBlocks[" + tgiIndex + "]"));
                        }
                    }
                }
                i++;
            }
            return results;
        }

        private List<IResourceConnection> Item_findObjds()
        {
            Diagnostics.Log("Item_findObjds");
            List<IResourceConnection> results = new List<IResourceConnection>();
            TGIBlockList ltgi = (TGIBlockList)base.Resource["TGIBlocks"].Value;
            TGIBlock tgi;
            StringBuilder builder = new StringBuilder();
            bool addOBJD;
            int missing = 0;
            for (int i = 0; i < ltgi.Count; i++)
            {
                tgi = ltgi[i];
                if (tgi.ResourceType != OBJD_TID) continue;
                addOBJD = true;
                if (preTestResources)
                {
                    SpecificResource objd = new SpecificResource(FileTable.GameContent, tgi);
                    if (objd.Resource != null)
                    {
                        Diagnostics.Log(String.Format("Item_findObjds: Found {0}", objd.LongName));
                    }
                    else
                    {
                        builder.AppendFormat("OBJD {0}\n", tgi);
                        missing++;
                        addOBJD = false;
                    }
                    if (addOBJD)
                    {
                        results.Add(new DefaultConnection(tgi, tgi, ResourceDataActions.FindWrite, 
                            "root.TGIBlocks[" + i + "]"));
                    }
                }
            }
            if (preTestResources && missing > 0)
            {
                Diagnostics.Show(builder.ToString(), 
                    String.Format("Item {0} has {1} missing OBJDs:", base.originalKey, missing));
            }
            return results;
        }

        public override List<IResourceKinHelper> CreateKinHelpers(object constraints)
        {
            List<IResourceKinHelper> results = new List<IResourceKinHelper>();
            if (wantThumbs && (CatalogType)base.originalKey.ResourceType != CatalogType.ModularResource)
            {
                SpecificResource sr = new SpecificResource(FileTable.GameContent, base.originalKey);
                for (int i = 0; i < 3; i++)
                {
                    results.Add(new ThumbnailKinFinder(base.originalKey, base.resource,
                        NeededThumbnailSizes[i]));
                }
            }
            if ((CatalogType)base.originalKey.ResourceType == CatalogType.CAS_Part)
            {
            }
            return results;
        }

        private static readonly THUM.THUMSize[] NeededThumbnailSizes = new THUM.THUMSize[]
        {
            THUM.THUMSize.small, 
            THUM.THUMSize.medium, 
            THUM.THUMSize.large,
        };
        //Thumbnails for everything but walls
        //PNGs come from :Objects; Icons come from :Images; Thumbs come from :Thumbnails.
        private List<IResourceConnection> SlurpThumbnails()
        {
            Diagnostics.Log("SlurpThumbnails");
            List<IResourceConnection> results = new List<IResourceConnection>();
            /*for (int i = 0; i < 3; i++)
            {
                IResourceKey rk = THUM.getImageRK(NeededThumbnailSizes[i], selectedItem);
                if (THUM.PNGTypes[i] == rk.ResourceType)
                    Add(size + "PNG", rk);
                else if ((CatalogType)base.originalKey.ResourceType == CatalogType.CatalogRoofPattern)
                    Add(size + "Icon", rk);
                else
                    Add(size + "Thumb", rk);
            }/**/
            return results;
        }
        //0x515CA4CD is very different - but they do come from :Thumbnails, at least.
        private List<IResourceConnection> CWAL_SlurpThumbnails()
        {
            Diagnostics.Log("CWAL_SlurpThumbnails");
            List<IResourceConnection> results = new List<IResourceConnection>();
            Dictionary<THUM.THUMSize, uint> CWALThumbTypes = new Dictionary<THUM.THUMSize, uint>();
            CWALThumbTypes.Add(THUM.THUMSize.small, 0x0589DC44);
            CWALThumbTypes.Add(THUM.THUMSize.medium, 0x0589DC45);
            CWALThumbTypes.Add(THUM.THUMSize.large, 0x0589DC46);
            List<IResourceIndexEntry> seen = new List<IResourceIndexEntry>();
            foreach (THUM.THUMSize size in new THUM.THUMSize[] { THUM.THUMSize.small, THUM.THUMSize.medium, THUM.THUMSize.large, })
            {
                //int i = 0;
                uint type = CWALThumbTypes[size];
                foreach (var ppt in FileTable.Thumbnails)
                    foreach (var sr in ppt.FindAll(rie => rie.ResourceType == type && rie.Instance == base.originalKey.Instance))
                    {
                        if (seen.Exists(sr.ResourceIndexEntry.Equals)) continue;
                        //Add(size + "[" + i++ + "]Thumb", sr.RequestedRK);
                    }
            }/**/
            return results;
        }

        public CatalogResourceNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
