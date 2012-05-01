using System;
using System.Collections.Generic;
using s3pi.Interfaces;
using s3pi.Filetable;

namespace s3piwrappers.SceneGraph.Managers
{
    public class ThumbnailKinFinder : AResourceKinFinder
    {
        private bool isPNG;
        private ulong thumInstance = 0;
        private uint thumType;
        private bool isCWAL;

        public override bool IsKindred(IResourceKey parentKey, IResourceKey key)
        {
            if (this.thumInstance != 0)
            {
                return key.ResourceType == this.thumType
                    && key.Instance == this.thumInstance
                    && (!this.isCWAL || (key.ResourceGroup & 0x00FFFFFF) > 0);
            }
            return key.ResourceType == this.thumType
                && key.Instance == parentKey.Instance
                && (!this.isCWAL || (key.ResourceGroup & 0x00FFFFFF) > 0);
        }

        public override void CreateKindredRK(IResourceKey parentKey,
            IResourceKey newParentKey, ref IResourceKey kindredKey)
        {
            kindredKey.ResourceType = this.thumType;
            kindredKey.ResourceGroup = (uint)(this.isCWAL ? 1 : 0);
            kindredKey.Instance = newParentKey.Instance;
        }

        public override IResourceNode CreateKin(IResource resource,
            IResourceKey originalKey, object constraints)
        {
            return new DefaultNode(resource, originalKey);
        }

        public override List<SpecificResource> FindKindredResources(IResourceKey parentKey)
        {
            if (this.thumType == 0)
                return null;
            if (this.isCWAL)
                return ResourceGraph.SlurpKindredResources(parentKey, this);

            List<SpecificResource> results = new List<SpecificResource>();
            SpecificResource sr = THUM.getItem(this.isPNG,
                this.thumInstance != 0 ? this.thumInstance : parentKey.Instance, this.thumType);
            if (sr != null && sr.Resource != null)
                results.Add(sr);
            return results;
        }

        private void CompleteKinName()
        {
            if (this.isPNG)
                base.kinName += "PNG";
            else if (CatalogType.CatalogRoofPattern == (CatalogType)this.thumType)
                base.kinName += "Icon";
            else
                base.kinName += "Thumb";
        }

        public ThumbnailKinFinder(uint parentType, THUM.THUMSize size, bool isPNGInstance)
            : base(size.ToString())
        {
            this.isPNG = isPNGInstance;
            this.thumType = THUM.getThumbType(parentType, size, isPNGInstance);
            this.isCWAL = parentType == 0x515CA4CD;
            this.CompleteKinName();
        }

        public ThumbnailKinFinder(IResourceKey parentKey, IResource parent, THUM.THUMSize size)
            : base(size.ToString())
        {
            this.isPNG = false;
            if (THUM.CType(parentKey) == CatalogType.ModularResource)
            {
                this.thumType = 0;
            }
            else if (THUM.CType(parentKey) == CatalogType.CAS_Part)
            {
                this.thumType = THUM.getThumbType(parentKey.ResourceType, size, false);
            }
            else
            {
                this.thumInstance = (parent != null) ?
                    (ulong)parent["CommonBlock.PngInstance"].Value : 0;
                this.isPNG = this.thumInstance != 0;
                this.thumType = THUM.getThumbType(parentKey.ResourceType, size, this.isPNG);
            }
            this.CompleteKinName();
        }
    }
}