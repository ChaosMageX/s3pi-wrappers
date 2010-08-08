using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    public class SkinControllerResourceHandler : AResourceHandler
    {
        public SkinControllerResourceHandler()
        {
            base.Add(typeof(SkinControllerResource), new List<string>() { "0x00AE6C67" });
        }
    }
    public class SkinControllerResource : AResource
    {
        public class EntryList : DependentList<Entry>
        {
            public EntryList(EventHandler handler) : base(handler) { }

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override Entry CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, Entry element)
            {
                throw new NotImplementedException();
            }
        }
        public class Entry : AHandlerElement, IEquatable<Entry>
        {
            private string mName;
            private float[] mMatrix = new float[12];
            public Entry(int APIversion, EventHandler handler, Entry basis)
                : this(APIversion, handler, basis.mName, basis.mMatrix) { }
            public Entry(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public Entry(int APIversion, EventHandler handler, String name, float[] matrix)
                : base(APIversion, handler)
            {
                mName = name;
                mMatrix = matrix;
            }
            public string Value
            {
                get 
                { 
                    StringBuilder s = new StringBuilder();
                    s.AppendFormat("{0}\n", mName);
                    s.AppendFormat("[{0,8:0.00000}{1,8:0.00000}{2,8:0.00000}]\n", mMatrix[0], mMatrix[1], mMatrix[2]);
                    s.AppendFormat("[{0,8:0.00000}{1,8:0.00000}{2,8:0.00000}]\n", mMatrix[3], mMatrix[4], mMatrix[5]);
                    s.AppendFormat("[{0,8:0.00000}{1,8:0.00000}{2,8:0.00000}]\n", mMatrix[6], mMatrix[7], mMatrix[8]);
                    s.AppendFormat("[{0,8:0.00000}{1,8:0.00000}{2,8:0.00000}]\n", mMatrix[9], mMatrix[10], mMatrix[11]);

                    return s.ToString();
                }
            }
            [ElementPriority(1)]
            public string Name
            {
                get { return mName; }
                set { mName = value; OnElementChanged(); }
            }

            [ElementPriority(2)]
            public float[] Matrix
            {
                get { return mMatrix; }
                set { mMatrix = value; OnElementChanged(); }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Entry(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Entry other)
            {
                return mName.Equals(other.mName);
            }
        }
        private UInt32 mVersion;
        private EntryList mEntries;
        public SkinControllerResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            mEntries = new EntryList(this.OnResourceChanged);
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }

        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                sb.AppendFormat("\nEntries:\n");
                for (int i = 0; i < mEntries.Count; i++) sb.AppendFormat("==[{0}]==\n{1}\n", i, mEntries[i].Value);
                return sb.ToString();
            }
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(2)]
        public EntryList Entries
        {
            get { return mEntries; }
            set { mEntries = value; OnResourceChanged(this, new EventArgs()); }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s, Encoding.BigEndianUnicode);
            List<string> names = new List<string>();
            mVersion = br.ReadUInt32();
            int count1 = br.ReadInt32();
            for (int i = 0; i < count1; i++) names.Add(br.ReadString());
            int count2 = br.ReadInt32();
            if (checking && count2 != count1) throw new Exception("Expected name count and matrix to be equal.");
            for (int i = 0; i < count2; i++)
            {
                float[] matrix = new float[12];
                for(int j=0;j<12;j++)
                {
                    matrix[j] = br.ReadSingle();
                }
                mEntries.Add(new Entry(0, this.OnResourceChanged, names[i], matrix));
            }
        }
        protected override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            bw.Write(mEntries.Count);
            foreach (var entry in mEntries) Write7BitStr(s, entry.Name, Encoding.BigEndianUnicode);
            bw.Write(mEntries.Count);
            foreach (var entry in mEntries)
            {
                for (int j = 0; j < 12; j++)
                {
                    bw.Write(entry.Matrix[j]);
                }
            }
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}
