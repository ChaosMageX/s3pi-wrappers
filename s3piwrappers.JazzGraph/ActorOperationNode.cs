using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class ActorOperationNode : MulticastDecisionGraphNode
    {
        public const uint ResourceType = 0x02EEEBDE;
        public const string ResourceTag = "AcOp";

        private ActorDefinition mActor;
        protected readonly JazzActorOperationNode.ActorOperation mOperation;
        private uint mOperand;

        public ActorOperationNode(
            JazzActorOperationNode.ActorOperation operation)
            : base(ResourceType, ResourceTag)
        {
            this.mActor = null;
            this.mOperation = operation;
            this.mOperand = 0;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzActorOperationNode jaon 
                = new JazzActorOperationNode(0, null, s);
            jaon.ActorDefinitionIndex = this.mActor == null
                ? NullCRef : this.mActor.ChunkReference;
            jaon.ActorOp = this.mOperation;
            jaon.Operand = this.mOperand;
            if (this.TargetCount > 0)
            {
                JazzChunk.ChunkReferenceList dgi = jaon.DecisionGraphIndexes;
                DecisionGraphNode[] targets = this.Targets;
                Array.Sort(targets, 0, targets.Length,
                    AChunkObject.InstantiationComparer.Instance);
                for (int i = 0; i < targets.Length; i++)
                {
                    dgi.Add(targets[i] == null 
                        ? NullCRef : targets[i].ChunkReference);
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jaon);
        }

        public ActorDefinition Actor
        {
            get { return this.mActor; }
            set
            {
                if (this.mActor != value)
                {
                    this.mActor = value;
                }
            }
        }

        public JazzActorOperationNode.ActorOperation Operation
        {
            get { return this.mOperation; }
        }

        public bool Operand
        {
            get { return this.mOperand != 0; }
            set
            {
                uint val = (uint)(value ? 1 : 0);
                if (this.mOperand != val)
                {
                    this.mOperand = val;
                }
            }
        }
    }
}
