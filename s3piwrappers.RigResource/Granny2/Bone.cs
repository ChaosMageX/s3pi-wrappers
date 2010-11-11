using System;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers.Granny2
{
    public class Bone : GrannyElement, IEquatable<Bone>
    {
        private String mName;
        private Int32 mParentIndex;
        private Transform mLocalTransform;
        private Matrix4x4 mInverseWorld4x4;

        public Bone(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mName = "<New Bone>";
            mParentIndex = -1;
            mLocalTransform = new Transform(0, handler);
            mInverseWorld4x4 = new Matrix4x4(0, handler);
        }
        public Bone(int APIversion, EventHandler handler, Bone basis)
            : this(APIversion, handler, basis.mName, basis.mParentIndex, new Transform(0,handler, basis.mLocalTransform), new Matrix4x4(0,handler,basis.mInverseWorld4x4)) { }

        public Bone(int APIversion, EventHandler handler, string name, int parentIndex, Transform localTransform, Matrix4x4 inverseWorld4X4)
            : base(APIversion, handler)
        {
            mName = name;
            mParentIndex = parentIndex;
            mLocalTransform = localTransform;
            mInverseWorld4x4 = inverseWorld4X4;
        }
        internal Bone(int apiVersion, EventHandler handler, _Bone data)
            : base(apiVersion, handler) { FromStruct(data); }

        [ElementPriority(1)]
        public string Name
        {
            get { return mName; }
            set { if(mName!=value){mName = value; OnElementChanged();} }
        }
        [ElementPriority(2)]
        public int ParentIndex
        {
            get { return mParentIndex; }
            set { if(mParentIndex!=value){mParentIndex = value; OnElementChanged();} }
        }
        [ElementPriority(3)]
        public Transform LocalTransform
        {
            get { return mLocalTransform; }
            set { if(mLocalTransform!=value){mLocalTransform = value; OnElementChanged();} }
        }
        [ElementPriority(4)]
        public Matrix4x4 InverseWorld4X4
        {
            get { return mInverseWorld4x4; }
            set { if(mInverseWorld4x4!=value){mInverseWorld4x4 = value; OnElementChanged();} }
        }

        internal void FromStruct(_Bone data)
        {
            mName = data.Name;
            mParentIndex = data.ParentIndex;
            mLocalTransform = new Transform(0, handler, data.LocalTransform);
            mInverseWorld4x4 = new Matrix4x4(0, handler, data.InverseWorld4x4);
        }

        internal _Bone ToStruct()
        {
            var b = new _Bone();
            b.Name = Name;
            b.ParentIndex = ParentIndex;
            b.LocalTransform = LocalTransform.ToStruct();
            b.InverseWorld4x4 = InverseWorld4X4.ToStruct();
            return b;
        }
        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Name:\t{0}\n", mName);
                sb.AppendFormat("ParentIndex:\t{0}\n", mParentIndex);
                sb.AppendFormat("LocalTransform:\n{0}\n", mLocalTransform.Value);
                sb.AppendFormat("InverseWorld4x4:\n{0}\n", mInverseWorld4x4.Value);
                return sb.ToString();
            }
        }
        public override string ToString()
        {
            return Name.ToString();
        }


        public bool Equals(Bone other)
        {
            return base.Equals(other);
        }
    }
}