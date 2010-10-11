using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class RenameNameBoneOp : BoneOp
    {
        public override string Name
        {
            get { return "Rename..."; }
        }

        public override void Execute(Bone bone)
        {
            var dialog = new InputStringDialog(Name, bone.Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                BoneManager.SetName(bone, dialog.InputString);
            }
        }
    }
}