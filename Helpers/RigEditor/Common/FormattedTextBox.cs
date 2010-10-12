using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace s3piwrappers.RigEditor.Common
{
    internal abstract class FormattedTextBox<T> : TextBox
    {
        private T mValue;
        private Color mErrorColour = Color.FromArgb(255, 200, 200);
        private Color mDefaultColour = Color.FromArgb(255, 255, 255);
        private bool mValid;

        public FormattedTextBox()
        {
            Text = FormatValue(mValue);
            TextAlign = HorizontalAlignment.Right;
        }
        [Browsable(false)]
        public virtual T Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                Text = FormatValue(value);
            }
        }
        [Browsable(true)]
        [Category("Design")]
        public Color ErrorColour
        {
            get { return mErrorColour; }
            set { mErrorColour = value; }
        }


        [Browsable(true)]
        [Category("Design")]
        public Color DefaultColour
        {
            get { return mDefaultColour; }
            set { mDefaultColour = value; }
        }

        [Browsable(false)]
        public bool Valid
        {
            get { return mValid; }
        }

        protected abstract bool Validate(string s);
        protected abstract T Parse(string s);
        protected virtual string FormatValue(T val)
        {
            return val.ToString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            if (!Validate(Text))
            {
                BackColor = ErrorColour;
                mValid = false;
                e.Cancel = true;
            }
            base.OnValidating(e);
        }
        protected override void OnValidated(EventArgs e)
        {
            BackColor = DefaultColour;
            mValid = true;
            mValue = Parse(Text);
            Text = FormatValue(mValue);
            base.OnValidated(e);
        }
    }
}