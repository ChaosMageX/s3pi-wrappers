using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class AreaTypeAttributes : FlagFieldViewModel<FTPT.AreaType>
    {
        private FTPT.Area mArea;

        public AreaTypeAttributes(FTPT.Area area)
        {
            mArea = area;
        }
        public bool ForPlacement
        {
            get { return this.Get(FTPT.AreaType.ForPlacement); }
            set { this.Set(FTPT.AreaType.ForPlacement, value); OnPropertyChanged("ForPlacement"); }
        }
        public bool ForPathing
        {
            get { return this.Get(FTPT.AreaType.ForPathing); }
            set { this.Set(FTPT.AreaType.ForPathing, value); OnPropertyChanged("ForPathing"); }
        }
        public bool ForShell
        {
            get { return this.Get(FTPT.AreaType.ForShell); }
            set { this.Set(FTPT.AreaType.ForShell, value); OnPropertyChanged("ForShell"); }
        }
        public bool IsDiscouraged
        {
            get { return this.Get(FTPT.AreaType.IsDiscouraged); }
            set { this.Set(FTPT.AreaType.IsDiscouraged, value); OnPropertyChanged("IsDiscouraged"); }
        }
        public bool IsEnabled
        {
            get { return this.Get(FTPT.AreaType.IsEnabled); }
            set { this.Set(FTPT.AreaType.IsEnabled, value); OnPropertyChanged("IsEnabled"); }
        }
        protected override uint Actual
        {
            get { return (uint) mArea.AreaTypeFlags; }
            set { mArea.AreaTypeFlags = (FTPT.AreaType) value; }
        }
    }
}