using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceKinHelper : IResourceKinHelper
    {
        protected string kinName;

        public string KinName
        {
            get { return this.kinName; }
        }

        public virtual bool IsKindred(IResourceKey parentKey, IResourceKey key)
        {
            return key.Instance == parentKey.Instance;
        }

        public virtual void CreateKindredRK(IResourceKey parentKey,
            IResourceKey newParentKey, ref IResourceKey kindredKey)
        {
            kindredKey.Instance = newParentKey.Instance;
        }

        public abstract IResourceNode CreateKin(IResource resource, 
            IResourceKey originalKey, object constraints);

        public AResourceKinHelper(string name)
        {
            this.kinName = name;
        }
    }
}
