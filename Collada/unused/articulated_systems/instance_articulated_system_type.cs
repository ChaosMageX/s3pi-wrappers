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
    public class instance_articulated_system_type : object, INotifyPropertyChanged
    {
        private kinematics_bind_type[] bindField;

        private kinematics_setparam_type[] setparamField;

        private kinematics_newparam_type[] newparamField;

        private extra_type[] extraField;

        private string sidField;

        private string urlField;

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
        [XmlAttribute(DataType = "anyURI")]
        public string url
        {
            get { return this.urlField; }
            set
            {
                this.urlField = value;
                this.RaisePropertyChanged("url");
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