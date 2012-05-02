using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// This semi-static interface is used to find kindred resources
    /// of a specified parent resource based on their resource keys.
    /// This is often used for multiple resources and its behavior
    /// is determined by its implementation rather than its instance.
    /// </summary>
    public interface IResourceKinHelper
    {
        /// <summary>
        /// A string describing the type of kindred resources 
        /// resolved and created by this helper.
        /// </summary>
        string KinName { get; }

        /// <summary>
        /// Whether or not the kindred resources
        /// resolved and created by this helper
        /// are DDS images or should be treated as such
        /// when searching the FileTable for each kindred resource's data
        /// that will be passed to this helper's 
        /// <see cref="CreateKin"/> function.
        /// </summary>
        bool IsKinDDS { get; }

        /// <summary>
        /// Whether or not the kindred resources
        /// resolved and created by this helper
        /// are thumbnail images or should be treated as such
        /// when searching the FileTable for each kindred resource's data
        /// that will be passed to this helper's 
        /// <see cref="CreateKin"/> function.
        /// </summary>
        bool IsKinThum { get; }

        /// <summary>
        /// Tests whether a specified resource key is kindred to
        /// a specified parent resource.
        /// </summary>
        /// <remarks>
        /// This function is used to find resources in the FileTable
        /// that are not referenced in this node's internal resource data,
        /// but are related to this node based on their resource keys.
        /// </remarks>
        bool IsKindred(IResourceKey parentKey, IResourceKey key);

        /// <summary>
        /// Alters the given kindred resource key based on 
        /// the specified old and new parent resource keys.
        /// </summary>
        void CreateKindredRK(IResourceKey parentKey, 
            IResourceKey newParentKey, ref IResourceKey kindredKey);

        /// <summary>
        /// This function either clones the resource
        /// from the provided original or creates a new one from scratch,
        /// subject to the specified constraints, if any.
        /// </summary>
        /// <param name="resource">The resource 
        /// created by the <see cref="s3pi.WrapperDealer.WrapperDealer"/>
        /// from data located by the <see cref="T:s3pi.Filetable.FileTable"/>
        /// via the <see cref="OriginalChildKey"/> property.</param>
        /// <param name="constraints">Constraints to control 
        /// how the new resource node is created from the original resource.</param>
        /// <returns>A new resource node instance containing resource
        /// data that will be will be changed by its child connections
        /// and written to a package.</returns>
        IResourceNode CreateKin(IResource resource, 
            IResourceKey originalKey, object constraints);
    }
}
