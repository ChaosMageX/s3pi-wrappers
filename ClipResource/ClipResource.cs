using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3piwrappers.Geometry;
namespace s3piwrappers
{
    public enum ClipEventType
    {
        None,
        Parent,
        UnParent,
        Sound,
        Script,
        Effect,
        Visibility,
        Unk7,
        Unk8,
        DestroyProp,
        StopEffect

    }
    /// <summary>
    /// Wrapper for the animation resource
    /// </summary>
    /// <remarks>Incomplete; not S3PE-friendly at all yet, no saving</remarks>
    public class ClipResource : AResource
    {
        #region ActorSlot
        public class ActorSlotEntryList : List<ActorSlotEntry>
        {
            public ActorSlotEntryList(Stream s)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                UInt32 padding = br.ReadUInt32(); //0x7e7e7e7e
                uint count = br.ReadUInt32();
                long startOffset = s.Position;
                long currentPos = startOffset;
                for (int i = 0; i < count; i++)
                {
                    long elementOffset = br.ReadUInt32() + startOffset;
                    currentPos = s.Position;
                    s.Seek(elementOffset, SeekOrigin.Begin);
                    ActorSlotEntry entry = new ActorSlotEntry(s);
                    Add(entry);
                    s.Seek(currentPos, SeekOrigin.Begin);
                }
            }
        }
        public class ActorSlotEntry
        {
            private UInt32 mIndex;
            private string mActorName;
            private string mSlotName;
            public ActorSlotEntry(Stream s)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIndex = br.ReadUInt32();
                mActorName = br.ReadZString();
                s.Seek(511 - mActorName.Length, SeekOrigin.Current);
                mSlotName = br.ReadZString();
            }
            public override string ToString()
            {
                return String.Format("{0:X8}:{1},{2}",mIndex,mActorName,mSlotName);
            }
        }
        public class ActorSlotTable : List<ActorSlotEntryList>
        {
            public ActorSlotTable(Stream s)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                uint count = br.ReadUInt32();
                long startOffset = s.Position;
                long currentPos = startOffset;
                for (int i = 0; i < count; i++)
                {
                    long elementOffset = br.ReadUInt32() + startOffset;
                    currentPos = s.Position;
                    s.Seek(elementOffset, SeekOrigin.Begin);
                    ActorSlotEntryList list = new ActorSlotEntryList(s);
                    Add(list);
                    s.Seek(currentPos, SeekOrigin.Begin);
                }
            }

        } 
        #endregion

        #region Events
        public class EventTable
        {
            private UInt32 mVersion;
            private List<Event> mEvents;
            public EventTable(Stream s)
            {
                mEvents = new List<Event>();
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "=CE=") 
                    throw new Exception(String.Format("Bad ClipEvent header: Expected \"=CE=\", but got {0}", magic));
                mVersion = br.ReadUInt32();
                uint count = br.ReadUInt32();
                long endOffset = br.ReadUInt32();
                long startOffset = br.ReadUInt32();
                for (uint i = 0; i < count; i++)
                {
                    ClipEventType type = (ClipEventType)br.ReadUInt16();
                    Event e = Event.CreateInstance(type, s);
                    mEvents.Add(e);
                }
            }

        }
        public abstract class Event
        {
            protected Event(ClipEventType type, Stream s)
            {
                mType = type;
                if (s != null) Parse(s);
            }
            private ClipEventType mType;
            private UInt16 mShort01;
            private UInt32 mId;
            private Single mTimecode;
            private Single mFloat01;
            private Single mFloat02;
            private UInt32 mInt01;
            private String mEventName;
            public static Event CreateInstance(ClipEventType type, Stream s)
            {
                switch (type)
                {
                    case ClipEventType.Parent: return new ParentEvent(type,s);
                    case ClipEventType.DestroyProp: return new DestroyPropEvent(type, s);
                    case ClipEventType.Effect:return new EffectEvent(type,s);
                    case ClipEventType.Sound: return new SoundEvent(type, s);
                    case ClipEventType.Script: return new ScriptEvent(type, s);
                    case ClipEventType.Visibility: return new VisibilityEvent(type, s);
                    case ClipEventType.StopEffect: return new StopEffectEvent(type, s);
                    case ClipEventType.UnParent: return new UnparentEvent(type, s);
                    default: throw new NotImplementedException(String.Format("Event type: {0} not implemented",type));
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
                mEventName = br.ReadZString();
                while ((s.Position % 4) != 0) br.ReadByte(); //padding to next DWORD
            }
            public virtual void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
            }
            public override string ToString()
            {
                return mType.ToString();
            }
        }
        public class ParentEvent : Event
        {
            internal ParentEvent(ClipEventType type, Stream s) : base(type, s) { }

            private UInt32 mActorNameHash;
            private UInt32 mObjectNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown01;
            private Single[] mMatrix4x4;
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
        }
        public class UnparentEvent : Event
        {
            internal UnparentEvent(ClipEventType type, Stream s) : base(type, s) { }
            private UInt32 mObjectNameHash;
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mObjectNameHash = br.ReadUInt32();
            }
        }
        public class SoundEvent : Event
        {
            internal SoundEvent(ClipEventType type, Stream s) : base(type, s) { }
            private String mSoundName;
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mSoundName = br.ReadZString();
                s.Seek(127 - mSoundName.Length, SeekOrigin.Current);
            }
        }
        public class ScriptEvent : Event
        {
            internal ScriptEvent(ClipEventType type, Stream s) : base(type, s) { }
        }
        public class EffectEvent : Event
        {
            internal EffectEvent(ClipEventType type, Stream s) : base(type, s) { }
            private UInt32 mUnknown01;
            private UInt32 mUnknown02;
            private UInt32 mEffectNameHash;
            private UInt32 mActorNameHash;
            private UInt32 mSlotNameHash;
            private UInt32 mUnknown03;
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

        }
        public class VisibilityEvent : Event
        {
            internal VisibilityEvent(ClipEventType type, Stream s) : base(type, s) { }
            private Single mVisibility;
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mVisibility = br.ReadSingle();
            }
        }
        public class DestroyPropEvent : Event
        {
            internal DestroyPropEvent(ClipEventType type, Stream s) : base(type, s) { }
            private UInt32 mPropNameHash;
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mPropNameHash = br.ReadUInt32();
            }
        }
        public class StopEffectEvent : Event
        {
            private UInt32 mEffectNameHash;
            private UInt32 mUnknown02;
            internal StopEffectEvent(ClipEventType type, Stream s) : base(type, s) { }
            protected override void Parse(Stream s)
            {
                base.Parse(s);
                BinaryReader br = new BinaryReader(s);
                mEffectNameHash = br.ReadUInt32();
                mUnknown02 = br.ReadUInt32();
            }

        }
        #endregion

        #region Constructors
        public ClipResource(int apiVersion, Stream s):base(apiVersion,s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            Parse(s);
        }
        #endregion

        #region Fields
        private S3Clip mClip;
        private ActorSlotTable mActorSlotTable;
        private EventTable mEventTable;
        private string mActorName;
        private UInt32 mUnknown01;
        private UInt32 mUnknown02;
        private Quaternion mQuat; 
        #endregion

        #region I/O
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            UInt32 typeId = br.ReadUInt32();
            if (typeId != 0x6B20C4F3) throw new Exception("Not a valid CLIP resource");
            long linkedClipOffset = br.ReadUInt32() + s.Position - 4;
            long clipSize = br.ReadUInt32();
            long clipOffset = br.ReadUInt32() + s.Position - 4;
            long slotOffset = br.ReadUInt32() + s.Position - 4;
            long actorOffset = br.ReadUInt32() + s.Position - 4;
            long eventOffset = br.ReadUInt32() + s.Position - 4;
            mUnknown01 = br.ReadUInt32();
            mUnknown02 = br.ReadUInt32();
            long endOffset = br.ReadUInt32() + s.Position - 4;
            s.Seek(actorOffset, SeekOrigin.Begin);
            mActorName = br.ReadZString();


            s.Seek(endOffset, SeekOrigin.Begin);
            Single x = br.ReadSingle();
            Single y = br.ReadSingle();
            Single z = br.ReadSingle();
            Single w = br.ReadSingle();
            mQuat = new Quaternion(x, y, z, w);

            //s.Seek(clipOffset, SeekOrigin.Begin);
            //Stream clipStream = new MemoryStream(br.ReadBytes((int)clipSize));
            //clipStream.Position = 0L;
            //mClip = new S3Clip(clipStream);

            s.Seek(slotOffset, SeekOrigin.Begin);
            mActorSlotTable = new ActorSlotTable(s);
            s.Seek(eventOffset, SeekOrigin.Begin);
            mEventTable = new EventTable(s);
        }
        protected override Stream UnParse()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region s3pi
        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }
        static bool checking = s3pi.Settings.Settings.Checking;
        const int kRecommendedApiVersion = 1;
        #endregion

    }
}
