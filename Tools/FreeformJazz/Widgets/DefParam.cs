using System;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DefParam
    {
        private ParamDefinition mParam;
        private StateMachineScene mScene;

        public DefParam(ParamDefinition param, StateMachineScene scene)
        {
            this.mParam = param ?? throw new ArgumentNullException("param");
            this.mScene = scene ?? throw new ArgumentNullException("scene");
        }

        public ParamDefinition GetParam()
        {
            return this.mParam;
        }

        private class NameCommand
            : PropertyCommand<ParamDefinition, string>
        {
            public NameCommand(DefParam param, 
                string newValue, bool extendable)
                : base(param.mParam, "Name", newValue, extendable)
            {
                this.mLabel = "Set Parameter Definition Name";
            }
        }

        private void CreateNameCommand(object value)
        {
            this.mScene.Container.UndoRedo.Submit(new NameCommand(this, value.ToString(), false));
        }

        [Undoable("CreateNameCommand")]
        public string Name
        {
            get { return this.mParam.Name; }
            set
            {
                if (this.mParam.Name != value)
                {
                    this.mParam.Name = value;
                }
            }
        }

        private class DefaultValueCommand
            : PropertyCommand<ParamDefinition, string>
        {
            public DefaultValueCommand(DefParam param,
                string newValue, bool extendable)
                : base(param.mParam, "DefaultValue", newValue, extendable)
            {
                this.mLabel = "Set Parameter Definition Default Value";
            }
        }

        private void CreateDefaultValueCommand(object value)
        {
            this.mScene.Container.UndoRedo.Submit(new DefaultValueCommand(this, value.ToString(), false));
        }

        [Undoable("CreateDefaultValueCommand")]
        public string DefaultValue
        {
            get { return this.mParam.DefaultValue; }
            set
            {
                if (this.mParam.DefaultValue != value)
                {
                    this.mParam.DefaultValue = value;
                }
            }
        }
    }
}
