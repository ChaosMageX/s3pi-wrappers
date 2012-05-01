using System;
using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceKinFinder : AResourceKinHelper, IResourceKinFinder
    {
        public virtual List<SpecificResource> FindKindredResources(IResourceKey parentKey)
        {
            return ResourceGraph.SlurpKindredResources(parentKey, this);
        }

        public AResourceKinFinder(string name) : base(name) { }
    }
}
