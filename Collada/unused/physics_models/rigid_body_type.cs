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
    public class rigid_body_type : object, INotifyPropertyChanged
    {
        private rigid_body_typeTechnique_common technique_commonField;

        private technique_type[] techniqueField;

        private extra_type[] extraField;

        private string idField;

        private string sidField;

        private string nameField;

        /// <remarks />
        public rigid_body_typeTechnique_common technique_common
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