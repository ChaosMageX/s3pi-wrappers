using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;

namespace s3piwrappers.EffectCloner.Swarm
{
    public class VisualEffectBuilder : VisualEffect, IComparable<VisualEffectBuilder>
    {
        #region Content Fields And Data
        protected string mEffectName;
        protected SectionDataList<IndexBuilder> mIndexBuilders;
        protected List<EffectBuilder> mEffectBuilders;

        [ElementPriority(0)]
        public string EffectName
        {
            get { return this.mEffectName; }
            set
            {
                if (this.mEffectName != value)
                {
                    this.OnEffectNameChanged(this.mEffectName, value);
                    this.mEffectName = value;
                }
            }
        }

        private void OnEffectNameChanged(string oldName, string newName)
        {
            this.dirty = true;
            if (this.handler != null)
                this.handler(this, new EffectNameChangedEventArgs(oldName, newName));
        }

        [ElementPriority(20)]
        public SectionDataList<IndexBuilder> Effects
        {
            get { return this.mIndexBuilders; }
            set { this.mIndexBuilders = value; }
        }

        public List<EffectBuilder> EffectBuilders
        {
            get { return this.mEffectBuilders; }
        }

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                fields.Remove("Items");
                fields.Remove("EffectBuilders");
                return fields;
            }
        }
        #endregion

        public VisualEffectBuilder(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            this.mEffectName = "<Insert_Effect_Name>";
            this.CastIndices();
        }

        public VisualEffectBuilder(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
            this.mEffectName = "<Insert_Effect_Name>";
            this.CastIndices();
        }

        public VisualEffectBuilder(VisualEffectHandle handle, VisualEffect basis, EffectResourceBuilder builder)
            : base(0, null, basis)
        {
            this.mEffectName = handle.EffectName;
            this.CastIndices();
            this.CreateEffectBuilders(builder);
        }

        public VisualEffectBuilder(VisualEffectHandle handle, VisualEffect basis, EffectResource resource)
            : base(0, null, basis)
        {
            this.mEffectName = handle.EffectName;
            this.CastIndices();
            this.CreateEffectBuilders(resource);
        }

        public VisualEffectBuilder(VisualEffectHandle handle, VisualEffect basis, 
            EffectResourceBuilder builder, EffectResource resource)
            : base(0, null, basis)
        {
            this.mEffectName = handle.EffectName;
            this.CastIndices();
            this.CreateEffectBuilders(builder, resource);
        }

        private void CastIndices()
        {
            // Cast Index to IndexBuilder
            MemoryStream s = new MemoryStream();
            base.Items.UnParse(s);
            s.Position = 0L;
            this.mIndexBuilders = new SectionDataList<IndexBuilder>(base.handler, base.mSection, s);
            //base.Items = null;
        }

        public override void UnParse(Stream stream)
        {
            MemoryStream s = new MemoryStream();
            this.mIndexBuilders.UnParse(s);
            s.Position = 0L;
            base.Items = new SectionDataList<Index>(base.handler, base.mSection, s);
            s.Position = 0L;
            base.UnParse(stream);
        }

        public void CreateEffectBuilders(EffectResourceBuilder resBuilder)
        {
            // Find and Clone Individual Effects
            IndexBuilder iBldr;
            EffectBuilder eBldr;
            VisualEffectType eType;
            int i, index, count = this.mIndexBuilders.Count;
            this.mEffectBuilders = new List<EffectBuilder>(count);
            for (i = 0; i < count; i++)
            {
                iBldr = this.mIndexBuilders[i] as IndexBuilder;
                eType = iBldr.EffectType;
                if (eType == (VisualEffectType)0)
                {
                    string header = "Effect: " + this.mEffectName + "; Index: " + i.ToString("X2")
                        + "; BlockIndex: " + iBldr.BlockIndex.ToString("X4");
                    System.Diagnostics.Debug.WriteLine(header + "; Unrecognized VisualEffectType (0); Assuming Particle (1)");
                    eType = VisualEffectType.Particle;
                }
                eBldr = new EffectBuilder(eType, iBldr.BlockIndex);
                index = this.mEffectBuilders.IndexOf(eBldr);
                if (index >= 0)
                    iBldr.SetEffectBuilder(this.mEffectBuilders[index]);
                else
                {
                    eBldr.Effect = resBuilder.GetEffect(eType, iBldr.BlockIndex);
                    this.mEffectBuilders.Add(eBldr);
                    iBldr.SetEffectBuilder(eBldr);
                }
            }
        }

        public void CreateEffectBuilders(EffectResourceBuilder resBuilder, EffectResource resource)
        {
            // Find and Clone Individual Effects
            IndexBuilder iBldr;
            EffectBuilder eBldr;
            VisualEffectType eType;
            s3piwrappers.Effects.Effect effect;
            int i, index, count = this.mIndexBuilders.Count;
            this.mEffectBuilders = new List<EffectBuilder>(count);
            for (i = 0; i < count; i++)
            {
                iBldr = this.mIndexBuilders[i] as IndexBuilder;
                eType = iBldr.EffectType;
                if (eType == (VisualEffectType)0)
                {
                    string header = "Effect: " + this.mEffectName + "; Index: " + i.ToString("X2")
                        + "; BlockIndex: " + iBldr.BlockIndex.ToString("X4");
                    System.Diagnostics.Debug.WriteLine(header + "; Unrecognized VisualEffectType (0); Assuming Particle (1)");
                    eType = VisualEffectType.Particle;
                }
                eBldr = new EffectBuilder(eType, iBldr.BlockIndex);
                index = this.mEffectBuilders.IndexOf(eBldr);
                if (index >= 0)
                    iBldr.SetEffectBuilder(this.mEffectBuilders[index]);
                else
                {
                    effect = EffectHelper.FindEffect(eType, iBldr.BlockIndex, resource);
                    if (effect != null)
                    {
                        EffectResource.EffectSection eSection = null;
                        if (resBuilder != null)
                            eSection = resBuilder.GetEffectSection(eType);
                        eBldr.Effect = EffectHelper.CloneSectionData(effect, null, eSection) 
                            as s3piwrappers.Effects.Effect;
                        this.mEffectBuilders.Add(eBldr);
                        iBldr.SetEffectBuilder(eBldr);
                    }
                }
            }
        }

        public void CreateEffectBuilders(EffectResource resource)
        {
            // Find and Clone Individual Effects
            IndexBuilder iBldr;
            EffectBuilder eBldr;
            VisualEffectType eType;
            int i, index, count = this.mIndexBuilders.Count;
            this.mEffectBuilders = new List<EffectBuilder>(count);
            for (i = 0; i < count; i++)
            {
                iBldr = this.mIndexBuilders[i] as IndexBuilder;
                eType = iBldr.EffectType;
                if (eType == (VisualEffectType)0)
                {
                    string header = "Effect: " + this.mEffectName + "; Index: " + i.ToString("X2")
                        + "; BlockIndex: " + iBldr.BlockIndex.ToString("X4");
                    System.Diagnostics.Debug.WriteLine(header + "; Unrecognized VisualEffectType (0); Assuming Particle (1)");
                    eType = VisualEffectType.Particle;
                }
                eBldr = new EffectBuilder(eType, iBldr.BlockIndex);
                index = this.mEffectBuilders.IndexOf(eBldr);
                if (index >= 0)
                    iBldr.SetEffectBuilder(this.mEffectBuilders[index]);
                else
                {
                    eBldr.Effect = EffectHelper.FindEffect(eType, iBldr.BlockIndex, resource);
                    this.mEffectBuilders.Add(eBldr);
                    iBldr.SetEffectBuilder(eBldr);
                }
            }
        }

        private void CloneEffectBuilders(List<EffectBuilder> eBuilders,
            EventHandler handler, EffectResourceBuilder dest)
        {
            int i, index, count = eBuilders.Count;
            this.mEffectBuilders = new List<EffectBuilder>(count);
            if (dest == null)
            {
                for (i = 0; i < count; i++)
                {
                    this.mEffectBuilders.Add(eBuilders[i].Clone(handler));
                }
            }
            else
            {
                EffectResource.EffectSection eSection;
                for (i = 0; i < count; i++)
                {
                    eSection = dest.GetEffectSection(eBuilders[i].EffectType);
                    this.mEffectBuilders.Add(eBuilders[i].Clone(handler, eSection));
                }
            }
            IndexBuilder iBuilder;
            EffectBuilder eBuilder;
            count = this.mIndexBuilders.Count;
            for (i = 0; i < count; i++)
            {
                iBuilder = this.mIndexBuilders[i] as IndexBuilder;
                eBuilder = new EffectBuilder(iBuilder.EffectType, iBuilder.BlockIndex);
                index = this.mEffectBuilders.IndexOf(eBuilder);
                iBuilder.SetEffectBuilder(this.mEffectBuilders[index]);
            }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return Clone(this.mEffectName, handler, null, false);
        }

        public VisualEffectBuilder Clone(string name, EventHandler handler)
        {
            return Clone(name, handler, null, true);
        }

        public VisualEffectBuilder Clone(EventHandler handler, EffectResourceBuilder dest)
        {
            return Clone(this.mEffectName, handler, dest, false);
        }

        public VisualEffectBuilder Clone(string name, EventHandler handler,
            EffectResourceBuilder dest)
        {
            return Clone(name, handler, dest, true);
        }

        private VisualEffectBuilder Clone(string name, EventHandler handler,
            EffectResourceBuilder dest, bool safe)
        {
            MemoryStream s = new MemoryStream();
            this.UnParse(s);
            s.Position = 0L;
            VisualEffectBuilder builder;
            if (dest == null)
                builder = new VisualEffectBuilder(0, handler, base.mSection, s);
            else
                builder = new VisualEffectBuilder(0, handler, dest.VisualEffects, s);
            //builder.mEffectName = new VisualEffectHandle(0, handler, this.mEffectName);
            builder.mEffectName = safe ? EffectHelper.CreateSafeEffectName(name, true) : name;
            builder.CloneEffectBuilders(this.mEffectBuilders, handler, dest);

            return builder;
        }

        /// <summary>
        /// This is mainly used for alphabetically sorting the Visual Effect Handle
        /// dictionary when the Effect Resource is compiled.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(VisualEffectBuilder other)
        {
            return this.mEffectName.CompareTo(other.mEffectName);
        }
    }
}
