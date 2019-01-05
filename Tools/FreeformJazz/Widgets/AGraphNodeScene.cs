using System;
using GraphForms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    /// <summary>
    /// A basic implementation of the <see cref="GraphScene"/> class with 
    /// an internal array for storing <see cref="AGraphNode"/> instances
    /// and overrides of Mouse events to allow those instances to get dragged
    /// around by the user's mouse movements.
    /// </summary>
    public abstract class AGraphNodeScene : GraphScene
    {
        private AGraphNode[] mNodes;
        private int mNodeCount;

        public AGraphNodeScene()
        {
            this.mNodes = new AGraphNode[0];
            this.mNodeCount = 0;
        }

        #region List Functions

        /// <summary>
        /// Adds a given <see cref="AGraphNode"/> instance node to this scene
        /// and returns the index at which it was added. If the given node is
        /// already in this scene, its current index is returned. Otherwise,
        /// the last index is returned as it is the last node added.
        /// </summary>
        /// <param name="node">The <see cref="AGraphNode"/> instance 
        /// to be added to this scene, if it isn't already in the scene.</param>
        /// <returns>The index at which the given <see cref="AGraphNode"/> 
        /// instance is added to this scene, which is the last index unless 
        /// it's already in this scene, where its current index is returned.
        /// </returns>
        /// <remarks>The function basically doubles as an IndexOf function if
        /// the code invoking it is aware that the given <see cref="AGraphNode"/>
        /// instance is already present in this scene.</remarks>
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

        /// <summary>
        /// Removes the given <see cref="AGraphNode"/> instance from this scene,
        /// and returns true if it was successfully or false if it wasn't in 
        /// this scene to begin with.
        /// </summary>
        /// <param name="node">The <see cref="AGraphNode"/> instance 
        /// to remove from the scene.</param>
        /// <returns>true if the given <see cref="AGraphNode"/> instance is 
        /// successfully removed from this scene, or false if it wasn't in
        /// this scene to begin with.</returns>
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

        /// <summary>
        /// The number of <see cref="AGraphNode"/> instances currently contained in this scene.
        /// </summary>
        public int NodeCount
        {
            get { return this.mNodeCount; }
        }

        #endregion

        #region Mouse Functions

        private float mLastMouseX;
        private float mLastMouseY;

        /// <summary>
        /// Overrides the <see cref="GraphElement.OnMouseDown(GraphMouseEventArgs)"/> function
        /// in order to record the last point in the scene at which the mouse was depressed.
        /// </summary>
        /// <param name="e">The mouse event data of this event.</param>
        /// <returns>true to signify that mouse event has been handled by this event handler.
        /// </returns>
        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            this.mLastMouseX = e.SceneX;
            this.mLastMouseY = e.SceneY;
            return base.OnMouseDown(e);
        }

        /// <summary>
        /// Invoked whenever a <see cref="AGraphNode"/> instance that has been "grabbed"
        /// by the mouse during a previous MouseDown event is being moved by the mouse,
        /// which allows classes descended from <see cref="AGraphNodeScene"/> to take 
        /// appropriate actions pertaining to the node being moved and implement any
        /// effects it may have on other nodes in the scene and the scene itself.
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OnNodeMovedByMouse(AGraphNode node)
        {
        }

        /// <summary>
        /// Overrides the <see cref="GraphElement.OnMouseMove(GraphMouseEventArgs)"/> function
        /// in order to move around <see cref="AGraphNode"/> instances that have been "grabbed"
        /// by the mouse and invoke the <see cref="AGraphNodeScene.OnNodeMovedByMouse(AGraphNode)"/>
        /// function for each of those <see cref="AGraphNode"/> instances.
        /// </summary>
        /// <param name="e">The mouse event data of this event.</param>
        /// <returns>true to signify that mouse event has been handled by this event handler.
        /// </returns>
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

        /// <summary>
        /// Overrides the <see cref="GraphElement.OnMouseUp(GraphMouseEventArgs)"/> function
        /// in order to "ungrab" <see cref="AGraphNode"/> instances that had been previously
        /// "grabbed" by the mouse from an earlier MouseDown event sent to the scene.
        /// </summary>
        /// <param name="e">The mouse event data of this event.</param>
        /// <returns>true to signify that mouse event has been handled by this event handler.
        /// </returns>
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

        /// <summary>
        /// Overrides the <see cref="GraphScene.OnMouseLeave"/> function in order
        /// to "let go of" <see cref="AGraphNode"/> instances that had been previously
        /// "grabbed" by the mouse from an earlier MouseDown event sent to the scene.
        /// </summary>
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

        #endregion
    }
}
