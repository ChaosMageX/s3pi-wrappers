using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor
{
    public class BoneActionEventArgs
    {
        public BoneActionEventArgs(Bone bone)
        {
            mBone = bone;
        }

        private Bone mBone;
        public Bone Bone
        {
            get { return mBone; }
        }
    }
}