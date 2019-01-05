using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3pi.GenericRCOLResource;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGStopNode : DGAnimNode
    {
        private StopAnimationNode mStopNode;

        private GraphicsPath mBorderPath;

        //private string mActorString;
        //private string mPropsString;

        private string mTextString;

        public DGStopNode(StopAnimationNode san, StateNode state)
            : base(san, state, false)
        {
            if (san == null)
            {
                throw new ArgumentNullException("san");
            }
            this.mStopNode = san;

            this.UpdateVisualization();
        }

        #region Visualization

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
            float w = 40;
            float h = 15;

            Graphics g = this.mScene.StateView.CreateGraphics();
            StringBuilder sb = new StringBuilder();

            ActorDefinition ad = this.mStopNode.Actor;
            /*this.mActorString = "Actor: " + (ad == null ? "" : ad.Name);
            //w = Math.Max(w, kAGW * this.mActorString.Length + 4);
            size = g.MeasureString(this.mActorString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.AppendLine("Actor: " + (ad == null ? "" : ad.Name));

            /*this.mPropsString = "Flags: " 
                + AnimFlagString(this.mStopNode.Flags);
            //w = Math.Max(w, kAGW * this.mPropsString.Length + 4);
            size = g.MeasureString(this.mPropsString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.Append("Flags: " + AnimFlagString(this.mStopNode.Flags));

            this.mTextString = sb.ToString();
            size = g.MeasureString(this.mTextString, sTextFont);
            w = Math.Max(w, size.Width + 5);
            h = Math.Max(h, size.Height + 5);

            this.mEntryAnchor.SetPosition(0, h / 2);
            this.mTargetAnchor.SetPosition(w, h / 2);

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

        private static readonly Pen sBorderPen = new Pen(Color.Black, 0.5f);

        private static SolidBrush sBGBrush
            = new SolidBrush(Color.DarkOrange);

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
            float bbp = 4 * AnchorPoint.Radius + 5;
            Graphics g = e.Graphics;
            RectangleF bbox = this.BoundingBox;
            StringAlignment sa = TextFormat.Alignment;
            TextFormat.Alignment = StringAlignment.Near;
            g.FillPath(Brushes.DarkOrange, this.mBorderPath);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            g.DrawPath(sBorderPen, this.mBorderPath);
            bbox.X = 2.5f;
            bbox.Y = 2.5f;
            bbox.Width = bbox.Width - bbp;
            /*bbox.Height = kMGH;
            g.DrawString(this.mActorString, TextFont,
                TextBrush, bbox, TextFormat);
            bbox.Y = bbox.Y + kMGH;
            g.DrawString(this.mPropsString, TextFont,
                TextBrush, bbox, TextFormat);/* */
            bbox.Height = bbox.Height - bbp;
            g.DrawString(this.mTextString, sTextFont,
                sTextBrush, bbox, TextFormat);
            TextFormat.Alignment = sa;
        }

        #endregion
    }
}
