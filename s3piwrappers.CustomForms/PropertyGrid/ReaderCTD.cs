using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [Editor(typeof(ReaderEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ReaderConverter))]
    public class ReaderCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        public ReaderCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

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
            return new PropertyDescriptorCollection(new PropertyDescriptor[] { new ReaderPropertyDescriptor() });
        }

        public PropertyDescriptorCollection GetProperties() { return GetProperties(null); }

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) { return this; }

        #endregion

        public class ReaderPropertyDescriptor : PropertyDescriptor
        {
            ReaderEditor editor;
            public ReaderPropertyDescriptor() : base("Export/Import/Edit value", null) { }

            public override object GetEditor(Type editorBaseType)
            {
                if (editorBaseType == typeof(System.Drawing.Design.UITypeEditor))
                {
                    if (editor == null) editor = new ReaderEditor();
                    return editor;
                }
                return base.GetEditor(editorBaseType);
            }

            public override bool CanResetValue(object component) { throw new InvalidOperationException(); }

            public override void ResetValue(object component) { throw new InvalidOperationException(); }

            public override Type PropertyType { get { throw new InvalidOperationException(); } }

            public override object GetValue(object component) { throw new InvalidOperationException(); }

            public override bool IsReadOnly { get { throw new InvalidOperationException(); } }

            public override void SetValue(object component, object value) { throw new InvalidOperationException(); }

            public override Type ComponentType { get { throw new InvalidOperationException(); } }

            public override bool ShouldSerializeValue(object component) { throw new InvalidOperationException(); }
        }

        public class ReaderEditor : UITypeEditor
        {
            ReaderEditorPanel ui;
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (ui == null)
                    ui = new ReaderEditorPanel();

                ReaderCTD o = value as ReaderCTD;
                ui.SetField(o.owner, o.field);
                ui.EdSvc = edSvc;
                edSvc.DropDownControl(ui);
                // the ui (a) updates the value and (b) closes the dropdown

                return o.owner[o.field].Value;
            }
        }

        public class ReaderConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (typeof(string).IsAssignableFrom(destinationType)) return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (typeof(string).IsAssignableFrom(destinationType))
                    return "Import/Export/View Hex/Edit...";
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }

}
