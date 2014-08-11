using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.Resources;
using s3piwrappers.SWB;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.EffectCloner.Swarm
{
    public class EffectResourceBuilder : AResource
    {
        private class DefaultHandleComparer : IComparer<VisualEffectName>
        {
            public int Compare(VisualEffectName x, VisualEffectName y)
            {
                return x.CompareTo(y);
            }
        }
        private static readonly DefaultHandleComparer sDefaultComparer = new DefaultHandleComparer();

        /*public class VisualEffectSection : Section<VisualEffectBuilder>
        {
            public VisualEffectSection(int apiVersion, EventHandler handler, VisualEffectSection basis)
                : base(apiVersion, handler, basis) { }

            public VisualEffectSection(int apiVersion, EventHandler handler, ushort version)
                : base(apiVersion, handler, 0, version) { }

            public VisualEffectSection(int apiVersion, EventHandler handler, ushort version, Stream s)
                : base(apiVersion, handler, 0, version, s) { }
        }/**/

        public class VisualEffectHandleList : AHandlerList<VisualEffectName>,
            s3pi.Interfaces.IGenericAdd
        {
            private class StringHandleComparer : IComparer<VisualEffectName>
            {
                private string mValue;
                public StringHandleComparer(string value) { this.mValue = value; }
                public int Compare(VisualEffectName x, VisualEffectName y)
                {
                    if (x == null)
                        return (y == null) ? 0 : this.mValue.CompareTo(y.EffectName);
                    if (y == null)
                        return (x == null) ? 0 : x.EffectName.CompareTo(this.mValue);
                    return x.CompareTo(y);
                }
            }

            // Methods
            public VisualEffectHandleList(EventHandler handler)
                : base(handler, -1L) { }

            public VisualEffectHandleList(EventHandler handler, Stream s)
                : base(handler, -1L)
            {
                this.Parse(s);
            }

            protected void Parse(Stream s)
            {
                BinaryStreamWrapper wrapper = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                uint index = wrapper.ReadUInt32();
                while (index != uint.MaxValue)
                {
                    VisualEffectName item = new VisualEffectName(0, base.handler, s, index);
                    index = wrapper.ReadUInt32();
                    base.Add(item);
                }
            }

            public void UnParse(Stream s)
            {
                BinaryStreamWrapper wrapper = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                int count = base.Count;
                for (int i = 0; i < count; i++)
                {
                    base[i].UnParse(s);
                }
                wrapper.Write(uint.MaxValue);
            }

            public void Add()
            {
                this.Add(new object[0]);
            }

            public override void Add(VisualEffectName item)
            {
                this.Add(item, true);
            }

            protected void Add(VisualEffectName item, bool sorted)
            {
                int num = base.BinarySearch(0, this.Count, item, sDefaultComparer);
                if (num >= 0)
                {
                    throw new ArgumentException("Adding Duplicate");
                    //ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
                }
                if (sorted)
                    base.Insert(~num, item);
                else
                    base.Add(item);
            }

            public bool Add(params object[] fields)
            {
                if (fields == null)
                {
                    return false;
                }
                object[] destinationArray = new object[2 + fields.Length];
                destinationArray[0] = 0;
                destinationArray[1] = base.handler;
                Array.Copy(fields, 0, destinationArray, 2, fields.Length);
                this.Add((VisualEffectName)Activator.CreateInstance(typeof(VisualEffectName), destinationArray), true);
                return true;
            }

            void System.Collections.IList.Clear()
            {
                this.Clear();
            }

            void System.Collections.IList.RemoveAt(int num1)
            {
                this.RemoveAt(num1);
            }

            public override void Sort()
            {
                base.Sort(0, base.Count, sDefaultComparer);
            }

            public int BinarySearch(string key)
            {
                return base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
            }

            public int ClosestIndexOf(string key)
            {
                int num = base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
                return (num < 0) ? ~num : num;
            }

            public bool QuickReplace(string oldName, string newName)
            {
                int oldIndex = base.BinarySearch(0, base.Count, null, new StringHandleComparer(oldName));
                if (oldIndex < 0)
                    return false;
                int newIndex = base.BinarySearch(0, base.Count, null, new StringHandleComparer(newName));
                if (newIndex >= 0)
                    return false;
                EventHandler handler = base.handler;
                base.handler = null;
                newIndex = ~newIndex;
                if (newIndex == oldIndex)
                {
                    base[newIndex].EffectName = newName;
                }
                else if (newIndex > oldIndex)
                {
                    VisualEffectName oldHandle = base[oldIndex];
                    oldHandle.EffectName = newName;
                    for (int i = oldIndex; i < newIndex; i++)
                        base[i] = base[i + 1];
                    base[newIndex] = oldHandle;
                }
                else
                {
                    VisualEffectName newHandle = base[newIndex];
                    newHandle.EffectName = newName;
                    for (int j = newIndex; j < oldIndex; j++)
                        base[j] = base[j + 1];
                    base[oldIndex] = newHandle;
                }
                base.handler = handler;
                base.OnListChanged();
                return true;
            }

            public bool ContainsKey(string key)
            {
                return base.BinarySearch(0, this.Count, null, new StringHandleComparer(key)) >= 0;
            }

            public bool Remove(string key)
            {
                int num = base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
                if (num >= 0)
                {
                    this.RemoveAt(num);
                    return true;
                }
                return false;
            }

            public bool TryGetValue(string key, out uint value)
            {
                int num = base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
                if (num >= 0)
                {
                    value = base[num].Index;
                    return true;
                }
                value = uint.MaxValue;
                return false;
            }

            public uint this[string key]
            {
                get
                {
                    int num = base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
                    if (num < 0)
                        throw new KeyNotFoundException();
                    return base[num].Index;
                }
                set
                {
                    int num = base.BinarySearch(0, base.Count, null, new StringHandleComparer(key));
                    if (num >= 0)
                    {
                        base[num].Index = value;
                    }
                    else
                    {
                        MemoryStream s = new MemoryStream();
                        BinaryStreamWrapper wrapper = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                        wrapper.WriteString(key, StringType.ZeroDelimited);
                        s.Position = 0L;
                        base.Insert(~num, new VisualEffectName(0, base.handler, s, value));
                    }
                }
            }

            public void CopyNamesTo(string[] array, int arrayIndex)
            {
                int count = base.Count;
                if (array == null)
                    throw new ArgumentNullException("array");
                if ((arrayIndex < 0) || (arrayIndex > array.Length))
                    throw new ArgumentOutOfRangeException("arrayIndex");
                if ((array.Length - arrayIndex) < count)
                {
                    throw new ArgumentException("Array Plus Offset Too Small");
                }
                for (int i = 0; i < count; i++)
                {
                    array[i + arrayIndex] = base[i].EffectName;
                }
            }

            public void CopyIndicesTo(uint[] array, int arrayIndex)
            {
                int count = base.Count;
                if (array == null)
                    throw new ArgumentNullException("array");
                if ((arrayIndex < 0) || (arrayIndex > array.Length))
                    throw new ArgumentOutOfRangeException("arrayIndex");
                if ((array.Length - arrayIndex) < count)
                {
                    throw new ArgumentException("Array Plus Offset Too Small");
                }
                for (int i = 0; i < count; i++)
                {
                    array[i + arrayIndex] = base[i].Index;
                }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public void Add(Type instanceType)
            {
                throw new NotImplementedException();
            }
        }

        #region Constant Values
        private const int sEffectSectionCount = 14;

        private static readonly VisualEffectType[] sEffectTypeList = new VisualEffectType[]
        {
            VisualEffectType.Particle, VisualEffectType.Metaparticle, VisualEffectType.Decal,
            VisualEffectType.Sequence, VisualEffectType.Sound,        VisualEffectType.Shake,
            VisualEffectType.Camera,   VisualEffectType.Model,        VisualEffectType.Screen,
            VisualEffectType.Game,     VisualEffectType.FastParticle, VisualEffectType.Distribute,
            VisualEffectType.Ribbon,   VisualEffectType.Sprite
        };

        private static readonly int[] sEffectVersionList = new int[] 
        { 
            4, 1, 2,
            1, 1, 1,
            1, 1, 1,
            1, 1, 1,
            2, 2
        };

        private const int sResourceSectionCount = 2;

        private static readonly VisualResourceType[] sResourceTypeList = new VisualResourceType[]
        {
            VisualResourceType.Map, VisualResourceType.Material
        };

        private static readonly int[] sResourceVersionList = new int[] { 0, 0 };
        #endregion

        #region Content Fields
        private ushort mVersion = 2;
        private EffectResource.EffectSectionList mEffectSectionList;
        private int[] mEffectSectionIndices;
        private EffectResource.ResourceSectionList mResourceSectionList;
        private int[] mResourceSectionIndices;
        //private ushort mVisualEffectVersion = 2;
        //private SectionDataList<VisualEffectBuilder> mVisualEffectBuilders;
        private EffectResource.VisualEffectSection mVisualEffectSection;
        private byte[] mReserved;
        private VisualEffectHandleList mVisualEffectHandleList;

        [ElementPriority(1)]
        public ushort Version
        {
            get { return this.mVersion; }
            set { this.mVersion = value; this.OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(4)]
        public EffectResource.VisualEffectSection VisualEffects
        {
            get { return this.mVisualEffectSection; }
            set { this.mVisualEffectSection = value; this.OnResourceChanged(this, new EventArgs()); }
        }
        #endregion

        #region Constructors and Parsing
        public EffectResourceBuilder()
            : base(0, null)
        {
            int i;
            // Initialize Effect Sections
            this.mEffectSectionList = new EffectResource.EffectSectionList(this.OnResourceChanged);
            for (i = 0; i < sEffectSectionCount; i++)
                this.mEffectSectionList.Add((int)sEffectTypeList[i], sEffectVersionList[i]);
            this.mEffectSectionIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            // Initialize Resource Sections
            this.mResourceSectionList = new EffectResource.ResourceSectionList(this.OnResourceChanged);
            for (i = 0; i < sResourceSectionCount; i++)
                this.mResourceSectionList.Add((int)sResourceTypeList[i], sResourceVersionList[i]);
            this.mResourceSectionIndices = new int[] { 0, 1 };
            // Initialize Visual Effect Builders
            this.mVisualEffectSection = new EffectResource.VisualEffectSection(0, this.OnResourceChanged, 2);
            //this.mVisualEffectBuilders = new SectionDataList<VisualEffectBuilder>(base.OnResourceChanged, null);
            // Initialize Reserved
            this.mReserved = new byte[] { 0xff, 0xff, 0xff, 0xff };
            // Initialize Handle List
            this.mVisualEffectHandleList = new VisualEffectHandleList(this.OnResourceChanged);
        }

        public EffectResourceBuilder(EffectResourceBuilder original, bool cloneEffects)
            : base(0, null)
        {
            int i, count = original.mEffectSectionList.Count;
            // Initialize Effect Sections
            this.mEffectSectionList = new EffectResource.EffectSectionList(this.OnResourceChanged);
            EffectResource.EffectSection eSection;
            for (i = 0; i < count; i++)
            {
                eSection = original.mEffectSectionList[i];
                this.mEffectSectionList.Add((int)eSection.Type, (int)eSection.Version);
            }
            this.InitEffectSectionIndices();
            // Initialize Resource Sections
            count = original.mResourceSectionList.Count;
            this.mResourceSectionList = new EffectResource.ResourceSectionList(this.OnResourceChanged);
            EffectResource.ResourceSection rSection;
            for (i = 0; i < count; i++)
            {
                rSection = original.mResourceSectionList[i];
                this.mResourceSectionList.Add((int)rSection.Type, (int)rSection.Version);
            }
            this.InitResourceSectionIndices();
            // Initialize Visual Effect Builders
            this.mVisualEffectSection = new EffectResource.VisualEffectSection(0, this.OnResourceChanged, 2);
            //this.mVisualEffectBuilders = new SectionDataList<VisualEffectBuilder>(this.OnResourceChanged, null);
            // Initialize Reserved
            this.mReserved = new byte[] { 0xff, 0xff, 0xff, 0xff };
            // Initialize Handle List
            this.mVisualEffectHandleList = new VisualEffectHandleList(this.OnResourceChanged);
        }

        public EffectResourceBuilder(Stream s)
            : base(0, s)
        {
            this.Parse(s);
        }

        protected void Parse(Stream s)
        {
            BinaryStreamWrapper wrapper = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
            this.mVersion = wrapper.ReadUInt16();
            // Initialize Effect Sections
            this.mEffectSectionList = new EffectResource.EffectSectionList(this.OnResourceChanged, s);
            this.InitEffectSectionIndices();
            // Initialize Resource Sections
            this.mResourceSectionList = new EffectResource.ResourceSectionList(this.OnResourceChanged, s);
            this.InitResourceSectionIndices();
            // Initialize Visual Effect Builders
            this.mVisualEffectSection = new EffectResource.VisualEffectSection(0, this.OnResourceChanged, wrapper.ReadUInt16());
            //this.mVisualEffectVersion = wrapper.ReadUInt16();
            //this.mVisualEffectBuilders = new SectionDataList<VisualEffectBuilder>(this.OnResourceChanged, null, s);
            // Initialize Reserved
            this.mReserved = wrapper.ReadBytes(4);
            // Initialize Handle List
            this.mVisualEffectHandleList = new VisualEffectHandleList(this.OnResourceChanged, s);
            // Initialize Visual Effect Builder Properties
            this.InitEffectNames();
            this.InitEffectBuilders();
        }

        private void InitEffectSectionIndices()
        {
            int i, j, count = this.mEffectSectionList.Count;
            VisualEffectType eType;
            this.mEffectSectionIndices = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            for (i = 0; i < count; i++)
            {
                eType = (VisualEffectType)this.mEffectSectionList[i].Type;
                for (j = 0; j < sEffectSectionCount; j++)
                    if (eType == sEffectTypeList[j])
                        this.mEffectSectionIndices[j] = i;
            }
            if (count < sEffectSectionCount)
            {
                for (i = 0; i < sEffectSectionCount; i++)
                {
                    if (this.mEffectSectionIndices[i] == -1)
                    {
                        this.mEffectSectionList.Add((int)sEffectTypeList[i], sEffectVersionList[i]);
                        this.mEffectSectionIndices[i] = this.mEffectSectionList.Count - 1;
                    }
                }
            }
        }

        private void InitResourceSectionIndices()
        {
            int i, j, count = this.mResourceSectionList.Count;
            VisualResourceType rType;
            this.mResourceSectionIndices = new int[] { -1, -1 };
            for (i = 0; i < count; i++)
            {
                rType = (VisualResourceType)this.mResourceSectionList[i].Type;
                for (j = 0; j < sResourceSectionCount; j++)
                    if (rType == sResourceTypeList[j])
                        this.mResourceSectionIndices[j] = i;
            }
            if (count < sResourceSectionCount)
            {
                for (i = 0; i < sResourceSectionCount; i++)
                {
                    if (this.mResourceSectionIndices[i] == -1)
                    {
                        this.mResourceSectionList.Add((int)sResourceTypeList[i], sResourceVersionList[i]);
                        this.mResourceSectionIndices[i] = this.mResourceSectionList.Count - 1;
                    }
                }
            }
        }

        private void InitEffectNames()
        {
            VisualEffectBuilder builder;
            foreach (VisualEffectName handle in this.mVisualEffectHandleList)
            {
                builder = this.mVisualEffectSection.Items[(int)handle.Index] as VisualEffectBuilder;
                //builder = this.mVisualEffectBuilders[(int)handle.Index] as VisualEffectBuilder;
                if (builder != null)
                {
                    builder.EffectName = handle.EffectName;
                }
            }
        }

        private void InitEffectBuilders()
        {
            int count = this.mVisualEffectSection.Items.Count;
            //int count = this.mVisualEffectBuilders.Count;
            VisualEffectBuilder builder;
            for (int i = 0; i < count; i++)
            {
                builder = this.mVisualEffectSection.Items[i] as VisualEffectBuilder;
                //builder = this.mVisualEffectBuilders[i] as VisualEffectBuilder;
                if (builder != null)
                {
                    builder.CreateEffectBuilders(this);
                }
            }
        }
        #endregion

        #region Effects and Resources
        public EffectResource.EffectSection GetEffectSection(VisualEffectType eType)
        {
            int index = -1;
            for (int i = 0; i < sEffectSectionCount; i++)
            {
                if (eType == sEffectTypeList[i])
                {
                    index = i;
                    break;
                }
            }
            return this.mEffectSectionList[this.mEffectSectionIndices[index]];
        }

        public Effect GetEffect(VisualEffectType eType, int eIndex)
        {
            EffectResource.EffectSection section = GetEffectSection(eType);
            return section.Items[eIndex] as Effect;
        }

        public void SetAllEffectReferences(string oldEffectName, string newEffectName)
        {
            oldEffectName = EffectHelper.CreateSafeEffectName(oldEffectName, true);
            newEffectName = EffectHelper.CreateSafeEffectName(newEffectName, false);
            int i;
            EffectResource.EffectSection metaSection = GetEffectSection(VisualEffectType.Metaparticle);
            if (metaSection != null)
            {
                MetaparticleEffect metaEffect;
                int metaCount = metaSection.Items.Count;
                for (i = 0; i < metaCount; i++)
                {
                    metaEffect = metaSection.Items[i] as MetaparticleEffect;
                    if (metaEffect.ComponentName.ToLowerInvariant() == oldEffectName)
                        metaEffect.ComponentName = newEffectName;
                    if (metaEffect.ComponentType.ToLowerInvariant() == oldEffectName)
                        metaEffect.ComponentType = newEffectName;
                }
            }
            EffectResource.EffectSection seqSection = GetEffectSection(VisualEffectType.Sequence);
            if (seqSection != null)
            {
                SequenceEffect seqEffect;
                int seqCount = seqSection.Items.Count;
                for (i = 0; i < seqCount; i++)
                {
                    seqEffect = seqSection.Items[i] as SequenceEffect;
                    for (int j = 0; j < seqEffect.Elements.Count; j++)
                    {
                        if (seqEffect.Elements[j].EffectName == oldEffectName)
                            seqEffect.Elements[j].EffectName = newEffectName;
                    }
                }
            }
        }

        public EffectResource.ResourceSection GetResourceSection(VisualResourceType rType)
        {
            int index = -1;
            for (int i = 0; i < sResourceSectionCount; i++)
            {
                if (rType == sResourceTypeList[i])
                {
                    index = i;
                    break;
                }
            }
            return this.mResourceSectionList[this.mResourceSectionIndices[index]];
        }

        public Resource GetResource(VisualResourceType rType, uint rIndex)
        {
            EffectResource.ResourceSection section = GetResourceSection(rType);
            return section.Items[(int)rIndex] as Resource;
        }
        #endregion

        #region Cloning and Merging
        public void Merge(EffectResourceBuilder builder)
        {
            throw new NotImplementedException();
        }

        public static void CloneVisualEffect(string name,
            EffectResourceBuilder source, EffectResourceBuilder dest)
        {
            name = EffectHelper.CreateSafeEffectName(name, true);
            int index = source.mVisualEffectHandleList.BinarySearch(name);
            if (index >= 0)
            {
                CloneVisualEffect(source.mVisualEffectHandleList[index], source, dest);
            }
        }

        public static void CloneVisualEffect(VisualEffectName handle,
            EffectResourceBuilder source, EffectResourceBuilder dest)
        {
            VisualEffectBuilder builder = source.mVisualEffectSection.Items[(int)handle.Index] as VisualEffectBuilder;
            dest.mVisualEffectSection.Items.Add(item: builder.Clone(dest.OnResourceChanged, dest));
        }

        public void AddVisualEffect(VisualEffectName handle, EffectResource resource)
        {
            VisualEffect visualEffect = EffectHelper.FindVisualEffect(handle, resource);
            this.mVisualEffectSection.Items.Add(item: new VisualEffectBuilder(handle, visualEffect, resource));
        }
        #endregion

        #region UnParsing
        private void FlushEffectBuilders()
        {
            EffectResource.EffectSection eSection;
            EffectBuilder eBuilder;
            List<EffectBuilder> eBuilders;
            int i, j, eCount, veCount = this.mVisualEffectSection.Items.Count/*this.mVisualEffectBuilders.Count/**/;
            for (i = 0; i < veCount; i++)
            {
                eBuilders = (this.mVisualEffectSection.Items[i] as VisualEffectBuilder).EffectBuilders;
                //eBuilders = (this.mVisualEffectBuilders[i] as VisualEffectBuilder).EffectBuilders;
                eCount = eBuilders.Count;
                for (j = 0; j < eCount; j++)
                {
                    eBuilder = eBuilders[j];
                    // Check if the Effect Builder already exists
                    eSection = GetEffectSection(eBuilder.EffectType);
                    if (eBuilder.EffectIndex <= eSection.Items.Count
                        && eBuilder.Effect.Equals(eSection.Items[(int)eBuilder.EffectIndex]))
                        continue;
                    // Effect doesn't exist; flush it to the section
                    eSection.Items.Add(item: EffectHelper.CloneSectionData(eBuilder.Effect, null, eSection));
                    eBuilder.EffectIndex = eSection.Items.Count - 1;
                }
            }
        }

        private void FlushEffectNames()
        {
            VisualEffectBuilder builder;
            VisualEffectName handle;
            string effectName;
            int num, count = this.mVisualEffectSection.Items.Count/*this.mVisualEffectBuilders.Count/**/;
            this.mVisualEffectHandleList = new VisualEffectHandleList(base.OnResourceChanged);
            for (int i = 0; i < count; i++)
            {
                builder = this.mVisualEffectSection.Items[i] as VisualEffectBuilder;
                //builder = this.mVisualEffectBuilders[i] as VisualEffectBuilder;
                effectName = EffectHelper.CreateSafeEffectName(builder.EffectName, true);
                num = this.mVisualEffectHandleList.BinarySearch(effectName);
                if (num < 0)
                {
                    handle = new VisualEffectName(0, base.OnResourceChanged);
                    handle.EffectName = effectName;
                    handle.Index = (uint)i;
                    this.mVisualEffectHandleList.Insert(~num, handle);
                }
            }
        }

        public void FlushEffects()
        {
            this.FlushEffectBuilders();
            this.FlushEffectNames();
        }

        protected override Stream UnParse()
        {
            this.FlushEffectBuilders();
            this.FlushEffectNames();
            MemoryStream s = new MemoryStream();
            this.UnParse(s);
            return s;
        }

        public void UnParse(Stream stream)
        {
            BinaryStreamWrapper wrapper = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            wrapper.Write(this.mVersion);
            // Write Effect Sections
            this.mEffectSectionList.UnParse(stream);
            // Write Resource Sections
            this.mResourceSectionList.UnParse(stream);
            // Write Visual Effects
            wrapper.Write(this.mVisualEffectSection.Version);
            this.mVisualEffectSection.UnParse(stream);
            //wrapper.Write(this.mVisualEffectVersion);
            //this.mVisualEffectBuilders.UnParse(stream);
            // Write Reserved
            wrapper.Write(this.mReserved);
            // Write Handle List
            this.mVisualEffectHandleList.UnParse(stream);
        }
        #endregion

        public override int RecommendedApiVersion
        {
            get { return 0; }
        }
    }
}
