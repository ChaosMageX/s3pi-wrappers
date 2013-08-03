using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceConnection : IResourceConnection
    {
        protected readonly string absolutePath;
        private readonly IResourceKey originalChildKey;
        protected readonly bool isChildDDS;
        protected readonly bool isChildThum;
        //protected readonly bool isChildRCOLBlock;
        protected readonly ResourceDataActions childDataActions;

        public string AbsolutePath
        {
            get { return this.absolutePath; }
        }

        public IResourceKey OriginalChildKey
        {
            get
            {
                // Cloned to prevent accidentally changing the internal values
                return new TGIBlock(0, null, this.originalChildKey); 
            }
        }

        public bool IsChildDDS
        {
            get { return this.isChildDDS; }
        }

        public bool IsChildThum
        {
            get { return this.isChildThum; }
        }

        /*public bool IsChildRCOLBlock
        {
            get { return this.isChildRCOLBlock; }
        }/**/

        public ResourceDataActions ChildDataActions
        {
            get { return this.childDataActions; }
        }

        public virtual bool AlwaysCreateChild
        {
            get { return false; }
        }

        public abstract IResourceNode CreateChild(IResource resource, object constraints);

        public abstract bool SetParentReferenceRK(IResourceKey newKey);

        public AResourceConnection(IResourceKey childKey, string path, 
            ResourceDataActions childActions)
        {
            this.absolutePath = path;
            this.originalChildKey = childKey;
            this.childDataActions = childActions;
            this.isChildDDS = ResourceGraph.IsDDS(childKey.ResourceType);
            this.isChildThum = ResourceGraph.IsThum(childKey.ResourceType);
        }
    }
}
