using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using GraphForms;
using GraphForms.Algorithms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class StateEdge : GraphElement, IGraphEdge<StateNode>, IUpdateable
    {
        public static readonly float sArrowSize = 6;
        private static readonly Pen sArrowPen;

        private static double sAngle = Math.PI * 10.0 / 180.0;

        private static Color sBorderNormalColor = Color.Black;
        private static Color sBorderSelectColor = Color.Yellow;

        public static double Angle
        {
            get { return sAngle; }
            set
            {
                if (sAngle != value)
                {
                    if (value < 0 || value > Math.PI / 2)
                    {
                        throw new ArgumentOutOfRangeException("Angle");
                    }
                    sAngle = value;
                }
            }
        }

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

        static StateEdge()
        {
            /*GraphicsPath ahPath = new GraphicsPath();
            float sin = (float)(sArrowSize * Math.Sin(Math.PI / 3.0));
            float cos = sArrowSize * 0.5f;
            ahPath.AddPolygon(new PointF[] 
            { 
                new PointF(0, 0), 
                new PointF(cos, -sin), 
                new PointF(-cos, -sin) 
            });
            CustomLineCap arrowHead = new CustomLineCap(ahPath, null);/* */

            sArrowPen = new Pen(Color.Black, 1);
            sArrowPen.StartCap = LineCap.RoundAnchor;
            //sArrowPen.CustomEndCap = arrowHead;
            sArrowPen.EndCap = LineCap.Round;
            sArrowPen.CustomEndCap = new AdjustableArrowCap(
                sArrowSize, sArrowSize, true);
            sArrowPen.LineJoin = LineJoin.Round;
        }

        private StateNode mSrcNode;
        private StateNode mDstNode;

        private bool bSelected;

        private float mSrcX;
        private float mSrcY;

        private float mDstX;
        private float mDstY;

        private double mLength;

        //private Pen mArrowPen;

        public StateEdge(StateNode srcNode, StateNode dstNode)
        {
            if (srcNode == null)
                throw new ArgumentNullException("srcNode");
            if (dstNode == null)
                throw new ArgumentNullException("dstNode");
            this.mSrcNode = srcNode;
            this.mDstNode = dstNode;

            this.bSelected = false;

            this.mSrcX = 0;
            this.mSrcY = 0;

            this.mDstX = 0;
            this.mDstY = 0;

            this.mLength = 0;

            /*this.mArrowPen = new Pen(Color.Black, 1);
            this.mArrowPen.StartCap = LineCap.RoundAnchor;
            this.mArrowPen.EndCap = LineCap.Round;
            this.mArrowPen.CustomEndCap = new AdjustableArrowCap(
                sArrowSize, sArrowSize, true);
            this.mArrowPen.LineJoin = LineJoin.Round;/* */

            //this.IgnoreMouseEvents = true;
            this.Zvalue = -1;
        }

        public StateNode SrcNode
        {
            get { return this.mSrcNode; }
        }

        public StateNode DstNode
        {
            get { return this.mDstNode; }
        }

        public void SetSrcNode(StateNode srcNode)
        {
            if (srcNode == null)
                throw new ArgumentNullException("srcNode");
            this.mSrcNode = srcNode;
            this.Update();
        }

        public void SetDstNode(StateNode dstNode)
        {
            if (dstNode == null)
                throw new ArgumentNullException("dstNode");
            this.mDstNode = dstNode;
            this.Update();
        }

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

        public float Weight
        {
            get { return 1; }
        }

        public static RectangleF AddEdgePath(
            StateNode srcNode, StateNode dstNode,
            GraphElement owner, GraphicsPath path)
        {
            SizeF srcPt = srcNode.ItemTranslate(owner);
            SizeF dstPt = dstNode.ItemTranslate(owner);

            double dx = dstPt.Width - srcPt.Width;
            double dy = dstPt.Height - srcPt.Height;

            double length = Math.Sqrt(dx * dx + dy * dy);
            dx = dx / length;
            dy = dy / length;

            double cos = Math.Cos(sAngle);
            double sin = Math.Sin(sAngle);

            double rad = srcNode.Radius;
            float srcX = srcPt.Width + (float)(rad * (cos * dx - sin * dy));
            float srcY = srcPt.Height + (float)(rad * (cos * dy + sin * dx));

            rad = dstNode.Radius;
            float dstX = dstPt.Width - (float)(rad * (cos * dx + sin * dy));
            float dstY = dstPt.Height - (float)(rad * (cos * dy - sin * dx));

            path.AddLine(srcX, srcY, dstX, dstY);

            float extra = 1 + 2 * sArrowSize;

            dx = Math.Abs(dstX - srcX) + extra;
            dy = Math.Abs(dstY - srcY) + extra;
            srcX = Math.Min(srcX, dstX) - extra / 2;
            srcY = Math.Min(srcY, dstY) - extra / 2;

            return new RectangleF(srcX, srcY, (float)dx, (float)dy);
        }

        public void Update()
        {
            SizeF srcPt = this.mSrcNode.ItemTranslate(this);
            SizeF dstPt = this.mDstNode.ItemTranslate(this);

            double dx = dstPt.Width - srcPt.Width;
            double dy = dstPt.Height - srcPt.Height;

            double length = Math.Sqrt(dx * dx + dy * dy);
            dx = dx / length;
            dy = dy / length;

            double cos = Math.Cos(sAngle);
            double sin = Math.Sin(sAngle);

            double rad = this.mSrcNode.Radius;
            this.mSrcX = srcPt.Width + (float)(rad * (cos * dx - sin * dy));
            this.mSrcY = srcPt.Height + (float)(rad * (cos * dy + sin * dx));

            rad = this.mDstNode.Radius;
            this.mDstX = dstPt.Width - (float)(rad * (cos * dx + sin * dy));
            this.mDstY = dstPt.Height - (float)(rad * (cos * dy - sin * dx));

            dx = this.mDstX - this.mSrcX;
            dy = this.mDstY - this.mSrcY;
            this.mLength = Math.Sqrt(dx * dx + dy * dy);

            this.BoundingBox = this.CalcBBox();
        }

        private RectangleF CalcBBox()
        {
            float extra = 1 + 2 * sArrowSize;

            float x = Math.Min(this.mSrcX, this.mDstX) - extra / 2;
            float y = Math.Min(this.mSrcY, this.mDstY) - extra / 2;
            float w = Math.Abs(this.mDstX - this.mSrcX) + extra;
            float h = Math.Abs(this.mDstY - this.mSrcY) + extra;

            return new RectangleF(x, y, w, h);
        }

        public override bool Contains(PointF point)
        {
            if (point.X < Math.Min(this.mSrcX, this.mDstX) ||
                point.X > Math.Max(this.mSrcX, this.mDstX) ||
                point.Y < Math.Min(this.mSrcY, this.mDstY) ||
                point.Y > Math.Max(this.mSrcY, this.mDstY))
            {
                return false;
            }
            // ||a x b|| = ||a|| ||b|| sin A = ||a|| ||b|| (d / ||a||)
            float cp = (point.X - this.mSrcX) * (this.mDstY - this.mSrcY)
                     - (point.Y - this.mSrcY) * (this.mDstX - this.mSrcX);
            return Math.Abs(cp / this.mLength) <= 2;
        }

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            if (!e.Handled)
            {
                StateMachineScene sms = this.Parent as StateMachineScene;
                sms.SelectStateEdge(this, 
                    (Control.ModifierKeys & Keys.Control) != Keys.Control);
            }
            return base.OnMouseDown(e);
        }

        //private static readonly double s13pi = Math.PI / 3;
        //private static readonly double s23pi = 2 * Math.PI / 3;

        protected override void OnDrawBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                // Draw the line itself
                sArrowPen.Color = this.bSelected 
                    ? sBorderSelectColor : sBorderNormalColor;
                g.DrawLine(sArrowPen, 
                    this.mSrcX, this.mSrcY, this.mDstX, this.mDstY);

                // Draw the arrows
                /*double sz = 2 * sArrowSize;
                double len = Math.Sqrt(
                    (this.mDstX - this.mSrcX) * (this.mDstX - this.mSrcX) +
                    (this.mDstY - this.mSrcY) * (this.mDstY - this.mSrcY));
                double a = Math.Acos((this.mDstX - this.mSrcX) / len);
                if ((this.mDstY - this.mSrcY) >= 0)
                    a = 2 * Math.PI - a;/* */

                /*PointF srcP1 = new PointF(this.mSrcX, this.mSrcY);
                PointF srcP2 = new PointF(
                    (float)(mSrcX + Math.Sin(a + s13pi) * sz),
                    (float)(mSrcY + Math.Cos(a + s13pi) * sz));
                PointF srcP3 = new PointF(
                    (float)(mSrcX + Math.Sin(a + s23pi) * sz),
                    (float)(mSrcY + Math.Cos(a + s23pi) * sz));/* */
                /*PointF dstP1 = new PointF(this.mDstX, this.mDstY);
                PointF dstP2 = new PointF(
                    (float)(mDstX + Math.Sin(a - s13pi) * sz),
                    (float)(mDstY + Math.Cos(a - s13pi) * sz));
                PointF dstP3 = new PointF(
                    (float)(mDstX + Math.Sin(a - s23pi) * sz),
                    (float)(mDstY + Math.Cos(a - s23pi) * sz));/* */
                //g.FillPolygon(Brushes.Black, 
                //    new PointF[] { srcP1, srcP2, srcP3 });
                //g.FillPolygon(Brushes.Black,
                //    new PointF[] { dstP1, dstP2, dstP3 });
            }
            catch (OverflowException)
            {
            }
            g.SmoothingMode = sm;
        }
    }
}
