using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using FootprintViewer.Commands;
using s3pi.GenericRCOLResource;

namespace FootprintViewer.Models
{
    public class AreaViewModel : AbstractViewModel
    {
        private FootprintEditorViewModel mParent;
        private FTPT.Area mArea;
        private ObservableCollection<PointViewModel> mPoints;
        private PointViewModel mSelectedPoint;
        private float mOffsetX;
        private float mOffsetZ;
        private ICommand mDeletePointCommand;
        private ICommand mAddPointCommand;
        public AreaViewModel(FootprintEditorViewModel parent, FTPT.Area area)
        {
            mParent = parent;
            mArea = area;
            if (mArea.ClosedPolygon.Any())
            {
                this.mOffsetX = mArea.ClosedPolygon.Min(x => x.X);
                this.mOffsetZ = mArea.ClosedPolygon.Min(x => x.Z);
            }
            mPoints = new ObservableCollection<PointViewModel>();
            foreach (var pt in area.ClosedPolygon)
            {
                Add(new PointViewModel(this, pt));
            }
            this.SelectedPoint = this.Points.FirstOrDefault();
            this.mDeletePointCommand = new UserCommand<AreaViewModel>(x => x != null && x.SelectedPoint != null && x.Points.Contains(x.SelectedPoint), x => x.Remove(x.SelectedPoint));
            this.mAddPointCommand = new UserCommand<AreaViewModel>(x => x != null && true, x => x.Add());
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
            point.X = this.mOffsetX;
            point.Z = this.mOffsetZ;
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
            FixOffsets();
        }
        private void FixOffsets()
        {
            var min = this.Area.Lower;
            min.X = 0;
            min.Z = 0;
            var max = this.Area.Upper;
            max.X = 0;
            max.Z = 0;
            foreach (var pointViewModel in Points)
            {
                var point = pointViewModel.Point;
                point.X = this.OffsetX + pointViewModel.X;
                point.Z = this.OffsetZ + pointViewModel.Z;
                if (point.X < min.X)
                {
                    min.X = point.X;
                }
                if (point.X > max.X)
                {
                    max.X = point.X;
                }
                if (point.Z < min.Z)
                {
                    min.Z = point.Z;
                }
                if (point.Z > max.Z)
                {
                    max.Z = point.Z;
                }
            }
            this.OnPropertyChanged("Points");
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
            get { return mOffsetZ; }
            set
            {
                mOffsetZ = value;
                FixOffsets();
                OnPropertyChanged("OffsetZ");
            }
        }

        public float OffsetX
        {
            get { return mOffsetX; }
            set
            {
                mOffsetX = value;
                FixOffsets();
                OnPropertyChanged("OffsetX");
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