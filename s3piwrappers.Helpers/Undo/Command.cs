using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Helpers.Undo
{
    public abstract class Command : IDisposable
    {
        protected bool bGroupWithPrevCommand;
        protected string mLabel;

        public Command()
        {
            this.bGroupWithPrevCommand = false;
            this.mLabel = "";
        }

        public Command(bool groupWithPrevCommand, string label)
        {
            this.bGroupWithPrevCommand = groupWithPrevCommand;
            this.mLabel = label;
        }

        public bool GroupWithPreviousCommand
        {
            get { return this.bGroupWithPrevCommand; }
        }

        public virtual string Label
        {
            get { return this.mLabel; }
        }

        public virtual void Dispose()
        {
        }

        public abstract bool Execute();

        public abstract void Undo();

        public virtual void Redo()
        {
            this.Execute();
        }

        public virtual bool IsExtendable(Command possibleExtension)
        {
            return false;
        }

        public virtual void Extend(Command possibleExtension)
        {
        }
    }
}
