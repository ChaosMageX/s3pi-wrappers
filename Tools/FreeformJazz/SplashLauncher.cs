using System;
using System.Windows.Forms;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.FreeformJazz
{
    public class SplashLauncher : IDisposable
    {
        private bool bStarted;
        private Splash mSplash;

        public SplashLauncher()
        {
            this.bStarted = false;
            this.mSplash = new Splash("Starting " + MainForm.kName);
        }

        private void largeStatusMessage(string msg)
        {
            this.mSplash.LargeMessage = msg;
            Application.DoEvents();
        }

        private void smallStatusMessage(string msg)
        {
            this.mSplash.SmallMessage = msg;
            Application.DoEvents();
        }

        public void Start()
        {
            if (!this.bStarted && this.mSplash != null)
            {
                this.mSplash.Show();
                Application.DoEvents();
                GlobalManager.StatusMessage +=
                    new Action<string>(this.largeStatusMessage);
                KeyNameMap.StatusMessage +=
                    new Action<string>(this.smallStatusMessage);
                ResourceMgr.StatusMessage +=
                    new Action<string>(this.smallStatusMessage);
                this.bStarted = true;
            }
        }

        public void End()
        {
            if (this.bStarted && this.mSplash != null)
            {
                this.mSplash.Close();
                Application.DoEvents();
                GlobalManager.StatusMessage -=
                    new Action<string>(this.largeStatusMessage);
                KeyNameMap.StatusMessage -=
                    new Action<string>(this.smallStatusMessage);
                ResourceMgr.StatusMessage -=
                    new Action<string>(this.smallStatusMessage);
                this.bStarted = false;
            }
        }

        public void Dispose()
        {
            if (this.mSplash != null)
            {
                if (this.bStarted)
                {
                    this.End();
                }
                this.mSplash.Dispose();
                this.mSplash = null;
            }
        }

        public static void Launch()
        {
            using (SplashLauncher sl = new SplashLauncher())
            {
                sl.Start();
                GlobalManager.Load();
                sl.End();
            }
        }
    }
}
