using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class IntersectionAttributes : FlagFieldViewModel<FTPT.AllowIntersection>
    {
        private readonly FTPT.Area mArea;

        public IntersectionAttributes(FTPT.Area area)
        {
            mArea = area;
        }

        public bool Fences
        {
            get { return Get(FTPT.AllowIntersection.Fences); }
            set
            {
                Set(FTPT.AllowIntersection.Fences, value);
                OnPropertyChanged("Fences");
            }
        }

        public bool ModularStairs
        {
            get { return Get(FTPT.AllowIntersection.ModularStairs); }
            set
            {
                Set(FTPT.AllowIntersection.ModularStairs, value);
                OnPropertyChanged("ModularStairs");
            }
        }

        public bool Objects
        {
            get { return Get(FTPT.AllowIntersection.Objects); }
            set
            {
                Set(FTPT.AllowIntersection.Objects, value);
                OnPropertyChanged("Objects");
            }
        }

        public bool ObjectsOfSameType
        {
            get { return Get(FTPT.AllowIntersection.ObjectsOfSameType); }
            set
            {
                Set(FTPT.AllowIntersection.ObjectsOfSameType, value);
                OnPropertyChanged("ObjectsOfSameType");
            }
        }

        public bool Roofs
        {
            get { return Get(FTPT.AllowIntersection.Roofs); }
            set
            {
                Set(FTPT.AllowIntersection.Roofs, value);
                OnPropertyChanged("Roofs");
            }
        }

        public bool Sims
        {
            get { return Get(FTPT.AllowIntersection.Sims); }
            set
            {
                Set(FTPT.AllowIntersection.Sims, value);
                OnPropertyChanged("Sims");
            }
        }

        public bool Walls
        {
            get { return Get(FTPT.AllowIntersection.Walls); }
            set
            {
                Set(FTPT.AllowIntersection.Walls, value);
                OnPropertyChanged("Walls");
            }
        }

        protected override uint Actual
        {
            get { return (uint) mArea.AllowIntersectionFlags; }
            set { mArea.AllowIntersectionFlags = (FTPT.AllowIntersection) value; }
        }
    }
}
