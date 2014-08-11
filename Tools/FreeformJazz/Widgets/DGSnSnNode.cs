using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGSnSnNode : DGNode
    {
        private NextStateNode mNextStateNode;
        private RefToState mNextState;
        
        private string mNextStateName;
        private GraphicsPath mBorderPath;

        public DGSnSnNode(NextStateNode nsn, StateNode state)
            : base(nsn, state)
        {
            if (nsn == null)
            {
                throw new ArgumentNullException("nsn");
            }
            this.mNextStateNode = nsn;
            this.mNextState = new RefToState(this.mScene, nsn.NextState);
            this.UpdateVisualization();
        }

        private class NextStateCommand : DGNodeRefPropertyCommand<
            DGSnSnNode, NextStateNode, State>
        {
            public NextStateCommand(DGSnSnNode dgsn,
                State newValue, bool extendable)
                : base(dgsn, dgsn.mNextStateNode, dgsn.mNextState, 
                "NextState", newValue, extendable)
            {
                this.mLabel = "Set Next State Node Reference";
            }
        }

        public RefToState NextState
        {
            get { return this.mNextState; }
            set
            {
                State state = value == null ? null : value.GetValue();
                if (this.mNextStateNode.NextState != state)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new NextStateCommand(this, state, false));
                }
            }
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
            float w, h;
            SizeF size;
            Graphics g = this.mScene.StateView.CreateGraphics();
            State state = this.mNextStateNode.NextState;
            if (state != null)
            {
                this.mNextStateName = state.Name;
                if (!string.IsNullOrEmpty(this.mNextStateName))
                {
                    //w = kAGW * this.mNextStateName.Length + 10;
                    size = g.MeasureString(this.mNextStateName, sTextFont);
                    w = Math.Max(40, size.Width + 5);
                    h = size.Height + 5;
                }
                else
                {
                    w = 40;
                    h = 10;
                }
            }
            else
            {
                w = 40;
                h = 10;
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
            if (!string.IsNullOrEmpty(this.mNextStateName))
            {
                float bbp = 4 * AnchorPoint.Radius + 5;
                RectangleF bbox = this.BoundingBox;
                bbox.X = 2.5f;
                bbox.Y = 2.5f;
                bbox.Width = bbox.Width - bbp;
                bbox.Height = bbox.Height - bbp;
                g.DrawString(this.mNextStateName, sTextFont, 
                    sTextBrush, bbox, TextFormat);
            }
        }
    }
}
