using System;
using System.Collections.Generic;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// Used by ResourceNodeDealer to identify "interesting" classes and assemblies.
    /// The class maps implementers of AResourceNode to uint representations of ResourceType.
    /// Hence each "node" assembly can contain multiple resource node types (Type key), 
    /// each of which supports one or more ResourceTypes (uint[] value).
    /// The single AResourceNodeHandler implementation summarizes what the assembly provides.
    /// </summary>
    public abstract class AResourceNodeHandler : Dictionary<Type, uint[]>,
        IResourceNodeHandler
    {
        /*public new virtual void Add(Type key, List<uint> value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (!typeof(IResourceNode).IsAssignableFrom(key))
                throw new InvalidOperationException(
                    "Only types that implement IResourceNode can be added");
            base.Add(key, value);
        }/**/
    }
}