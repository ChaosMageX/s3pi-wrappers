using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using s3pi.Filetable;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.Helpers.Resources
{
    public class ResourceMgr
    {
        public class ResEntry
        {
            public readonly uint GID;
            public readonly ulong IID;
            private string mName;
            private bool bNameIsLabel;

            public ResEntry(uint gid, ulong iid)
            {
                this.GID = gid;
                this.IID = iid;
                if (KeyNameReg.TryFindName(iid, out this.mName))
                {
                    this.bNameIsLabel = false;
                }
                else if (KeyNameReg.TryFindLabel(iid, out this.mName))
                {
                    this.bNameIsLabel = true;
                }
                else
                {
                    this.mName = "0x" + iid.ToString("X16");
                    this.bNameIsLabel = false;
                }
            }

            public string Name
            {
                get { return this.mName; }
            }

            public bool NameIsLabel
            {
                get { return this.bNameIsLabel; }
            }

            public void RefreshName(bool unhash = false)
            {
                if (!this.bNameIsLabel && 
                    this.IID != FNVHash.HashString64(this.mName))
                {
                    if (KeyNameReg.TryFindName(
                        this.IID, out this.mName))
                    {
                        this.bNameIsLabel = false;
                    }
                    else if (KeyNameReg.TryFindLabel(
                        this.IID, out this.mName))
                    {
                        this.bNameIsLabel = true;
                    }
                    else if (unhash)
                    {
                        this.mName = KeyNameReg.UnhashName(this.IID);
                        this.bNameIsLabel = false;
                    }
                    else
                    {
                        this.mName = "0x" + this.IID.ToString("X16");
                        this.bNameIsLabel = false;
                    }
                }
            }
        }

        public class ResPackage
        {
            public readonly string Path;
            public readonly ResEntry[] Entries;

            public ResPackage(string path, int capacity)
            {
                this.Path = path;
                this.Entries = new ResEntry[capacity];
            }

            public void RefreshNames(bool unhash = false)
            {
                for (int i = this.Entries.Length - 1; i >= 0; i--)
                {
                    this.Entries[i].RefreshName(unhash);
                }
            }

            public override string ToString()
            {
                return this.Entries.Length.ToString() + " | " + this.Path;
            }
        }

        public static event Action<string> StatusMessage;

        private static Dictionary<uint, ResourceMgr> sResourceManagers
            = new Dictionary<uint, ResourceMgr>();

        public static ResourceMgr GetResourceManager(uint resourceType)
        {
            ResourceMgr manager;
            if (!sResourceManagers.TryGetValue(resourceType, out manager))
            {
                manager = new ResourceMgr(resourceType);
                sResourceManagers[resourceType] = manager;
            }
            return manager;
        }

        private uint mTID;
        private List<string> mExt;

        private bool isValid(IResourceIndexEntry rie)
        {
            return rie.ResourceType == this.mTID;
        }
        private Predicate<IResourceIndexEntry> mIsValid;

        private ResourceMgr(uint tid)
        {
            this.mTID = tid;
            string tidStr = string.Concat("0x", tid.ToString("X8"));
            if (s3pi.Extensions.ExtList.Ext.ContainsKey(tidStr))
            {
                this.mExt = s3pi.Extensions.ExtList.Ext[tidStr];
            }
            else if (s3pi.Extensions.ExtList.Ext.ContainsKey("*"))
            {
                this.mExt = s3pi.Extensions.ExtList.Ext["*"];
            }
            else
            {
                this.mExt = new List<string>(2);
                this.mExt.Add("DATA");
                this.mExt.Add(".dat");
            }
            this.mIsValid = new Predicate<IResourceIndexEntry>(isValid);
        }

        public uint ResourceType
        {
            get { return this.mTID; }
        }

        private ResPackage[] LoadPackages(
            List<PathPackageTuple> ppts, string name)
        {
            if (ppts == null)
            {
                return null;
            }
            int i;
            ResPackage rp;
            List<IResourceIndexEntry> rieList;
            List<ResPackage> results = new List<ResPackage>();
            string status = null;
            if (StatusMessage != null)
            {
                status = "Searching for " + this.mExt[0] +
                    "(0x" + this.mTID.ToString("X8") + ") in " +
                    name + " Package:\n";
            }
            foreach (PathPackageTuple ppt in ppts)
            {
                if (ppt != null && ppt.Package != null)
                {
                    if (StatusMessage != null)
                    {
                        StatusMessage(status + ppt.Path);
                    }
                    rieList = ppt.Package.FindAll(this.mIsValid);
                    if (rieList != null && rieList.Count > 0)
                    {
                        i = 0;
                        rp = new ResPackage(ppt.Path, rieList.Count);
                        foreach (IResourceIndexEntry rie in rieList)
                        {
                            rp.Entries[i++] = new ResEntry(
                                rie.ResourceGroup, rie.Instance);
                        }
                        results.Add(rp);
                    }
                }
            }
            return results.ToArray();
        }

        private ResPackage[] RefreshPackages(
            List<PathPackageTuple> ppts, ResPackage[] rps, string name)
        {
            if (ppts == null)
            {
                return null;
            }
            if (rps == null)
            {
                return this.LoadPackages(ppts, name);
            }
            int i, count = rps.Length;
            bool flag;
            ResPackage rp;
            List<IResourceIndexEntry> rieList;
            List<ResPackage> results = new List<ResPackage>();
            foreach (PathPackageTuple ppt in ppts)
            {
                if (ppt != null && ppt.Package != null)
                {
                    flag = true;
                    for (i = count - 1; i >= 0 && flag; i--)
                    {
                        rp = rps[i];
                        if (string.Equals(rp.Path, ppt.Path))
                        {
                            flag = false;
                            results.Add(rp);
                            count--;
                            if (i < count)
                                Array.Copy(rps, i + 1, rps, i, count - i);
                            rps[count] = null;
                        }
                    }
                    if (flag)
                    {
                        rieList = ppt.Package.FindAll(this.mIsValid);
                        if (rieList != null && rieList.Count > 0)
                        {
                            i = 0;
                            rp = new ResPackage(ppt.Path, rieList.Count);
                            foreach (IResourceIndexEntry rie in rieList)
                            {
                                rp.Entries[i++] = new ResEntry(
                                    rie.ResourceGroup, rie.Instance);
                            }
                            results.Add(rp);
                        }
                    }
                }
            }
            return results.ToArray();
        }

        private ResPackage[] mCurrent = null;

        public ResPackage[] Current
        {
            get
            {
                this.LoadCurrent();
                return this.mCurrent;
            }
        }

        public void LoadCurrent()
        {
            if (mCurrent == null && FileTableExt.Current.Count > 0)
            {
                this.mCurrent = this.LoadPackages(
                    FileTableExt.Current, "Current");
            }
        }

        public void RefreshCurrent()
        {
            this.mCurrent = this.RefreshPackages(
                FileTableExt.Current, this.mCurrent, "Current");
        }

        public void ResetCurrent()
        {
            if (this.mCurrent != null)
            {
                this.mCurrent = null;
            }
        }

        private ResPackage[] mCustomContent = null;

        public ResPackage[] CustomContent
        {
            get
            {
                this.LoadCustomContent();
                return this.mCustomContent;
            }
        }

        public void LoadCustomContent()
        {
            if (this.mCustomContent == null &&
                    FileTable.CustomContentEnabled &&
                    FileTable.CustomContent != null)
            {
                this.mCustomContent = this.LoadPackages(
                    FileTable.CustomContent, "Custom Content");
            }
        }

        public void RefreshCustomContent()
        {
            this.mCustomContent = this.RefreshPackages(
                FileTable.CustomContent, this.mCustomContent, 
                "Custom Content");
        }

        public void ResetCustomContent()
        {
            if (this.mCustomContent != null)
            {
                this.mCustomContent = null;
            }
        }

        private ResPackage[] mGameCore = null;

        public ResPackage[] GameCore
        {
            get
            {
                this.LoadGameCore();
                return this.mGameCore;
            }
        }

        public void LoadGameCore()
        {
            if (this.mGameCore == null && FileTable.FileTableEnabled)
            {
                bool ccEnabled = FileTable.CustomContentEnabled;
                PathPackageTuple ppt = FileTable.Current;
                FileTable.CustomContentEnabled = false;
                FileTable.Current = null;
                this.mGameCore = this.LoadPackages(
                    FileTableExt.GameCore, "Game Core");
                FileTable.CustomContentEnabled = ccEnabled;
                FileTable.Current = ppt;
            }
        }

        public void ResetGameCore()
        {
            if (this.mGameCore != null)
            {
                this.mGameCore = null;
            }
        }

        private ResPackage[] mGameContent = null;

        public ResPackage[] GameContent
        {
            get
            {
                this.LoadGameContent();
                return this.mGameContent;
            }
        }

        public void LoadGameContent()
        {
            if (this.mGameContent == null && FileTable.FileTableEnabled)
            {
                bool ccEnabled = FileTable.CustomContentEnabled;
                PathPackageTuple ppt = FileTable.Current;
                FileTable.CustomContentEnabled = false;
                FileTable.Current = null;
                this.mGameContent = this.LoadPackages(
                    FileTable.GameContent, "Game Content");
                FileTable.CustomContentEnabled = ccEnabled;
                FileTable.Current = ppt;
            }
        }

        public void ResetGameContent()
        {
            if (this.mGameContent != null)
            {
                this.mGameContent = null;
            }
        }

        private ResPackage[] mDDSImages = null;

        public ResPackage[] DDSImages
        {
            get
            {
                this.LoadDDSImages();
                return this.mDDSImages;
            }
        }

        public void LoadDDSImages()
        {
            if (this.mDDSImages == null && FileTable.FileTableEnabled)
            {
                bool ccEnabled = FileTable.CustomContentEnabled;
                PathPackageTuple ppt = FileTable.Current;
                FileTable.CustomContentEnabled = false;
                FileTable.Current = null;
                this.mDDSImages = this.LoadPackages(
                    FileTable.DDSImages, "DDS Images");
                FileTable.CustomContentEnabled = ccEnabled;
                FileTable.Current = ppt;
            }
        }

        public void ResetDDSImages()
        {
            if (this.mDDSImages != null)
            {
                this.mDDSImages = null;
            }
        }

        private ResPackage[] mThumbnails = null;

        public ResPackage[] Thumbnails
        {
            get
            {
                this.LoadThumbnails();
                return this.mThumbnails;
            }
        }

        public void LoadThumbnails()
        {
            if (this.mThumbnails == null && FileTable.FileTableEnabled)
            {
                bool ccEnabled = FileTable.CustomContentEnabled;
                PathPackageTuple ppt = FileTable.Current;
                FileTable.CustomContentEnabled = false;
                FileTable.Current = null;
                this.mThumbnails = this.LoadPackages(
                    FileTable.Thumbnails, "Thumbnails");
                FileTable.CustomContentEnabled = ccEnabled;
                FileTable.Current = ppt;
            }
        }

        public void ResetThumbnails()
        {
            if (this.mThumbnails != null)
            {
                this.mThumbnails = null;
            }
        }

        public void LoadAll()
        {
            LoadCurrent();
            LoadCustomContent();
            LoadGameCore();
            LoadGameContent();
            LoadDDSImages();
            LoadThumbnails();
        }

        public void ResetAll()
        {
            ResetCurrent();
            ResetCustomContent();
            ResetGameCore();
            ResetGameContent();
            ResetDDSImages();
            ResetThumbnails();
        }

        public bool ContainsResource(ulong iid, uint gid)
        {
            int i, j;
            ResEntry entry;
            ResPackage package;
            if (this.mCurrent != null)
            {
                for (i = this.mCurrent.Length - 1; i >= 0; i--)
                {
                    package = this.mCurrent[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            if (this.mCustomContent != null)
            {
                for (i = this.mCustomContent.Length - 1; i >= 0; i--)
                {
                    package = this.mCustomContent[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            if (this.mGameCore != null)
            {
                for (i = this.mGameCore.Length - 1; i >= 0; i--)
                {
                    package = this.mGameCore[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            if (this.mGameContent != null)
            {
                for (i = this.mGameContent.Length - 1; i >= 0; i--)
                {
                    package = this.mGameContent[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            if (this.mDDSImages != null)
            {
                for (i = this.mDDSImages.Length - 1; i >= 0; i--)
                {
                    package = this.mDDSImages[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            if (this.mThumbnails != null)
            {
                for (i = this.mThumbnails.Length - 1; i >= 0; i--)
                {
                    package = this.mThumbnails[i];
                    for (j = package.Entries.Length - 1; j >= 0; j--)
                    {
                        entry = package.Entries[j];
                        if (entry.IID == iid && entry.GID == gid)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
