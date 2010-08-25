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
    public class instance_rigid_constraint_type : object, INotifyPropertyChanged
    {
        private extra_type[] extraField;

        private string constraintField;

        private string sidField;

        private string nameField;

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
        public string constraint
        {
            get { return this.constraintField; }
            set
            {
                this.constraintField = value;
                this.RaisePropertyChanged("constraint");
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