using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class SurfaceAttributes : FlagFieldViewModel<FTPT.SurfaceAttribute>
    {

        private FTPT.Area mArea;
        public SurfaceAttributes(FTPT.Area area)
        {
            mArea = area;
            
        }

        public bool Inside
        {
            get { return this.Get(FTPT.SurfaceAttribute.Inside); }
            set { this.Set(FTPT.SurfaceAttribute.Inside, value); OnPropertyChanged("Inside"); }
        }



        public bool Outside
        {
            get { return this.Get(FTPT.SurfaceAttribute.Outside); }
            set { this.Set(FTPT.SurfaceAttribute.Outside, value); OnPropertyChanged("Outside"); }
        }

        public bool Slope
        {
            get { return this.Get(FTPT.SurfaceAttribute.Slope); }
            set { this.Set(FTPT.SurfaceAttribute.Slope, value); OnPropertyChanged("Slope"); }
        }

        public bool Unknown08
        {
            get { return this.Get(FTPT.SurfaceAttribute.Unknown08); }
            set { this.Set(FTPT.SurfaceAttribute.Unknown08, value); OnPropertyChanged("Unknown08"); }
        }

        protected override uint Actual
        {
            get { return (uint) mArea.SurfaceAttributeFlags; }
            set { mArea.SurfaceAttributeFlags = (FTPT.SurfaceAttribute) value; }
        }
    }
}