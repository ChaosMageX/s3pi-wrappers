using System;
using System.Collections.Generic;
using s3piwrappers.Helpers.Collections;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class SlotBuilder
    {
        public class ActorSlot
        {
            private SlotBuilder mSB;
            private uint mChainID;
            private uint mSlotID;
            private uint mActorNameHash;
            private uint mSlotNameHash;

            public ActorSlot(SlotBuilder sb)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                this.mSB = sb;
                this.mChainID = 0;
                this.mSlotID = 0;
                this.mActorNameHash = 0;
                this.mSlotNameHash = 0;
            }

            public ActorSlot(SlotBuilder sb, uint chainID, 
                uint slotID, uint actorNameHash, uint slotNameHash)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                this.mSB = sb;
                this.mChainID = chainID;
                this.mSlotID = slotID;
                this.mActorNameHash = actorNameHash;
                this.mSlotNameHash = slotNameHash;
            }

            public ActorSlot(SlotBuilder sb, 
                SlotSetupBuilder.ActorSlot slot)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                if (slot == null)
                {
                    throw new ArgumentNullException("slot");
                }
                this.mSB = sb;
                this.mChainID = slot.ChainId;
                this.mSlotID = slot.SlotId;
                this.mActorNameHash = slot.ActorNameHash;
                this.mSlotNameHash = slot.SlotNameHash;
            }

            public void AddToSSB(SlotSetupBuilder ssb)
            {
                ssb.AddSlotAssignment(this.mChainID, this.mSlotID,
                    this.mActorNameHash, this.mSlotNameHash);
            }

            private class ChainIDCommand : Command
            {
                private ActorSlot mSlot;
                private uint mOldVal;
                private uint mNewVal;

                public ChainIDCommand(ActorSlot slot, uint newValue)
                {
                    this.mSlot = slot;
                    this.mOldVal = slot.mChainID;
                    this.mNewVal = newValue;
                    this.mLabel = "Set Chain ID of Actor Slot";
                }

                public override bool Execute()
                {
                    this.mSlot.mChainID = this.mNewVal;
                    this.mSlot.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlot.mChainID = this.mOldVal;
                    this.mSlot.mSB.CommitLists();
                }
            }

            private class SlotIDCommand : Command
            {
                private ActorSlot mSlot;
                private uint mOldVal;
                private uint mNewVal;

                public SlotIDCommand(ActorSlot slot, uint newValue)
                {
                    this.mSlot = slot;
                    this.mOldVal = slot.mSlotID;
                    this.mNewVal = newValue;
                    this.mLabel = "Set Slot ID of Actor Slot";
                }

                public override bool Execute()
                {
                    this.mSlot.mSlotID = this.mNewVal;
                    this.mSlot.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlot.mSlotID = this.mOldVal;
                    this.mSlot.mSB.CommitLists();
                }
            }

            private class ActorNameHashCommand : Command
            {
                private ActorSlot mSlot;
                private uint mOldVal;
                private uint mNewVal;

                public ActorNameHashCommand(ActorSlot slot, uint newValue)
                {
                    this.mSlot = slot;
                    this.mOldVal = slot.mActorNameHash;
                    this.mNewVal = newValue;
                    this.mLabel = "Set Actor Name Hash of Actor Slot";
                }

                public override bool Execute()
                {
                    this.mSlot.mActorNameHash = this.mNewVal;
                    this.mSlot.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlot.mActorNameHash = this.mOldVal;
                    this.mSlot.mSB.CommitLists();
                }
            }

            private class SlotNameHashCommand : Command
            {
                private ActorSlot mSlot;
                private uint mOldVal;
                private uint mNewVal;

                public SlotNameHashCommand(ActorSlot slot, uint newValue)
                {
                    this.mSlot = slot;
                    this.mOldVal = slot.mSlotNameHash;
                    this.mNewVal = newValue;
                    this.mLabel = "Set Slot Name Hash of Actor Slot";
                }

                public override bool Execute()
                {
                    this.mSlot.mSlotNameHash = this.mNewVal;
                    this.mSlot.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlot.mSlotNameHash = this.mOldVal;
                    this.mSlot.mSB.CommitLists();
                }
            }

            public uint ChainID
            {
                get { return this.mChainID; }
                set
                {
                    if (this.mChainID != value)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new ChainIDCommand(this, value));
                    }
                }
            }

            public uint SlotID
            {
                get { return this.mSlotID; }
                set
                {
                    if (this.mSlotID != value)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new SlotIDCommand(this, value));
                    }
                }
            }

            public uint ActorNameHash
            {
                get { return this.mActorNameHash; }
                set
                {
                    if (this.mActorNameHash != value)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new ActorNameHashCommand(this, value));
                    }
                }
            }

            public uint SlotNameHash
            {
                get { return this.mSlotNameHash; }
                set
                {
                    if (this.mSlotNameHash != value)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new SlotNameHashCommand(this, value));
                    }
                }
            }
        }

        public class ActorSuffix
        {
            private SlotBuilder mSB;
            private RefToActor mActor;
            private RefToParam mParam;

            public ActorSuffix(SlotBuilder sb)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                this.mSB = sb;
                this.mActor = new RefToActor(sb.mScene, null);
                this.mParam = new RefToParam(sb.mScene, null);
            }

            public ActorSuffix(SlotBuilder sb, 
                ActorDefinition actor, ParamDefinition param)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                this.mSB = sb;
                this.mActor = new RefToActor(sb.mScene, actor);
                this.mParam = new RefToParam(sb.mScene, param);
            }

            public ActorSuffix(SlotBuilder sb, 
                SlotSetupBuilder.ActorSuffix suffix)
            {
                if (sb == null)
                {
                    throw new ArgumentNullException("sb");
                }
                if (suffix == null)
                {
                    throw new ArgumentNullException("suffix");
                }
                this.mSB = sb;
                this.mActor = new RefToActor(sb.mScene, suffix.Actor);
                this.mParam = new RefToParam(sb.mScene, suffix.Param);
            }

            public void AddToSSB(SlotSetupBuilder ssb)
            {
                ssb.AddNamespaceSlotSuffix(
                    this.mActor.GetValue(), this.mParam.GetValue());
            }

            private class ActorCommand : Command
            {
                private ActorSuffix mSuffix;
                private ActorDefinition mOldVal;
                private ActorDefinition mNewVal;

                public ActorCommand(ActorSuffix suffix,
                    ActorDefinition newValue)
                {
                    this.mSuffix = suffix;
                    this.mOldVal = suffix.mActor.GetValue();
                    this.mNewVal = newValue;
                    this.mLabel = "Set Actor of Actor Suffix";
                }

                public override bool Execute()
                {
                    this.mSuffix.mActor.SetValue(this.mNewVal);
                    this.mSuffix.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffix.mActor.SetValue(this.mOldVal);
                    this.mSuffix.mSB.CommitLists();
                }
            }

            private class ParamCommand : Command
            {
                private ActorSuffix mSuffix;
                private ParamDefinition mOldVal;
                private ParamDefinition mNewVal;

                public ParamCommand(ActorSuffix suffix,
                    ParamDefinition newValue)
                {
                    this.mSuffix = suffix;
                    this.mOldVal = suffix.mParam.GetValue();
                    this.mNewVal = newValue;
                    this.mLabel = "Set Param of Actor Suffix";
                }

                public override bool Execute()
                {
                    this.mSuffix.mParam.SetValue(this.mNewVal);
                    this.mSuffix.mSB.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffix.mParam.SetValue(this.mOldVal);
                    this.mSuffix.mSB.CommitLists();
                }
            }

            public RefToActor Actor
            {
                get { return this.mActor; }
                set
                {
                    ActorDefinition ad 
                        = value == null ? null : value.GetValue();
                    if (this.mActor.GetValue() != ad)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new ActorCommand(this, ad));
                    }
                }
            }

            public RefToParam Param
            {
                get { return this.mParam; }
                set
                {
                    ParamDefinition pd
                        = value == null ? null : value.GetValue();
                    if (this.mParam.GetValue() != pd)
                    {
                        this.mSB.mScene.Container.UndoRedo.Submit(
                            new ParamCommand(this, pd));
                    }
                }
            }
        }

        public class ActorSlotList : List<ActorSlot>, IHasGenericInsert
        {
            private SlotBuilder mSlotBuilder;

            public ActorSlotList(SlotBuilder slotBuilder)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            public ActorSlotList(SlotBuilder slotBuilder, int capacity)
                : base(capacity)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            public ActorSlotList(SlotBuilder slotBuilder,
                IEnumerable<ActorSlot> collection)
                : base(collection)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            private class AddNewItemCommand : Command
            {
                private ActorSlotList mSlotList;
                private ActorSlot mSlot;

                public AddNewItemCommand(ActorSlotList asl)
                {
                    this.mSlotList = asl;
                    this.mSlot = new ActorSlot(asl.mSlotBuilder);
                    this.mLabel = "Add New Actor Slot to Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSlotList.Add(this.mSlot);
                    this.mSlotList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlotList.RemoveAt(this.mSlotList.Count - 1, false);
                    this.mSlotList.mSlotBuilder.CommitLists();
                }
            }

            private class InsertNewItemCommand : Command
            {
                private ActorSlotList mSlotList;
                private ActorSlot mSlot;
                private int mIndex;

                public InsertNewItemCommand(ActorSlotList asl, int index)
                {
                    this.mSlotList = asl;
                    this.mSlot = new ActorSlot(asl.mSlotBuilder);
                    this.mIndex = index;
                    this.mLabel = "Insert New Actor Slot into Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSlotList.Insert(this.mIndex, this.mSlot);
                    this.mSlotList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlotList.RemoveAt(this.mIndex, false);
                    this.mSlotList.mSlotBuilder.CommitLists();
                }
            }

            private class RemoveItemAtCommand : Command
            {
                private ActorSlotList mSlotList;
                private ActorSlot mSlot;
                private int mIndex;

                public RemoveItemAtCommand(ActorSlotList asl, int index)
                {
                    this.mSlotList = asl;
                    this.mSlot = asl[index];
                    this.mIndex = index;
                    this.mLabel = "Remove Actor Slot from Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSlotList.RemoveAt(this.mIndex, false);
                    this.mSlotList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlotList.Insert(this.mIndex, this.mSlot);
                    this.mSlotList.mSlotBuilder.CommitLists();
                }
            }

            private class ClearCommand : Command
            {
                private ActorSlotList mSlotList;
                private ActorSlot[] mSlots;

                public ClearCommand(ActorSlotList asl)
                {
                    this.mSlotList = asl;
                    this.mSlots = asl.ToArray();
                    this.mLabel = "Clear Actor Slots from Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSlotList.Clear(false);
                    this.mSlotList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSlotList.InsertRange(0, this.mSlots);
                    this.mSlotList.mSlotBuilder.CommitLists();
                }
            }

            public bool Add()
            {
                this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                    new AddNewItemCommand(this));
                return true;
            }

            public bool Insert(int index)
            {
                this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                    new InsertNewItemCommand(this, index));
                return true;
            }

            public new bool Remove(ActorSlot slot)
            {
                int index = this.IndexOf(slot);
                if (index >= 0)
                {
                    this.RemoveAt(index, true);
                    return true;
                }
                return false;
            }

            public new void RemoveAt(int index)
            {
                this.RemoveAt(index, true);
            }

            private void RemoveAt(int index, bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                        new RemoveItemAtCommand(this, index));
                }
                else
                {
                    base.RemoveAt(index);
                }
            }

            public new void Clear()
            {
                this.Clear(true);
            }

            private void Clear(bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                        new ClearCommand(this));
                }
                else
                {
                    base.Clear();
                }
            }
        }

        public class ActorSuffixList : List<ActorSuffix>, IHasGenericInsert
        {
            private SlotBuilder mSlotBuilder;

            public ActorSuffixList(SlotBuilder slotBuilder)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            public ActorSuffixList(SlotBuilder slotBuilder, int capacity)
                : base(capacity)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            public ActorSuffixList(SlotBuilder slotBuilder, 
                IEnumerable<ActorSuffix> collection)
                : base(collection)
            {
                if (slotBuilder == null)
                {
                    throw new ArgumentNullException("slotBuilder");
                }
                this.mSlotBuilder = slotBuilder;
            }

            private class AddNewItemCommand : Command
            {
                private ActorSuffixList mSuffList;
                private ActorSuffix mSuff;

                public AddNewItemCommand(ActorSuffixList asl)
                {
                    this.mSuffList = asl;
                    this.mSuff = new ActorSuffix(asl.mSlotBuilder);
                    this.mLabel = "Add New Actor Suffix to Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSuffList.Add(this.mSuff);
                    this.mSuffList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffList.RemoveAt(this.mSuffList.Count - 1, false);
                    this.mSuffList.mSlotBuilder.CommitLists();
                }
            }

            private class InsertNewItemCommand : Command
            {
                private ActorSuffixList mSuffList;
                private ActorSuffix mSuff;
                private int mIndex;

                public InsertNewItemCommand(ActorSuffixList asl, int index)
                {
                    this.mSuffList = asl;
                    this.mSuff = new ActorSuffix(asl.mSlotBuilder);
                    this.mIndex = index;
                    this.mLabel = "Insert New Actor Suffix into Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSuffList.Insert(this.mIndex, this.mSuff);
                    this.mSuffList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffList.RemoveAt(this.mIndex, false);
                    this.mSuffList.mSlotBuilder.CommitLists();
                }
            }

            private class RemoveItemAtCommand : Command
            {
                private ActorSuffixList mSuffList;
                private ActorSuffix mSuff;
                private int mIndex;

                public RemoveItemAtCommand(ActorSuffixList asl, int index)
                {
                    this.mSuffList = asl;
                    this.mSuff = asl[index];
                    this.mIndex = index;
                    this.mLabel = "Remove Actor Suffix from Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSuffList.RemoveAt(this.mIndex, false);
                    this.mSuffList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffList.Insert(this.mIndex, this.mSuff);
                    this.mSuffList.mSlotBuilder.CommitLists();
                }
            }

            private class ClearCommand : Command
            {
                private ActorSuffixList mSuffList;
                private ActorSuffix[] mSuffs;

                public ClearCommand(ActorSuffixList asl)
                {
                    this.mSuffList = asl;
                    this.mSuffs = asl.ToArray();
                    this.mLabel = "Clear Actor Suffixes from Slot Builder";
                }

                public override bool Execute()
                {
                    this.mSuffList.Clear(false);
                    this.mSuffList.mSlotBuilder.CommitLists();
                    return true;
                }

                public override void Undo()
                {
                    this.mSuffList.InsertRange(0, this.mSuffs);
                    this.mSuffList.mSlotBuilder.CommitLists();
                }
            }

            public bool Add()
            {
                this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                    new AddNewItemCommand(this));
                return true;
            }

            public bool Insert(int index)
            {
                this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                    new InsertNewItemCommand(this, index));
                return true;
            }

            public new bool Remove(ActorSuffix suffix)
            {
                int index = this.IndexOf(suffix);
                if (index >= 0)
                {
                    this.RemoveAt(index, true);
                    return true;
                }
                return false;
            }

            public new void RemoveAt(int index)
            {
                this.RemoveAt(index, true);
            }

            private void RemoveAt(int index, bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                        new RemoveItemAtCommand(this, index));
                }
                else
                {
                    base.RemoveAt(index);
                }
            }

            public new void Clear()
            {
                this.Clear(true);
            }

            private void Clear(bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mSlotBuilder.mScene.Container.UndoRedo.Submit(
                        new ClearCommand(this));
                }
                else
                {
                    base.Clear();
                }
            }
        }

        private StateMachineScene mScene;
        private SlotSetupBuilder mSSB;
        private ActorSlotList mSlotList;
        private ActorSuffixList mSuffixList;

        public SlotBuilder(StateMachineScene scene, SlotSetupBuilder ssb)
        {
            if (scene == null)
            {
                throw new ArgumentNullException("scene");
            }
            if (ssb == null)
            {
                throw new ArgumentNullException("ssb");
            }
            this.mScene = scene;
            this.mSSB = ssb;
            this.InitLists();
        }

        public SlotBuilder(StateMachineScene scene, 
            SlotSetupBuilder ssb, SlotBuilder sb)
        {
            if (scene == null)
            {
                throw new ArgumentNullException("scene");
            }
            if (ssb == null)
            {
                throw new ArgumentNullException("ssb");
            }
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }
            this.mScene = scene;
            this.mSSB = ssb;
            this.mSlotList = new ActorSlotList(this, sb.mSlotList);
            this.mSuffixList = new ActorSuffixList(this, sb.mSuffixList);
            this.CommitLists();
        }

        private void InitLists()
        {
            if (this.mSlotList == null)
            {
                if (this.mSSB.ActorSlotCount > 0)
                {
                    SlotSetupBuilder.ActorSlot[] slots 
                        = this.mSSB.ActorSlotArray;
                    this.mSlotList 
                        = new ActorSlotList(this, slots.Length);
                    for (int i = 0; i < slots.Length; i++)
                    {
                        this.mSlotList.Add(
                            new ActorSlot(this, slots[i]));
                    }
                }
                else
                {
                    this.mSlotList = new ActorSlotList(this);
                }
                if (this.mSSB.ActorSuffixCount > 0)
                {
                    SlotSetupBuilder.ActorSuffix[] suffs
                        = this.mSSB.ActorSuffixArray;
                    this.mSuffixList 
                        = new ActorSuffixList(this, suffs.Length);
                    for (int j = 0; j < suffs.Length; j++)
                    {
                        this.mSuffixList.Add(
                            new ActorSuffix(this, suffs[j]));
                    }
                }
                else
                {
                    this.mSuffixList = new ActorSuffixList(this);
                }
            }
        }

        private void CommitLists()
        {
            if (this.mSlotList != null)
            {
                this.mSSB.Reset();
                foreach (ActorSlot slot in this.mSlotList)
                {
                    slot.AddToSSB(this.mSSB);
                }
                foreach (ActorSuffix suffix in this.mSuffixList)
                {
                    suffix.AddToSSB(this.mSSB);
                }
            }
        }

        public ActorSlotList SlotList
        {
            get { return this.mSlotList; }
            set
            {
                if (this.mSlotList != value)
                {
                    this.mSlotList = new ActorSlotList(this, value);
                    this.CommitLists();
                }
            }
        }

        public ActorSuffixList SuffixList
        {
            get { return this.mSuffixList; }
            set
            {
                if (this.mSuffixList != value)
                {
                    this.mSuffixList = new ActorSuffixList(this, value);
                    this.CommitLists();
                }
            }
        }
    }
}
