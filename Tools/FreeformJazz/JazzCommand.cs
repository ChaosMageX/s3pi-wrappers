using System;
using s3piwrappers.Helpers;

namespace s3piwrappers.FreeformJazz
{
    public abstract class JazzCommand : Command
    {
        protected readonly JazzGraphContainer mContainer;

        public JazzCommand(JazzGraphContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.mContainer = container;
        }

        public JazzCommand(JazzGraphContainer container, 
            bool groupWithPrevCommand, string label)
            : base(groupWithPrevCommand, label)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.mContainer = container;
        }
    }
}
