using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers.Granny2
{
    public class BoneList : DependentList<Bone>
    {
        public BoneList(EventHandler handler) : base(handler) { }
        public BoneList(EventHandler handler, IEnumerable<Bone> ilt) : base(handler, ilt) { }

        public override void Add()
        {
            base.Add(new object[] { });
        }

        public override void UnParse(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override void Parse(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override Bone CreateElement(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override void WriteElement(System.IO.Stream s, Bone element)
        {
            throw new NotSupportedException();
        }

    }
}