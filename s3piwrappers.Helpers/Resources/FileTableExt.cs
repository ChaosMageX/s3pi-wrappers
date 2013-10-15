using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Filetable;

namespace s3piwrappers.Helpers.Resources
{
    public static class FileTableExt
    {
        public static readonly List<PathPackageTuple> Current
            = new List<PathPackageTuple>();

        // <GameCore>
        //   <Folder path="Game/Bin">
        //     <Package isPatch="false" priority="0">gameplay.package</Package>
        //     <Package isPatch="false" priority="0">scripts.package</Package>
        //     <Package isPatch="false" priority="0">simcore.package</Package>
        //     <Package isPatch="false" priority="0">Gameplay/GameplayData.package</Package>
        //     <Package isPatch="false" priority="0">Jazz/JazzData.package</Package>
        //     <Package isPatch="false" priority="0">Misc/fallback.package</Package>
        //     <Package isPatch="false" priority="0">UI/UI.package</Package>
        //   </Folder>
        // </GameCore>

        private static readonly string sCoreFolderPath = "Game/Bin";

        private static readonly string[] sCorePackagePaths = new string[]
        {
            "gameplay.package", "scripts.package", "simcore.package",
            "Gameplay/GameplayData.package",
            "Jazz/JazzData.package",
            "Misc/fallback.package",
            "UI/UI.package"
        };

        private class GamePriorityComp : IComparer<Game>
        {
            public static readonly GamePriorityComp Instance
                = new GamePriorityComp();

            public int Compare(Game x, Game y)
            {
                return x.Priority == y.Priority 
                    ? 0 : (x.Priority > y.Priority ? -1 : 1);
            }
        }

        private class PTagPriorityComp : IComparer<PackageTag>
        {
            public static readonly PTagPriorityComp Instance
                = new PTagPriorityComp();

            public int Compare(PackageTag x, PackageTag y)
            {
                return x.Priority == y.Priority
                    ? 0 : (x.Priority > y.Priority ? -1 : 1);
            }
        }

        private static List<PackageTag> GetGameCorePackageTags()
        {
            int i, j, index;
            string folderPath, path, fullPath;
            Game[] games = GameFolders.Games.ToArray();
            Array.Sort(games, 0, games.Length, GamePriorityComp.Instance);
            
            PackageTag pTag;
            List<PackageTag> pTags = new List<PackageTag>();
            foreach (Game game in games)
            {
                if (game.Enabled)
                {
                    folderPath = Path.Combine(
                        game.UserInstallDir, sCoreFolderPath);
                    if (Directory.Exists(folderPath))
                    {
                        for (i = 0; i < sCorePackagePaths.Length; i++)
                        {
                            path = Path.Combine(
                                folderPath, sCorePackagePaths[i]);
                            if (File.Exists(path))
                            {
                                fullPath = Path.GetFullPath(path);
                                index = -1;
                                for (j = pTags.Count - 1; 
                                     j >= 0 && index < 0; j--)
                                {
                                    pTag = pTags[j];
                                    if (fullPath == 
                                        Path.GetFullPath(pTag.Path))
                                    {
                                        index = j;
                                    }
                                }
                                if (index < 0)
                                {
                                    pTag = new PackageTag(path, false, 0);
                                    pTags.Add(pTag);
                                }
                            }
                        }
                    }
                }
            }
            if (pTags.Count > 1)
            {
                pTags.Sort(PTagPriorityComp.Instance);
            }
            return pTags;
        }

        private static List<PathPackageTuple> sGameCorePackages = null;

        private static List<PathPackageTuple> GetGameCoreList()
        {
            List<PathPackageTuple> res = new List<PathPackageTuple>();
            if (FileTable.Current != null)
            {
                res.Add(FileTable.Current);
            }
            if (FileTable.CustomContentEnabled &&
                FileTable.CustomContent != null)
            {
                res.AddRange(FileTable.CustomContent);
            }
            if (FileTable.FileTableEnabled)
            {
                if (sGameCorePackages == null)
                {
                    List<PackageTag> pTags = GetGameCorePackageTags();
                    sGameCorePackages 
                        = new List<PathPackageTuple>(pTags.Count);
                    foreach (PackageTag p in pTags)
                    {
                        sGameCorePackages.Add(
                            new PathPackageTuple(p.Path, false));
                    }
                }
                res.AddRange(sGameCorePackages);
            }
            return res.Count == 0 ? null : res;
        }

        public static List<PathPackageTuple> GameCore
        {
            get { return GetGameCoreList(); }
        }

        public static void Reset()
        {
            if (sGameCorePackages != null)
            {
                foreach (PathPackageTuple ppt in sGameCorePackages)
                {
                    if (ppt.Package != null)
                    {
                        s3pi.Package.Package.ClosePackage(0, ppt.Package);
                    }
                }
                sGameCorePackages = null;
            }
        }

        public static void CloseCustomContent()
        {
            if (FileTable.CustomContent != null)
            {
                foreach (PathPackageTuple ppt in FileTable.CustomContent)
                {
                    if (ppt != null && ppt.Package != null)
                    {
                        s3pi.Package.Package.ClosePackage(0, ppt.Package);
                    }
                }
            }
        }
    }
}
