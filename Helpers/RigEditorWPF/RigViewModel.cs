using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using s3piwrappers.Granny2;
using System.Collections.ObjectModel;
using System.Linq;
using s3piwrappers.RigEditor.Geometry;
using System.Windows;

namespace s3piwrappers.RigEditor
{
    public class RigViewModel : AbstractViewModel, IHaveBones
    {
        private readonly WrappedGrannyData mGrannyData;
        private IList<BoneViewModel> mChildren;
        private readonly BoneManager mManager;

        private bool mHasChanged;
        private bool mIsSaving;

        public bool IsSaving
        {
            get { return mIsSaving; }
        }

        public BoneManager Manager
        {
            get { return mManager; }
        }

        public ICommand AddBoneCommand { get; private set; }
        public ICommand GetMatrixInfoCommand { get; private set; }
        public ICommand CommitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public RigViewModel(RigResource rig)
        {
            rig.ResourceChanged += new EventHandler(OnResourceChanged);
            mIsSaving = false;
            mChildren = new ObservableCollection<BoneViewModel>();
            mGrannyData = (WrappedGrannyData)rig.Rig.GrannyData;
            mManager = new BoneManager();
            mManager.Bones = mGrannyData.FileInfo.Skeleton.Bones;
            foreach (var bone in mGrannyData.FileInfo.Skeleton.Bones)
            {
                if (bone.ParentIndex == -1)
                {
                    mChildren.Add(new BoneViewModel(this, bone, mManager));
                }
            }
            mManager.BoneAdded += new BoneActionEventHandler(OnBoneAdded);
            mManager.BoneRemoved += new BoneActionEventHandler(OnBoneRemoved);
            mManager.BoneParentChanged += new BoneActionEventHandler(OnBoneParentChanged);
            AddBoneCommand = new UserCommand<RigViewModel>(x => true, y => y.Manager.AddBone(new Bone(0, null), null));
            GetMatrixInfoCommand = new UserCommand<RigViewModel>(x => true, ExecuteMatrixInfo);
            CommitCommand = new UserCommand<RigViewModel>(x => x!=null&& x.HasChanged, y => { mIsSaving = true; Application.Current.Shutdown(); });
            CancelCommand = new UserCommand<RigViewModel>(x => true, y => { mIsSaving = false; Application.Current.Shutdown(); });

        }

        private void OnResourceChanged(object sender, EventArgs e)
        {
            HasChanged = true;
        }
        public bool HasChanged
        {
            get { return mHasChanged; }
            set { mHasChanged = value; OnPropertyChanged("HasChanged"); }
        }

        static void ExecuteMatrixInfo(RigViewModel param)
        {
            var sb = new StringBuilder();
            foreach (var b in param.Manager.Bones)
            {
                GetInfo(param.Manager, b, sb);
            }
            var dialog = new InfoDialog(sb.ToString(), "Matrix Info");
            dialog.ShowDialog();
        }
        private static Matrix GetAbsoluteTransform(BoneManager manager, Bone b)
        {
            var transforms = new List<Matrix>();
            while (b != null)
            {
                var q = new Quaternion(b.LocalTransform.Orientation.X, b.LocalTransform.Orientation.Y, b.LocalTransform.Orientation.Z, b.LocalTransform.Orientation.W);
                var p = new Vector3(b.LocalTransform.Position.X, b.LocalTransform.Position.Y, b.LocalTransform.Position.Z);
                var s = new Vector3(b.LocalTransform.ScaleShearX.X, b.LocalTransform.ScaleShearY.Y,
                                    b.LocalTransform.ScaleShearZ.Z);
                Matrix t = Matrix.CreateTransformMatrix(q, s, p);
                transforms.Add(t);
                b = manager.GetParent(b);
            }
            transforms.Reverse();
            var absolute = transforms[0];
            transforms.RemoveAt(0);
            while (transforms.Count > 0)
            {
                Matrix m = transforms[0];
                transforms.RemoveAt(0);
                absolute = absolute * m;
            }
            return absolute;
        }
        private static void GetInfo(BoneManager manager, Bone bone, StringBuilder sb)
        {
            var t = GetAbsoluteTransform(manager, bone);
            var ti = t.GetInverse();
            sb.AppendLine("============================");
            sb.AppendFormat("Name:\t{0}\r\n", bone.Name);
            sb.AppendFormat("Hashed:\t0x{0:X8}\r\n", FNV32.GetHash(bone.Name));
            sb.AppendLine("Absolute Transform (RSLT):");
            sb.AppendLine(t.TransposedString());
            sb.AppendLine("Absolute Transform Inverse (SKIN):");
            sb.AppendLine(ti.ToString());

        }
        void OnBoneParentChanged(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            var child = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
            if (parent == null)
            {
                if (child == null)
                {
                    mChildren.Add(new BoneViewModel(this, e.Bone, sender));
                }
            }
            else
            {
                if (child != null)
                {
                    mChildren.Remove(child);
                }
            }
        }

        void OnBoneRemoved(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            if (parent == null)
            {
                var view = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
                if (view != null)
                {
                    mChildren.Remove(view);
                }
            }
        }

        void OnBoneAdded(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            if (parent == null)
            {
                var view = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
                if (view == null)
                {
                    mChildren.Add(new BoneViewModel(this, e.Bone, sender));
                }
            }
        }
        public String Filename
        {
            get { return mGrannyData.FileInfo.FromFileName; }
            set { mGrannyData.FileInfo.FromFileName = value; OnPropertyChanged("Filename"); }
        }
        public String SkeletonName
        {
            get { return mGrannyData.FileInfo.Skeleton.Name; }
            set
            {
                mGrannyData.FileInfo.Skeleton.Name = value;
                mGrannyData.FileInfo.Model.Name = value; 
                OnPropertyChanged("SkeletonName");
            }
        }
        public IList<BoneViewModel> Children
        {
            get { return mChildren; }
            set { mChildren = value; OnPropertyChanged("Children"); }
        }
    }
}
