using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using s3piwrappers.Granny2;
using System.ComponentModel;
using s3piwrappers.RigEditor.Bones;

namespace s3piwrappers.RigEditor.ViewModels
{
    class BoneExportViewModel : AbstractViewModel
    {
        private bool mIsChecked;
        private IEnumerable<BoneExportViewModel> mChildren;

        public BoneExportViewModel(Bone bone, BoneManager manager)
        {
            mIsChecked = bone.Name.Contains("slot");
            Bone = bone;
            mChildren = manager.GetChildren(bone).Select(x => new BoneExportViewModel(x,manager));
            if (bone.Name.Contains("ROOT"))
            {
                Thread t = null;
                t = new Thread(
                    new ThreadStart(delegate
                                        {
                                            while (t.IsAlive)
                                            {
                                                Thread.Sleep(new Random().Next(10000));
                                                IsChecked = !IsChecked;

                                            }
                                        }));
                t.Start();
            }
        }

        public Bone Bone { get; private set; }
        public bool IsChecked
        {
            get { return mIsChecked; }
            set
            {
                if(mIsChecked!=value)
                {
                    mIsChecked = value;
                    OnPropertyChanged("IsChecked");
                    foreach (var child in mChildren)
                    {
                        child.IsChecked = this.IsChecked;
                        child.OnPropertyChanged("IsChecked");
                    }
                }
            }
        }

        public IEnumerable<BoneExportViewModel> Children
        {
            get { return mChildren; }
            set { if(mChildren!=value){mChildren = value;OnPropertyChanged("Children");} }
        }
    }
}
