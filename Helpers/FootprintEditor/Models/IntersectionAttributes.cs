using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class IntersectionAttributes : FlagFieldViewModel<FTPT.AllowIntersection>
    {
        private FTPT.Area mArea;

        public IntersectionAttributes(FTPT.Area area)
        {
            mArea = area;
        }
        public bool Fences
        {
            get { return this.Get(FTPT.AllowIntersection.Fences); }
            set { this.Set(FTPT.AllowIntersection.Fences, value); OnPropertyChanged("Fences"); }
        }

        public bool ModularStairs
        {
            get { return this.Get(FTPT.AllowIntersection.ModularStairs); }
            set { this.Set(FTPT.AllowIntersection.ModularStairs, value); OnPropertyChanged("ModularStairs"); }
        }
        public bool Objects
        {
            get { return this.Get(FTPT.AllowIntersection.Objects); }
            set { this.Set(FTPT.AllowIntersection.Objects, value); OnPropertyChanged("Objects"); }
        }
        public bool ObjectsOfSameType
        {
            get { return this.Get(FTPT.AllowIntersection.ObjectsOfSameType); }
            set { this.Set(FTPT.AllowIntersection.ObjectsOfSameType, value); OnPropertyChanged("ObjectsOfSameType"); }
        }
        public bool Roofs
        {
            get { return this.Get(FTPT.AllowIntersection.Roofs); }
            set { this.Set(FTPT.AllowIntersection.Roofs, value); OnPropertyChanged("Roofs"); }
        }
        public bool Sims
        {
            get { return this.Get(FTPT.AllowIntersection.Sims); }
            set { this.Set(FTPT.AllowIntersection.Sims, value); OnPropertyChanged("Sims"); }
        }
        public bool Walls
        {
            get { return this.Get(FTPT.AllowIntersection.Walls); }
            set { this.Set(FTPT.AllowIntersection.Walls, value); OnPropertyChanged("Walls"); }
        }

        protected override uint Actual
        {
            get { return (uint)this.mArea.AllowIntersectionFlags;  }
            set { this.mArea.AllowIntersectionFlags = (FTPT.AllowIntersection) value; }
        }
    }
}