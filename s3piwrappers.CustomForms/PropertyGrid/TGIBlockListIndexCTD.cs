using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [Editor(typeof(TGIBlockListIndexEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(TGIBlockListIndexConverter))]
    public class TGIBlockListIndexCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected DependentList<TGIBlock> tgiBlocks;
        protected object component;
        public TGIBlockListIndexCTD(AApiVersionedFields owner, string field, DependentList<TGIBlock> tgiBlocks, object component)
        {
            this.owner = owner;
            this.field = field;
            this.tgiBlocks = tgiBlocks;
            this.component = component;
        }

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

        public class TGIBlockListIndexEditor : UITypeEditor
        {
            TGIBlockSelection ui;
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (ui == null)
                    ui = new TGIBlockSelection();

                TGIBlockListIndexCTD o = value as TGIBlockListIndexCTD;
                ui.SetField(o.owner, o.field, o.tgiBlocks);
                ui.EdSvc = edSvc;
                edSvc.DropDownControl(ui);
                // the ui (a) updates the value and (b) closes the dropdown

                return o.owner[o.field].Value;
            }
        }

        public class TGIBlockListIndexConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType.Equals(typeof(string))) return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value != null && value.GetType().Equals(typeof(string)))
                {
                    string str = ((string)value).Trim();
                    try
                    {
                        AApiVersionedFieldsCTD.TypedValuePropertyDescriptor pd = (AApiVersionedFieldsCTD.TypedValuePropertyDescriptor)context.PropertyDescriptor;
                        ulong num = Convert.ToUInt64(str, str.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10);
                        return Convert.ChangeType(num, pd.FieldType);
                    }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data: " + str, ex); }
                }
                return base.ConvertFrom(context, culture, value);
            }/**/

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType.Equals(typeof(string))) return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value as TGIBlockListIndexCTD != null && destinationType.Equals(typeof(string)))
                {
                    try
                    {
                        TGIBlockListIndexCTD ctd = (TGIBlockListIndexCTD)value;
                        string name = ctd.field.Split(' ').Length == 1 ? ctd.field : ctd.field.Split(new char[] { ' ' }, 2)[1].Trim();
                        return "" + ctd.owner[name];
                    }
                    //catch { }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data", ex); }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
