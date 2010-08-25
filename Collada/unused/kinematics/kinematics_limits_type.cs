using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class kinematics_limits_type : object, INotifyPropertyChanged
    {
        private common_float_or_param_type minField;

        private common_float_or_param_type maxField;

        /// <remarks />
        public common_float_or_param_type min
        {
            get { return this.minField; }
            set
            {
                this.minField = value;
                this.RaisePropertyChanged("min");
            }
        }

        /// <remarks />
        public common_float_or_param_type max
        {
            get { return this.maxField; }
            set
            {
                this.maxField = value;
                this.RaisePropertyChanged("max");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}