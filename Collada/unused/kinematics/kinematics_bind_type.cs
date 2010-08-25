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
    public class kinematics_bind_type : object, INotifyPropertyChanged
    {
        private object itemField;

        private string symbolField;

        /// <remarks />
        [XmlElement("SIDREF", typeof (string))]
        [XmlElement("bool", typeof (bool))]
        [XmlElement("float", typeof (double))]
        [XmlElement("int", typeof (long))]
        [XmlElement("param", typeof (kinematics_param_type))]
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
        [XmlAttribute(DataType = "NCName")]
        public string symbol
        {
            get { return this.symbolField; }
            set
            {
                this.symbolField = value;
                this.RaisePropertyChanged("symbol");
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