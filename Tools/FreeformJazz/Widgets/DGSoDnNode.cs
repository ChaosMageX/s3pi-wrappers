using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGSoDnNode : DGNode
    {
        private SelectOnDestinationNode mSoDn;

        private GraphicsPath mBorderPath;

        private AnchorPoint[] mCaseAnchors;
        private string[] mCaseStrings;
        private int mCaseCount;
        private float mMGH;

        public DGSoDnNode(SelectOnDestinationNode sodn, StateMachineScene scene)
            : base(sodn, scene)
        {
            if (sodn == null)
            {
                throw new ArgumentNullException("sodn");
            }
            this.mSoDn = sodn;

            this.mCaseAnchors = new AnchorPoint[0];
            this.mCaseStrings = new string[0];
            this.mCaseCount = 0;
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
            if (this.mScene.StateView == null)
            {
                return;
            }
            SizeF size;
            float h, w = 40;
            Graphics g = this.mScene.StateView.CreateGraphics();
            this.mCaseCount = this.mSoDn.CaseCount;
            this.mMGH = 7;
            if (this.mCaseCount == 0)
            {
                h = 15;
            }
            else
            {
                int i;
                AnchorPoint ap;
                State caseValue;
                string caseString;
                SelectOnDestinationNode.Case[] cases = this.mSoDn.Cases;
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
                    caseValue = cases[i].Value;
                    caseString = caseValue == null ? null : caseValue.Name;
                    this.mCaseStrings[i] = caseString;
                    if (!string.IsNullOrEmpty(caseString))
                    {
                        size = g.MeasureString(caseString, sTextFont);
                        w = Math.Max(w, size.Width + 5);
                        this.mMGH = Math.Max(this.mMGH, size.Height);
                    }
                    if (this.mCaseAnchors[i] == null)
                    {
                        ap = new AnchorPoint(this, 1, 0);
                        this.mCaseAnchors[i] = ap;
                    }
                }
                h = 2.5f;
                for (i = 0; i < this.mCaseCount; i++)
                {
                    h += this.mMGH / 2;
                    ap = this.mCaseAnchors[i];
                    ap.SetPosition(w, h);
                    h += this.mMGH / 2;
                }
                h = this.mMGH * this.mCaseCount + 5;
            }

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
            if (this.mCaseCount > 0)
            {
                float bbp = 4 * AnchorPoint.Radius + 5;
                string caseString;
                RectangleF bbox = this.BoundingBox;
                bbox.X = 2.5f;
                bbox.Y = 2.5f;
                bbox.Width = bbox.Width - bbp;
                bbox.Height = this.mMGH;
                for (int i = 0; i < this.mCaseCount; i++)
                {
                    caseString = this.mCaseStrings[i];
                    if (!string.IsNullOrEmpty(caseString))
                    {
                        g.DrawString(caseString, sTextFont,
                            sTextBrush, bbox, TextFormat);
                    }
                    bbox.Y = bbox.Y + this.mMGH;
                }
            }
        }
    }
}
