using System.Collections.Generic;
using System.Linq;
using s3piwrappers.RigEditor.Bones;

namespace s3piwrappers.RigEditor.ViewModels
{
    class SkeletonExportViewModel : AbstractViewModel
    {
        private readonly BoneManager mManager;
        private IEnumerable<BoneExportViewModel> mChildren;
        public SkeletonExportViewModel(BoneManager manager)
        {
            
            mChildren = manager.Bones.Where(x => x.ParentBoneIndex == -1).Select(x => new BoneExportViewModel(x, manager));
            mManager = manager;
        }

        public IEnumerable<BoneExportViewModel> Children
        {
            get { return mChildren; }
            set { if(mChildren!=value){mChildren = value; OnPropertyChanged("Children");} }
        }
    }
}
