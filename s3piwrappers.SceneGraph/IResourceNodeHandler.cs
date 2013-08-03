using System;
using System.Collections.Generic;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// Used by <see cref="AResourceNodeHandler"/>, which is used by 
    /// <see cref="ResourceNodeDealer"/>
    /// to identify "interesting" classes within assemblies
    /// </summary>
    internal interface IResourceNodeHandler : IDictionary<Type, uint[]>
    {
    }
}