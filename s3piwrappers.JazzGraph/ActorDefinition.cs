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
    public class ActorDefinition : AChunkObject
    {
        public const uint ResourceType = 0x02EEDB2F;
        public const string ResourceTag = "S_AD";

        public class NameComparer : IComparer<ActorDefinition>
        {
            public static readonly NameComparer Instance
                = new NameComparer();

            private StringComparer mComparer
                = StringComparer.OrdinalIgnoreCase;

            public int Compare(ActorDefinition x, ActorDefinition y)
            {
                return this.mComparer.Compare(x.mName, y.mName);
            }
        }

        private string mName;
        private uint mNameHash;
        private bool bNameIsHash;

        public ActorDefinition(string name)
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
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzActorDefinition jad = new JazzActorDefinition(0, null, s);
            jad.NameHash = this.mNameHash;
            if (!this.bNameIsHash &&
                nameMap != null && !nameMap.ContainsKey(this.mNameHash) && 
                (exportAllNames || !KeyNameReg.HasName(this.mNameHash)))
            {
                nameMap[this.mNameHash] = this.mName;
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jad);
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
    }
}
