using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using s3piwrappers.RigEditor.Bones;
using s3piwrappers.RigEditor.Commands;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor.ViewModels
{
    public interface IHaveBones
    {
        IList<BoneViewModel> Children { get; }
    }

    public class BoneViewModel : AbstractViewModel, IHaveBones
    {
        private readonly RigEditorViewModel mRig;
        private readonly RigResource.RigResource.Bone mBone;
        private readonly BoneManager mManager;
        private readonly ObservableCollection<BoneViewModel> mChildren;
        private readonly IHaveBones mParent;
        private EulerAngle mRotation;
        private readonly ICommand mSetOppositeCommand;

        public BoneViewModel(RigEditorViewModel rig,IHaveBones parent, RigResource.RigResource.Bone bone, BoneManager manager)
        {
            if (rig == null) throw new ArgumentNullException("rig");
            if (bone == null) throw new ArgumentNullException("bone");
            if (manager == null) throw new ArgumentNullException("manager");
            mRig = rig;
            mChildren = new ObservableCollection<BoneViewModel>();
            mParent = parent;
            mBone = bone;
            mManager = manager;
            foreach (var b in manager.GetChildren(mBone))
            {
                mChildren.Add(new BoneViewModel(mRig,this, b, mManager));
            }
            mRotation = new EulerAngle(new Quaternion(bone.Orientation.A, bone.Orientation.B, bone.Orientation.C, bone.Orientation.D));
            manager.BoneAdded += new BoneActionEventHandler(OnBoneAdded);
            manager.BoneRemoved += new BoneActionEventHandler(OnBoneRemoved);
            manager.BoneParentChanged += new BoneActionEventHandler(OnBoneParentChanged);
            this.mSetOppositeCommand = new UserCommand<BoneViewModel>(x => true, ExecuteSetOpposite);
        }
        private static void ExecuteSetOpposite(BoneViewModel view)
        {
            var choices = view.Manager.Bones.OrderBy(x=> x.Name);
            var dialog = new BoneSelectDialog(choices, "Select a New Opposite...");
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                view.Opposite = view.Manager.Bones.IndexOf(dialog.SelectedBone);
            }
        }


        private void OnBoneParentChanged(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            var child = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
            if (parent != null)
            {
                if(parent.Equals(mBone) && child == null)
                {
                    mChildren.Add(new BoneViewModel(mRig,this,e.Bone,sender));
                }
                if (child != null && !sender.GetParent(e.Bone).Equals(mBone))
                {
                    mChildren.Remove(child);
                }
            } else
            {
                if (child != null)
                    mChildren.Remove(child);
            }
        }

        private void OnBoneRemoved(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            if (parent !=null && parent.Equals(mBone))
            {
                var child = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
                if (child != null)
                {
                    mChildren.Remove(child);
                }
            }
        }

        private void OnBoneAdded(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            if (parent !=null && parent.Equals(mBone))
            {
                mChildren.Add(new BoneViewModel(mRig,this, e.Bone, sender));
            }
        }

        public bool HasChildren { get { return mChildren.Count > 0; } }
        public BoneManager Manager { get { return mManager; } }
        public IHaveBones Parent { get { return mParent; } }
        public RigResource.RigResource.Bone Bone { get { return mBone; } }
        public IList<BoneViewModel> Children { get { return mChildren; } }
        public RigEditorViewModel Rig { get { return mRig; } }
        public string Tag
        {
            get
            {
                var bone = this;
                var ix = bone.Manager.IndexOfBone(bone.Manager.Bones, bone.Bone);
                var opposite = bone.Manager.Bones[(int)bone.Bone.OpposingBoneIndex];
                var format = String.Format("(0x{1:X8}) {0}", bone.BoneName, bone.HashedName);
                return format;
            }
        }
        public ICommand SetOppositeCommand
        {
            get { return mSetOppositeCommand; }
        }
        public Int32 Opposite
        {
            get { return mBone.OpposingBoneIndex; }
            set { mBone.OpposingBoneIndex = value; OnPropertyChanged("Opposite"); OnPropertyChanged("Tag"); OnPropertyChanged("OppositeName"); }
        }
        public String OppositeName
        {
            get { return Manager.Bones[Opposite].Name; }
        }
        public UInt32 Flags
        {
            get { return mBone.Unknown2; }
            set { mBone.Unknown2 = value; OnPropertyChanged("Flags"); }
        }
        public String BoneName
        {
            get { return mBone.Name; }
            set 
            { 
                mBone.Name = value;
                this.HashedName = FNV32.GetHash(mBone.Name);
                OnPropertyChanged("BoneName"); OnPropertyChanged("Tag"); OnPropertyChanged("OppositeName");
                
            }
        }
        public UInt32 HashedName
        {
            get { return mBone.Hash; }
            set { mBone.Hash = value; OnPropertyChanged("HashedName"); OnPropertyChanged("Tag"); }
        }
      
        public float ScaleX
        {
            get { return mBone.Scaling.X; }
            set
            {
                mBone.Scaling.X = value;
                OnPropertyChanged("ScaleX");
            }
        }
        public float ScaleY
        {
            get { return mBone.Scaling.Y; }
            set
            {
                mBone.Scaling.Y = value;
                OnPropertyChanged("ScaleY");
            }
        }
        public float ScaleZ
        {
            get { return mBone.Scaling.Z; }
            set
            {
                mBone.Scaling.Z = value;
                OnPropertyChanged("ScaleZ");
            }
        }


        public float PositionX
        {
            get { return mBone.Position.X; }
            set
            {
                mBone.Position.X = value;
                OnPropertyChanged("PositionX");
            }
        }
        public float PositionY
        {
            get { return mBone.Position.Y; }
            set
            {
                mBone.Position.Y = value;
                OnPropertyChanged("PositionY");
            }
        }
        public float PositionZ
        {
            get { return mBone.Position.Z; }
            set
            {
                mBone.Position.Z = value;
                OnPropertyChanged("PositionZ");
            }
        }
        private void SetEulers()
        {
            var q = new Quaternion(new Matrix(mRotation));
            mBone.Orientation.A = (float)q.X;
            mBone.Orientation.B = (float)q.Y;
            mBone.Orientation.C = (float)q.Z;
            mBone.Orientation.D = (float)q.W;
        }
        private static double RadToDeg(double rad)
        {
            return (rad * 180f) / Math.PI;
        }
        private static double DegToRad(double deg)
        {
            return (deg / 180f) * Math.PI;
        }

        public EulerAngle Rotation
        {
            get { return mRotation; }
            set { mRotation = value; OnPropertyChanged("RotationX"); OnPropertyChanged("RotationY"); OnPropertyChanged("RotationZ"); }
        }
        public double RotationX
        {
            get { return RadToDeg(mRotation.Roll); }
            set
            {
                mRotation.Roll = DegToRad(value);
                SetEulers();
                OnPropertyChanged("RotationX");
            }
        }
        public double RotationY
        {
            get { return RadToDeg(mRotation.Yaw); }
            set
            {
                mRotation.Yaw = DegToRad(value);
                SetEulers();
                OnPropertyChanged("RotationY");
            }
        }
        public double RotationZ
        {
            get { return RadToDeg(mRotation.Pitch); }
            set
            {
                mRotation.Pitch = DegToRad(value);
                SetEulers();
                OnPropertyChanged("RotationZ");
            }
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

    }
}