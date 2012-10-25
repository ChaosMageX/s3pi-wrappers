using System;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public class DefaultConnection : AResourceConnection
    {
        protected RKContainer rkContainer;

        public override IResourceNode CreateChild(IResource resource, object constraints)
        {
            if (base.childDataActions < ResourceDataActions.Find)
                return ResourceNodeDealer.GetResource(rkContainer.RootField as ARCOLBlock,
                                                      base.OriginalChildKey);
            else
                return ResourceNodeDealer.GetResource(resource, base.OriginalChildKey);
        }

        /*public static bool SetRKInField(IResourceKey newKey, IResourceKey oldKey,
            string path, AApiVersionedFields rootField, 
            TextRKContainer textHelper = null)
        {
            string[] fieldNames = path.Split(new char[] { '.' },
                StringSplitOptions.RemoveEmptyEntries);
            if (fieldNames.Length == 1)
            {
                IResourceKey rk = rootField as IResourceKey;
                rk.Instance = newKey.Instance;
            }
            int i, j, index, indexStrStart, indexStrLength;
            object elem, list = null;
            TypedValue tv;
            string name;
            AApiVersionedFields field = rootField;
            for (i = 1; i < fieldNames.Length; i++)
            {
                name = fieldNames[i];
                indexStrStart = name.IndexOf('[');
                if (indexStrStart != -1)
                {
                    tv = field[name.Substring(0, indexStrStart)];
                    list = tv.Value;
                }
                else
                {
                    tv = field[name];
                    // TODO: Should we account for the possibility of value type resource keys?
                    if (typeof(IResourceKey).IsAssignableFrom(tv.Type))
                    {
                        IResourceKey rk = tv.Value as IResourceKey;
                        rk.Instance = newKey.Instance;
                        field[name] = new TypedValue(tv.Type, rk);
                        return true;
                    }
                    else if (typeof(TextReader).IsAssignableFrom(tv.Type))
                    {
                        textHelper.SetReferenceRK(oldKey, newKey);
                        field[name] = new TypedValue(tv.Type, textHelper.FlushReferenceRKs());
                        return true;
                    }
                    else
                    {
                        field = tv.Value as AApiVersionedFields;
                    }
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
                        if (elem is IResourceKey)
                        {
                            (elem as IResourceKey).Instance = newKey.Instance;
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
                    }
                    else
                    {
                        j = -1;
                        foreach (var e in list as IEnumerable)
                        {
                            if (index == ++j)
                            {
                                if (e is IResourceKey)
                                {
                                    IResourceKey rk = e as IResourceKey;
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
        }/**/

        public override bool SetParentReferenceRK(IResourceKey newKey)
        {
            return rkContainer.SetRK(base.OriginalChildKey, newKey);
        }

        /*public override bool SetParentReferenceRK(IResourceKey newKey)
        {
            string[] fieldNames = this.contentFieldPath.Split(new char[] { '.' }, 
                StringSplitOptions.RemoveEmptyEntries);
            if (fieldNames.Length == 1)
            {
                IResourceKey rk = this.rootField as IResourceKey;
                rk.Instance = newKey.Instance;
            }
            int i, j, index, indexStrStart, indexStrLength;
            TypedValue tv;
            string name;
            AApiVersionedFields field = this.rootField;
            for (i = 1; i < fieldNames.Length; i++)
            {
                name = fieldNames[i];
                indexStrStart = name.IndexOf('[');
                if (indexStrStart == -1)
                {
                    tv = field[name];
                    // TODO: Should we account for the possibility of value type resource keys?
                    if (typeof(IResourceKey).IsAssignableFrom(tv.Type))
                    {
                        IResourceKey rk = tv.Value as IResourceKey;
                        rk.Instance = newKey.Instance;
                        field[name] = tv;
                        return true;
                    }
                    if (typeof(TextReader).IsAssignableFrom(tv.Type))
                    {
                        this.textFieldHelper.SetReferenceRK(base.OriginalChildKey, newKey);
                        field[name] = new TypedValue(tv.Type, this.textFieldHelper.FlushReferenceRKs());
                    }
                    field = tv.Value as AApiVersionedFields;
                }
                else
                {
                    indexStrLength = name.IndexOf(']');
                    if (indexStrLength == -1)
                        indexStrLength = name.Length - 1 - indexStrStart;
                    else
                        indexStrLength = indexStrLength - 1 - indexStrStart;
                    index = int.Parse(name.Substring(indexStrStart, indexStrLength));
                    tv = field[name.Substring(0, indexStrStart)];
                    j = -1;
                    foreach (var e in tv.Value as IEnumerable)
                    {
                        if (index == ++j)
                        {
                            if (e is IResourceKey)
                            {
                                IResourceKey rk = e as IResourceKey;
                                rk.Instance = newKey.Instance;
                                return true;
                            }
                            else if (e is IEnumerable)
                            {
                                IEnumerable list = e as IEnumerable;
                                string suffix = name.Substring(indexStrStart + indexStrLength + 1);
                                indexStrStart = suffix.IndexOf('[');
                                while (indexStrStart != -1)
                                {
                                    indexStrLength = suffix.IndexOf(']');
                                    if (indexStrLength == -1)
                                        indexStrLength = suffix.Length - 1 - indexStrStart;
                                    else
                                        indexStrLength = indexStrLength - 1 - indexStrStart;
                                    int subIndex = int.Parse(suffix.Substring(indexStrStart, indexStrLength));
                                    int k = -1;
                                    foreach (var f in list)
                                    {
                                        if (index == ++k)
                                        {
                                            if (f is IResourceKey)
                                            {
                                                IResourceKey rk = f as IResourceKey;
                                                rk.Instance = newKey.Instance;
                                                return true;
                                            }
                                            else if (f is IEnumerable)
                                            {
                                                list = f as IEnumerable;
                                                suffix = suffix.Substring(indexStrStart + indexStrLength + 1);
                                                indexStrStart = suffix.IndexOf('[');
                                            }
                                            else
                                            {
                                                field = f as AApiVersionedFields;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                field = e as AApiVersionedFields;
                            }
                        }
                    }
                }
            }
            return false;
        }/**/

        public DefaultConnection(IResourceKey rk,
                                 AApiVersionedFields rkField, ResourceDataActions childActions,
                                 string absolutePath, string rkFieldPath = "root",
                                 Predicate<IResourceKey> validate = null)
            : base(rk, absolutePath, childActions)
        {
            rkContainer = new RKContainer(rkFieldPath, rkField, absolutePath, validate);
        }

        public DefaultConnection(IResourceKey rk,
                                 RKContainer rkContainer, ResourceDataActions childActions,
                                 string absolutePath)
            : base(rk, absolutePath, childActions)
        {
            this.rkContainer = rkContainer;
        }
    }
}
