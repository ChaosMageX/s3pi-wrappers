﻿using System;
using System.Collections.Generic;
using System.Linq;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal abstract class BoneOp : IComparable<BoneOp>
    {
        private static IEnumerable<Type> sBoneOps;
        protected static IPasteBoneOp sCurrentPasteOp;
        private Bone mTargetBone;
        private BoneManager mBoneManager;
        static BoneOp()
        {
            var a = typeof(BoneOp).Assembly;
            sBoneOps = a
                .GetTypes()
                .Where(t => typeof (BoneOp).IsAssignableFrom(t) && !t.IsAbstract);

        }
        public static IEnumerable<BoneOp> GetOps(BoneManager manager,Bone bone)
        {
            var ops = new List<BoneOp>();
            foreach (var t in sBoneOps)
            {
                var op = (BoneOp)Activator.CreateInstance(t);
                op.mBoneManager = manager;
                op.TargetBone = bone;
                if (op.CanExecute())
                    ops.Add(op);
            }
            ops.Sort();
            return ops;
        }
        public abstract string Name { get; }

        public BoneManager BoneManager
        {
            get { return mBoneManager; }
            set { mBoneManager = value; }
        }

        public Bone TargetBone
        {
            get { return mTargetBone; }
            set { mTargetBone = value; }
        }


        public virtual bool CanExecute()
        {
            return TargetBone != null;
        }

        public abstract void Execute(Bone bone);

        public int CompareTo(BoneOp other)
        {
            return Name.CompareTo(other.Name);
        }
        public void OnExecute(object sender, EventArgs e)
        {
            Execute(TargetBone);
        }
    }
}
