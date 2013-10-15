using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGRandNode : DGNode
    {
        private RandomNode mRandomNode;

        private GraphicsPath mBorderPath;

        private AnchorPoint[] mSliceAnchors;
        private string[] mSliceStrings;
        private int mSliceCount;
        private float mMGH;

        public DGRandNode(RandomNode rand, StateMachineScene scene)
            : base(rand, scene)
        {
            if (rand == null)
            {
                throw new ArgumentNullException("rand");
            }
            this.mRandomNode = rand;

            this.mSliceAnchors = new AnchorPoint[0];
            this.mSliceStrings = new string[0];
            this.mSliceCount = 0;
            this.mMGH = 7;

            this.UpdateVisualization();
        }

        private static Font sTextFont 
            = new Font(FontFamily.GenericSansSerif, 5);

        public static Font TextFont
        {
            get { return sTextFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Font");
                }
                if (!sTextFont.Equals(value))
                {
                    sTextFont = value;
                }
            }
        }

        public override void UpdateVisualization()
        {
            SizeF size;
            float h, w = 10;
            Graphics g = this.mScene.StateView.CreateGraphics();
            this.mSliceCount = this.mRandomNode.SliceCount;
            this.mMGH = 7;
            if (this.mSliceCount == 0)
            {
                h = 15;
            }
            else
            {
                int i;
                AnchorPoint ap;
                string sliceString;
                RandomNode.Slice[] slices = this.mRandomNode.Slices;
                if (this.mSliceAnchors.Length < this.mSliceCount)
                {
                    AnchorPoint[] sAnchors 
                        = new AnchorPoint[this.mSliceCount];
                    Array.Copy(this.mSliceAnchors, 0, sAnchors, 0,
                        this.mSliceAnchors.Length);
                    this.mSliceAnchors = sAnchors;
                    this.mSliceStrings = new string[this.mSliceCount];
                }
                for (i = 0; i < this.mSliceCount; i++)
                {
                    sliceString = slices[i].Weight.ToString();
                    size = g.MeasureString(sliceString, sTextFont);
                    w = Math.Max(w, size.Width + 5);
                    this.mMGH = Math.Max(this.mMGH, size.Height);
                    this.mSliceStrings[i] = sliceString;
                }
                if (this.mSliceCount == 1)
                {
                    h = this.mMGH + 5;
                    this.mSliceStrings[0] 
                        = slices[0].Weight.ToString();
                    if (this.mSliceAnchors[0] == null)
                    {
                        ap = new AnchorPoint(this, 1, 0);
                        ap.SetPosition(w, h / 2);
                        this.mSliceAnchors[0] = ap;
                    }
                }
                else
                {
                    this.mSliceStrings[0] 
                        = slices[0].Weight.ToString();
                    ap = this.mSliceAnchors[0];
                    if (ap == null)
                    {
                        ap = new AnchorPoint(this, 0, -1);
                        this.mSliceAnchors[0] = ap;
                    }
                    else
                    {
                        ap.SetDirection(0, -1);
                    }
                    ap.SetPosition(w / 2, 0);

                    h = this.mMGH * this.mSliceCount + 5;
                    this.mSliceCount--;
                    this.mSliceStrings[this.mSliceCount] 
                        = slices[this.mSliceCount].Weight.ToString();
                    ap = this.mSliceAnchors[this.mSliceCount];
                    if (ap == null)
                    {
                        ap = new AnchorPoint(this, 0, 1);
                        this.mSliceAnchors[this.mSliceCount] = ap;
                    }
                    else
                    {
                        ap.SetDirection(0, 1);
                    }
                    ap.SetPosition(w / 2, h);

                    h = this.mMGH + 2.5f;
                    for (i = 1; i < this.mSliceCount; i++)
                    {
                        this.mSliceStrings[i] 
                            = slices[i].Weight.ToString();
                        ap = this.mSliceAnchors[i];
                        if (ap == null)
                        {
                            ap = new AnchorPoint(this, 1, 0);
                            this.mSliceAnchors[i] = ap;
                        }
                        else
                        {
                            ap.SetDirection(1, 0);
                        }
                        h += this.mMGH / 2;
                        ap.SetPosition(w, h);
                        h += this.mMGH / 2;
                    }
                    this.mSliceCount++;
                    h = this.mMGH * this.mSliceCount + 5;
                }
            }

            this.mEntryAnchor.SetPosition(0, h / 2);

            if (this.mBorderPath == null)
            {
                this.mBorderPath = new GraphicsPath();
            }
            this.mBorderPath.Reset();
            AddRoundedRectangle(this.mBorderPath, 0, 0, w, h, 5);

            float bbp = 2 * AnchorPoint.Radius;
            this.BoundingBox = new RectangleF(
                -bbp, -bbp, w + 2 * bbp, h + 2 * bbp);
        }

        public AnchorPoint GetSliceAnchor(int index)
        {
            return this.mSliceAnchors[index];
        }

        public override AnchorPoint GetAnchorFor(DGEdge edge)
        {
            if (this.mEntryAnchor.Edges.Contains(edge))
            {
                return this.mEntryAnchor;
            }
            AnchorPoint ap;
            for (int i = this.mSliceCount - 1; i >= 0; i--)
            {
                ap = this.mSliceAnchors[i];
                if (ap.Edges.Contains(edge))
                {
                    return ap;
                }
            }
            return null;
        }

        public override Region Shape()
        {
            return new Region(this.mBorderPath);
        }

        private static readonly Pen sBorderPen = new Pen(Color.Black, 0.5f);

        private static SolidBrush sBGBrush
            = new SolidBrush(Color.Wheat);

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

        private static SolidBrush sTextBrush = new SolidBrush(Color.Black);

        public static Color TextColor
        {
            get { return sTextBrush.Color; }
            set
            {
                Color color = sTextBrush.Color;
                if (!color.Equals(value))
                {
                    sTextBrush.Color = value;
                }
            }
        }

        protected override void OnDrawBackground(
            System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillPath(sBGBrush, this.mBorderPath);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            g.DrawPath(sBorderPen, this.mBorderPath);
            if (this.mSliceCount > 0)
            {
                float bbp = 4 * AnchorPoint.Radius + 5;
                string sliceString;
                RectangleF rect = this.BoundingBox;
                rect.X = 2.5f;
                rect.Y = 2.5f;
                rect.Width = rect.Width - bbp;
                rect.Height = this.mMGH;
                for (int i = 0; i < this.mSliceCount; i++)
                {
                    sliceString = this.mSliceStrings[i];
                    if (!string.IsNullOrEmpty(sliceString))
                    {
                        g.DrawString(sliceString, sTextFont, 
                            sTextBrush, rect, TextFormat);
                    }
                    rect.Y = rect.Y + this.mMGH;
                }
            }
        }
    }
}
