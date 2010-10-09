using System;
using System.IO;
using s3pi.Interfaces;
using s3pi.Settings;

namespace s3piwrappers
{
    public abstract class GrannyData : AHandlerElement
    {
        internal static GrannyData CreateInstance(int apiVersion, EventHandler handler, Stream s)
        {
            return hasGranny2Dll ? (GrannyData)new WrappedGrannyData(apiVersion, handler, s) : (GrannyData)new RawGrannyData(apiVersion, handler, s);
        }
        internal static GrannyData CreateInstance(int apiVersion, EventHandler handler)
        {
            return hasGranny2Dll ? (GrannyData)new WrappedGrannyData(apiVersion, handler) : (GrannyData)new RawGrannyData(apiVersion, handler);
        }
        static GrannyData()
        {
            string dir = Path.GetDirectoryName(typeof(RigResource).Assembly.Location);
            hasGranny2Dll = File.Exists(Path.Combine(dir, "granny2.dll"));
        }
        private static bool hasGranny2Dll = false;
        protected const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;

        public GrannyData(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
        }
        public GrannyData(int APIversion, EventHandler handler, Stream s)
            : this(APIversion, handler)
        {
            Parse(s);
        }

        public abstract string Value { get; }
        protected abstract void Parse(Stream s);
        public abstract Stream UnParse();

        [ElementPriority(0)]
        public virtual BinaryReader Data
        {
            get
            {
                return new BinaryReader(UnParse());
            }
            set
            {
                if (value.BaseStream.CanSeek)
                {
                    value.BaseStream.Position = 0L;
                    this.Parse(value.BaseStream);
                }
                else
                {
                    MemoryStream s = new MemoryStream();
                    byte[] buffer = new byte[0x100000];
                    for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                    {
                        s.Write(buffer, 0, i);
                    }
                    this.Parse(s);
                }
                OnElementChanged();
            }
        }
    }
}