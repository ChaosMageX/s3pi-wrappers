using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class DecisionGraph : AChunkObject
    {
        public const uint ResourceType = 0x02EEDB18;
        public const string ResourceTag = "S_DG";

        private State mState;
        private List<DecisionGraphNode> mDecisionMakers;
        private List<DecisionGraphNode> mEntryPoints;

        public DecisionGraph()
            : base(ResourceType, ResourceTag)
        {
            this.mState = null;
            this.mDecisionMakers = new List<DecisionGraphNode>();
            this.mEntryPoints = new List<DecisionGraphNode>();
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzDecisionGraph jdg = new JazzDecisionGraph(0, null, s);
            this.mDecisionMakers.Sort(
                AChunkObject.InstantiationComparer.Instance);
            JazzChunk.ChunkReferenceList odgi 
                = jdg.OutboundDecisionGraphIndexes;
            foreach (DecisionGraphNode dm in this.mDecisionMakers)
            {
                odgi.Add(dm == null ? NullCRef : dm.ChunkReference);
            }
            this.mEntryPoints.Sort(
                AChunkObject.InstantiationComparer.Instance);
            JazzChunk.ChunkReferenceList idgi 
                = jdg.InboundDecisionGraphIndexes;
            foreach (DecisionGraphNode ep in this.mEntryPoints)
            {
                idgi.Add(ep == null ? NullCRef : ep.ChunkReference);
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jdg);
        }

        public State State
        {
            get { return this.mState; }
            set
            {
                if (this.mState != value)
                {
                    // TODO: Make sure this doesn't cause any recursion
                    // loops with State.DecisionGraph
                    if (this.mState != null)
                    {
                        this.mState.DecisionGraph = null;
                    }
                    if (value != null)
                    {
                        value.DecisionGraph = this;
                    }
                    this.mState = value;
                }
            }
        }

        public void AddDecisionMaker(DecisionGraphNode node)
        {
            if (!this.mDecisionMakers.Contains(node))
            {
                /*int index = this.mEntryPoints.IndexOf(node);
                if (index >= 0)
                {
                    this.mEntryPoints.RemoveAt(index);
                }/* */
                this.mDecisionMakers.Add(node);
            }
        }

        public void RemoveDecisionMaker(DecisionGraphNode node)
        {
            int index = this.mDecisionMakers.IndexOf(node);
            if (index >= 0)
            {
                this.mDecisionMakers.RemoveAt(index);
            }
        }

        public void ClearDecisionMakers()
        {
            /*DecisionGraphNode dgn;
            for (int i = this.mDecisionMakers.Count - 1; i >= 0; i--)
            {
                dgn = this.mDecisionMakers[i];
            }/* */
            this.mDecisionMakers.Clear();
        }

        public int DecisionMakerCount
        {
            get { return this.mDecisionMakers.Count; }
        }

        public DecisionGraphNode[] DecisionMakers
        {
            get { return this.mDecisionMakers.ToArray(); }
        }

        public void AddEntryPoint(DecisionGraphNode node)
        {
            if (!this.mEntryPoints.Contains(node))
            {
                /*int index = this.mDecisionMakers.IndexOf(node);
                if (index >= 0)
                {
                    this.mDecisionMakers.RemoveAt(index);
                }/* */
                this.mEntryPoints.Add(node);
            }
        }

        public void RemoveEntryPoint(DecisionGraphNode node)
        {
            int index = this.mEntryPoints.IndexOf(node);
            if (index >= 0)
            {
                this.mEntryPoints.RemoveAt(index);
            }
        }

        public void ClearEntryPoints()
        {
            /*DecisionGraphNode dgn;
            for (int i = this.mEntryPoints.Count - 1; i >= 0; i--)
            {
                dgn = this.mEntryPoints[i];
            }/* */
            this.mEntryPoints.Clear();
        }

        public int EntryPointCount
        {
            get { return this.mEntryPoints.Count; }
        }

        public DecisionGraphNode[] EntryPoints
        {
            get { return this.mEntryPoints.ToArray(); }
        }

        public void Remove(DecisionGraphNode node)
        {
            //if (node.mDecisionGraph == this)
            {
                int index = this.mDecisionMakers.IndexOf(node);
                if (index >= 0)
                {
                    this.mDecisionMakers.RemoveAt(index);
                }
                index = this.mEntryPoints.IndexOf(node);
                if (index >= 0)
                {
                    this.mEntryPoints.RemoveAt(index);
                }
                //node.mDecisionGraph = null;
            }
        }
    }
}
