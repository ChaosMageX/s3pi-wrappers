using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using s3pi.GenericRCOLResource;
using s3piwrappers.Commands;

namespace s3piwrappers.Models
{
    public class FootprintEditorViewModel : AbstractViewModel
    {
        private GenericRCOLResource mRcol;
        private readonly FTPT mFootprint;
        private readonly ObservableCollection<AreaViewModel> mFootprintAreas;
        private readonly ObservableCollection<AreaViewModel> mSlotAreas;
        private bool mIsSaving;
        private AreaViewModel mSelectedArea;

        private double? mCursorX;

        public double? CursorX
        {
            get { return mCursorX; }
            set
            {
                mCursorX = value;
                OnPropertyChanged("CursorX");
                OnPropertyChanged("StatusText");
            }
        }

        private double? mCursorZ;

        public double? CursorZ
        {
            get { return mCursorZ; }
            set
            {
                mCursorZ = value;
                OnPropertyChanged("CursorZ");
                OnPropertyChanged("StatusText");
            }
        }

        private string mBackgroundImagePath;

        public string BackgroundImagePath
        {
            get { return mBackgroundImagePath; }
            set
            {
                mBackgroundImagePath = value;
                OnPropertyChanged("BackgroundImagePath");
            }
        }

        public string StatusText
        {
            get
            {
                var sb = new StringBuilder();
                if (CursorX != null && CursorZ != null)
                {
                    sb.AppendFormat("({0:0.00},{1:0.00})", CursorX, CursorZ);
                }
                return sb.ToString();
            }
        }

        private AreaViewModel mSelectedFootprint;

        private AreaViewModel mSelectedSlot;

        public AreaViewModel SelectedSlot
        {
            get { return mSelectedSlot; }
            set
            {
                mSelectedSlot = value;
                OnPropertyChanged("SelectedSlot");
                if (value != null)
                {
                    SelectedFootprint = null;
                    SelectedArea = value;
                }
            }
        }

        public AreaViewModel SelectedFootprint
        {
            get { return mSelectedFootprint; }
            set
            {
                mSelectedFootprint = value;
                OnPropertyChanged("SelectedFootprint");
                if (value != null)
                {
                    SelectedSlot = null;
                    SelectedArea = value;
                }
            }
        }

        private string mReferenceMesh;

        public String ReferenceMesh
        {
            get { return mReferenceMesh; }
            set
            {
                mReferenceMesh = value;
                OnPropertyChanged("ReferenceMesh");
            }
        }

        public ObservableCollection<AreaViewModel> SlotAreas
        {
            get { return mSlotAreas; }
        }

        public AreaViewModel SelectedArea
        {
            get { return mSelectedArea; }
            set
            {
                mSelectedArea = value;
                OnPropertyChanged("SelectedArea");
                OnPropertyChanged("SelectedArea");
            }
        }

        public ObservableCollection<AreaViewModel> FootprintAreas
        {
            get { return mFootprintAreas; }
        }

        public bool IsSaving
        {
            get { return mIsSaving; }
        }


        public ICommand AddSlotCommand { get; private set; }
        public ICommand DeleteSlotCommand { get; private set; }
        public ICommand CloneSlotCommand { get; private set; }


        public ICommand AddFootprintCommand { get; private set; }
        public ICommand DeleteFootprintCommand { get; private set; }
        public ICommand CloneFootprintCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }
        public ICommand CommitCommand { get; private set; }

        public ICommand SetBackgroundImageCommand { get; private set; }
        public ICommand ClearBackgroundImageCommand { get; private set; }
        public ICommand SetReferenceMeshCommand { get; private set; }
        public ICommand ClearReferenceMeshCommand { get; private set; }

        public FootprintEditorViewModel(GenericRCOLResource rcol)
        {
            mRcol = rcol;
            mFootprint = rcol.ChunkEntries[0].RCOLBlock as FTPT;
            mFootprintAreas = new ObservableCollection<AreaViewModel>(mFootprint.FootprintAreas.Select(x => new AreaViewModel(this, x)));
            mSlotAreas = new ObservableCollection<AreaViewModel>(mFootprint.SlotAreas.Select(x => new AreaViewModel(this, x)));

            AddSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null, x => x.AddSlotArea());
            CloneSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckSlotArea(), x => x.CopySlotArea());
            DeleteSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckSlotArea(), x => x.DeleteArea());


            AddFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null, x => x.AddFootprintArea());
            CloneFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckFootprintArea(), x => x.CopyFootprintArea());
            DeleteFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckFootprintArea(), x => x.DeleteArea());


            CommitCommand = new UserCommand<FootprintEditorViewModel>(x => true, y =>
                {
                    mIsSaving = true;
                    Application.Current.Shutdown();
                });
            CancelCommand = new UserCommand<FootprintEditorViewModel>(x => true, y =>
                {
                    mIsSaving = false;
                    Application.Current.Shutdown();
                });


            SetBackgroundImageCommand = new UserCommand<FootprintEditorViewModel>(
                x => true,
                y =>
                    {
                        var d = new OpenFileDialog {CheckFileExists = true, Multiselect = false};
                        if (d.ShowDialog() == true)
                        {
                            BackgroundImagePath = d.FileName;
                        }
                    }
                );
            ClearBackgroundImageCommand = new UserCommand<FootprintEditorViewModel>(
                x => x != null && !String.IsNullOrEmpty(x.BackgroundImagePath),
                y => y.BackgroundImagePath = null
                );
            SetReferenceMeshCommand = new UserCommand<FootprintEditorViewModel>(
                x => true,
                y =>
                    {
                        var d = new OpenFileDialog {CheckFileExists = true, Multiselect = false};
                        if (d.ShowDialog() == true)
                        {
                            ReferenceMesh = d.FileName;
                        }
                    }
                );
            ClearReferenceMeshCommand = new UserCommand<FootprintEditorViewModel>(
                x => x != null && !String.IsNullOrEmpty(x.ReferenceMesh),
                y => y.ReferenceMesh = null
                );
            SelectedFootprint = FootprintAreas.FirstOrDefault();
            if (SelectedArea == null)
            {
                SelectedSlot = SlotAreas.FirstOrDefault();
            }
        }


        private void AddSlotArea()
        {
            AddSlotArea(null);
        }

        private void AddSlotArea(FTPT.Area area)
        {
            area = NewArea(area);
            mFootprint.SlotAreas.Add(area);
            area = mFootprint.SlotAreas.Last();
            var vm = new AreaViewModel(this, area);
            mSlotAreas.Add(vm);
            SelectedArea = vm;
        }

        private void CopySlotArea()
        {
            AddSlotArea(SelectedArea.Area);
        }


        private bool CheckSlotArea()
        {
            return mSelectedArea != null && mSlotAreas.Contains(mSelectedArea);
        }


        private bool CheckFootprintArea()
        {
            return mSelectedArea != null && mFootprintAreas.Contains(mSelectedArea);
        }

        private void DeleteArea()
        {
            AreaViewModel area = SelectedArea;
            mFootprint.SlotAreas.Remove(area.Area);
            mFootprint.FootprintAreas.Remove(area.Area);
            mSlotAreas.Remove(area);
            mFootprintAreas.Remove(area);
        }

        private void AddFootprintArea()
        {
            AddFootprintArea(null);
        }

        private void AddFootprintArea(FTPT.Area area)
        {
            area = NewArea(area);
            mFootprint.FootprintAreas.Add(area);
            area = mFootprint.FootprintAreas.Last();
            var vm = new AreaViewModel(this, area);
            mFootprintAreas.Add(vm);
            SelectedArea = vm;
        }

        private void CopyFootprintArea()
        {
            AddFootprintArea(SelectedArea.Area);
        }

        private FTPT.Area NewArea(FTPT.Area basis)
        {
            FTPT.Area area = null;
            if (basis == null)
            {
                area = new FTPT.Area(0, null, mFootprint.Version);
            }
            else
            {
                area = new FTPT.Area(0, null, mFootprint.Version, basis.Name, basis.Priority, basis.AreaTypeFlags, basis.ClosedPolygon.Select(x => x.Clone(null)).Cast<FTPT.PolygonPoint>().ToList(), basis.AllowIntersectionFlags, basis.SurfaceTypeFlags, basis.SurfaceAttributeFlags, basis.LevelOffset, basis.Lower, basis.Upper);
            }
            return area;
        }
    }
}
