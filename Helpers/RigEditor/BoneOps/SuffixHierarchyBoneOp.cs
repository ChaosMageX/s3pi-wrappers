using System.Windows.Forms;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;
using System.Linq;
namespace s3piwrappers.RigEditor.BoneOps
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
        public override bool CanExecute()
        {
            return (base.CanExecute() && BoneManager.GetChildren(TargetBone).Any());
        }
        private void Suffix(Bone b, string suffix)
        {
            BoneManager.SetName(b, b.Name+suffix);
        }
    }
}