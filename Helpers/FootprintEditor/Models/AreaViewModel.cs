using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using s3pi.GenericRCOLResource;
using s3piwrappers.Commands;
using System;

namespace s3piwrappers.Models
{
    public class AreaViewModel : AbstractViewModel
    {
        private FootprintEditorViewModel mParent;
        private FTPT.Area mArea;
        private ObservableCollection<PointViewModel> mPoints;
        private PointViewModel mSelectedPoint;
        private ICommand mDeletePointCommand;
        private ICommand mAddPointCommand;


        private AreaTypeAttributes mAreaTypeAttributes;
        private SurfaceTypeAttributes mSurfaceTypeAttributes;
        private SurfaceAttributes mSurfaceAttributes;
        private IntersectionAttributes mIntersectionAttributes;
        public AreaViewModel(FootprintEditorViewModel parent, FTPT.Area area)
        {
            this.mParent = parent;
            this.mArea = area;
            this.mAreaTypeAttributes = new AreaTypeAttributes(this.mArea);
            this.mSurfaceTypeAttributes = new SurfaceTypeAttributes(this.mArea);
            this.mSurfaceAttributes = new SurfaceAttributes(this.mArea);
            this.mIntersectionAttributes = new IntersectionAttributes(this.mArea);

            this.mPoints = new ObservableCollection<PointViewModel>();
            foreach (var pt in area.ClosedPolygon)
            {
                this.Add(new PointViewModel(this, pt));
            }
            this.SelectedPoint = this.Points.FirstOrDefault();
            this.mDeletePointCommand = new UserCommand<AreaViewModel>(x => x != null && x.SelectedPoint != null && x.Points.Contains(x.SelectedPoint), x => x.Remove(x.SelectedPoint));
            
            this.mAddPointCommand = new UserCommand<AreaViewModel>(x => x != null && true, x => x.Add());
            
        }

        public IntersectionAttributes IntersectionAttributes
        {
            get { return mIntersectionAttributes; }
        }

        public SurfaceAttributes SurfaceAttributes
        {
            get { return mSurfaceAttributes; }
        }

        public SurfaceTypeAttributes SurfaceTypeAttributes
        {
            get { return mSurfaceTypeAttributes; }
        }

        public AreaTypeAttributes AreaTypeAttributes
        {
            get { return mAreaTypeAttributes; }
        }

        public FTPT.Area Area
        {
            get { return mArea; }
        }

        public ICommand AddPointCommand
        {
            get { return mAddPointCommand; }
        }

        public ICommand DeletePointCommand
        {
            get { return mDeletePointCommand; }
        }

        public void Add()
        {
            this.mArea.ClosedPolygon.Add();
            var point = this.mArea.ClosedPolygon.Last();
            point.X = this.OffsetX;
            point.Z = this.OffsetZ;
            var vm = new PointViewModel(this, point);
            this.Add(vm);
        }
        public void Add(PointViewModel vm)
        {
            vm.PropertyChanged += OnPointChanged;
            this.mPoints.Add(vm);
            this.SelectedPoint = vm;
            this.OnPropertyChanged("Points");
        }
        public void Remove(PointViewModel model)
        {
            model.PropertyChanged -= OnPointChanged;
            this.mPoints.Remove(model);
            this.SelectedPoint = this.mPoints.LastOrDefault();
            this.OnPropertyChanged("Points");
        }
        private void OnPointChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Points");
            OnPropertyChanged("OffsetZ");
            OnPropertyChanged("OffsetX");
        }
        public Byte Priority
        {
            get { return mArea.Priority; }
            set { mArea.Priority = value;OnPropertyChanged("Priority"); }
        }
        public uint Name
        {
            get { return mArea.Name; }
            set { mArea.Name = value; OnPropertyChanged("Name"); OnPropertyChanged("Text"); }
        }

        public byte LevelOffset
        {
            get { return mArea.LevelOffset; }
            set { mArea.LevelOffset = value; OnPropertyChanged("LevelOffset"); }
        }

        public float OffsetZ
        {
            get { return mArea.ClosedPolygon.Any() ? mArea.ClosedPolygon.Min(x => x.Z) : 0f; }
            set
            {
                var diff =value- this.OffsetZ ;
                foreach (var pointViewModel in Points)
                {
                    pointViewModel.Z += diff;
                }
                OnPropertyChanged("OffsetZ");
                OnPropertyChanged("Points");
            }
        }

        public float OffsetX
        {
            get { return mArea.ClosedPolygon.Any() ? mArea.ClosedPolygon.Min(x => x.X) : 0f; }
            set
            {
                var diff = value - this.OffsetX ;
                foreach (var pointViewModel in Points)
                {
                    pointViewModel.X += diff;
                }
                OnPropertyChanged("OffsetX");
                OnPropertyChanged("Points");
            }
        }
        public string Text
        {
            get { return string.Format("0x{0:X8}", this.Name); }
        }
        public PointViewModel SelectedPoint
        {
            get { return mSelectedPoint; }
            set { mSelectedPoint = value; OnPropertyChanged("SelectedPoint"); }
        }

        public ObservableCollection<PointViewModel> Points
        {
            get { return mPoints; }
        }
    }
}