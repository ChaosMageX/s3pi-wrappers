using System;
using System.Collections.Generic;
using System.Text;
using s3pi.GenericRCOLResource;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public abstract class DGMulticastNode : DGNode
    {
        public const JazzAnimationFlags kMirrorMask
            = JazzAnimationFlags.Mirror
            | JazzAnimationFlags.OverrideMirror;
        public const JazzAnimationFlags kTimingPriorityMask
            = JazzAnimationFlags.UseTimingPriority
            | JazzAnimationFlags.UseTimingPriorityAsClockMaster;
        public const JazzAnimationFlags kBaseClipMask
            = JazzAnimationFlags.BaseClipIsSocial
            | JazzAnimationFlags.BaseClipIsObjectOnly;
        public const JazzAnimationFlags kAdditiveClipMask
            = JazzAnimationFlags.AdditiveClipIsSocial
            | JazzAnimationFlags.AdditiveClipIsObjectOnly;

        public static string AnimFlagString(JazzAnimationFlags flags)
        {
            if (flags == JazzAnimationFlags.TimingNormal)
            {
                return "Timing Normal";
            }
            StringBuilder fsb = new StringBuilder();
            if ((flags & JazzAnimationFlags.TimingMask)
                != JazzAnimationFlags.TimingNormal)
            {
                if ((flags & JazzAnimationFlags.TimingMaster)
                    == JazzAnimationFlags.TimingMaster)
                {
                    fsb.Append("Timing Master");
                }
                else if ((flags & JazzAnimationFlags.TimingSlave)
                    == JazzAnimationFlags.TimingSlave)
                {
                    fsb.Append("Timing Slave");
                }
                else
                {
                    fsb.Append("Timing Ignored");
                }
            }
            else
            {
                fsb.Append("Timing Normal");
            }
            if ((flags & JazzAnimationFlags.AtEnd)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | At End");
            }
            if ((flags & JazzAnimationFlags.LoopAsNeeded)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Loop As Needed");
            }
            if ((flags & JazzAnimationFlags.OverridePriority)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Override Priority");
            }
            if ((flags & kMirrorMask)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | (Override) Mirror");
            }
            else if ((flags & JazzAnimationFlags.Mirror)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Mirror");
            }
            else if ((flags & JazzAnimationFlags.OverrideMirror)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Override Mirror");
            }
            if ((flags & JazzAnimationFlags.Interruptible)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Interruptible");
            }
            if ((flags & JazzAnimationFlags.ForceBlend)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Force Blend");
            }
            if ((flags & kTimingPriorityMask)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Use Timing Priority (As Clock Master)");
            }
            else
                if ((flags & JazzAnimationFlags.UseTimingPriority)
                    == JazzAnimationFlags.UseTimingPriority)
                {
                    fsb.Append(" | Use Timing Priority");
                }
                else
                    if ((flags & JazzAnimationFlags.UseTimingPriorityAsClockMaster)
                        != JazzAnimationFlags.TimingNormal)
                    {
                        fsb.Append(" | Use Timing Priority As Clock Master");
                    }
            if ((flags & JazzAnimationFlags.HoldPose)
                != JazzAnimationFlags.HoldPose)
            {
                fsb.Append(" | Hold Pose");
            }
            if ((flags & JazzAnimationFlags.BlendMotionAccumulation)
                != JazzAnimationFlags.TimingNormal)
            {
                fsb.Append(" | Blend Motion Accumulation");
            }
            return fsb.ToString();
        }

        private MulticastDecisionGraphNode mMcDgn;

        protected readonly AnchorPoint mTargetAnchor;

        public DGMulticastNode(MulticastDecisionGraphNode mcdgn,
            StateMachineScene scene)
            : base(mcdgn, scene)
        {
            this.mMcDgn = mcdgn;
            this.mTargetAnchor = new AnchorPoint(this, 1, 0);
        }

        public AnchorPoint TargetAnchor
        {
            get { return this.mTargetAnchor; }
        }

        public override AnchorPoint GetAnchorFor(DGEdge edge)
        {
            if (this.mEntryAnchor.Edges.Contains(edge))
            {
                return this.mEntryAnchor;
            }
            if (this.mTargetAnchor.Edges.Contains(edge))
            {
                return this.mTargetAnchor;
            }
            return null;
        }
    }
}
