using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Helpers.Undo
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UndoableAttribute : Attribute
    {
        public readonly string CreateCommandFunctionName;

        public UndoableAttribute(string createCommandFunctionName)
        {
            this.CreateCommandFunctionName = createCommandFunctionName;
        }
    }
}
