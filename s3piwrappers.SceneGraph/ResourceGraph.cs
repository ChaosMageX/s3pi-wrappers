using System;
using System.Collections;
using System.Collections.Generic;
using s3pi.Extensions;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3pi.Package;
using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph
{
    // The connection core's job is to return connection core implementations
    // with a function that modifies part of their parent's resource data.

    // What if a parent resource references the same child resource 
    // more than once in its resource data?
    // Simple: It is up to the user to determine whether to node core
    // returns multiple identical connections to the same child.
    // The graph just makes sure the child resource isn't instantiated
    // for each duplicate connection unless the connection core implementation
    // explicitly states that a new instance of the resource node must be
    // instantiated for each instance of the connection.
    public class ResourceGraph
    {
        #region RK Type

        private static readonly uint[] thumResources = new uint[]
            {
                0x0580A2B4, 0x0580A2B5, 0x0580A2B6,
                0x0589DC44, 0x0589DC45, 0x0589DC46, //this one is on the wiki twice
                0x05B17698, 0x05B17699, 0x05B1769A,
                0x05B1B524, 0x05B1B525, 0x05B1B526,
                0x2653E3C8, 0x2653E3C9, 0x2653E3CA,
                0x2D4284F0, 0x2D4284F1, 0x2D4284F2,
                0x5DE9DBA0, 0x5DE9DBA1, 0x5DE9DBA2,
                0x626F60CC, 0x626F60CD, 0x626F60CE //, 0xFCEAB65B
            };

        public static bool IsThum(uint resourceType)
        {
            for (int i = 0; i < thumResources.Length; i++)
                if (resourceType == thumResources[i])
                    return true;
            return false;
        }

/**/

        /*private static readonly uint[] ddsResources = new uint[] {
            0x00B2D882, 0x8FFB80F6,
        };
        public static bool IsDDS(uint resourceType)
        {
            for (int i = 0; i < ddsResources.Length; i++) 
                if (resourceType == ddsResources[i]) 
                    return true;
            return false;
        }/**/

        public static bool IsDDS(uint resourceType)
        {
            return resourceType == 0x00B2D882 ||
                resourceType == 0x8FFB80F6;
        }

        public enum RFileType
        {
            Unknown,
            Game,
            DDS,
            Thum
        }

        public static RFileType GetFileType(uint resourceType)
        {
            if (IsDDS(resourceType))
                return RFileType.DDS;
            else if (IsThum(resourceType))
                return RFileType.Thum;
            else
                return RFileType.Game;
        }

        #endregion

        #region RK Content Field Search and Retrieval

        /*private static readonly string[] excludes = new string[] { "Value", "Stream", "AsBytes", };
        private static bool IsExcludedContentField(string field)
        {
            for (int i = 0; i < excludes.Length; i++) if (field == excludes[i]) return true;
            return false;
        }/**/

        private static bool IsExcludedContentField(string field)
        {
            return field == "Value" ||
                field == "Stream" ||
                field == "AsBytes";
        }

        public static Dictionary<string, R> SlurpRKsFromRK<R>(string key,
                                                              IResourceKey rk, bool includeDDSes = true)
            where R : class, IResourceKey
        {
            var item = new SpecificResource(FileTable.GameContent, rk);
            if (item.Resource != null) return SlurpRKsFromField<R>(key, (AResource) item.Resource, includeDDSes);
            else Diagnostics.Show(String.Format("RK {0} not found", key));
            return new Dictionary<string, R>();
        }

        // This is meant to be used to retrieve and change the part of the resource data
        // referenced by a node created with the SlurpRKsFromField function.
        // The path string should be stored in the connection itself, 
        // since it's code in the connection that actually alters its parent's resource data.
        public static R GetRKfromField<R>(AApiVersionedFields field, string path)
            where R : class, IResourceKey
        {
            string[] fieldNames = path.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (fieldNames.Length == 0)
            {
                return null;
            }
            if (fieldNames.Length == 1)
            {
                return field as R;
            }
            int i, j, index, indexStrStart, indexStrLength;
            TypedValue tv;
            string name;
            for (i = 1; i < fieldNames.Length; i++)
            {
                name = fieldNames[i];
                indexStrStart = name.IndexOf('[');
                if (indexStrStart == -1)
                {
                    try
                    {
                        tv = field[name];
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    field = tv.Value as AApiVersionedFields;
                    if (field == null)
                    {
                        return null;
                    }
                }
                else
                {
                    indexStrLength = name.IndexOf(']');
                    if (indexStrLength == -1)
                        indexStrLength = name.Length - 1 - indexStrStart;
                    else
                        indexStrLength = indexStrLength - 1 - indexStrStart;
                    if (!int.TryParse(name.Substring(indexStrStart, indexStrLength), out index))
                    {
                        return null;
                    }
                    try
                    {
                        tv = field[name.Substring(0, indexStrStart)];
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    if (!typeof (IEnumerable).IsAssignableFrom(tv.Type))
                    {
                        return null;
                    }
                    j = 0;
                    foreach (object e in tv.Value as IEnumerable)
                    {
                        if (j == index)
                        {
                            field = e as AApiVersionedFields;
                            break;
                        }
                        j++;
                    }
                    if (field == null)
                    {
                        return null;
                    }
                }
            }
            return field as R;
        }

        /// <summary>
        ///   Recursively searches the given field and all its fields for 
        ///   <see cref="T:s3pi.Interfaces.TGIBlock" /> instances,
        ///   and compiles them into a dictionary with the content field path
        ///   to access them from the given root field.
        /// </summary>
        public static Dictionary<string, R> SlurpRKsFromField<R>(string key,
                                                                 AApiVersionedFields field, bool includeDDSes = true)
            where R : class, IResourceKey
        {
            //.FindAll(key, x => x is TGIBlock || x is GenericRCOLResource.ChunkEntry)//Don't look inside ChunkEntries
            //.Where(x => 
            //    x.Item2 is TGIBlock//but we only actually want TGIBlocks
            //http://dino.drealm.info/develforums/s3pi/index.php?topic=1190.0 and maybe not DDSes
            //    && (includeDDSes || (!ddsResources.Contains((x.Item2 as TGIBlock).ResourceType))))

            Dictionary<string, R> subKeys, keys = new Dictionary<string, R>();

            if (field is R && (includeDDSes || !IsDDS((field as R).ResourceType)))
                keys.Add(key, field as R);

            List<string> contentFields = field.ContentFields;
            TypedValue tv;
            string contentField, elem, name;
            int i, j;
            for (i = 0; i < contentFields.Count; i++)
            {
                contentField = contentFields[i];
                if (IsExcludedContentField(contentField))
                    continue;
                tv = field[contentField];
                name = key + "." + contentField;

                // string is enumerable but we want to treat it as a single value
                if (typeof (string).IsAssignableFrom(tv.Type))
                    continue;

                else if (typeof (IEnumerable).IsAssignableFrom(tv.Type))
                {
                    j = 0;
                    foreach (object e in tv.Value as IEnumerable)
                    {
                        elem = name + "[" + (j++) + "]";
                        if (e is AApiVersionedFields)
                        {
                            subKeys = SlurpRKsFromField<R>(elem, e as AApiVersionedFields, includeDDSes);
                            foreach (KeyValuePair<string, R> subKey in subKeys)
                                keys.Add(subKey.Key, subKey.Value);
                        }
                        else if (e is R && (includeDDSes || !IsDDS((e as R).ResourceType)))
                            keys.Add(elem, e as R);
                    }
                }
                else if (typeof (AApiVersionedFields).IsAssignableFrom(tv.Type))
                {
                    subKeys = SlurpRKsFromField<R>(name, tv.Value as AApiVersionedFields, includeDDSes);
                    foreach (KeyValuePair<string, R> subKey in subKeys)
                        keys.Add(subKey.Key, subKey.Value);
                }
                else if (tv.Value is R && (includeDDSes || !IsDDS((tv.Value as R).ResourceType)))
                    keys.Add(name, tv.Value as R);
            }

            return keys;
        }

        private sealed class SlurpKindredRKsHelper
        {
            public readonly List<IResourceIndexEntry> Seen = new List<IResourceIndexEntry>();
            public readonly IResourceKey ParentKey;
            public readonly IResourceKinHelper KinHelper;

            public SlurpKindredRKsHelper(IResourceKey parentKey, IResourceKinHelper kinHelper)
            {
                ParentKey = parentKey;
                KinHelper = kinHelper;
            }

            public bool IsKindred(IResourceIndexEntry rie)
            {
                if (KinHelper.IsKindred(ParentKey, rie))
                {
                    var rk = new RK(rie);
                    for (int i = 0; i < Seen.Count; i++)
                    {
                        if (rk.Equals(Seen[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public static List<SpecificResource> SlurpKindredResources(
            IResourceKey parentKey, IResourceKinHelper kinHelper)
        {
            var resources = new List<SpecificResource>();
            int i, j;
            List<PathPackageTuple> ppts = FileTable.GameContent;
            if (kinHelper.IsKinDDS) ppts = FileTable.DDSImages;
            if (kinHelper.IsKinThum) ppts = FileTable.Thumbnails;
            if (ppts != null)
            {
                var helper = new SlurpKindredRKsHelper(parentKey, kinHelper);
                List<IResourceIndexEntry> entries;
                for (i = 0; i < ppts.Count; i++)
                {
                    entries = ppts[i].Package.FindAll(helper.IsKindred);
                    if (entries != null)
                    {
                        for (j = 0; j < entries.Count; j++)
                        {
                            helper.Seen.Add(entries[j]);
                            resources.Add(new SpecificResource(ppts[i], entries[j]));
                        }
                    }
                }
            }
            return resources;
        }

        #endregion

        public static string PrintRK(IResourceKey key)
        {
            return string.Format("{0:X8}:{1:X8}:{2:X16}",
                                 key.ResourceType, key.ResourceGroup, key.Instance);
        }

        public static string PrintRKRef(IResourceKey key, string fieldPath)
        {
            return string.Format("{0:X8}:{1:X8}:{2:X16} at {3}",
                                 key.ResourceType, key.ResourceGroup, key.Instance,
                                 fieldPath);
        }

        public static string PrintRKTag(IResourceKey key, string tag)
        {
            return string.Format("{3}:{0:X8}:{1:X8}:{2:X16}",
                                 key.ResourceType, key.ResourceGroup, key.Instance, tag);
        }

        public class GraphNode
        {
            private readonly string originalName;
            private string name;
            private RK key;
            private readonly RFileType fileType;

            /// <summary>
            ///   The original unique key of the resource stored in this node:
            ///   the resource being cloned.  This is a readonly value
            ///   set in the node's constructor.
            /// </summary>
            public readonly RK OriginalKey;

            public readonly string ExtensionTag;
            //public readonly bool IsRCOLBlock;
            public readonly ResourceDataActions NodeActions;

            protected internal ResourceGraph Graph;
            protected internal IResourceNode Core;
            protected internal PathPackageTuple Origin;
            protected internal List<GraphConnectionRef> ParentConnections = new List<GraphConnectionRef>();
            protected internal List<GraphConnectionRef> ChildConnections = new List<GraphConnectionRef>();
            protected internal List<GraphConnectionKin> KinOwnerConnections = new List<GraphConnectionKin>();
            protected internal List<GraphConnectionKin> KindredConnections = new List<GraphConnectionKin>();

            /// <summary>
            ///   True if this node is kindred to another node
            ///   and should not have its resource key directly set
            ///   because it will be set by its next of kin.
            /// </summary>
            public bool IsKindred
            {
                get { return KinOwnerConnections.Count > 0; }
            }

            public GraphNode(ResourceGraph graph, IResourceKey originalKey, IResourceNode core,
                             PathPackageTuple origin, ResourceDataActions nodeActions, RFileType fileType = RFileType.Unknown)
            {
                Graph = graph;
                key = new RK(originalKey);
                OriginalKey = new RK(originalKey);
                ExtensionTag = ExtList.Ext[originalKey.ResourceType][0];
                NodeActions = nodeActions;
                Core = core;
                Origin = origin;
                if (fileType == RFileType.Unknown)
                    this.fileType = GetFileType(originalKey.ResourceType);
                else
                    this.fileType = fileType;
                switch (this.fileType)
                {
                case RFileType.Game:
                    name = NameMap.GameNMap.GetName(originalKey.Instance, origin);
                    break;
                case RFileType.DDS:
                    name = NameMap.DDSNMap.GetName(originalKey.Instance, origin);
                    break;
                case RFileType.Thum:
                    name = NameMap.ThumNMap.GetName(originalKey.Instance, origin);
                    break;
                }
                originalName = name;
            }

            public void ResetName()
            {
                name = originalName;
            }

            public void ResetKey()
            {
                if (!key.Equals(OriginalKey))
                {
                    string log = "Resetting " + PrintRKTag(key, ExtensionTag)
                        + " back to " + PrintRK(OriginalKey);
                    Diagnostics.Log("Started " + log);
                    var rk = new RK(OriginalKey);
                    //this.OriginalKey.ResourceType = this.key.ResourceType;
                    //this.OriginalKey.ResourceGroup = this.key.ResourceGroup;
                    OriginalKey.Instance = key.Instance;
                    Key = rk;
                    //this.OriginalKey.ResourceType = this.key.ResourceType;
                    //this.OriginalKey.ResourceGroup = this.key.ResourceGroup;
                    OriginalKey.Instance = key.Instance;
                    Diagnostics.Log("Finished " + log);
                }
            }

            internal void RemoveFromGraph(bool removeOrphans)
            {
                Diagnostics.Log("Removing " + PrintRKTag(key, ExtensionTag)
                    + ":\"" + name + "\"...");
                ResetKey();
                ResetName();
                string logName = PrintRKTag(key, ExtensionTag)
                    + ":\"" + name + "\"...";
                GraphNode node;
                int i, j, subCount, count;

                Diagnostics.Log("Disconnecting Parents of " + logName);
                GraphConnectionRef gcr;
                count = ParentConnections.Count;
                for (i = 0; i < count; i++)
                {
                    gcr = ParentConnections[i];
                    node = gcr.Parent;
                    subCount = node.ChildConnections.Count;
                    for (j = 0; j < subCount; j++)
                    {
                        if (gcr.Equals(node.ChildConnections[j]))
                        {
                            node.ChildConnections.RemoveAt(j);
                            break;
                        }
                    }
                }
                Diagnostics.Log("Parents Disconnected: " + count);
                ParentConnections.Clear();

                Diagnostics.Log("Disconnecting Kin Owners of " + logName);
                GraphConnectionKin gck;
                count = KinOwnerConnections.Count;
                for (i = 0; i < count; i++)
                {
                    gck = KinOwnerConnections[i];
                    node = gck.Parent;
                    subCount = node.KindredConnections.Count;
                    for (j = 0; j < subCount; j++)
                    {
                        if (gck.Equals(node.KindredConnections[j]))
                        {
                            node.KindredConnections.RemoveAt(j);
                            break;
                        }
                    }
                }
                Diagnostics.Log("Kin Owners Disconnected: " + count);
                KinOwnerConnections.Clear();

                Diagnostics.Log("Disconnecting Children of " + logName);
                count = ChildConnections.Count;
                for (i = 0; i < count; i++)
                {
                    gcr = ChildConnections[i];
                    node = gcr.Child;
                    subCount = node.ParentConnections.Count;
                    for (j = 0; j < subCount; j++)
                    {
                        if (gcr.Equals(node.ParentConnections[j]))
                        {
                            node.ParentConnections.RemoveAt(j);
                            break;
                        }
                    }
                }
                Diagnostics.Log("Children Disconnected: " + count);

                Diagnostics.Log("Disconnecting Kin of " + logName);
                count = KindredConnections.Count;
                for (i = 0; i < count; i++)
                {
                    gck = KindredConnections[i];
                    node = gck.Child;
                    subCount = node.KinOwnerConnections.Count;
                    for (j = 0; j < subCount; j++)
                    {
                        if (gck.Equals(node.KinOwnerConnections[j]))
                        {
                            node.KinOwnerConnections.RemoveAt(j);
                            break;
                        }
                    }
                }
                Diagnostics.Log("Kin Disconnected: " + count);

                if (removeOrphans)
                {
                    Diagnostics.Log("Removing Orphaned Children and Kin of " + logName);
                    count = ChildConnections.Count;
                    for (i = 0; i < count; i++)
                    {
                        node = ChildConnections[i].Child;
                        if (node.ParentConnections.Count == 0 &&
                            node.KinOwnerConnections.Count == 0)
                        {
                            node.RemoveFromGraph(removeOrphans);
                        }
                    }

                    count = KindredConnections.Count;
                    for (i = 0; i < count; i++)
                    {
                        node = KindredConnections[i].Child;
                        if (node.KinOwnerConnections.Count == 0 &&
                            node.ParentConnections.Count == 0)
                        {
                            node.RemoveFromGraph(removeOrphans);
                        }
                    }
                }
                ChildConnections.Clear();
                KindredConnections.Clear();

                Diagnostics.Log("Removing From Lookup Table: " + logName);
                subCount = -1;
                count = Graph.nodeLookupTable.Count;
                for (i = 0; i < count && subCount < 0; i++)
                {
                    if (Equals(Graph.nodeLookupTable[i]))
                    {
                        Graph.nodeLookupTable.RemoveAt(i);
                        subCount = i;
                    }
                }
                if (subCount < 0)
                    Diagnostics.Log("Removing From Duplicate Table: " + logName);
                count = Graph.nodeDuplicates.Count;
                for (i = 0; i < count && subCount < 0; i++)
                {
                    if (Equals(Graph.nodeDuplicates[i]))
                    {
                        Graph.nodeDuplicates.RemoveAt(i);
                        subCount = i;
                    }
                }
                Diagnostics.Log("Removal Complete");
            }

            /// <summary>
            ///   This is the name of the resource stored in this node,
            ///   which is retrieved from and written to name map resources.
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            ///   This is the unique key of the resource stored in this node.
            ///   When this is set, the renaming functions of all the
            ///   parent connections are invoked to set the corresponding
            ///   references in their resource data.
            /// </summary>
            public RK Key
            {
                get { return new RK(key); }
                set
                {
                    if (!key.Equals(value))
                    {
                        string newKeyStr = PrintRK(value);
                        int i, count = ParentConnections.Count;
                        for (i = 0; i < count; i++)
                        {
                            if (!ParentConnections[i].Core.SetParentReferenceRK(value))
                            {
                                Diagnostics.Log("Set Ref RK " + newKeyStr
                                    + " Failed for " + ParentConnections[i].Core.AbsolutePath);
                            }
                        }
                        count = KindredConnections.Count;
                        for (i = 0; i < count; i++)
                        {
                            KindredConnections[i].SetKindredRK(value);
                        }
                        if (!Core.SetRK(value, OriginalKey))
                        {
                            Diagnostics.Log("Setting RK " + newKeyStr + " Failed for "
                                + PrintRKTag(OriginalKey, ExtensionTag));
                        }
                        key = value;
                    }
                }
            }

            /// <summary>
            ///   A list of all connections that point to this node,
            ///   and have their <see cref="GraphConnection.Child" />
            ///   set to this node.  When the IID of this node is set, 
            ///   this list is used to set the corresponding references
            ///   in the resource data of every node on the other end
            ///   of these connections.
            /// </summary>
            public GraphConnection[] Parents
            {
                get { return ParentConnections.ToArray(); }
            }

            /// <summary>
            ///   A list of all connections originating from this node,
            ///   and have their <see cref="GraphConnection.Parent" />
            ///   set to this node.  This list is generated by the node's 
            ///   implementation of <see cref="" />, 
            ///   and contains all nodes referenced in this node's resource data.
            ///   When the IIDs of the nodes on the other end of these connections 
            ///   are set, their corresponding IIDs are set in this node's resource data.
            /// </summary>
            public GraphConnection[] Children
            {
                get { return ChildConnections.ToArray(); }
            }
        }

        public abstract class GraphConnection
        {
            /// <summary>
            ///   The Arrow Tail.
            ///   The Resource Node that generated this connection,
            ///   and will have the references in its resource data 
            ///   changed by this connection.
            /// </summary>
            protected internal GraphNode Parent;

            /// <summary>
            ///   The Arrow Tip.
            ///   The Resource Node that this connection references.
            ///   When the IID of this node is set,
            ///   this connection changes all IIDs in the resource data
            ///   of the <see cref="Parent" /> to match.
            /// </summary>
            protected internal GraphNode Child;

            /// <summary>
            ///   The parent resource graph that contains this connection.
            /// </summary>
            protected internal ResourceGraph Graph;

            public GraphConnection(ResourceGraph graph, GraphNode parent)
            {
                Graph = graph;
                Parent = parent;
            }
        }

        public class GraphConnectionRef : GraphConnection
        {
            protected internal IResourceConnection Core;

            public GraphConnectionRef(ResourceGraph graph, GraphNode parent, IResourceConnection core)
                : base(graph, parent)
            {
                Core = core;
            }
        }

        public class GraphConnectionKin : GraphConnection
        {
            protected internal IResourceKinHelper Core;

            public GraphConnectionKin(ResourceGraph graph, GraphNode parent, IResourceKinHelper core)
                : base(graph, parent)
            {
                Core = core;
            }

            protected internal void SetKindredRK(IResourceKey newParentKey)
            {
                IResourceKey kinKey = Child.Key;
                Core.CreateKindredRK(Parent.OriginalKey, newParentKey, ref kinKey);
                Child.Key = new RK(kinKey);
            }
        }

        private readonly List<GraphNode> nodeLookupTable = new List<GraphNode>();
        // This is used for storing nodes that already exist in the lookup table,
        // but were re-created because the connection's AlwaysCreateChild was true.
        private readonly List<GraphNode> nodeDuplicates = new List<GraphNode>();

        private void CreateChildren(GraphNode graphNode, object constraints)
        {
            string graphNodeKeyStr = PrintRKTag(graphNode.OriginalKey, graphNode.ExtensionTag);
            int i, j, k, count, foundIndex;
            GraphNode childNode;
            GraphConnectionRef childConnection;
            IResourceConnection child;
            IResourceKey childKey;
            List<IResourceConnection> children = graphNode.Core.SlurpConnections(constraints);
            count = (children == null) ? 0 : children.Count;
            Diagnostics.Log("Resolving " + count + " RK Refs from " + graphNodeKeyStr);
            for (i = 0; i < count; i++)
            {
                child = children[i];
                childKey = child.OriginalChildKey;
                childConnection = new GraphConnectionRef(this, graphNode, child);
                childNode = null;
                // Try to find an already existing child node in the lookup table.
                foundIndex = -1;
                for (j = 0; j < nodeLookupTable.Count && foundIndex < 0; j++)
                {
                    if (nodeLookupTable[j].OriginalKey.Equals(childKey))
                        foundIndex = j;
                }
                if (foundIndex >= 0 && !child.AlwaysCreateChild)
                {
                    childNode = nodeLookupTable[foundIndex];
                }
                else
                {
                    var fileType = RFileType.Game;
                    SpecificResource sr = null;
                    IResourceNode childCore = null;
                    if (child.ChildDataActions < ResourceDataActions.Find)
                    {
                        childCore = child.CreateChild(null, constraints);
                    }
                    else
                    {
                        List<PathPackageTuple> searchList; // = child.IsChildDDS ? FileTable.DDSImages :
                        //child.IsChildThum ? FileTable.Thumbnails : FileTable.GameContent;
                        if (child.IsChildDDS)
                        {
                            fileType = RFileType.DDS;
                            searchList = FileTable.DDSImages;
                        }
                        else if (child.IsChildThum)
                        {
                            fileType = RFileType.Thum;
                            searchList = FileTable.Thumbnails;
                        }
                        else
                        {
                            fileType = RFileType.Game;
                            searchList = FileTable.GameContent;
                        }
                        sr = new SpecificResource(searchList, child.OriginalChildKey);
                        if (sr.ResourceIndexEntry == null || sr.Resource == null)
                        {
                            Diagnostics.Log("Unresolved RK Ref:" + PrintRKRef(childKey, child.AbsolutePath)
                                + ((sr.Exception == null) ? "" :
                                                                   string.Concat(" Error:\r\n", sr.Exception.ToString())));
                        }
                        else
                        {
                            childCore = child.CreateChild(sr.Resource, constraints);
                        }
                    }
                    if (childCore == null)
                    {
                        Diagnostics.Log("CreateChild Failed:" + PrintRKRef(childKey, child.AbsolutePath));
                    }
                    else
                    {
                        childNode = new GraphNode(this, childKey, childCore,
                                                  sr == null ? null : sr.PathPackage, child.ChildDataActions, fileType);
                        if (child.AlwaysCreateChild && foundIndex >= 0)
                        {
                            Diagnostics.Log("Dup Ref Node Added:" + PrintRKRef(childKey, child.AbsolutePath));
                            nodeDuplicates.Add(childNode);
                        }
                        else
                        {
                            Diagnostics.Log("New Ref Node Added:" + PrintRKRef(childKey, child.AbsolutePath));
                            nodeLookupTable.Add(childNode);
                        }
                        CreateChildren(childNode, constraints);
                    }
                }
                if (childNode != null)
                {
                    graphNode.ChildConnections.Add(childConnection);
                    childConnection.Child = childNode;
                    childNode.ParentConnections.Add(childConnection);
                }
            }
            IResourceIndexEntry kinKey;
            GraphConnectionKin kinConnection;
            IResourceKinHelper kinHelper;
            List<SpecificResource> kin;
            List<IResourceKinHelper> kinHelpers = graphNode.Core.CreateKinHelpers(constraints);
            count = (kinHelpers == null) ? 0 : kinHelpers.Count;
            Diagnostics.Log("Resolving " + count + " RK Kin types for " + graphNodeKeyStr);
            for (i = 0; i < kinHelpers.Count; i++)
            {
                kinHelper = kinHelpers[i];
                Diagnostics.Log("Slurping " + kinHelper.KinName + " Kin of " + graphNodeKeyStr);
                if (kinHelper is IResourceKinFinder)
                    kin = (kinHelper as IResourceKinFinder).FindKindredResources(graphNode.OriginalKey);
                else
                    kin = SlurpKindredResources(graphNode.OriginalKey, kinHelper);
                for (j = 0; j < kin.Count; j++)
                {
                    kinKey = kin[j].ResourceIndexEntry;
                    if (kinKey == null)
                    {
                        continue;
                    }
                    kinConnection = new GraphConnectionKin(this, graphNode, kinHelper);
                    childNode = null;
                    // Try to find an already existing kindred node in the lookup table.
                    foundIndex = -1;
                    for (k = 0; k < nodeLookupTable.Count && foundIndex < 0; k++)
                    {
                        if (nodeLookupTable[k].OriginalKey.Equals(kinKey))
                            foundIndex = k;
                    }
                    if (foundIndex >= 0)
                    {
                        childNode = nodeLookupTable[foundIndex];
                        if (!childNode.IsKindred)
                        {
                            Diagnostics.Log("Non-Kindred Node "
                                + PrintRKTag(kinKey, childNode.ExtensionTag)
                                + " is " + kinHelper.KinName + " Kin of "
                                + graphNodeKeyStr);
                        }
                        else
                        {
                            Diagnostics.Log("Existing Kindred Node "
                                + PrintRKTag(kinKey, childNode.ExtensionTag)
                                + " is " + kinHelper.KinName + " Kin of "
                                + graphNodeKeyStr);
                        }
                    }
                    else
                    {
                        IResource kinResource = kin[j].Resource;
                        if (kinResource == null)
                        {
                            Diagnostics.Log("Unresolved RK Kin:" + PrintRK(kinKey)
                                + ((kin[j].Exception == null) ? "" :
                                                                       string.Concat(" Error:\r\n", kin[j].Exception.ToString())));
                        }
                        else
                        {
                            IResourceNode kinCore = kinHelper.CreateKin(kinResource, kinKey, constraints);
                            if (kinCore == null)
                            {
                                Diagnostics.Log("CreateKin Failed:" + PrintRK(kinKey));
                            }
                            else
                            {
                                childNode = new GraphNode(this, kinKey, kinCore,
                                                          kin[j].PathPackage, ResourceDataActions.FindWrite);
                                nodeLookupTable.Add(childNode);
                                Diagnostics.Log("New Kin Node Added:" + PrintRKTag(kinKey, childNode.ExtensionTag));
                                CreateChildren(childNode, constraints);
                            }
                        }
                    }
                    if (childNode != null)
                    {
                        graphNode.KindredConnections.Add(kinConnection);
                        kinConnection.Child = childNode;
                        childNode.KinOwnerConnections.Add(kinConnection);
                    }
                }
            }
        }

        public bool AddResource(IResourceKey originalKey, object constraints)
        {
            string tag;
            int i, foundIndex = -1;
            for (i = 0; i < nodeLookupTable.Count && foundIndex < 0; i++)
            {
                if (nodeLookupTable[i].OriginalKey.Equals(originalKey))
                    foundIndex = i;
            }
            if (foundIndex >= 0)
            {
                tag = ExtList.Ext[originalKey.ResourceType][0];
                Diagnostics.Log("RK Already in Graph:" + PrintRKRef(originalKey, tag));
                return false;
            }
            var sr = new SpecificResource(FileTable.GameContent, originalKey);
            if (sr.ResourceIndexEntry == null || sr.Resource == null)
            {
                tag = ExtList.Ext[originalKey.ResourceType][0];
                Diagnostics.Log("Unresolved RK:" + PrintRKRef(originalKey, tag));
                return false;
            }
            else
            {
                IResourceNode node = ResourceNodeDealer.GetResource(sr.Resource, originalKey);
                var graphNode = new GraphNode(this, originalKey, node,
                                              sr.PathPackage, ResourceDataActions.FindWrite);
                Diagnostics.Log("New Node Added:" + PrintRKRef(originalKey, graphNode.ExtensionTag));
                nodeLookupTable.Add(graphNode);
                CreateChildren(graphNode, constraints);
                return true;
            }
        }

        public bool RemoveResource(IResourceKey key, bool removeOrphans)
        {
            GraphNode node = null;
            int i, count = nodeLookupTable.Count;
            for (i = 0; i < count && node == null; i++)
            {
                if (nodeLookupTable[i].OriginalKey.Equals(key))
                    node = nodeLookupTable[i];
            }
            if (node == null)
                return false;
            node.RemoveFromGraph(removeOrphans);
            return true;
        }

        internal ResourceGraph(IResourceNode rootNode, object constraints)
        {
            var graphNode = new GraphNode(this, rootNode.OriginalKey,
                                          rootNode, null, ResourceDataActions.FindWrite);
            nodeLookupTable.Add(graphNode);
            CreateChildren(graphNode, constraints);
        }

        public ResourceGraph(IResourceKey rootOriginalKey, object constraints)
        {
            var sr = new SpecificResource(FileTable.GameContent, rootOriginalKey);
            string tag = ExtList.Ext[rootOriginalKey.ResourceType][0];
            if (sr.ResourceIndexEntry == null || sr.Resource == null)
            {
                Diagnostics.Log("Unresolved RK Root:" + PrintRKRef(rootOriginalKey, tag));
            }
            else
            {
                IResourceNode node = ResourceNodeDealer.GetResource(sr.Resource, rootOriginalKey);
                var graphNode = new GraphNode(this, rootOriginalKey, node,
                                              sr.PathPackage, ResourceDataActions.FindWrite);
                Diagnostics.Log("New Root Node Added:" + PrintRKRef(rootOriginalKey, tag));
                nodeLookupTable.Add(graphNode);
                CreateChildren(graphNode, constraints);
            }
        }

        public delegate void Generator(ulong oldIID, string oldName,
                                       ref ulong newIID, ref string newName, bool isDuplicate);

        private struct Renumber
        {
            public readonly ulong OldIID;
            public ulong NewIID;
            public readonly string OldName;
            public string NewName;

            public Renumber(ulong iid, string name)
            {
                OldIID = NewIID = iid;
                OldName = NewName = name;
            }
        }

        public void RenumberResourceKeys(Generator generator)
        {
            GraphNode node;
            ulong nodeID, prevID = 0ul;
            int i, j, foundIndex, count = nodeLookupTable.Count;

            // Gather all unique IID and generate new ones
            Diagnostics.Log("Generating New IIDs/Names...");
            Renumber renum;
            var uniqueIIDs = new List<Renumber>(count);
            for (i = 0; i < count; i++)
            {
                node = nodeLookupTable[i];
                if (!node.IsKindred)
                {
                    nodeID = node.OriginalKey.Instance;
                    foundIndex = -1;
                    for (j = 0; j < uniqueIIDs.Count && foundIndex < 0; j++)
                    {
                        if (uniqueIIDs[j].OldIID == nodeID)
                            foundIndex = j;
                    }
                    if (foundIndex == -1)
                    {
                        renum = new Renumber(nodeID, node.Name);
                        generator(nodeID, renum.OldName,
                                  ref renum.NewIID, ref renum.NewName, false);
                        if (renum.NewIID == nodeID || renum.NewIID == prevID)
                            throw new InvalidOperationException(
                                "Generator isn't creating unique instance IDs");
                        prevID = renum.NewIID;
                        uniqueIIDs.Add(renum);
                    }
                }
            }
            Diagnostics.Log(string.Concat("Generated ", uniqueIIDs.Count.ToString(), " new IIDs/Names"));

            // Renumber and Rename Nodes
            Diagnostics.Log("Renumbering and Renaming Resources...");
            RK rk;
            for (i = 0; i < count; i++)
            {
                node = nodeLookupTable[i];
                if (!node.IsKindred)
                {
                    nodeID = node.OriginalKey.Instance;
                    foundIndex = -1;
                    for (j = 0; j < uniqueIIDs.Count && foundIndex < 0; j++)
                    {
                        if (uniqueIIDs[j].OldIID == nodeID)
                            foundIndex = j;
                    }
                    rk = node.Key;
                    rk.Instance = uniqueIIDs[foundIndex].NewIID;
                    Diagnostics.Log("Renumbering "
                        + PrintRKTag(node.OriginalKey, node.ExtensionTag)
                        + " to " + PrintRK(rk));
                    node.Key = rk;
                    Diagnostics.Log("Renaming "
                        + PrintRKTag(node.OriginalKey, node.ExtensionTag)
                        + ": \"" + node.Name + "\"");
                    node.Name = uniqueIIDs[foundIndex].NewName;
                    Diagnostics.Log("Renamed from \"" + uniqueIIDs[foundIndex].OldName
                        + "\" to \"" + node.Name + "\"");
                }
            }
        }

        public void ResetResourceKeys()
        {
            int i, count = nodeLookupTable.Count;
            for (i = 0; i < count; i++)
            {
                if (!nodeLookupTable[i].IsKindred)
                {
                    nodeLookupTable[i].ResetKey();
                    nodeLookupTable[i].ResetName();
                }
            }
        }

        public IPackage CreatePackage()
        {
            IPackage result = Package.NewPackage(0);
            GraphNode node;
            int i, count = nodeLookupTable.Count;

            // Commit changes to resource data
            Diagnostics.Log("Committing Resource Data Changes...");
            for (i = 0; i < count; i++)
            {
                node = nodeLookupTable[i];
                if (!node.Core.CommitChanges())
                {
                    Diagnostics.Log("CommitChanges Failed for "
                        + PrintRKTag(node.Key, node.ExtensionTag));
                }
            }

            // Write resource data and names to package
            Diagnostics.Log("Writing Resource Data To Package...");
            var nmap = new NameMap.PackagedNMap(result, true);
            for (i = 0; i < count; i++)
            {
                node = nodeLookupTable[i];
                if ((node.NodeActions & ResourceDataActions.Write) == ResourceDataActions.None)
                    continue;
                // TODO: Ensure every resource is forced to update its stream
                result.AddResource(node.Key, node.Core.Resource.Stream, false);
                if (!nmap.SetName(node.Key.Instance, node.Name, false))
                {
                    Diagnostics.Log("Failed to set NameMap Entry for "
                        + PrintRKTag(node.Key, node.ExtensionTag)
                        + ": \"" + node.Name + "\"");
                }
            }
            Diagnostics.Log("Writing NameMap To Package...");
            nmap.CommitChanges();

            Diagnostics.Log("COMPLETE! Package is now ready to be saved.");

            return result;
        }
    }
}
