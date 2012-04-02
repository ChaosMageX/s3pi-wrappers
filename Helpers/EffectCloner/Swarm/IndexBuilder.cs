using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.SWB;

namespace s3piwrappers.EffectCloner.Swarm
{
    public class IndexBuilder : VisualEffect.Index
    {
        protected EffectBuilder mEffectBuilder;

        public IndexBuilder(int APIversion, EventHandler handler, ISection section)
            : base(APIversion, handler, section) { }

        public IndexBuilder(int APIversion, EventHandler handler, SectionData basis)
            : base(APIversion, handler, basis) { }

        public IndexBuilder(int APIversion, EventHandler handler, ISection section, Stream s)
            : base(APIversion, handler, section, s) { }

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                fields.Remove("BlockType");
                fields.Remove("BlockIndex");
                return fields;
            }
        }

        [ElementPriority(1)]
        public VisualEffectType EffectType
        {
            get { return (VisualEffectType)base.BlockType; }
            set
            {
                /*byte blockType = (byte)value;
                if (base.BlockType != blockType)
                {
                    base.BlockType = blockType;
                }/**/
            }
        }

        [ElementPriority(20)]
        public Effect EffectData
        {
            get
            {
                if (this.mEffectBuilder != null)
                    return this.mEffectBuilder.Effect;
                else
                    return null;
            }
            set
            {
                if (this.mEffectBuilder != null)
                    this.mEffectBuilder.Effect = value;
            }
        }

        public void SetEffectBuilder(EffectResourceBuilder source)
        {
            if (this.mEffectBuilder != null)
            {
                this.mEffectBuilder.RemoveIndex(this);
            }
            this.mEffectBuilder = new EffectBuilder(this.EffectType, this.BlockIndex);
            this.mEffectBuilder.Effect = source.GetEffect(this.EffectType, this.BlockIndex);
            this.mEffectBuilder.AddIndex(this);
        }

        public void SetEffectBuilder(EffectBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (this.mEffectBuilder != null)
            {
                this.mEffectBuilder.RemoveIndex(this);
            }
            this.mEffectBuilder = builder;
            this.mEffectBuilder.AddIndex(this);
            this.BlockType = (byte)this.mEffectBuilder.EffectType;
            this.BlockIndex = this.mEffectBuilder.EffectIndex;
        }
    }
}
