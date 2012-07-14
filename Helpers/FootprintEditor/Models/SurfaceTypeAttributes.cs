using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class SurfaceTypeAttributes : FlagFieldViewModel<FTPT.SurfaceType>
    {
        
        private FTPT.Area mArea;

        public SurfaceTypeAttributes(FTPT.Area area)
        {
            mArea = area;
        }

        public bool Air
        {
            get { return this.Get(FTPT.SurfaceType.Air); }
            set { this.Set(FTPT.SurfaceType.Air, value); OnPropertyChanged("Air"); }
        }
        public bool AnySurface
        {
            get { return this.Get(FTPT.SurfaceType.AnySurface); }
            set { this.Set(FTPT.SurfaceType.AnySurface, value); OnPropertyChanged("AnySurface"); }
        }

        public bool Fence
        {
            get { return this.Get(FTPT.SurfaceType.Fence); }
            set { this.Set(FTPT.SurfaceType.Fence, value); OnPropertyChanged("Fence"); }
        }

        public bool Floor
        {
            get { return this.Get(FTPT.SurfaceType.Floor); }
            set { this.Set(FTPT.SurfaceType.Floor, value); OnPropertyChanged("Floor"); }
        }

        public bool Pond
        {
            get { return this.Get(FTPT.SurfaceType.Pond); }
            set { this.Set(FTPT.SurfaceType.Pond, value); OnPropertyChanged("Pond"); }
        }

        public bool Pool
        {
            get { return this.Get(FTPT.SurfaceType.Pool); }
            set { this.Set(FTPT.SurfaceType.Pool, value); OnPropertyChanged("Pool"); }
        }

        public bool Roof
        {
            get { return this.Get(FTPT.SurfaceType.Roof); }
            set { this.Set(FTPT.SurfaceType.Roof, value); OnPropertyChanged("Roof"); }
        }
        public bool Terrain
        {
            get { return this.Get(FTPT.SurfaceType.Terrain); }
            set { this.Set(FTPT.SurfaceType.Terrain, value); OnPropertyChanged("Terrain"); }
        }
        protected override uint Actual
        {
            get { return (uint) mArea.SurfaceTypeFlags; }
            set { mArea.SurfaceTypeFlags = (FTPT.SurfaceType) value; }
        }
    }
}