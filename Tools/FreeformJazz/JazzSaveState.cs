using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.FreeformJazz
{
    public enum JazzSaveState
    {
        /// <summary>
        /// Changes have not been serialized in any way yet.
        /// </summary>
        Dirty,
        /// <summary>
        /// Changes have been serialized into local memory,
        /// but not into a file on the hard drive yet.
        /// </summary>
        Committed,
        /// <summary>
        /// Changes have been serialized into local memory
        /// and into a file on the hard drive.
        /// </summary>
        Saved
    }
}
