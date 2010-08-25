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
    public class instance_physics_model_type : object, INotifyPropertyChanged
    {
        private instance_force_field_type[] instance_force_fieldField;

        private instance_rigid_body_type[] instance_rigid_bodyField;

        private instance_rigid_constraint_type[] instance_rigid_constraintField;

        private extra_type[] extraField;

        private string urlField;

        private string sidField;

        private string nameField;

        private string parentField;

        /// <remarks />
        [XmlElement("instance_force_field")]
        public instance_force_field_type[] instance_force_field
        {
            get { return this.instance_force_fieldField; }
            set
            {
                this.instance_force_fieldField = value;
                this.RaisePropertyChanged("instance_force_field");
            }
        }

        /// <remarks />
        [XmlElement("instance_rigid_body")]
        public instance_rigid_body_type[] instance_rigid_body
        {
            get { return this.instance_rigid_bodyField; }
            set
            {
                this.instance_rigid_bodyField = value;
                this.RaisePropertyChanged("instance_rigid_body");
            }
        }

        /// <remarks />
        [XmlElement("instance_rigid_constraint")]
        public instance_rigid_constraint_type[] instance_rigid_constraint
        {
            get { return this.instance_rigid_constraintField; }
            set
            {
                this.instance_rigid_constraintField = value;
                this.RaisePropertyChanged("instance_rigid_constraint");
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

        /// <remarks />
        [XmlAttribute(DataType = "anyURI")]
        public string parent
        {
            get { return this.parentField; }
            set
            {
                this.parentField = value;
                this.RaisePropertyChanged("parent");
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