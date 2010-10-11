using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class DeleteHierarchyBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Delete Hierarchy"; }
        }

        public override void Execute(Bone bone)
        {
            BoneManager.DeleteBone(bone,true);
        }
    }
}