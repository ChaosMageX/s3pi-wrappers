using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using s3pi.Interfaces;
using System.Text;
using System.Linq;

namespace s3piwrappers.Granny2
{
    public class Skeleton : GrannyElement, IEquatable<Skeleton>
    {
        private String mName;
        private BoneList mBones;


        public Skeleton(int APIversion, EventHandler handler): this(APIversion, handler,"",new BoneList(handler)){}
        public Skeleton(int APIversion, EventHandler handler, Skeleton basis) : this(APIversion, handler, basis.mName, new BoneList(handler, basis.mBones)) { }
        public Skeleton(int APIversion, EventHandler handler, string name, BoneList bones)
            : base(APIversion, handler)
        {
            mName = name;
            mBones = bones;
        }
        internal Skeleton(int APIversion, EventHandler handler, _Skeleton s) : base(APIversion, handler) { FromStruct(s); }
        
        [ElementPriority(1)]
        public string Name
        {
            get { return mName; }
            set { if(mName!=value){mName = value; OnElementChanged();} }
        }
        [ElementPriority(2)]
        public BoneList Bones
        {
            get { return mBones; }
            set { if(mBones!=value){mBones = value; OnElementChanged();} }
        }

        internal void FromStruct(_Skeleton data)
        {
            var bones = new List<Bone>();
            mName = data.Name;
            Int32 count = data.BoneCount;
            Int32 elementSize = Marshal.SizeOf(typeof(_Bone));
            IntPtr pCur = data.Bones;
            for (int i = 0; i < count; i++)
            {
                bones.Add(new Bone(0, handler, pCur.S<_Bone>()));
                pCur = new IntPtr(pCur.ToInt32() + elementSize);
            }
            mBones = new BoneList(handler,bones);
        }

        internal _Skeleton ToStruct()
        {
            _Skeleton s = new _Skeleton();
            s.Name = Name;
            s.BoneCount = Bones.Count;
            Int32 elementSize = Marshal.SizeOf(typeof(_Bone));
            IntPtr pBones = Marshal.AllocHGlobal(Bones.Count * elementSize);
            IntPtr pCur = new IntPtr(pBones.ToInt32());
            for (int i = 0; i < Bones.Count; i++)
            {
                Marshal.StructureToPtr(Bones[i].ToStruct(), pCur, false);
                pCur = new IntPtr(pCur.ToInt32() + elementSize);
            }
            s.Bones = pBones;
            return s;
        }
        public override string Value
        {
            get
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Name:\t{0}\n", mName);
                sb.AppendFormat("Bones:\n");
                for (int i = 0; i < mBones.Count; i++)
                {
                    sb.AppendFormat("==Bone[{0}]==\n{1}\n", i, mBones[i].Value);

                }
                return sb.ToString();
            }
        }
        public override string ToString()
        {
            return mName.ToString();
        }

        public bool Equals(Skeleton other)
        {
            return base.Equals(other);
        }
    }
}