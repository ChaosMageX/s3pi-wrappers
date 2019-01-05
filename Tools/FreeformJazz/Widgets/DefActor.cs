using System;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DefActor
    {
        private ActorDefinition mActor;
        private StateMachineScene mScene;

        public DefActor(ActorDefinition actor, StateMachineScene scene)
        {
            this.mActor = actor ?? throw new ArgumentNullException("actor");
            this.mScene = scene ?? throw new ArgumentNullException("scene");
        }

        public ActorDefinition GetActor()
        {
            return this.mActor;
        }

        private class NameCommand
            : PropertyCommand<ActorDefinition, string>
        {
            public NameCommand(DefActor actor, 
                string newValue, bool extendable)
                : base(actor.mActor, "Name", newValue, extendable)
            {
                this.mLabel = "Set Actor Definition Name";
            }
        }

        private void CreateNameCommand(object value)
        {
            this.mScene.Container.UndoRedo.Submit(new NameCommand(this, value.ToString(), false));
        }

        [Undoable("CreateNameCommand")]
        public string Name
        {
            get { return this.mActor.Name; }
            set
            {
                if (this.mActor.Name != value)
                {
                    this.mActor.Name = value;
                }
            }
        }
    }
}
