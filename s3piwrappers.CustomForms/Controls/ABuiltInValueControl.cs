using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var types = typeof(ABuiltInValueControl).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(ABuiltInValueControl).IsAssignableFrom(t));
            foreach (var t in types)
            {
                var fi = t.GetField("resourceTypes", BindingFlags.NonPublic | BindingFlags.Static);
                if (fi == null)
                    continue;
                if (!fi.FieldType.HasElementType || fi.FieldType.GetElementType() != typeof(uint))
                    continue;
                builtInValueControlLookup.Add(new KeyValuePair<List<uint>, Type>(
                   new List<uint>((uint[])fi.GetValue(fi)), t));
            }
        }

        public static bool Exists(uint resourceType)
        {
            return builtInValueControlLookup
                .Where(x => x.Key.Contains(resourceType))
                .Select(x => Activator.CreateInstance(x.Value, new object[] { Stream.Null, }) as ABuiltInValueControl)
                .Where(x => x.IsAvailable)
                .FirstOrDefault() != null;
        }

        public static IBuiltInValueControl Lookup(uint resourceType, Stream s)
        {
            return builtInValueControlLookup
                .Where(x => x.Key.Contains(resourceType))
                .Select(x => Activator.CreateInstance(x.Value, new object[] { s, }) as ABuiltInValueControl)
                .Where(x => x.IsAvailable)
                .FirstOrDefault();
        }
    }
}
