using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal class CloneBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Clone"; }
        }

        public override void Execute(Bone bone)
        {
            var newBone = new Bone(0, null, bone);
            BoneManager.AddBone(newBone, BoneManager.GetParent(bone));
        }
    }
}