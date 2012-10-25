using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using s3pi.GenericRCOLResource;
using s3piwrappers.Commands;

namespace s3piwrappers.Models
{
    public class AreaViewModel : AbstractViewModel
    {
        private FootprintEditorViewModel mParent;
        private readonly FTPT.Area mArea;
        private readonly ObservableCollection<PointViewModel> mPoints;
        private PointViewModel mSelectedPoint;
        private readonly ICommand mDeletePointCommand;
        private readonly ICommand mAddPointCommand;


        private readonly AreaTypeAttributes mAreaTypeAttributes;
        private readonly SurfaceTypeAttributes mSurfaceTypeAttributes;
        private readonly SurfaceAttributes mSurfaceAttributes;
        private readonly IntersectionAttributes mIntersectionAttributes;

        public AreaViewModel(FootprintEditorViewModel parent, FTPT.Area area)
        {
            mParent = parent;
            mArea = area;
            mAreaTypeAttributes = new AreaTypeAttributes(mArea);
            mSurfaceTypeAttributes = new SurfaceTypeAttributes(mArea);
            mSurfaceAttributes = new SurfaceAttributes(mArea);
            mIntersectionAttributes = new IntersectionAttributes(mArea);

            mPoints = new ObservableCollection<PointViewModel>();
            foreach (FTPT.PolygonPoint pt in area.ClosedPolygon)
            {
                Add(new PointViewModel(this, pt));
            }
            SelectedPoint = Points.FirstOrDefault();
            mDeletePointCommand = new UserCommand<AreaViewModel>(x => x != null && x.SelectedPoint != null && x.Points.Contains(x.SelectedPoint), x => x.Remove(x.SelectedPoint));

            mAddPointCommand = new UserCommand<AreaViewModel>(x => x != null && true, x => x.Add());
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
            mArea.ClosedPolygon.Add();
            FTPT.PolygonPoint point = mArea.ClosedPolygon.Last();
            point.X = OffsetX;
            point.Z = OffsetZ;
            var vm = new PointViewModel(this, point);
            Add(vm);
        }

        public void Add(PointViewModel vm)
        {
            vm.PropertyChanged += OnPointChanged;
            mPoints.Add(vm);
            SelectedPoint = vm;
            OnPropertyChanged("Points");
        }

        public void Remove(PointViewModel model)
        {
            model.PropertyChanged -= OnPointChanged;
            mPoints.Remove(model);
            SelectedPoint = mPoints.LastOrDefault();
            OnPropertyChanged("Points");
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
            set
            {
                mArea.Priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public uint Name
        {
            get { return mArea.Name; }
            set
            {
                mArea.Name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Text");
            }
        }

        public byte LevelOffset
        {
            get { return mArea.LevelOffset; }
            set
            {
                mArea.LevelOffset = value;
                OnPropertyChanged("LevelOffset");
            }
        }

        public float OffsetZ
        {
            get { return mArea.ClosedPolygon.Any() ? mArea.ClosedPolygon.Min(x => x.Z) : 0f; }
            set
            {
                float diff = value - OffsetZ;
                foreach (PointViewModel pointViewModel in Points)
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
                float diff = value - OffsetX;
                foreach (PointViewModel pointViewModel in Points)
                {
                    pointViewModel.X += diff;
                }
                OnPropertyChanged("OffsetX");
                OnPropertyChanged("Points");
            }
        }

        public string Text
        {
            get { return string.Format("0x{0:X8}", Name); }
        }

        public PointViewModel SelectedPoint
        {
            get { return mSelectedPoint; }
            set
            {
                mSelectedPoint = value;
                OnPropertyChanged("SelectedPoint");
            }
        }

        public ObservableCollection<PointViewModel> Points
        {
            get { return mPoints; }
        }
    }
}
