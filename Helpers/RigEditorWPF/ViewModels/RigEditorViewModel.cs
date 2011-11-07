using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using s3pi.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using s3piwrappers.RigEditor.Bones;
using s3piwrappers.RigEditor.Commands;
using s3piwrappers.RigEditor.Geometry;
using System.Windows;
using Quaternion = s3pi.Interfaces.Quaternion;

namespace s3piwrappers.RigEditor.ViewModels
{
    public class RigEditorViewModel : AbstractViewModel, IHaveBones
    {
        private readonly RigResource.RigResource mRig;
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
        public ICommand ExportRigCommand { get; private set; }
        public RigEditorViewModel(RigResource.RigResource rig)
        {
            rig.ResourceChanged += new EventHandler(OnResourceChanged);
            mIsSaving = false;
            mChildren = new ObservableCollection<BoneViewModel>();
            mRig =  rig;
            mManager = new BoneManager();
            mManager.Bones = mRig.Bones;
            foreach (var bone in mRig.Bones)
            {
                if (bone.ParentBoneIndex == -1)
                {
                    mChildren.Add(new BoneViewModel(this,this, bone, mManager));
                }
            }
            mManager.BoneAdded += new BoneActionEventHandler(OnBoneAdded);
            mManager.BoneRemoved += new BoneActionEventHandler(OnBoneRemoved);
            mManager.BoneParentChanged += new BoneActionEventHandler(OnBoneParentChanged);
            AddBoneCommand = new UserCommand<RigEditorViewModel>(x => true, y => y.Manager.AddBone(new RigResource.RigResource.Bone(0, null), null));
            GetMatrixInfoCommand = new UserCommand<RigEditorViewModel>(x => true, ExecuteMatrixInfo);
            CommitCommand = new UserCommand<RigEditorViewModel>(x => true, y => { mIsSaving = true; Application.Current.Shutdown(); });
            CancelCommand = new UserCommand<RigEditorViewModel>(x => true, y => { mIsSaving = false; Application.Current.Shutdown(); });
            IResourceKey key = new TGIBlock(0,null);
            key.ResourceType = 0x00000000;
        }
        public IList<RigResource.RigResource.Bone> Bones
        {
            get { return new ObservableCollection<RigResource.RigResource.Bone>(mRig.Bones); }
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
        static void ExecuteMatrixInfo(RigEditorViewModel param)
        {
            var sb = new StringBuilder();
            foreach (var b in param.Manager.Bones)
            {
                GetInfo(param.Manager, b, sb);
            }
            var dialog = new InfoDialog(sb.ToString(), "Matrix Info");
            dialog.ShowDialog();
        }
        private static Matrix GetAbsoluteTransform(BoneManager manager, RigResource.RigResource.Bone b)
        {
            var transforms = new List<Matrix>();
            while (b != null)
            {
                var q = new Geometry.Quaternion(b.Orientation.A, b.Orientation.B, b.Orientation.C, b.Orientation.D);
                var p = new Vector3(b.Position.X, b.Position.Y, b.Position.Z);
                var s = new Vector3(b.Scaling.X, b.Scaling.Y,b.Scaling.Z);
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
        private static void GetInfo(BoneManager manager, RigResource.RigResource.Bone bone, StringBuilder sb)
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
                    mChildren.Add(new BoneViewModel(this,this, e.Bone, sender));
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
                    var vm = new BoneViewModel(this, this, e.Bone, sender);
                    vm.Opposite = mManager.Bones.IndexOf(e.Bone);
                    mChildren.Add(vm);
                }
            }
        }
        public String SkeletonName
        {
            get { return mRig.SkeletonName; }
            set
            {
                mRig.SkeletonName = value;
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
