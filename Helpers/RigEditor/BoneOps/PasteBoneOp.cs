using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal class PasteBoneOp : BoneOp
    {
        public override string Name
        {
            get { return string.Format("Paste({0})",sCurrentPasteOp.GetSourceName()); }
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