using System;
using System.ComponentModel;
using System.Drawing.Design;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    [Editor(typeof(RefToActor.RefEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(RefToActor.RefConverter))]
    public class RefToActor : RefToValue<ActorDefinition>
    {
        public RefToActor(StateMachineScene scene, ActorDefinition ad)
            : base(scene, "ActorDefinitions", "ActorDefinitionCount", ad)
        {
        }
    }
}
