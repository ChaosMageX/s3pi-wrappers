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
    public class motion_axis_info_type : object, INotifyPropertyChanged
    {
        private kinematics_bind_type[] bindField;

        private kinematics_newparam_type[] newparamField;

        private kinematics_setparam_type[] setparamField;

        private common_float_or_param_type speedField;

        private common_float_or_param_type accelerationField;

        private common_float_or_param_type decelerationField;

        private common_float_or_param_type jerkField;

        private string sidField;

        private string axisField;

        private string nameField;

        /// <remarks />
        [XmlElement("bind")]
        public kinematics_bind_type[] bind
        {
            get { return this.bindField; }
            set
            {
                this.bindField = value;
                this.RaisePropertyChanged("bind");
            }
        }

        /// <remarks />
        [XmlElement("newparam")]
        public kinematics_newparam_type[] newparam
        {
            get { return this.newparamField; }
            set
            {
                this.newparamField = value;
                this.RaisePropertyChanged("newparam");
            }
        }

        /// <remarks />
        [XmlElement("setparam")]
        public kinematics_setparam_type[] setparam
        {
            get { return this.setparamField; }
            set
            {
                this.setparamField = value;
                this.RaisePropertyChanged("setparam");
            }
        }

        /// <remarks />
        public common_float_or_param_type speed
        {
            get { return this.speedField; }
            set
            {
                this.speedField = value;
                this.RaisePropertyChanged("speed");
            }
        }

        /// <remarks />
        public common_float_or_param_type acceleration
        {
            get { return this.accelerationField; }
            set
            {
                this.accelerationField = value;
                this.RaisePropertyChanged("acceleration");
            }
        }

        /// <remarks />
        public common_float_or_param_type deceleration
        {
            get { return this.decelerationField; }
            set
            {
                this.decelerationField = value;
                this.RaisePropertyChanged("deceleration");
            }
        }

        /// <remarks />
        public common_float_or_param_type jerk
        {
            get { return this.jerkField; }
            set
            {
                this.jerkField = value;
                this.RaisePropertyChanged("jerk");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return this.sidField; }
            set
            {
                this.sidField = value;
                this.RaisePropertyChanged("sid");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string axis
        {
            get { return this.axisField; }
            set
            {
                this.axisField = value;
                this.RaisePropertyChanged("axis");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string name
        {
            get { return this.nameField; }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("name");
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