using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [Editor(typeof(TGIBlockListEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(TGIBlockListConverter))]
    public class TGIBlockListCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        public TGIBlockListCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

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

        public class TGIBlockListEditor : UITypeEditor
        {
            System.Windows.Forms.TGIBlockListEditorForm.MainForm ui;
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.Modal; }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (ui == null)
                {
                    ui = new System.Windows.Forms.TGIBlockListEditorForm.MainForm();
                    //ui.Icon = ((System.Drawing.Icon)(new ComponentResourceManager(typeof(MainForm)).GetObject("$this.Icon")));
                    ui.Icon = Settings.Default.FormIcon;
                }

                TGIBlockListCTD ctd = value as TGIBlockListCTD;
                DependentList<TGIBlock> list = ctd.owner[ctd.field].Value as DependentList<TGIBlock>;

                ui.Items = list;
                DialogResult dr = edSvc.ShowDialog(ui);

                if (dr != DialogResult.OK) return value;

                list.Clear();
                list.AddRange(ui.Items);

                return value;
            }
        }

        public class TGIBlockListConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (typeof(string).Equals(destinationType)) return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                TGIBlockListCTD ctd = value as TGIBlockListCTD;
                DependentList<TGIBlock> list = ctd.owner[ctd.field].Value as DependentList<TGIBlock>;

                if (typeof(string).Equals(destinationType)) return list == null ? "(null)" : "(TGI Blocks: 0x" + list.Count.ToString("X") + ")";
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                TGIBlockListCTD ctd = value as TGIBlockListCTD;
                ICollection ary = (ICollection)ctd.owner[ctd.field].Value;

                AApiVersionedFieldsCTD.TypedValuePropertyDescriptor[] pds = new AApiVersionedFieldsCTD.TypedValuePropertyDescriptor[ary.Count];
                string fmt = "[{0:X" + ary.Count.ToString("X").Length + "}] {1}";
                int i = 0;
                foreach (var o in ary)
                {
                    try
                    {
                        pds[i] = new AApiVersionedFieldsCTD.TypedValuePropertyDescriptor(ctd.owner, String.Format(fmt, i, ctd.field), new Attribute[] { });
                    }
                    catch (Exception ex) { throw ex; }
                    i++;
                }
                return new PropertyDescriptorCollection(pds);
            }
        }
    }
}
