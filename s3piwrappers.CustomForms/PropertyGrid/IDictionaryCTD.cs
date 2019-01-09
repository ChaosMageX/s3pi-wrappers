using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [Editor(typeof(IDictionaryEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(IDictionaryConverter))]
    public class IDictionaryCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        IDictionary value;
        public IDictionaryCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }
        public IDictionaryCTD(IDictionary value) { this.value = value; }

        public IDictionary Value { get { if (value == null) value = (IDictionary)owner[field].Value; return value; } }

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

        public class IDictionaryEditor : UITypeEditor
        {
            DictionaryEntry getDefault(Type kvt)
            {
                var getter = kvt.GetMethod("GetDefault");
                if (getter != null)
                    return (DictionaryEntry)getter.Invoke(null, null);
                throw new ArgumentException(string.Format("{0} does not have method 'GetDefault'.", kvt.Name));
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.Modal; }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                IDictionaryCTD field = (IDictionaryCTD)value;
                if (field.Value == null) return value;

                int count = 0;
                Type[] interfaces = field.Value.GetType().GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (interfaces[i].Name == "IDictionary`2") count++;
                }
                if (count != 1) return value;
                DictionaryEntry entry = getDefault(field.Value.GetType());

                List<object> oldKeys = new List<object>();
                AsKVPList list = new AsKVPList(entry);
                foreach (var k in field.Value.Keys) { list.Add(new AsKVP(0, null, k, field.Value[k])); oldKeys.Add(k); }

                NewGridForm ui = new NewGridForm(list);
                edSvc.ShowDialog(ui);

                List<object> newKeys = new List<object>();
                foreach (AsKVP kvp in list)
                    newKeys.Add(kvp["Key"].Value);

                List<object> delete = new List<object>();
                foreach (var k in field.Value.Keys) if (!newKeys.Contains(k)) delete.Add(k);
                foreach (object k in delete) field.Value.Remove(k);

                bool dups = false;
                foreach (AsKVP kvp in list)
                    if (oldKeys.Contains(kvp["Key"].Value)) field.Value[kvp["Key"].Value] = kvp["Val"].Value;
                    else if (!field.Value.Contains(kvp["Key"].Value)) field.Value.Add(kvp["Key"].Value, kvp["Val"].Value);
                    else dups = true;
                if (dups)
                    CopyableMessageBox.Show("Duplicate keyed entries were dropped.", "s3pe", CopyableMessageBoxButtons.OK, CopyableMessageBoxIcon.Warning);

                return value;
            }
        }

        public class IDictionaryConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (typeof(string).Equals(destinationType)) return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                IDictionaryCTD ctd = value as IDictionaryCTD;
                IDictionary id = (IDictionary)ctd.Value;

                if (typeof(string).Equals(destinationType)) return id == null ? "(null)" : "(Dictionary: 0x" + id.Count.ToString("X") + ")";
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
