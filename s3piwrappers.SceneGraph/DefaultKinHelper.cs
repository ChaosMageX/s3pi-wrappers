using System;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public class DefaultKinHelper : AResourceKinHelper
    {
        private readonly uint resourceType;

        public uint ResourceType
        {
            get { return this.resourceType; }
        }

        public override bool IsKindred(IResourceKey parentKey, IResourceKey key)
        {
            return key.ResourceType == this.resourceType && key.Instance == parentKey.Instance;
        }

        public override void CreateKindredRK(IResourceKey parentKey,
            IResourceKey newParentKey, ref IResourceKey kindredKey)
        {
            kindredKey.ResourceType = this.resourceType;
            kindredKey.Instance = newParentKey.Instance;
        }

        public override IResourceNode CreateKin(IResource resource, IResourceKey originalKey, object constraints)
        {
            return ResourceNodeDealer.GetResource(resource, originalKey);
        }

        public DefaultKinHelper(uint resourceType, string name)
            : base(name)
        {
            this.resourceType = resourceType;
            if (string.IsNullOrWhiteSpace(name))
            {
                this.kinName = string.Concat("0x", resourceType.ToString("X8"));
            }
        }
    }
}