using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool.BoneOps
{
    internal class SuffixHierarchyBoneOp : BoneOp
    {

        public override string Name
        {
            get { return "Add Suffix to Hierarchy..."; }
        }

        public override void Execute(Bone bone)
        {
            var dialog = new InputStringDialog(Name);
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                Suffix(bone, dialog.InputString);
                var descendants = BoneManager.GetDescendants(bone);
                foreach (var descendant in descendants)
                {
                    Suffix(descendant, dialog.InputString);
                }
            }
        }
        private void Suffix(Bone b, string suffix)
        {
            BoneManager.SetName(b, b.Name+suffix);
        }
    }
}