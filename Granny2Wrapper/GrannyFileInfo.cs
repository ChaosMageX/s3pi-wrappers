using System;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers.Granny2
{
    public class GrannyFileInfo : GrannyElement
    {
        public GrannyFileInfo(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mArtToolInfo = new ArtToolInfo(0, handler);
            mModel = new Model(0, handler);
            mSkeleton = new Skeleton(0, handler);
        }

        public GrannyFileInfo(int APIversion, EventHandler handler, GrannyFileInfo basis)
            : this(APIversion, handler, basis.ArtToolInfo, basis.Skeleton, basis.Model, basis.FromFileName) { }

        public GrannyFileInfo(int APIversion, EventHandler handler, ArtToolInfo artToolInfo, Skeleton skeleton, Model model, string fromFileName)
            : base(APIversion, handler)
        {
            mArtToolInfo = new ArtToolInfo(0, handler, artToolInfo);
            mSkeleton = new Skeleton(0, handler, skeleton);
            mModel = new Model(0, handler, model);
            mFromFileName = fromFileName;
        }
        internal GrannyFileInfo(int APIversion, EventHandler handler, _GrannyFileInfo f)
            : base(APIversion, handler) { FromStruct(f); }



        private ArtToolInfo mArtToolInfo;
        private Skeleton mSkeleton;
        private Model mModel;
        private string mFromFileName;

        [ElementPriority(1)]
        public ArtToolInfo ArtToolInfo
        {
            get { return mArtToolInfo; }
            set { mArtToolInfo = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public string FromFileName
        {
            get { return mFromFileName; }
            set { mFromFileName = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public Model Model
        {
            get { return mModel; }
            set { mModel = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public Skeleton Skeleton
        {
            get { return mSkeleton; }
            set { mSkeleton = value; OnElementChanged(); }
        }

        internal void FromStruct(_GrannyFileInfo fileInfo)
        {
            mFromFileName = fileInfo.FromFileName;
            mArtToolInfo = new ArtToolInfo(0,handler,fileInfo.ArtToolInfo.S<_ArtToolInfo>());
            mSkeleton = new Skeleton(0, handler, fileInfo.Skeletons.S<IntPtr>().S<_Skeleton>());
            mModel = new Model(0, handler, fileInfo.Models.S<IntPtr>().S<_Model>());
        }
        internal _GrannyFileInfo ToStruct()
        {
            var file = new _GrannyFileInfo();
            var artToolInfo = mArtToolInfo.ToStruct();
            var skeleton = Skeleton.ToStruct();
            var model = Model.ToStruct();
            var pSkeleton = skeleton.Ptr();
            file.FromFileName = FromFileName;
            file.ArtToolInfo = artToolInfo.Ptr();
            file.SkeletonCount = 1;
            file.ModelCount = 1;
            model.Skeleton = pSkeleton;
            file.Models = model.Ptr().Ptr();
            file.Skeletons = pSkeleton.Ptr();
            return file;
        }

        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("FromFilename:\t{0}\n", mFromFileName);
                sb.AppendFormat("Art Tool Info:\n{0}\n", mArtToolInfo.Value);
                sb.AppendFormat("Model:\n{0}\n", mModel.Value);
                sb.AppendFormat("Skeleton:\n{0}\n", mSkeleton.Value);
                return sb.ToString();
            }
        }
        public override string ToString()
        {
            return mFromFileName.ToString();
        }
    }
}
