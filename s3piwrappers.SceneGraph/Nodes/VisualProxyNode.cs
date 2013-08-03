using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class VisualProxyNode : DefaultNode
    {
        public const uint VPXY_TID = 0x736884F1;

        private static readonly uint[] kinTIDs = new uint[]
        {
            0x02019972, // MTST
            0x0333406C  // PresetXML
        };

        public override string GetContentPathRootName()
        {
            return "vpxy";
        }

        private static readonly string[] kinLogNames = new string[]
        {
            "MTST", "PresetXML"
        };

        protected override uint[] GetKindredResourceTypes(out string[] kinNames)
        {
            kinNames = kinLogNames;
            return kinTIDs;
        }

        public VisualProxyNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
