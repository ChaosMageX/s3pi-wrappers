using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class CatalogBrushNode : CatalogNode
    {
        public const uint CTPT_TID
            = (uint) CatalogType.CatalogTerrainPaintBrush;

        public static readonly uint[] brushTIDs = new[]
            {
                (uint) CatalogType.CatalogTerrainGeometryBrush,
                (uint) CatalogType.CatalogTerrainWaterBrush,
                (uint) CatalogType.CatalogTerrainPaintBrush
            };

        #region CTPT Lookup

        // Brushes not shown in catalog
        private static readonly Dictionary<ulong, SpecificResource> CTPTBrushIndexToPair0
            = new Dictionary<ulong, SpecificResource>();

        // Brushes shown in catalog
        private static readonly Dictionary<ulong, SpecificResource> CTPTBrushIndexToPair1
            = new Dictionary<ulong, SpecificResource>();

        private static bool IsCTPT(IResourceIndexEntry rie)
        {
            return rie.ResourceType == CTPT_TID;
        }

        private const byte BBProdStatusShowInCatalog = 0x01;

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
                    var sr = new SpecificResource(ppt, lrs[j]);
                    if (sr.Resource != null)
                    {
                        var status = (byte) sr.Resource["CommonBlock.BuildBuyProductStatusFlags"].Value;
                        var brushIndex = (ulong) sr.Resource["CommonBlock.NameGUID"].Value;
                        if ((status & BBProdStatusShowInCatalog) == 0) // do not list
                        {
                            if (!CTPTBrushIndexToPair0.ContainsKey(brushIndex)) //Try to leave one behind...
                                CTPTBrushIndexToPair0.Add(brushIndex, sr);
                        }
                        else
                        {
                            if (!CTPTBrushIndexToPair1.ContainsKey(brushIndex)) //Try to leave one behind...
                                CTPTBrushIndexToPair1.Add(brushIndex, sr);
                        }
                    }
                }
            }
        }

        public sealed class CTPTConnection : AResourceConnection
        {
            private readonly SpecificResource brushPairCounterPart;

            public override IResourceNode CreateChild(IResource resource, object constraints)
            {
                return new CatalogBrushNode(brushPairCounterPart.Resource, base.OriginalChildKey);
            }

            public override bool SetParentReferenceRK(IResourceKey newKey)
            {
                return true;
            }

            public CTPTConnection(SpecificResource brush)
                : base(brush.ResourceIndexEntry, "CatalogTerrainPaintBrush.CommonBlock.NameGUID",
                       ResourceDataActions.Write)
            {
                brushPairCounterPart = brush;
            }
        }

        #endregion

        public CatalogBrushNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        private List<IResourceConnection> CTPT_addPair()
        {
            Diagnostics.Log("CTPT_addPair");

            var status = (byte) base.resource["CommonBlock.BuildBuyProductStatusFlags"].Value;
            Dictionary<ulong, SpecificResource> indexToPair;
            if ((status & BBProdStatusShowInCatalog) != 0)
                indexToPair = CTPTBrushIndexToPair0;
            else
                indexToPair = CTPTBrushIndexToPair1;

            //int brushIndex = ("" + base.Resource["BrushTexture"]).GetHashCode();
            var brushIndex = (ulong) base.resource["CommonBlock.NameGUID"].Value;
            if (indexToPair.ContainsKey(brushIndex))
            {
                //Add("ctpt_pair", CTPTBrushIndexToPair[brushIndex].RequestedRK);
                var results = new List<IResourceConnection>();
                results.Add(new CTPTConnection(indexToPair[brushIndex]));
                return results;
            }
            else
            {
                Diagnostics.Show(String.Format("CTPT {0} BrushIndex {1} not found",
                                               ResourceGraph.PrintRKTag(base.originalKey, "CatalogTerrainPaintBrush"),
                                               brushIndex), "No ctpt_pair item");
                return new List<IResourceConnection>();
            }
        }

        private IResourceConnection CTPT_addBrushTexture()
        {
            Diagnostics.Log("CTPT_addBrushTexture");
            var bt = (TGIBlock) base.Resource["BrushTexture"].Value;
            return new DefaultConnection(bt, bt, ResourceDataActions.FindWrite, "root.BrushTexture");
        }

        private IResourceConnection brush_addBrushShape()
        {
            Diagnostics.Log("brush_addBrushShape");
            var pt = (TGIBlock) base.Resource["ProfileTexture"].Value;
            return new DefaultConnection(pt, pt, ResourceDataActions.FindWrite, "root.ProfileTexture");
        }

        public override bool CommitChanges()
        {
            if (base.originalKey.ResourceType == CTPT_TID) //Both CTPTs
            {
                var uniqueObject = (string) base.resource["CommonBlock.Name"].Value;

                var status = (byte) base.resource["CommonBlock.BuildBuyProductStatusFlags"].Value;
                uint brushIndex = FNV32.GetHash(uniqueObject) << 1;
                if ((status & BBProdStatusShowInCatalog) != 0)
                    base.resource["CommonBlock.UISortPriority"] = new TypedValue(typeof (uint), brushIndex);
                else
                    base.resource["CommonBlock.UISortPriority"] = new TypedValue(typeof (uint), brushIndex + 1);
            }
            throw new NotImplementedException("TODO: Ensure Name stays the same between both CTPTs");
            //return base.CommitChanges();
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            var results = new List<IResourceConnection>();
            if (!base.justSelf)
            {
                if (base.originalKey.ResourceType == CTPT_TID)
                {
                    results.AddRange(CTPT_addPair());
                    results.Add(CTPT_addBrushTexture());
                }
                if (base.isDeepClone)
                {
                    results.Add(brush_addBrushShape());
                }
            }
            return results;
        }
    }
}
