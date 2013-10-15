using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace s3piwrappers.CustomForms.Design
{
    public class ByteSliderEditor : UITypeEditor
    {
        private class ByteSliderUI : Control
        {
            private TrackBar mSlider;
            private TextBox mText;
            private Button mOKBtn;

            private IWindowsFormsEditorService mEdSvc;
            private object mValue;

            public ByteSliderUI()
            {
                this.InitializeComponent();
            }

            public object Value
            {
                get { return this.mValue; }
            }

            private void InitializeComponent()
            {
                this.mSlider = new TrackBar();
                this.mText = new TextBox();
                this.mOKBtn = new Button();
                base.SetBounds(0, 0, 90, 90);
                
                this.mSlider.Location = new Point(0, 0);
                this.mSlider.Size = new Size(90, 45);
                this.mSlider.Anchor 
                    = AnchorStyles.Right | AnchorStyles.Left 
                    | AnchorStyles.Bottom | AnchorStyles.Top;
                this.mSlider.TabIndex = 0;
                this.mSlider.TabStop = true;
                this.mSlider.Minimum = 0;
                this.mSlider.Maximum = 255;
                this.mSlider.TickFrequency = 16;
                this.mSlider.SmallChange = 1;
                this.mSlider.LargeChange = 16;
                this.mSlider.ValueChanged += 
                    new EventHandler(sliderValueChanged);

                this.mText.Location = new Point(0, 45);
                this.mText.Size = new Size(90, 20);
                this.mText.Anchor 
                    = AnchorStyles.Right | AnchorStyles.Left
                    | AnchorStyles.Bottom | AnchorStyles.Top;
                this.mText.TabIndex = 1;
                this.mText.TabStop = false;
                this.mText.ReadOnly = true;
                this.mText.TextAlign = HorizontalAlignment.Center;

                this.mOKBtn.Location = new Point(0, 65);
                this.mOKBtn.Size = new Size(90, 25);
                this.mOKBtn.Anchor 
                    = AnchorStyles.Right | AnchorStyles.Left
                    | AnchorStyles.Bottom | AnchorStyles.Top;
                this.mOKBtn.TabIndex = 2;
                this.mOKBtn.TabStop = true;
                this.mOKBtn.Text = "OK";
                this.mOKBtn.Click += 
                    new EventHandler(okButtonClick);

                base.Controls.Clear();
                base.Controls.AddRange(
                    new Control[] { this.mSlider, this.mText, this.mOKBtn });
            }

            private void sliderValueChanged(object sender, EventArgs e)
            {
                this.mValue = (byte)this.mSlider.Value;
                this.mText.Text = this.mSlider.Value.ToString();
            }

            private void okButtonClick(object sender, EventArgs e)
            {
                if (this.mEdSvc != null)
                {
                    this.mEdSvc.CloseDropDown();
                }
            }

            public void Start(IWindowsFormsEditorService edSvc, object value)
            {
                this.mEdSvc = edSvc;
                this.mValue = value;
                byte bValue = (byte)value;
                this.mSlider.Value = bValue;
                this.mText.Text = bValue.ToString();
            }

            public void End()
            {
                this.mEdSvc = null;
                this.mValue = null;
            }
        }

        private ByteSliderUI mByteSliderUI;

        public override UITypeEditorEditStyle GetEditStyle(
            System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(
            System.ComponentModel.ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc 
                    = (IWindowsFormsEditorService)provider.GetService(
                        typeof(IWindowsFormsEditorService));
                if (edSvc == null)
                {
                    return value;
                }
                if (this.mByteSliderUI == null)
                {
                    this.mByteSliderUI = new ByteSliderUI();
                }
                this.mByteSliderUI.Start(edSvc, value);
                edSvc.DropDownControl(this.mByteSliderUI);
                value = this.mByteSliderUI.Value;
                this.mByteSliderUI.End();
            }
            return value;
        }
    }
}
