using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class AddBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Add..."; }
        }

        public override void Execute(Bone bone)
        {
            var dialog = new InputStringDialog(Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                var newBone = new Bone(0, null);
                newBone.Name = dialog.InputString;
                BoneManager.AddBone(newBone,bone);
            }
        }
        public override bool CanExecute()
        {
            return true;
        }
    }
}