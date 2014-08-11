using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class RandomNode : DecisionGraphNode
    {
        public const uint ResourceType = 0x02EEDB70;
        public const string ResourceTag = "Rand";

        public class Slice
        {
            public float Weight;
            public readonly List<DecisionGraphNode> Targets;

            public Slice()
            {
                this.Weight = 0;
                this.Targets = new List<DecisionGraphNode>();
            }

            public Slice(float weight)
            {
                this.Weight = weight;
                this.Targets = new List<DecisionGraphNode>();
            }
        }

        private List<Slice> mSlices;
        private JazzRandomNode.Flags mFlags;

        public RandomNode()
            : base(ResourceType, ResourceTag)
        {
            this.mSlices = new List<Slice>();
            this.mFlags = JazzRandomNode.Flags.None;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzRandomNode jrn = new JazzRandomNode(0, null, s);
            if (this.mSlices.Count > 0)
            {
                JazzRandomNode.Outcome outcome;
                JazzRandomNode.OutcomeList oList = jrn.Outcomes;
                JazzChunk.ChunkReferenceList dgi;
                foreach (Slice slice in this.mSlices)
                {
                    outcome = new JazzRandomNode.Outcome(0, null);
                    outcome.Weight = slice.Weight;
                    dgi = outcome.DecisionGraphIndexes;
                    foreach (DecisionGraphNode dgn in slice.Targets)
                    {
                        dgi.Add(dgn == null ? NullCRef : dgn.ChunkReference);
                    }
                    oList.Add(outcome);
                }
            }
            jrn.Properties = this.mFlags;
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jrn);
        }

        public List<Slice> Slices
        {
            get { return this.mSlices; }
        }

        public JazzRandomNode.Flags Flags
        {
            get { return this.mFlags; }
            set
            {
                if (this.mFlags != value)
                {
                    this.mFlags = value;
                }
            }
        }

        public void SetFlags(JazzRandomNode.Flags flags, bool value)
        {
            if (value)
            {
                this.mFlags |= flags;
            }
            else
            {
                this.mFlags &= ~flags;
            }
        }
    }
}
