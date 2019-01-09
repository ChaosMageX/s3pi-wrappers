using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [Editor(typeof(EnumChooserEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(EnumChooserConverter))]
    public class EnumChooserCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        public EnumChooserCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

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

        public class EnumChooserConverter : TypeConverter
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
                    AApiVersionedFieldsCTD.TypedValuePropertyDescriptor pd = (AApiVersionedFieldsCTD.TypedValuePropertyDescriptor)context.PropertyDescriptor;
                    string v = value as string;
                    try
                    {
                        if (v.Split(' ').Length > 1) v = v.Split(' ')[0];
                        if (new List<string>(Enum.GetNames(pd.FieldType)).Contains(v))
                            return Enum.Parse(pd.FieldType, v);
                        return Enum.ToObject(pd.FieldType, Convert.ToUInt64(v, v.StartsWith("0x") ? 16 : 10));
                    }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data: " + v, ex); }
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
                if (value as EnumChooserCTD != null && destinationType.Equals(typeof(string)))
                {
                    try
                    {
                        EnumChooserCTD ctd = (EnumChooserCTD)value;
                        TypedValue tv = (TypedValue)AApiVersionedFieldsCTD.GetFieldValue(ctd.owner, ctd.field);
                        return "" + AApiVersionedFieldsCTD.GetFieldValue(ctd.owner, ctd.field);
                    }
                    catch (Exception ex) { throw new NotSupportedException("Invalid data", ex); }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public class EnumChooserEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }

            public override bool IsDropDownResizable { get { return true; } }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                EnumChooserCTD ctd = (EnumChooserCTD)value;
                TypedValue tv = AApiVersionedFieldsCTD.GetFieldValue(ctd.owner, ctd.field);

                List<string> enumValues = new List<string>();
                int index = -1;
                int i = 0;
                foreach (Enum e in Enum.GetValues(tv.Type))
                {
                    if (e.Equals((Enum)tv.Value)) index = i;
                    enumValues.Add(new TypedValue(e.GetType(), e, "X"));
                    i++;
                }

                int maxWidth = Application.OpenForms[0].Width / 3;
                if (maxWidth < Screen.PrimaryScreen.WorkingArea.Size.Width / 4) maxWidth = Screen.PrimaryScreen.WorkingArea.Size.Width / 4;
                int maxHeight = Application.OpenForms[0].Height / 3;
                if (maxHeight < Screen.PrimaryScreen.WorkingArea.Size.Height / 4) maxHeight = Screen.PrimaryScreen.WorkingArea.Size.Height / 4;

                TextBox tb = new TextBox
                {
                    AutoSize = true,
                    Font = new ListBox().Font,
                    Margin = new Padding(0),
                    MaximumSize = new System.Drawing.Size(maxWidth, maxHeight),
                    Multiline = true,
                    Lines = enumValues.ToArray(),
                };
                tb.PerformLayout();

                ListBox lb = new ListBox()
                {
                    IntegralHeight = false,
                    Margin = new Padding(0),
                    Size = tb.PreferredSize,
                    Tag = edSvc,
                };
                lb.Items.AddRange(enumValues.ToArray());
                lb.PerformLayout();

                if (index >= 0) { lb.SelectedIndices.Add(index); }
                lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
                edSvc.DropDownControl(lb);

                return lb.SelectedItem == null ? value : (Enum)new EnumChooserConverter().ConvertFrom(context, System.Globalization.CultureInfo.CurrentCulture, lb.SelectedItem);
            }

            void lb_SelectedIndexChanged(object sender, EventArgs e) { ((sender as ListBox).Tag as IWindowsFormsEditorService).CloseDropDown(); }
        }
    }
}
