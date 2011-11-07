namespace s3piwrappers.RigEditor.Bones
{
    public class BoneActionEventArgs
    {
        public BoneActionEventArgs(RigResource.RigResource.Bone bone)
        {
            mBone = bone;
        }

        private RigResource.RigResource.Bone mBone;
        public RigResource.RigResource.Bone Bone
        {
            get { return mBone; }
        }
    }
}