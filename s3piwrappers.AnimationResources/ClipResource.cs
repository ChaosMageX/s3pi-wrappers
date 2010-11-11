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
        public abstract class DependentElement : AHandlerElement
        {
            protected DependentElement(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {

            }
            protected DependentElement(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                Parse(s);
            }
            protected DependentElement(int apiVersion, EventHandler handler, DependentElement basis)
                : base(apiVersion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            protected abstract void Parse(Stream s);
            public abstract void UnParse(Stream s);



            public override AHandlerElement Clone(EventHandler handler)
            {
                MemoryStream ms = new MemoryStream();
                UnParse(ms);
                return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, ms });
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }
        }

        public class DependentElementList<T> : AResource.DependentList<T>
            where T : DependentElement, IEquatable<T>
        {
            public DependentElementList(EventHandler handler) : base(handler) { }
            public DependentElementList(EventHandler handler, Stream s) : base(handler, s) { }
            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override T CreateElement(Stream s)
            {
                return (T)Activator.CreateInstance(typeof(T), new object[] { 0, elementHandler, s });
            }

            protected override void WriteElement(Stream s, T element)
            {
                element.UnParse(s);
            }
        }
        public class CountedOffsetItemList<T> : DependentElementList<T>
            where T : DependentElement, IEquatable<T>
        {
            public CountedOffsetItemList(EventHandler handler) : base(handler) { }
            public CountedOffsetItemList(EventHandler handler, Stream s) : base(handler, s) { }
            protected override void Parse(Stream s)
            {
                base.Clear();
                BinaryReader br = new BinaryReader(s);
                uint count = ReadCount(s);
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
                WriteCount(s, (uint)base.Count);
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
        }
        
        public class IKTargetTable : DependentElement
        {
            private CountedOffsetItemList<IKChainEntry> mIkChains;
            public IKTargetTable(int apiVersion, EventHandler handler) : base(apiVersion, handler)
            {
                mIkChains = new CountedOffsetItemList<IKChainEntry>(base.handler);
            }
            public IKTargetTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

            public CountedOffsetItemList<IKChainEntry> IKChains
            {
                get { return mIkChains; }
                set { mIkChains = value; OnElementChanged(); }
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
            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIkChains = new CountedOffsetItemList<IKChainEntry>(handler, s);
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                mIkChains.UnParse(s);
            }
        }
        public class IKChainEntry : DependentElement, IEquatable<IKChainEntry>
        {
            private CountedOffsetItemList<IKTarget> mIkTargets;
            public IKChainEntry(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mIkTargets = new CountedOffsetItemList<IKTarget>(handler);
            }
            public IKChainEntry(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

            public IKChainEntry(int apiVersion, EventHandler handler, IKChainEntry basis)
                : base(apiVersion, handler, basis)
            {
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
            public CountedOffsetItemList<IKTarget> IKTargets
            {
                get { return mIkTargets; }
                set { mIkTargets = value; OnElementChanged(); }
            }

            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                if (br.ReadUInt32() != 0x7E7E7E7E) throw new InvalidDataException(String.Format("Expected 0x7E7E7E7E Padding at 0x{0:X8}", s.Position - 4)); //7E7E7E7E padding
                mIkTargets = new CountedOffsetItemList<IKTarget>(handler, s);
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(new byte[] { 0x7E, 0x7E, 0x7E, 0x7E }); //7E7E7E7E padding
                mIkTargets.UnParse(s);
            }
            public bool Equals(IKChainEntry other)
            {
                return base.Equals(other);
            }

        }
        public class IKTarget : DependentElement, IEquatable<IKTarget>
        {
            private UInt32 mIndex;
            private string mTargetNamespace = String.Empty;
            private string mTargetName = String.Empty;
            public IKTarget(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public IKTarget(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

            public IKTarget(int apiVersion, EventHandler handler, IKTarget basis)
                : base(apiVersion, handler, basis)
            {
            }
            public string Value
            {
                get { return ToString(); }
            }
            [ElementPriority(1)]
            public uint Index
            {
                get { return mIndex; }
                set { mIndex = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public string TargetNamespace
            {
                get { return mTargetNamespace; }
                set { mTargetNamespace = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public string TargetName
            {
                get { return mTargetName; }
                set { mTargetName = value; OnElementChanged(); }
            }

            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIndex = br.ReadUInt32();
                mTargetNamespace = ReadZString(br,512);
                mTargetName = ReadZString(br,512);
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndex);
                WriteZString(bw,mTargetNamespace, 0x23, 512);
                WriteZString(bw,mTargetName, 0x23, 512);
            }
            public override string ToString()
            {
                return String.Format("(0x{0:X8}){1}:{2}", mIndex, mTargetNamespace, mTargetName);
            }

            public bool Equals(IKTarget other)
            {
                return mIndex.Equals(other.mIndex) && mTargetNamespace.Equals(other.mTargetNamespace) && mTargetName.Equals(other.mTargetName);
            }
        }
        

                public class EventTable : DependentElement
        {
            private UInt32 mVersion;
            private EventList mEvents;
            public EventTable(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mEvents = new EventList(handler);
            }
            public EventTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public EventTable(int apiVersion, EventHandler handler, EventTable basis)
                : base(apiVersion, handler, basis)
            {
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
            [ElementPriority(1)]
            public uint Version
            {
                get { return mVersion; }
                set { mVersion = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public EventList Events
            {
                get { return mEvents; }
                set { mEvents = value; OnElementChanged(); }
            }


            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "=CE=")
                    throw new InvalidDataException(String.Format("Bad ClipEvent header: Expected \"=CE=\", but got {0}", magic));
                mVersion = br.ReadUInt32();
                mEvents = new EventList(handler, s);
            }


            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(Encoding.ASCII.GetBytes("=CE="));
                bw.Write(mVersion);
                mEvents.UnParse(s);

            }
        }
        public class EventList : DependentElementList<Event>
        {

            public EventList(EventHandler handler) : base(handler) { }
            public EventList(EventHandler handler, Stream s) : base(handler, s) { }
            public override bool Add(params object[] fields)
            {
                if (fields.Length == 0) return false;
                if (fields.Length == 1 && typeof(Event).IsAssignableFrom(fields[0].GetType()))
                {
                    ((IList<Event>)this).Add((Event)fields[0]);
                    return true;
                }
                Add(Event.CreateInstance(0, this.handler, (ClipEventType)(int)fields[0]));
                return true;
            }
            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                uint count = ReadCount(s);
                long endOffset = br.ReadUInt32() + 4 + s.Position;
                long startOffset = br.ReadUInt32();
                if (checking && count > 0 && startOffset != 4)
                    throw new Exception(String.Format("Expected startOffset of 4 at =CE= section, but got 0x{0:X8}", startOffset));
                for (uint i = 0; i < count; i++) { ((IList<Event>)this).Add(CreateElement(s)); }
                if (checking && s.Position != endOffset)
                    throw new Exception(String.Format("Expected endOffset of 0x{0:X8} at =CE= section, but got 0x{1:X8}", endOffset, s.Position));
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                WriteCount(s, (uint)base.Count);
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
                BinaryReader br = new BinaryReader(s);
                ClipEventType type = (ClipEventType)br.ReadUInt16();
                return Event.CreateInstance(0, handler, type, s);
            }
            protected override void WriteElement(Stream s, Event element)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((ushort)element.Type);
                base.WriteElement(s, element);
            }
        }
        public abstract class Event : DependentElement, IEquatable<Event>
        {

            protected Event(int apiVersion, EventHandler handler, ClipEventType type)
                : base(apiVersion, handler)
            {
                mType = type;
                mFloat01 = -1f;
                mFloat02 = -1f;
                mShort01 = 0xC1E4;
            }
            protected Event(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : this(apiVersion, handler, type)
            {
                if (s != null) Parse(s);
            }
            protected Event(int apiVersion, EventHandler handler, Event basis)
                : base(apiVersion, handler)
            {
                mType = basis.Type;
            }
            private ClipEventType mType;
            private UInt16 mShort01;
            private UInt32 mId;
            private Single mTimecode;
            private Single mFloat01;
            private Single mFloat02;
            private UInt32 mInt01;
            private String mEventName = String.Empty;

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
                set { mEventName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint Id
            {
                get { return mId; }
                set { mId = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public ushort Short01
            {
                get { return mShort01; }
                set { mShort01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Timecode
            {
                get { return mTimecode; }
                set { mTimecode = value; OnElementChanged(); }
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
            protected override void Parse(Stream s)
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
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mShort01);
                bw.Write(mId);
                bw.Write(mTimecode);
                bw.Write(mFloat01);
                bw.Write(mFloat02);
                bw.Write(mInt01);
                bw.Write(mEventName.Length);
                WriteZString(bw,mEventName);
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
        }
        [ConstructorParameters(new object[] { ClipEventType.Parent })]
        public class ParentEvent : Event
        {
            internal ParentEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
                : base(apiVersion, handler, type)
            {
                mMatrix4x4 = new Single[16];
                if (s != null) Parse(s);
            }
            public ParentEvent(int apiVersion, EventHandler handler, ParentEvent basis)
                : base(apiVersion, handler, basis)
            {
            }

            private UInt32 mActorNameHash;
            private UInt32 mObjectNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown01;
            private Single[] mMatrix4x4;

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    sb.AppendFormat("Actor:\t0x{0:X8}\n", mActorNameHash);
                    sb.AppendFormat("Object:\t0x{0:X8}\n", mObjectNameHash);
                    sb.AppendFormat("Slot:\t0x{0:X8}\n", mSlotNameHash);
                    sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                    sb.AppendFormat("Matrix:\n");
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mMatrix4x4[0], mMatrix4x4[1], mMatrix4x4[2], mMatrix4x4[3]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mMatrix4x4[4], mMatrix4x4[5], mMatrix4x4[6], mMatrix4x4[7]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mMatrix4x4[8], mMatrix4x4[9], mMatrix4x4[10], mMatrix4x4[11]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n"
                        , mMatrix4x4[12], mMatrix4x4[13], mMatrix4x4[14], mMatrix4x4[15]);
                    return sb.ToString();
                }
            }
            [ElementPriority(8)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set { mActorNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public uint ObjectNameHash
            {
                get { return mObjectNameHash; }
                set { mObjectNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set { mSlotNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public float[] Matrix4X4
            {
                get { return mMatrix4x4; }
                set { mMatrix4x4 = value; OnElementChanged(); }
            }
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mActorNameHash = br.ReadUInt32();
                mObjectNameHash = br.ReadUInt32();
                mSlotNameHash = br.ReadUInt32();
                mUnknown01 = br.ReadUInt32();
                mMatrix4x4 = new Single[16];
                for (int i = 0; i < 16; i++) { mMatrix4x4[i] = br.ReadSingle(); }
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mActorNameHash);
                bw.Write(mObjectNameHash);
                bw.Write(mSlotNameHash);
                bw.Write(mUnknown01);
                for (int i = 0; i < 16; i++) bw.Write(mMatrix4x4[i]);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.UnParent })]
        public class UnparentEvent : Event
        {
            internal UnparentEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public UnparentEvent(int apiVersion, EventHandler handler, UnparentEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
            private UInt32 mObjectNameHash;

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
                set { mObjectNameHash = value; OnElementChanged(); }
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
        }
        [ConstructorParameters(new object[] { ClipEventType.Sound })]
        public class SoundEvent : Event
        {
            internal SoundEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public SoundEvent(int apiVersion, EventHandler handler, SoundEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
            private String mSoundName;

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
                set { mSoundName = value; OnElementChanged(); }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mSoundName = ReadZString(br,128);
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
                WriteZString(bw, mSoundName, 0x00, 128);
            }
        }
        [ConstructorParameters(new object[] { ClipEventType.Script })]
        public class ScriptEvent : Event
        {
            internal ScriptEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            public ScriptEvent(int apiVersion, EventHandler handler, ScriptEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
        }

        [ConstructorParameters(new object[] { ClipEventType.Effect })]
        public class EffectEvent : Event
        {
            internal EffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            public EffectEvent(int apiVersion, EventHandler handler, EffectEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
            private UInt32 mUnknown01;
            private UInt32 mUnknown02;
            private UInt32 mEffectNameHash;
            private UInt32 mActorNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown03;

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
                set { mUnknown01 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public uint Unknown02
            {
                get { return mUnknown02; }
                set { mUnknown02 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public uint EffectNameHash
            {
                get { return mEffectNameHash; }
                set { mEffectNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set { mActorNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set { mSlotNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public uint Unknown03
            {
                get { return mUnknown03; }
                set { mUnknown03 = value; OnElementChanged(); }
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

        }
        [ConstructorParameters(new object[] { ClipEventType.Visibility })]
        public class VisibilityEvent : Event
        {
            internal VisibilityEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            public VisibilityEvent(int apiVersion, EventHandler handler, VisibilityEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
            private Single mVisibility;

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
                set { mVisibility = value; OnElementChanged(); }
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
        }

        [ConstructorParameters(new object[] { ClipEventType.DestroyProp })]
        public class DestroyPropEvent : Event
        {
            internal DestroyPropEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            public DestroyPropEvent(int apiVersion, EventHandler handler, DestroyPropEvent basis)
                : base(apiVersion, handler, basis)
            {
            }
            private UInt32 mPropNameHash;

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
                set { mPropNameHash = value; OnElementChanged(); }
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
        }

        [ConstructorParameters(new object[] { ClipEventType.StopEffect })]
        public class StopEffectEvent : Event
        {
            private UInt32 mEffectNameHash;
            private UInt32 mUnknown01;
            internal StopEffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }
            public StopEffectEvent(int apiVersion, EventHandler handler, StopEffectEvent basis)
                : base(apiVersion, handler, basis)
            {
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
                set { mEffectNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
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

        }
        
        public class ClipEndSection : DependentElement
        {
            private Single mX;
            private Single mY;
            private Single mZ;
            private Single mW;

            public ClipEndSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }

            public ClipEndSection(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public ClipEndSection(int apiVersion, EventHandler handler, ClipEndSection basis)
                : base(apiVersion, handler, basis)
            {
            }
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float W
            {
                get { return mW; }
                set { mW = value; OnElementChanged(); }
            }

            public string Value
            {
                get { return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW); }
            }
            protected override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mX = br.ReadSingle();
                mY = br.ReadSingle();
                mZ = br.ReadSingle();
                mW = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mX);
                bw.Write(mY);
                bw.Write(mZ);
                bw.Write(mW);
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
        //private S3Clip mAnimation;
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
            set { mUnknown01 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public uint Unknown02
        {
            get { return mUnknown02; }
            set { mUnknown02 = value; OnResourceChanged(this, new EventArgs()); }
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
            set { mIKTargetInfo = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public string ActorName
        {
            get { return mActorName; }
            set { mActorName = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public EventTable EventSection
        {
            get { return mEventSectionTable; }
            set { mEventSectionTable = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(7)]
        public ClipEndSection EndSection
        {
            get { return mEndSection; }
            set { mEndSection = value; OnResourceChanged(this, new EventArgs()); }
        }
        

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);

            //header
            if (br.ReadUInt32() != 0x6B20C4F3) throw new
                InvalidDataException("Not a valid CLIP resource: Expected CLIP ID(0x6B20C4F3)");
            uint offset;
            offset = br.ReadUInt32();
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

            if (checking && s.Position != clipOffset)
                throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8} but got 0x{1:X8}.", clipOffset, s.Position));

            mS3Clip = new byte[(int)clipSize];
            mS3Clip = br.ReadBytes((int)clipSize);
            while ((s.Position % 4) != 0)
                if (br.ReadByte() != 0x7E && checking)
                    throw new InvalidDataException("Expected padding char 0x7E");

            if (ikOffset > 0)
            {
                if (checking && s.Position != ikOffset)
                    throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8} but got 0x{1:X8}.", ikOffset, s.Position));
                mIKTargetInfo = new IKTargetTable(0, this.OnResourceChanged, s);
                while ((s.Position % 4) != 0)
                    if (br.ReadByte() != 0x7E && checking)
                        throw new InvalidDataException("Expected padding char 0x7E");
            }
            else
            {
                mIKTargetInfo = new IKTargetTable(0, this.OnResourceChanged);
            }
            if (checking && s.Position != actorOffset)
                throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8} but got 0x{1:X8}.", actorOffset, s.Position));
            mActorName = ReadZString(br);
            while ((s.Position % 4) != 0)
                if (br.ReadByte() != 0x7E && checking)
                    throw new InvalidDataException("Expected padding char 0x7E");

            if (checking && s.Position != eventOffset)
                throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8} but got 0x{1:X8}.", eventOffset, s.Position));
            mEventSectionTable = new EventTable(0, this.OnResourceChanged, s);
            while ((s.Position % 4) != 0)
                if (br.ReadByte() != 0x7E && checking)
                    throw new InvalidDataException("Expected padding char 0x7E");

            if (checking && s.Position != endOffset)
                throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8} but got 0x{1:X8}.", endOffset, s.Position));
            mEndSection = new ClipEndSection(0, this.OnResourceChanged, s);

            if (checking && s.Position != s.Length)
                throw new InvalidDataException("Unexpected end of data.");

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
            while ((s.Position % 4) != 0) bw.Write((byte)0x7e); //padding to next dword

            if (hasIkData)
            {
                ikOffset = s.Position;
                mIKTargetInfo.UnParse(s);
                while ((s.Position % 4) != 0) bw.Write((byte)0x7e); //padding to next dword
            }
            actorOffset = s.Position;
            WriteZString(bw,mActorName);
            while ((s.Position % 4) != 0) bw.Write((byte)0x7e); //padding to next dword

            eventOffset = s.Position;
            mEventSectionTable.UnParse(s);
            while ((s.Position % 4) != 0) bw.Write((byte)0x7e); //padding to next dword

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
            return ReadZString(br,0);
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
            WriteZString(bw,s, 0x00, 0);
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
