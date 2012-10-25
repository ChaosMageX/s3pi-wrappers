using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Managers
{
    public class THUM
    {
        public static CatalogType CType(IResourceKey rk)
        {
            return (CatalogType) rk.ResourceType;
        }

        public static SpecificResource ItemForTGIBlock0(SpecificResource item)
        {
            IResourceKey rk = ((TGIBlockList) item.Resource["TGIBlocks"].Value)[0];
            return new SpecificResource(FileTable.GameContent, rk);
        }

        private static bool GetThumbnailImageAbort()
        {
            return false;
        }

        private static readonly Image defaultThumbnail = SGResources.defaultThumbnail
            .GetThumbnailImage(256, 256, GetThumbnailImageAbort, IntPtr.Zero);

        /*Image.FromFile(Path.Combine(Path.GetDirectoryName(typeof(THUM).Assembly.Location), 
                "Resources/defaultThumbnail.png"),
            true).GetThumbnailImage(256, 256, () => false, IntPtr.Zero);/**/
        private static readonly Dictionary<uint, uint[]> thumTypes;
        private static readonly ushort[] thumSizes = new ushort[] {32, 64, 128,};
        private static uint defType = 0x319E4F1D;

        static THUM()
        {
            thumTypes = new Dictionary<uint, uint[]>();
            thumTypes.Add(0x034AEECB, new uint[] {0x626F60CC, 0x626F60CD, 0x626F60CE,}); //Create-a-Sim Part
            thumTypes.Add(0x319E4F1D, new uint[] {0x0580A2B4, 0x0580A2B5, 0x0580A2B6,}); //Catalog Object
            thumTypes.Add(0xCF9A4ACE, new uint[] {0x00000000, 0x00000000, 0x00000000,}); //Modular Resource
            thumTypes.Add(0x0418FE2A, new uint[] {0x2653E3C8, 0x2653E3C9, 0x2653E3CA,}); //Catalog Fence
            thumTypes.Add(0x049CA4CD, new uint[] {0x5DE9DBA0, 0x5DE9DBA1, 0x5DE9DBA2,}); //Catalog Stairs
            thumTypes.Add(0x04AC5D93, thumTypes[0x319E4F1D]); //Catalog Proxy Product
            thumTypes.Add(0x04B30669, thumTypes[0x319E4F1D]); //Catalog Terrain Geometry Brush
            thumTypes.Add(0x04C58103, new uint[] {0x2D4284F0, 0x2D4284F1, 0x2D4284F2,}); //Catalog Railing
            thumTypes.Add(0x04ED4BB2, new uint[] {0x05B1B524, 0x05B1B525, 0x05B1B526,}); //Catalog Terrain Paint Brush
            thumTypes.Add(0x04F3CC01, new uint[] {0x05B17698, 0x05B17699, 0x05B1769A,}); //Catalog Fireplace
            thumTypes.Add(0x060B390C, thumTypes[0x319E4F1D]); //Catalog Terrain Water Brush
            thumTypes.Add(0x0A36F07A, thumTypes[0x319E4F1D]); //Catalog Fountain Pool
            thumTypes.Add(0x316C78F2, thumTypes[0x319E4F1D]); //Catalog Foundation
            thumTypes.Add(0x515CA4CD, new uint[] {0x0589DC44, 0x0589DC45, 0x0589DC46,}); //Catalog Wall/Floor Pattern
            thumTypes.Add(0x9151E6BC, new uint[] {0x00000000, 0x00000000, 0x00000000,}); //Catalog Wall -- doesn't have any
            thumTypes.Add(0x91EDBD3E, thumTypes[0x319E4F1D]); //Catalog Roof Style
            thumTypes.Add(0xF1EDBD86, thumTypes[0x319E4F1D]); //Catalog Roof Pattern
        }

        public enum THUMSize
        {
            small = 0,
            medium,
            large,
            defSize = large,
        }

        public static uint[] PNGTypes = new uint[] {0x2E75C764, 0x2E75C765, 0x2E75C766,};

        public static bool isCWALThumbType(uint thumbType)
        {
            //return thumTypes[0x515CA4CD].Contains(type);
            return thumbType == 0x0589DC44
                || thumbType == 0x0589DC45
                || thumbType == 0x0589DC46;
        }

        public static uint getThumbType(uint parentType, THUMSize size, bool isPNGInstance)
        {
            return (isPNGInstance ? PNGTypes : thumTypes[parentType])[(int) size];
        }

        public Image this[ulong instance]
        {
            get { return this[instance, THUMSize.defSize, false]; }
        }

        public Image this[ulong instance, THUMSize size]
        {
            get { return this[instance, size, false]; }
            set { this[instance, size, false] = value; }
        }

        public Image this[ulong instance, bool isPNGInstance]
        {
            get { return this[instance, THUMSize.defSize, isPNGInstance]; }
        }

        public Image this[ulong instance, THUMSize size, bool isPNGInstance]
        {
            get { return this[defType, instance, size, isPNGInstance]; }
            set { this[defType, instance, size, isPNGInstance] = value; }
        }

        public Image this[uint type, ulong instance]
        {
            get { return this[type, instance, THUMSize.defSize, false]; }
        }

        public Image this[uint type, ulong instance, THUMSize size]
        {
            get { return this[type, instance, size, false]; }
            set { this[type, instance, size, false] = value; }
        }

        public Image this[uint type, ulong instance, THUMSize size, bool isPNGInstance]
        {
            get
            {
                SpecificResource item = getItem(isPNGInstance ? FileTable.GameContent : FileTable.Thumbnails, instance, (isPNGInstance ? PNGTypes : thumTypes[type])[(int) size]);
                if (item != null && item.Resource != null)
                    return Image.FromStream(item.Resource.Stream);
                return null;
            }
            set
            {
                SpecificResource item = getItem(isPNGInstance ? FileTable.GameContent : FileTable.Thumbnails, instance, (isPNGInstance ? PNGTypes : thumTypes[type])[(int) size]);
                if (item == null || item.Resource == null)
                    throw new ArgumentException();

                Image thumb;
                thumb = value.GetThumbnailImage(thumSizes[(int) size], thumSizes[(int) size], GetThumbnailImageAbort, IntPtr.Zero);
                thumb.Save(item.Resource.Stream, ImageFormat.Png);
                item.Commit();
            }
        }

        private class ThumbnailComparer : IComparer<IResourceIndexEntry>
        {
            public readonly ulong Instance;
            public readonly uint Type;

            public ThumbnailComparer(ulong instance, uint type)
            {
                Instance = instance;
                Type = type;
            }

            public bool IsThumbnailOf(IResourceIndexEntry rie)
            {
                return rie.ResourceType == Type && rie.Instance == Instance;
            }

            public int Compare(IResourceIndexEntry x, IResourceIndexEntry y)
            {
                return (x.ResourceGroup & 0x07FFFFFF).CompareTo(y.ResourceGroup & 0x07FFFFFF);
            }
        }

        internal static SpecificResource getItem(bool isPNGInstance, ulong instance, uint thumbType)
        {
            return getItem(isPNGInstance ? FileTable.GameContent : FileTable.Thumbnails, instance, thumbType);
        }

        private static SpecificResource getItem(List<PathPackageTuple> ppts, ulong instance, uint thumbType)
        {
            if (ppts == null) return null;
            if (thumbType == 0x00000000) return null;
            var comparer = new ThumbnailComparer(instance, thumbType);
            bool isNotCWAL = !isCWALThumbType(thumbType);
            PathPackageTuple ppt;
            List<IResourceIndexEntry> lsr;
            int i, j;
            for (i = 0; i < ppts.Count; i++)
            {
                ppt = ppts[i];
                lsr = ppt.Package.FindAll(comparer.IsThumbnailOf);
                lsr.Sort(comparer);
                for (j = 0; j < lsr.Count; j++)
                {
                    if (isNotCWAL || (lsr[j].ResourceGroup & 0x00FFFFFF) > 0)
                        return new SpecificResource(ppt, lsr[j]);
                }
            }
            return null;
        }

        public static IResourceKey getImageRK(THUMSize size, SpecificResource item)
        {
            if (CType(item.RequestedRK) == CatalogType.ModularResource)
            {
                return RK.NULL;
            }
            else if (CType(item.RequestedRK) == CatalogType.CAS_Part)
            {
                SpecificResource sr = getRK(item.RequestedRK.ResourceType, item.RequestedRK.Instance, size, false);
                return sr == null ? RK.NULL : sr.RequestedRK;
            }
            else
            {
                ulong png = (item.Resource != null) ? (ulong) item.Resource["CommonBlock.PngInstance"].Value : 0;
                SpecificResource sr = getRK(item.RequestedRK.ResourceType, png != 0 ? png : item.RequestedRK.Instance, size, png != 0);
                return sr == null ? RK.NULL : sr.RequestedRK;
            }
        }

        public static SpecificResource getTHUM(THUMSize size, SpecificResource item)
        {
            if (CType(item.RequestedRK) == CatalogType.ModularResource)
            {
                return null;
            }
            else if (CType(item.RequestedRK) == CatalogType.CAS_Part)
            {
                return getRK(item.RequestedRK.ResourceType, item.RequestedRK.Instance, size, false);
            }
            else
            {
                ulong png = (item.Resource != null) ? (ulong) item.Resource["CommonBlock.PngInstance"].Value : 0;
                return getRK(item.RequestedRK.ResourceType, png != 0 ? png : item.RequestedRK.Instance, size, png != 0);
            }
        }

        private static SpecificResource getRK(uint type, ulong instance, THUMSize size, bool isPNGInstance)
        {
            return getItem(isPNGInstance ? FileTable.GameContent : FileTable.Thumbnails, instance, (isPNGInstance ? PNGTypes : thumTypes[type])[(int) size]);
        }

        public static IResourceKey getNewRK(THUMSize size, SpecificResource item)
        {
            if (CType(item.RequestedRK) == CatalogType.ModularResource)
            {
                return RK.NULL;
            }
            else if (CType(item.RequestedRK) == CatalogType.CAS_Part)
            {
                return getNewRK(item.RequestedRK.ResourceType, item.RequestedRK.Instance, size, false);
            }
            else
            {
                ulong png = (item.Resource != null) ? (ulong) item.Resource["CommonBlock.PngInstance"].Value : 0;
                return getNewRK(item.RequestedRK.ResourceType, png != 0 ? png : item.RequestedRK.Instance, size, png != 0);
            }
        }

        private static IResourceKey getNewRK(uint type, ulong instance, THUMSize size, bool isPNGInstance)
        {
            return new RK(RK.NULL)
                {
                    ResourceType = (isPNGInstance ? PNGTypes : thumTypes[type])[(int) size],
                    ResourceGroup = (uint) (type == 0x515CA4CD ? 1 : 0),
                    Instance = instance,
                };
        }

        public static Image getLargestThumbOrDefault(SpecificResource item)
        {
            Image img = getImage(THUMSize.large, item);
            if (img != null) return img;
            img = getImage(THUMSize.medium, item);
            if (img != null) return img;
            img = getImage(THUMSize.small, item);
            if (img != null) return img;
            return defaultThumbnail;
        }

        public static Image getImage(THUMSize size, SpecificResource item)
        {
            if (CType(item.RequestedRK) == CatalogType.ModularResource)
            {
                return getImage(size, ItemForTGIBlock0(item));
            }
            else if (CType(item.RequestedRK) == CatalogType.CAS_Part)
            {
                return Thumb[item.RequestedRK.ResourceType, item.RequestedRK.Instance, size, false];
            }
            else
            {
                ulong png = (item.Resource != null) ? (ulong) item.Resource["CommonBlock.PngInstance"].Value : 0;
                return Thumb[item.RequestedRK.ResourceType, png != 0 ? png : item.RequestedRK.Instance, size, png != 0];
            }
        }

        private static THUM thumb;

        public static THUM Thumb
        {
            get
            {
                if (thumb == null)
                    thumb = new THUM();
                return thumb;
            }
        }

        /*public static IResourceKey makeImage(THUMSize size, SpecificResource item)
        {
            if (item.CType() == CatalogType.ModularResource)
                return RK.NULL;
            else
            {
                IResourceKey rk = getImageRK(size, item);
                if (rk.Equals(RK.NULL))
                {
                    rk = getNewRK(size, item);
                    SpecificResource thum = FileTable.Current.AddResource(rk);
                    defaultThumbnail.GetThumbnailImage(thumSizes[(int)size], thumSizes[(int)size], gtAbort, System.IntPtr.Zero)
                        .Save(thum.Resource.Stream, System.Drawing.Imaging.ImageFormat.Png);
                    thum.Commit();
                }
                return rk;
            }
        }/**/
    }
}
