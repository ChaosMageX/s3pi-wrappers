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
            if (actor == null)
            {
                throw new ArgumentNullException("actor");
            }
            if (scene == null)
            {
                throw new ArgumentNullException("scene");
            }
            this.mActor = actor;
            this.mScene = scene;
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

        public string Name
        {
            get { return this.mActor.Name; }
            set
            {
                if (this.mActor.Name != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new NameCommand(this, value, false));
                }
            }
        }
    }
}
