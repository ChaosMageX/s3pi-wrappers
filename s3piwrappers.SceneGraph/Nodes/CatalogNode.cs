using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph.Nodes
{
    /// <summary>
    ///   Handles Modular Catalog Resources, Fireplaces,
    ///   and all Catalog Resources that only need thumbnails.
    /// </summary>
    public class CatalogNode : DefaultNode
    {
        public static readonly uint OBJD_TID = 0x319E4F1D;
        public static readonly uint CFIR_TID = (uint) CatalogType.CatalogFireplace;
        public static readonly uint MDLR_TID = (uint) CatalogType.ModularResource;

        public static readonly uint[] thumbAndModularTIDs = new[]
            {
                (uint) CatalogType.CatalogProxyProduct,
                (uint) CatalogType.CatalogFountainPool,
                (uint) CatalogType.CatalogFoundation,
                (uint) CatalogType.CatalogWallStyle,
                (uint) CatalogType.CatalogRoofStyle,
                (uint) CatalogType.CatalogFireplace,
                (uint) CatalogType.ModularResource
            };

        public enum Mode
        {
            None = 0,
            FromGame,
            FromUser,
        }

        protected Mode mode = Mode.None;

        /// <summary>
        ///   Determines whether or not attempt find in the FileTable 
        ///   the resources referenced in the catalog resource's data
        ///   before the ResourceGraph tests for their existence.
        /// </summary>
        protected bool preTestResources = false;

        protected bool wantThumbs = false;
        protected bool justSelf = false;
        protected bool isDeepClone = true;

        public CatalogNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        private List<IResourceConnection> Item_findObjds()
        {
            Diagnostics.Log("Item_findObjds");
            var results = new List<IResourceConnection>();
            var ltgi = (TGIBlockList) base.Resource["TGIBlocks"].Value;
            TGIBlock tgi;
            var builder = new StringBuilder();
            bool addOBJD;
            int missing = 0;
            for (int i = 0; i < ltgi.Count; i++)
            {
                tgi = ltgi[i];
                if (tgi.ResourceType != OBJD_TID) continue;
                addOBJD = true;
                if (preTestResources)
                {
                    var objd = new SpecificResource(FileTable.GameContent, tgi);
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

        public override string GetContentPathRootName()
        {
            string rootName = Enum.GetName(typeof (CatalogType), base.originalKey.ResourceType);
            if (string.IsNullOrEmpty(rootName))
                return base.GetContentPathRootName();
            return rootName;
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            uint tid = base.originalKey.ResourceType;
            if (tid == MDLR_TID || tid == CFIR_TID)
                return Item_findObjds();
            else
                return null;
        }

        private static readonly THUM.THUMSize[] NeededThumbnailSizes = new[]
            {
                THUM.THUMSize.small,
                THUM.THUMSize.medium,
                THUM.THUMSize.large,
            };

        public override List<IResourceKinHelper> CreateKinHelpers(object constraints)
        {
            var results = new List<IResourceKinHelper>();
            if (wantThumbs && base.originalKey.ResourceType != MDLR_TID)
            {
                var sr = new SpecificResource(FileTable.GameContent, base.originalKey);
                for (int i = 0; i < 3; i++)
                {
                    results.Add(new ThumbnailKinFinder(base.originalKey, base.resource,
                                                       NeededThumbnailSizes[i]));
                }
            }
            return results;
        }
    }
}
