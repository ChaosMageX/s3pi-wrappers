using System;
using GraphForms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public abstract class AGraphNode : GraphElement
    {
        private readonly AGraphNodeScene mScene;
        private readonly int mIndex;

        public AGraphNode(AGraphNodeScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");
            this.mScene = scene;
            this.mIndex = this.mScene.AddNode(this);
        }

        public int Index
        {
            get { return this.mIndex; }
        }

        private bool bMouseGrabbed = false;

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
