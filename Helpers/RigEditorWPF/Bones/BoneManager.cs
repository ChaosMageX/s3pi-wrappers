using System;
using System.Collections.Generic;
namespace s3piwrappers.RigEditor.Bones
{
    public class BoneManager
    {
        private IList<RigResource.RigResource.Bone> mBones;

        public IList<RigResource.RigResource.Bone> Bones
        {
            get { return mBones; }
            set { mBones = value; OnBoneSourceChanged(this, new EventArgs()); }
        }
        public RigResource.RigResource.Bone GetParent(RigResource.RigResource.Bone b)
        {
            if (b.ParentBoneIndex == -1) return null;
            else return mBones[b.ParentBoneIndex];
        }
        public int IndexOfBone(IList<RigResource.RigResource.Bone> bones, RigResource.RigResource.Bone bone )
        {
            for (int i = 0; i < bones.Count; i++)
            {
                if (bone == bones[i]) return i;
            }
            return -1;
        }
        public IEnumerable<RigResource.RigResource.Bone> GetChildren(RigResource.RigResource.Bone b)
        {
            var ix = IndexOfBone(mBones,b);
            for (int i = 0; i < mBones.Count; i++)
            {
                if (mBones[i].ParentBoneIndex == ix) yield return mBones[i];
            }
        }
        public void AddBone(RigResource.RigResource.Bone child, RigResource.RigResource.Bone parent)
        {
            Bones.Add(child);
            OnBoneAdded(this, new BoneActionEventArgs(child));
            SetParent(child, parent);
        }
        public void DeleteBone(RigResource.RigResource.Bone b, bool recursive)
        {
            if (b != null && mBones.Contains(b))
            {
                var deletedIndex = IndexOfBone(mBones,b);
                var parent = b.ParentBoneIndex > -1 ? mBones[b.ParentBoneIndex] : null;

                mBones.Remove(b);
                OnBoneRemoved(this, new BoneActionEventArgs(b));
                var trash = new List<RigResource.RigResource.Bone>();
                for (int i = 0; i < mBones.Count; i++)
                {
                    if (mBones[i].ParentBoneIndex > deletedIndex)
                    {
                        mBones[i].ParentBoneIndex--;
                    }
                    else if (mBones[i].ParentBoneIndex == deletedIndex)
                    {
                        if (recursive)
                        {
                            trash.Add(mBones[i]);
                        }
                    }
                }
                foreach (var item in trash) DeleteBone(item, recursive);
            }
        }
        public IEnumerable<RigResource.RigResource.Bone> GetDescendants(RigResource.RigResource.Bone b)
        {
            var list = new List<RigResource.RigResource.Bone>();
            GetDescendantsRecursive(b, list);
            return list;
        }
        private void GetDescendantsRecursive(RigResource.RigResource.Bone b, IList<RigResource.RigResource.Bone> list)
        {
            var ix  = IndexOfBone(mBones,b);
            for (int i = 0; i < mBones.Count; i++)
            {
                if (mBones[i].ParentBoneIndex == ix)
                {
                    list.Add(mBones[i]);
                    GetDescendantsRecursive(mBones[i], list);
                }
            }
        }
        public void SetParent(RigResource.RigResource.Bone child, RigResource.RigResource.Bone parent)
        {
            if (child.ParentBoneIndex != IndexOfBone(mBones, parent))
            {
                child.ParentBoneIndex = parent == null ? -1 : IndexOfBone(mBones, parent);
                OnBoneParentChanged(this, new BoneActionEventArgs(child));
            }
        }
        public void SetName(RigResource.RigResource.Bone b, string name)
        {
            if (b.Name != name)
            {
                b.Name = name;
                OnBoneUpdated(this, new BoneActionEventArgs(b));
            }
        }
        public void UpdateBone(RigResource.RigResource.Bone b)
        {
            OnBoneUpdated(this, new BoneActionEventArgs(b));
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
