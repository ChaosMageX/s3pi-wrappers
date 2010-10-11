using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class PasteBoneOp : BoneOp
    {
        public override string Name
        {
            get { return string.Format("Paste({0})",((BoneOp)sCurrentPasteOp).Name); }
        }
        public override bool CanExecute()
        {
            return sCurrentPasteOp != null;
        }
        public override void Execute(Bone bone)
        {
            sCurrentPasteOp.Paste(bone);
        }
    }
}