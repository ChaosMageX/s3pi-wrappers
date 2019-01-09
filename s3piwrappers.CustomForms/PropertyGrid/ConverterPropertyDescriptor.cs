using System;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    public class ConverterPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        AApiVersionedFields owner;
        string field;
        object component;
        public ConverterPropertyDescriptor(AApiVersionedFields owner, string field, object component, Attribute[] attr)
            : base(field, attr) { this.owner = owner; this.field = field; this.component = component; }

        //public override string Name { get { return field; } }

        public override bool CanResetValue(object component) { throw new InvalidOperationException(); }

        public override void ResetValue(object component) { throw new InvalidOperationException(); }

        public override Type PropertyType { get { throw new InvalidOperationException(); } }

        public override object GetValue(object component) { throw new InvalidOperationException(); }

        public override bool IsReadOnly { get { throw new InvalidOperationException(); } }

        public override void SetValue(object component, object value) { throw new InvalidOperationException(); }

        //public override Type ComponentType { get { throw new InvalidOperationException(); } }
        public override Type ComponentType { get { return component.GetType(); } }

        //public override bool ShouldSerializeValue(object component) { throw new InvalidOperationException(); }
        public override bool ShouldSerializeValue(object component) { return true; }
    }
}
