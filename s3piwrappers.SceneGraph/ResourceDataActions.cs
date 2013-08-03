using System;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// Actions for a <see cref="ResourceGraph"/> 
    /// to take relating the resource data stored 
    /// in each node it contains.
    /// </summary>
    [Flags]
    public enum ResourceDataActions : byte
    {
        /// <summary>
        /// Take no actions relating to the resource data
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Attempt to locate the resource data in the FileTable
        /// </summary>
        Find = 0x01,
        /// <summary>
        /// Write the resource data to the final package
        /// </summary>
        Write = 0x02,
        /// <summary>
        /// Combined Find and Write resource data action flags
        /// </summary>
        FindWrite = 0x03
    }
}
