using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using s3piwrappers.Granny2;

namespace s3piwrappers.BoneTool
{
    internal class BoneTreeView : TreeView
    {
        private BoneManager mBoneManager;
        private Dictionary<Bone, TreeNode> mBoneMap;
        public BoneTreeView()
        {
            mBoneManager = new BoneManager();
            mBoneMap = new Dictionary<Bone, TreeNode>();
            ContextMenu = new ContextMenu();
            ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
            mBoneManager.BoneAdded += new BoneActionEventHandler(mBoneManager_BoneAdded);
            mBoneManager.BoneRemoved += new BoneActionEventHandler(mBoneManager_BoneRemoved);
            mBoneManager.BoneNameChanged += new BoneActionEventHandler(mBoneManager_BoneNameChanged);
            mBoneManager.BoneParentChanged += new BoneActionEventHandler(mBoneManager_BoneParentChanged);
        }

        private void RemoveNode(TreeNode node)
        {

            if (node.Parent == null)
            {
                Nodes.Remove(node);
            }
            else
            {
                node.Parent.Nodes.Remove(node);
            }
        }
        private void AddNode(TreeNode child, TreeNode parent)
        {
            if (parent == null) Nodes.Add(child);
            else parent.Nodes.Add(child);
        }
        private void mBoneManager_BoneParentChanged(BoneManager sender, BoneActionEventArgs e)
        {
            var parent = mBoneManager.GetParent(e.Bone);
            var parentNode = parent == null ? null : mBoneMap[parent];
            var childNode = mBoneMap[e.Bone];
            RemoveNode(childNode);
            AddNode(childNode, parentNode);
        }

        private void mBoneManager_BoneNameChanged(BoneManager sender, BoneActionEventArgs e)
        {
            mBoneMap[e.Bone].Text = e.Bone.Name;
        }

        private void mBoneManager_BoneRemoved(BoneManager sender, BoneActionEventArgs e)
        {
            var n = mBoneMap[e.Bone];
            if (n.Parent == null) Nodes.Remove(n);
            else {n.Parent.Nodes.Remove(n);}
            mBoneMap[e.Bone] = null;
        }
        private void mBoneManager_BoneAdded(BoneManager sender, BoneActionEventArgs e)
        {
            var node = CreateNode(e.Bone);
            mBoneMap[e.Bone] = node;
            var parent = mBoneManager.GetParent(e.Bone);
            var parentNode = parent == null ? null : mBoneMap[parent];
            AddNode(node,parentNode);
        }
        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            var m = (ContextMenu)sender;
            m.MenuItems.Clear();
            Bone b = SelectedNode == null ? null : SelectedNode.Tag as Bone;
            foreach (var op in BoneOp.GetOps(mBoneManager, b))
            {
                m.MenuItems.Add(op.Name, op.OnExecute);
            }
            

        }
        public IList<Bone> Bones
        {
            get { return mBoneManager.Bones; }
            set { mBoneManager.Bones = value; if(value!=null)BuildTree(); }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            SelectedNode = GetNodeAt(e.Location);
        }
        private static TreeNode CreateNode(Bone b)
        {
            var node = new TreeNode(b.Name);
            node.Tag = b;
            return node;
        }
        public void BuildTree()
        {
            Nodes.Clear();
            var nodes = Bones.Select(x => CreateNode(x)).ToArray();
            for (int i = 0; i < Bones.Count; i++)
            {
                mBoneMap[Bones[i]] = nodes[i];
                if (Bones[i].ParentIndex >= 0)
                {
                    nodes[Bones[i].ParentIndex].Nodes.Add(nodes[i]);
                }
                else
                {
                    Nodes.Add(nodes[i]);
                }
            }
        }
    }
}

