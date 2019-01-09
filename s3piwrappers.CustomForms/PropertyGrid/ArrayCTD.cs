using System;
using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [TypeConverter(typeof(ArrayConverter))]
    public class ArrayCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        public ArrayCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }

        public string GetClassName() { return TypeDescriptor.GetClassName(this, true); }

        public string GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }

        public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }

        public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }

        public System.ComponentModel.PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }

        public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }

        public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }

        public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[] { new ConverterPropertyDescriptor(owner, field, component, null), });
        }

        public PropertyDescriptorCollection GetProperties() { return GetProperties(null); }

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) { return this; }

        #endregion

        public Array Value { get { return (Array)owner[field].Value; } }

        public class ArrayConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (typeof(string).Equals(destinationType)) return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                ArrayCTD ctd = value as ArrayCTD;
                Array ary = ctd.owner[ctd.field].Value as Array;

                if (typeof(string).Equals(destinationType)) return ary == null ? "(null)" : "(Array: 0x" + ary.Length.ToString("X") + ")";
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                ArrayCTD ctd = value as ArrayCTD;
                AApiVersionedFields owner = new ArrayOwner(ctd.owner, ctd.field);
                Array ary = ctd.Value;
                Type type = ary.GetType().GetElementType();
                string fmt = type.Name + " [{0:X" + ary.Length.ToString("X").Length + "}]";

                AApiVersionedFieldsCTD.TypedValuePropertyDescriptor[] pds = new AApiVersionedFieldsCTD.TypedValuePropertyDescriptor[ary.Length];
                for (int i = 0; i < ary.Length; i++)
                {
                    try
                    {
                        pds[i] = new AApiVersionedFieldsCTD.TypedValuePropertyDescriptor(owner, String.Format(fmt, i), new Attribute[] { });
                    }
                    catch (Exception ex) { throw ex; }
                }
                return new PropertyDescriptorCollection(pds);
            }
        }
    }
}
