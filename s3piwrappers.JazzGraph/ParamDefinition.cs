using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class ParamDefinition : AChunkObject, IHasHashedName
    {
        public const uint ResourceType = 0x02EEDB46;
        public const string ResourceTag = "S_PD";

        public class NameComparer : IComparer<ParamDefinition>
        {
            public static readonly NameComparer Instance
                = new NameComparer();

            private StringComparer mComparer
                = StringComparer.OrdinalIgnoreCase;

            public int Compare(ParamDefinition x, ParamDefinition y)
            {
                return this.mComparer.Compare(x.mName, y.mName);
            }
        }

        private string mName;
        private uint mNameHash;
        private bool bNameIsHash;

        private string mDefaultValue;
        private uint mDefaultHash;
        private bool bDefaultIsHash;

        public ParamDefinition(string name)
            : base(ResourceType, ResourceTag)
        {
            this.mName = name;
            if (name == null)
            {
                this.mNameHash = 0;
                this.bNameIsHash = true;
            }
            else if (!name.StartsWith("0x") ||
                !uint.TryParse(name.Substring(2),
                    System.Globalization.NumberStyles.HexNumber,
                    null, out this.mNameHash))
            {
                this.mNameHash = FNVHash.HashString32(name);
                this.bNameIsHash = false;
            }
            else
            {
                this.bNameIsHash = true;
            }
            this.mDefaultValue = null;
            this.mDefaultHash = 0;
            this.bDefaultIsHash = true;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzParameterDefinition jpd 
                = new JazzParameterDefinition(0, null, s);
            jpd.NameHash = this.mNameHash;
            jpd.DefaultValue = this.mDefaultHash;
            if (!this.bNameIsHash &&
                nameMap != null && !nameMap.ContainsKey(this.mNameHash) &&
                (exportAllNames || !KeyNameReg.HasName(this.mNameHash)))
            {
                nameMap[this.mNameHash] = this.mName;
            }
            if (!this.bDefaultIsHash &&
                nameMap != null && !nameMap.ContainsKey(this.mDefaultHash) &&
                (exportAllNames || !KeyNameReg.HasName(this.mDefaultHash)))
            {
                nameMap[this.mDefaultHash] = this.mDefaultValue;
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jpd);
        }

        public string Name
        {
            get { return this.mName; }
            set
            {
                if (value == null)
                {
                    this.mName = null;
                    this.mNameHash = 0;
                    this.bNameIsHash = true;
                }
                else if (!value.Equals(this.mName))
                {
                    this.mName = value;
                    if (!value.StartsWith("0x") ||
                        !uint.TryParse(value.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out this.mNameHash))
                    {
                        this.mNameHash = FNVHash.HashString32(value);
                        this.bNameIsHash = false;
                    }
                    else
                    {
                        this.bNameIsHash = true;
                    }
                }
            }
        }

        public uint NameHash
        {
            get { return this.mNameHash; }
        }

        public bool NameIsHash
        {
            get { return this.bNameIsHash; }
        }

        public string DefaultValue
        {
            get { return this.mDefaultValue; }
            set
            {
                if (value == null)
                {
                    this.mDefaultValue = null;
                    this.mDefaultHash = 0;
                    this.bDefaultIsHash = true;
                }
                else if (!value.Equals(this.mDefaultValue))
                {
                    this.mDefaultValue = value;
                    if (!value.StartsWith("0x") ||
                        !uint.TryParse(value.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out this.mDefaultHash))
                    {
                        this.mDefaultHash = FNVHash.HashString32(value);
                        this.bDefaultIsHash = false;
                    }
                    else
                    {
                        this.bDefaultIsHash = true;
                    }
                }
            }
        }

        public uint DefaultHash
        {
            get { return this.mDefaultHash; }
        }

        public bool DefaultIsHash
        {
            get { return this.bDefaultIsHash; }
        }
    }
}
