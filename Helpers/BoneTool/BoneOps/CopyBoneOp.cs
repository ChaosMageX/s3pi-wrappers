using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class CopyBoneOp : BoneOp, IPasteBoneOp
    {

        public override string Name
        {
            get { return "Copy"; }
        }

        private Bone mCopy;
        public override void Execute(Bone bone)
        {
            sCurrentPasteOp = this;
            mCopy = bone;
        }

        public void Paste(Bone bone)
        {
            var newBone = new Bone(0, null, mCopy);
            BoneManager.AddBone(newBone,bone);
        }
    }
}