using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal class UnParentBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "UnParent"; }
        }

        public override void Execute(Bone bone)
        {
            BoneManager.SetParent(bone,null);
        }
    }
}