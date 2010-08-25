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
    public class cone_type : object, INotifyPropertyChanged
    {
        private double radiusField;

        private float angleField;

        private extra_type[] extraField;

        /// <remarks />
        public double radius
        {
            get { return this.radiusField; }
            set
            {
                this.radiusField = value;
                this.RaisePropertyChanged("radius");
            }
        }

        /// <remarks />
        public float angle
        {
            get { return this.angleField; }
            set
            {
                this.angleField = value;
                this.RaisePropertyChanged("angle");
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