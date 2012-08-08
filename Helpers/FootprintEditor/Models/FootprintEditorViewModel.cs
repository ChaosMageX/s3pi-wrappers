﻿using System.Linq;
using s3pi.GenericRCOLResource;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using s3piwrappers.Commands;
using System.Text;
using System;
using Microsoft.Win32;

namespace s3piwrappers.Models
{
    public class FootprintEditorViewModel : AbstractViewModel
    {
        private GenericRCOLResource mRcol;
        private FTPT mFootprint;
        private ObservableCollection<AreaViewModel> mFootprintAreas;
        private ObservableCollection<AreaViewModel> mSlotAreas;
        private bool mIsSaving;
        private AreaViewModel mSelectedArea;

        public int MinZoom
        {
            get { return 5; }
        }
        public int MaxZoom
        {
            get { return 10000; }
        }
        private int mZoom;
        public int Zoom
        {
            get { return mZoom; }
            set { mZoom = value; OnPropertyChanged("Zoom"); OnPropertyChanged("StatusText"); }
        }

        private double? mCursorX;
        public double? CursorX
        {
            get { return mCursorX; }
            set { mCursorX = value; OnPropertyChanged("CursorX"); OnPropertyChanged("StatusText"); }
        }

        private double? mCursorZ;
        public double? CursorZ
        {
            get { return mCursorZ; }
            set
            {
                mCursorZ = value; OnPropertyChanged("CursorZ");
                OnPropertyChanged("StatusText");
            }
        }

        private string mBackgroundImagePath;
        public string BackgroundImagePath
        {
            get { return mBackgroundImagePath; }
            set { mBackgroundImagePath = value; OnPropertyChanged("BackgroundImagePath"); }
        }

        public string StatusText
        {
            get
            {
                var sb = new StringBuilder();
                if (CursorX != null && CursorZ != null)
                {
                    sb.AppendFormat("({0},{1}) %{2}", CursorX, CursorZ,Zoom);
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
            set { mReferenceMesh = value; OnPropertyChanged("ReferenceMesh"); }
        }

        public ObservableCollection<AreaViewModel> SlotAreas
        {
            get { return mSlotAreas; }
        }

        public AreaViewModel SelectedArea
        {
            get { return mSelectedArea; }
            set { mSelectedArea = value; OnPropertyChanged("SelectedArea"); OnPropertyChanged("SelectedArea"); }
        }

        public ObservableCollection<AreaViewModel> FootprintAreas
        {
            get { return mFootprintAreas; }
        }

        public bool IsSaving { get { return mIsSaving; } }


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
            this.mRcol = rcol;
            this.Zoom = 100;
            this.mFootprint = rcol.ChunkEntries[0].RCOLBlock as FTPT;
            this.mFootprintAreas = new ObservableCollection<AreaViewModel>(mFootprint.FootprintAreas.Select(x => new AreaViewModel(this, x)));
            this.mSlotAreas = new ObservableCollection<AreaViewModel>(mFootprint.SlotAreas.Select(x => new AreaViewModel(this, x)));

            this.AddSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null, x => x.AddSlotArea());
            this.CloneSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckSlotArea(), x => x.CopySlotArea());
            this.DeleteSlotCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckSlotArea(), x => x.DeleteArea());



            this.AddFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null, x => x.AddFootprintArea());
            this.CloneFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckFootprintArea(), x => x.CopyFootprintArea());
            this.DeleteFootprintCommand = new UserCommand<FootprintEditorViewModel>(x => x != null && x.CheckFootprintArea(), x => x.DeleteArea());


            this.CommitCommand = new UserCommand<FootprintEditorViewModel>(x => true, y => { mIsSaving = true; Application.Current.Shutdown(); });
            this.CancelCommand = new UserCommand<FootprintEditorViewModel>(x => true, y => { mIsSaving = false; Application.Current.Shutdown(); });


            this.SetBackgroundImageCommand = new UserCommand<FootprintEditorViewModel>(
                x => true,
                y =>
                {
                    var d = new OpenFileDialog { CheckFileExists = true, Multiselect = false };
                    if (d.ShowDialog() == true)
                    {
                        this.BackgroundImagePath = d.FileName;
                    }

                }
                );
            this.ClearBackgroundImageCommand = new UserCommand<FootprintEditorViewModel>(
                x => x != null && !String.IsNullOrEmpty(x.BackgroundImagePath),
                y => y.BackgroundImagePath = null
            );
            this.SetReferenceMeshCommand = new UserCommand<FootprintEditorViewModel>(
                x => true,
                y =>
                {
                    var d = new OpenFileDialog { CheckFileExists = true, Multiselect = false };
                    if (d.ShowDialog() == true)
                    {
                        this.ReferenceMesh = d.FileName;
                    }

                }
                );
            this.ClearReferenceMeshCommand = new UserCommand<FootprintEditorViewModel>(
                x => x != null && !String.IsNullOrEmpty(x.ReferenceMesh),
                y => y.ReferenceMesh = null
            );
            this.SelectedFootprint = this.FootprintAreas.FirstOrDefault();
            if (this.SelectedArea == null)
            {
                this.SelectedSlot = this.SlotAreas.FirstOrDefault();
            }
        }


        private void AddSlotArea()
        {
            this.AddSlotArea(null);
        }
        private void AddSlotArea(FTPT.Area area)
        {
            area = this.NewArea(area);
            this.mFootprint.SlotAreas.Add(area);
            area = this.mFootprint.SlotAreas.Last();
            var vm = new AreaViewModel(this, area);
            this.mSlotAreas.Add(vm);
            this.SelectedArea = vm;


        }

        private void CopySlotArea()
        {
            this.AddSlotArea(this.SelectedArea.Area);
        }


        private bool CheckSlotArea()
        {
            return this.mSelectedArea != null && this.mSlotAreas.Contains(this.mSelectedArea);
        }


        private bool CheckFootprintArea()
        {
            return this.mSelectedArea != null && this.mFootprintAreas.Contains(this.mSelectedArea);
        }
        private void DeleteArea()
        {
            var area = this.SelectedArea;
            this.mFootprint.SlotAreas.Remove(area.Area);
            this.mFootprint.FootprintAreas.Remove(area.Area);
            this.mSlotAreas.Remove(area);
            this.mFootprintAreas.Remove(area);
        }

        private void AddFootprintArea()
        {
            this.AddFootprintArea(null);
        }
        private void AddFootprintArea(FTPT.Area area)
        {
            area = this.NewArea(area);
            this.mFootprint.FootprintAreas.Add(area);
            area = this.mFootprint.FootprintAreas.Last();
            var vm = new AreaViewModel(this, area);
            this.mFootprintAreas.Add(vm);
            this.SelectedArea = vm;


        }
        private void CopyFootprintArea()
        {
            this.AddFootprintArea(this.SelectedArea.Area);
        }
        private FTPT.Area NewArea(FTPT.Area basis)
        {
            FTPT.Area area = null;
            if (basis == null)
            {
                area = new FTPT.Area(0, null, this.mFootprint.Version);
            }
            else
            {
                area = new FTPT.Area(0, null, this.mFootprint.Version, basis.Name, basis.Priority, basis.AreaTypeFlags, basis.ClosedPolygon.Select(x => x.Clone(null)).Cast<FTPT.PolygonPoint>().ToList(), basis.AllowIntersectionFlags, basis.SurfaceTypeFlags, basis.SurfaceAttributeFlags, basis.LevelOffset, basis.Lower, basis.Upper);
            }
            return area;

        }
    }
}
