using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceNode : IResourceNode
    {
        protected readonly IResourceKey originalKey;
        protected IResource resource;

        public IResourceKey OriginalKey
        {
            get
            {
                // Cloned to prevent accidentally changing the internal values
                return new TGIBlock(0, null, this.originalKey); 
            }
        }

        public IResource Resource
        {
            get { return this.resource; }
        }

        public virtual bool SetRK(IResourceKey newKey, IResourceKey originalKey)
        {
            return true;
        }

        public virtual bool CommitChanges()
        {
            return true;
        }

        public abstract List<IResourceConnection> SlurpConnections(object constraints);

        public virtual List<IResourceKinHelper> CreateKinHelpers(object constraints)
        {
            return null;
        }

        public virtual List<IResourceStblHandle> SlurpStblHandles(object constraints)
        {
            return null;
        }

        public AResourceNode(IResource resource, IResourceKey originalKey)
        {
            this.resource = resource;
        }
    }
}
