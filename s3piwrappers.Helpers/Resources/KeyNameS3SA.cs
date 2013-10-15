using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3pi.WrapperDealer;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.Helpers.Resources
{
    public class KeyNameS3SA
    {
        public const uint S3SA_TID = 0x073FAA07;

        private static bool IsS3SA(IResourceIndexEntry rie)
        {
            return rie.ResourceType == S3SA_TID;
        }
        private static readonly Predicate<IResourceIndexEntry> sIsS3SA
            = new Predicate<IResourceIndexEntry>(IsS3SA);

        private class KNAPackage
        {
            public string Path;
            public Dictionary<ulong, string> Names;
            public Dictionary<ulong, string> Labels;
            public Dictionary<string, uint> EnumValues;

            public KNAPackage(string path)
            {
                this.Path = path;
                this.Names = new Dictionary<ulong, string>();
                this.Labels = new Dictionary<ulong, string>();
                this.EnumValues = new Dictionary<string, uint>();
            }
        }

        private KNAPackage[] mKNAPackages;

        private KeyNameS3SA(List<PathPackageTuple> ppts)
        {
            if (ppts == null)
            {
                this.mKNAPackages = null;
                return;
            }
            long len;
            byte[] data;
            IResource res;
            KNAPackage knap;
            Assembly asm;
            ScriptResource.ScriptResource s3sa;
            List<IResourceIndexEntry> rieList;
            List<KNAPackage> knaps = new List<KNAPackage>();
            foreach (PathPackageTuple ppt in ppts)
            {
                if (ppt != null && ppt.Package != null)
                {
                    rieList = ppt.Package.FindAll(sIsS3SA);
                    if (rieList != null && rieList.Count > 0)
                    {
                        knap = new KNAPackage(ppt.Path);
                        foreach (IResourceIndexEntry rie in rieList)
                        {
                            res = null;
                            try
                            {
                                res = WrapperDealer.GetResource(
                                    0, ppt.Package, rie);
                            }
                            catch { }
                            if (res != null)
                            {
                                s3sa = res as ScriptResource.ScriptResource;
                                if (s3sa != null)
                                {
                                    len = s3sa.Assembly.BaseStream.Length;
                                    data = new byte[len];
                                    s3sa.Assembly.BaseStream.Read(
                                        data, 0, (int)len);
                                    asm = Assembly.ReflectionOnlyLoad(data);
                                    if (asm != null)
                                    {
                                        ScanNames(asm, knap);
                                    }
                                }
                            }
                        }
                        if (knap.Names.Count > 0)
                        {
                            knaps.Add(knap);
                        }
                    }
                }
            }
            if (knaps.Count == 0)
            {
                this.mKNAPackages = null;
            }
            else
            {
                this.mKNAPackages = knaps.ToArray();
            }
        }

        private static void ScanNames(Assembly asm, KNAPackage knap)
        {
            int v;
            uint uv;
            object value;
            string fName, vs;
            bool interesting;
            Type[] types = null;
            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                types = rtle.Types;
            }
            catch (Exception)
            {
            }
            if (types != null)
            {
                foreach (Type t in types)
                {
                    if (t != null && 
                        !t.IsGenericType && !t.Name.StartsWith("<"))
                    {
                        if (t.IsEnum)
                        {
                            interesting = false;
                            switch (t.Namespace)
                            {
                                case "Sims3.SimIFace.SACS":
                                case "Sims3.SimIFace":
                                    interesting = true;
                                    break;
                            }
                            foreach (FieldInfo finfo in t.GetFields())
                            {
                                if (finfo.IsLiteral)
                                {
                                    if (interesting)
                                    {
                                        value = finfo.GetRawConstantValue();
                                        fName = string.Concat(
                                            t.FullName, ".", finfo.Name);
                                        if (value is int)
                                        {
                                            knap.EnumValues[fName] 
                                                = (uint)((int)value);
                                        }
                                        else if (value is uint)
                                        {
                                            knap.EnumValues[fName]
                                                = (uint)value;
                                        }
                                        else
                                        {
                                            vs = value.ToString();
                                            if (uint.TryParse(vs, out uv))
                                            {
                                                knap.EnumValues[fName] = uv;
                                            }
                                            else if (int.TryParse(vs, out v))
                                            {
                                                uv = (uint)v;
                                                knap.EnumValues[fName] = uv;
                                            }
                                        }
                                    }
                                    AddValue(finfo.Name, t.FullName, knap);
                                }
                            }
                        }
                        AddValue(t.Name, t.FullName, knap);
                    }
                }
            }
        }

        private static void AddValue(string val, string lbl, KNAPackage knap)
        {
            ulong hash = FNVHash.HashString32(val);
            if (knap.Names.ContainsKey(hash))
            {
                knap.Labels[hash] = "";
            }
            else
            {
                knap.Names[hash] = val;
                if (lbl != null)
                {
                    knap.Labels[hash] = lbl;
                }
            }
            hash = FNVHash.HashString64(val);
            if (knap.Names.ContainsKey(hash))
            {
                knap.Labels[hash] = "";
            }
            else
            {
                knap.Names[hash] = val;
                if (lbl != null)
                {
                    knap.Labels[hash] = lbl;
                }
            }
        }

        public bool TryFindName(ulong key, out string name)
        {
            if (key == 0)
            {
                name = null;
                return true;
            }
            if (this.mKNAPackages == null)
            {
                name = null;
                return false;
            }
            KNAPackage knmp;
            for (int i = 0; i < this.mKNAPackages.Length; i++)
            {
                knmp = this.mKNAPackages[i];
                if (knmp.Names.TryGetValue(key, out name))
                {
                    return true;
                }
            }
            name = null;
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
            if (this.mKNAPackages == null)
            {
                name = null;
                packagePath = null;
                return false;
            }
            KNAPackage knmp;
            for (int i = 0; i < this.mKNAPackages.Length; i++)
            {
                knmp = this.mKNAPackages[i];
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

        private static KeyNameS3SA sCurrent = null;

        public static KeyNameS3SA Current
        {
            get
            {
                if (sCurrent == null && FileTable.Current != null)
                {
                    List<PathPackageTuple> ppts
                        = new List<PathPackageTuple>(1);
                    ppts.Add(FileTable.Current);
                    sCurrent = new KeyNameS3SA(ppts);
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

        private static KeyNameS3SA sCustomContent = null;

        public static KeyNameS3SA CustomContent
        {
            get
            {
                if (sCustomContent == null &&
                    FileTable.CustomContentEnabled &&
                    FileTable.CustomContent != null)
                {
                    sCustomContent = new KeyNameS3SA(FileTable.CustomContent);
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

        private static KeyNameS3SA sGameCore = null;

        public static KeyNameS3SA GameCore
        {
            get
            {
                if (sGameCore == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sGameCore = new KeyNameS3SA(FileTableExt.GameCore);
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

        private static KeyNameS3SA sGameContent = null;

        public static KeyNameS3SA GameContent
        {
            get
            {
                if (sGameContent == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sGameContent = new KeyNameS3SA(FileTable.GameContent);
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

        private static KeyNameS3SA sDDSImages = null;

        public static KeyNameS3SA DDSImages
        {
            get
            {
                if (sDDSImages == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sDDSImages = new KeyNameS3SA(FileTable.DDSImages);
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

        private static KeyNameS3SA sThumbnails = null;

        public static KeyNameS3SA Thumbnails
        {
            get
            {
                if (sThumbnails == null && FileTable.FileTableEnabled)
                {
                    bool ccEnabled = FileTable.CustomContentEnabled;
                    PathPackageTuple ppt = FileTable.Current;
                    FileTable.CustomContentEnabled = false;
                    FileTable.Current = null;
                    sThumbnails = new KeyNameS3SA(FileTable.Thumbnails);
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
