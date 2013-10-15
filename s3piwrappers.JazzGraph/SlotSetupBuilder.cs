using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph
{
    public class SlotSetupBuilder
    {
        public class ActorSlot
        {
            public readonly uint ChainId;
            public readonly uint SlotId;
            public readonly uint ActorNameHash;
            public readonly uint SlotNameHash;

            public ActorSlot(uint chainId, uint slotId,
                uint actorNameHash, uint slotNameHash)
            {
                this.ChainId = chainId;
                this.SlotId = slotId;
                this.ActorNameHash = actorNameHash;
                this.SlotNameHash = slotNameHash;
            }
        }

        public class ActorSuffix
        {
            public readonly ActorDefinition Actor;
            public readonly ParamDefinition Param;

            public ActorSuffix(ActorDefinition actor, ParamDefinition param)
            {
                this.Actor = actor;
                this.Param = param;
            }
        }

        private List<ActorSlot> mActorSlotList;
        private List<ActorSuffix> mActorSuffixList;

        public SlotSetupBuilder()
        {
            this.mActorSlotList = new List<ActorSlot>();
            this.mActorSuffixList = new List<ActorSuffix>();
        }

        public void AddNamespaceSlotSuffix(ActorDefinition targetNamespace,
            ParamDefinition suffixParameter)
        {
            this.mActorSuffixList.Add(
                new ActorSuffix(targetNamespace, suffixParameter));
        }

        public void AddSlotAssignment(uint chainId, uint slotId,
            uint targetNamespaceHash, uint targetSlotNameHash)
        {
            this.mActorSlotList.Add(new ActorSlot(chainId, slotId, 
                targetNamespaceHash, targetSlotNameHash));
        }

        public int ActorSlotCount
        {
            get { return this.mActorSlotList.Count; }
        }

        public ActorSlot[] ActorSlotArray
        {
            get { return this.mActorSlotList.ToArray(); }
        }

        public int ActorSuffixCount
        {
            get { return this.mActorSuffixList.Count; }
        }

        public ActorSuffix[] ActorSuffixArray
        {
            get { return this.mActorSuffixList.ToArray(); }
        }

        public void Reset()
        {
            this.mActorSlotList.Clear();
            this.mActorSuffixList.Clear();
        }
    }
}
