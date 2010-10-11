using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class PasteBoneOp : BoneOp
    {
        public override string Name
        {
            get { return "Paste"; }
        }
        public override bool CanExecute()
        {
            return sTarget != null;
        }
        public override void Execute(Bone bone)
        {
            sCurrentPasteOp.Paste(bone);
        }
    }
}