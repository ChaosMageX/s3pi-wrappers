using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// The purpose of a resource connection is
    /// to contain and manage code that alters
    /// the internal resource data of its parent
    /// <see cref="IResourceNode"/> that created it.
    /// </summary>
    public interface IResourceConnection
    {
        /// <summary>
        /// The content field path within the parent resource data
        /// (as an <see cref="T:s3pi.Interfaces.AApiVersionedFields"/>)
        /// to the content field containing the resource key reference
        /// that this connection represents.
        /// </summary>
        /// <remarks>
        /// This is currently used only for diagnostics,
        /// but it should still follow the Content Field Path syntax rules,
        /// in case it is ever used by code to directly access the field.
        /// </remarks>
        string AbsolutePath { get; }

        /// <summary>
        /// The original resource key of the child resource,
        /// which is used to find and clone the resource 
        /// from the <see cref="T:s3pi.Filetable.FileTable"/>,
        /// and as a basis for creating the new resource key.
        /// </summary>
        IResourceKey OriginalChildKey { get; }

        /// <summary>
        /// Whether or not the child resource of this connection
        /// is a DDS image or should be treated as one
        /// when searching the FileTable for child resource's data
        /// that will be passed to this connection's 
        /// <see cref="CreateChild"/> function.
        /// </summary>
        bool IsChildDDS { get; }

        /// <summary>
        /// Whether or not the child resource of this connection
        /// is a thumbnail image or should be treated as one
        /// when searching the FileTable for child resource's data
        /// that will be passed to this connection's 
        /// <see cref="CreateChild"/> function.
        /// </summary>
        bool IsChildThum { get; }

        /// <summary><para>
        /// Whether or not the child resource node of this connection
        /// is actually an internal RCOL block within the parent
        /// node's resource data, 
        /// but still needs to have a new resource key generated
        /// for the ChunkEntry's TGIBlock.
        /// </para><para>
        /// If true, the ResourceGraph will pass a null 
        /// <see cref="T:s3pi.Interfaces.IResource"/> instance 
        /// to the <see cref="CreateChild"/> function.
        /// </para></summary>
        /// <remarks>
        /// This is mainly used to determine whether or not the 
        /// ResourceGraph searches the FileTable for a resource
        /// with the <see cref="OriginalChildKey"/>.
        /// </remarks>
        bool IsChildRCOLBlock { get; }

        /// <summary>
        /// Whether or not a new child resource node is created
        /// when the <see cref="ResourceGraph"/> finds
        /// an already existing resource node with the same child key.
        /// This should only return true for exceptional cases.
        /// </summary>
        bool AlwaysCreateChild { get; }

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
        IResourceNode CreateChild(IResource resource, object constraints);

        /// <summary>
        /// This is the key function of a resource connection,
        /// which writes the given resource key to the parent
        /// node's internal resource data.
        /// </summary>
        bool SetParentReferenceRK(IResourceKey newKey);
    }
}
