using System.Security.Cryptography;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph
{
    public abstract class AResourceStblHandle : IResourceStblHandle
    {
        private readonly string name;
        private readonly ulong originalKey;
        private readonly string originalHashKey;
        private ulong key;
        private string hashKey;
        private readonly string[] locale = new string[0x17];

        public string Name
        {
            get { return name; }
        }

        public ulong OriginalKey
        {
            get { return originalKey; }
        }

        public ulong Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                }
            }
        }

        public string OriginalHashKey
        {
            get { return originalHashKey; }
        }

        public string HashKey
        {
            get { return hashKey; }
            set
            {
                if (hashKey != value)
                {
                    hashKey = value;
                    Rehash();
                }
            }
        }

        public void Rehash()
        {
            if (!string.IsNullOrWhiteSpace(hashKey))
            {
                Key = FNV64.GetHash(hashKey);
            }
        }

        public string[] Locale
        {
            get { return locale; }
        }

        public string this[STBL.Lang lang]
        {
            get { return locale[(int) lang]; }
            set { locale[(int) lang] = value; }
        }

        public abstract bool CommitChanges();

        public AResourceStblHandle(string name, ulong key, string hashKey = "")
        {
            this.name = name;
            originalKey = key;
            this.key = key;
            originalHashKey = hashKey;
            this.hashKey = hashKey;
            // TODO: Use string table manager to try to fill in locale
        }
    }
}
