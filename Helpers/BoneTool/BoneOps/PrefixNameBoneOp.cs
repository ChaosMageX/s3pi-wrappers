using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class PrefixNameBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Add Prefix to Hierarchy..."; }
        }

        public override void Execute(Bone bone)
        {
            var dialog = new InputStringDialog(Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                Prefix(bone, dialog.InputString);
                var descendants = BoneManager.GetDescendants(bone);
                foreach (var descendant in descendants)
                {
                    Prefix(descendant,dialog.InputString);
                }
            }
        }
        private void Prefix(Bone b, string prefix)
        {
            BoneManager.SetName(b, prefix + b.Name);
        }
    }
}