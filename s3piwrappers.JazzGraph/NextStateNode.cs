using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class NextStateNode : DecisionGraphNode
    {
        public const uint ResourceType = 0x02EEEBDC;
        public const string ResourceTag = "SNSN";

        private State mNextState;

        public NextStateNode()
            : base(ResourceType, ResourceTag)
        {
            this.mNextState = null;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzNextStateNode jnsn = new JazzNextStateNode(0, null, s);
            jnsn.StateIndex = this.mNextState == null
                ? NullCRef : this.mNextState.ChunkReference;
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jnsn);
        }

        public State NextState
        {
            get { return this.mNextState; }
            set
            {
                if (this.mNextState != value)
                {
                    this.mNextState = value;
                }
            }
        }
    }
}
