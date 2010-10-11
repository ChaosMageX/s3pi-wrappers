using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
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