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
    public class DGEdge 
        : GraphElement, IGraphEdge<DGNode>, IUpdateable, IEquatable<DGEdge>
    {
        private static readonly Pen sArrowPen;

        private static Color sBorderNormalColor = Color.Black;
        private static Color sBorderInEditColor = Color.Navy;
        private static Color sBorderSelectColor = Color.Yellow;

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

        static DGEdge()
        {
            sArrowPen = new Pen(Color.Black, 1);
            sArrowPen.StartCap = LineCap.RoundAnchor;
            //sArrowPen.EndCap = LineCap.ArrowAnchor;
            sArrowPen.CustomEndCap = new AdjustableArrowCap(3, 3);
            sArrowPen.LineJoin = LineJoin.Round;
        }

        private DGNode mSrcNode;
        private DGNode mDstNode;

        private bool bInEditMode;
        private bool bVisible;

        //private PointF mSrcPt;
        //private PointF mDstPt;

        private GraphicsPath mPath;

        public DGEdge(DGNode srcNode, DGNode dstNode)
        {
            if (srcNode == null)
                throw new ArgumentNullException("srcNode");
            if (dstNode == null)
                throw new ArgumentNullException("dstNode");
            this.mSrcNode = srcNode;
            this.mDstNode = dstNode;

            this.bInEditMode = false;
            this.bVisible = true;

            this.mPath = new GraphicsPath();

            //this.IgnoreMouseEvents = true;
            this.Zvalue = -1;
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
                    this.bVisible = value;
                    this.Invalidate();
                }
            }
        }

        private static readonly Pen sHitTestPen = new Pen(Color.Black, 4);

        public override bool Contains(PointF point)
        {
            return this.mPath.IsOutlineVisible(point, sHitTestPen);
        }

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            if (!e.Handled)
            {
                GraphElement sn = this.Parent;
                StateMachineScene sms = sn.Parent as StateMachineScene;
                sms.SelectDGEdge(this,
                    (Control.ModifierKeys & Keys.Control) != Keys.Control);
            }
            return base.OnMouseDown(e);
        }

        protected override bool IsDrawn()
        {
            return this.bVisible;
        }

        public DGNode SrcNode
        {
            get { return this.mSrcNode; }
        }

        public DGNode DstNode
        {
            get { return this.mDstNode; }
        }

        public void SetSrcNode(DGNode srcNode)
        {
            if (srcNode == null)
                throw new ArgumentNullException("srcNode");
            this.mSrcNode = srcNode;
            this.Update();
        }

        public void SetDstNode(DGNode dstNode)
        {
            if (dstNode == null)
                throw new ArgumentNullException("dstNode");
            this.mDstNode = dstNode;
            this.Update();
        }

        public float Weight
        {
            get { return 1; }
        }

        private static float sCurveSize = 25;

        public static float CurveSize
        {
            get { return sCurveSize; }
            set
            {
                if (sCurveSize != value)
                {
                    if (sCurveSize <= 0)
                    {
                        throw new ArgumentOutOfRangeException("CurveSize");
                    }
                    sCurveSize = value;
                }
            }
        }

        public void Update()
        {
            AnchorPoint srcAP = this.mSrcNode.GetAnchorFor(this);
            AnchorPoint dstAP = this.mDstNode.GetAnchorFor(this);
            if (srcAP != null && dstAP != null)
            {
                SizeF srcPt = srcAP.ItemTranslate(this);
                SizeF dstPt = dstAP.ItemTranslate(this);

                //this.mSrcPt = new PointF(srcPt.Width, srcPt.Height);
                //this.mDstPt = new PointF(dstPt.Width, dstPt.Height);

                float d1 = sCurveSize;

                float x1 = srcPt.Width;
                float y1 = srcPt.Height;
                float x2 = x1 + d1 * srcAP.DirectionX;
                float y2 = y1 + d1 * srcAP.DirectionY;

                float x4 = dstPt.Width;
                float y4 = dstPt.Height;
                float x3 = x4 + d1 * dstAP.DirectionX;
                float y3 = y4 + d1 * dstAP.DirectionY;

                if (x1 == x2 && x3 == x4 && x2 == x3 && 
                    Math.Abs(y4 - y1) < 2 * sCurveSize)
                {
                    // Src & Dst lines are both vertical and intersect
                    y2 = y3 = (y4 - y1) / 2;
                }
                else if (x1 != x2 || x3 != x4)
                {
                    if (x1 == x2 && 
                        ((x3 <= x1 && x1 <= x4) || (x4 <= x1 && x1 <= x3)))
                    {
                        // Src line is vertical
                        // y = d1 * x + b1 => b1 = y4 - d1 * x4
                        d1 = (y4 - y3) / (x4 - x3);
                        float y = d1 * (x1 - x4) + y4;
                        if ((y2 <= y && y <= y1) || (y1 <= y && y <= y2))
                        {
                            // Dst line intersects Src line at y 
                            y3 = y2 = y;
                            x3 = x2;
                        }
                    }
                    else if (x3 == x4 &&
                        ((x1 <= x4 && x4 <= x2) || (x2 <= x4 && x4 <= x1)))
                    {
                        // Dst line is vertical
                        // y = d1 * x + b1 => b1 = y1 - d1 * x1
                        d1 = (y2 - y1) / (x2 - x1);
                        float y = d1 * (x4 - x1) + y1;
                        if ((y4 <= y && y <= y3) || (y3 <= y && y <= y4))
                        {
                            // Src line intersects Dst line at y
                            y2 = y3 = y;
                            x2 = x3;
                        }
                    }
                    else
                    {
                        float d3;
                        // y = d1 * x + b1 => b1 = y1 - d1 * x1
                        d1 = (y2 - y1) / (x2 - x1);
                        // y = d3 * x + b3 => b3 = y3 - d3 * x3
                        d3 = (y4 - y3) / (x4 - x3);
                        if (d1 == d3 && d1 * (y4 - y1) == x1 - x4)
                        {
                            // Src & Dst lines are parallel and aligned
                            x2 = x3 = (x1 + x4) / 2;
                            y2 = y3 = (y1 + y4) / 2;
                        }
                        else
                        {
                            // d1 * x + b1 = d3 * x + b3
                            // x = (b1 - b3) / (d3 - d1)
                            float x = (d3 * x3 - d1 * x1 - y3 + y1) 
                                    / (d3 - d1);
                            float y = d1 * (x - x1) + y1;
                            if (x2 < x1)
                            {
                                d1 = x1;
                                x1 = x2;
                                x2 = d1;
                            }
                            if (y2 < y1)
                            {
                                d1 = y1;
                                y1 = y2;
                                y2 = d1;
                            }
                            if (x1 <= x && x <= x2 && y1 <= y && y <= y2)
                            {
                                // Src & Dst lines intersect at (x, y)
                                x2 = x3 = x;
                                y2 = y3 = y;
                            }
                            else
                            {
                                d1 = sCurveSize;
                                x2 = x1 + d1 * srcAP.DirectionX;
                                y2 = y1 + d1 * srcAP.DirectionY;
                            }
                            x1 = srcPt.Width;
                            y1 = srcPt.Height;
                        }
                    }
                }

                this.mPath.Reset();
                this.mPath.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);

                if (x2 < x1)
                {
                    d1 = x1;
                    x1 = x2;
                    x2 = d1;
                }
                x1 = Math.Min(x1, x3);
                x1 = Math.Min(x1, x4);
                x2 = Math.Max(x2, x3);
                x2 = Math.Max(x2, x4);

                if (y2 < y1)
                {
                    d1 = y1;
                    y1 = y2;
                    y2 = d1;
                }
                y1 = Math.Min(y1, y3);
                y1 = Math.Min(y1, y4);
                y2 = Math.Max(y2, y3);
                y2 = Math.Max(y2, y4);

                d1 = 4;

                this.BoundingBox = new RectangleF(
                    x1 - d1 / 2, y1 - d1 / 2, x2 - x1 + d1, y2 - y1 + d1);
            }
            //this.BoundingBox = this.CalcBBox();
        }

        /*private RectangleF CalcBBox()
        {
            float extra = 4;

            float x = Math.Min(mSrcPt.X, mDstPt.X) - extra / 2;
            float y = Math.Min(mSrcPt.Y, mDstPt.Y) - extra / 2;
            float w = Math.Abs(mDstPt.X - mSrcPt.X) + extra;
            float h = Math.Abs(mDstPt.Y - mSrcPt.Y) + extra;

            return new RectangleF(x, y, w, h);
        }/* */

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

        protected override void OnDrawBackground(
            System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                // Draw the line itself
                sArrowPen.Color = this.bSelected
                    ? sBorderSelectColor : (this.bInEditMode
                    ? sBorderInEditColor : sBorderNormalColor);
                g.DrawPath(sArrowPen, this.mPath);
            }
            catch (OverflowException)
            {
            }
            g.SmoothingMode = sm;
        }

        public bool Equals(DGEdge other)
        {
            return other != null &&
                other.mSrcNode.Equals(this.mSrcNode) &&
                other.mDstNode.Equals(this.mDstNode);
        }
    }
}
