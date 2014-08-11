using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3piwrappers.Effects;
using s3piwrappers.SWB;

namespace s3piwrappers.EffectCloner.Swarm
{
    public static class EffectHelper
    {
        public static string CreateSafeEffectName(string handle, bool toLower)
        {
            string result = toLower ? handle.ToLowerInvariant() : handle;
            result = result.Replace(' ', '_');
            return result;
        }

        public static SectionData CloneSectionData(SectionData data, EventHandler handler)
        {
            return CloneSectionData(data, handler, data.Section);
        }

        public static SectionData CloneSectionData(SectionData data, EventHandler handler, ISection section)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            data.UnParse(s);
            s.Position = 0L;
            return (SectionData)Activator.CreateInstance(data.GetType(),
                data.RequestedApiVersion, handler, section, s);
        }

        public class StringHandleComparer : IComparer<VisualEffectName>
        {
            public string Value;
            public StringHandleComparer(string value)
            {
                this.Value = value;
            }
            public int Compare(VisualEffectName x, VisualEffectName y)
            {
                if (x == null)
                    return (y == null) ? 0 : this.Value.CompareTo(y.EffectName);
                if (y == null)
                    return (x == null) ? 0 : x.EffectName.CompareTo(this.Value);
                return 0;
            }
        }

        public static int BinarySearchForEffectHandle(string name, EffectResource resource)
        {
            //VisualEffectHandle target = new VisualEffectHandle(0, null);
            //target.EffectName = CreateSafeEffectName(name);
            return resource.VisualEffectNames.BinarySearch(null, new StringHandleComparer(name));
        }

        public static VisualEffectName FindVisualEffectHandle(string name, EffectResource resource)
        {
            string effectName = CreateSafeEffectName(name, true);
            int index = BinarySearchForEffectHandle(effectName, resource);
            if (index < 0)
            {
                //throw new Exception("Unable to find effect handle named " + effectName);
                return null;
            }
            return resource.VisualEffectNames[index];
        }

        public static VisualEffect FindVisualEffect(string name, EffectResource resource)
        {
            VisualEffectName handle = FindVisualEffectHandle(name, resource);
            if (handle == null)
                return null;
            return resource.VisualEffectSections.Items[(int)handle.Index] as VisualEffect;
        }

        public static VisualEffect FindVisualEffect(VisualEffectName handle, EffectResource resource)
        {
            return resource.VisualEffectSections.Items[(int)handle.Index] as VisualEffect;
        }

        public static EffectResource.EffectSection FindEffectSection(VisualEffectType eType, EffectResource resource)
        {
            int effectSectionCount = resource.EffectSections.Count;
            EffectResource.EffectSection eSection;
            for (int i = 0; i < effectSectionCount; i++)
            {
                eSection = resource.EffectSections[i];
                if (eSection.Type == (ushort)eType)
                    return eSection;
            }
            return null;
        }

        public static Effect FindEffect(VisualEffectType eType, int eIndex, EffectResource resource)
        {
            EffectResource.EffectSection eSection = FindEffectSection(eType, resource);
            if (eSection == null)
                return null;
            return eSection.Items[eIndex] as Effect;
        }

        public static VisualEffectType GetEffectType<E>() where E : Effect
        {
            Type effectType = typeof(E);
            if (effectType == typeof(ParticleEffect))
                return VisualEffectType.Particle;
            if (effectType == typeof(MetaparticleEffect))
                return VisualEffectType.Metaparticle;
            if (effectType == typeof(DecalEffect))
                return VisualEffectType.Decal;
            if (effectType == typeof(SequenceEffect))
                return VisualEffectType.Sequence;
            if (effectType == typeof(SoundEffect))
                return VisualEffectType.Sound;
            if (effectType == typeof(ShakeEffect))
                return VisualEffectType.Shake;
            if (effectType == typeof(CameraEffect))
                return VisualEffectType.Camera;
            if (effectType == typeof(ModelEffect))
                return VisualEffectType.Model;
            if (effectType == typeof(ScreenEffect))
                return VisualEffectType.Screen;
            /*if (effectType == typeof(GameEffect))
                return VisualEffectType.Game;
            if (effectType == typeof(FastParticleEffect))
                return VisualEffectType.FastParticle;/**/
            if (effectType == typeof(DistributeEffect))
                return VisualEffectType.Distribute;
            if (effectType == typeof(RibbonEffect))
                return VisualEffectType.Ribbon;
            if (effectType == typeof(SpriteEffect))
                return VisualEffectType.Sprite;
            return (VisualEffectType)0;
        }

        public static E[] FindEffects<E>(VisualEffectName handle, 
            EffectResource resource) where E : Effect
        {
            VisualEffectType eType = GetEffectType<E>();
            byte bType = (byte)eType;
            List<E> effects = new List<E>();
            EffectResource.EffectSection effectSection = FindEffectSection(eType, resource);
            if (effectSection == null)
                return null;
            VisualEffect visualEffect = FindVisualEffect(handle, resource);
            int indexCount = visualEffect.Descriptions.Count;
            VisualEffect.Description veIndex;
            for (int i = 0; i < indexCount; i++)
            {
                veIndex = visualEffect.Descriptions[i] as VisualEffect.Description;
                if (veIndex.ComponentType == bType)
                {
                    effects.Add(effectSection.Items[veIndex.ComponentIndex] as E);
                }
            }
            return effects.ToArray();
        }

        public static string[] GetSurfaceStrings(VisualEffectName handle, EffectResource resource)
        {
            byte pEffectType = (byte)VisualEffectType.Particle;
            byte mEffectType = (byte)VisualEffectType.Metaparticle;
            byte dEffectType = (byte)VisualEffectType.Distribute;
            EffectResource.EffectSection pEffects = FindEffectSection(VisualEffectType.Particle, resource);
            EffectResource.EffectSection mEffects = FindEffectSection(VisualEffectType.Metaparticle, resource);
            EffectResource.EffectSection dEffects = FindEffectSection(VisualEffectType.Distribute, resource);

            List<string> surfaceStrings = new List<string>();
            DataList<Effect.Surface> surfaces;
            VisualEffect visualEffect = FindVisualEffect(handle, resource);
            VisualEffect.Description index;
            Effect.Surface surface;
            int i, j, count, indexCount = visualEffect.Descriptions.Count;
            for (i = 0; i < indexCount; i++)
            {
                index = visualEffect.Descriptions[i] as VisualEffect.Description;
                if (index.ComponentType == pEffectType)
                {
                    ParticleEffect particle = pEffects.Items[index.ComponentIndex] as ParticleEffect;
                    surfaces = particle.Surfaces;
                    count = surfaces.Count;
                    for (j = 0; j < count; j++)
                    {
                        surface = surfaces[j];
                        if (!string.IsNullOrEmpty(surface.String01))
                            surfaceStrings.Add(string.Format("{0:X4}(P).Surfaces[{1}].String01:{2}", i, j, surface.String01));
                        if (!string.IsNullOrEmpty(surface.String02))
                            surfaceStrings.Add(string.Format("{0:X4}(P).Surfaces[{1}].String02:{2}", i, j, surface.String02));
                    }
                }
                if (index.ComponentType == mEffectType)
                {
                    MetaparticleEffect metaparticle = mEffects.Items[index.ComponentIndex] as MetaparticleEffect;
                    surfaces = metaparticle.Surfaces;
                    count = surfaces.Count;
                    for (j = 0; j < count; j++)
                    {
                        surface = metaparticle.Surfaces[j];
                        if (!string.IsNullOrEmpty(surface.String01))
                            surfaceStrings.Add(string.Format("{0:X4}(M).Surfaces[{1}].String01:{2}", i, j, surface.String01));
                        if (!string.IsNullOrEmpty(surface.String02))
                            surfaceStrings.Add(string.Format("{0:X4}(M).Surfaces[{1}].String02:{2}", i, j, surface.String02));
                    }
                }
                if (index.ComponentType == dEffectType)
                {
                    DistributeEffect distribute = dEffects.Items[index.ComponentIndex] as DistributeEffect;
                    surfaces = distribute.Surfaces;
                    count = surfaces.Count;
                    for (j = 0; j < count; j++)
                    {
                        surface = surfaces[j];
                        if (!string.IsNullOrEmpty(surface.String01))
                            surfaceStrings.Add(string.Format("{0:X4}(D).Surfaces[{1}].String01:{2}", i, j, surface.String01));
                        if (!string.IsNullOrEmpty(surface.String02))
                            surfaceStrings.Add(string.Format("{0:X4}(D).Surfaces[{1}].String02:{2}", i, j, surface.String02));
                    }
                }
            }
            return surfaceStrings.ToArray();
        }

        public static void SetAllEffectReferences(string oldEffectName, string newEffectName, 
            EffectResource resource)
        {
            oldEffectName = CreateSafeEffectName(oldEffectName, true);
            newEffectName = CreateSafeEffectName(newEffectName, false);
            int i;
            EffectResource.EffectSection metaSection = FindEffectSection(VisualEffectType.Metaparticle, resource);
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
            EffectResource.EffectSection seqSection = FindEffectSection(VisualEffectType.Sequence, resource);
            if (seqSection != null)
            {
                SequenceEffect seqEffect;
                int seqCount = seqSection.Items.Count;
                for (i = 0; i < seqCount; i++)
                {
                    seqEffect = seqSection.Items[i] as SequenceEffect;
                    for (int j = 0; j < seqEffect.Elements.Count; j++)
                    {
                        if (seqEffect.Elements[j].EffectName.ToLowerInvariant() == oldEffectName)
                            seqEffect.Elements[j].EffectName = newEffectName;
                    }
                }
            }
        }

        public static string[] GetEffectNameList(EffectResource resource)
        {
            EffectResource.VisualEffectNameList handleList = resource.VisualEffectNames;
            int count = handleList.Count;
            string[] nameList = new string[count];
            for (int i = 0; i < count; i++)
                nameList[i] = handleList[i].EffectName;
            return nameList;
        }

        public static void WriteEffectNameList(EffectResource resource, System.IO.TextWriter writer)
        {
            EffectResource.VisualEffectNameList handleList = resource.VisualEffectNames;
            int count = handleList.Count;
            for (int i = 0; i < count; i++)
            {
                writer.WriteLine(handleList[i].EffectName);
            }
        }

        private class FixedCollection<T> : ICollection<T>
        {
            private T[] values;

            public FixedCollection(int size)
            {
                this.values = new T[size];
            }

            public FixedCollection(T[] values)
            {
                this.values = values;
            }

            public void Add(T item)
            {
                throw new InvalidOperationException();
            }

            public void Clear()
            {
                Array.Clear(this.values, 0, this.values.Length);
            }

            public bool Contains(T item)
            {
                return this.values.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                this.values.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return this.values.Length; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(T item)
            {
                throw new InvalidOperationException();
            }

            public class Enumerator : IEnumerator<T>
            {
                private int index = 0;
                private FixedCollection<T> parent;

                public Enumerator(FixedCollection<T> parent)
                {
                    this.parent = parent;
                }

                public T Current
                {
                    get { return this.parent.values[this.index]; }
                }

                public void Dispose()
                {
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return this.parent.values[this.index]; }
                }

                public bool MoveNext()
                {
                    return (this.index++) >= this.parent.values.Length;
                }

                public void Reset()
                {
                    this.index = 0;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(this);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.values.GetEnumerator();
            }
        }

        private class FixedDictionary<K, V> : IDictionary<K, V>
            where K : IEquatable<K>
        {
            private K[] keys;
            private V[] values;

            public FixedDictionary(int size)
            {
                this.keys = new K[size];
                this.values = new V[size];
            }

            public int IndexOfKey(K key)
            {
                int index = -1;
                int count = this.keys.Length;
                for (int i = 0; i < count; i++)
                {
                    if (this.keys[i].Equals(key))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }

            public void SetValue(int index, K key, V value)
            {
                this.keys[index] = key;
                this.values[index] = value;
            }

            public void Add(K key, V value)
            {
                throw new InvalidOperationException();
            }

            public bool ContainsKey(K key)
            {
                return this.IndexOfKey(key) >= 0;
            }

            public ICollection<K> Keys
            {
                get { return new FixedCollection<K>(this.keys); }
            }

            public bool Remove(K key)
            {
                throw new InvalidOperationException();
            }

            public bool TryGetValue(K key, out V value)
            {
                int index = IndexOfKey(key);
                if (index >= 0)
                {
                    value = this.values[index];
                    return true;
                }
                value = default(V);
                return false;
            }

            public ICollection<V> Values
            {
                get { return new FixedCollection<V>(this.values); }
            }

            public V this[K key]
            {
                get
                {
                    int index = IndexOfKey(key);
                    if (index >= 0)
                        return this.values[index];
                    return default(V);
                }
                set
                {
                    int index = IndexOfKey(key);
                    if (index >= 0)
                        this.values[index] = value;
                }
            }

            public void Add(KeyValuePair<K, V> item)
            {
                throw new InvalidOperationException();
            }

            public void Clear()
            {
                Array.Clear(this.keys, 0, this.keys.Length);
                Array.Clear(this.values, 0, this.values.Length);
            }

            public bool Contains(KeyValuePair<K, V> item)
            {
                throw new InvalidOperationException();
            }

            public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
            {
                int size = this.keys.Length;
                for (int i = 0; i < size; i++)
                {
                    array[i + arrayIndex] = new KeyValuePair<K, V>(this.keys[i], this.values[i]);
                }
            }

            public int Count
            {
                get { return this.keys.Length; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(KeyValuePair<K, V> item)
            {
                throw new InvalidOperationException();
            }

            public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            {
                throw new InvalidOperationException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new InvalidOperationException();
            }
        }

        public static SortedList<string, uint> GetEffectHandleList(EffectResource resource)
        {
            EffectResource.VisualEffectNameList handleList = resource.VisualEffectNames;
            int count = handleList.Count;
            FixedDictionary<string, uint> fd = new FixedDictionary<string, uint>(count);
            for (int i = 0; i < count; i++)
                fd.SetValue(i, handleList[i].EffectName, handleList[i].Index);
            return new SortedList<string, uint>(fd);
        }
    }
}
