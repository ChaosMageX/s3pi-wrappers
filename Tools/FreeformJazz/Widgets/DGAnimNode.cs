using System;
using s3pi.GenericRCOLResource;
using s3piwrappers.Helpers;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public abstract class DGAnimNode : DGMulticastNode
    {
        private AnimationNode mAnimNode;
        private RefToActor mActor;
        private bool bPlay;

        public DGAnimNode(AnimationNode an, StateNode state, bool play)
            : base(an, state)
        {
            this.mAnimNode = an;
            this.mActor = new RefToActor(state.Scene, 
                an == null ? null : an.Actor);
            this.bPlay = play;
        }

        private abstract class AnimNodePropertyCommand<P>
            : DGNodePropertyCommand<DGAnimNode, AnimationNode, P>
        {
            public AnimNodePropertyCommand(DGAnimNode dgan, 
                string property, P newValue, bool extendable)
                : base(dgan, dgan.mAnimNode, 
                    property, newValue, extendable)
            {
                this.mLabel = "Set " + 
                    (dgan.bPlay ? "Play" : "Stop") + " Animation Node ";
            }
        }

        private class FlagsCommand 
            : AnimNodePropertyCommand<JazzAnimationFlags>
        {
            public FlagsCommand(DGAnimNode dgan,
                JazzAnimationFlags newValue, bool extendable)
                : base(dgan, "Flags", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Flags";
            }
        }

        private class PriorityCommand 
            : AnimNodePropertyCommand<JazzChunk.AnimationPriority>
        {
            public PriorityCommand(DGAnimNode dgan,
                JazzChunk.AnimationPriority newValue, bool extendable)
                : base(dgan, "Priority", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Priority";
            }
        }

        private class BlendInTimeCommand : AnimNodePropertyCommand<float>
        {
            public BlendInTimeCommand(DGAnimNode dgan,
                float newValue, bool extendable)
                : base(dgan, "BlendInTime", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Blend In Time";
            }
        }

        private class BlendOutTimeCommand : AnimNodePropertyCommand<float>
        {
            public BlendOutTimeCommand(DGAnimNode dgan,
                float newValue, bool extendable)
                : base(dgan, "BlendOutTime", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Blend Out Time";
            }
        }

        private class SpeedCommand : AnimNodePropertyCommand<float>
        {
            public SpeedCommand(DGAnimNode dgan,
                float newValue, bool extendable)
                : base(dgan, "Speed", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Speed";
            }
        }

        /*private class ActorCommand : JazzCommand
        {
            private DGAnimNode mDGAN;
            private ActorDefinition mOldVal;
            private ActorDefinition mNewVal;
            private bool bExtendable;

            public ActorCommand(DGAnimNode dgan, 
                ActorDefinition newValue, bool extendable)
                : base(dgan.mScene.Container)
            {
                this.mDGAN = dgan;
                this.mOldVal = dgan.mAnimNode.Actor;
                this.mNewVal = newValue;
                this.bExtendable = extendable;
                this.mLabel = "Set " +
                    (dgan.bPlay ? "Play" : "Stop") + " Animation Node Actor";
            }

            public override bool Execute()
            {
                this.mDGAN.mAnimNode.Actor = this.mNewVal;
                this.mDGAN.mActor.SetValue(this.mNewVal);
                this.mDGAN.UpdateVisualization();
                return true;
            }

            public override void Undo()
            {
                this.mDGAN.mAnimNode.Actor = this.mOldVal;
                this.mDGAN.mActor.SetValue(this.mOldVal);
                this.mDGAN.UpdateVisualization();
            }

            public override bool IsExtendable(Command possibleExt)
            {
                if (!this.bExtendable)
                {
                    return false;
                }
                ActorCommand ac = possibleExt as ActorCommand;
                if (ac == null || ac.mContainer != this.mContainer ||
                    ac.mDGAN != this.mDGAN || ac.mNewVal == this.mOldVal)
                {
                    return false;
                }
                return true;
            }

            public override void Extend(Command possibleExt)
            {
                ActorCommand ac = possibleExt as ActorCommand;
                this.mNewVal = ac.mNewVal;
            }
        }/* */

        private class ActorCommand : DGNodeRefPropertyCommand<
            DGAnimNode, AnimationNode, ActorDefinition>
        {
            public ActorCommand(DGAnimNode dgan,
                ActorDefinition newValue, bool extendable)
                : base(dgan, dgan.mAnimNode, dgan.mActor, 
                "Actor", newValue, extendable)
            {
                this.mLabel = "Set " +
                    (dgan.bPlay ? "Play" : "Stop") + " Animation Node Actor";
            }
        }

        private class TimingPriorityCommand
            : AnimNodePropertyCommand<JazzChunk.AnimationPriority>
        {
            public TimingPriorityCommand(DGAnimNode dgan,
                JazzChunk.AnimationPriority newValue, bool extendable)
                : base(dgan, "TimingPriority", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Priority";
            }
        }

        public JazzAnimationFlags Flags
        {
            get { return this.mAnimNode.Flags; }
            set
            {
                if (this.mAnimNode.Flags != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new FlagsCommand(this, value, false));
                }
            }
        }

        public JazzChunk.AnimationPriority Priority
        {
            get { return this.mAnimNode.Priority; }
            set
            {
                if (this.mAnimNode.Priority != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new PriorityCommand(this, value, false));
                }
            }
        }

        public float BlendInTime
        {
            get { return this.mAnimNode.BlendInTime; }
            set
            {
                if (this.mAnimNode.BlendInTime != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new BlendInTimeCommand(this, value, false));
                }
            }
        }

        public float BlendOutTime
        {
            get { return this.mAnimNode.BlendOutTime; }
            set
            {
                if (this.mAnimNode.BlendOutTime != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new BlendOutTimeCommand(this, value, false));
                }
            }
        }

        public float Speed
        {
            get { return this.mAnimNode.Speed; }
            set
            {
                if (this.mAnimNode.Speed != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new SpeedCommand(this, value, false));
                }
            }
        }

        public RefToActor Actor
        {
            get { return this.mActor; }
            set
            {
                if (this.mAnimNode == null)
                {
                    if (value != null)
                    {
                        this.mActor = new RefToActor(
                            this.mScene, value.GetValue());
                    }
                }
                else
                {
                    ActorDefinition ad 
                        = value == null ? null : value.GetValue();
                    if (this.mAnimNode.Actor != ad)
                    {
                        this.mScene.Container.UndoRedo.Submit(
                            new ActorCommand(this, ad, false));
                    }
                }
            }
        }

        public JazzChunk.AnimationPriority TimingPriority
        {
            get { return this.mAnimNode.TimingPriority; }
            set
            {
                if (this.mAnimNode.TimingPriority != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new TimingPriorityCommand(this, value, false));
                }
            }
        }
    }
}
