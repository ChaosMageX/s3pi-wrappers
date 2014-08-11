using System;
using System.Collections.Generic;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class StateMachine : AChunkObject, IHasHashedName
    {
        public const uint ResourceType = 0x02D5DF13;
        public const string ResourceTag = "S_SM";

        private string mName;
        private uint mNameHash;
        private bool bNameIsHash;
        private List<ActorDefinition> mActorDefinitions;
        private List<ParamDefinition> mParamDefinitions;
        private List<State> mStates;
        private NamespaceMap mNamespaceMap;
        private JazzStateMachine.Flags mFlags;
        private JazzChunk.AnimationPriority mDefaultPriority;
        private JazzChunk.AwarenessLevel mAwarenessOverlayLevel;

        private List<ActorDefinition> mExtraActors;
        private List<ParamDefinition> mExtraParams;
        private List<State> mExtraStates;
        private List<DecisionGraph> mDGs;
        private List<DecisionGraphNode> mDGNodes;

        public StateMachine(string name)
            : base(ResourceType, ResourceTag)
        {
            this.mName = name;
            this.mNameHash = name == null ? 0 : FNVHash.HashString32(name);
            this.bNameIsHash = name == null;
            this.mActorDefinitions = new List<ActorDefinition>();
            this.mParamDefinitions = new List<ParamDefinition>();
            this.mStates = new List<State>();
            this.mNamespaceMap = new NamespaceMap(this);
            this.mFlags = JazzStateMachine.Flags.Default;
            this.mDefaultPriority = JazzChunk.AnimationPriority.Unset;
            this.mAwarenessOverlayLevel = JazzChunk.AwarenessLevel.Unset;

            this.mExtraActors = new List<ActorDefinition>();
            this.mExtraParams = new List<ParamDefinition>();
            this.mExtraStates = new List<State>();
            this.mDGs = new List<DecisionGraph>();
            this.mDGNodes = new List<DecisionGraphNode>();
        }

        public StateMachine(GenericRCOLResource jazzResource)
            : this("")
        {
            if (jazzResource == null)
            {
                throw new ArgumentNullException("jazzResource");
            }
            GenericRCOLResource.ChunkEntryList chunkEntries
                = jazzResource.ChunkEntries;
            if (chunkEntries == null || chunkEntries.Count == 0)
            {
                throw new ArgumentException(
                    "RCOL Resource is empty", "jazzResource");
            }
            KeyNameReg.RefreshKeyNameMaps();
            uint hash;
            string name;
            int i, j, index = -1;
            State state;
            ActorDefinition ad;
            ParamDefinition pd;
            DecisionGraphNode dgn;
            JazzStateMachine jazzSM = null;
            GenericRCOLResource.ChunkEntry ce;
            AChunkObject[] chunks = new AChunkObject[chunkEntries.Count];
            List<ActorDefinition> actorDefs = new List<ActorDefinition>();
            List<ParamDefinition> paramDefs = new List<ParamDefinition>();

            #region Phase 1: Instantiate Chunks and Copy over value fields
            for (i = 0; i < chunkEntries.Count; i++)
            {
                ce = chunkEntries[i];
                hash = ce.TGIBlock.ResourceType;
                switch (hash)
                {
                    case PlayAnimationNode.ResourceType:
                        JazzPlayAnimationNode jpan
                            = ce.RCOLBlock as JazzPlayAnimationNode;
                        PlayAnimationNode lan = new PlayAnimationNode();
                        lan.ClipKey = new RK(jpan.ClipResource);
                        lan.TrackMaskKey = new RK(jpan.TkmkResource);
                        // lan.SlotSetup copied over later
                        lan.AdditiveClipKey 
                            = new RK(jpan.AdditiveClipResource);
                        lan.ClipPattern = jpan.Animation;
                        lan.AdditiveClipPattern = jpan.AdditiveAnimation;
                        lan.Flags = jpan.AnimationNodeFlags;
                        lan.Priority = jpan.AnimationPriority1;
                        lan.BlendInTime = jpan.BlendInTime;
                        lan.BlendOutTime = jpan.BlendOutTime;
                        lan.Speed = jpan.Speed;
                        // lan.Actor set later
                        lan.TimingPriority = jpan.TimingPriority;
                        chunks[i] = lan;
                        break;
                    case StopAnimationNode.ResourceType:
                        JazzStopAnimationNode jsan
                            = ce.RCOLBlock as JazzStopAnimationNode;
                        StopAnimationNode san = new StopAnimationNode();
                        san.Flags = jsan.AnimationFlags;
                        san.Priority = jsan.AnimationPriority1;
                        san.BlendInTime = jsan.BlendInTime;
                        san.BlendOutTime = jsan.BlendOutTime;
                        san.Speed = jsan.Speed;
                        // san.Actor set later
                        san.TimingPriority = jsan.TimingPriority;
                        chunks[i] = san;
                        break;
                    case ActorOperationNode.ResourceType:
                        JazzActorOperationNode jaon
                            = ce.RCOLBlock as JazzActorOperationNode;
                        ActorOperationNode aon
                            = new ActorOperationNode(jaon.ActorOp);
                        // aon.Target set later
                        aon.Operand = jaon.Operand != 0;
                        chunks[i] = aon;
                        break;
                    case CreatePropNode.ResourceType:
                        JazzCreatePropNode jcpn
                            = ce.RCOLBlock as JazzCreatePropNode;
                        CreatePropNode cpn = new CreatePropNode();
                        // cpn.PropActor set later
                        // cpn.PropParameter set later
                        cpn.PropKey = new RK(jcpn.PropResource);
                        chunks[i] = cpn;
                        break;
                    case RandomNode.ResourceType:
                        JazzRandomNode jrand
                            = ce.RCOLBlock as JazzRandomNode;
                        RandomNode rand = new RandomNode();
                        // rand.Slices set later
                        rand.Flags = jrand.Properties;
                        chunks[i] = rand;
                        break;
                    case SelectOnParameterNode.ResourceType:
                        // sopn.Parameter set later
                        // sopn.Cases set later
                        chunks[i] = new SelectOnParameterNode();
                        break;
                    case SelectOnDestinationNode.ResourceType:
                        // sodn.Cases set later
                        chunks[i] = new SelectOnDestinationNode();
                        break;
                    case NextStateNode.ResourceType:
                        // nsn.NextState set later
                        chunks[i] = new NextStateNode();
                        break;
                    case DecisionGraph.ResourceType:
                        // dg.State set later
                        // dg.DecisionMakers set later
                        // dg.EntryPoints set later
                        chunks[i] = new DecisionGraph();
                        break;
                    case State.ResourceType:
                        JazzState js = ce.RCOLBlock as JazzState;
                        hash = js.NameHash;
                        if (!KeyNameReg.TryFindName(hash, out name))
                            name = KeyNameReg.UnhashName(hash);
                        state = new State(name);
                        state.Flags = js.Properties;
                        // state.DecisionGraph set later
                        // state.Transitions set later
                        state.AwarenessOverlayLevel
                            = js.AwarenessOverlayLevel;
                        chunks[i] = state;
                        break;
                    case ParamDefinition.ResourceType:
                        JazzParameterDefinition jpd
                            = ce.RCOLBlock as JazzParameterDefinition;
                        hash = jpd.NameHash;
                        if (!KeyNameReg.TryFindName(hash, out name))
                            name = KeyNameReg.UnhashName(hash);
                        pd = new ParamDefinition(name);
                        
                        hash = jpd.DefaultValue;
                        if (!KeyNameReg.TryFindName(hash, out name))
                            name = KeyNameReg.UnhashName(hash);
                        pd.DefaultValue = name;
                        chunks[i] = pd;
                        paramDefs.Add(pd);
                        break;
                    case ActorDefinition.ResourceType:
                        JazzActorDefinition jad
                            = ce.RCOLBlock as JazzActorDefinition;
                        hash = jad.NameHash;
                        if (!KeyNameReg.TryFindName(hash, out name))
                            name = KeyNameReg.UnhashName(hash);
                        ad = new ActorDefinition(name);
                        chunks[i] = ad;
                        actorDefs.Add(ad);
                        break;
                    case StateMachine.ResourceType:
                        if (index != -1)
                        {
                            throw new Exception(
                                "More than one State Machine in RCOL");
                        }
                        index = i;
                        jazzSM = ce.RCOLBlock as JazzStateMachine;
                        hash = jazzSM.NameHash;
                        if (hash == 0)
                        {
                            name = null;
                            this.bNameIsHash = true;
                        }
                        else if (!KeyNameReg.TryFindName(hash, out name))
                        {
                            name = KeyNameReg.UnhashName(hash);
                            this.bNameIsHash = true;
                        }
                        else
                        {
                            this.bNameIsHash = false;
                        }
                        this.mName = name;
                        this.mNameHash = hash;
                        // this.mActorDefinitions set later
                        // this.mParameterDefinitions set later
                        // this.mStates set later
                        this.mFlags = jazzSM.Properties;
                        this.mDefaultPriority = jazzSM.AutomationPriority;
                        this.mAwarenessOverlayLevel
                            = jazzSM.AwarenessOverlayLevel;
                        chunks[i] = this;
                        break;
                }
            }
            if (index == -1)
            {
                throw new Exception("RCOL does not contain a Jazz Graph");
            }
            #endregion

            #region Phase 2: Copy over fields referencing other chunks
            for (i = 0; i < chunkEntries.Count; i++)
            {
                ce = chunkEntries[i];
                switch (ce.TGIBlock.ResourceType)
                {
                    case PlayAnimationNode.ResourceType:
                        JazzPlayAnimationNode jpan
                            = ce.RCOLBlock as JazzPlayAnimationNode;
                        PlayAnimationNode lan
                            = chunks[i] as PlayAnimationNode;
                        index = jpan.ActorDefinitionIndex.TGIBlockIndex;
                        lan.Actor = index < 0
                            ? null : chunks[index + 1] as ActorDefinition;
                        break;
                    case StopAnimationNode.ResourceType:
                        JazzStopAnimationNode jsan
                            = ce.RCOLBlock as JazzStopAnimationNode;
                        StopAnimationNode san
                            = chunks[i] as StopAnimationNode;
                        index = jsan.ActorDefinitionIndex.TGIBlockIndex;
                        san.Actor = index < 0
                            ? null : chunks[index + 1] as ActorDefinition;
                        break;
                    case ActorOperationNode.ResourceType:
                        JazzActorOperationNode jaon
                            = ce.RCOLBlock as JazzActorOperationNode;
                        ActorOperationNode aon
                            = chunks[i] as ActorOperationNode;
                        index = jaon.ActorDefinitionIndex.TGIBlockIndex;
                        aon.Actor = index < 0
                            ? null : chunks[index + 1] as ActorDefinition;
                        break;
                    case CreatePropNode.ResourceType:
                        JazzCreatePropNode jcpn
                            = ce.RCOLBlock as JazzCreatePropNode;
                        CreatePropNode cpn = chunks[i] as CreatePropNode;
                        index = jcpn.ActorDefinitionIndex.TGIBlockIndex;
                        cpn.PropActor = index < 0
                            ? null : chunks[index + 1] as ActorDefinition;
                        index = jcpn.ParameterDefinitionIndex.TGIBlockIndex;
                        cpn.PropParam = index < 0
                            ? null : chunks[index + 1] as ParamDefinition;
                        break;
                    case RandomNode.ResourceType:
                        JazzRandomNode jrand = ce.RCOLBlock as JazzRandomNode;
                        RandomNode rand = chunks[i] as RandomNode;
                        RandomNode.Slice slice;
                        List<RandomNode.Slice> slices = rand.Slices;
                        foreach (JazzRandomNode.Outcome oc in jrand.Outcomes)
                        {
                            slice = new RandomNode.Slice(oc.Weight);
                            foreach (GenericRCOLResource.ChunkReference cr
                                in oc.DecisionGraphIndexes)
                            {
                                index = cr.TGIBlockIndex;
                                dgn = index < 0 ? null 
                                    : chunks[index + 1] as DecisionGraphNode;
                                slice.Targets.Add(dgn);
                            }
                            slices.Add(slice);
                        }
                        break;
                    case SelectOnParameterNode.ResourceType:
                        JazzSelectOnParameterNode jsopn
                            = ce.RCOLBlock as JazzSelectOnParameterNode;
                        SelectOnParameterNode sopn
                            = chunks[i] as SelectOnParameterNode;
                        index = jsopn.ParameterDefinitionIndex.TGIBlockIndex;
                        sopn.Parameter = index < 0 ? null 
                            : chunks[index + 1] as ParamDefinition;
                        foreach (JazzSelectOnParameterNode.Match mp
                            in jsopn.Matches)
                        {
                            hash = mp.TestValue;
                            if (!KeyNameReg.TryFindName(hash, out name))
                                name = KeyNameReg.UnhashName(hash);
                            foreach (GenericRCOLResource.ChunkReference cr
                                in mp.DecisionGraphIndexes)
                            {
                                index = cr.TGIBlockIndex;
                                dgn = index < 0 ? null 
                                    : chunks[index + 1] as DecisionGraphNode;
                                sopn.AddCaseTarget(name, dgn);
                            }
                        }
                        break;
                    case SelectOnDestinationNode.ResourceType:
                        JazzSelectOnDestinationNode jsodn
                            = ce.RCOLBlock as JazzSelectOnDestinationNode;
                        SelectOnDestinationNode sodn
                            = chunks[i] as SelectOnDestinationNode;
                        foreach (JazzSelectOnDestinationNode.Match md
                            in jsodn.Matches)
                        {
                            index = md.StateIndex.TGIBlockIndex;
                            state = index < 0 ? null 
                                : chunks[index + 1] as State;
                            foreach (GenericRCOLResource.ChunkReference cr
                                in md.DecisionGraphIndexes)
                            {
                                index = cr.TGIBlockIndex;
                                dgn = index < 0 ? null 
                                    : chunks[index + 1] as DecisionGraphNode;
                                sodn.AddCaseTarget(state, dgn);
                            }
                        }
                        break;
                    case NextStateNode.ResourceType:
                        JazzNextStateNode jnsn
                            = ce.RCOLBlock as JazzNextStateNode;
                        NextStateNode nsn = chunks[i] as NextStateNode;
                        index = jnsn.StateIndex.TGIBlockIndex;
                        nsn.NextState = index < 0 ? null 
                            : chunks[index + 1] as State;
                        break;
                    case DecisionGraph.ResourceType:
                        JazzDecisionGraph jdg
                            = ce.RCOLBlock as JazzDecisionGraph;
                        DecisionGraph dg = chunks[i] as DecisionGraph;
                        foreach (GenericRCOLResource.ChunkReference dm
                            in jdg.OutboundDecisionGraphIndexes)
                        {
                            index = dm.TGIBlockIndex;
                            dgn = index < 0 ? null 
                                : chunks[index + 1] as DecisionGraphNode;
                            dg.AddDecisionMaker(dgn);
                        }
                        foreach (GenericRCOLResource.ChunkReference ep
                            in jdg.InboundDecisionGraphIndexes)
                        {
                            index = ep.TGIBlockIndex;
                            dgn = index < 0 ? null 
                                : chunks[index + 1] as DecisionGraphNode;
                            dg.AddEntryPoint(dgn);
                        }
                        break;
                    case State.ResourceType:
                        State transition;
                        JazzState js = ce.RCOLBlock as JazzState;
                        state = chunks[i] as State;
                        index = js.DecisionGraphIndex.TGIBlockIndex;
                        state.DecisionGraph = index < 0 ? null 
                            : chunks[index + 1] as DecisionGraph;
                        foreach (GenericRCOLResource.ChunkReference trans
                            in js.OutboundStateIndexes)
                        {
                            index = trans.TGIBlockIndex;
                            transition = index < 0 ? null 
                                : chunks[index + 1] as State;
                            state.AddTransition(transition);
                        }
                        break;
                    case ParamDefinition.ResourceType:
                    case ActorDefinition.ResourceType:
                        break;
                    case StateMachine.ResourceType:
                        jazzSM = ce.RCOLBlock as JazzStateMachine;
                        foreach (GenericRCOLResource.ChunkReference jad
                            in jazzSM.ActorDefinitionIndexes)
                        {
                            index = jad.TGIBlockIndex;
                            ad = index < 0 ? null 
                                : chunks[index + 1] as ActorDefinition;
                            this.AddActorDefinition(ad);
                        }
                        foreach (GenericRCOLResource.ChunkReference jpd
                            in jazzSM.PropertyDefinitionIndexes)
                        {
                            index = jpd.TGIBlockIndex;
                            pd = index < 0 ? null 
                                : chunks[index + 1] as ParamDefinition;
                            this.AddParamDefinition(pd);
                        }
                        foreach (GenericRCOLResource.ChunkReference jst
                            in jazzSM.StateIndexes)
                        {
                            index = jst.TGIBlockIndex;
                            state = index < 0  ? null 
                                : chunks[index + 1] as State;
                            this.AddState(state);
                        }
                        break;
                }
            }
            #endregion

            #region Phase 3: Copy over animation slots and Find "Extras"
            for (i = 0; i < chunkEntries.Count; i++)
            {
                ce = chunkEntries[i];
                switch (ce.TGIBlock.ResourceType)
                {
                    case PlayAnimationNode.ResourceType:
                        JazzPlayAnimationNode jpan 
                            = ce.RCOLBlock as JazzPlayAnimationNode;
                        PlayAnimationNode lan 
                            = chunks[i] as PlayAnimationNode;
                        SlotSetupBuilder ssb = lan.SlotSetup;
                        foreach (JazzPlayAnimationNode.ActorSlot slot
                            in jpan.ActorSlots)
                        {
                            ssb.AddSlotAssignment(slot.ChainId, slot.SlotId, 
                                slot.ActorNameHash, slot.SlotNameHash);
                        }
                        foreach (JazzPlayAnimationNode.ActorSuffix suffix
                            in jpan.ActorSuffixes)
                        {
                            hash = suffix.ActorNameHash;
                            ad = null;
                            if (hash != 0)
                            {
                                index = -1;
                                for (i = actorDefs.Count - 1;
                                     i >= 0 && index == -1; i--)
                                {
                                    ad = actorDefs[i];
                                    if (ad.NameHash == hash)
                                    {
                                        index = i;
                                    }
                                }
                                if (index < 0)
                                {
                                    if (!KeyNameReg.TryFindName(hash, out name))
                                        name = KeyNameReg.UnhashName(hash);
                                    ad = new ActorDefinition(name);
                                    actorDefs.Add(ad);
                                    this.mExtraActors.Add(ad);
                                }
                            }
                            hash = suffix.SuffixHash;
                            pd = null;
                            if (hash != 0)
                            {
                                index = -1;
                                for (i = paramDefs.Count - 1;
                                     i >= 0 && index == -1; i--)
                                {
                                    pd = paramDefs[i];
                                    if (pd.NameHash == hash)
                                    {
                                        index = i;
                                    }
                                }
                                if (index < 0)
                                {
                                    if (!KeyNameReg.TryFindName(hash, out name))
                                        name = KeyNameReg.UnhashName(hash);
                                    pd = new ParamDefinition(name);
                                    paramDefs.Add(pd);
                                    this.mExtraParams.Add(pd);
                                }
                            }
                            ssb.AddNamespaceSlotSuffix(ad, pd);
                        }
                        break;
                    case State.ResourceType:
                        state = chunks[i] as State;
                        index = this.mStates.IndexOf(state);
                        if (index < 0)
                        {
                            this.mExtraStates.Add(state);
                        }
                        break;
                    case ParamDefinition.ResourceType:
                        pd = chunks[i] as ParamDefinition;
                        index = this.mParamDefinitions.IndexOf(pd);
                        if (index < 0)
                        {
                            this.mExtraParams.Add(pd);
                        }
                        break;
                    case ActorDefinition.ResourceType:
                        ad = chunks[i] as ActorDefinition;
                        index = this.mActorDefinitions.IndexOf(ad);
                        if (index < 0)
                        {
                            this.mExtraActors.Add(ad);
                        }
                        break;
                }
            }
            #endregion

            #region Phase 4: Copy over Animation CLIP Namespace Map
            RK rk;
            List<RK> rks = this.SlurpReferencedRKs();
            List<uint> foldedClipInstances = new List<uint>(rks.Count);
            for (i = rks.Count - 1; i >= 0; i--)
            {
                rk = rks[i];
                if (rk.TID == 0x6b20c4f3)
                {
                    hash = (uint)((rk.IID >> 0x20) ^ (rk.IID & 0xffffffff));
                    foldedClipInstances.Add(hash);
                }
                else
                {
                    rks.RemoveAt(i);
                }
            }
            index = 0;
            Anim animation;
            Anim[] animations = new Anim[jazzSM.Animations.Count];
            foreach (JazzStateMachine.Animation anim in jazzSM.Animations)
            {
                hash = anim.NameHash;
                if (!foldedClipInstances.Contains(hash))
                {
                    animation = new Anim();
                    animation.SrcFileHash = hash;
                    if (KeyNameReg.TryFindName(hash, out name))
                    {
                        animation.SrcFileName = name;
                        animation.SrcFileIsValid = true;
                    }
                    else
                    {
                        animation.SrcFileName 
                            = KeyNameReg.UnhashName(hash);
                        animation.SrcFileIsValid = false;
                    }
                    hash = anim.Actor1Hash;
                    if (KeyNameReg.TryFindName(hash, out name))
                    {
                        animation.Namespace = name;
                    }
                    else
                    {
                        animation.Namespace 
                            = string.Concat("0x", hash.ToString("X8"));
                    }
                    hash = anim.Actor2Hash;
                    ad = null;
                    if (hash != 0)
                    {
                        j = -1;
                        for (i = actorDefs.Count - 1; i >= 0 && j < 0; i--)
                        {
                            ad = actorDefs[i];
                            if (ad.NameHash == hash)
                            {
                                j = i;
                            }
                        }
                        if (j < 0)
                        {
                            if (!KeyNameReg.TryFindName(hash, out name))
                                name = KeyNameReg.UnhashName(hash);
                            ad = new ActorDefinition(name);
                            actorDefs.Add(ad);
                            this.mExtraActors.Add(ad);
                        }
                    }
                    animation.Actor = ad;
                    animations[index++] = animation;
                }
            }
            ulong clipHash;
            Dictionary<ulong, string> keyNameMap 
                = new Dictionary<ulong, string>();
            for (i = index - 1; i >= 0; i--)
            {
                animation = animations[i];
                this.mNamespaceMap.SetNamespaceMap(
                    animation.SrcFileName,
                    animation.Namespace,
                    animation.Actor);
                if (animation.SrcFileIsValid)
                {
                    clipHash = FNVHash.HashString64(animation.SrcFileName);
                    keyNameMap[clipHash] = animation.SrcFileName;
                }
            }
            // Update the Key to Filename Map of the Namespace Map
            SortedDictionary<RK, string> k2fn 
                = this.mNamespaceMap.KeyToFilenameMap;
            k2fn.Clear();
            for (i = rks.Count - 1; i >= 0; i--)
            {
                rk = rks[i];
                if (keyNameMap.TryGetValue(rk.IID, out name) ||
                    KeyNameReg.TryFindName(rk.IID, out name))
                {
                    k2fn[rk] = name;
                }
            }
            #endregion
        }

        private class Anim
        {
            public uint SrcFileHash;
            public string SrcFileName;
            public bool SrcFileIsValid;
            public string Namespace;
            public ActorDefinition Actor;
        }

        private ulong mInstanceId;

        public GenericRCOLResource ExportToResource(ulong instanceId, 
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            this.mInstanceId = instanceId;
            GenericRCOLResource rcol = new GenericRCOLResource(0, null);
            AChunkObject[] chunks = this.FindAllChunks(false);
            GenericRCOLResource.ChunkEntryList chunkEntries
                = rcol.ChunkEntries;
            for (int i = 0; i < chunks.Length; i++)
            {
                chunkEntries.Add(chunks[i].Export(nameMap, exportAllNames));
            }
            // TODO: Ensure the data in rcol is flushed to its Stream.
            // Currently this is done automatically when its Stream is
            // retrieved because adding to the chunk entry list triggers its
            // OnResourceChanged() listener, which marks it dirty, causing 
            // its Unparse() to be invoked when its Stream is retrieved.
            return rcol;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            if (nameMap == null)
            {
                throw new ArgumentNullException("nameMap");
            }
            System.IO.Stream s = null;
            TGIBlock tgi = new TGIBlock(0, null, "ITG", 
                ResourceType, 0, this.mInstanceId);
            JazzStateMachine jsm = new JazzStateMachine(0, null, s);
            jsm.NameHash = this.mNameHash;

            if (!this.bNameIsHash &&
                nameMap != null && !nameMap.ContainsKey(this.mNameHash) &&
                (exportAllNames || !KeyNameReg.HasName(this.mNameHash)))
            {
                nameMap[this.mNameHash] = this.mName;
            }

            ActorDefinition[] ads = this.mActorDefinitions.ToArray();
            Array.Sort(ads, 0, ads.Length, 
                ActorDefinition.NameComparer.Instance);
            JazzChunk.ChunkReferenceList actorList
                = jsm.ActorDefinitionIndexes;
            foreach (ActorDefinition ad in ads)
            {
                actorList.Add(ad == null ? NullCRef : ad.ChunkReference);
            }

            ParamDefinition[] pds = this.mParamDefinitions.ToArray();
            Array.Sort(pds, 0, pds.Length,
                ParamDefinition.NameComparer.Instance);
            JazzChunk.ChunkReferenceList paramList
                = jsm.PropertyDefinitionIndexes;
            foreach (ParamDefinition pd in pds)
            {
                paramList.Add(pd == null ? NullCRef : pd.ChunkReference);
            }

            State[] states = this.mStates.ToArray();
            Array.Sort(states, 0, states.Length, 
                State.NameComparer.Instance);
            JazzChunk.ChunkReferenceList stateList = jsm.StateIndexes;
            foreach (State state in states)
            {
                stateList.Add(
                    state == null ? NullCRef : state.ChunkReference);
            }

            uint hash;
            string name;
            ulong clipIID;
            NamespaceMap.AnimSource[] sources 
                = this.mNamespaceMap.GetSourceToNamespaceToActorArray();
            for (int i = 0; i < sources.Length; i++)
            {
                name = sources[i].FileName;
                if (!name.StartsWith("0x") ||
                    !uint.TryParse(name.Substring(2),
                        System.Globalization.NumberStyles.HexNumber,
                        null, out hash))
                {
                    clipIID = FNVHash.HashString64(name);
                    if (!nameMap.ContainsKey(clipIID))
                    {
                        nameMap[clipIID] = name;
                    }
                }
            }
            this.mNamespaceMap.UpdateKeyToFilenameMap(nameMap);
            this.mNamespaceMap.ExportToList(jsm.Animations, 
                nameMap, exportAllNames);

            jsm.Properties = this.mFlags;
            jsm.AutomationPriority = this.mDefaultPriority;
            jsm.AwarenessOverlayLevel = this.mAwarenessOverlayLevel;

            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jsm);
        }

        public void RefreshHashNames()
        {
            this.SlurpChunks();
            string name;
            if (this.bNameIsHash &&
                KeyNameReg.TryFindName(this.mNameHash, out name))
            {
                this.mName = name;
                this.bNameIsHash = false;
            }
            this.mNamespaceMap.RefreshHashNames();
            foreach (ActorDefinition actor in this.mActorDefinitions)
            {
                if (actor.NameIsHash &&
                    KeyNameReg.TryFindName(actor.NameHash, out name))
                {
                    actor.Name = name;
                }
            }
            foreach (ActorDefinition actor2 in this.mExtraActors)
            {
                if (actor2.NameIsHash &&
                    KeyNameReg.TryFindName(actor2.NameHash, out name))
                {
                    actor2.Name = name;
                }
            }
            foreach (ParamDefinition param in this.mParamDefinitions)
            {
                if (param.NameIsHash &&
                    KeyNameReg.TryFindName(param.NameHash, out name))
                {
                    param.Name = name;
                }
                if (param.DefaultIsHash &&
                    KeyNameReg.TryFindName(param.DefaultHash, out name))
                {
                    param.DefaultValue = name;
                }
            }
            foreach (ParamDefinition param2 in this.mExtraParams)
            {
                if (param2.NameIsHash &&
                    KeyNameReg.TryFindName(param2.NameHash, out name))
                {
                    param2.Name = name;
                }
                if (param2.DefaultIsHash &&
                    KeyNameReg.TryFindName(param2.DefaultHash, out name))
                {
                    param2.DefaultValue = name;
                }
            }
            foreach (State state in this.mStates)
            {
                if (state.NameIsHash &&
                    KeyNameReg.TryFindName(state.NameHash, out name))
                {
                    state.Name = name;
                }
            }
            foreach (State state2 in this.mExtraStates)
            {
                if (state2.NameIsHash &&
                    KeyNameReg.TryFindName(state2.NameHash, out name))
                {
                    state2.Name = name;
                }
            }
            SelectOnParameterNode sopn;
            foreach (DecisionGraphNode dgn in this.mDGNodes)
            {
                sopn = dgn as SelectOnParameterNode;
                if (sopn != null)
                {
                    sopn.RefreshHashNames();
                }
            }
            //this.mExtraActors.Clear();
            //this.mExtraActors.AddRange(extraActors);
            //this.mExtraParams.Clear();
            //this.mExtraParams.AddRange(extraParams);
            //this.mExtraStates.Clear();
            //this.mExtraStates.AddRange(extraStates);
        }

        private AChunkObject[] FindAllChunks(bool refreshHashNames)
        {
            this.SlurpChunks();
            string name;
            int count = 1 + this.mDGNodes.Count + this.mDGs.Count
                + this.mStates.Count + this.mExtraStates.Count
                + this.mParamDefinitions.Count + this.mExtraParams.Count
                + this.mActorDefinitions.Count + this.mExtraActors.Count;
            AChunkObject[] chunks = new AChunkObject[count];

            if (refreshHashNames)
            {
                if (this.bNameIsHash && 
                    KeyNameReg.TryFindName(this.mNameHash, out name))
                {
                    this.mName = name;
                    this.bNameIsHash = false;
                }
                this.mNamespaceMap.RefreshHashNames();
            }
            this.ChunkReference.TGIBlockIndex = -1;
            chunks[0] = this;
            count = 0;
            foreach (ActorDefinition actor in this.mActorDefinitions)
            {
                if (refreshHashNames && actor.NameIsHash &&
                    KeyNameReg.TryFindName(actor.NameHash, out name))
                {
                    actor.Name = name;
                }
                actor.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = actor;
            }
            foreach (ActorDefinition actor2 in this.mExtraActors)
            {
                if (refreshHashNames && actor2.NameIsHash &&
                    KeyNameReg.TryFindName(actor2.NameHash, out name))
                {
                    actor2.Name = name;
                }
                actor2.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = actor2;
            }
            foreach (ParamDefinition param in this.mParamDefinitions)
            {
                if (refreshHashNames)
                {
                    if (param.NameIsHash &&
                        KeyNameReg.TryFindName(param.NameHash, out name))
                    {
                        param.Name = name;
                    }
                    if (param.DefaultIsHash &&
                        KeyNameReg.TryFindName(param.DefaultHash, out name))
                    {
                        param.DefaultValue = name;
                    }
                }
                param.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = param;
            }
            foreach (ParamDefinition param2 in this.mExtraParams)
            {
                if (refreshHashNames)
                {
                    if (param2.NameIsHash &&
                        KeyNameReg.TryFindName(param2.NameHash, out name))
                    {
                        param2.Name = name;
                    }
                    if (param2.DefaultIsHash &&
                        KeyNameReg.TryFindName(param2.DefaultHash, out name))
                    {
                        param2.DefaultValue = name;
                    }
                }
                param2.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = param2;
            }
            foreach (State state in this.mStates)
            {
                if (refreshHashNames && state.NameIsHash &&
                    KeyNameReg.TryFindName(state.NameHash, out name))
                {
                    state.Name = name;
                }
                state.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = state;
            }
            foreach (State state2 in this.mExtraStates)
            {
                if (refreshHashNames && state2.NameIsHash &&
                    KeyNameReg.TryFindName(state2.NameHash, out name))
                {
                    state2.Name = name;
                }
                state2.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = state2;
            }
            foreach (DecisionGraph dg in this.mDGs)
            {
                dg.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = dg;
            }
            SelectOnParameterNode sopn;
            foreach (DecisionGraphNode dgn in this.mDGNodes)
            {
                if (refreshHashNames)
                {
                    sopn = dgn as SelectOnParameterNode;
                    if (sopn != null)
                    {
                        sopn.RefreshHashNames();
                    }
                }
                dgn.ChunkReference.TGIBlockIndex = count;
                chunks[++count] = dgn;
            }
            //this.mExtraActors.Clear();
            //this.mExtraActors.AddRange(extraActors);
            //this.mExtraParams.Clear();
            //this.mExtraParams.AddRange(extraParams);
            //this.mExtraStates.Clear();
            //this.mExtraStates.AddRange(extraStates);
            return chunks;
        }

        private void SlurpChunks()
        {
            int i, j, index;
            State state;
            State[] transitions;
            DecisionGraph dg;
            DecisionGraphNode target;
            DecisionGraphNode[] targets;
            //ActorDefinition[] extraActors = this.mExtraActors.ToArray();
            //ParamDefinition[] extraParams = this.mExtraParams.ToArray();
            //State[] extraStates = this.mExtraStates.ToArray();
            this.mExtraActors.Clear();
            this.mExtraParams.Clear();
            this.mExtraStates.Clear();
            this.mDGs.Clear();
            this.mDGNodes.Clear();
            KeyNameReg.RefreshKeyNameMaps();

            ActorDefinition ad;
            NamespaceMap.AnimNamespace[] nss;
            NamespaceMap.AnimSource[] sources
                = this.mNamespaceMap.GetSourceToNamespaceToActorArray();
            for (i = sources.Length - 1; i >= 0; i--)
            {
                nss = sources[i].Namespaces;
                for (j = nss.Length; j >= 0; j--)
                {
                    ad = nss[j].Actor;
                    if (ad != null)
                    {
                        index = this.mActorDefinitions.IndexOf(ad);
                        if (index < 0)
                        {
                            index = this.mExtraActors.IndexOf(ad);
                            if (index < 0)
                            {
                                this.mExtraActors.Add(ad);
                            }
                        }
                    }
                }
            }
            // this.mStates.Count needs to be reread every iteration
            // because it could be expanded with slurped up States.
            for (j = 0; j < this.mStates.Count; j++)
            {
                state = this.mStates[j];
                dg = state.DecisionGraph;
                if (dg != null)
                {
                    index = this.mDGs.IndexOf(dg);
                    if (index < 0)
                    {
                        this.mDGs.Add(dg);
                    }
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                    if (dg.EntryPointCount > 0)
                    {
                        targets = dg.EntryPoints;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = transitions.Length - 1; i >= 0; i--)
                {
                    state = transitions[i];
                    if (state != null)
                    {
                        index = this.mStates.IndexOf(state);
                        if (index < 0)
                        {
                            index = this.mExtraStates.IndexOf(state);
                            if (index < 0)
                            {
                                this.mExtraStates.Add(state);
                            }
                        }
                    }
                }
            }
            // this.mExtraStates.Count needs to be reread every iteration
            // because it could be expanded with slurped up States.
            for (j = 0; j < this.mExtraStates.Count; j++)
            {
                state = this.mExtraStates[j];
                dg = state.DecisionGraph;
                if (dg != null)
                {
                    index = this.mDGs.IndexOf(dg);
                    if (index < 0)
                    {
                        this.mDGs.Add(dg);
                    }
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                    if (dg.EntryPointCount > 0)
                    {
                        targets = dg.EntryPoints;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = transitions.Length - 1; i >= 0; i--)
                {
                    state = transitions[i];
                    if (state != null)
                    {
                        index = this.mStates.IndexOf(state);
                        if (index < 0)
                        {
                            index = this.mExtraStates.IndexOf(state);
                            if (index < 0)
                            {
                                this.mExtraStates.Add(state);
                            }
                        }
                    }
                }
            }
        }

        private void SlurpDGNodes(DecisionGraphNode node)
        {
            this.mDGNodes.Add(node);
            int i, j;
            State state;
            DecisionGraphNode target;
            DecisionGraphNode[] targets;
            if (node is NextStateNode)
            {
                NextStateNode nsn = node as NextStateNode;
                state = nsn.NextState;
                if (state != null)
                {
                    i = this.mStates.IndexOf(state);
                    if (i < 0)
                    {
                        i = this.mExtraStates.IndexOf(state);
                        if (i < 0)
                        {
                            this.mExtraStates.Add(state);
                        }
                    }
                }
            }
            else if (node is RandomNode)
            {
                RandomNode rand = node as RandomNode;
                List<RandomNode.Slice> slices = rand.Slices;
                if (slices.Count > 0)
                {
                    for (i = slices.Count - 1; i >= 0; i--)
                    {
                        targets = slices[i].Targets.ToArray();
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                }
            }
            else if (node is SelectOnDestinationNode)
            {
                SelectOnDestinationNode sodn = node as SelectOnDestinationNode;
                if (sodn.CaseCount > 0)
                {
                    SelectOnDestinationNode.Case[] cases = sodn.Cases;
                    for (i = 0; i < cases.Length; i++)
                    {
                        targets = cases[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                        state = cases[i].Value;
                        if (state != null)
                        {
                            j = this.mStates.IndexOf(state);
                            if (j < 0)
                            {
                                j = this.mExtraStates.IndexOf(state);
                                if (j < 0)
                                {
                                    this.mExtraStates.Add(state);
                                }
                            }
                        }
                    }
                }
            }
            else if (node is SelectOnParameterNode)
            {
                SelectOnParameterNode sopn = node as SelectOnParameterNode;
                if (sopn.CaseCount > 0)
                {
                    SelectOnParameterNode.Case[] cases = sopn.Cases;
                    for (i = 0; i < cases.Length; i++)
                    {
                        targets = cases[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                }
                ParamDefinition param = sopn.Parameter;
                if (param != null)
                {
                    i = this.mParamDefinitions.IndexOf(param);
                    if (i < 0)
                    {
                        i = this.mExtraParams.IndexOf(param);
                        if (i < 0)
                        {
                            this.mExtraParams.Add(param);
                        }
                    }
                }
            }
            else if (node is MulticastDecisionGraphNode)
            {
                MulticastDecisionGraphNode mcdgn 
                    = node as MulticastDecisionGraphNode;
                if (mcdgn.TargetCount > 0)
                {
                    targets = mcdgn.Targets;
                    for (i = 0; i < targets.Length; i++)
                    {
                        target = targets[i];
                        if (target != null &&
                            !this.mDGNodes.Contains(target))
                        {
                            this.SlurpDGNodes(target);
                        }
                    }
                }
                if (node is CreatePropNode)
                {
                    CreatePropNode cpn = node as CreatePropNode;
                    ActorDefinition propActor = cpn.PropActor;
                    if (propActor != null)
                    {
                        i = this.mActorDefinitions.IndexOf(propActor);
                        if (i < 0)
                        {
                            i = this.mExtraActors.IndexOf(propActor);
                            if (i < 0)
                            {
                                this.mExtraActors.Add(propActor);
                            }
                        }
                    }
                    ParamDefinition propParam = cpn.PropParam;
                    if (propParam != null)
                    {
                        i = this.mParamDefinitions.IndexOf(propParam);
                        if (i < 0)
                        {
                            i = this.mExtraParams.IndexOf(propParam);
                            if (i < 0)
                            {
                                this.mExtraParams.Add(propParam);
                            }
                        }
                    }
                }
                else if (node is ActorOperationNode)
                {
                    ActorOperationNode aon = node as ActorOperationNode;
                    ActorDefinition aTarget = aon.Actor;
                    if (aTarget != null)
                    {
                        i = this.mActorDefinitions.IndexOf(aTarget);
                        if (i < 0)
                        {
                            i = this.mExtraActors.IndexOf(aTarget);
                            if (i < 0)
                            {
                                this.mExtraActors.Add(aTarget);
                            }
                        }
                    }
                }
                else if (node is AnimationNode)
                {
                    AnimationNode an = node as AnimationNode;
                    ActorDefinition ad = an.Actor;
                    if (ad != null)
                    {
                        i = this.mActorDefinitions.IndexOf(ad);
                        if (i < 0)
                        {
                            i = this.mExtraActors.IndexOf(ad);
                            if (i < 0)
                            {
                                this.mExtraActors.Add(ad);
                            }
                        }
                    }
                    if (node is PlayAnimationNode)
                    {
                        PlayAnimationNode pan = node as PlayAnimationNode;
                        SlotSetupBuilder ssb = pan.SlotSetup;
                        if (ssb.ActorSuffixCount > 0)
                        {
                            ParamDefinition pd;
                            SlotSetupBuilder.ActorSuffix[] suffixes
                                = ssb.ActorSuffixArray;
                            for (i = 0; i < suffixes.Length; i++)
                            {
                                ad = suffixes[i].Actor;
                                if (ad != null)
                                {
                                    j = this.mActorDefinitions.IndexOf(ad);
                                    if (j < 0)
                                    {
                                        j = this.mExtraActors.IndexOf(ad);
                                        if (j < 0)
                                        {
                                            this.mExtraActors.Add(ad);
                                        }
                                    }
                                }
                                pd = suffixes[i].Param;
                                if (pd != null)
                                {
                                    j = this.mParamDefinitions.IndexOf(pd);
                                    if (j < 0)
                                    {
                                        j = this.mExtraParams.IndexOf(pd);
                                        if (j < 0)
                                        {
                                            this.mExtraParams.Add(pd);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Recursively finds all of the unique resource keys referenced by
        /// every <see cref="DecisionGraphNode"/> instance in every state in
        /// this state machine.
        /// </summary>
        /// <returns>A list of all unique resource keys referenced by nodes
        /// in this state machine.</returns>
        public List<RK> SlurpReferencedRKs()
        {
            List<RK> rks = new List<RK>();
            int i, j;
            State state;
            State[] transitions;
            DecisionGraph dg;
            DecisionGraphNode target;
            DecisionGraphNode[] targets;
            this.mExtraStates.Clear();
            this.mDGNodes.Clear();
            for (j = 0; j < this.mStates.Count; j++)
            {
                state = this.mStates[j];
                dg = state.DecisionGraph;
                if (dg != null)
                {
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                    if (dg.EntryPointCount > 0)
                    {
                        targets = dg.EntryPoints;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = transitions.Length - 1; i >= 0; i--)
                {
                    state = transitions[i];
                    if (state != null)
                    {
                        if (!this.mStates.Contains(state) &&
                            !this.mExtraStates.Contains(state))
                        {
                            this.mExtraStates.Add(state);
                        }
                    }
                }
            }
            for (j = 0; j < this.mExtraStates.Count; j++)
            {
                state = this.mExtraStates[j];
                dg = state.DecisionGraph;
                if (dg != null)
                {
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                    if (dg.EntryPointCount > 0)
                    {
                        targets = dg.EntryPoints;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = transitions.Length - 1; i >= 0; i--)
                {
                    state = transitions[i];
                    if (state != null)
                    {
                        if (!this.mStates.Contains(state) &&
                            !this.mExtraStates.Contains(state))
                        {
                            this.mExtraStates.Add(state);
                        }
                    }
                }
            }
            return rks;
        }

        /// <summary>
        /// Recursively finds all of the unique resource keys referenced by
        /// the given <paramref name="node"/> (if it is a 
        /// <see cref="CreatePropNode"/> or <see cref="PlayAnimationNode"/>)
        /// and by all of the <see cref="DecisionGraphNode"/> instances it 
        /// references, and adds them to the given resource key list.
        /// </summary>
        /// <param name="node">The decision graph node to recursively search
        /// for external resource key references.</param>
        /// <param name="rks">The list of all unique resource keys referenced
        /// by the given <paramref name="node"/> and every 
        /// <see cref="DecisionGraphNode"/> instance it references.</param>
        private void SlurpDGNodeRKs(DecisionGraphNode node, List<RK> rks)
        {
            this.mDGNodes.Add(node);
            int i, j;
            State state;
            DecisionGraphNode target;
            DecisionGraphNode[] targets; 
            if (node is NextStateNode)
            {
                NextStateNode nsn = node as NextStateNode;
                state = nsn.NextState;
                if (state != null)
                {
                    i = this.mStates.IndexOf(state);
                    if (i < 0)
                    {
                        i = this.mExtraStates.IndexOf(state);
                        if (i < 0)
                        {
                            this.mExtraStates.Add(state);
                        }
                    }
                }
            }
            else if (node is RandomNode)
            {
                RandomNode rand = node as RandomNode;
                List<RandomNode.Slice> slices = rand.Slices;
                if (slices.Count > 0)
                {
                    for (i = slices.Count - 1; i >= 0; i--)
                    {
                        targets = slices[i].Targets.ToArray();
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
            }
            else if (node is SelectOnDestinationNode)
            {
                SelectOnDestinationNode sodn = node as SelectOnDestinationNode;
                if (sodn.CaseCount > 0)
                {
                    SelectOnDestinationNode.Case[] cases = sodn.Cases;
                    for (i = 0; i < cases.Length; i++)
                    {
                        targets = cases[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                        state = cases[i].Value;
                        if (state != null)
                        {
                            j = this.mStates.IndexOf(state);
                            if (j < 0)
                            {
                                j = this.mExtraStates.IndexOf(state);
                                if (j < 0)
                                {
                                    this.mExtraStates.Add(state);
                                }
                            }
                        }
                    }
                }
            }
            else if (node is SelectOnParameterNode)
            {
                SelectOnParameterNode sopn = node as SelectOnParameterNode;
                if (sopn.CaseCount > 0)
                {
                    SelectOnParameterNode.Case[] cases = sopn.Cases;
                    for (i = 0; i < cases.Length; i++)
                    {
                        targets = cases[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
            }
            else if (node is MulticastDecisionGraphNode)
            {
                MulticastDecisionGraphNode mcdgn
                    = node as MulticastDecisionGraphNode;
                if (mcdgn.TargetCount > 0)
                {
                    targets = mcdgn.Targets;
                    for (i = 0; i < targets.Length; i++)
                    {
                        target = targets[i];
                        if (target != null &&
                            !this.mDGNodes.Contains(target))
                        {
                            this.SlurpDGNodeRKs(target, rks);
                        }
                    }
                }
                if (node is CreatePropNode)
                {
                    CreatePropNode cpn = node as CreatePropNode;
                    RK propKey = cpn.PropKey;
                    if (!rks.Contains(propKey))
                    {
                        rks.Add(propKey);
                    }
                }
                else if (node is PlayAnimationNode)
                {
                    PlayAnimationNode lan = node as PlayAnimationNode;
                    RK key = lan.ClipKey;
                    if (!rks.Contains(key))
                    {
                        rks.Add(key);
                    }
                    key = lan.AdditiveClipKey;
                    if (!rks.Contains(key))
                    {
                        rks.Add(key);
                    }
                    key = lan.TrackMaskKey;
                    if (!rks.Contains(key))
                    {
                        rks.Add(key);
                    }
                }
            }
        }

        public string Name
        {
            get { return this.mName; }
            set
            {
                if (value == null)
                {
                    this.mName = null;
                    this.mNameHash = 0;
                    this.bNameIsHash = true;
                }
                else if (!value.Equals(this.mName))
                {
                    this.mName = value;
                    if (!value.StartsWith("0x") ||
                        !uint.TryParse(value.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out this.mNameHash))
                    {
                        this.mNameHash = FNVHash.HashString32(value);
                        this.bNameIsHash = false;
                    }
                    else
                    {
                        this.bNameIsHash = true;
                    }
                }
            }
        }

        public uint NameHash
        {
            get { return this.mNameHash; }
        }

        public bool NameIsHash
        {
            get { return this.bNameIsHash; }
        }

        public bool AddActorDefinition(ActorDefinition actor)
        {
            if (this.mActorDefinitions.Contains(actor))
            {
                return false;
            }
            this.mActorDefinitions.Add(actor);
            return true;
        }

        public bool RemoveActorDefinition(ActorDefinition actor)
        {
            int index = this.mActorDefinitions.IndexOf(actor);
            if (index < 0)
            {
                return false;
            }
            this.mActorDefinitions.RemoveAt(index);
            return true;
        }

        public void ClearActorDefinitions()
        {
            this.mActorDefinitions.Clear();
        }

        public int ActorDefinitionCount
        {
            get { return this.mActorDefinitions.Count; }
        }

        public ActorDefinition[] ActorDefinitions
        {
            get { return this.mActorDefinitions.ToArray(); }
        }

        public bool AddParamDefinition(ParamDefinition param)
        {
            if (this.mParamDefinitions.Contains(param))
            {
                return false;
            }
            this.mParamDefinitions.Add(param);
            return true;
        }

        public bool RemoveParamDefinition(ParamDefinition param)
        {
            int index = this.mParamDefinitions.IndexOf(param);
            if (index < 0)
            {
                return false;
            }
            this.mParamDefinitions.RemoveAt(index);
            return true;
        }

        public void ClearParamDefinitions()
        {
            this.mParamDefinitions.Clear();
        }

        public int ParamDefinitionCount
        {
            get { return this.mParamDefinitions.Count; }
        }

        public ParamDefinition[] ParamDefinitions
        {
            get { return this.mParamDefinitions.ToArray(); }
        }

        public bool AddState(State state)
        {
            if (this.mStates.Contains(state))
            {
                return false;
            }
            this.mStates.Add(state);
            state.StateMachine = this;
            return true;
        }

        public bool RemoveState(State state)
        {
            int index = this.mStates.IndexOf(state);
            if (index < 0)
            {
                return false;
            }
            this.mStates.RemoveAt(index);
            state.StateMachine = null;
            return true;
        }

        public int StateCount
        {
            get { return this.mStates.Count; }
        }

        public State[] States
        {
            get { return this.mStates.ToArray(); }
        }

        public JazzStateMachine.Flags Flags
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

        public void SetFlags(JazzStateMachine.Flags flags, bool value)
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

        public JazzChunk.AnimationPriority DefaultPriority
        {
            get { return this.mDefaultPriority; }
            set
            {
                if (this.mDefaultPriority != value)
                {
                    this.mDefaultPriority = value;
                }
            }
        }

        public JazzChunk.AwarenessLevel AwarenessOverlayLevel
        {
            get { return this.mAwarenessOverlayLevel; }
            set
            {
                if (this.mAwarenessOverlayLevel != value)
                {
                    this.mAwarenessOverlayLevel = value;
                }
            }
        }
    }
}
