using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    ///   This interfaces functions exactly like <see cref="IResourceKinHelper" />, 
    ///   except that it is also responsible for gathering the resources
    ///   from the FileTable.  This is used when only a certain number of kin
    ///   are to be returned, and certain kin take priority over others
    /// </summary>
    public interface IResourceKinFinder : IResourceKinHelper
    {
        List<SpecificResource> FindKindredResources(IResourceKey parentKey);
    }
}
