using System;
using System.Collections.Generic;
using s3piwrappers.Effects;
using s3piwrappers.SWB;

namespace s3piwrappers.EffectCloner.Swarm
{
    public class EffectBuilder : IEquatable<EffectBuilder>
    {
        public VisualEffectType EffectType;
        protected int mEffectIndex;
        public Effect Effect;
        protected List<IndexBuilder> mIndices = new List<IndexBuilder>();

        public EffectBuilder(VisualEffectType eType, int eIndex)
        {
            this.EffectType = eType;
            this.mEffectIndex = eIndex;
        }

        public EffectBuilder(VisualEffectType eType, int eIndex, Effect effect)
        {
            this.EffectType = eType;
            this.mEffectIndex = eIndex;
            this.Effect = effect;
        }

        public void AddIndex(IndexBuilder index)
        {
            if (this.mIndices.IndexOf(index) < 0)
                this.mIndices.Add(index);
        }

        public bool RemoveIndex(IndexBuilder index)
        {
            return this.mIndices.Remove(index);
        }

        public int EffectIndex
        {
            get { return this.mEffectIndex; }
            set
            {
                if (this.mEffectIndex != value)
                {
                    this.mEffectIndex = value;
                    int count = this.mIndices.Count;
                    for (int i = 0; i < count; i++)
                        this.mIndices[i].ComponentIndex = value;
                }
            }
        }

        public EffectBuilder Clone(EventHandler handler)
        {
            return this.Clone(handler, this.Effect.Section);
        }

        public EffectBuilder Clone(EventHandler handler, ISection section)
        {
            EffectBuilder builder = new EffectBuilder(this.EffectType, this.mEffectIndex);
            builder.Effect = EffectHelper.CloneSectionData(this.Effect, handler, section) as Effect;
            return builder;
        }

        public bool Equals(EffectBuilder other)
        {
            return this.EffectType == other.EffectType && this.mEffectIndex == other.mEffectIndex;
        }
    }
}
