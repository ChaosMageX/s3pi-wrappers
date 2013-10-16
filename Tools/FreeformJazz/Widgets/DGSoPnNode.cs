using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGSoPnNode : DGNode
    {
        private SelectOnParameterNode mSoPn;

        private GraphicsPath mBorderPath;
        private GraphicsPath mHeadPath;
        private GraphicsPath mBodyPath;

        private string mParamString;
        private AnchorPoint[] mCaseAnchors;
        private string[] mCaseStrings;
        private int mCaseCount;
        private float mHeadMGH;
        private float mBodyMGH;

        public DGSoPnNode(SelectOnParameterNode sopn, StateMachineScene scene)
            : base(sopn, scene)
        {
            if (sopn == null)
            {
                throw new ArgumentNullException("sopn");
            }
            this.mSoPn = sopn;

            this.mCaseAnchors = new AnchorPoint[0];
            this.mCaseStrings = new string[0];
            this.mCaseCount = 0;
            this.mHeadMGH = 7;
            this.mBodyMGH = 7;

            this.UpdateVisualization();
        }

        private static Font sHeadFont 
            = new Font(FontFamily.GenericSansSerif, 5);
        private static Font sBodyFont 
            = new Font(FontFamily.GenericSansSerif, 5);

        public static Font HeadFont
        {
            get { return sHeadFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Font");
                }
                if (!sHeadFont.Equals(value))
                {
                    sHeadFont = value;
                }
            }
        }

        public static Font BodyFont
        {
            get { return sBodyFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Font");
                }
                if (!sBodyFont.Equals(value))
                {
                    sBodyFont = value;
                }
            }
        }

        public override void UpdateVisualization()
        {
            if (this.mScene.StateView == null)
            {
                return;
            }
            SizeF size;
            float h, w;
            Graphics g = this.mScene.StateView.CreateGraphics();
            this.mHeadMGH = 7;
            this.mBodyMGH = 7;
            if (this.mSoPn.Parameter == null)
            {
                this.mParamString = null;
                w = 40;
            }
            else
            {
                this.mParamString = this.mSoPn.Parameter.Name;
                size = g.MeasureString(this.mParamString, sHeadFont);
                w = Math.Max(40, size.Width + 10);
                this.mHeadMGH = Math.Max(this.mHeadMGH, size.Height);
            }
            this.mCaseCount = this.mSoPn.CaseCount;
            if (this.mCaseCount == 0)
            {
                h = 0;
            }
            else
            {
                int i;
                AnchorPoint ap;
                string caseString;
                SelectOnParameterNode.Case[] cases = this.mSoPn.Cases;
                if (this.mCaseAnchors.Length < this.mCaseCount)
                {
                    AnchorPoint[] cAnchors 
                        = new AnchorPoint[this.mCaseCount];
                    Array.Copy(this.mCaseAnchors, 0, cAnchors, 0,
                        this.mCaseAnchors.Length);
                    this.mCaseAnchors = cAnchors;
                    this.mCaseStrings = new string[this.mCaseCount];
                }
                for (i = 0; i < this.mCaseCount; i++)
                {
                    caseString = cases[i].Value;
                    this.mCaseStrings[i] = caseString;
                    if (!string.IsNullOrEmpty(caseString))
                    {
                        size = g.MeasureString(caseString, sBodyFont);
                        w = Math.Max(w, size.Width + 5);
                        this.mBodyMGH = Math.Max(this.mBodyMGH, size.Height);
                    }
                    if (this.mCaseAnchors[i] == null)
                    {
                        ap = new AnchorPoint(this, 1, 0);
                        this.mCaseAnchors[i] = ap;
                    }
                }
                h = this.mHeadMGH + 2.5f;
                for (i = 0; i < this.mCaseCount; i++)
                {
                    h += this.mBodyMGH / 2;
                    ap = this.mCaseAnchors[i];
                    ap.SetPosition(w, h);
                    h += this.mBodyMGH / 2;
                }
                h = 10 * this.mCaseCount + 2.5f;
            }

            this.mEntryAnchor.SetPosition(0, 10);

            if (this.mBorderPath == null)
            {
                this.mBorderPath = new GraphicsPath();
                this.mHeadPath = new GraphicsPath();
                this.mBodyPath = new GraphicsPath();
            }
            this.mBorderPath.Reset();
            AddRoundedRectangle(this.mBorderPath, 
                0, 0, w, h + this.mHeadMGH + 2.5f, 5);
            this.mHeadPath.Reset();
            AddRoundedRectangle(this.mHeadPath, 
                0, 0, w, this.mHeadMGH + 2.5f, 5, RectCorners.Top);
            this.mBodyPath.Reset();
            AddRoundedRectangle(this.mBodyPath, 
                0, this.mHeadMGH + 2.5f, w, h, 5, RectCorners.Bottom);

            float bbp = 2 * AnchorPoint.Radius;
            this.BoundingBox = new RectangleF(-bbp, -bbp, 
                w + 2 * bbp, h + this.mHeadMGH + 5 + 2 * bbp);
        }

        public AnchorPoint GetCaseAnchorAt(int index)
        {
            return this.mCaseAnchors[index];
        }

        public override AnchorPoint GetAnchorFor(DGEdge edge)
        {
            if (this.mEntryAnchor.Edges.Contains(edge))
            {
                return this.mEntryAnchor;
            }
            AnchorPoint ap;
            for (int i = this.mCaseCount - 1; i >= 0; i--)
            {
                ap = this.mCaseAnchors[i];
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

        private static SolidBrush sHeadBrush = new SolidBrush(Color.Green);
        private static SolidBrush sBodyBrush = new SolidBrush(Color.White);

        public static Color HeadColor
        {
            get { return sHeadBrush.Color; }
            set
            {
                Color color = sHeadBrush.Color;
                if (!color.Equals(value))
                {
                    sHeadBrush.Color = value;
                }
            }
        }

        public static Color BodyColor
        {
            get { return sBodyBrush.Color; }
            set
            {
                Color color = sBodyBrush.Color;
                if (!color.Equals(value))
                {
                    sBodyBrush.Color = value;
                }
            }
        }

        private static SolidBrush sHeadTextBrush 
            = new SolidBrush(Color.Black);
        private static SolidBrush sBodyTextBrush
            = new SolidBrush(Color.Black);

        public static Color HeadTextColor
        {
            get { return sHeadTextBrush.Color; }
            set
            {
                Color color = sHeadTextBrush.Color;
                if (!color.Equals(value))
                {
                    sHeadTextBrush.Color = value;
                }
            }
        }

        public static Color BodyTextColor
        {
            get { return sBodyTextBrush.Color; }
            set
            {
                Color color = sBodyTextBrush.Color;
                if (!color.Equals(value))
                {
                    sBodyTextBrush.Color = value;
                }
            }
        }

        protected override void OnDrawBackground(
            System.Windows.Forms.PaintEventArgs e)
        {
            float bbp = 4 * AnchorPoint.Radius + 5;
            Graphics g = e.Graphics;
            g.FillPath(sHeadBrush, this.mHeadPath);
            g.FillPath(sBodyBrush, this.mBodyPath);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            g.DrawPath(sBorderPen, this.mBorderPath);
            RectangleF bbox = this.BoundingBox;
            bbox.X = 2.5f;
            bbox.Y = 2.5f;
            bbox.Width = bbox.Width - bbp;
            bbox.Height = this.mHeadMGH;
            if (!string.IsNullOrEmpty(this.mParamString))
            {
                g.DrawString(this.mParamString, sHeadFont,
                    sHeadTextBrush, bbox, TextFormat);
            }
            bbox.Y = bbox.Y + this.mHeadMGH;
            bbox.Height = this.mBodyMGH;
            if (this.mCaseCount > 0)
            {
                string caseString;
                
                for (int i = 0; i < this.mCaseCount; i++)
                {
                    caseString = this.mCaseStrings[i];
                    if (!string.IsNullOrEmpty(caseString))
                    {
                        g.DrawString(caseString, sBodyFont,
                            sBodyTextBrush, bbox, TextFormat);
                    }
                    bbox.Y = bbox.Y + this.mBodyMGH;
                }
            }
        }
    }
}
