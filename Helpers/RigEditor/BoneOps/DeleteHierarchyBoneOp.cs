using s3piwrappers.Granny2;
using System.Linq;
namespace s3piwrappers.RigEditor.BoneOps
{
    internal class DeleteHierarchyBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Delete Hierarchy"; }
        }
        public override bool CanExecute()
        {
            return (base.CanExecute() && BoneManager.GetChildren(TargetBone).Any());
        }

        public override void Execute(Bone bone)
        {
            BoneManager.DeleteBone(bone,true);
        }
    }
}