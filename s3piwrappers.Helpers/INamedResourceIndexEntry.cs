using System;
using s3pi.Interfaces;

namespace s3piwrappers.Helpers
{
    public interface INamedResourceIndexEntry : IResourceIndexEntry
    {
        string ResourceName { get; set; }
    }
}
