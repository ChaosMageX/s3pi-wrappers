using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using s3pi.GenericRCOLResource;
using s3piwrappers.AnimatedTextureEditor.Commands;

namespace s3piwrappers.AnimatedTextureEditor.ViewModels
{
    internal class AnimViewModel : AbstractViewModel
    {
        public class FrameViewModel : AbstractViewModel
        {
            private readonly ANIM.TextureFrame mFrame;
            private readonly AnimViewModel mParent;

            public FrameViewModel(AnimViewModel parent, ANIM.TextureFrame frame)
            {
                mParent = parent;
                mFrame = frame;
            }

            public ANIM.TextureFrame Frame
            {
                get { return mFrame; }
            }

            public ImageSource Image
            {
                get
                {
                    var s = new MemoryStream(mFrame.AsBytes);
                    if (s.Length > 0)
                    {
                        var dds = new DdsFile();
                        dds.Load(s, true);
                        Bitmap img = dds.Image;
                        var ms = new MemoryStream();
                        img.Save(ms, ImageFormat.Png);
                        var bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.StreamSource = new MemoryStream(ms.ToArray());
                        bmp.EndInit();
                        return bmp;
                    }
                    return null;
                }
            }

            public Byte[] Data
            {
                get { return mFrame.Data; }
                set
                {
                    mFrame.Data = value;
                    OnPropertyChanged("Image");
                }
            }

            public override string ToString()
            {
                return String.Format("Frame[{0}]", mParent.Frames.IndexOf(this));
            }
        }

        private readonly ANIM mANIM;
        private ObservableCollection<FrameViewModel> mFrames;

        public AnimViewModel(GenericRCOLResource rcolResource)
        {
            IsSaving = false;
            mANIM = (ANIM) rcolResource.ChunkEntries.FirstOrDefault().RCOLBlock;
            CurrentFrameIndex = -1;
            SyncFrames();
            CommitCommand = new UserCommand<AnimViewModel>(x => true, y =>
                {
                    IsSaving = true;
                    Application.Current.Shutdown();
                });
            CancelCommand = new UserCommand<AnimViewModel>(x => true, y =>
                {
                    IsSaving = false;
                    Application.Current.Shutdown();
                });
            RemoveFrameCommand = new UserCommand<AnimViewModel>(x => CurrentFrameIndex >= 0, RemoveFrame);
            AddFrameCommand = new UserCommand<AnimViewModel>(x => true, AddFrame);
            ShiftFrameUpCommand = new UserCommand<AnimViewModel>(x => x != null && x.CurrentFrameIndex > 0, ShiftFrameUp);
            ShiftFrameDownCommand = new UserCommand<AnimViewModel>(x => x != null && x.CurrentFrameIndex < x.Frames.Count - 1 && x.CurrentFrameIndex != -1, ShiftFrameDown);
            ImportCommand = new UserCommand<AnimViewModel>(x => x != null && x.GetSelectedFrame() != null, ImportDds);
            ExportCommand = new UserCommand<AnimViewModel>(x => x != null && x.GetSelectedFrame() != null && x.GetSelectedFrame().Frame.Stream.Length > 0, ExportDds);
        }

        public void SyncFrames()
        {
            int prev = CurrentFrameIndex;
            mFrames = new ObservableCollection<FrameViewModel>();
            foreach (ANIM.TextureFrame f in mANIM.Frames) mFrames.Add(new FrameViewModel(this, f));
            if (mFrames.Count > 0 && prev == -1) prev = 0;
            OnPropertyChanged("Frames");
            CurrentFrameIndex = prev;
        }

        public Boolean IsSaving { get; private set; }

        public ICommand CommitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ICommand AddFrameCommand { get; private set; }
        public ICommand RemoveFrameCommand { get; private set; }
        public ICommand ShiftFrameUpCommand { get; private set; }
        public ICommand ShiftFrameDownCommand { get; private set; }

        public ICommand ImportCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }

        public float FrameRate
        {
            get { return mANIM.Framerate; }
            set
            {
                mANIM.Framerate = value;
                OnPropertyChanged("FrameRate");
            }
        }

        public UInt32 Version
        {
            get { return mANIM.Version; }
            set
            {
                mANIM.Version = value;
                OnPropertyChanged("Version");
            }
        }

        public IList<FrameViewModel> Frames
        {
            get { return mFrames; }
        }

        private int mCurrentFrameIndex;

        public Int32 CurrentFrameIndex
        {
            get { return mCurrentFrameIndex; }
            set
            {
                mCurrentFrameIndex = value;
                OnPropertyChanged("CurrentFrameIndex");
            }
        }

        private FrameViewModel GetSelectedFrame()
        {
            if (CurrentFrameIndex >= mFrames.Count)
            {
                CurrentFrameIndex = -1;
            }
            return CurrentFrameIndex == -1 ? null : Frames[CurrentFrameIndex];
        }

        private static void RemoveFrame(AnimViewModel view)
        {
            view.mANIM.Frames.Remove(view.GetSelectedFrame().Frame);
            view.SyncFrames();
        }

        private static void AddFrame(AnimViewModel view)
        {
            view.mANIM.Frames.Add();
            view.SyncFrames();
        }

        private static void ShiftFrameUp(AnimViewModel view)
        {
            ANIM.TextureList frames = view.mANIM.Frames;
            ANIM.TextureFrame frame = view.GetSelectedFrame().Frame;
            int index = frames.IndexOf(frame);
            ANIM.TextureFrame previous = frames[index - 1];
            frames[index - 1] = frame;
            frames[index] = previous;
            view.SyncFrames();
            view.CurrentFrameIndex -= 1;
        }

        private static void ShiftFrameDown(AnimViewModel view)
        {
            ANIM.TextureList frames = view.mANIM.Frames;
            ANIM.TextureFrame frame = view.GetSelectedFrame().Frame;
            int index = frames.IndexOf(frame);
            ANIM.TextureFrame next = frames[index + 1];
            frames[index + 1] = frame;
            frames[index] = next;
            view.SyncFrames();
            view.CurrentFrameIndex += 1;
        }

        private static void ImportDds(AnimViewModel parent)
        {
            FrameViewModel view = parent.GetSelectedFrame();
            var dialog = new OpenFileDialog {AddExtension = true, CheckFileExists = true, CheckPathExists = true, DefaultExt = ".dds", Filter = "DDS File(*.dds)|*.dds"};
            if ((bool) dialog.ShowDialog())
            {
                using (FileStream f = File.OpenRead(dialog.FileName))
                {
                    var buffer = new byte[f.Length];
                    f.Read(buffer, 0, buffer.Length);
                    view.Data = buffer;
                }
            }
        }

        private static void ExportDds(AnimViewModel parent)
        {
            FrameViewModel view = parent.GetSelectedFrame();
            var dialog = new SaveFileDialog {AddExtension = true, CheckPathExists = true, DefaultExt = ".dds", Filter = "DDS File(*.dds)|*.dds"};
            if ((bool) dialog.ShowDialog())
            {
                using (FileStream f = File.Create(dialog.FileName))
                {
                    byte[] buffer = view.Frame.AsBytes;
                    f.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
