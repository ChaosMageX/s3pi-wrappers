using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using GraphForms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class AnchorPoint : GraphElement
    {
        private static float sRad = 2.5f;
        private static float sRadSquared = 6.25f;

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

        public readonly List<DGEdge> Edges;

        private DGNode mOwner;

        private float mDirX;
        private float mDirY;

        public AnchorPoint(DGNode owner, float dirX, float dirY)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.Edges = new List<DGEdge>();

            this.mOwner = owner;
            this.SetParent(owner);

            this.mDirX = dirX;
            this.mDirY = dirY;

            this.UpdateBoundingBox();
        }

        public void UpdateBoundingBox()
        {
            this.BoundingBox = new RectangleF(
                sRad, sRad, 2 * sRad, 2 * sRad);
        }

        public float DirectionX
        {
            get { return this.mDirX; }
        }

        public float DirectionY
        {
            get { return this.mDirY; }
        }

        public void SetDirection(float dirX, float dirY)
        {
            if (this.mDirX != dirX || this.mDirY != dirY)
            {
                this.mDirX = dirX;
                this.mDirY = dirY;
                foreach (DGEdge edge in this.Edges)
                {
                    edge.Update();
                }
            }
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

        protected override void OnPositionChanged()
        {
            foreach (DGEdge edge in this.Edges)
            {
                edge.Update();
            }
            base.OnPositionChanged();
        }

        protected override bool IsDrawn()
        {
            return this.mOwner.Visible;
        }

        private static SolidBrush sBGBrush = new SolidBrush(Color.White);

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
            e.Graphics.DrawEllipse(Pens.Black, 
                -sRad, -sRad, 2 * sRad, 2 * sRad);
        }
    }
}
