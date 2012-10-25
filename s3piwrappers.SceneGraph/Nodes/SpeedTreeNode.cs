using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class SpeedTreeNode : DefaultNode
    {
        public const uint _SPT_TID = 0x00B552EA;
        public const uint SPT2_TID = 0x021D7E8C;

        public override string GetContentPathRootName()
        {
            switch (base.originalKey.ResourceType)
            {
            case _SPT_TID:
                return "spt";
            case SPT2_TID:
                return "spt2";
            default:
                return base.GetContentPathRootName();
            }
        }

        protected override uint[] GetKindredResourceTypes(out string[] kinNames)
        {
            switch (base.originalKey.ResourceType)
            {
            case _SPT_TID:
                kinNames = new[] {"spt2"};
                return new[] {SPT2_TID};
            case SPT2_TID:
                kinNames = new[] {"spt"};
                return new[] {_SPT_TID};
            default:
                kinNames = null;
                return null;
            }
        }

        public SpeedTreeNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
