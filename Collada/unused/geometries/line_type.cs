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
    public class line_type : object, INotifyPropertyChanged
    {
        private string originField;

        private string directionField;

        private extra_type[] extraField;

        /// <remarks />
        public string origin
        {
            get { return this.originField; }
            set
            {
                this.originField = value;
                this.RaisePropertyChanged("origin");
            }
        }

        /// <remarks />
        public string direction
        {
            get { return this.directionField; }
            set
            {
                this.directionField = value;
                this.RaisePropertyChanged("direction");
            }
        }

        /// <remarks />
        [XmlElement("extra")]
        public extra_type[] extra
        {
            get { return this.extraField; }
            set
            {
                this.extraField = value;
                this.RaisePropertyChanged("extra");
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