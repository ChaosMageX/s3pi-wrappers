using System;
using System.ComponentModel;
using System.Drawing.Design;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    [Editor(typeof(RefEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(RefConverter))]
    public class RefToState : RefToValue<State>
    {
        public RefToState(StateMachineScene scene, State state)
            : base(scene, "States", "StateCount", state)
        {
        }
    }
}
