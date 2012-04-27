using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceStblHandle : IResourceStblHandle
    {
        private readonly string name;
        private readonly ulong originalKey;
        private readonly string originalHashKey;
        private ulong key;
        private string hashKey;
        private string[] locale = new string[0x17];

        public string Name
        {
            get { return this.name; }
        }

        public ulong OriginalKey
        {
            get { return this.originalKey; }
        }

        public ulong Key
        {
            get { return this.key; }
            set
            {
                if (this.key != value)
                {
                    this.key = value;
                }
            }
        }

        public string OriginalHashKey
        {
            get { return this.originalHashKey; }
        }

        public string HashKey
        {
            get { return this.hashKey; }
            set
            {
                if (this.hashKey != value)
                {
                    this.hashKey = value;
                    this.Rehash();
                }
            }
        }

        public void Rehash()
        {
            if (!string.IsNullOrWhiteSpace(this.hashKey))
            {
                this.Key = System.Security.Cryptography.FNV64.GetHash(this.hashKey);
            }
        }

        public string[] Locale
        {
            get { return this.locale; }
        }

        public string this[Managers.STBL.Lang lang]
        {
            get
            {
                return this.locale[(int)lang];
            }
            set
            {
                this.locale[(int)lang] = value;
            }
        }

        public abstract bool CommitChanges();

        public AResourceStblHandle(string name, ulong key, string hashKey = "")
        {
            this.name = name;
            this.originalKey = key;
            this.key = key;
            this.originalHashKey = hashKey;
            this.hashKey = hashKey;
            // TODO: Use string table manager to try to fill in locale
        }
    }
}
