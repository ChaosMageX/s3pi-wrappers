using System;
using System.Collections.Generic;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor
{
    internal class BoneManager
    {
        private IList<Bone> mBones;
        public BoneManager(){}

        public IList<Bone> Bones
        {
            get { return mBones; }
            set { mBones = value; OnBoneSourceChanged(this, new EventArgs()); }
        }
        public Bone GetParent(Bone b)
        {
            if (b.ParentIndex == -1) return null;
            else return mBones[b.ParentIndex];
        }
        public IEnumerable<Bone> GetChildren(Bone b)
        {
            var ix = mBones.IndexOf(b);
            for (int i = 0; i < mBones.Count; i++)
            {
                if (mBones[i].ParentIndex == ix) yield return mBones[i];
            }
        }
        public void AddBone(Bone child, Bone parent)
        {
            Bones.Add(child);
            OnBoneAdded(this, new BoneActionEventArgs(child));
            SetParent(child, parent);
        }
        public void DeleteBone(Bone b, bool recursive)
        {
            if (b != null && mBones.Contains(b))
            {
                var deletedIndex = mBones.IndexOf(b);
                var parent = b.ParentIndex > -1 ? mBones[b.ParentIndex] : null;

                mBones.Remove(b);
                OnBoneRemoved(this, new BoneActionEventArgs(b));
                var trash = new List<Bone>();
                for (int i = 0; i < mBones.Count; i++)
                {
                    if (mBones[i].ParentIndex > deletedIndex)
                    {
                        mBones[i].ParentIndex--;
                    }
                    else if (mBones[i].ParentIndex == deletedIndex)
                    {
                        if (recursive)
                        {
                            trash.Add(mBones[i]);
                        }
                        else
                        {
                            SetParent(mBones[i], parent);
                        }
                    }
                }
                foreach (var item in trash) DeleteBone(item, recursive);
            }
        }
        public IEnumerable<Bone> GetDescendants(Bone b)
        {
            var list = new List<Bone>();
            GetDescendantsRecursive(b, list);
            return list;
        }
        private void GetDescendantsRecursive(Bone b, IList<Bone> list)
        {
            var ix = mBones.IndexOf(b);
            for (int i = 0; i < mBones.Count; i++)
            {
                if (mBones[i].ParentIndex == ix)
                {
                    list.Add(mBones[i]);
                    GetDescendantsRecursive(mBones[i], list);
                }
            }
        }
        public void SetParent(Bone child, Bone parent)
        {
            if (child.ParentIndex != mBones.IndexOf(parent))
            {
                child.ParentIndex = parent == null ? -1 : mBones.IndexOf(parent);
                OnBoneParentChanged(this, new BoneActionEventArgs(child));
            }
        }
        public void SetName(Bone b, string name)
        {
            if (b.Name != name)
            {
                b.Name = name;
                OnBoneUpdated(this, new BoneActionEventArgs(b));
            }
        }
        public void UpdateBone(Bone b)
        {
            OnBoneUpdated(this,new BoneActionEventArgs(b));
        }
        private void OnBoneParentChanged(BoneManager sender, BoneActionEventArgs e)
        {
            if (BoneParentChanged != null)
                BoneParentChanged(sender, e);
        }
        private void OnBoneRemoved(BoneManager sender, BoneActionEventArgs e)
        {
            if (BoneRemoved != null)
                BoneRemoved(sender, e);
        }
        private void OnBoneAdded(BoneManager sender, BoneActionEventArgs e)
        {
            if (BoneAdded != null)
                BoneAdded(sender, e);
        }
        private void OnBoneUpdated(BoneManager sender, BoneActionEventArgs e)
        {
            if (BoneUpdated != null)
                BoneUpdated(sender, e);
        }
        private void OnBoneSourceChanged(object sender, EventArgs e)
        {
            if (BoneSourceChanged != null)
                BoneSourceChanged(sender, e);
        }
        public event EventHandler BoneSourceChanged;
        public event BoneActionEventHandler BoneParentChanged;
        public event BoneActionEventHandler BoneRemoved;
        public event BoneActionEventHandler BoneAdded;
        public event BoneActionEventHandler BoneUpdated;
    }
}
