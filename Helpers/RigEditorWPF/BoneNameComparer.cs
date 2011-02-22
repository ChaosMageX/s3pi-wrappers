using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor
{
    class BoneNameComparer :Comparer<Bone>
    {
        public override int Compare(Bone x, Bone y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
