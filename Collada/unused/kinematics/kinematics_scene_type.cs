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
    public class kinematics_scene_type : object, INotifyPropertyChanged
    {
        private asset_type assetField;

        private instance_kinematics_model_type[] instance_kinematics_modelField;

        private instance_articulated_system_type[] instance_articulated_systemField;

        private extra_type[] extraField;

        private string idField;

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
        [XmlElement("instance_articulated_system")]
        public instance_articulated_system_type[] instance_articulated_system
        {
            get { return this.instance_articulated_systemField; }
            set
            {
                this.instance_articulated_systemField = value;
                this.RaisePropertyChanged("instance_articulated_system");
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
        [XmlAttribute(DataType = "ID")]
        public string id
        {
            get { return this.idField; }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
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