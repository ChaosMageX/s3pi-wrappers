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
    public class motion_type : object, INotifyPropertyChanged
    {
        private instance_articulated_system_type instance_articulated_systemField;

        private motion_technique_type technique_commonField;

        private technique_type[] techniqueField;

        private extra_type[] extraField;

        /// <remarks />
        public instance_articulated_system_type instance_articulated_system
        {
            get { return this.instance_articulated_systemField; }
            set
            {
                this.instance_articulated_systemField = value;
                this.RaisePropertyChanged("instance_articulated_system");
            }
        }

        /// <remarks />
        public motion_technique_type technique_common
        {
            get { return this.technique_commonField; }
            set
            {
                this.technique_commonField = value;
                this.RaisePropertyChanged("technique_common");
            }
        }

        /// <remarks />
        [XmlElement("technique")]
        public technique_type[] technique
        {
            get { return this.techniqueField; }
            set
            {
                this.techniqueField = value;
                this.RaisePropertyChanged("technique");
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