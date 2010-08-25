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
    public class library_physics_models_type : object, INotifyPropertyChanged
    {
        private asset_type assetField;

        private physics_model_type[] physics_modelField;

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
        [XmlElement("physics_model")]
        public physics_model_type[] physics_model
        {
            get { return this.physics_modelField; }
            set
            {
                this.physics_modelField = value;
                this.RaisePropertyChanged("physics_model");
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