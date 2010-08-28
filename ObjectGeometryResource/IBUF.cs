using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;
namespace s3piwrappers
{
    public class IBUF2: IBUF
    {
        public IBUF2(int apiVersion, EventHandler handler, IBUF basis) : base(apiVersion, handler, basis)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
        {
        }

        public override uint ResourceType
        {
            get
            {
                return 0x0229684F;
            }
        }
    }
    public class IBUF : ARCOLBlock
    {
        public class Triangle : AHandlerElement, IEquatable<Triangle>,IComparable<Triangle>
        {
            private UInt16 mA;
            private UInt16 mB;
            private UInt16 mC;

            public Triangle(int APIversion, EventHandler handler, UInt16 a, UInt16 b, UInt16 c)
                : base(APIversion, handler)
            {
                mA = a;
                mB = b;
                mC = c;
            }
            public Triangle(int APIversion, EventHandler handler, Triangle basis)
                : this(APIversion, handler, basis.mA, basis.mB, basis.mC)
            {
            }
            public Triangle(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            [ElementPriority(1)]
            public ushort A
            {
                get { return mA; }
                set { mA = value; OnElementChanged(); }
            }

            [ElementPriority(2)]
            public ushort B
            {
                get { return mB; }
                set { mB = value; OnElementChanged(); }
            }

            [ElementPriority(3)]
            public ushort C
            {
                get { return mC; }
                set { mC = value; OnElementChanged(); }
            }
            public override string ToString()
            {
                return String.Format("[{0},{1},{2}]", mA, mB, mC);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Triangle(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public bool Equals(Triangle other)
            {
                return other.mA == mA && other.mB == mB && other.mC == mC;
            }

            public int CompareTo(Triangle other)
            {
                if (!A.Equals(other.A))
                {
                    return A.CompareTo(other.A);
                }
                else if (!B.Equals(other.B))
                {
                    return B.CompareTo(other.B);
                }
                else if (!C.Equals(other.C))
                {
                    return C.CompareTo(other.C);
                } 
                else
                {
                    return 0;
                }
            }
        }
        public class TriangleList : AHandlerList<Triangle>,IGenericAdd
        {
            public TriangleList(EventHandler handler)
                : base(handler)
            {
            }

            public bool Add(params object[] fields)
            {
                if(fields.Length==1&&typeof(Triangle).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Triangle) fields[0]);
                    return true;
                }
                return false;
            }

            public void Add()
            {
                Add(new Triangle(0, base.handler));
            }
        }
        private UInt32 mVersion;
        private bool mCompressed;
        private UInt32 mUnknown01;
        private TriangleList mTriangles;
        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(2)]
        public bool Compressed { get { return mCompressed; } set { mCompressed = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(3)]
        public UInt32 Unknown01 { get { return mUnknown01; } set { mUnknown01 = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(4)]
        public TriangleList Triangles { get { return mTriangles; } set { mTriangles = value; OnRCOLChanged(this, new EventArgs()); } }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Compressed:\t{0}\n", mCompressed);
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("Triangles[{0}]\n", mTriangles.Count);
                return sb.ToString();
            }
        }
        public IBUF(int apiVersion, EventHandler handler, IBUF basis)
            : base(apiVersion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public IBUF(int apiVersion, EventHandler handler)
            : base(apiVersion, handler, null)
        {
        }
        public IBUF(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {

        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new IBUF(0, handler, this);
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            var isCompressed = br.ReadUInt32();
            mCompressed = isCompressed > 0 ? true : false;

            mUnknown01 = br.ReadUInt32();
            uint count = (uint)((s.Length - s.Position) / 2);
            mTriangles = new TriangleList(handler);
            ushort a = 0;
            int i = 0;
            while(i<count)
            {
                ushort[] indices = new ushort[3];
                for (int j = 0; j < indices.Length; j++)
                {
                    ushort b = br.ReadUInt16();
                    if(mCompressed)
                    {
                        a += b;
                        indices[j] = a;
                    }
                    else
                    {
                        indices[j] = b;
                    }

                    i++;
                }
                Triangle t = new Triangle(0, handler, indices[0], indices[1], indices[2]);
                mTriangles.Add(t);
            }
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write(mCompressed ?1:0);
            bw.Write(mUnknown01);
            if (mTriangles == null) mTriangles = new TriangleList(handler);
            mTriangles.Sort();
            ushort a = 0;
            foreach (var triangle in mTriangles)
            {
                ushort[] indices = new ushort[]{triangle.A,triangle.B,triangle.C};
                for (int i = 0; i < 3; i++)
                {
                    if(mCompressed)
                    {
                        bw.Write((ushort)(indices[i]-a));
                        a = indices[i];
                    }
                    else
                    {
                        bw.Write(indices[i]);
                    }
                }
            }
            return s;
        }

        public override uint ResourceType
        {
            get { return 0x01D0E70F; }
        }

        public override string Tag
        {
            get { return "IBUF"; }
        }
        static bool checking = Settings.Checking;
    }
}
