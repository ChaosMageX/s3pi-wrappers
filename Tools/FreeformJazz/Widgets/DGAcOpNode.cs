﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3pi.GenericRCOLResource;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGAcOpNode : DGMulticastNode
    {
        private ActorOperationNode mAcOpNode;

        private GraphicsPath mBorderPath;

        private string mTextString;

        public DGAcOpNode(ActorOperationNode aon, StateMachineScene scene)
            : base(aon, scene)
        {
            if (aon == null)
            {
                throw new ArgumentNullException("aon");
            }
            this.mAcOpNode = aon;

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
            float w = 20;
            float h = 15;
            Graphics g = this.mScene.StateView.CreateGraphics();
            StringBuilder sb = new StringBuilder();

            ActorDefinition ad = this.mAcOpNode.Actor;
            sb.AppendLine("Ac: " + (ad == null ? "" : ad.Name));

            sb.Append("Op: ");
            switch (this.mAcOpNode.Operation)
            {
                case JazzActorOperationNode.ActorOperation.None:
                    sb.Append("None");
                    break;
                case JazzActorOperationNode.ActorOperation.SetMirror:
                    sb.Append(this.mAcOpNode.Operand 
                        ? "Set Mirror" : "Unset Mirror");
                    break;
            }

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
            g.FillPath(sBGBrush, this.mBorderPath);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            g.DrawPath(sBorderPen, this.mBorderPath);
            bbox.X = 2.5f;
            bbox.Y = 2.5f;
            bbox.Width = bbox.Width - bbp;
            bbox.Height = bbox.Height - bbp;
            g.DrawString(this.mTextString, sTextFont,
                sTextBrush, bbox, TextFormat);
            TextFormat.Alignment = sa;
        }
    }
}
