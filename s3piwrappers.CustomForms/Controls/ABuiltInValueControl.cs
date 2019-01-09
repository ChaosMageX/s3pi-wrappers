using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace s3piwrappers.CustomForms.Controls
{
    public abstract class ABuiltInValueControl : IBuiltInValueControl
    {
        public abstract bool IsAvailable { get; }
        public abstract Control ValueControl { get; }
        public abstract IEnumerable<ToolStripItem> GetContextMenuItems(EventHandler cbk);

        protected ABuiltInValueControl(Stream s) { }

        static List<KeyValuePair<List<uint>, Type>> builtInValueControlLookup = new List<KeyValuePair<List<uint>, Type>>();
        static ABuiltInValueControl()
        {
            Type t;
            FieldInfo fi;
            Type[] types = typeof(ABuiltInValueControl).Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                t = types[i];
                if (!t.IsAbstract && typeof(ABuiltInValueControl).IsAssignableFrom(t))
                {
                    fi = t.GetField("resourceTypes", BindingFlags.NonPublic | BindingFlags.Static);
                    if (fi != null &&
                        fi.FieldType.HasElementType &&
                        fi.FieldType.GetElementType() == typeof(uint))
                    {
                        builtInValueControlLookup.Add(new KeyValuePair<List<uint>, Type>(
                                           new List<uint>((uint[])fi.GetValue(fi)), t));
                    }
                }
            }
        }

        public static bool Exists(uint resourceType)
        {
            return Lookup(resourceType, Stream.Null) != null;
        }

        public static IBuiltInValueControl Lookup(uint resourceType, Stream s)
        {
            object[] args = new object[] { s, };
            ABuiltInValueControl control;
            foreach (var x in builtInValueControlLookup)
            {
                if (x.Key.Contains(resourceType))
                {
                    control = Activator.CreateInstance(x.Value, args) as ABuiltInValueControl;
                    if (control.IsAvailable)
                        return control;
                }
            }
            return null;
        }
    }
}
