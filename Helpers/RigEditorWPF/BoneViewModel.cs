using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using s3piwrappers.Granny2;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
{
    public interface IHaveBones
    {
        IList<BoneViewModel> Children { get; }
    }

    public class BoneViewModel : AbstractViewModel, IHaveBones
    {

        private readonly Bone mBone;
        private readonly BoneManager mManager;
        private readonly ObservableCollection<BoneViewModel> mChildren;
        private readonly IHaveBones mParent;
        private EulerAngle mRotation;

        public BoneViewModel(IHaveBones parent, Bone bone, BoneManager manager)
        {
            if (bone == null) throw new ArgumentNullException("bone");
            if (manager == null) throw new ArgumentNullException("manager");

            mChildren = new ObservableCollection<BoneViewModel>();
            mParent = parent;
            mBone = bone;
            mManager = manager;
            foreach (var b in manager.GetChildren(mBone))
            {
                mChildren.Add(new BoneViewModel(this, b, mManager));
            }
            mRotation = new EulerAngle(new Quaternion(bone.LocalTransform.Orientation.X, bone.LocalTransform.Orientation.Y, bone.LocalTransform.Orientation.Z, bone.LocalTransform.Orientation.W));
            manager.BoneAdded += new BoneActionEventHandler(OnBoneAdded);
            manager.BoneRemoved += new BoneActionEventHandler(OnBoneRemoved);
            manager.BoneParentChanged += new BoneActionEventHandler(OnBoneParentChanged);
        }


        private void OnBoneParentChanged(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = sender.GetParent(e.Bone);
            var child = mChildren.FirstOrDefault(x => x.Bone == e.Bone);
            if (parent != null)
            {
                if(parent.Equals(mBone) && child == null)
                {
                    mChildren.Add(new BoneViewModel(this,e.Bone,sender));
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
                mChildren.Add(new BoneViewModel(this, e.Bone, sender));
            }
        }

        public bool HasChildren { get { return mChildren.Count > 0; } }
        public BoneManager Manager { get { return mManager; } }
        public IHaveBones Parent { get { return mParent; } }
        public Bone Bone { get { return mBone; } }
        public IList<BoneViewModel> Children { get { return mChildren; } }

        public String BoneName
        {
            get { return mBone.Name; }
            set { mBone.Name = value; OnPropertyChanged("BoneName"); OnPropertyChanged("HashedName"); }
        }
        public string HashedName
        {
            get { return "0x" + FNV32.GetHash(mBone.Name).ToString("X8"); }
            set { }
        }
        public bool PositionEnabled
        {
            get { return (mBone.LocalTransform.Flags & TransformFlags.Position) != 0; }
            set
            {
                mBone.LocalTransform.Flags = value
                    ? mBone.LocalTransform.Flags |= TransformFlags.Position
                    : mBone.LocalTransform.Flags &= ((TransformFlags)0xFFFFFFFF ^ TransformFlags.Position);
                OnPropertyChanged("PositionEnabled");
            }
        }
        public bool RotationEnabled
        {
            get { return (mBone.LocalTransform.Flags & TransformFlags.Orientation) != 0; }
            set
            {
                mBone.LocalTransform.Flags = value
                    ? mBone.LocalTransform.Flags |= TransformFlags.Orientation
                    : mBone.LocalTransform.Flags &= ((TransformFlags)0xFFFFFFFF ^ TransformFlags.Orientation);
                OnPropertyChanged("RotationEnabled");
            }
        }
        public bool ScaleEnabled
        {
            get { return (mBone.LocalTransform.Flags & TransformFlags.ScaleShear) != 0; }
            set
            {
                mBone.LocalTransform.Flags = value
                    ? mBone.LocalTransform.Flags |= TransformFlags.ScaleShear
                    : mBone.LocalTransform.Flags &= ((TransformFlags)0xFFFFFFFF ^ TransformFlags.ScaleShear);
                OnPropertyChanged("ScaleEnabled");
            }
        }
        public float ScaleX
        {
            get { return mBone.LocalTransform.ScaleShearX.X; }
            set
            {
                mBone.LocalTransform.ScaleShearX.X = value;
                OnPropertyChanged("ScaleX");
            }
        }
        public float ScaleY
        {
            get { return mBone.LocalTransform.ScaleShearY.Y; }
            set
            {
                mBone.LocalTransform.ScaleShearY.Y = value;
                OnPropertyChanged("ScaleY");
            }
        }
        public float ScaleZ
        {
            get { return mBone.LocalTransform.ScaleShearZ.Z; }
            set
            {
                mBone.LocalTransform.ScaleShearZ.Z = value;
                OnPropertyChanged("ScaleZ");
            }
        }


        public float PositionX
        {
            get { return mBone.LocalTransform.Position.X; }
            set
            {
                mBone.LocalTransform.Position.X = value;
                OnPropertyChanged("PositionX");
            }
        }
        public float PositionY
        {
            get { return mBone.LocalTransform.Position.Y; }
            set
            {
                mBone.LocalTransform.Position.Y = value;
                OnPropertyChanged("PositionY");
            }
        }
        public float PositionZ
        {
            get { return mBone.LocalTransform.Position.Z; }
            set
            {
                mBone.LocalTransform.Position.Z = value;
                OnPropertyChanged("PositionZ");
            }
        }
        private void SetEulers()
        {
            var q = new Quaternion(new Matrix(mRotation));
            mBone.LocalTransform.Orientation.X = (float)q.X;
            mBone.LocalTransform.Orientation.Y = (float)q.Y;
            mBone.LocalTransform.Orientation.Z = (float)q.Z;
            mBone.LocalTransform.Orientation.W = (float)q.W;
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
            UpdateInverseWorld();
        }
        private void UpdateInverseWorld()
        {
            var q = new Quaternion(mBone.LocalTransform.Orientation.X, mBone.LocalTransform.Orientation.Y, mBone.LocalTransform.Orientation.Z, mBone.LocalTransform.Orientation.W);
            var p = new Vector3(mBone.LocalTransform.Position.X, mBone.LocalTransform.Position.Y, mBone.LocalTransform.Position.Z);
            var s = new Vector3(mBone.LocalTransform.ScaleShearX.X, mBone.LocalTransform.ScaleShearY.Y,
                                mBone.LocalTransform.ScaleShearZ.Z);
            Matrix mi = Matrix.CreateTransformMatrix(q, s, p).GetInverse();
            Matrix4x4 g = mBone.InverseWorld4X4;
            g.M0.X = (float)mi.M00;
            g.M0.Y = (float)mi.M10;
            g.M0.Z = (float)mi.M20;
            g.M0.W = (float)mi.M30;

            g.M1.X = (float)mi.M01;
            g.M1.Y = (float)mi.M11;
            g.M1.Z = (float)mi.M21;
            g.M1.W = (float)mi.M31;

            g.M2.X = (float)mi.M02;
            g.M2.Y = (float)mi.M12;
            g.M2.Z = (float)mi.M22;
            g.M2.W = (float)mi.M32;

            g.M3.X = (float)mi.M03;
            g.M3.Y = (float)mi.M13;
            g.M3.Z = (float)mi.M23;
            g.M3.W = (float)mi.M33;

        }

    }
}