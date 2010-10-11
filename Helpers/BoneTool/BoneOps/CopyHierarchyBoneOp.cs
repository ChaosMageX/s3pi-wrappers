using System.Windows.Forms;
using s3piwrappers.Granny2;
using System.Collections.Generic;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class CopyHierarchyBoneOp : BoneOp, IPasteBoneOp
    {
        public override string Name
        {
            get { return "Copy Hierarchy"; }
        }

        private BoneHierarchy mCopy;
        public override void Execute(Bone bone)
        {
            sCurrentPasteOp = this;
            mCopy = CopyHierarchy(bone);


        }
        private BoneHierarchy CopyHierarchy(Bone b)
        {
            var clone = new Bone(0, null, b);
            var h = new BoneHierarchy(clone);
            foreach (var child in BoneManager.GetChildren(b))
            {
                h.Children.Add(CopyHierarchy(child));
            }
            return h;
        }

        public void Paste(Bone bone)
        {
            sCurrentPasteOp = null;
            AddHierarchy(mCopy,bone);
        }
        private void AddHierarchy(BoneHierarchy h,Bone parent)
        {
            BoneManager.AddBone(h.Bone,parent);
            foreach (var child in h.Children)
            {
                AddHierarchy(child,h.Bone);
            }
        }
    }
}