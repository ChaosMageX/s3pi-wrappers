using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.JazzGraph
{
    public abstract class AnimationNode : MulticastDecisionGraphNode
    {
        private JazzAnimationFlags mFlags;
        private JazzChunk.AnimationPriority mPriority;
        private float mBlendInTime;
        private float mBlendOutTime;
        private float mSpeed;
        private ActorDefinition mActor;
        private JazzChunk.AnimationPriority mTimingPriority;

        public AnimationNode(uint nodeType, string nodeTag)
            : base(nodeType, nodeTag)
        {
            this.mFlags = JazzAnimationFlags.Default;
            this.mPriority = JazzChunk.AnimationPriority.Unset;
            this.mBlendInTime = 0;
            this.mBlendOutTime = 0;
            this.mSpeed = 1;
            this.mActor = null;
            this.mTimingPriority = JazzChunk.AnimationPriority.Unset;
        }

        public JazzAnimationFlags Flags
        {
            get { return this.mFlags; }
            set
            {
                if (this.mFlags != value)
                {
                    this.mFlags = value;
                }
            }
        }

        public void SetFlags(JazzAnimationFlags flags, bool value)
        {
            if (value)
            {
                this.mFlags |= flags;
            }
            else
            {
                this.mFlags &= ~flags;
            }
        }

        public JazzChunk.AnimationPriority Priority
        {
            get { return this.mPriority; }
            set
            {
                if (this.mPriority != value)
                {
                    this.mPriority = value;
                }
            }
        }

        public float BlendInTime
        {
            get { return this.mBlendInTime; }
            set
            {
                if (this.mBlendInTime != value)
                {
                    this.mBlendInTime = value;
                }
            }
        }

        public float BlendOutTime
        {
            get { return this.mBlendOutTime; }
            set
            {
                if (this.mBlendOutTime != value)
                {
                    this.mBlendOutTime = value;
                }
            }
        }

        public float Speed
        {
            get { return this.mSpeed; }
            set
            {
                if (this.mSpeed != value)
                {
                    this.mSpeed = value;
                }
            }
        }

        public ActorDefinition Actor
        {
            get { return this.mActor; }
            set
            {
                if (this.mActor != value)
                {
                    this.mActor = value;
                }
            }
        }

        public JazzChunk.AnimationPriority TimingPriority
        {
            get { return this.mTimingPriority; }
            set
            {
                if (this.mTimingPriority != value)
                {
                    this.mTimingPriority = value;
                }
            }
        }
    }
}
