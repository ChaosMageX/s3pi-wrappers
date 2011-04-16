using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using System.Linq;
using s3pi.Settings;
namespace s3piwrappers
{
    public enum ClipEventType
    {
        Parent = 0x0001,
        UnParent = 0x0002,
        Sound = 0x0003,
        Script = 0x0004,
        Effect = 0x0005,
        Visibility = 0x0006,
        DestroyProp = 0x0009,
        StopEffect = 0x000A

    }
    public class ClipResource : AResource
    {
        public abstract class CountedOffsetItemList<T> : DependentList<T>
            where T : AHandlerElement, IEquatable<T>
        {
            protected CountedOffsetItemList(EventHandler handler) : base(handler) { }
            protected CountedOffsetItemList(EventHandler handler, Stream s) : base(handler, s) { }
            protected CountedOffsetItemList(EventHandler handler, IEnumerable<T> ilt) : base(handler, ilt) { }

            protected override void Parse(Stream s)
            {
                base.Clear();
                BinaryReader br = new BinaryReader(s);
                int count = ReadCount(s);
                long startOffset = s.Position;
                long[] offsets = new long[count];
                for (int i = 0; i < count; i++)
                {
                    offsets[i] = br.ReadUInt32() + startOffset;
                }
                long endOffset = s.Position;
                for (int i = 0; i < count; i++)
                {
                    if (s.Position != offsets[i])
                        throw new InvalidDataException(String.Format("Bad Offset: Expected 0x{0:X8}, but got 0x{1:X8}.", offsets[i], s.Position));
                    ((IList<T>)this).Add(CreateElement(s));
                }
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                uint[] offsets = new uint[base.Count];
                WriteCount(s, Count);
                long startOffset = s.Position;
                for (int i = 0; i < base.Count; i++) { bw.Write(offsets[i]); }
                for (int i = 0; i < base.Count; i++)
                {
                    offsets[i] = (uint)(s.Position - startOffset);
                    WriteElement(s, this[i]);
                }
                long endOffset = s.Position;
                s.Seek(startOffset, SeekOrigin.Begin);
                for (int i = 0; i < base.Count; i++) { bw.Write(offsets[i]); }
                s.Seek(endOffset, SeekOrigin.Begin);
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected abstract override T CreateElement(Stream s);

            protected abstract override void WriteElement(Stream s, T element);
        }
        public class IKChainList : CountedOffsetItemList<IKChainEntry>
        {
            public IKChainList(EventHandler handler)
                : base(handler)
            {
            }

            public IKChainList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public IKChainList(EventHandler handler, IEnumerable<IKChainEntry> ilt)
                : base(handler, ilt)
            {
            }

            protected override IKChainEntry CreateElement(Stream s)
            {
                return new IKChainEntry(0, handler, s);
            }

            protected override void WriteElement(Stream s, IKChainEntry element)
            {
                element.UnParse(s);
            }
        }
        public class IKTargetList : CountedOffsetItemList<IKTarget>
        {
            public IKTargetList(EventHandler handler)
                : base(handler)
            {
            }

            public IKTargetList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public IKTargetList(EventHandler handler, IEnumerable<IKTarget> ilt)
                : base(handler, ilt)
            {
            }

            protected override IKTarget CreateElement(Stream s)
            {
                return new IKTarget(0, handler, s);
            }

            protected override void WriteElement(Stream s, IKTarget element)
            {
                element.UnParse(s);
            }
        }
        public class IKTargetTable : AHandlerElement
        {
            private IKChainList mIkChains;
            public IKTargetTable(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mIkChains = new IKChainList(base.handler);
            }
            public IKTargetTable(int apiVersion, EventHandler handler, IKTargetTable basis) : this(apiVersion, handler, basis.IKChains) { }
            public IKTargetTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public IKTargetTable(int APIversion, EventHandler handler, IKChainList ikChains)
                : base(APIversion, handler)
            {
                mIkChains = ikChains;
            }

            public IKChainList IKChains
            {
                get { return mIkChains; }
                set { if (mIkChains != value) { mIkChains = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                mIkChains = new IKChainList(handler, s);
            }
            public void UnParse(Stream s)
            {
                mIkChains.UnParse(s);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IKTargetTable(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mIkChains.Count; i++)
                    {
                        sb.AppendFormat("==IK Chain[{0}]==\n{1}", i, mIkChains[i].Value);
                    }
                    return sb.ToString();
                }
            }
        }
        public class IKChainEntry : AHandlerElement, IEquatable<IKChainEntry>
        {
            private IKTargetList mIkTargets;
            public IKChainEntry(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mIkTargets = new IKTargetList(handler);
            }
            public IKChainEntry(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public IKChainEntry(int apiVersion, EventHandler handler, IKChainEntry basis) : this(apiVersion, handler, basis.IKTargets) { }

            public IKChainEntry(int APIversion, EventHandler handler, IKTargetList ikTargets)
                : base(APIversion, handler)
            {
                mIkTargets = ikTargets;
            }

            public IKTargetList IKTargets
            {
                get { return mIkTargets; }
                set { if (mIkTargets != value) { mIkTargets = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                if (br.ReadUInt32() != 0x7E7E7E7E) throw new InvalidDataException(String.Format("Expected 0x7E7E7E7E Padding at 0x{0:X8}", s.Position - 4)); //7E7E7E7E padding
                mIkTargets = new IKTargetList(handler, s);
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(new byte[] { 0x7E, 0x7E, 0x7E, 0x7E }); //7E7E7E7E padding
                mIkTargets.UnParse(s);
            }
            public bool Equals(IKChainEntry other)
            {
                return base.Equals(other);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IKChainEntry(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mIkTargets.Count; i++)
                    {
                        sb.AppendFormat("Target[{0:00}]{1}\n", i, mIkTargets[i].Value);
                    }
                    return sb.ToString();
                }
            }
        }
        public class IKTarget : AHandlerElement, IEquatable<IKTarget>
        {
            private UInt32 mIndex;
            private string mTargetNamespace;
            private string mTargetName;
            public IKTarget(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public IKTarget(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
            public IKTarget(int apiVersion, EventHandler handler, IKTarget basis) : this(apiVersion, handler, basis.Index, basis.TargetNamespace, basis.TargetName) { }
            public IKTarget(int APIversion, EventHandler handler, uint index, string targetNamespace, string targetName)
                : base(APIversion, handler)
            {
                mIndex = index;
                mTargetNamespace = targetNamespace;
                mTargetName = targetName;
            }

            [ElementPriority(1)]
            public uint Index
            {
                get { return mIndex; }
                set { if (mIndex != value) { mIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public string TargetNamespace
            {
                get { return mTargetNamespace; }
                set { if (mTargetNamespace != value) { mTargetNamespace = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public string TargetName
            {
                get { return mTargetName; }
                set { if (mTargetName != value) { mTargetName = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIndex = br.ReadUInt32();
                mTargetNamespace = ReadZString(br, 512);
                mTargetName = ReadZString(br, 512);
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndex);
                WriteZString(bw, mTargetNamespace, 0x23, 512);
                WriteZString(bw, mTargetName, 0x23, 512);
            }
            public bool Equals(IKTarget other)
            {
                return mIndex.Equals(other.mIndex) && mTargetNamespace.Equals(other.mTargetNamespace) && mTargetName.Equals(other.mTargetName);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IKTarget(requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
            public override string ToString()
            {
                return String.Format("(0x{0:X8}){1}:{2}", mIndex, mTargetNamespace, mTargetName);
            }

            public string Value
            {
                get { return ToString(); }
            }
        }
        public class EventTable : AHandlerElement
        {
            private UInt32 mVersion;
            private EventList mEvents;
            public EventTable(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mEvents = new EventList(handler);
            }
            public EventTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
            public EventTable(int apiVersion, EventHandler handler, EventTable basis) : this(apiVersion, handler, basis.Version, basis.Events) { }

            public EventTable(int APIversion, EventHandler handler, uint version, EventList events)
                : base(APIversion, handler)
            {
                mVersion = version;
                mEvents = events;
            }

            [ElementPriority(1)]
            public uint Version
            {
                get { return mVersion; }
                set { if (mVersion != value) { mVersion = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public EventList Events
            {
                get { return mEvents; }
                set { if (mEvents != value) { mEvents = value; OnElementChanged(); } }
            }


            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "=CE=")
                    throw new InvalidDataException(String.Format("Bad ClipEvent header: Expected \"=CE=\", but got {0}", magic));
                mVersion = br.ReadUInt32();
                mEvents = new EventList(handler, s);
            }


            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(Encoding.ASCII.GetBytes("=CE="));
                bw.Write(mVersion);
                mEvents.UnParse(s);

            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new EventTable(requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                    sb.AppendFormat("Events:\n");
                    for (int i = 0; i < mEvents.Count; i++)
                    {
                        sb.AppendFormat("==Event[{0}]==\n{1}\n", i, mEvents[i].Value);
                    }
                    return sb.ToString();
                }
            }
        }
        public class EventList : DependentList<Event>
        {

            public EventList(EventHandler handler) : base(handler) { }
            public EventList(EventHandler handler, Stream s) : base(handler, s) { }

            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                int count = ReadCount(s);
                long endOffset = br.ReadUInt32() + 4 + s.Position;
                long startOffset = br.ReadUInt32();
                if (checking && count > 0 && startOffset != 4)
                    throw new Exception(String.Format("Expected startOffset of 4 at =CE= section, but got 0x{0:X8}", startOffset));
                for (uint i = 0; i < count; i++) { ((IList<Event>)this).Add(CreateElement(s)); }
                s.Seek(endOffset, SeekOrigin.Begin);
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                WriteCount(s, Count);
                long offsetPos = s.Position;
                bw.Write(0);
                bw.Write(base.Count > 0 ? 4 : 0);
                long startPos = s.Position;
                for (int i = 0; i < base.Count; i++) { WriteElement(s, this[i]); }
                long endPos = s.Position;
                uint size = (uint)(endPos - startPos);
                s.Seek(offsetPos, SeekOrigin.Begin);
                bw.Write(size);
                s.Seek(endPos, SeekOrigin.Begin);


            }
            protected override Event CreateElement(Stream s)
            {
                ClipEventType type = (ClipEventType)new BinaryReader(s).ReadUInt16();
                return Event.CreateInstance(0, handler, type, s);
            }
            protected override void WriteElement(Stream s, Event element)
            {
                new BinaryWriter(s).Write((ushort)element.Type);
                element.UnParse(s);
            }
            protected override Type GetElementType(params object[] fields)
            {
                if (fields != null && fields.Length > 0)
                {
                    if (typeof(Event).IsAssignableFrom(fields[0].GetType()))
                    {
                        return fields[0].GetType();
                    }
                    if (typeof(ClipEventType).IsAssignableFrom(fields[0].GetType()))
                    {
                        ClipEventType type = (ClipEventType)fields[0];
                        switch (type)
                        {
                            case ClipEventType.Parent: return typeof(ParentEvent);
                            case ClipEventType.DestroyProp: return typeof(DestroyPropEvent);
                            case ClipEventType.Effect: return typeof(EffectEvent);
                            case ClipEventType.Sound: return typeof(SoundEvent);
                            case ClipEventType.Script: return typeof(ScriptEvent);
                            case ClipEventType.Visibility: return typeof(VisibilityEvent);
                            case ClipEventType.StopEffect: return typeof(StopEffectEvent);
                            case ClipEventType.UnParent: return typeof(UnparentEvent);
                            default: throw new NotImplementedException(String.Format("Event type: {0} not implemented", type));
                        }
                    }
                }
                return base.GetElementType(fields);
            }
            public override void Add()
            {
                throw new NotSupportedException();
            }
        }
        public abstract class Event : AHandlerElement, IEquatable<Event>
        {

            private readonly ClipEventType mType;
            private UInt16 mShort01;
            private UInt32 mId;
            private Single mTimecode;
            private Single mFloat01;
            private Single mFloat02;
            private UInt32 mInt01;
            private String mEventName;

            protected Event(int apiVersion, EventHandler handler, ClipEventType type) : this(apiVersion, handler, type, 0xC1E4, 0, 0f, -1f, -1f, 0,"") { }
            protected Event(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : this(apiVersion, handler, type) { mType = type; Parse(s); }
            protected Event(int apiVersion, EventHandler handler, Event basis) : this(apiVersion, handler, basis.Type, basis.Short01, basis.Id, basis.Timecode, basis.Float01, basis.Float02, basis.Int01,basis.EventName) { }
            protected Event(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01,string eventName)
                : base(APIversion, handler)
            {
                mType = type;
                mShort01 = short01;
                mId = id;
                mTimecode = timecode;
                mFloat01 = float01;
                mFloat02 = float02;
                mInt01 = int01;
                mEventName = eventName;
            }

            public virtual string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Type:\t{0}\n", mType);
                    sb.AppendFormat("Id:\t0x{0:X4}\n", mId);
                    sb.AppendFormat("Timecode:\t{0,8:0.00000}\n", mTimecode);
                    sb.AppendFormat("Float01:\t{0,8:0.00000}\n", mFloat01);
                    sb.AppendFormat("Float02:\t{0,8:0.00000}\n", mFloat02);
                    sb.AppendFormat("Unknown:\t0x{0:X8}\n", mInt01);
                    sb.AppendFormat("Event Name:\t{0}\n", mEventName);
                    return sb.ToString();
                }
            }
            [ElementPriority(0)]
            public ClipEventType Type
            {
                get { return mType; }
            }
            [ElementPriority(1)]
            public string EventName
            {
                get { return mEventName; }
                set { if (mEventName != value) { mEventName = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public float Float01
            {
                get { return mFloat01; }
                set { if (mFloat01 != value) { mFloat01 = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public float Float02
            {
                get { return mFloat02; }
                set { if (mFloat02 != value) { mFloat02 = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public uint Id
            {
                get { return mId; }
                set { if (mId != value) { mId = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public uint Int01
            {
                get { return mInt01; }
                set { if (mInt01 != value) { mInt01 = value; OnElementChanged(); } }
            }
            [ElementPriority(6)]
            public ushort Short01
            {
                get { return mShort01; }
                set { if (mShort01 != value) { mShort01 = value; OnElementChanged(); } }
            }
            [ElementPriority(7)]
            public float Timecode
            {
                get { return mTimecode; }
                set { if (mTimecode != value) { mTimecode = value; OnElementChanged(); } }
            }
            public static Event CreateInstance(int apiVersion, EventHandler handler, ClipEventType type)
            {
                return CreateInstance(apiVersion, handler, type, null);
            }

            public static Event CreateInstance(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
            {
                switch (type)
                {
                    case ClipEventType.Parent: return new ParentEvent(apiVersion, handler, type, s);
                    case ClipEventType.DestroyProp: return new DestroyPropEvent(apiVersion, handler, type, s);
                    case ClipEventType.Effect: return new EffectEvent(apiVersion, handler, type, s);
                    case ClipEventType.Sound: return new SoundEvent(apiVersion, handler, type, s);
                    case ClipEventType.Script: return new ScriptEvent(apiVersion, handler, type, s);
                    case ClipEventType.Visibility: return new VisibilityEvent(apiVersion, handler, type, s);
                    case ClipEventType.StopEffect: return new StopEffectEvent(apiVersion, handler, type, s);
                    case ClipEventType.UnParent: return new UnparentEvent(apiVersion, handler, type, s);
                    default: throw new InvalidDataException(String.Format("Event type: {0} not implemented", type));
                }
            }
            protected virtual void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mShort01 = br.ReadUInt16();
                mId = br.ReadUInt32();
                mTimecode = br.ReadSingle();
                mFloat01 = br.ReadSingle();
                mFloat02 = br.ReadSingle();
                mInt01 = br.ReadUInt32();
                uint strlen = br.ReadUInt32();
                mEventName = ReadZString(br);
                while ((s.Position % 4) != 0) br.ReadByte(); //padding to next DWORD
            }
            public virtual void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mShort01);
                bw.Write(mId);
                bw.Write(mTimecode);
                bw.Write(mFloat01);
                bw.Write(mFloat02);
                bw.Write(mInt01);
                bw.Write(mEventName.Length);
                WriteZString(bw, mEventName);
                while ((s.Position % 4) != 0) bw.Write((byte)0x00); //padding to next DWORD
            }
            public override string ToString()
            {
                return mType.ToString();
            }

            public bool Equals(Event other)
            {
                return base.Equals(other);
            }

            public abstract override AHandlerElement Clone(EventHandler handler);

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Parent })]
        public class ParentEvent : Event
        {


            private UInt32 mActorNameHash;
            private UInt32 mObjectNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown01;
            private Single[] mTransform;

            public ParentEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { mTransform = new Single[16]; }
            public ParentEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            
            public ParentEvent(int apiVersion, EventHandler handler, ParentEvent basis) : base(apiVersion, handler, basis)
            {
                mActorNameHash = basis.ActorNameHash;
                mObjectNameHash = basis.ObjectNameHash;
                mSlotNameHash = basis.SlotNameHash;
                mUnknown01 = basis.Unknown01;
                mTransform = basis.Transform;
            }


            public ParentEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string eventName, uint actorNameHash, uint objectNameHash, uint slotNameHash, uint unknown01, float[] transform) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName)
            {
                mActorNameHash = actorNameHash;
                mObjectNameHash = objectNameHash;
                mSlotNameHash = slotNameHash;
                mUnknown01 = unknown01;
                mTransform = transform;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Actor:\t0x{0:X8}\n", mActorNameHash);
                    sb.AppendFormat("Object:\t0x{0:X8}\n", mObjectNameHash);
                    sb.AppendFormat("Slot:\t0x{0:X8}\n", mSlotNameHash);
                    sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                    sb.AppendFormat("Transform:\n");
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mTransform[0], mTransform[1], mTransform[2], mTransform[3]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mTransform[4], mTransform[5], mTransform[6], mTransform[7]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mTransform[8], mTransform[9], mTransform[10], mTransform[11]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mTransform[12], mTransform[13], mTransform[14], mTransform[15]);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set { if (mActorNameHash != value) { mActorNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            public uint ObjectNameHash
            {
                get { return mObjectNameHash; }
                set { if (mObjectNameHash != value) { mObjectNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(10)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set { if (mSlotNameHash != value) { mSlotNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(11)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { if (mUnknown01 != value) { mUnknown01 = value; OnElementChanged(); } }
            }
            [ElementPriority(12)]
            public float[] Transform
            {
                get { return mTransform; }
                set { if (mTransform != value) { mTransform = value; OnElementChanged(); } }
            }
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mActorNameHash = br.ReadUInt32();
                mObjectNameHash = br.ReadUInt32();
                mSlotNameHash = br.ReadUInt32();
                mUnknown01 = br.ReadUInt32();
                mTransform = new Single[16];
                for (int i = 0; i < 16; i++) { mTransform[i] = br.ReadSingle(); }
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mActorNameHash);
                bw.Write(mObjectNameHash);
                bw.Write(mSlotNameHash);
                bw.Write(mUnknown01);
                for (int i = 0; i < 16; i++) bw.Write(mTransform[i]);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ParentEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.UnParent })]
        public class UnparentEvent : Event
        {
            private UInt32 mObjectNameHash;

            public UnparentEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public UnparentEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public UnparentEvent(int apiVersion, EventHandler handler, UnparentEvent basis)
                : base(apiVersion, handler, basis)
            {
                mObjectNameHash = basis.ObjectNameHash;
            }

            public UnparentEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint objectNameHash,string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mObjectNameHash = objectNameHash;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Object:\t0x{0:X8}\n", mObjectNameHash);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint ObjectNameHash
            {
                get { return mObjectNameHash; }
                set { if (mObjectNameHash != value) { mObjectNameHash = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mObjectNameHash = br.ReadUInt32();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mObjectNameHash);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new UnparentEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Sound })]
        public class SoundEvent : Event
        {
            private String mSoundName;

            public SoundEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public SoundEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public SoundEvent(int apiVersion, EventHandler handler, SoundEvent basis)
                : base(apiVersion, handler, basis)
            {
                mSoundName = basis.SoundName;
            }

            public SoundEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string soundName, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mSoundName = soundName;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Sound Name:\t{0}\n", mSoundName);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public string SoundName
            {
                get { return mSoundName; }
                set { if (mSoundName != value) { mSoundName = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mSoundName = ReadZString(br, 128);
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                WriteZString(bw, mSoundName, 0x00, 128);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SoundEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Script })]
        public class ScriptEvent : Event
        {

            public ScriptEvent(int apiVersion, EventHandler handler, ScriptEvent basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ScriptEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public ScriptEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public ScriptEvent(int apiVersion, EventHandler handler, Event basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ScriptEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ScriptEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Effect })]
        public class EffectEvent : Event
        {

            private UInt32 mUnknown01;
            private UInt32 mUnknown02;
            private UInt32 mEffectNameHash;
            private UInt32 mActorNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown03;

            public EffectEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public EffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public EffectEvent(int apiVersion, EventHandler handler, EffectEvent basis)
                : base(apiVersion, handler, basis)
            {
                mUnknown01 = basis.Unknown01;
                mUnknown02 = basis.Unknown02;
                mEffectNameHash = basis.EffectNameHash;
                mActorNameHash = basis.ActorNameHash;
                mSlotNameHash = basis.SlotNameHash;
                mUnknown03 = basis.Unknown03;
            }

            public EffectEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint unknown01, uint unknown02, uint effectNameHash, uint actorNameHash, uint slotNameHash, uint unknown03, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mUnknown01 = unknown01;
                mUnknown02 = unknown02;
                mEffectNameHash = effectNameHash;
                mActorNameHash = actorNameHash;
                mSlotNameHash = slotNameHash;
                mUnknown03 = unknown03;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                    sb.AppendFormat("Unknown02:\t0x{0:X8}\n", mUnknown02);
                    sb.AppendFormat("Effect:\t0x{0:X8}\n", mEffectNameHash);
                    sb.AppendFormat("Actor:\t0x{0:X8}\n", mActorNameHash);
                    sb.AppendFormat("Slot:\t0x{0:X8}\n", mSlotNameHash);
                    sb.AppendFormat("Unknown03:\t0x{0:X8}\n", mUnknown03);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { if (mUnknown01 != value) { mUnknown01 = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            public uint Unknown02
            {
                get { return mUnknown02; }
                set { if (mUnknown02 != value) { mUnknown02 = value; OnElementChanged(); } }
            }
            [ElementPriority(10)]
            public uint EffectNameHash
            {
                get { return mEffectNameHash; }
                set { if (mEffectNameHash != value) { mEffectNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(11)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set { if (mActorNameHash != value) { mActorNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(12)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set { if (mSlotNameHash != value) { mSlotNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(13)]
            public uint Unknown03
            {
                get { return mUnknown03; }
                set { if (mUnknown03 != value) { mUnknown03 = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mUnknown01 = br.ReadUInt32();
                mUnknown02 = br.ReadUInt32();
                mEffectNameHash = br.ReadUInt32();
                mActorNameHash = br.ReadUInt32();
                mSlotNameHash = br.ReadUInt32();
                mUnknown03 = br.ReadUInt32();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mUnknown01);
                bw.Write(mUnknown02);
                bw.Write(mEffectNameHash);
                bw.Write(mActorNameHash);
                bw.Write(mSlotNameHash);
                bw.Write(mUnknown03);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return new EffectEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Visibility })]
        public class VisibilityEvent : Event
        {
            private Single mVisibility;

            public VisibilityEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public VisibilityEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public VisibilityEvent(int apiVersion, EventHandler handler, VisibilityEvent basis)
                : base(apiVersion, handler, basis)
            {
                mVisibility = basis.Visibility;
            }

            public VisibilityEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, float visibility, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mVisibility = visibility;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Visibility:\t{0,8:0.00000}\n", mVisibility);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public float Visibility
            {
                get { return mVisibility; }
                set { if (mVisibility != value) { mVisibility = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mVisibility = br.ReadSingle();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mVisibility);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VisibilityEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.DestroyProp })]
        public class DestroyPropEvent : Event
        {

            private UInt32 mPropNameHash;

            public DestroyPropEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public DestroyPropEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public DestroyPropEvent(int apiVersion, EventHandler handler, DestroyPropEvent basis)
                : base(apiVersion, handler, basis)
            {
                mPropNameHash = basis.PropNameHash;
            }

            public DestroyPropEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint propNameHash, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mPropNameHash = propNameHash;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Prop:\t0x{0:X8}\n", mPropNameHash);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint PropNameHash
            {
                get { return mPropNameHash; }
                set { if (mPropNameHash != value) { mPropNameHash = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mPropNameHash = br.ReadUInt32();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mPropNameHash);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new DestroyPropEvent(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.StopEffect })]
        public class StopEffectEvent : Event
        {
            private UInt32 mEffectNameHash;
            private UInt32 mUnknown01;

            public StopEffectEvent(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler, type)
            {
            }

            public StopEffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type, s)
            {
            }

            public StopEffectEvent(int apiVersion, EventHandler handler, StopEffectEvent basis)
                : base(apiVersion, handler, basis)
            {
                mEffectNameHash = basis.EffectNameHash;
                mUnknown01 = basis.Unknown01;
            }

            public StopEffectEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint effectNameHash, uint unknown01, string eventName)
                : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01,eventName)
            {
                mEffectNameHash = effectNameHash;
                mUnknown01 = unknown01;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Effect:\t0x{0:X8}\n", mEffectNameHash);
                    sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint EffectNameHash
            {
                get { return mEffectNameHash; }
                set { if (mEffectNameHash != value) { mEffectNameHash = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { if (mUnknown01 != value) { mUnknown01 = value; OnElementChanged(); } }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mEffectNameHash = br.ReadUInt32();
                mUnknown01 = br.ReadUInt32();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mEffectNameHash);
                bw.Write(mUnknown01);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return new StopEffectEvent(requestedApiVersion, handler, this);
            }
        }
        public class ClipEndSection : AHandlerElement
        {
            private Single mX;
            private Single mY;
            private Single mZ;
            private Single mW;

            public ClipEndSection(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public ClipEndSection(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
            public ClipEndSection(int apiVersion, EventHandler handler, ClipEndSection basis) : this(apiVersion, handler, basis.X, basis.Y, basis.Z, basis.W) { }
            public ClipEndSection(int APIversion, EventHandler handler, float x, float y, float z, float w)
                : base(APIversion, handler)
            {
                mX = x;
                mY = y;
                mZ = z;
                mW = w;
            }

            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { if (mX != value) { mX = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { if (mY != value) { mY = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { if(mZ!=value){mZ = value; OnElementChanged();} }
            }
            [ElementPriority(4)]
            public float W
            {
                get { return mW; }
                set { if(mW!=value){mW = value; OnElementChanged();} }
            }

            public string Value
            {
                get { return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW); }
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mX = br.ReadSingle();
                mY = br.ReadSingle();
                mZ = br.ReadSingle();
                mW = br.ReadSingle();
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mX);
                bw.Write(mY);
                bw.Write(mZ);
                bw.Write(mW);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ClipEndSection(requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public ClipResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            mS3Clip = new byte[0];
            mIKTargetInfo = new IKTargetTable(0, this.OnResourceChanged);
            mEventSectionTable = new EventTable(0, this.OnResourceChanged);
            mEndSection = new ClipEndSection(0, this.OnResourceChanged);

            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(s);
        }


        private UInt32 mUnknown01;
        private UInt32 mUnknown02;
        private byte[] mS3Clip;
        //private Clip mWrappedClipData;
        private IKTargetTable mIKTargetInfo;
        private string mActorName;
        private EventTable mEventSectionTable;
        private ClipEndSection mEndSection;


        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("Unknown02:\t0x{0:X8}\n", mUnknown02);
                if (mIKTargetInfo.IKChains.Count > 0)
                {
                    sb.AppendFormat("IK Target Info Table:\n{0}", mIKTargetInfo.Value);
                }
                sb.AppendFormat("Actor:\t{0}\n", mActorName);
                if (mEventSectionTable.Events.Count > 0)
                {
                    sb.AppendFormat("Event Table:\n{0}", mEventSectionTable.Value);
                }
                sb.AppendFormat("End Section:\n{0}\n", mEndSection.Value);
                return sb.ToString();

            }
        }
        [ElementPriority(1)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { if(mUnknown01!=value){mUnknown01 = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(2)]
        public uint Unknown02
        {
            get { return mUnknown02; }
            set { if(mUnknown02!=value){mUnknown02 = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(3)]
        public BinaryReader S3Clip
        {
            get
            {
                MemoryStream s = new MemoryStream(mS3Clip);
                s.Position = 0L;
                return new BinaryReader(s);
            }
            set
            {
                if (value.BaseStream.CanSeek)
                {
                    value.BaseStream.Position = 0L;
                    mS3Clip = value.ReadBytes((int)value.BaseStream.Length);
                }
                else
                {
                    MemoryStream s = new MemoryStream();
                    byte[] buffer = new byte[0x100000];
                    for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                    {
                        s.Write(buffer, 0, i);
                    }
                    mS3Clip = new BinaryReader(s).ReadBytes((int)s.Length);
                }
                OnResourceChanged(this, new EventArgs());
            }
        }
        [ElementPriority(4)]
        public IKTargetTable IKTargetInfo
        {
            get { return mIKTargetInfo; }
            set { if(mIKTargetInfo!=value){mIKTargetInfo = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(5)]
        public string ActorName
        {
            get { return mActorName; }
            set { if(mActorName!=value){mActorName = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(6)]
        public EventTable EventSection
        {
            get { return mEventSectionTable; }
            set { if(mEventSectionTable!=value){mEventSectionTable = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(7)]
        public ClipEndSection EndSection
        {
            get { return mEndSection; }
            set { if(mEndSection!=value){mEndSection = value; OnResourceChanged(this, new EventArgs());} }
        }

        //[ElementPriority(7)]
        //public Clip ReadonlyClipData
        //{
        //    get { return mWrappedClipData; }
        //    set { mWrappedClipData = value; }
        //}
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);

            //header
            if (br.ReadUInt32() != 0x6B20C4F3) throw new
                InvalidDataException("Not a valid CLIP resource: Expected CLIP ID(0x6B20C4F3)");
            uint offset = br.ReadUInt32();
            long linkedClipOffset = offset > 0 ? s.Position + offset - 4 : 0;
            long clipSize = br.ReadUInt32();
            offset = br.ReadUInt32();
            long clipOffset = offset > 0 ? s.Position + offset - 4 : 0;
            offset = br.ReadUInt32();
            long ikOffset = offset > 0 ? s.Position + offset - 4 : 0;
            offset = br.ReadUInt32();
            long actorOffset = offset > 0 ? s.Position + offset - 4 : 0;
            offset = br.ReadUInt32();
            long eventOffset = offset > 0 ? s.Position + offset - 4 : 0;
            mUnknown01 = br.ReadUInt32();
            mUnknown02 = br.ReadUInt32();
            offset = br.ReadUInt32();
            long endOffset = offset > 0 ? s.Position + offset - 4 : 0;

            if (checking && linkedClipOffset != 0)
                throw new NotSupportedException("Linked clip not supported.");
            if (checking && clipOffset == 0)
                throw new InvalidDataException("No Clip offset found");
            if (checking && actorOffset == 0)
                throw new InvalidDataException("No Actor offset found");
            if (checking && eventOffset == 0)
                throw new InvalidDataException("No Event offset found");
            if (checking && endOffset == 0)
                throw new InvalidDataException("No End offset found");
            for (int i = 0; i < 16; i++)
            {
                if (br.ReadByte() != 0 && checking)
                    throw new InvalidDataException("Expected 0x00");
            }

            s.Seek(clipOffset, SeekOrigin.Begin);
            mS3Clip = new byte[(int)clipSize];
            mS3Clip = br.ReadBytes((int)clipSize);
            //ReadonlyClipData = new Clip(0,this.OnResourceChanged,new MemoryStream(mS3Clip));
            

            if (ikOffset > 0)
            {
                s.Seek(ikOffset, SeekOrigin.Begin);
                mIKTargetInfo = new IKTargetTable(0, this.OnResourceChanged, s);
            }
            else
            {
                mIKTargetInfo = new IKTargetTable(0, this.OnResourceChanged);
            }
            s.Seek(actorOffset, SeekOrigin.Begin);
            mActorName = ReadZString(br);


            s.Seek(eventOffset, SeekOrigin.Begin);
            mEventSectionTable = new EventTable(0, this.OnResourceChanged, s);

            s.Seek(-16, SeekOrigin.End);
            mEndSection = new ClipEndSection(0, this.OnResourceChanged, s);

            if (checking && s.Position != s.Length)
                throw new InvalidDataException("Unexpected end of data.");

        }
        private static void WritePadding(Stream s)
        {
            var bw = new BinaryWriter(s);
            while ((s.Position % 4) != 0) bw.Write((byte)0x7E); //padding to next dword
        }
        protected override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(0x6B20C4F3);
            long mainOffsetList = s.Position;
            long clipSize = 0;
            long clipOffset = 0;
            long ikOffset = 0;
            long actorOffset = 0;
            long eventOffset = 0;
            long endOffset = 0;
            bool hasIkData = mIKTargetInfo.IKChains.Count > 0;
            s.Seek(52, SeekOrigin.Current);

            clipSize = mS3Clip.Length;
            clipOffset = s.Position;
            bw.Write(mS3Clip);
            WritePadding(s);

            if (hasIkData)
            {
                ikOffset = s.Position;
                mIKTargetInfo.UnParse(s);
                WritePadding(s);
            }
            actorOffset = s.Position;
            WriteZString(bw, mActorName);
            WritePadding(s);

            eventOffset = s.Position;
            mEventSectionTable.UnParse(s);
            WritePadding(s);

            endOffset = s.Position;
            mEndSection.UnParse(s);

            //write header last
            s.Seek(mainOffsetList, SeekOrigin.Begin);
            bw.Write((uint)(0));
            bw.Write((uint)clipSize);
            bw.Write((uint)(clipOffset - s.Position));
            bw.Write((uint)(hasIkData ? ikOffset - s.Position : 0));
            bw.Write((uint)(actorOffset - s.Position));
            bw.Write((uint)(eventOffset - s.Position));
            bw.Write(mUnknown01);
            bw.Write(mUnknown02);
            bw.Write((uint)(endOffset - s.Position));
            bw.Write(new byte[16]);
            s.Position = s.Length;
            return s;
        }

        public static string ReadZString(BinaryReader br)
        {
            return ReadZString(br, 0);
        }
        public static string ReadZString(BinaryReader br, int padLength)
        {
            String s = "";
            byte b = br.ReadByte();
            while (b != 0)
            {
                s += Encoding.ASCII.GetString(new byte[1] { b });
                b = br.ReadByte();
            }
            if (padLength != 0)
            {
                int count = padLength - 1 - s.Length;
                br.BaseStream.Seek(count, SeekOrigin.Current);
            }
            return s;
        }
        public static void WriteZString(BinaryWriter bw, String s)
        {
            WriteZString(bw, s, 0x00, 0);
        }
        public static void WriteZString(BinaryWriter bw, String s, byte paddingChar, int padLength)
        {
            if (!string.IsNullOrEmpty(s))
            {
                bw.Write(Encoding.ASCII.GetBytes(s));
            }

            bw.Write((byte)0x00);
            if (padLength > 0)
            {
                int count = padLength - 1;
                if (!string.IsNullOrEmpty(s)) count -= s.Length;
                for (int i = 0; i < count; i++)
                {
                    bw.Write(paddingChar);
                }
            }
        }
        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }


        static bool checking = Settings.Checking;
        const int kRecommendedApiVersion = 1;

    }
}
