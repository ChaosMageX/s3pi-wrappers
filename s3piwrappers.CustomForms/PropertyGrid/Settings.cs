using System;
using System.Drawing;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    /// <summary>
    /// Contains values utilized by various <see cref="System.Windows.Forms.Control"/> classes
    /// within the <see cref="s3piwrappers.CustomForms.PropertyGrid"/> namespace.
    /// </summary>
    /// <remarks>
    /// This is a substitute for the values and functionality contained within the MainForm of
    /// S3PE and its application settings, so that Property Grid components carried over from
    /// S3PE are capable of functioning outside of it and can be used in other applications.
    /// </remarks>
    public class Settings
    {
        private static Settings sDefault = new Settings();

        /// <summary>
        /// Singleton instance of <see cref="Settings"/> that is used by all created
        /// instances of various <see cref="System.Windows.Forms.Control"/> classes
        /// within the <see cref="s3piwrappers.CustomForms.PropertyGrid"/> namespace.
        /// </summary>
        public static Settings Default
        {
            get { return sDefault; }
        }

        private Icon mFormIcon;
        private Size mGridSize;

        private string mTextEditorCmd;
        private bool mTextEditorWantsQuotes;
        private bool mTextEditorIgnoreTS;

        private string mHexEditorCmd;
        private bool mHexEditorWantsQuotes;
        private bool mHexEditorIgnoreTS;

        /// <summary>
        /// Event triggered whenever a property of this 
        /// <see cref="Settings"/> instance is changed,
        /// sending the name of the property and the new value.
        /// </summary>
        public event Action<string, object> SettingChanged;
        /// <summary>
        /// Event triggered when <see cref="IssueException(Exception, string)"/>
        /// is invoked, which is used as a way of handling caught exceptions.
        /// </summary>
        public event Action<Exception, string> ExceptionIssued;

        /// <summary>
        /// Creates a new <see cref="Settings"/> instance with
        /// all of its properties set to their default values.
        /// </summary>
        public Settings()
        {
            this.mFormIcon = (new System.Windows.Forms.Form()).Icon;
            this.mGridSize = new Size(-1, -1);

            this.mTextEditorCmd = "";
            this.mTextEditorWantsQuotes = false;
            this.mTextEditorIgnoreTS = false;

            this.mHexEditorCmd = "";
            this.mHexEditorWantsQuotes = false;
            this.mHexEditorIgnoreTS = false;
        }

        public Icon FormIcon
        {
            get { return this.mFormIcon; }
            set
            {
                if (this.mFormIcon != value)
                {
                    this.mFormIcon = value;
                    this.SettingChanged?.Invoke("FormIcon", value);
                }
            }
        }

        public Size GridSize
        {
            get { return this.mGridSize; }
            set
            {
                if (this.mGridSize != value)
                {
                    this.mGridSize = value;
                    this.SettingChanged?.Invoke("GridSize", value);
                }
            }
        }

        public string TextEditorCmd
        {
            get { return this.mTextEditorCmd; }
            set
            {
                if (this.mTextEditorCmd != value)
                {
                    this.mTextEditorCmd = value;
                    this.SettingChanged?.Invoke("TextEditorCmd", value);
                }
            }
        }

        public bool TextEditorWantsQuotes
        {
            get { return this.mTextEditorWantsQuotes; }
            set
            {
                if (this.mTextEditorWantsQuotes != value)
                {
                    this.mTextEditorWantsQuotes = value;
                    this.SettingChanged?.Invoke("TextEditorWantsQuotes", value);
                }
            }
        }

        public bool TextEditorIgnoreTS
        {
            get { return this.mTextEditorIgnoreTS; }
            set
            {
                if (this.mTextEditorIgnoreTS != value)
                {
                    this.mTextEditorIgnoreTS = value;
                    this.SettingChanged?.Invoke("TextEditorIgnoreTS", value);
                }
            }
        }

        public string HexEditorCmd
        {
            get { return this.mHexEditorCmd; }
            set
            {
                if (this.mHexEditorCmd != value)
                {
                    this.mHexEditorCmd = value;
                    this.SettingChanged?.Invoke("HexEditorCmd", value);
                }
            }
        }

        public bool HexEditorWantsQuotes
        {
            get { return this.mHexEditorWantsQuotes; }
            set
            {
                if (this.mHexEditorWantsQuotes != value)
                {
                    this.mHexEditorWantsQuotes = value;
                    this.SettingChanged?.Invoke("HexEditorWantsQuotes", value);
                }
            }
        }

        public bool HexEditorIgnoreTS
        {
            get { return this.mHexEditorIgnoreTS; }
            set
            {
                if (this.mHexEditorIgnoreTS != value)
                {
                    this.mHexEditorIgnoreTS = value;
                    this.SettingChanged?.Invoke("HexEditorIgnoreTS", value);
                }
            }
        }

        /// <summary>
        /// Triggers the <see cref="ExceptionIssued"/> event so that 
        /// applications listening to it have a better way of handling
        /// exceptions created and caught by instances of various
        /// <see cref="System.Windows.Forms.Control"/> classes within the
        /// <see cref="s3piwrappers.CustomForms.PropertyGrid"/> namespace.
        /// </summary>
        /// <param name="ex">The caught <see cref="Exception"/> instance.</param>
        /// <param name="prefix">Additional details about the caught exception, 
        /// which are typically printed before <paramref name="ex"/> details
        /// are printed.</param>
        public void IssueException(Exception ex, string prefix)
        {
            this.ExceptionIssued?.Invoke(ex, prefix);
        }
    }
}
