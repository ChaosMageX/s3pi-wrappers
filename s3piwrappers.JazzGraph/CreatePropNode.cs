using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers;

namespace s3piwrappers.JazzGraph
{
    public class CreatePropNode : MulticastDecisionGraphNode
    {
        public const uint ResourceType = 0x02EEEBDD;
        public const string ResourceTag = "Prop";

        private ActorDefinition mPropActor;
        private ParamDefinition mPropParam;
        private RK mPropKey;

        public CreatePropNode()
            : base(ResourceType, ResourceTag)
        {
            this.mPropActor = null;
            this.mPropParam = null;
            this.mPropKey = new RK();
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzCreatePropNode jcpn = new JazzCreatePropNode(0, null, s);
            jcpn.ActorDefinitionIndex = this.mPropActor == null
                ? NullCRef : this.mPropActor.ChunkReference;
            jcpn.ParameterDefinitionIndex = this.mPropParam == null
                ? NullCRef : this.mPropParam.ChunkReference;
            jcpn.PropResource = this.mPropKey;
            if (this.TargetCount > 0)
            {
                JazzChunk.ChunkReferenceList dgi = jcpn.DecisionGraphIndexes;
                DecisionGraphNode[] targets = this.Targets;
                Array.Sort(targets, 0, targets.Length,
                    AChunkObject.InstantiationComparer.Instance);
                for (int i = 0; i < targets.Length; i++)
                {
                    dgi.Add(targets[i] == null
                        ? NullCRef : targets[i].ChunkReference);
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jcpn);
        }

        public ActorDefinition PropActor
        {
            get { return this.mPropActor; }
            set
            {
                if (this.mPropActor != value)
                {
                    this.mPropActor = value;
                }
            }
        }

        public ParamDefinition PropParam
        {
            get { return this.mPropParam; }
            set
            {
                if (this.mPropParam != value)
                {
                    this.mPropParam = value;
                }
            }
        }

        public RK PropKey
        {
            get { return this.mPropKey; }
            set
            {
                if (!this.mPropKey.Equals(value))
                {
                    this.mPropKey = value;
                }
            }
        }
    }
}
