using System;
using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

namespace s3piwrappers.SceneGraph.Managers
{
    public class NameMap
    {
        public class PackagedNMap
        {
            private const string NameMapTIDStr = "0x0166038C";

            private readonly IPackage package;

            private readonly List<KeyValuePair<IResourceIndexEntry, IResource>> nmaps
                = new List<KeyValuePair<IResourceIndexEntry, IResource>>();

            public PackagedNMap(IPackage package, bool create)
            {
                if (package == null)
                    throw new ArgumentNullException("package");
                this.package = package;
                IResource nmap;
                List<IResourceIndexEntry> rieList = package.FindAll(IsNameMap);
                if ((rieList == null || rieList.Count == 0) && create)
                {
                    var tgi = new TGIBlock(0, null, NameMapTID, 0u, 0ul);
                    nmap = WrapperDealer.CreateNewResource(0, NameMapTIDStr);
                    IResourceIndexEntry rie
                        = package.AddResource(tgi, nmap.Stream, false);
                    nmaps.Add(new KeyValuePair<IResourceIndexEntry,
                                  IResource>(rie, nmap));
                }
                else
                {
                    int i, count = rieList.Count;
                    for (i = 0; i < count; i++)
                    {
                        nmap = null;
                        try
                        {
                            nmap = WrapperDealer.GetResource(0, package, rieList[i]);
                        }
                        catch
                        {
                        }
                        if (nmap != null)
                            nmaps.Add(new KeyValuePair<IResourceIndexEntry,
                                          IResource>(rieList[i], nmap));
                    }
                }
            }

            public string GetName(ulong instance)
            {
                IDictionary<ulong, string> nmap;
                for (int i = 0; i < nmaps.Count; i++)
                {
                    nmap = nmaps[i].Value as IDictionary<ulong, string>;
                    if (nmap != null && nmap.ContainsKey(instance))
                        return nmap[instance];
                }
                return "";
            }

            public bool SetName(ulong instance, string name, bool replace)
            {
                IDictionary<ulong, string> nmap;
                int i, count = nmaps.Count;
                for (i = 0; i < count; i++)
                {
                    nmap = nmaps[i].Value as IDictionary<ulong, string>;
                    if (nmap == null) continue;
                    if (nmap.ContainsKey(instance))
                    {
                        if (replace)
                        {
                            nmap[instance] = name;
                            return true;
                        }
                        else
                            return false;
                    }
                }
                nmap = nmaps[0].Value as IDictionary<ulong, string>;
                if (nmap == null)
                    return false;
                nmap.Add(instance, name);
                return true;
            }

            public bool CommitChanges()
            {
                for (int i = 0; i < nmaps.Count; i++)
                {
                    package.ReplaceResource(nmaps[i].Key, nmaps[i].Value);
                }
                return true;
            }
        }

        private class PNMap
        {
            public readonly PathPackageTuple Package;
            public readonly IDictionary<ulong, string> NMap;

            public PNMap(PathPackageTuple package,
                         IDictionary<ulong, string> nmap)
            {
                Package = package;
                NMap = nmap;
            }
        }

        public const uint NameMapTID = 0x0166038C;

        private readonly SpecificResource latest;
        private readonly List<PNMap> namemaps;

        private static bool IsNameMap(IResourceIndexEntry rie)
        {
            return rie.ResourceType == NameMapTID;
        }

        public NameMap(List<PathPackageTuple> nameMapPPTs)
        {
            namemaps = new List<PNMap>();
            if (nameMapPPTs == null) return;
            List<IResourceIndexEntry> rieList;
            IResource resource;
            int i, j;
            for (i = 0; i < nameMapPPTs.Count; i++)
            {
                rieList = nameMapPPTs[i].Package.FindAll(IsNameMap);
                if (rieList == null) continue;
                for (j = 0; j < rieList.Count; j++)
                {
                    resource = null;
                    try
                    {
                        resource = WrapperDealer.GetResource(0, nameMapPPTs[i].Package, rieList[j]);
                    }
                    catch
                    {
                    }
                    if (resource != null)
                    {
                        var nmp = resource as IDictionary<ulong, string>;
                        if (nmp == null) continue;
                        if (latest == null)
                            latest = new SpecificResource(nameMapPPTs[i], rieList[j]);
                        namemaps.Add(new PNMap(nameMapPPTs[i], nmp));
                    }
                }
            }
        }

        public string this[ulong instance]
        {
            get
            {
                foreach (PNMap namemap in namemaps)
                    if (namemap.NMap.ContainsKey(instance))
                        return namemap.NMap[instance];
                return null;
            }
        }

        public string GetName(ulong instance, PathPackageTuple package)
        {
            int i;
            PNMap pnmap;
            if (package != null)
            {
                for (i = 0; i < namemaps.Count; i++)
                {
                    pnmap = namemaps[i];
                    if (package.Equals(pnmap.Package))
                    {
                        if (pnmap.NMap.ContainsKey(instance))
                            return pnmap.NMap[instance];
                    }
                }
            }
            for (i = 0; i < namemaps.Count; i++)
            {
                pnmap = namemaps[i];
                if (pnmap.NMap.ContainsKey(instance))
                    return pnmap.NMap[instance];
            }
            return null;
        }

        public IResourceKey ResourceKey
        {
            get { return latest == null ? RK.NULL : latest.RequestedRK; }
        }

        public IDictionary<ulong, string> Map
        {
            get { return latest == null ? null : (IDictionary<ulong, string>) latest.Resource; }
        }

        public void Commit()
        {
            latest.Commit();
        }

        #region Global

        private static NameMap gamenmap;
        private static NameMap ddsnmap;
        private static NameMap thumnmap;

        public static void Reset()
        {
            gamenmap = null;
        }

        public static NameMap GameNMap
        {
            get
            {
                if (gamenmap == null)
                    gamenmap = new NameMap(FileTable.GameContent);
                return gamenmap;
            }
        }

        public static NameMap DDSNMap
        {
            get
            {
                if (ddsnmap == null)
                    ddsnmap = new NameMap(FileTable.DDSImages);
                return ddsnmap;
            }
        }

        public static NameMap ThumNMap
        {
            get
            {
                if (thumnmap == null)
                    thumnmap = new NameMap(FileTable.Thumbnails);
                return thumnmap;
            }
        }

        public static bool IsOK
        {
            get { return GameNMap != null && GameNMap.Map != null && GameNMap.Map.Count > 0; }
        }

        public static string GetName(IResourceKey key)
        {
            return ResourceGraph.IsDDS(key.ResourceType) ? DDSNMap[key.Instance] :
                                                                                     ResourceGraph.IsThum(key.ResourceType) ? ThumNMap[key.Instance] :
                                                                                                                                                         GameNMap[key.Instance];
        }

        #endregion
    }
}
