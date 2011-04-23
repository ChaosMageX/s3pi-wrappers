using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3pi.Settings;

namespace s3piwrappers
{
    public class ClipResource : AResource
    {
        private const int kRecommendedApiVersion = 1;
        private static readonly bool checking = Settings.Checking;
        private string mActorName;
        private ClipEndSection mEndSection;
        private EventTable mEventSectionTable;
        private IKTargetTable mIKTargetInfo;
        private UInt32 mUnknown01;
        private UInt32 mUnknown02;

        public ClipResource(int apiVersion, Stream s) : base(apiVersion, s)
        {
            mS3Clip = new Clip(0,this.OnResourceChanged);
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


        public string Value
        {
            get
            {
                return ValueBuilder;
            }
        }

        [ElementPriority(1)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set
            {
                if (mUnknown01 != value)
                {
                    mUnknown01 = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(2)]
        public uint Unknown02
        {
            get { return mUnknown02; }
            set
            {
                if (mUnknown02 != value)
                {
                    mUnknown02 = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }
        
        [ElementPriority(4)]
        public IKTargetTable IKTargetInfo
        {
            get { return mIKTargetInfo; }
            set
            {
                if (mIKTargetInfo != value)
                {
                    mIKTargetInfo = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(5)]
        public string ActorName
        {
            get { return mActorName; }
            set
            {
                if (mActorName != value)
                {
                    mActorName = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(6)]
        public EventTable EventSection
        {
            get { return mEventSectionTable; }
            set
            {
                if (mEventSectionTable != value)
                {
                    mEventSectionTable = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(7)]
        public ClipEndSection EndSection
        {
            get { return mEndSection; }
            set
            {
                if (mEndSection != value)
                {
                    mEndSection = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        private Clip mS3Clip;

        [ElementPriority(3)]
        public Clip S3Clip { get { return mS3Clip; } set { mS3Clip = value; } }

        public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);

            //header
            if (br.ReadUInt32() != 0x6B20C4F3)
                throw new InvalidDataException("Not a valid CLIP resource: Expected CLIP ID(0x6B20C4F3)");
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
            byte[] clipBytes = new byte[(int) clipSize];
            clipBytes = br.ReadBytes((int) clipSize);
            mS3Clip = new Clip(0, this.OnResourceChanged, new MemoryStream(clipBytes));


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
            while ((s.Position%4) != 0) bw.Write((byte) 0x7E); //padding to next dword
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

            var clipStream = new MemoryStream();
            mS3Clip.UnParse(clipStream);
            clipStream.Position = 0L;
            var clipData = clipStream.ToArray();

            clipSize = clipData.Length;
            clipOffset = s.Position;
            bw.Write(clipData);

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
            bw.Write((uint) (0));
            bw.Write((uint) clipSize);
            bw.Write((uint) (clipOffset - s.Position));
            bw.Write((uint) (hasIkData ? ikOffset - s.Position : 0));
            bw.Write((uint) (actorOffset - s.Position));
            bw.Write((uint) (eventOffset - s.Position));
            bw.Write(mUnknown01);
            bw.Write(mUnknown02);
            bw.Write((uint) (endOffset - s.Position));
            bw.Write(new byte[16]);
            s.Position = s.Length;
            return s;
        }

        public static string ReadZString(BinaryReader br) { return ReadZString(br, 0); }

        public static string ReadZString(BinaryReader br, int padLength)
        {
            String s = "";
            byte b = br.ReadByte();
            while (b != 0)
            {
                s += Encoding.ASCII.GetString(new byte[1] {b});
                b = br.ReadByte();
            }
            if (padLength != 0)
            {
                int count = padLength - 1 - s.Length;
                br.BaseStream.Seek(count, SeekOrigin.Current);
            }
            return s;
        }

        public static void WriteZString(BinaryWriter bw, String s) { WriteZString(bw, s, 0x00, 0); }

        public static void WriteZString(BinaryWriter bw, String s, byte paddingChar, int padLength)
        {
            if (!string.IsNullOrEmpty(s))
            {
                bw.Write(Encoding.ASCII.GetBytes(s));
            }

            bw.Write((byte) 0x00);
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

        #region Nested type: ClipEndSection

        public class ClipEndSection : AHandlerElement
        {
            private Single mW;
            private Single mX;
            private Single mY;
            private Single mZ;

            public ClipEndSection(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public ClipEndSection(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public ClipEndSection(int apiVersion, EventHandler handler, ClipEndSection basis) : this(apiVersion, handler, basis.X, basis.Y, basis.Z, basis.W) { }

            public ClipEndSection(int APIversion, EventHandler handler, float x, float y, float z, float w) : base(APIversion, handler)
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
                set
                {
                    if (mX != value)
                    {
                        mX = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set
                {
                    if (mY != value)
                    {
                        mY = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set
                {
                    if (mZ != value)
                    {
                        mZ = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(4)]
            public float W
            {
                get { return mW; }
                set
                {
                    if (mW != value)
                    {
                        mW = value;
                        OnElementChanged();
                    }
                }
            }

            public string Value { get { return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW); } }
            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

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

            public override AHandlerElement Clone(EventHandler handler) { return new ClipEndSection(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: CountedOffsetItemList

        public abstract class CountedOffsetItemList<T> : DependentList<T> where T : AHandlerElement, IEquatable<T>
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
                    ((IList<T>) this).Add(CreateElement(s));
                }
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                uint[] offsets = new uint[base.Count];
                WriteCount(s, Count);
                long startOffset = s.Position;
                for (int i = 0; i < base.Count; i++)
                {
                    bw.Write(offsets[i]);
                }
                for (int i = 0; i < base.Count; i++)
                {
                    offsets[i] = (uint) (s.Position - startOffset);
                    WriteElement(s, this[i]);
                }
                long endOffset = s.Position;
                s.Seek(startOffset, SeekOrigin.Begin);
                for (int i = 0; i < base.Count; i++)
                {
                    bw.Write(offsets[i]);
                }
                s.Seek(endOffset, SeekOrigin.Begin);
            }

            public override void Add() { base.Add(new object[] {}); }

            protected abstract override T CreateElement(Stream s);

            protected abstract override void WriteElement(Stream s, T element);
        }

        #endregion

        #region Nested type: DestroyPropEvent

        [ConstructorParameters(new object[] {ClipEventType.DestroyProp})]
        public class DestroyPropEvent : Event
        {
            private UInt32 mPropNameHash;

            public DestroyPropEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public DestroyPropEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public DestroyPropEvent(int apiVersion, EventHandler handler, DestroyPropEvent basis) : base(apiVersion, handler, basis) { mPropNameHash = basis.PropNameHash; }

            public DestroyPropEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint propNameHash, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName) { mPropNameHash = propNameHash; }

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
                set
                {
                    if (mPropNameHash != value)
                    {
                        mPropNameHash = value;
                        OnElementChanged();
                    }
                }
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

            public override AHandlerElement Clone(EventHandler handler) { return new DestroyPropEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: EffectEvent

        [ConstructorParameters(new object[] {ClipEventType.Effect})]
        public class EffectEvent : Event
        {
            private UInt32 mActorNameHash;
            private UInt32 mEffectNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown01;
            private UInt32 mUnknown02;
            private UInt32 mUnknown03;

            public EffectEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public EffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public EffectEvent(int apiVersion, EventHandler handler, EffectEvent basis) : base(apiVersion, handler, basis)
            {
                mUnknown01 = basis.Unknown01;
                mUnknown02 = basis.Unknown02;
                mEffectNameHash = basis.EffectNameHash;
                mActorNameHash = basis.ActorNameHash;
                mSlotNameHash = basis.SlotNameHash;
                mUnknown03 = basis.Unknown03;
            }

            public EffectEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint unknown01, uint unknown02, uint effectNameHash, uint actorNameHash, uint slotNameHash, uint unknown03, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName)
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
                set
                {
                    if (mUnknown01 != value)
                    {
                        mUnknown01 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(9)]
            public uint Unknown02
            {
                get { return mUnknown02; }
                set
                {
                    if (mUnknown02 != value)
                    {
                        mUnknown02 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(10)]
            public uint EffectNameHash
            {
                get { return mEffectNameHash; }
                set
                {
                    if (mEffectNameHash != value)
                    {
                        mEffectNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(11)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set
                {
                    if (mActorNameHash != value)
                    {
                        mActorNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(12)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set
                {
                    if (mSlotNameHash != value)
                    {
                        mSlotNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(13)]
            public uint Unknown03
            {
                get { return mUnknown03; }
                set
                {
                    if (mUnknown03 != value)
                    {
                        mUnknown03 = value;
                        OnElementChanged();
                    }
                }
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


            public override AHandlerElement Clone(EventHandler handler) { return new EffectEvent(requestedApiVersion, handler, this); }
        }

        #endregion

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

        #region Nested type: Event

        public abstract class Event : AHandlerElement,
                                      IEquatable<Event>
        {
            private readonly ClipEventType mType;
            private String mEventName;
            private Single mFloat01;
            private Single mFloat02;
            private UInt32 mId;
            private UInt32 mInt01;
            private UInt16 mShort01;
            private Single mTimecode;

            protected Event(int apiVersion, EventHandler handler, ClipEventType type) : this(apiVersion, handler, type, 0xC1E4, 0, 0f, -1f, -1f, 0, "") { }

            protected Event(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : this(apiVersion, handler, type)
            {
                mType = type;
                Parse(s);
            }

            protected Event(int apiVersion, EventHandler handler, Event basis) : this(apiVersion, handler, basis.Type, basis.Short01, basis.Id, basis.Timecode, basis.Float01, basis.Float02, basis.Int01, basis.EventName) { }

            protected Event(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string eventName) : base(APIversion, handler)
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
            public ClipEventType Type { get { return mType; } }

            [ElementPriority(1)]
            public string EventName
            {
                get { return mEventName; }
                set
                {
                    if (mEventName != value)
                    {
                        mEventName = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public float Float01
            {
                get { return mFloat01; }
                set
                {
                    if (mFloat01 != value)
                    {
                        mFloat01 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public float Float02
            {
                get { return mFloat02; }
                set
                {
                    if (mFloat02 != value)
                    {
                        mFloat02 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(4)]
            public uint Id
            {
                get { return mId; }
                set
                {
                    if (mId != value)
                    {
                        mId = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(5)]
            public uint Int01
            {
                get { return mInt01; }
                set
                {
                    if (mInt01 != value)
                    {
                        mInt01 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(6)]
            public ushort Short01
            {
                get { return mShort01; }
                set
                {
                    if (mShort01 != value)
                    {
                        mShort01 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(7)]
            public float Timecode
            {
                get { return mTimecode; }
                set
                {
                    if (mTimecode != value)
                    {
                        mTimecode = value;
                        OnElementChanged();
                    }
                }
            }

            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

            #region IEquatable<Event> Members

            public bool Equals(Event other) { return base.Equals(other); }

            #endregion

            public static Event CreateInstance(int apiVersion, EventHandler handler, ClipEventType type) { return CreateInstance(apiVersion, handler, type, null); }

            public static Event CreateInstance(int apiVersion, EventHandler handler, ClipEventType type, Stream s)
            {
                switch (type)
                {
                    case ClipEventType.Parent:
                        return new ParentEvent(apiVersion, handler, type, s);
                    case ClipEventType.DestroyProp:
                        return new DestroyPropEvent(apiVersion, handler, type, s);
                    case ClipEventType.Effect:
                        return new EffectEvent(apiVersion, handler, type, s);
                    case ClipEventType.Sound:
                        return new SoundEvent(apiVersion, handler, type, s);
                    case ClipEventType.Script:
                        return new ScriptEvent(apiVersion, handler, type, s);
                    case ClipEventType.Visibility:
                        return new VisibilityEvent(apiVersion, handler, type, s);
                    case ClipEventType.StopEffect:
                        return new StopEffectEvent(apiVersion, handler, type, s);
                    case ClipEventType.UnParent:
                        return new UnparentEvent(apiVersion, handler, type, s);
                    default:
                        throw new InvalidDataException(String.Format("Event type: {0} not implemented", type));
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
                while ((s.Position%4) != 0) br.ReadByte(); //padding to next DWORD
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
                while ((s.Position%4) != 0) bw.Write((byte) 0x00); //padding to next DWORD
            }

            public override string ToString() { return mType.ToString(); }

            public abstract override AHandlerElement Clone(EventHandler handler);
        }

        #endregion

        #region Nested type: EventList

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
                for (uint i = 0; i < count; i++)
                {
                    ((IList<Event>) this).Add(CreateElement(s));
                }
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
                for (int i = 0; i < base.Count; i++)
                {
                    WriteElement(s, this[i]);
                }
                long endPos = s.Position;
                uint size = (uint) (endPos - startPos);
                s.Seek(offsetPos, SeekOrigin.Begin);
                bw.Write(size);
                s.Seek(endPos, SeekOrigin.Begin);
            }

            protected override Event CreateElement(Stream s)
            {
                ClipEventType type = (ClipEventType) new BinaryReader(s).ReadUInt16();
                return Event.CreateInstance(0, handler, type, s);
            }

            protected override void WriteElement(Stream s, Event element)
            {
                new BinaryWriter(s).Write((ushort) element.Type);
                element.UnParse(s);
            }

            protected override Type GetElementType(params object[] fields)
            {
                if (fields != null && fields.Length > 0)
                {
                    if (typeof (Event).IsAssignableFrom(fields[0].GetType()))
                    {
                        return fields[0].GetType();
                    }
                    if (typeof (ClipEventType).IsAssignableFrom(fields[0].GetType()))
                    {
                        ClipEventType type = (ClipEventType) fields[0];
                        switch (type)
                        {
                            case ClipEventType.Parent:
                                return typeof (ParentEvent);
                            case ClipEventType.DestroyProp:
                                return typeof (DestroyPropEvent);
                            case ClipEventType.Effect:
                                return typeof (EffectEvent);
                            case ClipEventType.Sound:
                                return typeof (SoundEvent);
                            case ClipEventType.Script:
                                return typeof (ScriptEvent);
                            case ClipEventType.Visibility:
                                return typeof (VisibilityEvent);
                            case ClipEventType.StopEffect:
                                return typeof (StopEffectEvent);
                            case ClipEventType.UnParent:
                                return typeof (UnparentEvent);
                            default:
                                throw new NotImplementedException(String.Format("Event type: {0} not implemented", type));
                        }
                    }
                }
                return base.GetElementType(fields);
            }

            public override void Add() { throw new NotSupportedException(); }
        }

        #endregion

        #region Nested type: EventTable

        public class EventTable : AHandlerElement
        {
            private EventList mEvents;
            private UInt32 mVersion;

            public EventTable(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mEvents = new EventList(handler); }

            public EventTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public EventTable(int apiVersion, EventHandler handler, EventTable basis) : this(apiVersion, handler, basis.Version, basis.Events) { }

            public EventTable(int APIversion, EventHandler handler, uint version, EventList events) : base(APIversion, handler)
            {
                mVersion = version;
                mEvents = events;
            }

            [ElementPriority(1)]
            public uint Version
            {
                get { return mVersion; }
                set
                {
                    if (mVersion != value)
                    {
                        mVersion = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public EventList Events
            {
                get { return mEvents; }
                set
                {
                    if (mEvents != value)
                    {
                        mEvents = value;
                        OnElementChanged();
                    }
                }
            }


            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

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

            public override AHandlerElement Clone(EventHandler handler) { return new EventTable(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: IKChainEntry

        public class IKChainEntry : AHandlerElement,
                                    IEquatable<IKChainEntry>
        {
            private IKTargetList mIkTargets;

            public IKChainEntry(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mIkTargets = new IKTargetList(handler); }

            public IKChainEntry(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public IKChainEntry(int apiVersion, EventHandler handler, IKChainEntry basis) : this(apiVersion, handler, basis.IKTargets) { }

            public IKChainEntry(int APIversion, EventHandler handler, IKTargetList ikTargets) : base(APIversion, handler) { mIkTargets = ikTargets; }

            public IKTargetList IKTargets
            {
                get { return mIkTargets; }
                set
                {
                    if (mIkTargets != value)
                    {
                        mIkTargets = value;
                        OnElementChanged();
                    }
                }
            }

            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

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

            #region IEquatable<IKChainEntry> Members

            public bool Equals(IKChainEntry other) { return base.Equals(other); }

            #endregion

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                if (br.ReadUInt32() != 0x7E7E7E7E)
                    throw new InvalidDataException(String.Format("Expected 0x7E7E7E7E Padding at 0x{0:X8}", s.Position - 4)); //7E7E7E7E padding
                mIkTargets = new IKTargetList(handler, s);
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(new byte[] {0x7E, 0x7E, 0x7E, 0x7E}); //7E7E7E7E padding
                mIkTargets.UnParse(s);
            }

            public override AHandlerElement Clone(EventHandler handler) { return new IKChainEntry(0, handler, this); }
        }

        #endregion

        #region Nested type: IKChainList

        public class IKChainList : CountedOffsetItemList<IKChainEntry>
        {
            public IKChainList(EventHandler handler) : base(handler) { }

            public IKChainList(EventHandler handler, Stream s) : base(handler, s) { }

            public IKChainList(EventHandler handler, IEnumerable<IKChainEntry> ilt) : base(handler, ilt) { }

            protected override IKChainEntry CreateElement(Stream s) { return new IKChainEntry(0, handler, s); }

            protected override void WriteElement(Stream s, IKChainEntry element) { element.UnParse(s); }
        }

        #endregion

        #region Nested type: IKTarget

        public class IKTarget : AHandlerElement,
                                IEquatable<IKTarget>
        {
            private UInt32 mIndex;
            private string mTargetName;
            private string mTargetNamespace;
            public IKTarget(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public IKTarget(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public IKTarget(int apiVersion, EventHandler handler, IKTarget basis) : this(apiVersion, handler, basis.Index, basis.TargetNamespace, basis.TargetName) { }

            public IKTarget(int APIversion, EventHandler handler, uint index, string targetNamespace, string targetName) : base(APIversion, handler)
            {
                mIndex = index;
                mTargetNamespace = targetNamespace;
                mTargetName = targetName;
            }

            [ElementPriority(1)]
            public uint Index
            {
                get { return mIndex; }
                set
                {
                    if (mIndex != value)
                    {
                        mIndex = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public string TargetNamespace
            {
                get { return mTargetNamespace; }
                set
                {
                    if (mTargetNamespace != value)
                    {
                        mTargetNamespace = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public string TargetName
            {
                get { return mTargetName; }
                set
                {
                    if (mTargetName != value)
                    {
                        mTargetName = value;
                        OnElementChanged();
                    }
                }
            }

            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }
            public string Value { get { return ToString(); } }

            #region IEquatable<IKTarget> Members

            public bool Equals(IKTarget other) { return mIndex.Equals(other.mIndex) && mTargetNamespace.Equals(other.mTargetNamespace) && mTargetName.Equals(other.mTargetName); }

            #endregion

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

            public override AHandlerElement Clone(EventHandler handler) { return new IKTarget(requestedApiVersion, handler, this); }

            public override string ToString() { return String.Format("(0x{0:X8}){1}:{2}", mIndex, mTargetNamespace, mTargetName); }
        }

        #endregion

        #region Nested type: IKTargetList

        public class IKTargetList : CountedOffsetItemList<IKTarget>
        {
            public IKTargetList(EventHandler handler) : base(handler) { }

            public IKTargetList(EventHandler handler, Stream s) : base(handler, s) { }

            public IKTargetList(EventHandler handler, IEnumerable<IKTarget> ilt) : base(handler, ilt) { }

            protected override IKTarget CreateElement(Stream s) { return new IKTarget(0, handler, s); }

            protected override void WriteElement(Stream s, IKTarget element) { element.UnParse(s); }
        }

        #endregion

        #region Nested type: IKTargetTable

        public class IKTargetTable : AHandlerElement
        {
            private IKChainList mIkChains;

            public IKTargetTable(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mIkChains = new IKChainList(base.handler); }

            public IKTargetTable(int apiVersion, EventHandler handler, IKTargetTable basis) : this(apiVersion, handler, basis.IKChains) { }

            public IKTargetTable(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            public IKTargetTable(int APIversion, EventHandler handler, IKChainList ikChains) : base(APIversion, handler) { mIkChains = ikChains; }

            public IKChainList IKChains
            {
                get { return mIkChains; }
                set
                {
                    if (mIkChains != value)
                    {
                        mIkChains = value;
                        OnElementChanged();
                    }
                }
            }

            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

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

            private void Parse(Stream s) { mIkChains = new IKChainList(handler, s); }
            public void UnParse(Stream s) { mIkChains.UnParse(s); }

            public override AHandlerElement Clone(EventHandler handler) { return new IKTargetTable(0, handler, this); }
        }

        #endregion

        #region Nested type: ParentEvent

        [ConstructorParameters(new object[] {ClipEventType.Parent})]
        public class ParentEvent : Event
        {
            private UInt32 mActorNameHash;
            private UInt32 mObjectNameHash;
            private UInt32 mSlotNameHash;
            private Single[] mTransform;
            private UInt32 mUnknown01;

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
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n", mTransform[0], mTransform[1], mTransform[2], mTransform[3]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n", mTransform[4], mTransform[5], mTransform[6], mTransform[7]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n", mTransform[8], mTransform[9], mTransform[10], mTransform[11]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]\n", mTransform[12], mTransform[13], mTransform[14], mTransform[15]);
                    return sb.ToString();
                }
            }

            [ElementPriority(8)]
            public uint ActorNameHash
            {
                get { return mActorNameHash; }
                set
                {
                    if (mActorNameHash != value)
                    {
                        mActorNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(9)]
            public uint ObjectNameHash
            {
                get { return mObjectNameHash; }
                set
                {
                    if (mObjectNameHash != value)
                    {
                        mObjectNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(10)]
            public uint SlotNameHash
            {
                get { return mSlotNameHash; }
                set
                {
                    if (mSlotNameHash != value)
                    {
                        mSlotNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(11)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set
                {
                    if (mUnknown01 != value)
                    {
                        mUnknown01 = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(12)]
            public float[] Transform
            {
                get { return mTransform; }
                set
                {
                    if (mTransform != value)
                    {
                        mTransform = value;
                        OnElementChanged();
                    }
                }
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
                for (int i = 0; i < 16; i++)
                {
                    mTransform[i] = br.ReadSingle();
                }
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

            public override AHandlerElement Clone(EventHandler handler) { return new ParentEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: ScriptEvent

        [ConstructorParameters(new object[] {ClipEventType.Script})]
        public class ScriptEvent : Event
        {
            public ScriptEvent(int apiVersion, EventHandler handler, ScriptEvent basis) : base(apiVersion, handler, basis) { }

            public ScriptEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public ScriptEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public ScriptEvent(int apiVersion, EventHandler handler, Event basis) : base(apiVersion, handler, basis) { }

            public ScriptEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName) { }

            public override AHandlerElement Clone(EventHandler handler) { return new ScriptEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: SoundEvent

        [ConstructorParameters(new object[] {ClipEventType.Sound})]
        public class SoundEvent : Event
        {
            private String mSoundName;

            public SoundEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public SoundEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public SoundEvent(int apiVersion, EventHandler handler, SoundEvent basis) : base(apiVersion, handler, basis) { mSoundName = basis.SoundName; }

            public SoundEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, string soundName, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName) { mSoundName = soundName; }

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
                set
                {
                    if (mSoundName != value)
                    {
                        mSoundName = value;
                        OnElementChanged();
                    }
                }
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

            public override AHandlerElement Clone(EventHandler handler) { return new SoundEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: StopEffectEvent

        [ConstructorParameters(new object[] {ClipEventType.StopEffect})]
        public class StopEffectEvent : Event
        {
            private UInt32 mEffectNameHash;
            private UInt32 mUnknown01;

            public StopEffectEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public StopEffectEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public StopEffectEvent(int apiVersion, EventHandler handler, StopEffectEvent basis) : base(apiVersion, handler, basis)
            {
                mEffectNameHash = basis.EffectNameHash;
                mUnknown01 = basis.Unknown01;
            }

            public StopEffectEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint effectNameHash, uint unknown01, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName)
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
                set
                {
                    if (mEffectNameHash != value)
                    {
                        mEffectNameHash = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(9)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set
                {
                    if (mUnknown01 != value)
                    {
                        mUnknown01 = value;
                        OnElementChanged();
                    }
                }
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


            public override AHandlerElement Clone(EventHandler handler) { return new StopEffectEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: UnparentEvent

        [ConstructorParameters(new object[] {ClipEventType.UnParent})]
        public class UnparentEvent : Event
        {
            private UInt32 mObjectNameHash;

            public UnparentEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public UnparentEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public UnparentEvent(int apiVersion, EventHandler handler, UnparentEvent basis) : base(apiVersion, handler, basis) { mObjectNameHash = basis.ObjectNameHash; }

            public UnparentEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, uint objectNameHash, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName) { mObjectNameHash = objectNameHash; }

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
                set
                {
                    if (mObjectNameHash != value)
                    {
                        mObjectNameHash = value;
                        OnElementChanged();
                    }
                }
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

            public override AHandlerElement Clone(EventHandler handler) { return new UnparentEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        #region Nested type: VisibilityEvent

        [ConstructorParameters(new object[] {ClipEventType.Visibility})]
        public class VisibilityEvent : Event
        {
            private Single mVisibility;

            public VisibilityEvent(int apiVersion, EventHandler handler, ClipEventType type) : base(apiVersion, handler, type) { }

            public VisibilityEvent(int apiVersion, EventHandler handler, ClipEventType type, Stream s) : base(apiVersion, handler, type, s) { }

            public VisibilityEvent(int apiVersion, EventHandler handler, VisibilityEvent basis) : base(apiVersion, handler, basis) { mVisibility = basis.Visibility; }

            public VisibilityEvent(int APIversion, EventHandler handler, ClipEventType type, ushort short01, uint id, float timecode, float float01, float float02, uint int01, float visibility, string eventName) : base(APIversion, handler, type, short01, id, timecode, float01, float02, int01, eventName) { mVisibility = visibility; }

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
                set
                {
                    if (mVisibility != value)
                    {
                        mVisibility = value;
                        OnElementChanged();
                    }
                }
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

            public override AHandlerElement Clone(EventHandler handler) { return new VisibilityEvent(requestedApiVersion, handler, this); }
        }

        #endregion

        public class Clip : AHandlerElement
        {
            
            #region Fields

            private String mAnimName;
            private Single mFrameDuration;
            private UInt16 mMaxFrameCount;
            private String mSrcName;
            private TrackList mTracks;
            private UInt32 mUnknown01;
            private UInt16 mUnknown02;
            private UInt32 mVersion;

            #endregion

            #region ContentFields

            [ElementPriority(1)]
            public UInt32 Version
            {
                get { return mVersion; }
                set
                {
                    mVersion = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public UInt32 Unknown01
            {
                get { return mUnknown01; }
                set
                {
                    mUnknown01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public Single FrameDuration
            {
                get { return mFrameDuration; }
                set
                {
                    mFrameDuration = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public UInt16 MaxFrameCount
            {
                get { return mMaxFrameCount; }
                set
                {
                    mMaxFrameCount = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public UInt16 Unknown02
            {
                get { return mUnknown02; }
                set
                {
                    mUnknown02 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public String AnimName
            {
                get { return mAnimName; }
                set
                {
                    mAnimName = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public String SrcName
            {
                get { return mSrcName; }
                set
                {
                    mSrcName = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(8)]
            public TrackList Tracks
            {
                get { return mTracks; }
                set
                {
                    mTracks = value;
                    OnElementChanged();
                }
            }

            public string Value { get { return ValueBuilder; } }

            #endregion

            #region Constructors

            public Clip(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mTracks = new TrackList(handler); }

            public Clip(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                s.Position = 0L;
                Parse(s);
            }

            #endregion

            #region I/O

            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                var foo = FOURCC(br.ReadUInt64());
                if (foo != "_pilC3S_")
                    throw new Exception("Bad clip header: Expected \"_S3Clip_\"");
                mVersion = br.ReadUInt32();
                mUnknown01 = br.ReadUInt32();
                mFrameDuration = br.ReadSingle();
                mMaxFrameCount = br.ReadUInt16();
                mUnknown02 = br.ReadUInt16();

                uint curveCount = br.ReadUInt32();
                uint indexedFloatCount = br.ReadUInt32();
                uint curveDataOffset = br.ReadUInt32();
                uint frameDataOffset = br.ReadUInt32();
                uint animNameOffset = br.ReadUInt32();
                uint srcNameOffset = br.ReadUInt32();

                if (Settings.Checking && s.Position != curveDataOffset)
                    throw new InvalidDataException("Bad Curve Data Offset");

                List<CurveDataInfo> curveDataInfos = new List<CurveDataInfo>();
                for (int i = 0; i < curveCount; i++)
                {
                    CurveDataInfo p = new CurveDataInfo();
                    p.FrameDataOffset = br.ReadUInt32();
                    p.TrackKey = br.ReadUInt32();
                    p.Offset = br.ReadSingle();
                    p.Scale = br.ReadSingle();
                    p.FrameCount = br.ReadUInt16();
                    p.Flags = new CurveDataFlags(br.ReadByte());
                    p.Type = (CurveType)br.ReadByte();
                    curveDataInfos.Add(p);
                }

                if (Settings.Checking && s.Position != animNameOffset)
                    throw new InvalidDataException("Bad Name Offset");
                mAnimName = ClipResource.ReadZString(br);
                if (Settings.Checking && s.Position != srcNameOffset)
                    throw new InvalidDataException("Bad SourceName Offset");
                mSrcName = ClipResource.ReadZString(br);

                if (Settings.Checking && s.Position != frameDataOffset)
                    throw new InvalidDataException("Bad Indexed Floats Offset");
                List<float> indexedFloats = new List<float>();
                for (int i = 0; i < indexedFloatCount; i++)
                {
                    indexedFloats.Add(br.ReadSingle());
                }

                Dictionary<uint, List<Curve>> trackMap = new Dictionary<uint, List<Curve>>();
                for (int i = 0; i < curveDataInfos.Count; i++)
                {
                    CurveDataInfo curveDataInfo = curveDataInfos[i];
                    if (Settings.Checking && s.Position != curveDataInfo.FrameDataOffset)
                        throw new InvalidDataException("Bad FrameData offset.");
                    Curve curve = (Curve)Activator.CreateInstance(Curve.GetCurveType(curveDataInfo.Type), new object[] { 0, handler, curveDataInfo.Type, s, curveDataInfo, indexedFloats });
                    if (!trackMap.ContainsKey(curveDataInfo.TrackKey)) trackMap[curveDataInfo.TrackKey] = new List<Curve>();
                    trackMap[curveDataInfo.TrackKey].Add(curve);
                }

                List<Track> tracks = new List<Track>();
                foreach (var k in trackMap.Keys)
                {
                    tracks.Add(new Track(0, handler, k, trackMap[k]));
                }
                mTracks = new TrackList(handler, tracks);

                if (Settings.Checking && s.Position != s.Length)
                    throw new InvalidDataException("Unexpected End of Clip.");
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(Encoding.ASCII.GetBytes("_pilC3S_"));
                bw.Write(mVersion);
                bw.Write(mUnknown01);
                bw.Write(mFrameDuration);
                bw.Write(mMaxFrameCount);
                bw.Write(Unknown02);

                List<float> indexedFloats = new List<float>();
                List<CurveDataInfo> curveDataInfos = new List<CurveDataInfo>();


                UInt32 curveCount = 0;
                byte[] frameData;
                using (var frameStream = new MemoryStream())
                {
                    foreach (var track in Tracks)
                    {
                        foreach (var curve in track.Curves)
                        {
                            curveCount++;
                            Single scale = 1f;
                            Single offset = 0f;

                            var values = curve.SelectFloats();
                            if (values.Any())
                            {
                                float min = values.Min();
                                float max = values.Max();
                                offset = (min + max) / 2f;
                                scale = (min - max) / 2f;
                            }
                            //not sure what really determines whether to index or not
                            Boolean isIndexed = curve.Frames.Count == 0 ? true : curve.Type == CurveType.Position ? IsIndexed(curve.Frames.Cast<Float3Frame>()) : false;
                            var flags = new CurveDataFlags();
                            flags.Format = isIndexed ? CurveDataFormat.Indexed : CurveDataFormat.Packed;
                            flags.Type = Curve.GetDataType(curve.Type);
                            flags.Static = curve.Frames.Count == 0;
                            var curveDataInfo = new CurveDataInfo { Offset = offset, Flags = flags, FrameCount = curve.Frames.Count, FrameDataOffset = (UInt32)frameStream.Position, Scale = scale, TrackKey = track.TrackKey, Type = curve.Type };
                            curve.UnParse(frameStream, curveDataInfo, indexedFloats);
                            curveDataInfos.Add(curveDataInfo);
                        }
                    }
                    frameData = frameStream.ToArray();
                }

                bw.Write(curveCount);
                bw.Write(indexedFloats.Count);
                long offsets = s.Position;
                uint curveDataOffset = 0;
                uint frameDataOffset = 0;
                uint animNameOffset = 0;
                uint srcNameOffset = 0;
                s.Seek(4 * sizeof(UInt32), SeekOrigin.Current);


                curveDataOffset = (uint)s.Position;
                uint frameOffset = (uint)(curveDataOffset + (20 * curveDataInfos.Count) + mAnimName.Length + mSrcName.Length + 2 + (sizeof(Single) * indexedFloats.Count));
                foreach (var curveDataInfo in curveDataInfos)
                {
                    bw.Write((curveDataInfo.FrameDataOffset + frameOffset));
                    bw.Write(curveDataInfo.TrackKey);
                    bw.Write(curveDataInfo.Offset);
                    bw.Write(curveDataInfo.Scale);
                    bw.Write((UInt16)curveDataInfo.FrameCount);
                    bw.Write(curveDataInfo.Flags.Raw);
                    bw.Write((Byte)curveDataInfo.Type);
                }

                animNameOffset = (uint)s.Position;
                ClipResource.WriteZString(bw, AnimName);
                srcNameOffset = (uint)s.Position;
                ClipResource.WriteZString(bw, SrcName);

                frameDataOffset = (uint)s.Position;
                foreach (var f in indexedFloats) bw.Write(f);
                bw.Write(frameData);
                s.Seek(offsets, SeekOrigin.Begin);
                bw.Write(curveDataOffset);
                bw.Write(frameDataOffset);
                bw.Write(animNameOffset);
                bw.Write(srcNameOffset);
                s.Position = s.Length;
            }

            #endregion

            private const int kRecommendedApiVersion = 1;
            public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

            public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }

            private static Boolean IsIndexed(IEnumerable<Float3Frame> source)
            {
                if (source.Count() < 5) return false;
                var x = source.Select(frame => frame.Data[0]).Distinct();
                var y = source.Select(frame => frame.Data[0]).Distinct();
                var z = source.Select(frame => frame.Data[0]).Distinct();

                return x.Count() == 1 && x.First() == 0 || y.Count() == 1 && y.First() == 0 || z.Count() == 1 && z.First() == 0;
            }

            public override AHandlerElement Clone(EventHandler handler) { throw new NotImplementedException(); }
        }
        public class Track : AHandlerElement,
                             IEquatable<Track>
        {
            private CurveList mCurves;
            private UInt32 mTrackKey;
            public Track(int APIversion, EventHandler handler) : base(APIversion, handler) { mCurves = new CurveList(handler); }

            public Track(int APIversion, EventHandler handler, Track basis) : this(APIversion, handler, basis.mTrackKey, basis.Curves) { }

            public Track(int APIversion, EventHandler handler, UInt32 trackKey, IEnumerable<Curve> curves)
                : this(APIversion, handler)
            {
                mTrackKey = trackKey;
                mCurves = new CurveList(handler, curves);
            }

            [ElementPriority(1)]
            public uint TrackKey
            {
                get { return mTrackKey; }
                set
                {
                    if (mTrackKey != value)
                    {
                        mTrackKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public CurveList Curves
            {
                get { return mCurves; }
                set
                {
                    mCurves = value;
                    OnElementChanged();
                }
            }
            public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }
            public override int RecommendedApiVersion { get { return 1; } }
            public string Value { get { return ValueBuilder; } }
            #region IEquatable<Track> Members
            public bool Equals(Track other) { return mTrackKey.Equals(other.mTrackKey); }
            #endregion
            public override AHandlerElement Clone(EventHandler handler) { return new Track(0, handler, this); }
        }
        public class TrackList : DependentList<Track>
        {
            public TrackList(EventHandler handler) : base(handler) { }
            public TrackList(EventHandler handler, IEnumerable<Track> ilt) : base(handler, ilt) { }
            public TrackList(EventHandler handler, long size) : base(handler, size) { }


            public override void Add() { base.Add(new object[] { }); }

            protected override Track CreateElement(Stream s) { throw new NotSupportedException(); }

            protected override void WriteElement(Stream s, Track element) { throw new NotSupportedException(); }
        }
        public abstract class Curve : AHandlerElement,
                                  IEquatable<Curve>
        {
            protected FrameList mFrames;
            protected CurveType mType;

            protected Curve(int apiVersion, EventHandler handler, CurveType type)
                : base(apiVersion, handler)
            {
                mType = type;
                mFrames = new FrameList(handler, Curve.GetDataType(type));
            }

            protected Curve(int apiVersion, EventHandler handler, Curve basis) : this(apiVersion, handler, basis.mType) { mFrames = new FrameList(handler, Curve.GetDataType(mType), basis.Frames); }

            public Curve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats) : this(apiVersion, handler, type) { Parse(s, info, indexedFloats); }

            [ElementPriority(2)]
            public CurveType Type { get { return mType; } }

            [ElementPriority(3)]
            public FrameList Frames
            {
                get { return mFrames; }
                set
                {
                    if (mFrames != value)
                    {
                        mFrames = value;
                        OnElementChanged();
                    }
                }
            }

            public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

            public override int RecommendedApiVersion { get { return 1; } }
            public virtual string Value { get { return ValueBuilder; } }

            #region IEquatable<Curve> Members

            public bool Equals(Curve other) { return base.Equals(other); }

            #endregion
            public static Int32 GetBitsPerFloat(CurveDataType curveType)
            {
                switch (curveType)
                {
                    case CurveDataType.Float3:
                        return 10;
                    case CurveDataType.Float4:
                        return 12;
                    default: throw new NotSupportedException();
                }
            }
            public static Int32 GetFloatCount(CurveDataType curveType)
            {
                switch (curveType)
                {
                    case CurveDataType.Float3:
                        return 3;
                    case CurveDataType.Float4:
                        return 4;
                    default: throw new NotSupportedException();
                }
            }
            public static Int32 GetPackedCount(CurveDataType curveType)
            {
                switch (curveType)
                {
                    case CurveDataType.Float3:
                        return 1;
                    case CurveDataType.Float4:
                        return 4;
                    default: throw new NotSupportedException();
                }
            }
            public static Type GetCurveType(CurveType t)
            {
                switch (t)
                {
                    case CurveType.Position:
                        return typeof(PositionCurve);
                    case CurveType.Orientation:
                        return typeof(OrientationCurve);
                    default:
                        throw new NotSupportedException();
                }
            }
            public static CurveDataType GetDataType(CurveType t)
            {
                switch (t)
                {
                    case CurveType.Position:
                        return CurveDataType.Float3;
                    case CurveType.Orientation:
                        return CurveDataType.Float4;
                    default:
                        throw new NotSupportedException();
                }
            }
            protected virtual void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats) { mFrames = new FrameList(handler, Curve.GetDataType(mType), s, info, indexedFloats); }
            public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats) { mFrames.UnParse(s, info, indexedFloats); }
            public override AHandlerElement Clone(EventHandler handler) { return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this }); }

            public virtual IEnumerable<float> SelectFloats()
            {
                var floats = new List<float>();
                foreach (var f in mFrames) floats.AddRange(f.GetFloatValues());
                return floats;
            }
        }
        [ConstructorParameters(new object[] { CurveType.Position })]
        public class PositionCurve : Curve
        {
            public PositionCurve(int apiVersion, EventHandler handler, CurveType type) : base(apiVersion, handler, type) { }
            public PositionCurve(int apiVersion, EventHandler handler, PositionCurve basis) : base(apiVersion, handler, basis) { }
            public PositionCurve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, type, s, info, indexedFloats) { }
        }
        [ConstructorParameters(new object[] { CurveType.Orientation })]
        public class OrientationCurve : Curve
        {
            public OrientationCurve(int apiVersion, EventHandler handler, OrientationCurve basis) : base(apiVersion, handler, basis) { }
            public OrientationCurve(int apiVersion, EventHandler handler, CurveType type) : base(apiVersion, handler, type) { }
            public OrientationCurve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, type, s, info, indexedFloats) { }
        }
        public class CurveList : DependentList<Curve>
        {
            public CurveList(EventHandler handler, IEnumerable<Curve> ilt) : base(handler, ilt) { }

            public CurveList(EventHandler handler) : base(handler) { }

            protected override Type GetElementType(params object[] fields)
            {
                if (fields.Length == 1 )
                {
                    if( typeof(CurveType).IsAssignableFrom(fields[0].GetType())) return Curve.GetCurveType((CurveType) fields[0]);
                    if (typeof(Curve).IsAssignableFrom(fields[0].GetType())) return fields[0].GetType();
                    
                }
                return base.GetElementType(fields);
            }

            #region Unused
            public override void Add() { throw new NotSupportedException(); }
            protected override Curve CreateElement(Stream s)
            {
                throw new NotSupportedException();
            }

            protected override void WriteElement(Stream s, Curve element)
            {
                throw new NotSupportedException();
            }
            #endregion
        }
        public abstract class Frame : AHandlerElement,
                                  IEquatable<Frame>
        {
            protected float[] mData;
            protected UInt16 mFlags;
            protected UInt16 mFrameIndex;

            protected Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mData = new float[Curve.GetFloatCount(DataType)]; }

            protected Frame(int apiVersion, EventHandler handler, Frame basis)
                : base(apiVersion, handler)
            {
                mData = basis.mData;
                mFrameIndex = basis.mFrameIndex;
                mFlags = basis.mFlags;
            }

            public Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : this(apiVersion, handler) { Parse(s, info, indexedFloats); }
            public abstract CurveDataType DataType { get; }
            protected abstract UInt64 ReadPacked(Stream stream);
            protected abstract void WritePacked(Stream stream, UInt64 packed);


            [ElementPriority(1)]
            public ushort FrameIndex
            {
                get { return mFrameIndex; }
                set
                {
                    if (mFrameIndex != value)
                    {
                        mFrameIndex = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public UInt16 Flags
            {
                get { return mFlags; }
                set
                {
                    if (mFlags != value)
                    {
                        mFlags = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public float[] Data { get { return mData; } set { mData = value; } }

            public string Value { get { return ValueBuilder; } }

            public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

            public override int RecommendedApiVersion { get { return 1; } }

            #region IEquatable<Frame> Members

            public bool Equals(Frame other)
            {
                var a = GetFloatValues();
                var b = other.GetFloatValues();
                if (a.Count() != b.Count()) return false;
                return !(from af in a from bf in b where af != bf select af).Any();
            }

            #endregion
            public static Type GetFrameType(CurveDataType dataType)
            {
                switch(dataType)
                {
                    case CurveDataType.Float3:
                        return typeof (Float3Frame);
                    case CurveDataType.Float4:
                        return typeof (Float4Frame);
                    default:throw new NotSupportedException();
                }
            }
            

            public void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
            {
                BinaryReader br = new BinaryReader(s);
                mFrameIndex = br.ReadUInt16();
                var flags = br.ReadUInt16();
                mFlags = (UInt16)(flags >> 4);

                switch (info.Flags.Format)
                {
                    case CurveDataFormat.Indexed:

                        for (int floatsRead = 0; floatsRead < Curve.GetFloatCount(DataType); floatsRead++)
                        {
                            var val = indexedFloats[br.ReadUInt16()];
                            if ((flags & 1 << floatsRead) != 0 ? true : false) val *= -1;
                            mData[floatsRead] = Unpack(val, info.Offset, info.Scale);

                        }
                        break;
                    case CurveDataFormat.Packed:
                        for (int packedRead = 0; packedRead < Curve.GetPackedCount(DataType); packedRead++)
                        {
                            ulong packed = ReadPacked(s);
                            for (int packedIndex = 0; packedIndex < Curve.GetFloatCount(DataType) / Curve.GetPackedCount(DataType); packedIndex++)
                            {
                                int floatIndex = packedIndex + packedRead;
                                int bitsPerFloat = Curve.GetBitsPerFloat(DataType);
                                ulong maxPackedVal = (ulong)(Math.Pow(2, bitsPerFloat) - 1);
                                ulong mask = (maxPackedVal << (packedIndex * bitsPerFloat));
                                float val = ((packed & mask) >> (packedIndex * bitsPerFloat)) / (float)maxPackedVal;
                                if ((flags & 1 << floatIndex) != 0 ? true : false) val *= -1;
                                mData[floatIndex] = Unpack(val, info.Offset, info.Scale);
                            }
                        }
                        break;
                }

            }
            public static float Unpack(float packed, float offset, float scale)
            {
                return (packed * scale) + offset;
            }
            public static float Pack(float unpacked, float offset, float scale)
            {
                return (unpacked - offset) / scale;
            }
            public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mFrameIndex);
                var flags = mFlags << 4;
                UInt16[] indices = null;
                ulong[] packedVals = null;
                switch (info.Flags.Format)
                {
                    case CurveDataFormat.Indexed:
                        indices = new ushort[Curve.GetFloatCount(DataType)];
                        for (int i = 0; i < Curve.GetFloatCount(DataType); i++)
                        {
                            float packedIndex = Pack(mData[i], info.Offset, info.Scale);
                            if (packedIndex < 0) flags |= (1 << i);
                            packedIndex = Math.Abs(packedIndex);
                            if (!indexedFloats.Contains(packedIndex)) indexedFloats.Add(packedIndex);
                            indices[i] = (UInt16)indexedFloats.IndexOf(packedIndex);
                        }
                        break;
                    case CurveDataFormat.Packed:
                        packedVals = new ulong[Curve.GetPackedCount(DataType)];
                        for (int packedWritten = 0; packedWritten < packedVals.Length; packedWritten++)
                        {
                            ulong packed = 0;
                            for (int packedIndex = 0; packedIndex < Curve.GetFloatCount(DataType) / packedVals.Length; packedIndex++)
                            {
                                int floatIndex = packedWritten + packedIndex;
                                double maxPackedVal = Math.Pow(2, Curve.GetBitsPerFloat(DataType)) - 1;
                                float val = (mData[floatIndex] - info.Offset) / info.Scale;
                                if (val < 0) flags |= (1 << floatIndex);
                                val = Math.Abs(val);
                                packed |= (ulong)Math.Floor(val * maxPackedVal) << (packedIndex * Curve.GetBitsPerFloat(DataType));

                            }
                            packedVals[packedWritten] = packed;
                        }
                        break;
                }
                bw.Write((UInt16)flags);
                switch (info.Flags.Format)
                {
                    case CurveDataFormat.Indexed:
                        for (int i = 0; i < indices.Length; i++)
                            bw.Write(indices[i]);
                        break;
                    case CurveDataFormat.Packed:
                        for (int i = 0; i < packedVals.Length; i++)
                            WritePacked(s, packedVals[i]);
                        break;
                }
            }

            public virtual IEnumerable<float> GetFloatValues() { return mData; }
            public override AHandlerElement Clone(EventHandler handler) { return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this }); }
        }
        [ConstructorParameters(new object[] { CurveDataType.Float3 })]
        public class Float3Frame : Frame,
                                   IEquatable<Float3Frame>
        {
            public Float3Frame(int apiVersion, EventHandler handler, Float3Frame basis) : base(apiVersion, handler, basis) { }
            public Float3Frame(int apiVersion, EventHandler handler, CurveDataType type) : base(apiVersion, handler) { }

            public Float3Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) { }
            public override CurveDataType DataType
            {
                get { return CurveDataType.Float3; }
            }
            protected override ulong ReadPacked(Stream stream) { return new BinaryReader(stream).ReadUInt32(); }
            protected override void WritePacked(Stream stream, ulong packed) { new BinaryWriter(stream).Write((UInt32)packed); }

            #region IEquatable<Float3Frame> Members

            public bool Equals(Float3Frame other) { return base.Equals(other); }

            #endregion
        }
        [ConstructorParameters(new object[] { CurveDataType.Float4 })]
        public class Float4Frame : Frame,
                                   IEquatable<Float4Frame>
        {
            public Float4Frame(int apiVersion, EventHandler handler, Float4Frame basis) : base(apiVersion, handler, basis) { }
            public Float4Frame(int apiVersion, EventHandler handler,CurveDataType type) : base(apiVersion, handler) { }

            public Float4Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) { }

            public override CurveDataType DataType
            {
                get { return CurveDataType.Float4; }
            }
            protected override ulong ReadPacked(Stream stream) { return new BinaryReader(stream).ReadUInt16(); }
            protected override void WritePacked(Stream stream, ulong packed) { new BinaryWriter(stream).Write((UInt16)packed); }

            #region IEquatable<Float4Frame> Members

            public bool Equals(Float4Frame other) { return base.Equals(other); }

            #endregion
        }
        public class FrameList : DependentList<Frame>
        {
            private readonly CurveDataType mDataType;

            public FrameList(EventHandler handler, CurveDataType type) : base(handler) { mDataType = type; }

            public FrameList(EventHandler handler, CurveDataType type, IEnumerable<Frame> ilt) : base(handler, ilt) { mDataType = type; }

            public FrameList(EventHandler handler, CurveDataType type, Stream s, CurveDataInfo info, IList<float> floats)
                : base(handler)
            {
                mDataType = type;
                Parse(s, info, floats);
            }

            public CurveDataType DataType { get { return mDataType; } }

            private void Parse(Stream s, CurveDataInfo info, IList<float> floats)
            {
                for (int i = 0; i < info.FrameCount; i++)
                {
                    ((IList<Frame>)this).Add(CreateElement(s, info, floats));
                }
            }

            public void UnParse(Stream s, CurveDataInfo info, IList<float> floats)
            {
                info.FrameDataOffset = (uint)s.Position;
                info.FrameCount = Count;
                for (int i = 0; i < Count; i++)
                {
                    this[i].UnParse(s, info, floats);
                }
            }
            protected override Type GetElementType(params object[] fields)
            {
                if (fields.Length > 0)
                {
                    if (typeof (Frame).IsAssignableFrom(fields[0].GetType())) return fields[0].GetType();
                    if (typeof (CurveDataType).IsAssignableFrom(fields[0].GetType())) return Frame.GetFrameType(mDataType);
                }
                return base.GetElementType(fields);
            }
            public override void Add() { Add(new object[] {  mDataType }); }

            protected virtual Frame CreateElement(Stream s, CurveDataInfo info, IList<float> floats)
            {
                var ctor = Frame.GetFrameType(mDataType)
                    .GetConstructor(new[] {typeof (int), typeof (EventHandler), typeof (Stream), typeof (CurveDataInfo), typeof (IList<float>)});
                return (Frame)ctor.Invoke(new object[]{0, handler, s, info, floats});
            }

            protected virtual void WriteElement(Stream s, CurveDataInfo info, IList<float> floats, Frame element) { element.UnParse(s, info, floats); }

            #region Unused

            protected override Frame CreateElement(Stream s) { throw new NotSupportedException(); }

            protected override void WriteElement(Stream s, Frame element) { throw new NotSupportedException(); }

            #endregion
        }
        public struct CurveDataFlags
        {
            private Byte mRaw;
            public CurveDataFlags(byte raw) { mRaw = raw; }


            public CurveDataType Type
            {
                get { return (CurveDataType)((mRaw & 0x07) >> 0); }
                set
                {
                    mRaw &= 0x1F;
                    mRaw |= (byte)((byte)value << 0);
                }
            }

            public Boolean Static
            {
                get { return ((mRaw & 0x08) >> 3) == 1 ? true : false; }
                set
                {
                    mRaw &= 0xF7;
                    mRaw |= (byte)((value ? 1 : 0) << 3);
                }
            }

            public CurveDataFormat Format
            {
                get { return (CurveDataFormat)((mRaw & 0xF0) >> 4); }
                set
                {
                    mRaw &= 0x0F;
                    mRaw |= (byte)(((byte)value) << 4);
                }
            }

            public Byte Raw { get { return mRaw; } set { mRaw = value; } }
        }
        public enum CurveDataFormat : byte
        {
            Indexed = 0x00,
            Packed = 0x01
        }
        public class CurveDataInfo
        {
            public CurveDataFlags Flags;
            public Int32 FrameCount;
            public UInt32 FrameDataOffset;
            public Single Offset;
            public Single Scale;
            public UInt32 TrackKey;
            public CurveType Type;
        }
        public enum CurveDataType : byte
        {
            Float1 = 0x01,
            Float3 = 0x02,
            Float4 = 0x04
        }
        public enum CurveType
        {
            Position = 0x01,
            Orientation = 0x02
        }
    }
}