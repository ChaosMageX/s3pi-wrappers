using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class SurfaceTypeAttributes : FlagFieldViewModel<FTPT.SurfaceType>
    {
        private readonly FTPT.Area mArea;

        public SurfaceTypeAttributes(FTPT.Area area)
        {
            mArea = area;
        }

        public bool Air
        {
            get { return Get(FTPT.SurfaceType.Air); }
            set
            {
                Set(FTPT.SurfaceType.Air, value);
                OnPropertyChanged("Air");
            }
        }

        public bool AnySurface
        {
            get { return Get(FTPT.SurfaceType.AnySurface); }
            set
            {
                Set(FTPT.SurfaceType.AnySurface, value);
                OnPropertyChanged("AnySurface");
            }
        }

        public bool Fence
        {
            get { return Get(FTPT.SurfaceType.Fence); }
            set
            {
                Set(FTPT.SurfaceType.Fence, value);
                OnPropertyChanged("Fence");
            }
        }

        public bool Floor
        {
            get { return Get(FTPT.SurfaceType.Floor); }
            set
            {
                Set(FTPT.SurfaceType.Floor, value);
                OnPropertyChanged("Floor");
            }
        }

        public bool Pond
        {
            get { return Get(FTPT.SurfaceType.Pond); }
            set
            {
                Set(FTPT.SurfaceType.Pond, value);
                OnPropertyChanged("Pond");
            }
        }

        public bool Pool
        {
            get { return Get(FTPT.SurfaceType.Pool); }
            set
            {
                Set(FTPT.SurfaceType.Pool, value);
                OnPropertyChanged("Pool");
            }
        }

        public bool Roof
        {
            get { return Get(FTPT.SurfaceType.Roof); }
            set
            {
                Set(FTPT.SurfaceType.Roof, value);
                OnPropertyChanged("Roof");
            }
        }

        public bool Terrain
        {
            get { return Get(FTPT.SurfaceType.Terrain); }
            set
            {
                Set(FTPT.SurfaceType.Terrain, value);
                OnPropertyChanged("Terrain");
            }
        }

        protected override uint Actual
        {
            get { return (uint) mArea.SurfaceTypeFlags; }
            set { mArea.SurfaceTypeFlags = (FTPT.SurfaceType) value; }
        }
    }
}
