using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.Helpers.IO;
using s3piwrappers.Resources;
using s3piwrappers.SWB;

namespace s3piwrappers
{
    public class EffectResource : AResource
    {
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
                case 0x01:
                    return typeof (ParticleEffectSection);
                case 0x02:
                    return typeof (MetaparticleEffectSection);
                case 0x03:
                    return typeof (DecalEffectSection);
                case 0x04:
                    return typeof (SequenceEffectSection);
                case 0x05:
                    return typeof (SoundEffectSection);
                case 0x06:
                    return typeof (ShakeEffectSection);
                case 0x07:
                    return typeof (CameraEffectSection);
                case 0x08:
                    return typeof (ModelEffectSection);
                case 0x09:
                    return typeof (ScreenEffectSection);
                case 0x0b:
                    return typeof (GameEffectSection);
                case 0x0c:
                    return typeof (FastParticleEffectSection);
                case 0x0D:
                    return typeof (DistributeEffectSection);
                case 0x0E:
                    return typeof (RibbonEffectSection);
                case 0x0F:
                    return typeof (SpriteEffectSection);
                default:
                    throw new NotSupportedException("Effect Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }

            public void Add(int type, int version)
            {
                Add(GetSectionType((ushort) type));
            }

            public override void Add()
            {
                throw new NotImplementedException();
            }
        }


        public abstract class EffectSection : Section, IEquatable<EffectSection>
        {
            protected EffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            protected EffectSection(int apiVersion, EventHandler handler,ushort version)
                : base(apiVersion, handler,version)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
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
                return Type.Equals(other.Type);
            }
        }

        public abstract class EffectSection<T> : EffectSection
            where T : Effect, IEquatable<T>
        {
            protected EffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mItems = new SectionDataList<T>(handler, this);
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, EffectSection<T> basis)
                : base(apiVersion, handler, basis)
            {
            }


            protected EffectSection(int apiVersion, EventHandler handler, ushort version) : base(apiVersion, handler, version)
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


        public class ParticleEffectSection : EffectSection<ParticleEffect>
        {
            public ParticleEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler, ParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler,0x0004)
            {
            }

            public override ushort Type
            {
                get { return 0x0001; }
            }
        }

        public class MetaparticleEffectSection : EffectSection<MetaparticleEffect>
        {
            public MetaparticleEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler, MetaparticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler,0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0002; }
            }
        }

        public class DecalEffectSection : EffectSection<DecalEffect>
        {
            public DecalEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public DecalEffectSection(int apiVersion, EventHandler handler, DecalEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DecalEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler,0x0002)
            {
            }

            public override ushort Type
            {
                get { return 0x0003; }
            }
        }

        public class SequenceEffectSection : EffectSection<SequenceEffect>
        {
            public SequenceEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public SequenceEffectSection(int apiVersion, EventHandler handler, SequenceEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SequenceEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler,0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0004; }
            }
        }

        public class SoundEffectSection : EffectSection<SoundEffect>
        {
            public SoundEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public SoundEffectSection(int apiVersion, EventHandler handler, SoundEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SoundEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x005; }
            }
        }

        public class ShakeEffectSection : EffectSection<ShakeEffect>
        {
            public ShakeEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public ShakeEffectSection(int apiVersion, EventHandler handler, ShakeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ShakeEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0006; }
            }
        }

        public class CameraEffectSection : EffectSection<CameraEffect>
        {
            public CameraEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public CameraEffectSection(int apiVersion, EventHandler handler, CameraEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public CameraEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0007; }
            }
        }

        public class ModelEffectSection : EffectSection<ModelEffect>
        {
            public ModelEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public ModelEffectSection(int apiVersion, EventHandler handler, ModelEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ModelEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0008; }
            }
        }

        public class ScreenEffectSection : EffectSection<ScreenEffect>
        {
            public ScreenEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public ScreenEffectSection(int apiVersion, EventHandler handler, ScreenEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ScreenEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x0009; }
            }
        }

        public class GameEffectSection : EffectSection<DefaultEffect>
        {
            public GameEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public GameEffectSection(int apiVersion, EventHandler handler, GameEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public GameEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x000B; }
            }
        }

        public class FastParticleEffectSection : EffectSection<DefaultEffect>
        {
            public FastParticleEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public FastParticleEffectSection(int apiVersion, EventHandler handler, FastParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public FastParticleEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x000C; }
            }
        }

        public class DistributeEffectSection : EffectSection<DistributeEffect>
        {
            public DistributeEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public DistributeEffectSection(int apiVersion, EventHandler handler, DistributeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DistributeEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0001)
            {
            }

            public override ushort Type
            {
                get { return 0x000D; }
            }
        }


        /*
         * Contribution from ChaosMageX
         */

        public class RibbonEffectSection : EffectSection<RibbonEffect>
        {
            public RibbonEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public RibbonEffectSection(int apiVersion, EventHandler handler, RibbonEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public RibbonEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0002)
            {
            }

            public override ushort Type
            {
                get { return 0x000E; }
            }
        }

        /*
         * Contribution from ChaosMageX
         */

        public class SpriteEffectSection : EffectSection<SpriteEffect>
        {
            public SpriteEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public SpriteEffectSection(int apiVersion, EventHandler handler, SpriteEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SpriteEffectSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler, 0x0002)
            {
            }

            public override ushort Type
            {
                get { return 0x000F; }
            }
        }


        public abstract class ResourceSection : Section, IEquatable<ResourceSection>
        {
            protected ResourceSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
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
                return Type.Equals(other.Type);
            }
        }

        public abstract class ResourceSection<T> : ResourceSection
            where T : Resource, IEquatable<T>
        {
            protected ResourceSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mItems = new SectionDataList<T>(handler, this);
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
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

        public class MapSection : ResourceSection<Map>
        {
            public MapSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, MapSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public override ushort Type
            {
                get { return 0x0000; }
            }
        }

        public class MaterialSection : ResourceSection<Material>
        {
            public MaterialSection(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, MaterialSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public override ushort Type
            {
                get { return 0x0001; }
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
                    return typeof (MapSection);
                case 0x0001:
                    return typeof (MaterialSection);
                default:
                    throw new NotSupportedException("Resource Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }

            public void Add(int type, int version)
            {
                Add(GetSectionType((ushort) type));
            }

            public override void Add()
            {
                throw new NotImplementedException();
            }
        }


        public class VisualEffectSection : Section<VisualEffect>
        {
            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version)
                : base(apiVersion, handler, version)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version, Stream s)
                : base(apiVersion, handler, version, s)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, VisualEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public override ushort Type
            {
                get { return 0x0000; }
            }
        }

        public class VisualEffectHandleList : DependentList<VisualEffectHandle>
        {
            public VisualEffectHandleList(EventHandler handler) : base(handler)
            {
            }

            public VisualEffectHandleList(EventHandler handler, Stream s)
                : base(handler)
            {
                Parse(s);
            }

            protected override void Parse(Stream s)
            {
                var bw = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                uint index = bw.ReadUInt32();
                while (index != 0xFFFFFFFF)
                {
                    var item = new VisualEffectHandle(0, base.handler, s, index);
                    index = bw.ReadUInt32();
                    base.Add(item);
                }
            }

            public override void UnParse(Stream s)
            {
                var w = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                foreach (VisualEffectHandle item in this)
                {
                    VisualEffectHandle handle = item;
                    (item).UnParse(s);
                }
                w.Write(0xFFFFFFFF);
            }

            protected override VisualEffectHandle CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, VisualEffectHandle element)
            {
                throw new NotImplementedException();
            }

            public override void Add()
            {
                throw new NotImplementedException();
            }
        }


        private UInt16 mVersion = 0x00000002;
        private EffectSectionList mEffectSections;
        private ResourceSectionList mResourceSections;
        private VisualEffectSection mVisualEffectSections;
        private byte[] mReserved;
        private VisualEffectHandleList mVisualEffectHandles;


        [ElementPriority(1)]
        public UInt16 Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(2)]
        public EffectSectionList EffectSections
        {
            get { return mEffectSections; }
            set
            {
                mEffectSections = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(3)]
        public ResourceSectionList ResourceSections
        {
            get { return mResourceSections; }
            set
            {
                mResourceSections = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(4)]
        public VisualEffectSection VisualEffectSections
        {
            get { return mVisualEffectSections; }
            set
            {
                mVisualEffectSections = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(5)]
        public byte[] Reserved
        {
            get { return mReserved; }
            set
            {
                mReserved = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(6)]
        public VisualEffectHandleList VisualEffectHandles
        {
            get { return mVisualEffectHandles; }
            set
            {
                mVisualEffectHandles = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        public EffectResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            if (stream == null)
            {
                stream = UnParse();
                OnResourceChanged(this, new EventArgs());
            }
            stream.Position = 0;
            Parse(stream);
        }

        public void Parse(Stream inputStream)
        {
            var s = new BinaryStreamWrapper(inputStream, ByteOrder.BigEndian);
            mVersion = s.ReadUInt16();
            mEffectSections = new EffectSectionList(OnResourceChanged, inputStream);
            mResourceSections = new ResourceSectionList(OnResourceChanged, inputStream);
            mVisualEffectSections = new VisualEffectSection(0, OnResourceChanged, s.ReadUInt16(), inputStream);
            mReserved = s.ReadBytes(4);
            mVisualEffectHandles = new VisualEffectHandleList(OnResourceChanged, inputStream);
        }

        protected override Stream UnParse()
        {
            var outputStream = new MemoryStream();
            var s = new BinaryStreamWrapper(outputStream, ByteOrder.BigEndian);
            s.Write(mVersion);
            if (mEffectSections == null) mEffectSections = new EffectSectionList(OnResourceChanged);
            mEffectSections.UnParse(outputStream);
            if (mResourceSections == null) mResourceSections = new ResourceSectionList(OnResourceChanged);
            mResourceSections.UnParse(outputStream);
            if (mVisualEffectSections == null) mVisualEffectSections = new VisualEffectSection(0, OnResourceChanged, 2);
            s.Write(mVisualEffectSections.Version);
            mVisualEffectSections.UnParse(outputStream);
            if (mReserved == null) mReserved = new byte[] {0xFF, 0xFF, 0xFF, 0xFF};
            s.Write(mReserved);
            if (mVisualEffectHandles == null) mVisualEffectHandles = new VisualEffectHandleList(OnResourceChanged);
            mVisualEffectHandles.UnParse(outputStream);
            return outputStream;
        }

        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                for (int i = 0; i < mVisualEffectHandles.Count; i++)
                {
                    VisualEffectHandle e = mVisualEffectHandles[i];
                    sb.AppendFormat("[0x{2:X8}]:(0x{0:X8}):{1}\r\n", FNV32.GetHash(e.EffectName), e.EffectName, i);
                }
                return sb.ToString();
            }
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedAPIVersion; }
        }

        private const int kRecommendedAPIVersion = 1;
    }
}
