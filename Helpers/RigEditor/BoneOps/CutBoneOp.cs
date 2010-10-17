using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal class CutBoneOp : BoneOp, IPasteBoneOp
    {

        public override string Name
        {
            get { return "Cut"; }
        }

        private BoneHierarchy mCopy;
        public override void Execute(Bone bone)
        {
            sCurrentPasteOp = this;
            mCopy = CopyHierarchy(bone);
            BoneManager.DeleteBone(bone,true);


        }
        private BoneHierarchy CopyHierarchy(Bone b)
        {
            var h = new BoneHierarchy(b);
            foreach (var child in BoneManager.GetChildren(b))
            {
                h.Children.Add(CopyHierarchy(child));
            }
            return h;
        }

        public void Paste(Bone bone)
        {
            sCurrentPasteOp = null;
            AddHierarchy(mCopy,bone);
        }
        private void AddHierarchy(BoneHierarchy h, Bone parent)
        {
            BoneManager.AddBone(h.Bone, parent);
            foreach (var child in h.Children)
            {
                AddHierarchy(child, h.Bone);
            }
        }
        
        public string GetSourceName()
        {
            return mCopy.Bone.Name + " Hierarchy";
        }
    }
}