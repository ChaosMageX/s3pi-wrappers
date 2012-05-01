using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class CASPartNode : CatalogNode
    {
        public const uint CASP_TID = (uint)CatalogType.CAS_Part;

        public CASPartNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        protected override bool ICanSlurpRK(IResourceKey key)
        {
            return key.ResourceType != 0x034AEECB //CASP
                && base.isDeepClone || (
                   key.ResourceType != 0x00B2D882 //_IMG
                && key.ResourceType != 0x0333406C //_XML
                && key.ResourceType != 0x8FFB80F6 //_DDS
                );
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            return base.SlurpAllRKs();
        }

        public sealed class PresetXmlKinHelper : AResourceKinHelper
        {
            public const uint XML_TID = 0x0333406C;

            public override bool IsKindred(IResourceKey parentKey, IResourceKey key)
            {
                return key.ResourceType == XML_TID
                    && key.Instance == parentKey.Instance;
            }

            public override void CreateKindredRK(IResourceKey parentKey, 
                IResourceKey newParentKey, ref IResourceKey kindredKey)
            {
                kindredKey.ResourceType = XML_TID;
                kindredKey.Instance = newParentKey.Instance;
            }

            public override IResourceNode CreateKin(IResource resource, 
                IResourceKey originalKey, object constraints)
            {
                return new CASPresetXmlNode(resource, originalKey);
            }

            public PresetXmlKinHelper() : base("casp.PresetXML") { }
        }

        public override List<IResourceKinHelper> CreateKinHelpers(object constraints)
        {
            List<IResourceKinHelper> results = base.CreateKinHelpers(constraints);
            results.Add(new PresetXmlKinHelper());
            return results;
        }
    }
}
