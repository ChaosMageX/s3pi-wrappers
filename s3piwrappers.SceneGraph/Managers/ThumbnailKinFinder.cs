using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Managers
{
    public class ThumbnailKinFinder : AResourceKinFinder
    {
        private readonly bool isPNG;
        private readonly ulong thumInstance;
        private readonly uint thumType;
        private readonly bool isCWAL;

        public override bool IsKinThum
        {
            get { return isCWAL || !isPNG; }
        }

        public override bool IsKindred(IResourceKey parentKey, IResourceKey key)
        {
            if (thumInstance != 0)
            {
                return key.ResourceType == thumType
                    && key.Instance == thumInstance
                    && (!isCWAL || (key.ResourceGroup & 0x00FFFFFF) > 0);
            }
            return key.ResourceType == thumType
                && key.Instance == parentKey.Instance
                && (!isCWAL || (key.ResourceGroup & 0x00FFFFFF) > 0);
        }

        public override void CreateKindredRK(IResourceKey parentKey,
                                             IResourceKey newParentKey, ref IResourceKey kindredKey)
        {
            kindredKey.ResourceType = thumType;
            kindredKey.ResourceGroup = (uint) (isCWAL ? 1 : 0);
            kindredKey.Instance = newParentKey.Instance;
        }

        public override IResourceNode CreateKin(IResource resource,
                                                IResourceKey originalKey, object constraints)
        {
            return new DefaultNode(resource, originalKey);
        }

        public override List<SpecificResource> FindKindredResources(IResourceKey parentKey)
        {
            if (thumType == 0)
                return null;
            if (isCWAL)
                return ResourceGraph.SlurpKindredResources(parentKey, this);

            var results = new List<SpecificResource>();
            SpecificResource sr = THUM.getItem(isPNG,
                                               thumInstance != 0 ? thumInstance : parentKey.Instance,
                                               thumType);
            if (sr != null && sr.Resource != null)
                results.Add(sr);
            return results;
        }

        private void CompleteKinName()
        {
            if (isPNG)
                base.kinName += "PNG";
            else if (CatalogType.CatalogRoofPattern == (CatalogType) thumType)
                base.kinName += "Icon";
            else
                base.kinName += "Thumb";
        }

        public ThumbnailKinFinder(uint parentType, THUM.THUMSize size, bool isPNGInstance)
            : base(size.ToString())
        {
            isPNG = isPNGInstance;
            thumType = THUM.getThumbType(parentType, size, isPNGInstance);
            isCWAL = parentType == 0x515CA4CD;
            CompleteKinName();
        }

        public ThumbnailKinFinder(IResourceKey parentKey, IResource parent, THUM.THUMSize size)
            : base(size.ToString())
        {
            isPNG = false;
            if (THUM.CType(parentKey) == CatalogType.ModularResource)
            {
                thumType = 0;
            }
            else if (THUM.CType(parentKey) == CatalogType.CAS_Part)
            {
                thumType = THUM.getThumbType(parentKey.ResourceType, size, false);
            }
            else
            {
                thumInstance = (parent != null) ?
                                                    (ulong) parent["CommonBlock.PngInstance"].Value : 0;
                isPNG = thumInstance != 0;
                thumType = THUM.getThumbType(parentKey.ResourceType, size, isPNG);
            }
            CompleteKinName();
        }
    }
}
