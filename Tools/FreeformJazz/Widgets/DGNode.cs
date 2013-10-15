using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
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

        private bool bInEditMode;
        private bool bVisible;

        private readonly DecisionGraphNode mDGN;
        protected readonly StateMachineScene mScene;
        protected readonly AnchorPoint mEntryAnchor;

        public DGNode(DecisionGraphNode dgn, StateMachineScene scene)
            : base(scene)
        {
            this.bInEditMode = false;
            this.bVisible = true;
            this.mDGN = dgn;
            this.mScene = scene;
            this.mEntryAnchor = new AnchorPoint(this, -1, 0);
        }

        public virtual void UpdateVisualization()
        {
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

        public StateMachineScene Scene
        {
            get { return this.mScene; }
        }

        public AnchorPoint EntryAnchor
        {
            get { return this.mEntryAnchor; }
        }

        public abstract AnchorPoint GetAnchorFor(DGEdge edge);

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
