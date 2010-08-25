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
    public class kinematics_setparam_type : object, INotifyPropertyChanged
    {
        private object itemField;

        private string refField;

        /// <remarks />
        [XmlElement("SIDREF", typeof (string))]
        [XmlElement("bool", typeof (bool))]
        [XmlElement("connect_param", typeof (kinematics_connect_param_type))]
        [XmlElement("float", typeof (double))]
        [XmlElement("int", typeof (long))]
        public object Item
        {
            get { return this.itemField; }
            set
            {
                this.itemField = value;
                this.RaisePropertyChanged("Item");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string @ref
        {
            get { return this.refField; }
            set
            {
                this.refField = value;
                this.RaisePropertyChanged("ref");
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