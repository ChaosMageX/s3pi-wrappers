using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor.BoneOps
{
    internal interface IPasteBoneOp
    {
        void Paste(Bone bone);
        string GetSourceName();
    }
}