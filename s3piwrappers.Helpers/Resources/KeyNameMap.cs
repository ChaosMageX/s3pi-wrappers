using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Extensions;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3pi.WrapperDealer;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.Helpers.Resources
{
    public class KeyNameMap
    {
        public const uint NameMapTID = 0x0166038C;

        public static readonly string NameMapExt;

        public static readonly string NameMapS3End;

        static KeyNameMap()
        {
            List<string> ext;
            string str = string.Concat("0x", NameMapTID.ToString("X8"));
            if (ExtList.Ext.ContainsKey(str))
            {
                ext = ExtList.Ext[str];
                NameMapExt = ext[1];
                NameMapS3End = string.Join("", ext.ToArray());
            }
            else if (ExtList.Ext.ContainsKey("*"))
            {
                ext = ExtList.Ext["*"];
                NameMapExt = ext[1];
                NameMapS3End = string.Join("", ext.ToArray());
            }
            else
            {
                NameMapExt = ".dat";
                NameMapS3End = ".dat";
            }
        }

        private class KNMPackage
        {
            public string Path;
            public Dictionary<ulong, string> Names;
            public Dictionary<ulong, string> Labels;
            public Dictionary<ulong, string> Generics;

            public KNMPackage(string path)
            {
                this.Path = path;
                this.Names = new Dictionary<ulong, string>();
                this.Labels = new Dictionary<ulong, string>();
                this.Generics = new Dictionary<ulong, string>();
            }
        }

        private class CLIP
        {
            public string Name;
            public string PackagePath;

            public CLIP(string value, string packagePath)
            {
                this.Name = value;
                this.PackagePath = packagePath;
            }
        }

        private class GenCLIP
        {
            public string GenericName;
            public Dictionary<ulong, CLIP> CLIPNames;

            public GenCLIP(string genericName)
            {
                this.GenericName = genericName;
                this.CLIPNames = new Dictionary<ulong, CLIP>();
            }
        }

        public static event Action<string> StatusMessage;

        public static bool IsNameMap(IResourceIndexEntry rie)
        {
            return rie.ResourceType == NameMapTID;
        }
        public static readonly Predicate<IResourceIndexEntry> IsNameMapPred
            = new Predicate<IResourceIndexEntry>(IsNameMap);

        private KNMPackage[] mKNMPackages;

        private Dictionary<ulong, GenCLIP> mGenCLIPs;

        private KeyNameMap(List<PathPackageTuple> ppts, string knmName)
        {
            if (ppts == null)
            {
                this.mKNMPackages = null;
                this.mGenCLIPs = null;
                return;
            }
            GenCLIP gc;
            ulong h, ch;
            string s, cs;
            KNMPackage knmp;
            IResource resource;
            IDictionary<ulong, string> map;
            List<IResourceIndexEntry> rieList;
            List<KeyValuePair<ulong, string>> names;
            List<KNMPackage> knmps = new List<KNMPackage>();
            foreach (PathPackageTuple ppt in ppts)
            {
                if (ppt != null && ppt.Package != null)
                {
                    if (StatusMessage != null)
                    {
                        StatusMessage("Searching " + knmName + 
                            " Package:\n" + ppt.Path);
                    }
                    rieList = ppt.Package.FindAll(IsNameMapPred);
                    if (rieList != null && rieList.Count > 0)
                    {
                        if (this.mGenCLIPs == null)
                        {
                            this.mGenCLIPs 
                                = new Dictionary<ulong, GenCLIP>();
                        }
                        knmp = new KNMPackage(ppt.Path);
                        foreach (IResourceIndexEntry rie in rieList)
                        {
                            resource = null;
                            try
                            {
                                resource = WrapperDealer.GetResource(
                                    0, ppt.Package, rie);
                            }
                            catch { }
                            if (resource != null)
                            {
                                map = resource as IDictionary<ulong, string>;
                                if (map != null && map.Count > 0)
                                {
                                    foreach (KeyValuePair<ulong, string> nm
                                        in map)
                                    {
                                        h = nm.Key;
                                        s = nm.Value;
                                        if (FNVHash.HashString64(s) == h ||
                                            FNVHash.HashString32(s) == h)
                                        {
                                            knmp.Names[h] = s;
                                        }
                                        else if (FNVCLIP.HashString(s) == h)
                                        {
                                            cs = FNVCLIP.GetGenericValue(s);
                                            ch = FNVHash.HashString64(cs);
                                            ch &= 0x7FFFFFFFFFFFFFFF;
                                            knmp.Generics[ch] = cs;
                                            if (!this.mGenCLIPs.TryGetValue(
                                                ch, out gc))
                                            {
                                                gc = new GenCLIP(cs);
                                                this.mGenCLIPs.Add(ch, gc);
                                            }
                                            gc.CLIPNames[h] 
                                                = new CLIP(s, knmp.Path);
                                        }
                                        else
                                        {
                                            knmp.Labels[h] = s;
                                        }
                                    }
                                }
                            }
                        }
                        if (knmp.Names.Count > 0)
                        {
                            names = new List<KeyValuePair<ulong, 
                                string>>(knmp.Names);
                            foreach (KeyValuePair<ulong, string> name
                                in names)
                            {
                                s = name.Value;
                                if (name.Key < 0x100000000)
                                {
                                    h = FNVHash.HashString64(s);
                                    if (!knmp.Names.ContainsKey(h))
                                        knmp.Names[h] = s;
                                }
                                else
                                {
                                    h = FNVHash.HashString32(s);
                                    if (!knmp.Names.ContainsKey(h))
                                        knmp.Names[h] = s;
                                }
                                cs = FNVCLIP.GetGenericValue(s);
                                if (!string.Equals(s, cs,
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    h = FNVHash.HashString32(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                    h = FNVHash.HashString64(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                }
                            }
                        }
                        if (knmp.Generics.Count > 0)
                        {
                            foreach (KeyValuePair<ulong, string> gen
                                in knmp.Generics)
                            {
                                s = gen.Value;
                                h = FNVHash.HashString32(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                h = FNVHash.HashString64(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                cs = FNVCLIP.GetGenericValue(s);
                                h = FNVHash.HashString32(cs);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = cs;
                                }
                                h = FNVHash.HashString64(cs);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = cs;
                                }
                                cs = string.Concat(cs, ".ma");
                                h = FNVHash.HashString32(cs);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = cs;
                                }
                                h = FNVHash.HashString64(cs);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = cs;
                                }
                                s = string.Concat(s, ".ma");
                                h = FNVHash.HashString32(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                h = FNVHash.HashString64(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                            }
                        }
                        if (knmp.Labels.Count > 0)
                        {
                            foreach (KeyValuePair<ulong, string> label 
                                in knmp.Labels)
                            {
                                s = label.Value;
                                h = FNVHash.HashString32(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                h = FNVHash.HashString64(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                cs = FNVCLIP.GetGenericValue(s);
                                if (!string.Equals(s, cs, 
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    h = FNVHash.HashString32(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                    h = FNVHash.HashString64(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                    cs = string.Concat(cs, ".ma");
                                    h = FNVHash.HashString32(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                    h = FNVHash.HashString64(cs);
                                    if (!knmp.Names.ContainsKey(h))
                                    {
                                        knmp.Names[h] = cs;
                                    }
                                }
                                s = string.Concat(s, ".ma");
                                h = FNVHash.HashString32(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                                h = FNVHash.HashString64(s);
                                if (!knmp.Names.ContainsKey(h))
                                {
                                    knmp.Names[h] = s;
                                }
                            }
                        }
                        if (knmp.Names.Count > 0)
                        {
                            knmps.Add(knmp);
                        }
                    }
                }
            }
            if (knmps.Count == 0)
            {
                this.mKNMPackages = null;
            }
            else
            {
                this.mKNMPackages = knmps.ToArray();
            }
        }

        public bool TryFindName(ulong key, out string name)
        {
            if (key == 0)
            {
                name = null;
                return true;
            }
            if (this.mKNMPackages == null)
            {
                name = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Names.TryGetValue(key, out name))
                {
                    return true;
                }
            }
            name = null;
            return false;
        }

        public bool TryFindLabel(ulong key, out string label)
        {
            if (this.mKNMPackages == null)
            {
                label = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Labels.TryGetValue(key, out label))
                {
                    return true;
                }
            }
            label = null;
            return false;
        }

        public bool TryFindNameOrLabel(ulong key, out string str)
        {
            if (this.mKNMPackages == null)
            {
                str = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Names.TryGetValue(key, out str))
                {
                    return true;
                }
                if (knmp.Labels.TryGetValue(key, out str))
                {
                    return true;
                }
            }
            str = null;
            return false;
        }

        public bool TryFindGenCLIP(ulong iid, out string clipName)
        {
            if (iid == 0)
            {
                clipName = null;
                return true;
            }
            if (this.mKNMPackages == null)
            {
                clipName = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Generics.TryGetValue(iid, out clipName))
                {
                    return true;
                }
            }
            clipName = null;
            return false;
        }

        public bool TryFindName(ulong key, 
            out string name, out string packagePath)
        {
            if (key == 0)
            {
                name = null;
                packagePath = null;
                return true;
            }
            if (this.mKNMPackages == null)
            {
                name = null;
                packagePath = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Names.TryGetValue(key, out name))
                {
                    packagePath = knmp.Path;
                    return true;
                }
            }
            name = null;
            packagePath = null;
            return false;
        }

        public bool TryFindLabel(ulong key,
            out string label, out string packagePath)
        {
            if (this.mKNMPackages == null)
            {
                label = null;
                packagePath = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Labels.TryGetValue(key, out label))
                {
                    packagePath = knmp.Path;
                    return true;
                }
            }
            label = null;
            packagePath = null;
            return false;
        }

        public bool TryFindNameOrLabel(ulong key,
            out string str, out string packagePath)
        {
            if (this.mKNMPackages == null)
            {
                str = null;
                packagePath = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Names.TryGetValue(key, out str))
                {
                    packagePath = knmp.Path;
                    return true;
                }
                if (knmp.Labels.TryGetValue(key, out str))
                {
                    packagePath = knmp.Path;
                    return true;
                }
            }
            str = null;
            packagePath = null;
            return false;
        }

        public bool TryFindGenCLIP(ulong iid,
            out string clipName, out string packagePath)
        {
            if (iid == 0)
            {
                clipName = null;
                packagePath = null;
                return true;
            }
            if (this.mKNMPackages == null)
            {
                clipName = null;
                packagePath = null;
                return false;
            }
            KNMPackage knmp;
            for (int i = 0; i < this.mKNMPackages.Length; i++)
            {
                knmp = this.mKNMPackages[i];
                if (knmp.Generics.TryGetValue(iid, out clipName))
                {
                    packagePath = knmp.Path;
                    return true;
                }
            }
            clipName = null;
            packagePath = null;
            return false;
        }

        private static KeyNameMap sCurrent = null;

        public static KeyNameMap Current
        {
            get
            {
                if (sCurrent == null && FileTableExt.Current.Count > 0)
                {
                    sCurrent = new KeyNameMap(
                        FileTableExt.Current, "Current");
                }
                return sCurrent;
            }
        }

        public static void ResetCurrent()
        {
            if (sCurrent != null)
            {
                sCurrent = null;
            }
        }

        private static KeyNameMap sCustomContent = null;

        public static KeyNameMap CustomContent
        {
            get
            {
                if (sCustomContent == null && 
                    FileTable.CustomContentEnabled &&
                    FileTable.CustomContent != null)
                {
                    sCustomContent = new KeyNameMap(
                        FileTable.CustomContent, "Custom Content");
                }
                return sCustomContent;
            }
        }

        public static void ResetCustomContent()
        {
            if (sCustomContent != null)
            {
                sCustomContent = null;
            }
        }

        private static KeyNameMap sGameCore = null;

        public static KeyNameMap GameCore
        {
            get
            {
                if (sGameCore == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sGameCore = new KeyNameMap(
                        FileTableExt.GameCore, "Game Core");
                    FileTable.CustomContentEnabled = ccEnabled;
                    FileTable.Current = ppt;
                }
                return sGameCore;
            }
        }

        public static void ResetGameCore()
        {
            if (sGameCore != null)
            {
                sGameCore = null;
            }
        }

        private static KeyNameMap sGameContent = null;

        public static KeyNameMap GameContent
        {
            get
            {
                if (sGameContent == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sGameContent = new KeyNameMap(
                        FileTable.GameContent, "Game Content");
                    FileTable.CustomContentEnabled = ccEnabled;
                    FileTable.Current = ppt;
                }
                return sGameContent;
            }
        }

        public static void ResetGameContent()
        {
            if (sGameContent != null)
            {
                sGameContent = null;
            }
        }

        private static KeyNameMap sDDSImages = null;

        public static KeyNameMap DDSImages
        {
            get
            {
                if (sDDSImages == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sDDSImages = new KeyNameMap(
                        FileTable.DDSImages, "DDS Images");
                    FileTable.CustomContentEnabled = ccEnabled;
                    FileTable.Current = ppt;
                }
                return sDDSImages;
            }
        }

        public static void ResetDDSImages()
        {
            if (sDDSImages != null)
            {
                sDDSImages = null;
            }
        }

        private static KeyNameMap sThumbnails = null;

        public static KeyNameMap Thumbnails
        {
            get
            {
                if (sThumbnails == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sThumbnails = new KeyNameMap(
                        FileTable.Thumbnails, "Thumbnails");
                    FileTable.CustomContentEnabled = ccEnabled;
                    FileTable.Current = ppt;
                }
                return sThumbnails;
            }
        }

        public static void ResetThumbnails()
        {
            if (sThumbnails != null)
            {
                sThumbnails = null;
            }
        }

        public static void ResetAll()
        {
            ResetCurrent();
            ResetCustomContent();
            ResetGameCore();
            ResetGameContent();
            ResetDDSImages();
            ResetThumbnails();
        }
    }
}
