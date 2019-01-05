using System;
using System.ComponentModel;
using System.Drawing.Design;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    [Editor(typeof(RefEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(RefConverter))]
    public class RefToParam : RefToValue<ParamDefinition>
    {
        public RefToParam(StateMachineScene scene, ParamDefinition pd)
            : base(scene, "ParamDefinitions", "ParamDefinitionCount", pd)
        {
        }
    }
}
