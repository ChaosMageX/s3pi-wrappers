using System;
using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [TypeConverter(typeof(EnumFlagsConverter))]
    public class EnumFlagsCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        public EnumFlagsCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

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

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) { return new PropertyDescriptorCollection(new PropertyDescriptor[] { new ConverterPropertyDescriptor(owner, field, component, null), }); }

        public PropertyDescriptorCollection GetProperties() { return GetProperties(null); }

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) { return this; }

        #endregion

        public class EnumFlagsConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType.Equals(typeof(string))) return true;
                return base.CanConvertTo(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value != null && value.GetType().Equals(typeof(string)))
                {
                    string[] content = ((string)value).Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string str = content[0];
                    try
                    {
                        AApiVersionedFieldsCTD.TypedValuePropertyDescriptor pd = (AApiVersionedFieldsCTD.TypedValuePropertyDescriptor)context.PropertyDescriptor;
                        ulong num = Convert.ToUInt64(str, str.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10);
                        return Enum.ToObject(pd.FieldType, num);
                    }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data: " + str, ex); }
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType.Equals(typeof(string))) return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value as EnumFlagsCTD != null && destinationType.Equals(typeof(string)))
                {
                    try
                    {
                        EnumFlagsCTD ctd = (EnumFlagsCTD)value;
                        string name = ctd.field.Split(' ').Length == 1 ? ctd.field : ctd.field.Split(new char[] { ' ' }, 2)[1].Trim();
                        TypedValue tv = ctd.owner[name];
                        return "0x" + Enum.Format(tv.Type, tv.Value, "X");
                    }
                    //catch { }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data", ex); }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            static int underlyingTypeToBits(Type t)
            {
                if (t.Equals(typeof(byte))) return 8;
                if (t.Equals(typeof(ushort))) return 16;
                if (t.Equals(typeof(uint))) return 32;
                if (t.Equals(typeof(ulong))) return 64;
                return -1;
            }
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                EnumFlagsCTD ctd = (EnumFlagsCTD)value;
                string name = ctd.field.Split(' ').Length == 1 ? ctd.field : ctd.field.Split(new char[] { ' ' }, 2)[1].Trim();
                Type enumType = AApiVersionedFields.GetContentFieldTypes(0, ctd.owner.GetType())[name];
                int bits = underlyingTypeToBits(Enum.GetUnderlyingType(enumType));
                EnumFlagPropertyDescriptor[] enumFlags = new EnumFlagPropertyDescriptor[bits];
                string fmt = "[{0:X" + bits.ToString("X").Length + "}] ";
                for (int i = 0; i < bits; i++)
                {
                    ulong u = (ulong)1 << i;
                    string s = (Enum)Enum.ToObject(enumType, u) + "";
                    if (s == u.ToString()) s = "-";
                    s = String.Format(fmt, i) + s;
                    enumFlags[i] = new EnumFlagPropertyDescriptor(ctd.owner, ctd.field, u, s);
                }
                return new PropertyDescriptorCollection(enumFlags);
            }

            public class EnumFlagPropertyDescriptor : PropertyDescriptor
            {
                AApiVersionedFields owner;
                string field;
                ulong mask;
                public EnumFlagPropertyDescriptor(AApiVersionedFields owner, string field, ulong mask, string value) : base(value, null) { this.owner = owner; this.field = field; this.mask = mask; }
                public override bool CanResetValue(object component) { return true; }

                public override Type ComponentType { get { throw new NotImplementedException(); } }

                ulong getFlags(AApiVersionedFields owner, string field)
                {
                    TypedValue tv = owner[field];
                    object o = Convert.ChangeType(tv.Value, Enum.GetUnderlyingType(tv.Type));
                    if (o.GetType().Equals(typeof(byte))) return (byte)o;
                    if (o.GetType().Equals(typeof(ushort))) return (ushort)o;
                    if (o.GetType().Equals(typeof(uint))) return (uint)o;
                    return (ulong)o;
                }
                public override object GetValue(object component)
                {
                    string name = field.Split(' ').Length == 1 ? field : field.Split(new char[] { ' ' }, 2)[1].Trim();
                    ulong old = getFlags(owner, name);
                    return (old & mask) != 0;
                }

                public override bool IsReadOnly { get { return !owner.GetType().GetProperty(field).CanWrite; } }

                public override Type PropertyType { get { return typeof(Boolean); } }

                public override void ResetValue(object component) { SetValue(component, false); }

                void setFlags(AApiVersionedFields owner, string field, ulong mask, bool value)
                {
                    ulong old = getFlags(owner, field);
                    ulong res = old & ~mask;
                    if (value) res |= mask;
                    Type t = AApiVersionedFields.GetContentFieldTypes(0, owner.GetType())[field];
                    TypedValue tv = new TypedValue(t, Enum.ToObject(t, res));
                    owner[field] = tv;
                }
                public override void SetValue(object component, object value)
                {
                    setFlags(owner, field, mask, (bool)value);
                    OnValueChanged(owner, EventArgs.Empty);
                }

                public override bool ShouldSerializeValue(object component) { return false; }
            }
        }
    }
}
