﻿using System.Collections.Generic;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    class BoneHierarchy
    {
        public Bone Bone { get; private set; }
        public IList<BoneHierarchy> Children { get; private set; }
        public BoneHierarchy(Bone b)
        {
            Bone = b;
            Children = new List<BoneHierarchy>();
        }
    }
}
