using System.Windows.Forms;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Common;
using System.Linq;
namespace s3piwrappers.RigEditor.BoneOps
{
    internal class ReplaceNameInHierarchyBoneOp : BoneOp
    {
        
        public override string  Name
        {
            get { return "Replace Name in Hierarchy..."; }
        }

        public override void  Execute(Bone bone)
        {
            var dialog = new FindAndReplaceDialog("Find and Replace...");
            dialog.ShowDialog();
            if(dialog.DialogResult == DialogResult.OK)
            {
                RenameBone(bone, dialog.FindString, dialog.ReplaceString);
                var descendants = BoneManager.GetDescendants(bone);
                foreach (var descendant in descendants)
                {
                    RenameBone(descendant, dialog.FindString, dialog.ReplaceString);
                }
            }
        }
        public override bool CanExecute()
        {
            return (base.CanExecute() && BoneManager.GetChildren(TargetBone).Any());
        }
        private void RenameBone(Bone bone,string find,string replace)
        {
            BoneManager.SetName(bone, bone.Name.Replace(find, replace));
        }
    }
}