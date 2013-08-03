using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public class RKContainer
    {
        #region Slurp and Set RK Fields
        public static bool IsLegalFieldName(string field)
        {
            char current;
            int i, length = field.Length;
            for (i = 0; i < length; i++)
            {
                current = field[i];
                if (current < '0') return false;
                if (current < '9' && i == 0) return false;
                if (current < 'A') return false;
                current = (char)(current | ' ');
                if (current > 'z') return false;
            }
            return true;
        }

        private static bool IsExcludedContentField(string field)
        {
            return field == "Value" ||
                   field == "Stream" ||
                   field == "AsBytes";
        }

        public static List<IResourceConnection> SlurpRKsFromIEnumerable(string absolutePath,
            string path, AApiVersionedFields rootField, IEnumerable list,
            List<RKContainer> containers, Predicate<IResourceKey> validate = null)
        {
            List<IResourceConnection> subKeys, keys = new List<IResourceConnection>();
            string elem, absoluteElem;
            int j = 0;
            foreach (var e in list)
            {
                elem = path + "[" + (j++) + "]";
                absoluteElem = absolutePath + "[" + j + "]";
                if (e is string)
                {
                    continue;
                }
                if (e is IEnumerable)
                {
                    subKeys = SlurpRKsFromIEnumerable(absoluteElem, elem, rootField,
                        e as IEnumerable, containers, validate);
                    keys.AddRange(subKeys);
                }
                if (e is AApiVersionedFields)
                {
                    subKeys = SlurpRKsFromField(absoluteElem, e as AApiVersionedFields,
                        containers, validate);
                    keys.AddRange(subKeys);
                }
                else if (e is TextReader)
                {
                    /* It is not possible to commit changes to any TextReader, BinaryReader, 
                       or value type element of an IEnumerable 
                       without replacing the entire IEnumerable instance in the content field,
                       and that isn't possible without prior knowledge 
                       of the IEnumerable content field's type and constructor. */
                }
                else if (e is BinaryReader)
                {
                }
                else if (e is IResourceKey && (validate == null || validate(e as IResourceKey)))
                {
                    keys.Add(new DefaultConnection(e as IResourceKey, rootField, ResourceDataActions.FindWrite, elem));
                }
            }
            return keys;
        }

        /// <summary>
        /// Recursively searches the given field and all its fields for 
        /// <see cref="T:s3pi.Interfaces.IResource"/> instances,
        /// and compiles them into a dictionary with the content field path
        /// to access them from the given root field.
        /// </summary>
        public static List<IResourceConnection> SlurpRKsFromField(string absolutePath,
            AApiVersionedFields rootField, List<RKContainer> containers,
            Predicate<IResourceKey> validate = null)
        {
            List<IResourceConnection> subKeys, keys = new List<IResourceConnection>();

            if (rootField is IResourceKey && (validate == null || validate(rootField as IResourceKey)))
                keys.Add(new DefaultConnection(rootField as IResourceKey, rootField,
                    ResourceDataActions.FindWrite, absolutePath));

            List<string> contentFields = rootField.ContentFields;
            TypedValue tv;
            string contentField, name, absoluteName;
            for (int i = 0; i < contentFields.Count; i++)
            {
                contentField = contentFields[i];
                if (IsExcludedContentField(contentField))
                    continue;
                tv = rootField[contentField];
                name = "root." + contentField;
                absoluteName = absolutePath + "." + contentField;

                // string is enumerable but we want to treat it as a single value
                if (typeof(string).IsAssignableFrom(tv.Type))
                    continue;

                else if (typeof(IEnumerable).IsAssignableFrom(tv.Type))
                {
                    subKeys = SlurpRKsFromIEnumerable(absoluteName, name, rootField,
                        tv.Value as IEnumerable, containers, validate);
                    keys.AddRange(subKeys);
                }
                else if (typeof(AApiVersionedFields).IsAssignableFrom(tv.Type))
                {
                    subKeys = SlurpRKsFromField(absoluteName,
                        tv.Value as AApiVersionedFields, containers, validate);
                    keys.AddRange(subKeys);
                }
                else if (typeof(TextReader).IsAssignableFrom(tv.Type))
                {
                    TextRKContainer textHelper = new TextRKContainer(name, rootField,
                        tv.Value as TextReader, absoluteName, validate);
                    keys.AddRange(textHelper.Owners);
                }
                else if (tv.Value is IResourceKey && (validate == null || validate(tv.Value as IResourceKey)))
                {
                    keys.Add(new DefaultConnection(tv.Value as IResourceKey, rootField, 
                        ResourceDataActions.FindWrite, name));
                }
            }

            return keys;
        }

        /// <param name="rkContainer">The container of the new resource key data;
        /// See remarks for currently supported types.</param>
        /// <param name="fieldPath">The string path of content fields
        /// and IEnumerable indices to follow to get to the Resource Key container
        /// to set to the value of the given container.</param>
        /// <remarks>
        /// Currently supported types for <paramref name="rkContainer"/>:
        /// <list type="bullet">
        /// <item><description>
        /// <see cref="T:s3pi.Interfaces.IResourceKey"/></description></item>
        /// <item><description>
        /// <see cref="T:System.IO.TextReader"/></description></item>
        /// <item><description>
        /// <see cref="T:System.IO.BinaryReader"/></description></item>
        /// </list>
        /// WARNING: The latter two items should be used with care,
        /// as they could result in an ArgumentException, 
        /// TargetException, or TargetInvocationException
        /// if the content field uses a derived type instead of the base type.
        /// </remarks>
        public static bool SetRKInField(object rkContainer,
            string fieldPath, AApiVersionedFields rootField)
        {
            string[] fieldNames = fieldPath.Split(new char[] { '.' },
                StringSplitOptions.RemoveEmptyEntries);
            IResourceKey newKey = rkContainer as IResourceKey;
            if (fieldNames.Length == 1)
            {
                IResourceKey rk = rootField as IResourceKey;
                rk.ResourceType = newKey.ResourceType;
                rk.ResourceGroup = newKey.ResourceGroup;
                rk.Instance = newKey.Instance;
            }
            int i, j, index, indexStrStart, indexStrLength;
            bool endOfPath = false;
            object elem, list = null;
            TypedValue tv;
            string name;
            AApiVersionedFields field = rootField;
            for (i = 1; i < fieldNames.Length; i++)
            {
                endOfPath = (i == (fieldNames.Length - 1));
                name = fieldNames[i];
                indexStrStart = name.IndexOf('[');
                if (indexStrStart == -1)
                {
                    tv = field[name];
                    if (typeof(IResourceKey).IsAssignableFrom(tv.Type) && endOfPath)
                    {
                        IResourceKey rk = (IResourceKey)tv.Value;
                        rk.ResourceType = newKey.ResourceType;
                        rk.ResourceGroup = newKey.ResourceGroup;
                        rk.Instance = newKey.Instance;
                        if (!tv.Type.IsClass)
                            field[name] = new TypedValue(tv.Type, rk);
                        return true;
                    }
                    else if (typeof(AApiVersionedFields).IsAssignableFrom(tv.Type))
                    {
                        field = tv.Value as AApiVersionedFields;
                    }
                    // TODO: Is there a danger of type mismatch?
                    else if (typeof(TextReader).IsAssignableFrom(tv.Type)
                        && rkContainer is TextReader)
                    {
                        field[name] = new TypedValue(tv.Type, rkContainer);
                    }
                    else if (typeof(BinaryReader).IsAssignableFrom(tv.Type)
                        && rkContainer is BinaryReader)
                    {
                        field[name] = new TypedValue(tv.Type, rkContainer);
                    }
                }
                else
                {
                    tv = field[name.Substring(0, indexStrStart)];
                    list = tv.Value;
                }
                while (indexStrStart != -1)
                {
                    indexStrLength = name.IndexOf(']');
                    if (indexStrLength == -1)
                        indexStrLength = name.Length - 1 - indexStrStart;
                    else
                        indexStrLength = indexStrLength - 1 - indexStrStart;
                    index = int.Parse(name.Substring(indexStrStart, indexStrLength));
                    if (list is IList)
                    {
                        elem = (list as IList)[index];
                        if (elem is IResourceKey && endOfPath)
                        {
                            IResourceKey rk = (IResourceKey)elem;
                            rk.ResourceType = newKey.ResourceType;
                            rk.ResourceGroup = newKey.ResourceGroup;
                            rk.Instance = newKey.Instance;
                            if (!elem.GetType().IsClass)
                                (list as IList)[index] = elem;
                            return true;
                        }
                        else if (elem is IEnumerable)
                        {
                            name = name.Substring(indexStrStart + indexStrLength + 1);
                            indexStrStart = name.IndexOf('[');
                            list = elem;
                        }
                        else if (elem is AApiVersionedFields)
                        {
                            field = elem as AApiVersionedFields;
                        }
                        else if (elem is TextReader && rkContainer is TextReader)
                        {
                            (list as IList)[index] = rkContainer;
                        }
                        else if (elem is BinaryReader && rkContainer is BinaryReader)
                        {
                            (list as IList)[index] = rkContainer;
                        }
                    }
                    else // IEnumerable is last resort: value type & reader elements can't be changed
                    {
                        j = -1;
                        foreach (var e in list as IEnumerable)
                        {
                            if (index == ++j)
                            {
                                if (e is IResourceKey && endOfPath)
                                {
                                    IResourceKey rk = (IResourceKey)e;
                                    rk.ResourceType = newKey.ResourceType;
                                    rk.ResourceGroup = newKey.ResourceGroup;
                                    rk.Instance = newKey.Instance;
                                    return true;
                                }
                                else if (e is IEnumerable)
                                {
                                    name = name.Substring(indexStrStart + indexStrLength + 1);
                                    indexStrStart = name.IndexOf('[');
                                    list = e;
                                }
                                else if (e is AApiVersionedFields)
                                {
                                    field = e as AApiVersionedFields;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        protected string absolutePath;
        protected string contentFieldPath;
        protected AApiVersionedFields rootField;
        protected Predicate<IResourceKey> validate;

        public string AbsolutePath
        {
            get { return this.absolutePath; }
        }

        public string ContentFieldPath
        {
            get { return this.contentFieldPath; }
        }

        public AApiVersionedFields RootField
        {
            get { return this.rootField; }
        }

        public virtual bool SetRK(IResourceKey oldKey, IResourceKey newKey)
        {
            return SetRKInField(newKey, this.contentFieldPath, this.rootField);
        }

        public virtual bool CommitChanges()
        {
            return true;
        }

        public RKContainer(string fieldPath, AApiVersionedFields rootField,
            string absolutePath, Predicate<IResourceKey> validate)
        {
            this.contentFieldPath = fieldPath;
            this.rootField = rootField;
            this.absolutePath = absolutePath;
            this.validate = validate;
        }
    }
}