using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
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