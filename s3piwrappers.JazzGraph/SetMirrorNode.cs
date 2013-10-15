using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.JazzGraph
{
    public class SetMirrorNode : ActorOperationNode
    {
        public SetMirrorNode()
            : base(JazzActorOperationNode.ActorOperation.SetMirror)
        {
        }

        public bool Mirror
        {
            get { return this.Operand; }
            set { this.Operand = value; }
        }
    }
}
