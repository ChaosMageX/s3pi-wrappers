using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Resources;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGPropNode : DGMulticastNode
    {
        private CreatePropNode mPropNode;
        private RefToActor mPropActor;
        private RefToParam mPropParam;

        private GraphicsPath mBorderPath;

        /*private string mActorString;
        private string mParamString;
        private string mKeyString;/* */

        private string mTextString;

        public DGPropNode(CreatePropNode cpn, StateNode state)
            : base(cpn, state)
        {
            if (cpn == null)
            {
                throw new ArgumentNullException("cpn");
            }
            this.mPropNode = cpn;
            this.mPropActor = new RefToActor(this.mScene, cpn.PropActor);
            this.mPropParam = new RefToParam(this.mScene, cpn.PropParam);
            this.UpdateVisualization();
        }

        private class PropActorCommand : DGNodeRefPropertyCommand<
            DGPropNode, CreatePropNode, ActorDefinition>
        {
            public PropActorCommand(DGPropNode dgpn,
                ActorDefinition newValue, bool extendable)
                : base(dgpn, dgpn.mPropNode, dgpn.mPropActor, 
                "PropActor", newValue, extendable)
            {
                this.mLabel = "Set Create Prop Node Actor";
            }
        }

        private class PropParamCommand : DGNodeRefPropertyCommand<
            DGPropNode, CreatePropNode, ParamDefinition>
        {
            public PropParamCommand(DGPropNode dgpn,
                ParamDefinition newValue, bool extendable)
                : base(dgpn, dgpn.mPropNode, dgpn.mPropParam,
                "PropParameter", newValue, extendable)
            {
                this.mLabel = "Set Create Prop Node Parameter";
            }
        }

        private class PropKeyCommand
            : DGNodePropertyCommand<DGPropNode, CreatePropNode, RK>
        {
            public PropKeyCommand(DGPropNode dgpn,
                RK newValue, bool extendable)
                : base(dgpn, dgpn.mPropNode, 
                    "PropKey", newValue, extendable)
            {
                this.mLabel = "Set Create Prop Node Prop Key";
            }
        }

        public RefToActor PropActor
        {
            get { return this.mPropActor; }
            set
            {
                ActorDefinition ad
                    = value == null ? null : value.GetValue();
                if (this.mPropNode.PropActor != ad)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new PropActorCommand(this, ad, false));
                }
            }
        }

        public RefToParam PropParam
        {
            get { return this.mPropParam; }
            set
            {
                ParamDefinition pd
                    = value == null ? null : value.GetValue();
                if (this.mPropNode.PropParam != pd)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new PropParamCommand(this, pd, false));
                }
            }
        }

        public RK PropKey
        {
            get { return this.mPropNode.PropKey; }
            set
            {
                if (!this.mPropNode.PropKey.Equals(value))
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new PropKeyCommand(this, value, false));
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
            SizeF size;
            float w = 20;
            float h = 15;

            Graphics g = this.mScene.StateView.CreateGraphics();
            StringBuilder sb = new StringBuilder();
            
            ActorDefinition ad = this.mPropNode.PropActor;
            /*this.mActorString = "A: " + (ad == null ? "" : ad.Name);
            //w = Math.Max(w, kAGW * this.mActorString.Length + 4);
            size = g.MeasureString(this.mActorString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.AppendLine(string.Concat("A: ", (ad == null ? "" : ad.Name)));
            
            ParamDefinition pd = this.mPropNode.PropParam;
            /*this.mParamString = "P: " + (pd == null ? "" : pd.Name);
            //w = Math.Max(w, kAGW * this.mParamString.Length + 4);
            size = g.MeasureString(this.mParamString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.AppendLine(string.Concat("P: ", (pd == null ? "" : pd.Name)));

            IResourceKey rk = this.mPropNode.PropKey;
            /*this.mKeyString = "K: " + rk.ToString();
            string name;
            if (KeyNameReg.TryFindName(rk.Instance, out name))
            {
                this.mKeyString += string.Concat(" ", name);
            }
            //w = Math.Max(w, kAGW * this.mKeyString.Length + 4);
            size = g.MeasureString(this.mKeyString, TextFont);
            w = Math.Max(w, size.Width + 5);/* */
            sb.Append(string.Concat("K: ", rk.ToString()));
            string name;
            if (KeyNameReg.TryFindName(rk.Instance, out name))
            {
                sb.Append(string.Concat(" ", name));
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
            /*bbox.Height = kMGH;
            g.DrawString(this.mActorString, TextFont, 
                TextBrush, bbox, TextFormat);
            bbox.Y = bbox.Y + kMGH;
            g.DrawString(this.mParamString, TextFont,
                TextBrush, bbox, TextFormat);
            bbox.Y = bbox.Y + kMGH;
            g.DrawString(this.mKeyString, TextFont,
                TextBrush, bbox, TextFormat);/* */
            bbox.Height = bbox.Height - bbp;
            g.DrawString(this.mTextString, sTextFont,
                sTextBrush, bbox, TextFormat);
            TextFormat.Alignment = sa;
        }
    }
}
