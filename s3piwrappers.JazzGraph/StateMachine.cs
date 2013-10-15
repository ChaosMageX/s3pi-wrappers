using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class StateMachine : AChunkObject
    {
        public const uint ResourceType = 0x02D5DF13;
        public const string ResourceTag = "S_SM";

        private string mName;
        private uint mNameHash;
        private bool bNameIsHash;
        private List<ActorDefinition> mActorDefinitions;
        private List<ParamDefinition> mParameterDefinitions;
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
            this.mParameterDefinitions = new List<ParamDefinition>();
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
            List<ParamDefinition> paramDefs 
                = new List<ParamDefinition>();
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
                        lan.ClipKey = jpan.ClipResource;
                        lan.TrackMaskKey = jpan.TkmkResource;
                        // TODO: Copy over slots
                        lan.AdditiveClipKey = jpan.AdditiveClipResource;
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
                        cpn.PropKey = jcpn.PropResource;
                        chunks[i] = cpn;
                        break;
                    case RandomNode.ResourceType:
                        JazzRandomNode jrand
                            = ce.RCOLBlock as JazzRandomNode;
                        RandomNode rand = new RandomNode();
                        j = 0;
                        foreach (JazzRandomNode.Outcome oc in jrand.Outcomes)
                        {
                            rand.SetSliceWeight(j++, oc.Weight);
                        }
                        // rand.Slices[].Targets set later
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
                        cpn.PropParameter = index < 0
                            ? null : chunks[index + 1] as ParamDefinition;
                        break;
                    case RandomNode.ResourceType:
                        JazzRandomNode jrand = ce.RCOLBlock as JazzRandomNode;
                        RandomNode rand = chunks[i] as RandomNode;
                        j = 0;
                        foreach (JazzRandomNode.Outcome oc in jrand.Outcomes)
                        {
                            foreach (GenericRCOLResource.ChunkReference cr
                                in oc.DecisionGraphIndexes)
                            {
                                index = cr.TGIBlockIndex;
                                dgn = index < 0 ? null 
                                    : chunks[index + 1] as DecisionGraphNode;
                                rand.AddSliceTarget(j, dgn);
                            }
                            j++;
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
                            this.mActorDefinitions.Add(ad);
                        }
                        foreach (GenericRCOLResource.ChunkReference jpd
                            in jazzSM.PropertyDefinitionIndexes)
                        {
                            index = jpd.TGIBlockIndex;
                            pd = index < 0 ? null 
                                : chunks[index + 1] as ParamDefinition;
                            this.mParameterDefinitions.Add(pd);
                        }
                        foreach (GenericRCOLResource.ChunkReference jst
                            in jazzSM.StateIndexes)
                        {
                            index = jst.TGIBlockIndex;
                            state = index < 0  ? null 
                                : chunks[index + 1] as State;
                            this.mStates.Add(state);
                        }
                        break;
                }
            }
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
                        index = this.mParameterDefinitions.IndexOf(pd);
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
            List<IResourceKey> rks = this.SlurpReferencedRKs();
            List<uint> foldedClipInstances = new List<uint>(rks.Count);
            foreach (IResourceKey rk in rks)
            {
                if (rk.ResourceType == 0x6b20c4f3)
                {
                    hash = (uint)((rk.Instance >> 0x20) ^ (rk.Instance & 0xffffffffUL));
                    if (!foldedClipInstances.Contains(hash))
                    {
                        foldedClipInstances.Add(hash);
                    }
                }
            }
            Anim animation;
            //string prevAnim, nextAnim;
            List<Anim> animations = new List<Anim>(jazzSM.Animations.Count);
            foreach (JazzStateMachine.Animation anim in jazzSM.Animations)
            {
                hash = anim.NameHash;
                if (!foldedClipInstances.Contains(hash))
                {
                    animation = new Anim();
                    animation.SourceFileHash = hash;
                    if (KeyNameReg.TryFindName(hash, out name))
                    {
                        animation.SourceFileName = name;
                    }
                    else
                    {
                        animation.SourceFileName 
                            = KeyNameReg.UnhashName(hash);
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
                    animation.Actor = ad;
                }
            }
            /*prevAnim = null;
            for (i = 0; i < animations.Count; i++)
            {
                animation = animations[i];
                if (animation.SourceFileHash != 0 &&
                    animation.SourceFileName == null)
                {
                    nextAnim = null;
                    for (j = i + 1; 
                         j < animations.Count && nextAnim == null; j++)
                    {
                        animation = animations[j];
                        if (animation.SourceFileHash != 0 &&
                            animation.SourceFileName != null)
                        {
                            nextAnim = animation.SourceFileName;
                        }
                    }
                    animation = animations[i];
                    hash = animation.SourceFileHash;
                    if (nextAnim == null)
                    {
                        if (prevAnim == null)
                        {
                            animation.SourceFileName 
                                = UnhashName(hash);
                        }
                        else
                        {
                            name = "";
                            for (j = 0; j < prevAnim.Length; j++)
                            {
                                if (prevAnim[j] < 0xff)
                                {
                                    name += ASCIIHelper.GetNextVisualChar(prevAnim[j]);
                                    break;
                                }
                                name += prevAnim[j];
                            }
                            if (j == prevAnim.Length)
                            {
                                name += "!";
                            }
                            animation.SourceFileName 
                                = UnhashName(hash, name);
                        }
                    }
                    else if (prevAnim == null)
                    {
                        animation.SourceFileName 
                            = UnhashName(hash, "!");
                    }
                    else if (prevAnim.Equals(nextAnim))
                    {
                        animation.SourceFileName = prevAnim;
                    }
                    else
                    {
                        name = "";
                        index = Math.Min(prevAnim.Length, nextAnim.Length);
                        for (j = 0; j < index; j++)
                        {
                            if (prevAnim[j] != nextAnim[j])
                            {
                                break;
                            }
                            name += prevAnim[j];
                        }
                    }
                    prevAnim = animation.SourceFileName;
                }
                else if (animation.SourceFileHash != 0 &&
                         animation.SourceFileName != null)
                {
                    prevAnim = animation.SourceFileName;
                }
            }/* */
            ulong clipHash;
            Dictionary<ulong, string> knm = new Dictionary<ulong, string>();
            for (i = 0; i < animations.Count; i++)
            {
                animation = animations[i];
                this.mNamespaceMap.SetNamespaceMap(
                    animation.SourceFileName,
                    animation.Namespace, 
                    animation.Actor);
                clipHash = FNVHash.HashString64(animation.SourceFileName);
                if (!knm.TryGetValue(clipHash, out name) &&
                    !KeyNameReg.TryFindName(clipHash, out name))
                {
                    knm[clipHash] = animation.SourceFileName;
                }
            }
            this.mNamespaceMap.UpdateKeyToFilenameMap(knm);
        }

        private class Anim
        {
            public uint SourceFileHash;
            public string SourceFileName;
            public string Namespace;
            public ActorDefinition Actor;
        }

        private ulong mInstanceId;

        public GenericRCOLResource ExportToResource(ulong instanceId, 
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            this.mInstanceId = instanceId;
            GenericRCOLResource rcol = new GenericRCOLResource(0, null);
            AChunkObject[] chunks = this.SlurpChunks();
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
            
            this.mActorDefinitions.Sort(
                ActorDefinition.NameComparer.Instance);
            JazzChunk.ChunkReferenceList actorList
                = jsm.ActorDefinitionIndexes;
            foreach (ActorDefinition ad in this.mActorDefinitions)
            {
                actorList.Add(ad == null ? NullCRef : ad.ChunkReference);
            }

            this.mParameterDefinitions.Sort(
                ParamDefinition.NameComparer.Instance);
            JazzChunk.ChunkReferenceList paramList
                = jsm.PropertyDefinitionIndexes;
            foreach (ParamDefinition pd in this.mParameterDefinitions)
            {
                paramList.Add(pd == null ? NullCRef : pd.ChunkReference);
            }

            this.mStates.Sort(State.NameComparer.Instance);
            JazzChunk.ChunkReferenceList stateList = jsm.StateIndexes;
            foreach (State state in this.mStates)
            {
                stateList.Add(
                    state == null ? NullCRef : state.ChunkReference);
            }

            ulong hash;
            string name;
            NamespaceMap.AnimSource[] sources 
                = this.mNamespaceMap.GetSourceToNamespaceToActorArray();
            for (int i = 0; i < sources.Length; i++)
            {
                hash = FNVHash.HashString64(sources[i].FileName);
                if (!nameMap.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    nameMap[hash] = sources[i].FileName;
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

        private AChunkObject[] SlurpChunks()
        {
            uint hash;
            //string name;
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
                                this.mDGNodes.Add(target);
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
                                this.mDGNodes.Add(target);
                                this.SlurpDGNodes(target);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = 0; i < transitions.Length; i++)
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
                                this.mDGNodes.Add(target);
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
                                this.mDGNodes.Add(target);
                            }
                        }
                    }
                }
                transitions = state.Transitions;
                for (i = 0; i < transitions.Length; i++)
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
            index = 1 + this.mDGNodes.Count + this.mDGs.Count
                + this.mStates.Count + this.mExtraStates.Count
                + this.mParameterDefinitions.Count + this.mExtraParams.Count
                + this.mActorDefinitions.Count + this.mExtraActors.Count;
            AChunkObject[] chunks = new AChunkObject[index];

            /*if (!KeyNameReg.Current.TryGetValue(this.mNameHash, out name) &&
                !KeyNameReg.TryFindName(this.mNameHash, out name))
            {
                KeyNameReg.Current[this.mNameHash] = this.mName;
            }/* */
            this.ChunkReference.TGIBlockIndex = -1;
            chunks[0] = this;
            index = 0;
            foreach (ActorDefinition actor in this.mActorDefinitions)
            {
                hash = actor.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = actor.Name;
                }/* */
                actor.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = actor;
            }
            foreach (ActorDefinition actor2 in this.mExtraActors)
            {
                hash = actor2.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = actor2.Name;
                }/* */
                actor2.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = actor2;
            }
            foreach (ParamDefinition param in this.mParameterDefinitions)
            {
                hash = param.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = param.Name;
                }
                hash = param.DefaultHash;
                if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = param.DefaultValue;
                }/* */
                param.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = param;
            }
            foreach (ParamDefinition param2 in this.mExtraParams)
            {
                hash = param2.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = param2.Name;
                }
                hash = param2.DefaultHash;
                if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = param2.DefaultValue;
                }/* */
                param2.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = param2;
            }
            foreach (State state3 in this.mStates)
            {
                hash = state3.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = state3.Name;
                }/* */
                state3.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = state3;
            }
            foreach (State state4 in this.mExtraStates)
            {
                hash = state4.NameHash;
                /*if (!KeyNameReg.Current.TryGetValue(hash, out name) &&
                    !KeyNameReg.TryFindName(hash, out name))
                {
                    KeyNameReg.Current[hash] = state4.Name;
                }/* */
                state4.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = state4;
            }
            foreach (DecisionGraph dg2 in this.mDGs)
            {
                dg2.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = dg2;
            }
            foreach (DecisionGraphNode dgn in this.mDGNodes)
            {
                dgn.ChunkReference.TGIBlockIndex = index;
                chunks[++index] = dgn;
            }
            //this.mExtraActors.Clear();
            //this.mExtraActors.AddRange(extraActors);
            //this.mExtraParams.Clear();
            //this.mExtraParams.AddRange(extraParams);
            //this.mExtraStates.Clear();
            //this.mExtraStates.AddRange(extraStates);
            return chunks;
        }

        private void SlurpDGNodes(DecisionGraphNode node)
        {
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
                if (rand.SliceCount > 0)
                {
                    RandomNode.Slice[] slices = rand.Slices;
                    for (i = 0; i < slices.Length; i++)
                    {
                        targets = slices[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null &&
                                !this.mDGNodes.Contains(target))
                            {
                                this.mDGNodes.Add(target);
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
                                this.mDGNodes.Add(target);
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
                //uint cHash;
                //string cValue;
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
                                this.mDGNodes.Add(target);
                                this.SlurpDGNodes(target);
                            }
                        }
                        /*cValue = cases[i].Value;
                        cHash = FNVHash.HashString32(cValue);
                        if (!KeyNameReg.Current.TryGetValue(cHash, out cValue) &&
                            !KeyNameReg.TryFindName(cHash, out cValue))
                        {
                            KeyNameReg.Current[cHash] = cases[i].Value;
                        }/* */
                    }
                }
                ParamDefinition param = sopn.Parameter;
                if (param != null)
                {
                    i = this.mParameterDefinitions.IndexOf(param);
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
                            this.mDGNodes.Add(target);
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
                    ParamDefinition propParam = cpn.PropParameter;
                    if (propParam != null)
                    {
                        i = this.mParameterDefinitions.IndexOf(propParam);
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
                                j = this.mActorDefinitions.IndexOf(ad);
                                if (j < 0)
                                {
                                    j = this.mExtraActors.IndexOf(ad);
                                    if (j < 0)
                                    {
                                        this.mExtraActors.Add(ad);
                                    }
                                }
                                pd = suffixes[i].Param;
                                j = this.mParameterDefinitions.IndexOf(pd);
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

        public List<IResourceKey> SlurpReferencedRKs()
        {
            List<IResourceKey> rks = new List<IResourceKey>();
            int i;
            DecisionGraph dg;
            DecisionGraphNode target;
            DecisionGraphNode[] targets;
            foreach (State state in this.mStates)
            {
                dg = state.DecisionGraph;
                if (dg != null)
                {
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null)
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
                            if (target != null)
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
            }
            foreach (State state2 in this.mExtraStates)
            {
                dg = state2.DecisionGraph;
                if (dg != null)
                {
                    if (dg.DecisionMakerCount > 0)
                    {
                        targets = dg.DecisionMakers;
                        for (i = 0; i < targets.Length; i++)
                        {
                            target = targets[i];
                            if (target != null)
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
                            if (target != null)
                            {
                                this.SlurpDGNodeRKs(target, rks);
                            }
                        }
                    }
                }
            }
            return rks;
        }

        private void SlurpDGNodeRKs(DecisionGraphNode node, 
            List<IResourceKey> rks)
        {
            int i, j;
            DecisionGraphNode target;
            DecisionGraphNode[] targets;
            if (node is RandomNode)
            {
                RandomNode rand = node as RandomNode;
                if (rand.SliceCount > 0)
                {
                    RandomNode.Slice[] slices = rand.Slices;
                    for (i = 0; i < slices.Length; i++)
                    {
                        targets = slices[i].Targets;
                        for (j = 0; j < targets.Length; j++)
                        {
                            target = targets[j];
                            if (target != null)
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
                            if (target != null)
                            {
                                this.SlurpDGNodeRKs(target, rks);
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
                            if (target != null)
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
                        if (target != null)
                        {
                            this.SlurpDGNodeRKs(target, rks);
                        }
                    }
                }
                if (node is CreatePropNode)
                {
                    CreatePropNode cpn = node as CreatePropNode;
                    IResourceKey propKey = cpn.PropKey;
                    if (!rks.Contains(propKey))
                    {
                        rks.Add(propKey);
                    }
                }
                else if (node is PlayAnimationNode)
                {
                    PlayAnimationNode lan = node as PlayAnimationNode;
                    IResourceKey key = lan.ClipKey;
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

        public int ActorDefinitionCount
        {
            get { return this.mActorDefinitions.Count; }
        }

        public ActorDefinition[] ActorDefinitions
        {
            get { return this.mActorDefinitions.ToArray(); }
        }

        public bool AddParameterDefinition(ParamDefinition param)
        {
            if (this.mParameterDefinitions.Contains(param))
            {
                return false;
            }
            this.mParameterDefinitions.Add(param);
            return true;
        }

        public bool RemoveParameterDefinition(ParamDefinition param)
        {
            int index = this.mParameterDefinitions.IndexOf(param);
            if (index < 0)
            {
                return false;
            }
            this.mParameterDefinitions.RemoveAt(index);
            return true;
        }

        public int ParameterDefinitionCount
        {
            get { return this.mParameterDefinitions.Count; }
        }

        public ParamDefinition[] ParameterDefinitions
        {
            get { return this.mParameterDefinitions.ToArray(); }
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
