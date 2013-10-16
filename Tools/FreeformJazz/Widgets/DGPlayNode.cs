using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Resources;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGPlayNode : DGMulticastNode
    {
        private static readonly RK sZeroKey = new RK();

        private PlayAnimationNode mPlayNode;

        private GraphicsPath mBorderPath;

        /*private string mActorString;
        private string mPropsString;
        private string mBaseClipString;
        private string mTkMkString;
        private string mAddiClipString;/* */

        private string mTextString;

        public DGPlayNode(PlayAnimationNode pan, StateMachineScene scene)
            : base(pan, scene)
        {
            if (pan == null)
            {
                throw new ArgumentNullException("san");
            }
            this.mPlayNode = pan;

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
                    throw new ArgumentNullException("TextFont");
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
            string name;
            IResourceKey rk;

            Graphics g = this.mScene.StateView.CreateGraphics();
            StringBuilder sb = new StringBuilder();

            ActorDefinition ad = this.mPlayNode.Actor;
            /*this.mActorString = "Actor: " + (ad == null ? "" : ad.Name);
            //w = Math.Max(w, kAGW * this.mActorString.Length + 4);
            size = g.MeasureString(this.mActorString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.AppendLine("Actor: " + (ad == null ? "" : ad.Name));

            /*this.mPropsString = "Flags: " 
                + AnimFlagString(this.mPlayNode.Flags);
            //w = Math.Max(w, kAGW * this.mPropsString.Length + 4);
            size = g.MeasureString(this.mPropsString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.Append("Flags: ");
            sb.Append(AnimFlagString(this.mPlayNode.Flags));

            JazzAnimationFlags flags = this.mPlayNode.Flags;

            rk = this.mPlayNode.ClipKey;
            if (!string.IsNullOrEmpty(this.mPlayNode.ClipPattern))
            {
                /*this.mBaseClipString = "Base: " 
                    + this.mPlayNode.ClipPattern;
                //w = Math.Max(w, kAGW * this.mBaseClipString.Length + 4);
                size = g.MeasureString(this.mBaseClipString, TextFont);
                w = Math.Max(w, size.Width + 5);
                h += kMGH;/* */
                sb.AppendLine();
                sb.Append("Base: ");
                sb.Append(this.mPlayNode.ClipPattern);

                if ((flags & kBaseClipMask)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social | Object Only)");
                }
                else if (
                    (flags & JazzAnimationFlags.BaseClipIsSocial)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social)");
                }
                else if (
                    (flags & JazzAnimationFlags.BaseClipIsObjectOnly)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Object Only)");
                }
            }
            else if (rk != null && !rk.Equals(sZeroKey) && rk.Instance != 0)
            {
                /*this.mBaseClipString = "Base: " + rk.ToString();
                if (KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    this.mBaseClipString += string.Concat(" ", name);
                }
                //w = Math.Max(w, kAGW * this.mBaseClipString.Length + 4);
                size = g.MeasureString(this.mBaseClipString, TextFont);
                w = Math.Max(w, size.Width + 5);
                h += kMGH;/* */
                sb.AppendLine();
                sb.Append("Base: ");
                if (KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else if (KeyNameReg.TryFindGenCLIP(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else if (KeyNameReg.TryFindLabel(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else
                {
                    sb.Append("0x" + rk.Instance.ToString("X16"));
                }
                if ((flags & kBaseClipMask)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social | Object Only)");
                }
                else if (
                    (flags & JazzAnimationFlags.BaseClipIsSocial)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social)");
                }
                else if (
                    (flags & JazzAnimationFlags.BaseClipIsObjectOnly)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Object Only)");
                }
            }
            /*else
            {
                this.mBaseClipString = null;
            }/* */

            rk = this.mPlayNode.TrackMaskKey;
            if (rk != null && !rk.Equals(sZeroKey) && rk.Instance != 0)
            {
                /*this.mTkMkString = "TkMk: " + rk.ToString();
                if (KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    this.mTkMkString += string.Concat(" ", name);
                }
                //w = Math.Max(w, kAGW * this.mTkMkString.Length + 4);
                size = g.MeasureString(this.mTkMkString, TextFont);
                w = Math.Max(w, size.Width + 5);
                h += kMGH;/* */
                sb.AppendLine();
                sb.Append("TkMk: ");
                if (KeyNameReg.TryFindNameOrLabel(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else
                {
                    sb.Append("0x" + rk.Instance.ToString("X16"));
                }
            }
            /*else
            {
                this.mTkMkString = null;
            }/* */

            rk = this.mPlayNode.AdditiveClipKey;
            if (!string.IsNullOrEmpty(this.mPlayNode.AdditiveClipPattern))
            {
                /*this.mAddiClipString = "Addi: " 
                    + this.mPlayNode.AdditiveClipPattern;
                //w = Math.Max(w, kAGW * this.mAddiClipString.Length + 4);
                size = g.MeasureString(this.mAddiClipString, TextFont);
                w = Math.Max(w, size.Width + 5);
                h += kMGH;/* */
                sb.AppendLine();
                sb.Append("Addi: ");
                sb.Append(this.mPlayNode.AdditiveClipPattern);

                if ((flags & kAdditiveClipMask)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social | Object Only)");
                }
                else if (
                    (flags & JazzAnimationFlags.AdditiveClipIsSocial)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social)");
                }
                else if (
                    (flags & JazzAnimationFlags.AdditiveClipIsObjectOnly)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Object Only)");
                }
            }
            else if (rk != null && !rk.Equals(sZeroKey) && rk.Instance != 0)
            {
                /*this.mAddiClipString = "Addi: " + rk.ToString();
                if (KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    this.mAddiClipString += string.Concat(" ", name);
                }
                //w = Math.Max(w, kAGW * this.mAddiClipString.Length + 4);
                size = g.MeasureString(this.mAddiClipString, TextFont);
                w = Math.Max(w, size.Width + 5);
                h += kMGH;/* */
                sb.AppendLine();
                sb.Append("Addi: ");
                if (KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else if (KeyNameReg.TryFindGenCLIP(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else if (KeyNameReg.TryFindLabel(rk.Instance, out name))
                {
                    sb.Append(name);
                }
                else
                {
                    sb.Append("0x" + rk.Instance.ToString("X16"));
                }
                if ((flags & kAdditiveClipMask)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social | Object Only)");
                }
                else if (
                    (flags & JazzAnimationFlags.AdditiveClipIsSocial)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Social)");
                }
                else if (
                    (flags & JazzAnimationFlags.AdditiveClipIsObjectOnly)
                    != JazzAnimationFlags.TimingNormal)
                {
                    sb.Append(" (Object Only)");
                }
            }
            /*else
            {
                this.mAddiClipString = null;
            }/* */

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
            /*bbox.Height = kMGH;
            g.DrawString(this.mActorString, TextFont,
                TextBrush, bbox, TextFormat);
            bbox.Y = bbox.Y + kMGH;
            g.DrawString(this.mPropsString, TextFont,
                TextBrush, bbox, TextFormat);
            if (this.mBaseClipString != null)
            {
                bbox.Y = bbox.Y + kMGH;
                g.DrawString(this.mBaseClipString, TextFont, 
                    TextBrush, bbox, TextFormat);
            }
            if (this.mAddiClipString != null)
            {
                bbox.Y = bbox.Y + kMGH;
                g.DrawString(this.mAddiClipString, TextFont,
                    TextBrush, bbox, TextFormat);
            }
            if (this.mTkMkString != null)
            {
                bbox.Y = bbox.Y + kMGH;
                g.DrawString(this.mTkMkString, TextFont,
                    TextBrush, bbox, TextFormat);
            }/* */
            bbox.Height = bbox.Height - bbp;
            g.DrawString(this.mTextString, sTextFont, 
                sTextBrush, bbox, TextFormat);
            TextFormat.Alignment = sa;
        }
    }
}
