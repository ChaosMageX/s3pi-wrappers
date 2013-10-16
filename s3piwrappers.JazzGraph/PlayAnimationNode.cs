using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.JazzGraph
{
    public class PlayAnimationNode : AnimationNode
    {
        public const uint ResourceType = 0x02EEDB5F;
        public const string ResourceTag = "Play";

        public static readonly RK DummyKey = new RK(0, 0, 0x02d5df126b20c4f3);

        private RK mClipKey;
        private RK mTrackMaskKey;
        private SlotSetupBuilder mSlotSetup;
        private RK mAdditiveClipKey;
        private string mClipPattern;
        private string mAdditiveClipPattern;

        public PlayAnimationNode()
            : base(ResourceType, ResourceTag)
        {
            this.mClipKey = new RK();
            this.mTrackMaskKey = new RK();
            this.mSlotSetup = new SlotSetupBuilder();
            this.mAdditiveClipKey = new RK();
            this.mClipPattern = null;
            this.mAdditiveClipPattern = null;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            int i;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzPlayAnimationNode jpan 
                = new JazzPlayAnimationNode(0, null, s);
            jpan.ClipResource = string.IsNullOrEmpty(this.mClipPattern)
                ? this.mClipKey : DummyKey;
            jpan.TkmkResource = this.mTrackMaskKey;
            if (this.mSlotSetup.ActorSlotCount > 0)
            {
                JazzPlayAnimationNode.ActorSlot jslot;
                JazzPlayAnimationNode.ActorSlotList jslots 
                    = jpan.ActorSlots;
                SlotSetupBuilder.ActorSlot slot;
                SlotSetupBuilder.ActorSlot[] slots
                    = this.mSlotSetup.ActorSlotArray;
                for (i = 0; i < slots.Length; i++)
                {
                    slot = slots[i];
                    jslot = new JazzPlayAnimationNode.ActorSlot(0, null, 
                        slot.ChainId, slot.SlotId, 
                        slot.ActorNameHash, slot.SlotNameHash);
                    jslots.Add(jslot);
                }
            }
            if (this.mSlotSetup.ActorSuffixCount > 0)
            {
                JazzPlayAnimationNode.ActorSuffix jsuffix;
                JazzPlayAnimationNode.ActorSuffixList jsuffixes
                    = jpan.ActorSuffixes;
                SlotSetupBuilder.ActorSuffix suffix;
                SlotSetupBuilder.ActorSuffix[] suffixes
                    = this.mSlotSetup.ActorSuffixArray;
                for (i = 0; i < suffixes.Length; i++)
                {
                    suffix = suffixes[i];
                    jsuffix = new JazzPlayAnimationNode.ActorSuffix(0, null);
                    jsuffix.ActorNameHash = suffix.Actor == null
                        ? 0 : suffix.Actor.NameHash;
                    jsuffix.SuffixHash = suffix.Param == null
                        ? 0 : suffix.Param.NameHash;
                    jsuffixes.Add(jsuffix);
                }
            }
            jpan.AdditiveClipResource 
                = string.IsNullOrEmpty(this.mAdditiveClipPattern)
                ? this.mAdditiveClipKey : DummyKey;
            jpan.Animation = this.mClipPattern;
            jpan.AdditiveAnimation = this.mAdditiveClipPattern;

            jpan.AnimationNodeFlags = this.Flags;
            jpan.AnimationPriority1 = this.Priority;
            jpan.BlendInTime = this.BlendInTime;
            jpan.BlendOutTime = this.BlendOutTime;
            jpan.Speed = this.Speed;
            ActorDefinition actor = this.Actor;
            jpan.ActorDefinitionIndex = actor == null
                ? NullCRef : actor.ChunkReference;
            jpan.TimingPriority = this.TimingPriority;
            if (this.TargetCount > 0)
            {
                JazzChunk.ChunkReferenceList dgi = jpan.DecisionGraphIndexes;
                DecisionGraphNode[] targets = this.Targets;
                Array.Sort(targets, 0, targets.Length,
                    AChunkObject.InstantiationComparer.Instance);
                for (i = 0; i < targets.Length; i++)
                {
                    dgi.Add(targets[i] == null
                        ? NullCRef : targets[i].ChunkReference);
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jpan);
        }

        public RK ClipKey
        {
            get { return this.mClipKey; }
            set
            {
                if (!this.mClipKey.Equals(value))
                {
                    this.mClipKey = value;
                }
            }
        }

        public RK TrackMaskKey
        {
            get { return this.mTrackMaskKey; }
            set
            {
                if (!this.mTrackMaskKey.Equals(value))
                {
                    this.mTrackMaskKey = value;
                }
            }
        }

        public SlotSetupBuilder SlotSetup
        {
            get { return this.mSlotSetup; }
        }

        public RK AdditiveClipKey
        {
            get { return this.mAdditiveClipKey; }
            set
            {
                if (!this.mAdditiveClipKey.Equals(value))
                {
                    this.mAdditiveClipKey = value;
                }
            }
        }

        public string ClipPattern
        {
            get { return this.mClipPattern; }
            set
            {
                if (this.mClipPattern != value)
                {
                    this.mClipPattern = value;
                }
            }
        }

        public string AdditiveClipPattern
        {
            get { return this.mAdditiveClipPattern; }
            set
            {
                if (this.mAdditiveClipPattern != value)
                {
                    this.mAdditiveClipPattern = value;
                }
            }
        }
    }
}
