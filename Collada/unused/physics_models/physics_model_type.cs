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
    public class physics_model_type : object, INotifyPropertyChanged
    {
        private asset_type assetField;

        private rigid_body_type[] rigid_bodyField;

        private rigid_constraint_type[] rigid_constraintField;

        private instance_physics_model_type[] instance_physics_modelField;

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
        [XmlElement("rigid_body")]
        public rigid_body_type[] rigid_body
        {
            get { return this.rigid_bodyField; }
            set
            {
                this.rigid_bodyField = value;
                this.RaisePropertyChanged("rigid_body");
            }
        }

        /// <remarks />
        [XmlElement("rigid_constraint")]
        public rigid_constraint_type[] rigid_constraint
        {
            get { return this.rigid_constraintField; }
            set
            {
                this.rigid_constraintField = value;
                this.RaisePropertyChanged("rigid_constraint");
            }
        }

        /// <remarks />
        [XmlElement("instance_physics_model")]
        public instance_physics_model_type[] instance_physics_model
        {
            get { return this.instance_physics_modelField; }
            set
            {
                this.instance_physics_modelField = value;
                this.RaisePropertyChanged("instance_physics_model");
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