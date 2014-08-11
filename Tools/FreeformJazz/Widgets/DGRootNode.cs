using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGRootNode : DGNode
    {
        private static float sRad = 7.5f;
        private static float sRadSquared = 56.25f;

        public static float Radius
        {
            get { return sRad; }
            set
            {
                if (sRad != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Radius");
                    }
                    sRad = value;
                    sRadSquared = value * value;
                }
            }
        }

        private DecisionGraph mDecisionGraph;

        public DGRootNode(DecisionGraph dg, StateNode state)
            : base(null, state)
        {
            if (dg == null)
            {
                throw new ArgumentNullException("dg");
            }
            this.mDecisionGraph = dg;

            this.mEntryAnchor.SetPosition(sRad, 0);
            this.mEntryAnchor.SetDirection(1, 0);

            this.UpdateVisualization();
        }

        public override void UpdateVisualization()
        {
            float r = sRad + 2 * AnchorPoint.Radius;
            this.BoundingBox = new RectangleF(-r, -r, 2 * r, 2 * r);
        }

        public DecisionGraph DecisionGraph
        {
            get { return this.mDecisionGraph; }
        }

        public override AnchorPoint GetAnchorFor(DGEdge edge)
        {
            if (this.mEntryAnchor.Edges.Contains(edge))
            {
                return this.mEntryAnchor;
            }
            return null;
        }

        public override Region Shape()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(-sRad, -sRad, 2 * sRad, 2 * sRad);
            return new Region(path);
        }

        public override bool Contains(PointF point)
        {
            return (point.X * point.X + point.Y * point.Y) <= sRadSquared;
        }

        private static readonly Pen sBorderPen = new Pen(Color.Black, 0.5f);

        private static SolidBrush sBGBrush
            = new SolidBrush(Color.Green);

        public static Color BackgroundColor
        {
            get { return sBGBrush.Color; }
            set
            {
                Color bgColor = sBGBrush.Color;
                if (!bgColor.Equals(value))
                {
                    sBGBrush.Color = value;
                }
            }
        }

        protected override void OnDrawBackground(
            System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.FillEllipse(sBGBrush, 
                -sRad, -sRad, 2 * sRad, 2 * sRad);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            e.Graphics.DrawEllipse(sBorderPen,
                -sRad, -sRad, 2 * sRad, 2 * sRad);
        }
    }
}
