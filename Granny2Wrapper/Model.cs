using System;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers.Granny2
{
    public class Model : GrannyElement,IEquatable<Model>
    {
        private String mName;
        private Transform mInitialPlacement;

        public Model(int APIversion, EventHandler handler) : base(APIversion, handler)
        {
            mInitialPlacement = new Transform(0,handler);
        }
        public Model(int APIversion, EventHandler handler, string name, Transform initialPlacement) : base(APIversion, handler)
        {
            mName = name;
            mInitialPlacement = new Transform(0,handler,initialPlacement);
        }
        public Model(int APIversion, EventHandler handler,Model basis) : this(APIversion, handler,basis.mName,basis.mInitialPlacement) { }
        internal Model(int APIversion, EventHandler handler, _Model m) : base(APIversion, handler) { FromStruct(m);}

        [ElementPriority(1)]
        public string Name
        {
            get { return mName; }
            set { mName = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public Transform InitialPlacement
        {
            get { return mInitialPlacement; }
            set { mInitialPlacement = value; OnElementChanged(); }
        }


        internal void FromStruct(_Model data)
        {
            Name = data.Name;
            InitialPlacement = new Transform(0,handler,data.InitialPlacement);
        }

        internal _Model ToStruct()
        {
            var m = new _Model();
            m.Name = Name;
            m.InitialPlacement = InitialPlacement.ToStruct();
            return m;
        }
        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Name:\t{0}\n", mName);
                sb.AppendFormat("InitialPlacement:\n{0}\n", mInitialPlacement.Value);
                return sb.ToString();
            }
        }
        public override string ToString()
        {
            return mName.ToString();
        }
        public bool Equals(Model other)
        {
            return mName.Equals(other.mName);
        }
    }
}