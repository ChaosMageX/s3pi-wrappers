using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal class DeleteBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Delete"; }
        }

        public override void Execute(Bone bone)
        {
            BoneManager.DeleteBone(bone,false);
        }
    }
}