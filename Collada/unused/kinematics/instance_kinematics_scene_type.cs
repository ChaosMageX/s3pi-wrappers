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
    public class instance_kinematics_scene_type : object, INotifyPropertyChanged
    {
        private asset_type assetField;

        private kinematics_newparam_type[] newparamField;

        private kinematics_setparam_type[] setparamField;

        private bind_kinematics_model_type[] bind_kinematics_modelField;

        private bind_joint_axis_type[] bind_joint_axisField;

        private extra_type[] extraField;

        private string urlField;

        private string sidField;

        private string nameField;

        /// <remarks />
        public asset_type asset
        {
            get { return this.assetField; }
            set
            {
                this.assetField = value;
                this.RaisePropertyChanged("asset");
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
        [XmlElement("bind_kinematics_model")]
        public bind_kinematics_model_type[] bind_kinematics_model
        {
            get { return this.bind_kinematics_modelField; }
            set
            {
                this.bind_kinematics_modelField = value;
                this.RaisePropertyChanged("bind_kinematics_model");
            }
        }

        /// <remarks />
        [XmlElement("bind_joint_axis")]
        public bind_joint_axis_type[] bind_joint_axis
        {
            get { return this.bind_joint_axisField; }
            set
            {
                this.bind_joint_axisField = value;
                this.RaisePropertyChanged("bind_joint_axis");
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