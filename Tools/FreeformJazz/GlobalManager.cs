using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Extensions;
using s3pi.Filetable;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.FreeformJazz
{
    public static class GlobalManager
    {
        public const uint kJazzTID = 0x02D5DF13;
        public const uint kClipTID = 0x6B20C4F3;

        public static readonly string kJazzExt;
        public static readonly string kClipExt;

        public static readonly string kJazzS3End;
        public static readonly string kClipS3End;

        public static ResourceMgr JazzManager;
        public static ResourceMgr ClipManager;

        public static event Action<string> StatusMessage;

        static GlobalManager()
        {
            string str;
            List<string> ext;
            bool flag = ExtList.Ext.ContainsKey("*");

            str = string.Concat("0x", kJazzTID.ToString("X8"));
            if (ExtList.Ext.ContainsKey(str))
            {
                ext = ExtList.Ext[str];
                kJazzExt = ext[1];
                kJazzS3End = string.Join("", ext.ToArray());
            }
            else if (flag)
            {
                ext = ExtList.Ext["*"];
                kJazzExt = ext[1];
                kJazzS3End = string.Join("", ext.ToArray());
            }
            else
            {
                kJazzExt = ".dat";
                kJazzS3End = ".dat";
            }

            str = string.Concat("0x", kClipTID.ToString("X8"));
            if (ExtList.Ext.ContainsKey(str))
            {
                ext = ExtList.Ext[str];
                kClipExt = ext[1];
                kClipS3End = string.Join("", ext.ToArray());
            }
            else if (flag)
            {
                ext = ExtList.Ext["*"];
                kClipExt = ext[1];
                kClipS3End = string.Join("", ext.ToArray());
            }
            else
            {
                kClipExt = ".dat";
                kClipS3End = ".dat";
            }
        }

        public static void Load()
        {
            FileTable.FileTableEnabled = true;
            // Load all the name maps into the registry
            if (StatusMessage != null)
            {
                StatusMessage("Loading Key Name Maps...");
            }
            KeyNameReg.RefreshKeyNameMaps();
            // Load all the available jazz graphs into their manager
            if (StatusMessage != null)
            {
                StatusMessage("Searching for Jazz Graph Resources...");
            }
            JazzManager = ResourceMgr.GetResourceManager(kJazzTID);
            JazzManager.LoadAll();
            // Load all the available animations into their manager
            if (StatusMessage != null)
            {
                StatusMessage("Searching for Animation Resources...");
            }
            ClipManager = ResourceMgr.GetResourceManager(kClipTID);
            ClipManager.LoadAll();
            // Close all open package file streams after loading
            if (StatusMessage != null)
            {
                StatusMessage("Closing Package Filestreams...");
            }
            FileTableExt.Reset();
            FileTableExt.CloseCustomContent();
            FileTable.Reset();
        }

        public static void RefreshCurrent()
        {
            // Reset Key Name Map Registry
            KeyNameMap.ResetCurrent();
            KeyNameS3SA.ResetCurrent();
            KeyNameReg.RefreshKeyNameMaps();
            // Reset Resource Managers
            JazzManager.RefreshCurrent();
            ClipManager.RefreshCurrent();
        }
    }
}
