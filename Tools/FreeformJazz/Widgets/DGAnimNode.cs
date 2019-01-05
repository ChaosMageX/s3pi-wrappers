using System;
using s3pi.GenericRCOLResource;
using s3piwrappers.Helpers.Undo;
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
            : PropertyCommand<DGAnimNode, P>
        {
            public AnimNodePropertyCommand(DGAnimNode dgan, 
                string property, P newValue)
                : base(dgan, property, newValue, false)
            {
                this.mLabel = "Set " + 
                    (dgan.bPlay ? "Play" : "Stop") + " Animation Node ";
            }
        }

        private class FlagsCommand 
            : AnimNodePropertyCommand<JazzAnimationFlags>
        {
            public FlagsCommand(DGAnimNode dgan,
                JazzAnimationFlags newValue)
                : base(dgan, "Flags", newValue)
            {
                this.mLabel = this.mLabel + "Flags";
            }
        }

        private class PriorityCommand 
            : AnimNodePropertyCommand<JazzChunk.AnimationPriority>
        {
            public PriorityCommand(DGAnimNode dgan,
                JazzChunk.AnimationPriority newValue)
                : base(dgan, "Priority", newValue)
            {
                this.mLabel = this.mLabel + "Priority";
            }
        }

        private class BlendInTimeCommand : AnimNodePropertyCommand<float>
        {
            public BlendInTimeCommand(DGAnimNode dgan, float newValue)
                : base(dgan, "BlendInTime", newValue)
            {
                this.mLabel = this.mLabel + "Blend In Time";
            }
        }

        private class BlendOutTimeCommand : AnimNodePropertyCommand<float>
        {
            public BlendOutTimeCommand(DGAnimNode dgan, float newValue)
                : base(dgan, "BlendOutTime", newValue)
            {
                this.mLabel = this.mLabel + "Blend Out Time";
            }
        }

        private class SpeedCommand : AnimNodePropertyCommand<float>
        {
            public SpeedCommand(DGAnimNode dgan, float newValue)
                : base(dgan, "Speed", newValue)
            {
                this.mLabel = this.mLabel + "Speed";
            }
        }

        private class ActorCommand : PropertyCommand<DGAnimNode, RefToActor>
        {
            public ActorCommand(DGAnimNode dgan,
                RefToActor newValue)
                : base(dgan, "Actor", newValue, false)
            {
                this.mLabel = "Set " +
                    (dgan.bPlay ? "Play" : "Stop") + " Animation Node Actor";
            }
        }

        private class TimingPriorityCommand
            : AnimNodePropertyCommand<JazzChunk.AnimationPriority>
        {
            public TimingPriorityCommand(DGAnimNode dgan,
                JazzChunk.AnimationPriority newValue)
                : base(dgan, "TimingPriority", newValue)
            {
                this.mLabel = this.mLabel + "Priority";
            }
        }

        private void CreateFlagsCommand(object value)
        {
            JazzAnimationFlags flags = (JazzAnimationFlags)value;
            this.mScene.Container.UndoRedo.Submit(new FlagsCommand(this, flags));
        }

        [Undoable("CreateFlagsCommand")]
        public JazzAnimationFlags Flags
        {
            get { return this.mAnimNode.Flags; }
            set
            {
                if (this.mAnimNode.Flags != value)
                {
                    this.mAnimNode.Flags = value;
                    this.UpdateVisualization();
                }
            }
        }

        private void CreatePriorityCommand(object value)
        {
            JazzChunk.AnimationPriority priority = (JazzChunk.AnimationPriority)value;
            this.mScene.Container.UndoRedo.Submit(new PriorityCommand(this, priority));
        }

        [Undoable("CreatePriorityCommand")]
        public JazzChunk.AnimationPriority Priority
        {
            get { return this.mAnimNode.Priority; }
            set
            {
                if (this.mAnimNode.Priority != value)
                {
                    this.mAnimNode.Priority = value;
                    this.UpdateVisualization();
                }
            }
        }

        private void CreateBlendInTimeCommand(object value)
        {
            float time = (float)value;
            this.mScene.Container.UndoRedo.Submit(new BlendInTimeCommand(this, time));
        }

        [Undoable("CreateBlendInTimeCommand")]
        public float BlendInTime
        {
            get { return this.mAnimNode.BlendInTime; }
            set
            {
                if (this.mAnimNode.BlendInTime != value)
                {
                    this.mAnimNode.BlendInTime = value;
                    this.UpdateVisualization();
                }
            }
        }

        private void CreateBlendOutTimeCommand(object value)
        {
            float time = (float)value;
            this.mScene.Container.UndoRedo.Submit(new BlendOutTimeCommand(this, time));
        }

        [Undoable("CreateBlendOutTimeCommand")]
        public float BlendOutTime
        {
            get { return this.mAnimNode.BlendOutTime; }
            set
            {
                if (this.mAnimNode.BlendOutTime != value)
                {
                    this.mAnimNode.BlendOutTime = value;
                    this.UpdateVisualization();
                }
            }
        }

        private void CreateSpeedCommand(object value)
        {
            float speed = (float)value;
            this.mScene.Container.UndoRedo.Submit(new SpeedCommand(this, speed));
        }

        [Undoable("CreateSpeedCommand")]
        public float Speed
        {
            get { return this.mAnimNode.Speed; }
            set
            {
                if (this.mAnimNode.Speed != value)
                {
                    this.mAnimNode.Speed = value;
                    this.UpdateVisualization();
                }
            }
        }

        private void CreateActorCommand(object value)
        {
            RefToActor rta = value as RefToActor;
            this.mScene.Container.UndoRedo.Submit(new ActorCommand(this, rta));
        }

        [Undoable("CreateActorCommand")]
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
                        this.UpdateVisualization();
                    }
                }
                else
                {
                    ActorDefinition ad 
                        = value == null ? null : value.GetValue();
                    if (this.mAnimNode.Actor != ad)
                    {
                        this.mAnimNode.Actor = ad;
                        this.UpdateVisualization();
                    }
                }
            }
        }

        private void CreateTimingPriorityCommand(object value)
        {
            JazzChunk.AnimationPriority priority = (JazzChunk.AnimationPriority)value;
            this.mScene.Container.UndoRedo.Submit(new TimingPriorityCommand(this, priority));
        }

        [Undoable("CreateTimingPriorityCommand")]
        public JazzChunk.AnimationPriority TimingPriority
        {
            get { return this.mAnimNode.TimingPriority; }
            set
            {
                if (this.mAnimNode.TimingPriority != value)
                {
                    this.mAnimNode.TimingPriority = value;
                    this.UpdateVisualization();
                }
            }
        }
    }
}
