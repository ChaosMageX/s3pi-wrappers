using s3pi.Extensions;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public class DefaultKinHelper : AResourceKinHelper
    {
        protected readonly uint resourceType;
        protected readonly bool isKinDDS;
        protected readonly bool isKinThum;

        public uint ResourceType
        {
            get { return resourceType; }
        }

        public override bool IsKinDDS
        {
            get { return isKinDDS; }
        }

        public override bool IsKinThum
        {
            get { return isKinThum; }
        }

        public override bool IsKindred(IResourceKey parentKey, IResourceKey key)
        {
            return key.ResourceType == resourceType && key.Instance == parentKey.Instance;
        }

        public override void CreateKindredRK(IResourceKey parentKey,
                                             IResourceKey newParentKey, ref IResourceKey kindredKey)
        {
            kindredKey.ResourceType = resourceType;
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
                try
                {
                    kinName = ExtList.Ext[resourceType][0];
                }
                catch
                {
                    kinName = string.Concat("0x", resourceType.ToString("X8"));
                }
                if (string.IsNullOrWhiteSpace(kinName))
                {
                    kinName = string.Concat("0x", resourceType.ToString("X8"));
                }
            }
            isKinDDS = ResourceGraph.IsDDS(resourceType);
            isKinThum = ResourceGraph.IsThum(resourceType);
        }
    }
}
