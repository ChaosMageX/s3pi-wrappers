using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class StopAnimationNode : AnimationNode
    {
        public const uint ResourceType = 0x0344D438;
        public const string ResourceTag = "Stop";

        public StopAnimationNode()
            : base(ResourceType, ResourceTag)
        {
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzStopAnimationNode jsan 
                = new JazzStopAnimationNode(0, null, s);
            jsan.AnimationFlags = this.Flags;
            jsan.AnimationPriority1 = this.Priority;
            jsan.BlendInTime = this.BlendInTime;
            jsan.BlendOutTime = this.BlendOutTime;
            jsan.Speed = this.Speed;
            ActorDefinition actor = this.Actor;
            jsan.ActorDefinitionIndex = actor == null
                ? NullCRef : actor.ChunkReference;
            jsan.TimingPriority = this.TimingPriority;
            if (this.TargetCount > 0)
            {
                JazzChunk.ChunkReferenceList dgi = jsan.DecisionGraphIndexes;
                DecisionGraphNode[] targets = this.Targets;
                Array.Sort(targets, 0, targets.Length,
                    AChunkObject.InstantiationComparer.Instance);
                for (int i = 0; i < targets.Length; i++)
                {
                    dgi.Add(targets[i] == null
                        ? NullCRef : targets[i].ChunkReference);
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jsan);
        }
    }
}
