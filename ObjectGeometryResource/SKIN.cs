using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;

namespace s3piwrappers
{
    public class SKIN : ARCOLBlock
    {
        public class JointList : AResource.DependentList<Joint>
        {
            public JointList(EventHandler handler)
                : base(handler)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override Joint CreateElement(Stream s)
            {
                throw new Exception();
            }

            protected override void WriteElement(Stream s, Joint element)
            {
                throw new Exception();
            }
        }
        public class Joint : AHandlerElement, IEquatable<Joint>
        {
            private UInt32 mJointNameHash;
            private Single[] mTransformationMatrix;
            public Joint(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mTransformationMatrix =
                new Single[12]
                {
                    1,0,0,0,
                    0,1,0,0,
                    0,0,1,0
                };
            }
            public Joint(int APIversion, EventHandler handler, UInt32 joint, Single[] matrix)
                : base(APIversion, handler)
            {
                mJointNameHash = joint;
                mTransformationMatrix = matrix;
            }
            public Joint(int APIversion, EventHandler handler, Joint j)
                : this(APIversion, handler, j.mJointNameHash, j.mTransformationMatrix)
            {
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("JointNameHash:\t0x{0:X8}\n", mJointNameHash);
                    sb.AppendFormat("Transformation:\n", mJointNameHash);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]\n", mTransformationMatrix[0], mTransformationMatrix[4], mTransformationMatrix[8]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]\n", mTransformationMatrix[1], mTransformationMatrix[5], mTransformationMatrix[9]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]\n", mTransformationMatrix[2], mTransformationMatrix[6], mTransformationMatrix[10]);
                    sb.AppendFormat("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]\n", mTransformationMatrix[3], mTransformationMatrix[7], mTransformationMatrix[11]);
                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public uint JointNameHash
            {
                get { return mJointNameHash; }
                set { mJointNameHash = value; OnElementChanged(); }
            }

            [ElementPriority(2)]
            public float[] TransformationMatrix
            {
                get { return mTransformationMatrix; }
                set { mTransformationMatrix = value; OnElementChanged(); }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Joint(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Joint other)
            {
                return mJointNameHash.Equals(other.mJointNameHash);
            }
        }
        public SKIN(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 0x00000001;
            mJoints = new JointList(handler);
        }
        public SKIN(int APIversion, EventHandler handler, SKIN basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public SKIN(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnRCOLChanged(this, new EventArgs()); }
        }

        [ElementPriority(2)]
        public JointList Joints
        {
            get { return mJoints; }
            set { mJoints = value; OnRCOLChanged(this,new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n",mVersion);
                if(mJoints.Count >0)
                {
                    sb.AppendFormat("Joints:\n");
                    for (int i = 0; i < mJoints.Count; i++)
                    {
                        sb.AppendFormat("=={0}==\n{1}\n", i, mJoints[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private UInt32 mVersion;
        private JointList mJoints;
        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mJoints = new JointList(handler);
            int count = br.ReadInt32();
            uint[] names = new uint[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = br.ReadUInt32();
            }
            for (int i = 0; i < count; i++)
            {
                Single[] matrix = new Single[12];
                for (int j = 0; j < matrix.Length; j++)
                {
                    matrix[j] = br.ReadSingle();
                }
                mJoints.Add(new Joint(0, handler, names[i], matrix));
            }
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            if (mJoints == null) mJoints = new JointList(handler);
            bw.Write(mJoints.Count);
            foreach (var j in mJoints) bw.Write(j.JointNameHash);
            foreach (var j in mJoints)
            {
                for (int i = 0; i < 12; i++) bw.Write(j.TransformationMatrix[i]);
            }
            return s;
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new SKIN(0, handler, this);
        }

        public override string Tag
        {
            get { return "SKIN"; }
        }

        public override uint ResourceType
        {
            get { return 0x01D0E76B; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}