using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph
{
    public interface IHasHashedName
    {
        string Name { get; set; }
        uint NameHash { get; }
        bool NameIsHash { get; }
    }
}
