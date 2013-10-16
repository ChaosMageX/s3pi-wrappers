using System;
using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.Helpers.Resources
{
    public static class KeyNameReg
    {
        [Flags]
        public enum KNM : byte
        {
            None = 0x00,
            Imported = 0x01,
            Current = 0x02,
            CustomContent = 0x04,
            GameCore = 0x08,
            GameContent = 0x10,
            DDSImages = 0x20,
            Thumbnails = 0x40,
            All = 0x7F
        }

        /// <summary>
        /// Key Name Maps to include in searches 
        /// for names, labels, and generic CLIPs.
        /// </summary>
        public static KNM IncludedKeyNameMaps = KNM.All;

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

        public static bool HasName(uint hash)
        {
            string name;
            return TryFindName(hash, out name);
        }

        public static bool TryFindName(uint hash, out string name)
        {
            name = null;
            if (hash == 0)
            {
                return true;
            }
            if (hash == FNVHash.TS3Offset32)
            {
                name = "";
                return true;
            }
            if (IncludedKeyNameMaps == KNM.None)
            {
                return false;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if ((IncludedKeyNameMaps & KNM.Imported) != KNM.None &&
                KeyNameMap.Imported.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Current) != KNM.None &&
                sCurrntKNM != null &&
                sCurrntKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.CustomContent) != KNM.None && 
                sCustomKNM != null &&
                sCustomKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameCore) != KNM.None &&
                sGCoreKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameContent) != KNM.None &&
                sGContKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.DDSImages) != KNM.None &&
                sImageKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Thumbnails) != KNM.None &&
                sThumbKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sCurrntKNA != null &&
                sCurrntKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNA != null &&
                sCustomKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGCoreKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNA.TryFindName(hash, out name))
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
            name = null;
            if (hash == 0)
            {
                return true;
            }
            if (hash == FNVHash.TS3Offset64)
            {
                name = "";
                return true;
            }
            if (IncludedKeyNameMaps == KNM.None)
            {
                return false;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if ((IncludedKeyNameMaps & KNM.Imported) != KNM.None &&
                KeyNameMap.Imported.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Current) != KNM.None &&
                sCurrntKNM != null &&
                sCurrntKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.CustomContent) != KNM.None &&
                sCustomKNM != null &&
                sCustomKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameCore) != KNM.None &&
                sGCoreKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameContent) != KNM.None &&
                sGContKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.DDSImages) != KNM.None &&
                sImageKNM.TryFindName(hash, out name))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Thumbnails) != KNM.None &&
                sThumbKNM.TryFindName(hash, out name))
            {
                return true;
            }
            /*if (sCurrntKNA != null &&
                sCurrntKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sCustomKNA != null &&
                sCustomKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGCoreKNA.TryFindName(hash, out name))
            {
                return true;
            }
            if (sGContKNA.TryFindName(hash, out name))
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
            label = null;
            if (IncludedKeyNameMaps == KNM.None)
            {
                return false;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if ((IncludedKeyNameMaps & KNM.Imported) != KNM.None &&
                KeyNameMap.Imported.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Current) != KNM.None &&
                sCurrntKNM != null &&
                sCurrntKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.CustomContent) != KNM.None &&
                sCustomKNM != null &&
                sCustomKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameCore) != KNM.None &&
                sGCoreKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameContent) != KNM.None &&
                sGContKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.DDSImages) != KNM.None &&
                sImageKNM.TryFindLabel(hash, out label))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Thumbnails) != KNM.None &&
                sThumbKNM.TryFindLabel(hash, out label))
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
            str = null;
            if (IncludedKeyNameMaps == KNM.None)
            {
                return false;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if ((IncludedKeyNameMaps & KNM.Imported) != KNM.None &&
                KeyNameMap.Imported.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Current) != KNM.None &&
                sCurrntKNM != null &&
                sCurrntKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.CustomContent) != KNM.None &&
                sCustomKNM != null &&
                sCustomKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameCore) != KNM.None &&
                sGCoreKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameContent) != KNM.None &&
                sGContKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.DDSImages) != KNM.None &&
                sImageKNM.TryFindNameOrLabel(hash, out str))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Thumbnails) != KNM.None &&
                sThumbKNM.TryFindNameOrLabel(hash, out str))
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
            clipName = null;
            if (clipIID == 0)
            {
                return true;
            }
            if (clipIID == (FNVHash.TS3Offset64 & 0x7FFFFFFFFFFFFFFF))
            {
                clipName = "";
                return true;
            }
            if (IncludedKeyNameMaps == KNM.None)
            {
                return false;
            }
            if (sGCoreKNM == null)
            {
                RefreshKeyNameMaps();
            }
            if ((IncludedKeyNameMaps & KNM.Imported) != KNM.None &&
                KeyNameMap.Imported.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Current) != KNM.None &&
                sCurrntKNM != null &&
                sCurrntKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.CustomContent) != KNM.None &&
                sCustomKNM != null &&
                sCustomKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameCore) != KNM.None &&
                sGCoreKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.GameContent) != KNM.None &&
                sGContKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.DDSImages) != KNM.None &&
                sImageKNM.TryFindGenCLIP(clipIID, out clipName))
            {
                return true;
            }
            if ((IncludedKeyNameMaps & KNM.Thumbnails) != KNM.None &&
                sThumbKNM.TryFindGenCLIP(clipIID, out clipName))
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
