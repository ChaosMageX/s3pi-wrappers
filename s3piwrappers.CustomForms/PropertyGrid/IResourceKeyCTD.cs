using System;
using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [TypeConverter(typeof(IResourceKeyConverter))]
    public class IResourceKeyCTD : AApiVersionedFieldsCTD
    {
        public IResourceKeyCTD(AApiVersionedFields owner, string field, object component) : base(owner, field, component) { }

        public class IResourceKeyConverter : AApiVersionedFieldsCTD.AApiVersionedFieldsCTDConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string) || typeof(IResourceKey).IsAssignableFrom(sourceType))
                    return true;
                return base.CanConvertTo(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                AApiVersionedFieldsCTD.TypedValuePropertyDescriptor pd = (AApiVersionedFieldsCTD.TypedValuePropertyDescriptor)context.PropertyDescriptor;
                IResourceKeyCTD rkCTD = (IResourceKeyCTD)pd.GetValue(null);
                AApiVersionedFields owner = rkCTD.owner;
                string field = rkCTD.field;
                object component = rkCTD.component;
                IResourceKey rk = (IResourceKey)AApiVersionedFieldsCTD.GetFieldValue(owner, field).Value;

                if (typeof(IResourceKey).IsAssignableFrom(value.GetType()))
                {
                    IResourceKey rkNew = (IResourceKey)value;
                    rk.ResourceType = rkNew.ResourceType;
                    rk.ResourceGroup = rkNew.ResourceGroup;
                    rk.Instance = rkNew.Instance;
                    return rk;
                }
                if (value != null && value is string)
                {
                    if (AResourceKey.TryParse((string)value, rk))
                        return rk;
                    else
                        throw new NotSupportedException("Invalid data: " + (string)value);
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
                if (destinationType.Equals(typeof(string)))
                {
                    IResourceKeyCTD ctd = value as IResourceKeyCTD;
                    IResourceKey rk = (IResourceKey)GetFieldValue(ctd.owner, ctd.field).Value;
                    return "" + rk;
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
