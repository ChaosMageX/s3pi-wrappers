using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph
{
    public abstract class DecisionGraphNode : AChunkObject
    {
        public DecisionGraphNode(uint nodeType, string nodeTag)
            : base(nodeType, nodeTag)
        {
        }
    }
}
