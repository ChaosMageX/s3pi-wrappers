using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    /// <summary>
    /// Responsible for associating ResourceType in the <see cref="IResourceKey"/> 
    /// with a particular class (a "node") that can contain it
    /// or the default resource node.
    /// </summary>
    public static class ResourceNodeDealer
    {
        /// <summary>
        /// Retrieve a resource node to contain the given resource, 
        /// readying the appropriate node class or the default node class
        /// </summary>
        /// <param name="resource">The resource to contain in a resource node</param>
        /// <param name="resourceKey">The identifying key of the given resource,
        /// which is used to find the right node to contain it</param>
        /// <param name="AlwaysDefault">When true, indicates ResourceNodeDealer 
        /// should always use the <see cref="DefaultNode"/> class</param>
        /// <returns>A resource node containing the given resource</returns>
        public static IResourceNode GetResource(IResource resource, IResourceKey resourceKey, 
            bool AlwaysDefault = false)
        {
            if (AlwaysDefault)
                return new DefaultNode(resource, resourceKey);
            else
                return NodeForType(resource, resourceKey);
        }

        /// <summary>
        /// Retrieve the resource node classes known to ResourceNodeDealer.
        /// </summary>
        public static KeyValuePair<uint, Type>[] TypeMap { get { return typeMap.ToArray(); } }

        /// <summary>
        /// Access the list of resource node types on the "disabled" list.
        /// </summary>
        /// <remarks>Updates to entries in the collection will be used next time a resource node is requested.
        /// Existing instances of a disabled node type will not be invalidated and it will remain possible to
        /// bypass ResourceNodeDealer and instantiate instances of the resource node class directly.</remarks>
        public static List<KeyValuePair<uint, Type>> Disabled { get { return disabled; } }

        #region Implementation
        private sealed class KVPComparer : IComparer<KeyValuePair<uint, Type>>
        {
            public int Compare(KeyValuePair<uint, Type> x, KeyValuePair<uint, Type> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }

        private static List<KeyValuePair<uint, Type>> typeMap;

        private static List<KeyValuePair<uint, Type>> disabled = new List<KeyValuePair<uint,Type>>();

        static ResourceNodeDealer()
        {
            string path, folder = Path.GetDirectoryName(typeof(ResourceNodeDealer).Assembly.Location);
            typeMap = new List<KeyValuePair<uint, Type>>();
            string[] dllPaths = Directory.GetFiles(folder, "*.dll");
            uint[] tids;
            AResourceNodeHandler arnhs;
            Type t;
            int i, j, k;
            for (i = 0; i < dllPaths.Length; i++)
            {
                path = dllPaths[i];
                try
                {
                    Assembly dotNetDll = Assembly.LoadFile(path);
                    Type[] types = dotNetDll.GetTypes();
                    for (j = 0; j < types.Length; j++)
                    {
                        t = types[j];
                        if (!t.IsSubclassOf(typeof(AResourceNodeHandler))) continue;

                        arnhs = (AResourceNodeHandler)t.GetConstructor(new Type[0]).Invoke(new object[0]);

                        if (arnhs == null) continue;

                        foreach (KeyValuePair<Type, uint[]> arnh in arnhs)
                        {
                            tids = arnh.Value;
                            if (tids != null)
                            {
                                for (k = 0; k < tids.Length; k++)
                                {
                                    typeMap.Add(new KeyValuePair<uint, Type>(tids[k], arnh.Key));
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            typeMap.Sort(new KVPComparer());
        }

        private static IResourceNode NodeForType(IResource resource, IResourceKey resourceKey)
        {
            bool flag;
            uint type = resourceKey.ResourceType;
            KeyValuePair<uint, Type> kvp;
            Type t = null;
            int i, j;
            for (i = 0; i < typeMap.Count && t == null; i++)
            {
                kvp = typeMap[i];
                if (kvp.Key == type)
                {
                    flag = true;
                    for (j = 0; j < disabled.Count && flag; j++)
                    {
                        if (disabled[j].Key == type &&
                            disabled[j].Value.Equals(kvp.Value))
                            flag = false;
                    }
                    if (flag)
                        t = kvp.Value;
                }
            }
            if (t == null)
            {
                return new DefaultNode(resource, resourceKey);
            }

            return (IResourceNode)t.GetConstructor(new Type[] { typeof(IResource), typeof(IResourceKey), })
                .Invoke(new object[] { resource, resourceKey });
        }
        #endregion
    }
}
