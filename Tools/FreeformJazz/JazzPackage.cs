using System;
using System.Collections.Generic;
using s3pi.Interfaces;
using s3pi.WrapperDealer;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.FreeformJazz
{
    public class JazzPackage : IEquatable<JazzPackage>
    {
        private static bool isNameMap(IResourceIndexEntry rie)
        {
            return rie.ResourceType == KeyNameMap.NameMapTID;
        }
        private static readonly Predicate<IResourceIndexEntry> sIsNameMap
            = new Predicate<IResourceIndexEntry>(isNameMap);

        public bool ReadOnly;
        public string Path;
        public IPackage Package;
        public string Title;
        public List<JazzGraphContainer> Graphs;

        public IResourceIndexEntry KNMapRIE;
        public NameMapResource.NameMapResource KNMapRes;

        public JazzPackage(string path, IPackage package, bool readOnly)
        {
            this.ReadOnly = readOnly;
            this.Path = path;
            this.Package = package;
            this.Title = MainForm.CreateTitle(path, readOnly);
            this.Graphs = new List<JazzGraphContainer>();

            this.KNMapRIE = this.Package.Find(sIsNameMap);
            if (this.KNMapRIE == null)
            {
                this.KNMapRes
                    = new NameMapResource.NameMapResource(0, null);
                this.KNMapRIE = this.Package.AddResource(
                    new RK(KeyNameMap.NameMapTID, 0, 0),
                    this.KNMapRes.Stream, true);
            }
            else
            {
                IResource res = null;
                try
                {
                    res = WrapperDealer.GetResource(
                        0, this.Package, this.KNMapRIE);
                }
                catch { }
                if (res != null)
                {
                    this.KNMapRes
                        = res as NameMapResource.NameMapResource;
                }
                if (this.KNMapRes == null)
                {
                    this.KNMapRes
                        = new NameMapResource.NameMapResource(0, null);
                    this.KNMapRIE = this.Package.AddResource(
                        new RK(KeyNameMap.NameMapTID, 0, 0),
                        this.KNMapRes.Stream, true);
                }
            }
        }

        public bool RemoveJazzGraph(JazzGraphContainer jgc)
        {
            if (jgc == null)
            {
                return false;
            }
            int index = jgc.Index;
            for (int i = this.Graphs.Count - 1; i >= 0; i--)
            {
                jgc = this.Graphs[i];
                if (jgc.Index == index)
                {
                    this.Graphs.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool Equals(JazzPackage other)
        {
            return other != null &&
                string.Equals(this.Path, other.Path) &&
                this.ReadOnly == other.ReadOnly;
        }
    }
}
