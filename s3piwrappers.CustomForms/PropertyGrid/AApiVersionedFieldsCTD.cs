using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    [TypeConverter(typeof(AApiVersionedFieldsCTDConverter))]
    public class AApiVersionedFieldsCTD : ICustomTypeDescriptor
    {
        protected AApiVersionedFields owner;
        protected string field;
        protected object component;
        AApiVersionedFields value;
        public AApiVersionedFieldsCTD(AApiVersionedFields value) { this.value = value; }
        public AApiVersionedFieldsCTD(AApiVersionedFields owner, string field, object component) { this.owner = owner; this.field = field; this.component = component; }

        #region Helpers
        public static string GetFieldIndex(string field)
        {
            string[] split = field.Split(' ');
            return split.Length == 1 ? null : split[split[0].Trim().StartsWith("[") ? 0 : 1].Trim();
        }

        public static string GetFieldName(string field)
        {
            string[] split = field.Split(' ');
            return split.Length == 1 ? field : split[split[0].Trim().StartsWith("[") ? 1 : 0].Trim();
        }

        public static Type GetFieldType(AApiVersionedFields owner, string field)
        {
            try
            {
                Type type = AApiVersionedFields.GetContentFieldTypes(0, owner.GetType())[GetFieldName(field)];

                if (field.IndexOf(' ') == -1)
                    return type;
                else
                {
                    if (type.HasElementType && !type.GetElementType().IsAbstract) return type.GetElementType();

                    Type baseType = GetGenericType(type);
                    if (baseType != null && !baseType.IsAbstract) return baseType;

                    return GetFieldValue(owner, field).Type;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public static TypedValue GetFieldValue(AApiVersionedFields owner, string field)
        {
            try
            {
                string index = GetFieldIndex(field);
                if (index == null)
                {
                    TypedValue tv = owner[field];
                    return tv.Type == typeof(Double) || tv.Type == typeof(Single) ? new TypedValue(tv.Type, tv.Value, "R") : tv;
                }
                else if (owner is ArrayOwner)
                    return owner[index];
                else
                {
                    object o = ((IGenericAdd)owner[GetFieldName(field)].Value)[Convert.ToInt32("0x" + index.Substring(1, index.Length - 2), 16)];
                    return new TypedValue(o.GetType(), o);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public static Type GetGenericType(Type type)
        {
            try
            {
                Type t = type;
                while (t.BaseType != typeof(object)) { t = t.BaseType; }
                if (t.GetGenericArguments().Length == 1) return t.GetGenericArguments()[0];
                return null;
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool isCollection(Type fieldType)
        {
            if (!typeof(ICollection).IsAssignableFrom(fieldType)) return false;
            Type baseType = GetGenericType(fieldType);
            return baseType != null && typeof(AApiVersionedFields).IsAssignableFrom(baseType);
        }
        #endregion

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes() { try { return TypeDescriptor.GetAttributes(this, true); } catch (Exception ex) { throw ex; } }

        public string GetClassName() { try { return TypeDescriptor.GetClassName(this, true); } catch (Exception ex) { throw ex; } }

        public string GetComponentName() { try { return TypeDescriptor.GetComponentName(this, true); } catch (Exception ex) { throw ex; } }

        public TypeConverter GetConverter() { try { return TypeDescriptor.GetConverter(this, true); } catch (Exception ex) { throw ex; } }

        public EventDescriptor GetDefaultEvent() { try { return TypeDescriptor.GetDefaultEvent(this, true); } catch (Exception ex) { throw ex; } }

        public PropertyDescriptor GetDefaultProperty() { try { return TypeDescriptor.GetDefaultProperty(this, true); } catch (Exception ex) { throw ex; } }

        public object GetEditor(Type editorBaseType) { try { return TypeDescriptor.GetEditor(this, editorBaseType, true); } catch (Exception ex) { throw ex; } }

        public EventDescriptorCollection GetEvents(Attribute[] attributes) { try { return TypeDescriptor.GetEvents(this, attributes, true); } catch (Exception ex) { throw ex; } }

        public EventDescriptorCollection GetEvents() { try { return TypeDescriptor.GetEvents(this, true); } catch (Exception ex) { throw ex; } }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            try
            {
                if (value == null) value = (AApiVersionedFields)GetFieldValue(owner, field).Value;

                List<string> filter = new List<string>(new string[] { "Stream", /*"AsBytes",/**/ "Value", });
                List<string> contentFields = value.ContentFields;
                List<TypedValuePropertyDescriptor> ltpdc = new List<TypedValuePropertyDescriptor>();
                foreach (string f in contentFields)
                {
                    if (filter.Contains(f)) continue;
                    if (!canWrite(value, f)) continue;
                    TypedValuePropertyDescriptor tvpd = new TypedValuePropertyDescriptor(value, f, null);
                    ltpdc.Add(new TypedValuePropertyDescriptor(value, f, new Attribute[] { new CategoryAttribute(tvpdToCategory(tvpd.PropertyType)) }));
                }
                List<PropertyDescriptor> lpdc = new List<PropertyDescriptor>(ltpdc.ToArray());
                int i = 0; while (i < ltpdc.Count && ltpdc[i].Priority < int.MaxValue) i++;
                if (typeof(IDictionary).IsAssignableFrom(value.GetType())) { lpdc.Insert(i, new IDictionaryPropertyDescriptor((IDictionary)value, "(this)", new Attribute[] { new CategoryAttribute("Lists") })); }
                return new PropertyDescriptorCollection(lpdc.ToArray());
            }
            catch (Exception ex) { throw ex; }
        }
        string tvpdToCategory(Type t)
        {
            if (t.Equals(typeof(EnumChooserCTD))) return "Values";
            if (t.Equals(typeof(EnumFlagsCTD))) return "Fields";
            if (t.Equals(typeof(AsHexCTD))) return "Values";
            if (t.Equals(typeof(ArrayCTD))) return "Lists";
            if (t.Equals(typeof(AApiVersionedFieldsCTD))) return "Fields";
            if (t.Equals(typeof(ICollectionAApiVersionedFieldsCTD))) return "Lists";
            if (t.Equals(typeof(TGIBlockListCTD))) return "Lists";
            if (t.Equals(typeof(IDictionaryCTD))) return "Lists";
            if (t.Equals(typeof(ReaderCTD))) return "Readers";
            return "Values";
        }
        bool canWrite(AApiVersionedFields owner, string field)
        {
            if (owner.GetType().Equals(typeof(AsKVP))) return true;
            if (owner.GetType().Equals(typeof(ArrayOwner))) return true;
            return owner.GetType().GetProperty(field).CanWrite;
        }

        public PropertyDescriptorCollection GetProperties() { try { return TypeDescriptor.GetProperties(new Attribute[] { }); } catch (Exception ex) { throw ex; } }

        public object GetPropertyOwner(PropertyDescriptor pd) { return this; }

        #endregion

        public class IDictionaryPropertyDescriptor : PropertyDescriptor
        {
            IDictionary value;
            public IDictionaryPropertyDescriptor(IDictionary value, string field, Attribute[] attrs) : base(field, attrs) { this.value = value; }

            public override bool CanResetValue(object component) { return false; }

            public override Type ComponentType { get { throw new NotImplementedException(); } }

            public override object GetValue(object component) { return new IDictionaryCTD(value); }

            public override bool IsReadOnly { get { return false; } }

            public override Type PropertyType { get { return typeof(IDictionaryCTD); } }

            public override void ResetValue(object component) { throw new NotImplementedException(); }

            public override void SetValue(object component, object value) { }

            public override bool ShouldSerializeValue(object component) { return true; }
        }

        public class TypedValuePropertyDescriptor : PropertyDescriptor
        {
            AApiVersionedFields owner;
            int priority = int.MaxValue;
            DependentList<TGIBlock> tgiBlocks = null;
            Type fieldType;
            public TypedValuePropertyDescriptor(AApiVersionedFields owner, string field, Attribute[] attrs)
                : base(field, attrs)
            {
                try
                {
                    this.owner = owner;
                    if (typeof(ArrayOwner).Equals(owner.GetType())) fieldType = ((ArrayOwner)owner).ElementType;
                    else if (typeof(AsKVP).Equals(owner.GetType())) fieldType = ((AsKVP)owner).GetType(Name);
                    else
                    {
                        string name = GetFieldName(field);
                        fieldType = GetFieldType(owner, field);
                        priority = ElementPriorityAttribute.GetPriority(owner.GetType(), name);
                        tgiBlocks = owner.GetTGIBlocks(name);
                    }
                }
                catch (Exception ex) { throw ex; }
            }

            public int Priority { get { return priority; } }

            public bool hasTGIBlocks { get { return tgiBlocks != null; } }
            public DependentList<TGIBlock> TGIBlocks { get { return tgiBlocks; } }

            public Type FieldType { get { return fieldType; } }

            public override bool CanResetValue(object component) { return false; }

            public override Type ComponentType { get { throw new NotImplementedException(); } }

            public override object GetValue(object component)
            {
                try
                {
                    Type t = PropertyType;
                    if (t.Equals(typeof(ReaderCTD))) return new ReaderCTD(owner, Name, component);
                    if (t.Equals(typeof(IDictionaryCTD))) return new IDictionaryCTD(owner, Name, component);
                    if (t.Equals(typeof(ICollectionAApiVersionedFieldsCTD))) return new ICollectionAApiVersionedFieldsCTD(owner, Name, component);
                    if (t.Equals(typeof(TGIBlockListCTD))) return new TGIBlockListCTD(owner, Name, component);
                    if (t.Equals(typeof(ArrayCTD))) return new ArrayCTD(owner, Name, component);
                    if (t.Equals(typeof(AApiVersionedFieldsCTD))) return new AApiVersionedFieldsCTD(owner, Name, component);
                    if (t.Equals(typeof(IResourceKeyCTD))) return new IResourceKeyCTD(owner, Name, component);
                    if (t.Equals(typeof(TGIBlockListIndexCTD))) return new TGIBlockListIndexCTD(owner, Name, tgiBlocks, component);
                    if (t.Equals(typeof(AsHexCTD))) return new AsHexCTD(owner, Name, component);
                    if (t.Equals(typeof(EnumChooserCTD))) return new EnumChooserCTD(owner, Name, component);
                    if (t.Equals(typeof(EnumFlagsCTD))) return new EnumFlagsCTD(owner, Name, component);
                    return GetFieldValue(owner, Name);
                }
                catch (Exception ex) { throw ex; }
            }

            public override bool IsReadOnly
            {
                get
                {
                    if (owner.GetType().Equals(typeof(ArrayOwner))) return false;
                    if (owner.GetType().Equals(typeof(AsKVP))) return false;
                    string name = Name.Split(' ').Length == 1 ? Name : Name.Split(new char[] { ' ' }, 2)[1].Trim();
                    return !owner.GetType().GetProperty(name).CanWrite;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    try
                    {
                        // Must test these before IConvertible
                        List<Type> simpleTypes = new List<Type>(new Type[] { typeof(bool), typeof(DateTime), typeof(decimal), typeof(double), typeof(float), typeof(string), });
                        if (simpleTypes.Contains(fieldType)) return fieldType;

                        // Must test enum before IConvertible
                        if (typeof(Enum).IsAssignableFrom(fieldType))
                            return fieldType.GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0 ? typeof(EnumChooserCTD) : typeof(EnumFlagsCTD);

                        if (typeof(IConvertible).IsAssignableFrom(fieldType))
                            return hasTGIBlocks ? typeof(TGIBlockListIndexCTD) : typeof(AsHexCTD);

                        if (typeof(IResourceKey).IsAssignableFrom(fieldType)) return typeof(IResourceKeyCTD);

                        if (typeof(AApiVersionedFields).IsAssignableFrom(fieldType)) return typeof(AApiVersionedFieldsCTD);


                        // More complex stuff

                        // Arrays
                        if (fieldType.HasElementType
                            && (typeof(IConvertible).IsAssignableFrom(fieldType.GetElementType())
                            || typeof(AApiVersionedFields).IsAssignableFrom(fieldType.GetElementType())
                            ))
                            return typeof(ArrayCTD);

                        if (isCollection(fieldType))
                        {
                            if (GetGenericType(fieldType) == typeof(TGIBlock))
                                return typeof(TGIBlockListCTD);

                            return typeof(ICollectionAApiVersionedFieldsCTD);
                        }

                        if (typeof(IDictionary).IsAssignableFrom(fieldType)) return typeof(IDictionaryCTD);

                        if (typeof(BinaryReader).IsAssignableFrom(fieldType) || typeof(TextReader).IsAssignableFrom(fieldType))
                            return typeof(ReaderCTD);

                        return fieldType;
                    }
                    catch (Exception ex) { throw ex; }
                }
            }

            public override void ResetValue(object component) { throw new NotImplementedException(); }

            public override void SetValue(object component, object value)
            {
                try
                {
                    string index = GetFieldIndex(Name);
                    if (index == null)
                        owner[Name] = new TypedValue(value.GetType(), value);
                    else if (owner is ArrayOwner)
                        owner[index] = new TypedValue(value.GetType(), value);
                    else
                        ((IGenericAdd)owner[GetFieldName(Name)].Value)[Convert.ToInt32("0x" + index.Substring(1, index.Length - 2), 16)] = value;
                    OnValueChanged(owner, EventArgs.Empty);
                }
                catch (Exception ex) { throw ex; }
            }

            public override bool ShouldSerializeValue(object component) { return true; }
        }

        public class AApiVersionedFieldsCTDConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (typeof(string).Equals(destinationType)) return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (typeof(string).Equals(destinationType)) return "";
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
