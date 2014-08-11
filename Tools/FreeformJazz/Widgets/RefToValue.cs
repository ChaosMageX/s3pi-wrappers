using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class RefToValue<T> where T : class, IHasHashedName
    {
        private StateMachineScene mScene;

        private string mArrayPropName;
        private System.Reflection.PropertyInfo mArrayProperty;

        private string mCountPropName;
        private System.Reflection.PropertyInfo mCountProperty;

        private T mValue;

        public RefToValue(StateMachineScene scene, 
            string arrayProperty, string countProperty, T value)
        {
            if (scene == null)
            {
                throw new ArgumentNullException("scene");
            }
            if (arrayProperty == null)
            {
                throw new ArgumentNullException("arrayProperty");
            }
            if (countProperty == null)
            {
                throw new ArgumentNullException("countProperty");
            }
            this.mScene = scene;

            this.mArrayPropName = arrayProperty;
            this.mArrayProperty 
                = typeof(StateMachine).GetProperty(arrayProperty);
            if (this.mArrayProperty == null)
            {
                throw new ArgumentException("StateMachine." + arrayProperty +
                    " does not exist or is inaccessible.", "arrayProperty");
            }

            this.mCountPropName = countProperty;
            this.mCountProperty
                = typeof(StateMachine).GetProperty(countProperty);
            if (this.mCountProperty == null)
            {
                throw new ArgumentException("StateMachine." + countProperty +
                    " does not exist or is inaccessible.", "countProperty");
            }

            this.mValue = value;
        }

        public T GetValue()
        {
            return this.mValue;
        }

        public void SetValue(T value)
        {
            this.mValue = value;
        }

        public class RefConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context,
                Type destinationType)
            {
                return typeof(string).Equals(destinationType) 
                    || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, 
                System.Globalization.CultureInfo culture, object value, 
                Type destinationType)
            {
                RefToValue<T> rtv = value as RefToValue<T>;
                if (rtv == null || !typeof(string).Equals(destinationType))
                {
                    return base.ConvertTo(context, culture, value, 
                        destinationType);
                }
                if (rtv.mValue == null)
                {
                    return "";
                }
                StateMachine sm = rtv.mScene.StateMachine;
                int i = (int)rtv.mCountProperty.GetValue(sm, null);
                if (i == 0)
                {
                    return "[?] " + rtv.mValue.Name;
                }
                T[] values = rtv.mArrayProperty.GetValue(sm, null) as T[];
                for (i = values.Length - 1; i >= 0; i--)
                {
                    if (rtv.mValue == values[i])
                    {
                        break;
                    }
                }
                string result;
                int j = values.Length.ToString("X").Length;
                if (i < 0)
                {
                    result = string.Concat("[", new string('?', j), "] ");
                }
                else
                {
                    result = string.Concat("[", i.ToString("X" + j), "] ");
                }
                return result + rtv.mValue.Name;
            }
        }

        public class RefEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(
                ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override bool IsDropDownResizable
            {
                get { return true; }
            }

            public override object EditValue(ITypeDescriptorContext context,
                IServiceProvider provider, object value)
            {
                if (provider == null)
                {
                    return value;
                }
                RefToValue<T> rtv = value as RefToValue<T>;
                IWindowsFormsEditorService edSvc
                    = (IWindowsFormsEditorService)provider.GetService(
                        typeof(IWindowsFormsEditorService));
                if (rtv == null || edSvc == null)
                {
                    return value;
                }
                StateMachine sm = rtv.mScene.StateMachine;
                int index = (int)rtv.mCountProperty.GetValue(sm, null);
                string[] items = new string[index + 1];
                items[0] = "";
                T[] values = rtv.mArrayProperty.GetValue(sm, null) as T[];
                if (index > 0)
                {
                    int i = index.ToString("X").Length;
                    string fmt = "[{0:X" + i + "}] ";
                    if (rtv.mValue == null)
                    {
                        for (i = index - 1; i >= 0; i--)
                        {
                            items[i + 1] = string.Format(fmt, i)
                                         + values[i].Name;
                        }
                        index = 0;
                    }
                    else
                    {
                        index = -1;
                        T val;
                        for (i = values.Length - 1; i >= 0; i--)
                        {
                            val = values[i];
                            if (rtv.mValue == val)
                            {
                                index = i + 1;
                            }
                            items[i + 1] = string.Format(fmt, i) + val.Name;
                        }
                    }
                }
                else
                {
                    index = rtv.mValue == null ? 0 : -1;
                }

                ListBox lb = new ListBox();
                TextBox tb = new TextBox();
                
                tb.AutoSize = true;
                tb.Font = lb.Font;
                tb.Margin = new Padding(0);
                tb.Multiline = true;
                tb.Lines = items;
                tb.PerformLayout();
                
                lb.IntegralHeight = false;
                lb.Margin = new Padding(0);
                lb.Size = tb.PreferredSize;
                lb.Tag = edSvc;
                lb.Items.AddRange(items);
                lb.PerformLayout();

                if (index >= 0)
                {
                    lb.SelectedIndex = index;
                }
                lb.SelectedIndexChanged += 
                    new EventHandler(lb_SelectedIndexChanged);
                edSvc.DropDownControl(lb);

                index = lb.SelectedIndex;
                if (index == 0)
                {
                    rtv.mValue = null;
                }
                else if (index > 0)
                {
                    rtv.mValue = values[index];
                }
                return value;
            }

            private static void lb_SelectedIndexChanged(
                object sender, EventArgs e)
            {
                ListBox lb = sender as ListBox;
                if (lb != null)
                {
                    lb.SelectedIndexChanged -= 
                        new EventHandler(lb_SelectedIndexChanged);
                    IWindowsFormsEditorService edSvc 
                        = lb.Tag as IWindowsFormsEditorService;
                    if (edSvc != null)
                    {
                        edSvc.CloseDropDown();
                    }
                }
            }
        }
    }
}
