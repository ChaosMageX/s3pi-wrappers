using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class State : AChunkObject
    {
        public const uint ResourceType = 0x02EEDAFE;
        public const string ResourceTag = "S_St";

        public class NameComparer : IComparer<State>
        {
            public static readonly NameComparer Instance
                = new NameComparer();

            private StringComparer mComparer 
                = StringComparer.OrdinalIgnoreCase;

            public int Compare(State x, State y)
            {
                return this.mComparer.Compare(x.mName, y.mName);
            }
        }

        private StateMachine mStateMachine;

        private string mName;
        private uint mNameHash;
        private bool bNameIsHash;
        private JazzState.Flags mFlags;
        private DecisionGraph mDecisionGraph;
        private List<State> mTransitions;
        private JazzChunk.AwarenessLevel mAwarenessOverlayLevel;

        public State(string name)
            : base(ResourceType, ResourceTag)
        {
            this.mStateMachine = null;
            
            this.mName = name;
            if (name == null)
            {
                this.mNameHash = 0;
                this.bNameIsHash = true;
            }
            else if (!name.StartsWith("0x") ||
                !uint.TryParse(name.Substring(2),
                    System.Globalization.NumberStyles.HexNumber,
                    null, out this.mNameHash))
            {
                this.mNameHash = FNVHash.HashString32(name);
                this.bNameIsHash = false;
            }
            else
            {
                this.bNameIsHash = true;
            }
            this.mFlags = JazzState.Flags.None;
            this.mDecisionGraph = null;
            this.mTransitions = new List<State>();
            this.mAwarenessOverlayLevel = JazzChunk.AwarenessLevel.Unset;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzState js = new JazzState(0, null, s);
            js.NameHash = this.mNameHash;
            js.Properties = this.mFlags;
            js.DecisionGraphIndex = this.mDecisionGraph == null
                ? NullCRef : this.mDecisionGraph.ChunkReference;
            this.mTransitions.Sort(NameComparer.Instance);
            JazzChunk.ChunkReferenceList osi = js.OutboundStateIndexes;
            foreach (State state in this.mTransitions)
            {
                osi.Add(state == null ? NullCRef : state.ChunkReference);
            }
            js.AwarenessOverlayLevel = this.mAwarenessOverlayLevel;

            if (!this.bNameIsHash &&
                nameMap != null && !nameMap.ContainsKey(this.mNameHash) &&
                (exportAllNames || !KeyNameReg.HasName(this.mNameHash)))
            {
                nameMap[this.mNameHash] = this.mName;
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, js);
        }

        public StateMachine StateMachine
        {
            get { return this.mStateMachine; }
            set
            {
                if (this.mStateMachine != value)
                {
                    if (this.mStateMachine != null)
                    {
                        this.mStateMachine.RemoveState(this);
                    }
                    if (value != null)
                    {
                        value.AddState(this);
                    }
                    this.mStateMachine = value;
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

        public JazzState.Flags Flags
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

        public bool GetFlags(JazzState.Flags flags)
        {
            return (this.mFlags & flags) != JazzState.Flags.None;
        }

        public void SetFlags(JazzState.Flags flags, bool value)
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

        public bool Public
        {
            get { return this.GetFlags(JazzState.Flags.Public); }
            set { this.SetFlags(JazzState.Flags.Public, value); }
        }

        public bool Entry
        {
            get { return this.GetFlags(JazzState.Flags.Entry); }
            set { this.SetFlags(JazzState.Flags.Entry, value); }
        }

        public bool Exit
        {
            get { return this.GetFlags(JazzState.Flags.Exit); }
            set { this.SetFlags(JazzState.Flags.Exit, value); }
        }

        public bool Loop
        {
            get { return this.GetFlags(JazzState.Flags.Loop); }
            set { this.SetFlags(JazzState.Flags.Loop, value); }
        }

        public bool OneShot
        {
            get { return this.GetFlags(JazzState.Flags.OneShot); }
            set { this.SetFlags(JazzState.Flags.OneShot, value); }
        }

        public bool OneShotHold
        {
            get { return this.GetFlags(JazzState.Flags.OneShotHold); }
            set { this.SetFlags(JazzState.Flags.OneShotHold, value); }
        }

        public bool Synchronized
        {
            get { return this.GetFlags(JazzState.Flags.Synchronized); }
            set { this.SetFlags(JazzState.Flags.Synchronized, value); }
        }

        public bool Join
        {
            get { return this.GetFlags(JazzState.Flags.Join); }
            set { this.SetFlags(JazzState.Flags.Join, value); }
        }

        public bool Explicit
        {
            get { return this.GetFlags(JazzState.Flags.Explicit); }
            set { this.SetFlags(JazzState.Flags.Explicit, value); }
        }

        public DecisionGraph DecisionGraph
        {
            get { return this.mDecisionGraph; }
            set
            {
                if (this.mDecisionGraph != value)
                {
                    // TODO: Make sure this doesn't cause any recursion
                    // loops with DecisionGraph.State
                    if (this.mDecisionGraph != null)
                    {
                        DecisionGraph dg = this.mDecisionGraph;
                        this.mDecisionGraph = null;
                        dg.State = null;
                    }
                    this.mDecisionGraph = value;
                    value.State = this;
                }
            }
        }

        public bool AddTransition(State target)
        {
            /*if (target != null && 
                target.mStateMachine != this.mStateMachine)
            {
                throw new ArgumentException("Source and Target States " + 
                    "must be in the same State Machine", "target");
            }/* */
            if (this.mTransitions.Contains(target))
            {
                return false;
            }
            this.mTransitions.Add(target);
            return true;
        }

        public bool RemoveTransition(State target)
        {
            int index = this.mTransitions.IndexOf(target);
            if (index < 0)
            {
                return false;
            }
            this.mTransitions.RemoveAt(index);
            return true;
        }

        public void ClearTransitions()
        {
            this.mTransitions.Clear();
        }

        public int TransitionCount
        {
            get { return this.mTransitions.Count; }
        }

        public State[] Transitions
        {
            get { return this.mTransitions.ToArray(); }
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
