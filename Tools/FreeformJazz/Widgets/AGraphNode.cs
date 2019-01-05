using System;
using GraphForms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    /// <summary>
    /// A basic implementation of the <see cref="GraphElement"/> class 
    /// which keeps track of when the user has clicked on it in the 
    /// <see cref="AGraphNodeScene"/> instance that contains it.
    /// </summary>
    public abstract class AGraphNode : GraphElement
    {
        private AGraphNodeScene mScene;

        /// <summary>
        /// Creates a new <see cref="AGraphNode"/> instance and
        /// adds it to the given <see cref="AGraphNodeScene"/> instance.
        /// </summary>
        /// <param name="scene">The <see cref="AGraphNodeScene"/> instance
        /// which will contain this node after it is created.</param>
        public AGraphNode(AGraphNodeScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");
            this.mScene = scene;
            this.mScene.AddNode(this);
        }

        private bool bMouseGrabbed = false;

        /// <summary>
        /// Returns whether or not this node is currently "grabbed"
        /// by the user's mouse and is being moved around the
        /// <see cref="AGraphNodeScene"/> that contains it.
        /// </summary>
        public bool MouseGrabbed
        {
            get { return this.bMouseGrabbed; }
            set
            {
                if (this.bMouseGrabbed != value)
                {
                    this.bMouseGrabbed = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Overrides the <see cref="GraphElement.OnMouseDown(GraphMouseEventArgs)"/> function
        /// in order to <see cref="GraphElement.Invalidate"/> itself if it's being clicked on
        /// by the user.
        /// </summary>
        /// <param name="e">The mouse event data of this event.</param>
        /// <returns>true to signify that mouse event has been handled by this event handler.</returns>
        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            if (!this.bMouseGrabbed && !e.Handled)
            {
                this.bMouseGrabbed = true;
                this.Invalidate();
            }
            return base.OnMouseDown(e);
        }
    }
}
