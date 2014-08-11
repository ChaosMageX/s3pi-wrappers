using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;
using GraphForms;
using GraphForms.Algorithms.Layout;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public abstract class DGNode : AGraphNode, ILayoutNode, IEquatable<DGNode>
    {
        /*/// <summary>
        /// Average Glyph Width for 5pt font
        /// </summary>
        public const float kAGW = 4;
        /// <summary>
        /// Maximum Glyph Height for 5pt font
        /// </summary>
        public const float kMGH = 8;
        /// <summary>
        /// Bounding box padding to prevent visual artifacts
        /// </summary>
        public const float kBBP = 5;/* */

        protected static Color sBorderNormalColor = Color.Black;
        protected static Color sBorderInEditColor = Color.Navy;
        protected static Color sBorderSelectColor = Color.Yellow;

        public static Color BorderNormalColor
        {
            get { return sBorderNormalColor; }
            set
            {
                if (!sBorderNormalColor.Equals(value))
                {
                    sBorderNormalColor = value;
                }
            }
        }

        public static Color BorderInEditColor
        {
            get { return sBorderInEditColor; }
            set
            {
                if (!sBorderInEditColor.Equals(value))
                {
                    sBorderInEditColor = value;
                }
            }
        }

        public static Color BorderSelectedColor
        {
            get { return sBorderSelectColor; }
            set
            {
                if (!sBorderSelectColor.Equals(value))
                {
                    sBorderSelectColor = value;
                }
            }
        }

        //public static readonly Brush TextBrush;
        public static readonly StringFormat TextFormat;
        //public static readonly Font TextFont;

        static DGNode()
        {
            //TextBrush = new SolidBrush(Color.Black);
            TextFormat = new StringFormat();
            TextFormat.Alignment = StringAlignment.Center;
            TextFormat.LineAlignment = StringAlignment.Center;
            //TextFont = new Font(FontFamily.GenericSansSerif, 5);
        }

        [Flags]
        protected enum RectCorners : byte
        {
            None = 0x00,
            TL = 0x01,
            TR = 0x02,
            BL = 0x04,
            BR = 0x08,
            Top    = 0x03,
            Bottom = 0x0C,
            Left   = 0x05,
            Right  = 0x0A,
            All  = 0x0f
        }

        protected static void AddRoundedRectangle(GraphicsPath path,
            float x, float y, float w, float h, 
            float rad, RectCorners corners = RectCorners.All)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            path.StartFigure();
            if (corners == RectCorners.None)
            {
                path.AddRectangle(new RectangleF(x, y, w, h));
            }
            else if (corners == RectCorners.All)
            {
                float d = 2 * rad;
                path.AddArc(x, y, d, d, 180, 90);
                path.AddLine(x + rad, y, x + w - rad, y);
                path.AddArc(x + w - d, y, d, d, -90, 90);
                path.AddLine(x + w, y + rad, x + w, y + h - rad);
                path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
                path.AddLine(x + w - rad, y + h, x + rad, y + h);
                path.AddArc(x, y + h - d, d, d, 90, 90);
                path.CloseFigure();
            }
            else
            {
                float d = 2 * rad;

                if ((corners & RectCorners.TL) == RectCorners.TL)
                {
                    path.AddArc(x, y, d, d, 180, 90);
                }

                if ((corners & RectCorners.Top) == RectCorners.Top)
                {
                    path.AddLine(x + rad, y, x + w - rad, y);
                    path.AddArc(x + w - d, y, d, d, -90, 90);
                }
                else if ((corners & RectCorners.TR) == RectCorners.TR)
                {
                    path.AddLine(x, y, x + w - rad, y);
                    path.AddArc(x + w - d, y, d, d, -90, 90);
                }
                else if ((corners & RectCorners.TL) == RectCorners.TL)
                {
                    path.AddLine(x + rad, y, x + w, y);
                }
                else
                {
                    path.AddLine(x, y, x + w, y);
                }

                if ((corners & RectCorners.Right) == RectCorners.Right)
                {
                    path.AddLine(x + w, y + rad, x + w, y + h - rad);
                    path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
                }
                else if ((corners & RectCorners.BR) == RectCorners.BR)
                {
                    path.AddLine(x + w, y, x + w, y + h - rad);
                    path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
                }
                else if ((corners & RectCorners.TR) == RectCorners.TR)
                {
                    path.AddLine(x + w, y + rad, x + w, y + h);
                }
                else
                {
                    path.AddLine(x + w, y, x + w, y + h);
                }

                if ((corners & RectCorners.Bottom) == RectCorners.Bottom)
                {
                    path.AddLine(x + w - rad, y + h, x + rad, y + h);
                    path.AddArc(x, y + h - d, d, d, 90, 90);
                }
                else if ((corners & RectCorners.BL) == RectCorners.BL)
                {
                    path.AddLine(x + w, y + h, x + rad, y + h);
                    path.AddArc(x, y + h - d, d, d, 90, 90);
                }
                else if ((corners & RectCorners.BR) == RectCorners.BR)
                {
                    path.AddLine(x + w - rad, y + h, x, y + h);
                }
                else
                {
                    path.AddLine(x + w, y + h, x, y + h);
                }

                path.CloseFigure();
            }
        }

        protected abstract class DGNodePropertyCommand<D, T, P>
            : PropertyCommand<T, P>
            where D : DGNode
            where T : DecisionGraphNode
        {
            protected D mDGNode;

            public DGNodePropertyCommand(D dgNode, T thing, 
                string property, P newValue, bool extendable)
                : base(thing, property, newValue, extendable)
            {
                this.mDGNode = dgNode;
            }

            public override bool Execute()
            {
                bool flag = base.Execute();
                if (flag)
                {
                    this.mDGNode.UpdateVisualization();
                }
                return flag;
            }

            public override void Undo()
            {
                base.Undo();
                this.mDGNode.UpdateVisualization();
            }
        }

        protected abstract class DGNodeRefPropertyCommand<D, T, P>
            : DGNodePropertyCommand<D, T, P>
            where D : DGNode
            where T : DecisionGraphNode
            where P : class, IHasHashedName
        {
            protected RefToValue<P> mRTV;

            public DGNodeRefPropertyCommand(D dgNode, T thing, 
                RefToValue<P> rtv, string property, P newValue, 
                bool extendable)
                : base(dgNode, thing, property, newValue, extendable)
            {
                this.mRTV = rtv;
            }

            public override bool Execute()
            {
                bool flag = base.Execute();
                if (flag)
                {
                    this.mRTV.SetValue(this.mNewVal);
                    this.mDGNode.UpdateVisualization();
                }
                return flag;
            }

            public override void Undo()
            {
                base.Undo();
                this.mRTV.SetValue(this.mOldVal);
                this.mDGNode.UpdateVisualization();
            }
        }

        [Flags]
        public enum DGFlag : byte
        {
            EntryPoint = 0x01,
            DecisionMaker = 0x02,
            Both = 0x03
        }

        private DGFlag mCategory = (DGFlag)0;

        private bool bInEditMode;
        private bool bVisible;

        private readonly DecisionGraphNode mDGN;
        protected readonly StateNode mState;
        protected readonly StateMachineScene mScene;
        protected readonly AnchorPoint mEntryAnchor;

        public DGNode(DecisionGraphNode dgn, StateNode state)
            : base(state.Scene)
        {
            this.bInEditMode = false;
            this.bVisible = true;
            this.mDGN = dgn;
            this.mState = state;
            this.mScene = state.Scene;
            this.mEntryAnchor = new AnchorPoint(this, -1, 0);
        }

        public virtual void UpdateVisualization()
        {
        }

        private class CategoryCommand : Command
        {
            private DGNode mNode;
            private DGFlag mOldValue;
            private DGFlag mNewValue;

            public CategoryCommand(DGNode node, DGFlag newValue)
            {
                this.mNode = node;
                this.mOldValue = node.mCategory;
                this.mNewValue = newValue;
                this.mLabel = "Change DG Node Category";
            }

            public override bool Execute()
            {
                this.SetValue(this.mNewValue);
                return true;
            }

            public override void Undo()
            {
                this.SetValue(this.mOldValue);
            }

            public override void Redo()
            {
                this.SetValue(this.mNewValue);
            }

            private void SetValue(DGFlag value)
            {
                DecisionGraph dg = this.mNode.mState.State.DecisionGraph;
                if (dg != null && this.mNode.mDGN != null)
                {
                    dg.Remove(this.mNode.mDGN);
                    if ((DGFlag.EntryPoint & value) ==
                         DGFlag.EntryPoint)
                    {
                        dg.AddEntryPoint(this.mNode.mDGN);
                    }
                    if ((DGFlag.DecisionMaker & value) ==
                         DGFlag.DecisionMaker)
                    {
                        dg.AddDecisionMaker(this.mNode.mDGN);
                    }
                }
                this.mNode.mCategory = value;
            }
        }

        public DGFlag Category
        {
            get { return this.mCategory; }
            set
            {
                if (this.mCategory != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new CategoryCommand(this, value));
                }
            }
        }

        public void SetCategory(DGFlag category, bool set = true)
        {
            if (set)
            {
                this.mCategory |= category;
            }
            else
            {
                this.mCategory &= ~category;
            }
        }

        public bool IsDecisionMaker
        {
            get 
            { 
                return (DGFlag.DecisionMaker & this.mCategory) 
                     == DGFlag.DecisionMaker; 
            }
        }

        public bool IsEntryPoint
        {
            get
            {
                return (DGFlag.EntryPoint & this.mCategory)
                     == DGFlag.EntryPoint;
            }
        }

        public bool InEditMode
        {
            get { return this.bInEditMode; }
            set
            {
                if (this.bInEditMode != value)
                {
                    this.bInEditMode = value;
                    this.Invalidate();
                }
            }
        }

        public bool Visible
        {
            get { return this.bVisible; }
            set
            {
                if (this.bVisible != value)
                {
                    this.IgnoreMouseEvents = !value;
                    this.bVisible = value;
                    this.Invalidate();
                }
            }
        }

        protected override bool IsDrawn()
        {
            return this.bVisible;
        }

        public DecisionGraphNode DGN
        {
            get { return this.mDGN; }
        }

        public StateNode State
        {
            get { return this.mState; }
        }

        public StateMachineScene Scene
        {
            get { return this.mScene; }
        }

        public AnchorPoint EntryAnchor
        {
            get { return this.mEntryAnchor; }
        }

        public abstract AnchorPoint GetAnchorFor(DGEdge edge);

        public interface IEdgeAction
        {
            public void Redo();

            public void Undo();
        }

        public class EdgeAction : IEdgeAction
        {
            private bool bAdd;
            private int mIndex;
            private DGEdge mEdge;
            private AnchorPoint mAP;

            public EdgeAction(bool add, int i, DGEdge edge, AnchorPoint ap)
            {
                if (edge == null)
                {
                    throw new ArgumentNullException("edge");
                }
                if (ap == null)
                {
                    throw new ArgumentNullException("ap");
                }
                if (i < 0)
                {
                    throw new ArgumentOutOfRangeException("i");
                }
                if (add)
                {
                    if (i > ap.Edges.Count)
                    {
                        throw new ArgumentOutOfRangeException("i");
                    }
                }
                else
                {
                    if (i >= ap.Edges.Count)
                    {
                        throw new ArgumentOutOfRangeException("i");
                    }
                    else if (ap.Edges[i] != edge)
                    {
                        throw new ArgumentException("ap.Edges[i] != edge");
                    }
                }
                this.bAdd = add;
                this.mIndex = i;
                this.mEdge = edge;
                this.mAP = ap;
            }

            public void Redo()
            {
                if (this.bAdd)
                {
                    this.mAP.Edges.Insert(this.mIndex, this.mEdge);
                }
                else
                {
                    this.mAP.Edges.RemoveAt(this.mIndex);
                }
            }

            public void Undo()
            {
                if (this.bAdd)
                {
                    this.mAP.Edges.RemoveAt(this.mIndex);
                }
                else
                {
                    this.mAP.Edges.Insert(this.mIndex, this.mEdge);
                }
            }
        }

        public virtual IEdgeAction AddEdge(DGEdge edge, AnchorPoint ap)
        {
            if (edge != null && ap != null)
            {
                return new EdgeAction(true, ap.Edges.Count, edge, ap);
            }
            return null;
        }

        public virtual IEdgeAction RemoveEdge(DGEdge edge)
        {
            if (edge != null)
            {
                AnchorPoint ap = this.GetAnchorFor(edge);
                if (ap != null)
                {
                    int i = ap.Edges.IndexOf(edge);
                    return new EdgeAction(false, i, edge, ap);
                }
            }
            return null;
        }

        public Box2F LayoutBBox
        {
            get 
            {
                System.Drawing.RectangleF bbox = this.BoundingBox;
                return new Box2F(bbox.X, bbox.Y, bbox.Width, bbox.Height);
            }
        }

        public bool PositionFixed
        {
            get { return this.MouseGrabbed; }
        }

        private bool bSelected = false;

        public bool Selected
        {
            get { return this.bSelected; }
            set
            {
                if (this.bSelected != value)
                {
                    this.bSelected = value;
                    this.Invalidate();
                }
            }
        }

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            bool flag = base.OnMouseDown(e);
            this.mScene.ClearStateNodeSelection();
            this.mScene.ClearStateEdgeSelection();
            if (!e.Handled)
            {
                this.mScene.SelectDGNode(this,
                    (Control.ModifierKeys & Keys.Control) != Keys.Control);
            }
            return flag;
        }

        public bool Equals(DGNode other)
        {
            return other != null && other.mDGN == this.mDGN;
        }
    }
}
