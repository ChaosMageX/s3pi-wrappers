using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class CutBoneOp : BoneOp, IPasteBoneOp
    {

        public override string Name
        {
            get { return "Cut"; }
        }

        public override void Execute(Bone bone)
        {
            sCurrentPasteOp = this;
            sTarget = bone;
        }

        public void Paste(Bone bone)
        {
            BoneManager.SetParent(sTarget, bone);
            sTarget = null;
        }
    }
}