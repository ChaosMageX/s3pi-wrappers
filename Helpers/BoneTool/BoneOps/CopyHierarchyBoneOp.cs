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

        public override void Execute(Bone bone)
        {
            sCurrentPasteOp = this;
            sTarget = bone;
        }

        public void Paste(Bone bone)
        {
            CloneBone(sTarget,bone);
            sTarget = null;
        }
        private void CloneBone(Bone bone,Bone parent)
        {
            var clone = new Bone(0, null, bone);
            BoneManager.AddBone(clone,parent);
            var children = BoneManager.GetChildren(bone);
            foreach (var child in children) CloneBone(child,clone);
            
        }
    }
}