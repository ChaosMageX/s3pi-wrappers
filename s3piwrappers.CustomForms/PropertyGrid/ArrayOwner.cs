using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    public class ArrayOwner : AApiVersionedFields
    {
        AApiVersionedFields owner;
        string field;
        TypedValue tv;
        IList list;
        public ArrayOwner(AApiVersionedFields owner, string field) { this.owner = owner; this.field = field; tv = owner[field]; list = (IList)tv.Value; }

        public Type ElementType { get { return tv.Value.GetType().GetElementType(); } }

        static System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^\[([\dA-F]+)\].*$");
        public override TypedValue this[string index]
        {
            get
            {
                if (!regex.IsMatch(index))
                    throw new ArgumentOutOfRangeException();
                int i = Convert.ToInt32("0x" + regex.Match(index).Groups[1].Value, 16);

                return new TypedValue(ElementType, ((IList)tv.Value)[i]);
            }
            set
            {
                if (!regex.IsMatch(index))
                    throw new ArgumentOutOfRangeException();
                int i = Convert.ToInt32("0x" + regex.Match(index).Groups[1].Value, 16);

                //list[i] = value.Value; <-- BYPASSES "new" set method in AHandlerList<T>
                //Lists
                PropertyInfo p = list.GetType().GetProperty("Item");
                if (p != null)
                    p.SetValue(list, value.Value, new object[] { i, });
                else
                {
                    //Arrays
                    MethodInfo m = list.GetType().GetMethod("Set");
                    if (m != null)
                        m.Invoke(list, new object[] { i, value.Value });
                    owner[field] = new TypedValue(tv.Type, list);
                }
            }
        }

        public override List<string> ContentFields
        {
            get
            {
                List<string> res = new List<string>();
                string fmt = "[{0:X" + list.Count.ToString("X").Length + "}] {1}";
                for (int i = 0; i < list.Count; i++)
                    res.Add(String.Format(fmt, i, field));
                return res;
            }
        }

        public override int RecommendedApiVersion { get { return 0; } }
    }
}
