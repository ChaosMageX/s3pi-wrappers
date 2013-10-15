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
            public readonly float Weight;
            public readonly DecisionGraphNode[] Targets;

            public Slice(float weight, DecisionGraphNode[] targets)
            {
                this.Weight = weight;
                this.Targets = targets;
            }
        }

        private class SliceImpl
        {
            public float Weight;
            public List<DecisionGraphNode> Targets;

            public SliceImpl(float weight)
            {
                this.Weight = weight;
                this.Targets = new List<DecisionGraphNode>();
            }
        }

        private List<SliceImpl> mSlices;
        private JazzRandomNode.Flags mFlags;

        public RandomNode()
            : base(ResourceType, ResourceTag)
        {
            this.mSlices = new List<SliceImpl>();
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
                foreach (SliceImpl slice in this.mSlices)
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

        public void AddSliceTarget(int slice, DecisionGraphNode target)
        {
            if (slice < 0 || slice > this.mSlices.Count)
            {
                throw new ArgumentOutOfRangeException("slice");
            }
            if (slice == this.mSlices.Count)
            {
                this.mSlices.Add(new SliceImpl(1));
            }
            SliceImpl sli = this.mSlices[slice];
            if (!sli.Targets.Contains(target))
            {
                sli.Targets.Add(target);
            }
        }

        public bool RemoveSliceTarget(int slice, DecisionGraphNode target)
        {
            if (slice < 0 || slice > this.mSlices.Count)
            {
                throw new ArgumentOutOfRangeException("slice");
            }
            if (slice == this.mSlices.Count)
            {
                return false;
            }
            SliceImpl sli = this.mSlices[slice];
            int index = sli.Targets.IndexOf(target);
            if (index < 0)
            {
                return false;
            }
            sli.Targets.RemoveAt(index);
            return true;
        }

        public float GetSliceWeight(int slice)
        {
            if (slice < 0 || slice > this.mSlices.Count)
            {
                throw new ArgumentOutOfRangeException("slice");
            }
            if (slice == this.mSlices.Count)
            {
                return 1;
            }
            return this.mSlices[slice].Weight;
        }

        public void SetSliceWeight(int slice, float weight)
        {
            if (slice < 0 || slice > this.mSlices.Count)
            {
                throw new ArgumentOutOfRangeException("slice");
            }
            if (slice == this.mSlices.Count)
            {
                this.mSlices.Add(new SliceImpl(weight));
            }
            else
            {
                SliceImpl sli = this.mSlices[slice];
                sli.Weight = weight;
            }
        }

        public int SliceCount
        {
            get { return this.mSlices.Count; }
        }

        public Slice[] Slices
        {
            get
            {
                SliceImpl sli;
                Slice[] slices = new Slice[this.mSlices.Count];
                for (int i = this.mSlices.Count - 1; i >= 0; i--)
                {
                    sli = this.mSlices[i];
                    slices[i] = new Slice(sli.Weight, sli.Targets.ToArray());
                }
                return slices;
            }
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
