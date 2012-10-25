using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class AreaTypeAttributes : FlagFieldViewModel<FTPT.AreaType>
    {
        private readonly FTPT.Area mArea;

        public AreaTypeAttributes(FTPT.Area area)
        {
            mArea = area;
        }

        public bool ForPlacement
        {
            get { return Get(FTPT.AreaType.ForPlacement); }
            set
            {
                Set(FTPT.AreaType.ForPlacement, value);
                OnPropertyChanged("ForPlacement");
            }
        }

        public bool ForPathing
        {
            get { return Get(FTPT.AreaType.ForPathing); }
            set
            {
                Set(FTPT.AreaType.ForPathing, value);
                OnPropertyChanged("ForPathing");
            }
        }

        public bool ForShell
        {
            get { return Get(FTPT.AreaType.ForShell); }
            set
            {
                Set(FTPT.AreaType.ForShell, value);
                OnPropertyChanged("ForShell");
            }
        }

        public bool IsDiscouraged
        {
            get { return Get(FTPT.AreaType.IsDiscouraged); }
            set
            {
                Set(FTPT.AreaType.IsDiscouraged, value);
                OnPropertyChanged("IsDiscouraged");
            }
        }

        public bool IsEnabled
        {
            get { return Get(FTPT.AreaType.IsEnabled); }
            set
            {
                Set(FTPT.AreaType.IsEnabled, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        protected override uint Actual
        {
            get { return (uint) mArea.AreaTypeFlags; }
            set { mArea.AreaTypeFlags = (FTPT.AreaType) value; }
        }
    }
}
