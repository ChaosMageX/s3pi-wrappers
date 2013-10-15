using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.Helpers.Resources
{
    public static class KeyNameReg
    {
        private static KeyNameMap sCurrntKNM = null;
        private static KeyNameMap sCustomKNM = null;
        private static KeyNameMap sGCoreKNM = null;
        private static KeyNameMap sGContKNM = null;
        private static KeyNameMap sImageKNM = null;
        private static KeyNameMap sThumbKNM = null;

        //private static KeyNameS3SA sCurrntKNA = null;
        //private static KeyNameS3SA sCustomKNA = null;
        //private static KeyNameS3SA sGCoreKNA = null;
        //private static KeyNameS3SA sGContKNA = null;

        public static void RefreshKeyNameMaps()
        {
            sCurrntKNM = KeyNameMap.Current;
            sCustomKNM = KeyNameMap.CustomContent;

            //sCurrntKNA = KeyNameS3SA.Current;
            //sCustomKNA = KeyNameS3SA.CustomContent;

            bool ftEnabled = FileTable.FileTableEnabled;
            FileTable.FileTableEnabled = true;
            
            sGCoreKNM = KeyNameMap.GameCore;
            //bool ccEnabled = FileTable.CustomContentEnabled;
            //PathPackageTuple ppt = FileTable.Current;
            //FileTable.CustomContentEnabled = false;
            //FileTable.Current = null;
            sGContKNM = KeyNameMap.GameContent;
            sImageKNM = KeyNameMap.DDSImages;
            sThumbKNM = KeyNameMap.Thumbnails;

            //sGCoreKNA = KeyNameS3SA.GameCore;
            //sGContKNA = KeyNameS3SA.GameContent;

            FileTable.FileTableEnabled = ftEnabled;
            //FileTable.CustomContentEnabled = ccEnabled;
            //FileTable.Current = ppt;
        }

        /*private static Dictionary<uint, string> sClip32Registry = null;

        private static bool IsClip(IResourceIndexEntry rie)
        {
            return rie.ResourceType == 0x6b20c4f3;
        }
        private static readonly Predicate<IResourceIndexEntry> sIsClip
            = new Predicate<IResourceIndexEntry>(IsClip);

        public static bool BuildClip32Registry()
        {
            if (sClip32Registry == null)
            {
                if (sGContKNM == null)
                {
                    RefreshKeyNameMaps();
                }
                sClip32Registry = new Dictionary<uint, string>();
                BuildClip32Registry(FileTable.GameContent);
                return true;
            }
            return false;
        }

        private static void BuildClip32Registry(
            List<PathPackageTuple> ppts)
        {
            if (ppts != null)
            {
                uint hash;
                string name, n;
                List<IResourceIndexEntry> rieList;
                foreach (PathPackageTuple ppt in ppts)
                {
                    if (ppt != null && ppt.Package != null)
                    {
                        rieList = ppt.Package.FindAll(sIsClip);
                        if (rieList != null && rieList.Count > 0)
                        {
                            foreach (IResourceIndexEntry rie in rieList)
                            {
                                if (TryFindName(rie.Instance, out name))
                                {
                                    hash = FNVHash.HashString32(name);
                                    if (!TryFindName(hash, out n))
                                    {
                                        sClip32Registry[hash] = name;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ClearClip32Registry()
        {
            if (sClip32Registry != null)
            {
                sClip32Registry = null;
            }
        }/* */

        public static bool HasName(uint hash)
        {
            string name;
            return TryFindName(hash, out name);
        }

        public static bool TryFindName(uint hash, out string name)
        {
            if (hash == 0)
            {
                name = null;
                return true;
            }
            if (hash == FNVHash.TS3Offset32)
            {
                name = "";
                return true;
            }
            if (sCurrntKNM != null &&
                sCurrntKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sCurrntKNA != null &&
                sCurrntKNA.TryFindName(hash, out name))
            {
                return true;
            }/* */
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            /*if (sClip32Registry != null &&
                sClip32Registry.TryGetValue(hash, out name))
            {
                return true;
            }/* */
            if (sGCoreKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sImageKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sThumbKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNM != null &&
                sCustomKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sGCoreKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNA != null &&
                sCustomKNA.TryFindName(hash, out name))
            {
                return true;
            }/* */
            return false;
        }

        public static bool HasName(ulong hash)
        {
            string name;
            return TryFindName(hash, out name);
        }

        public static bool TryFindName(ulong hash, out string name)
        {
            if (hash == 0)
            {
                name = null;
                return true;
            }
            if (hash == FNVHash.TS3Offset64)
            {
                name = "";
                return true;
            }
            if (sCurrntKNM != null &&
                sCurrntKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sCurrntKNA != null &&
                sCurrntKNA.TryFindName(hash, out name))
            {
                return true;
            }/* */
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if (sGCoreKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sImageKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sThumbKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNM != null &&
                sCustomKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sGCoreKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNA != null &&
                sCustomKNA.TryFindName(hash, out name))
            {
                return true;
            }/* */
            return false;
        }

        public static bool HasLabel(ulong hash)
        {
            string label;
            return TryFindLabel(hash, out label);
        }

        public static bool TryFindLabel(ulong hash, out string label)
        {
            if (sCurrntKNM != null &&
                sCurrntKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if (sGCoreKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if (sGContKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if (sImageKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if (sThumbKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if (sCustomKNM != null &&
                sCustomKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            return false;
        }

        public static bool HasNameOrLabel(ulong hash)
        {
            string str;
            return TryFindNameOrLabel(hash, out str);
        }

        public static bool TryFindNameOrLabel(ulong hash, out string str)
        {
            if (sCurrntKNM != null &&
                sCurrntKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if (sGCoreKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if (sGContKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if (sImageKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if (sThumbKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if (sCustomKNM != null &&
                sCustomKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            return false;
        }

        public static bool HasGenCLIP(ulong clipIID)
        {
            string clipName;
            return TryFindGenCLIP(clipIID, out clipName);
        }

        public static bool TryFindGenCLIP(
            ulong clipIID, out string clipName)
        {
            if (clipIID == 0)
            {
                clipName = null;
                return true;
            }
            if (clipIID == (FNVHash.TS3Offset64 & 0x7FFFFFFFFFFFFFFF))
            {
                clipName = "";
                return true;
            }
            if (sCurrntKNM != null &&
                sCurrntKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if (sGCoreKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if (sGContKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if (sImageKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if (sThumbKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if (sCustomKNM != null &&
                sCustomKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            return false;
        }

        private static readonly FNVSearchTable sSearchTable
            = FNVSearchTable.AllPrintable;
        private static readonly Dictionary<ulong, string> sUnhashRegistry
            = new Dictionary<ulong, string>();

        public static string UnhashName(uint hash)
        {
            string name;
            if (!sUnhashRegistry.TryGetValue(hash, out name))
            {
                /*FNVUnhasher32 unhasher 
                    = new FNVUnhasher32(hash, sSearchTable, 30, 1);
                unhasher.Start();
                while (!unhasher.Finished)
                {
                    System.Threading.Thread.Sleep(1);
                }
                name = unhasher.Results[0];
                sUnhashRegistry.Add(hash, name);/* */
                name = string.Concat("0x", hash.ToString("X8"));
            }
            return name;
        }

        public static string UnhashName(ulong hash)
        {
            string name;
            if (!sUnhashRegistry.TryGetValue(hash, out name))
            {
                /*FNVUnhasher64 unhasher 
                    = new FNVUnhasher64(hash, sSearchTable, 30, 1);
                unhasher.Start();
                while (!unhasher.Finished)
                {
                    System.Threading.Thread.Sleep(1);
                }
                name = unhasher.Results[0];
                sUnhashRegistry.Add(hash, name);/* */
                name = string.Concat("0x", hash.ToString("X16"));
            }
            return name;
        }
    }
}
