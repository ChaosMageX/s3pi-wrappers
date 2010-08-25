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
    public class kinematics_type : object, INotifyPropertyChanged
    {
        private instance_kinematics_model_type[] instance_kinematics_modelField;

        private kinematics_technique_type technique_commonField;

        private technique_type[] techniqueField;

        private extra_type[] extraField;

        /// <remarks />
        [XmlElement("instance_kinematics_model")]
        public instance_kinematics_model_type[] instance_kinematics_model
        {
            get { return this.instance_kinematics_modelField; }
            set
            {
                this.instance_kinematics_modelField = value;
                this.RaisePropertyChanged("instance_kinematics_model");
            }
        }

        /// <remarks />
        public kinematics_technique_type technique_common
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