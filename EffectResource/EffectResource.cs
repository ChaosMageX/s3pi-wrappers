using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.Resources;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;
using System.Collections;

namespace s3piwrappers
{
    public class EffectResource : AResource
    {
        #region Effect Section

        #region EffectSectionList
        public class EffectSectionList : SectionList<EffectSection>
        {
            public EffectSectionList(EventHandler handler)
                : base(handler)
            {
            }

            public EffectSectionList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            protected override Type GetSectionType(ushort id)
            {
                switch (id)
                {
                    case 0x01: return typeof(ParticleEffectSection);
                    case 0x02: return typeof(MetaparticleEffectSection);
                    case 0x03: return typeof(DecalEffectSection);
                    case 0x04: return typeof(SequenceEffectSection);
                    case 0x05: return typeof(SoundEffectSection);
                    case 0x06: return typeof(ShakeEffectSection);
                    case 0x07: return typeof(CameraEffectSection);
                    case 0x08: return typeof(ModelEffectSection);
                    case 0x09: return typeof(ScreenEffectSection);
                    case 0x0b: return typeof(GameEffectSection);
                    case 0x0c: return typeof(FastParticleEffectSection);
                    case 0x0D: return typeof(DistributeEffectSection);
                    case 0x0E: return typeof(RibbonEffectSection);
                    default: throw new NotSupportedException("Effect Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }
        }
        #endregion

        #region EffectSection
        public abstract class EffectSection : Section, IEquatable<EffectSection>
        {
            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, EffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            protected abstract override void Parse(Stream s);

            public abstract override void UnParse(Stream s);


            public bool Equals(EffectSection other)
            {
                return mType.Equals(other.mType);
            }
        }
        public abstract class EffectSection<T> : EffectSection
            where T : Effect, IEquatable<T>
        {
            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
                mItems = new SectionDataList<T>(handler, this);
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, EffectSection<T> basis)
                : base(apiVersion, handler, basis)
            {
            }


            protected override void Parse(Stream s)
            {
                mItems = new SectionDataList<T>(handler, this, s);
            }

            public override void UnParse(Stream s)
            {
                mItems.UnParse(s);
            }
        }

        #endregion

        [ConstructorParameters(new object[] { 0x0001, 0x0003 })]
        public class ParticleEffectSection : EffectSection<ParticleEffect>
        {
            public ParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler, ParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0002, 0x0001 })]
        public class MetaparticleEffectSection : EffectSection<MetaparticleEffect>
        {
            public MetaparticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler, MetaparticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0003, 0x0002 })]
        public class DecalEffectSection : EffectSection<DecalEffect>
        {
            public DecalEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public DecalEffectSection(int apiVersion, EventHandler handler, DecalEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DecalEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0004, 0x0001 })]
        public class SequenceEffectSection : EffectSection<SequenceEffect>
        {
            public SequenceEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public SequenceEffectSection(int apiVersion, EventHandler handler, SequenceEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SequenceEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0005, 0x0001 })]
        public class SoundEffectSection : EffectSection<SoundEffect>
        {
            public SoundEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public SoundEffectSection(int apiVersion, EventHandler handler, SoundEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SoundEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0006, 0x0001 })]
        public class ShakeEffectSection : EffectSection<ShakeEffect>
        {
            public ShakeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ShakeEffectSection(int apiVersion, EventHandler handler, ShakeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ShakeEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0007, 0x0001 })]
        public class CameraEffectSection : EffectSection<CameraEffect>
        {
            public CameraEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public CameraEffectSection(int apiVersion, EventHandler handler, CameraEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public CameraEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0008, 0x0001 })]
        public class ModelEffectSection : EffectSection<ModelEffect>
        {
            public ModelEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ModelEffectSection(int apiVersion, EventHandler handler, ModelEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ModelEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0009, 0x0001 })]
        public class ScreenEffectSection : EffectSection<ScreenEffect>
        {
            public ScreenEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ScreenEffectSection(int apiVersion, EventHandler handler, ScreenEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ScreenEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000B, 0x0001 })]
        public class GameEffectSection : EffectSection<DefaultEffect>
        {
            public GameEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public GameEffectSection(int apiVersion, EventHandler handler, GameEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public GameEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000C, 0x0001 })]
        public class FastParticleEffectSection : EffectSection<DefaultEffect>
        {
            public FastParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public FastParticleEffectSection(int apiVersion, EventHandler handler, FastParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public FastParticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000D, 0x0001 })]
        public class DistributeEffectSection : EffectSection<DistributeEffect>
        {
            public DistributeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public DistributeEffectSection(int apiVersion, EventHandler handler, DistributeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DistributeEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000E, 0x0001 })]
        public class RibbonEffectSection : EffectSection<DefaultEffect>
        {
            public RibbonEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public RibbonEffectSection(int apiVersion, EventHandler handler, RibbonEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public RibbonEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        #endregion

        #region Resource Section
        public abstract class ResourceSection : Section, IEquatable<ResourceSection>
        {
            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ResourceSection basis)
                : base(apiVersion, handler, basis)
            {
            }
            protected abstract override void Parse(Stream s);

            public abstract override void UnParse(Stream s);


            public bool Equals(ResourceSection other)
            {
                return mType.Equals(other.mType);
            }
        }
        public abstract class ResourceSection<T> : ResourceSection
            where T : Resource, IEquatable<T>
        {
            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
                mItems = new SectionDataList<T>(handler,this);
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ResourceSection<T> basis)
                : base(apiVersion, handler, basis)
            {
            }

            protected override void Parse(Stream s)
            {
                mItems = new SectionDataList<T>(handler, this, s);
            }

            public override void UnParse(Stream s)
            {
                mItems.UnParse(s);
            }

        }
        [ConstructorParameters(new object[] { 0x0000, 0x0000 })]
        public class MapSection : ResourceSection<Map>
        {
            public MapSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, MapSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0001, 0x0000 })]
        public class MaterialSection : ResourceSection<Material>
        {
            public MaterialSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, MaterialSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }
        public class ResourceSectionList : SectionList<ResourceSection>
        {
            public ResourceSectionList(EventHandler handler)
                : base(handler)
            {
            }

            public ResourceSectionList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            protected override Type GetSectionType(ushort id)
            {
                switch (id)
                {
                    case 0x000:
                        return typeof(MapSection);
                    case 0x0001:
                        return typeof(MaterialSection);
                    default: throw new NotSupportedException("Resource Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }
        }
        #endregion

        #region VisualEffect Section
        public class VisualEffectSection : Section<VisualEffect>
        {
            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version)
                : base(apiVersion, handler, 0, version)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version, Stream s)
                : base(apiVersion, handler, 0, version, s)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, VisualEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }

        public class VisualEffectHandleList : AHandlerList<VisualEffectHandle>, IGenericAdd
        {

            public VisualEffectHandleList(EventHandler handler) : base(handler) { }
            public VisualEffectHandleList(EventHandler handler, Stream s)
                : base(handler)
            {
                Parse(s);
            }
            protected void Parse(Stream s)
            {
                BinaryStreamWrapper bw = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                uint index = bw.ReadUInt32();
                while (index != 0xFFFFFFFF)
                {
                    VisualEffectHandle item = new VisualEffectHandle(0, base.handler, s, index);
                    index = bw.ReadUInt32();
                    base.Add(item);
                }
            }

            public void UnParse(Stream s)
            {
                BinaryStreamWrapper w = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                foreach (var item in this)
                {
                    VisualEffectHandle handle = (VisualEffectHandle)item;
                    ((VisualEffectHandle)item).UnParse(s);
                }
                w.Write((uint)0xFFFFFFFF);
            }

            public bool Add(params object[] fields)
            {
                if (fields == null)
                {
                    return false;
                }
                var args = new object[2 + fields.Length];
                args[0] = 0;
                args[1] = handler;
                Array.Copy(fields,0,args,2,fields.Length);
                base.Add((VisualEffectHandle)Activator.CreateInstance(typeof(VisualEffectHandle),args));
                return true;
            }

            public void Add()
            {
                Add(new object[]{});
            }
        }
        #endregion

        #region Fields
        private UInt16 mVersion = 0x00000002;
        private EffectSectionList mEffects;
        private ResourceSectionList mResources;
        private VisualEffectSection mVisualEffects;
        private byte[] mReserved;
        private VisualEffectHandleList mVisualEffectHandles;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public UInt16 Version
        {
            get { return mVersion; }
            set { mVersion = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public EffectSectionList Effects
        {
            get { return mEffects; }
            set { mEffects = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public ResourceSectionList Resources
        {
            get { return mResources; }
            set { mResources = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public VisualEffectSection VisualEffects
        {
            get { return mVisualEffects; }
            set { mVisualEffects = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public byte[] Reserved
        {
            get { return mReserved; }
            set { mReserved = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public VisualEffectHandleList VisualEffectHandles
        {
            get { return mVisualEffectHandles; }
            set { mVisualEffectHandles = value; OnResourceChanged(this, new EventArgs()); }
        }
        #endregion

        #region Constructors
        public EffectResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }
        #endregion

        public void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mVersion = s.ReadUInt16();
            mEffects = new EffectSectionList(this.OnResourceChanged, stream);
            mResources = new ResourceSectionList(this.OnResourceChanged, stream);
            mVisualEffects = new VisualEffectSection(0, this.OnResourceChanged, s.ReadUInt16(), stream);
            mReserved = s.ReadBytes(4);
            mVisualEffectHandles = new VisualEffectHandleList(this.OnResourceChanged, stream);
        }

        protected override Stream UnParse()
        {
            MemoryStream stream = new MemoryStream();
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mVersion);
            if (mEffects == null) mEffects = new EffectSectionList(this.OnResourceChanged);
            mEffects.UnParse(stream);
            if (this.mResources == null) this.mResources = new ResourceSectionList(this.OnResourceChanged);
            mResources.UnParse(stream);
            if (mVisualEffects == null) mVisualEffects = new VisualEffectSection(0, this.OnResourceChanged, 2);
            s.Write(mVisualEffects.Version);
            mVisualEffects.UnParse(stream);
            if (mReserved == null) mReserved = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            s.Write(mReserved);
            if (mVisualEffectHandles == null) mVisualEffectHandles = new VisualEffectHandleList(this.OnResourceChanged);
            mVisualEffectHandles.UnParse(stream);
            return stream;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedAPIVersion; }
        }
        const int kRecommendedAPIVersion = 1;
    }
}
