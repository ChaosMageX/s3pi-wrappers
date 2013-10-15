using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace s3piwrappers.CustomForms
{
    public partial class SingleStringPrompt : Form
    {
        private bool bSizeChangedByCode = false;

        public SingleStringPrompt()
        {
            InitializeComponent();
            this.responseTXT.Text = "";
        }

        public SingleStringPrompt(string prompt, string title, 
            string defaultResponse = "")
            : this()
        {
            this.Prompt = prompt;
            this.Text = title;
            this.responseTXT.Text = defaultResponse ?? "";
            this.okBTN.Enabled = !string.IsNullOrEmpty(defaultResponse);
        }

        public string Prompt
        {
            get { return this.promptLBL.Text; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (!this.promptLBL.Text.Equals(value))
                {
                    Graphics g = this.promptLBL.CreateGraphics();
                    SizeF size = g.MeasureString(value, 
                        this.promptLBL.Font, this.promptLBL.Width);
                    int h = (int)Math.Ceiling(size.Height);
                    if (h != this.promptLBL.Height)
                    {
                        h = h - this.promptLBL.Height;
                        this.promptLBL.Height += h;
                        this.responseTXT.Top += h;
                        this.bSizeChangedByCode = true;
                        this.Height += h;
                    }
                }
            }
        }

        public string Response
        {
            get { return this.responseTXT.Text; }
            set 
            {
                if (value == null)
                {
                    value = "";
                }
                if (!this.responseTXT.Text.Equals(value))
                {
                    this.responseTXT.Text = value;
                    this.okBTN.Enabled = !string.IsNullOrEmpty(value);
                }
            }
        }

        private const AnchorStyles kResponseAnchor 
            = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        public bool MultilineResponse
        {
            get { return this.responseTXT.Multiline; }
            set
            {
                if (this.responseTXT.Multiline != value)
                {
                    if (value)
                    {
                        this.responseTXT.Anchor
                            = kResponseAnchor | AnchorStyles.Bottom;
                        this.FormBorderStyle
                            = FormBorderStyle.SizableToolWindow;
                    }
                    else
                    {
                        this.responseTXT.Anchor = kResponseAnchor;
                        this.FormBorderStyle
                            = FormBorderStyle.FixedToolWindow;
                        int h = 20 - this.responseTXT.Height;
                        this.responseTXT.Height = 20;
                        this.bSizeChangedByCode = true;
                        this.Height += h;
                    }
                    this.responseTXT.Multiline = value;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.bSizeChangedByCode)
            {
                this.bSizeChangedByCode = false;
            }
            else
            {
                Graphics g = this.promptLBL.CreateGraphics();
                SizeF size = g.MeasureString(this.promptLBL.Text,
                    this.promptLBL.Font, this.promptLBL.Width);
                int h = Math.Min((int)Math.Ceiling(size.Height), 20);
                if (h != this.promptLBL.Height)
                {
                    h = h - this.promptLBL.Height;
                    this.promptLBL.Height += h;
                    this.responseTXT.Top += h;
                    if (this.responseTXT.Multiline)
                    {
                        this.responseTXT.Height -= h;
                    }
                    else
                    {
                        this.bSizeChangedByCode = true;
                        this.Height += h;
                    }
                }
            }
        }

        public bool OKEnabled
        {
            get { return this.okBTN.Enabled; }
            set { this.okBTN.Enabled = value; }
        }

        public void SetStartLocation(int xPos, int yPos)
        {
            if (xPos == -1 && yPos == -1)
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }
            else
            {
                if (xPos == -1)
                {
                    xPos = 600;
                }
                if (yPos == -1)
                {
                    yPos = 350;
                }
                this.StartPosition = FormStartPosition.Manual;
                Point point = new Point(xPos, yPos);
                this.DesktopLocation = point;
            }
        }

        public event EventHandler ResponseTextChanged;

        private void responseTextChanged(object sender, EventArgs e)
        {
            this.okBTN.Enabled 
                = !string.IsNullOrEmpty(this.responseTXT.Text);
            if (this.ResponseTextChanged != null)
            {
                this.ResponseTextChanged(this, e);
            }
        }
    }
}
