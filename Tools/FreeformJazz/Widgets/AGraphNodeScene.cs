using System;
using GraphForms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class AGraphNodeScene : GraphScene
    {
        private AGraphNode[] mNodes;
        private int mNodeCount;

        public AGraphNodeScene()
        {
            this.mNodes = new AGraphNode[0];
            this.mNodeCount = 0;
        }

        public int AddNode(AGraphNode node)
        {
            if (this.mNodeCount == 0)
            {
                if (this.mNodes.Length == 0)
                {
                    this.mNodes = new AGraphNode[4];
                }
            }
            else
            {
                for (int i = 0; i < this.mNodeCount; i++)
                {
                    if (this.mNodes[i] == node)
                    {
                        return i;
                    }
                }
                if (this.mNodes.Length == this.mNodeCount)
                {
                    AGraphNode[] nodes = new AGraphNode[2 * this.mNodeCount];
                    Array.Copy(this.mNodes, 0, nodes, 0, this.mNodeCount);
                    this.mNodes = nodes;
                }
            }
            this.mNodes[this.mNodeCount++] = node;
            return this.mNodeCount - 1;
        }

        public bool RemoveNode(AGraphNode node)
        {
            if (this.mNodeCount == 0)
            {
                return false;
            }
            int i;
            for (i = 0; i < this.mNodeCount; i++)
            {
                if (this.mNodes[i] == node)
                {
                    break;
                }
            }
            if (i == this.mNodeCount)
            {
                return false;
            }
            this.mNodeCount--;
            if (i < this.mNodeCount)
            {
                Array.Copy(this.mNodes, i + 1, this.mNodes, i, 
                    this.mNodeCount - i);
            }
            this.mNodes[this.mNodeCount] = null;
            return true;
        }

        public int NodeCount
        {
            get { return this.mNodeCount; }
        }

        private float mLastMouseX;
        private float mLastMouseY;

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            this.mLastMouseX = e.SceneX;
            this.mLastMouseY = e.SceneY;
            return base.OnMouseDown(e);
        }

        protected virtual void OnNodeMovedByMouse(AGraphNode node)
        {
        }

        protected override bool OnMouseMove(GraphMouseEventArgs e)
        {
            AGraphNode node;
            for (int i = 0; i < this.mNodeCount; i++)
            {
                node = this.mNodes[i];
                if (node.MouseGrabbed)
                {
                    node.SetPosition(
                        node.X + e.SceneX - this.mLastMouseX,
                        node.Y + e.SceneY - this.mLastMouseY);
                    this.OnNodeMovedByMouse(node);
                }
            }
            this.mLastMouseX = e.SceneX;
            this.mLastMouseY = e.SceneY;
            return base.OnMouseMove(e);
        }

        protected override bool OnMouseUp(GraphMouseEventArgs e)
        {
            AGraphNode node;
            for (int i = 0; i < this.mNodeCount; i++)
            {
                node = this.mNodes[i];
                node.MouseGrabbed = false;
            }
            return base.OnMouseUp(e);
        }

        protected override void OnMouseLeave()
        {
            AGraphNode node;
            for (int i = 0; i < this.mNodeCount; i++)
            {
                node = this.mNodes[i];
                node.MouseGrabbed = false;
            }
            base.OnMouseLeave();
        }
    }
}
