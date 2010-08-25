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
    public class motion_technique_type : object, INotifyPropertyChanged
    {
        private motion_axis_info_type[] axis_infoField;

        private motion_effector_info_type effector_infoField;

        /// <remarks />
        [XmlElement("axis_info")]
        public motion_axis_info_type[] axis_info
        {
            get { return this.axis_infoField; }
            set
            {
                this.axis_infoField = value;
                this.RaisePropertyChanged("axis_info");
            }
        }

        /// <remarks />
        public motion_effector_info_type effector_info
        {
            get { return this.effector_infoField; }
            set
            {
                this.effector_infoField = value;
                this.RaisePropertyChanged("effector_info");
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