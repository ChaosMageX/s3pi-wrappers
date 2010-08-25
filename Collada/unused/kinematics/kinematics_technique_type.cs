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
    public class kinematics_technique_type : object, INotifyPropertyChanged
    {
        private kinematics_axis_info_type[] axis_infoField;

        private kinematics_frame_type frame_originField;

        private kinematics_frame_type frame_tipField;

        private kinematics_frame_type frame_tcpField;

        private kinematics_frame_type frame_objectField;

        /// <remarks />
        [XmlElement("axis_info")]
        public kinematics_axis_info_type[] axis_info
        {
            get { return this.axis_infoField; }
            set
            {
                this.axis_infoField = value;
                this.RaisePropertyChanged("axis_info");
            }
        }

        /// <remarks />
        public kinematics_frame_type frame_origin
        {
            get { return this.frame_originField; }
            set
            {
                this.frame_originField = value;
                this.RaisePropertyChanged("frame_origin");
            }
        }

        /// <remarks />
        public kinematics_frame_type frame_tip
        {
            get { return this.frame_tipField; }
            set
            {
                this.frame_tipField = value;
                this.RaisePropertyChanged("frame_tip");
            }
        }

        /// <remarks />
        public kinematics_frame_type frame_tcp
        {
            get { return this.frame_tcpField; }
            set
            {
                this.frame_tcpField = value;
                this.RaisePropertyChanged("frame_tcp");
            }
        }

        /// <remarks />
        public kinematics_frame_type frame_object
        {
            get { return this.frame_objectField; }
            set
            {
                this.frame_objectField = value;
                this.RaisePropertyChanged("frame_object");
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