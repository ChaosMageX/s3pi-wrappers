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
    public class fx_common_newparam_type : object, INotifyPropertyChanged
    {
        private string semanticField;

        private object itemField;

        private ItemChoiceType itemElementNameField;

        private string sidField;

        /// <remarks />
        [XmlElement(DataType = "NCName")]
        public string semantic
        {
            get { return this.semanticField; }
            set
            {
                this.semanticField = value;
                this.RaisePropertyChanged("semantic");
            }
        }

        /// <remarks />
        [XmlElement("float", typeof (double))]
        [XmlElement("float2", typeof (double))]
        [XmlElement("float3", typeof (double))]
        [XmlElement("float4", typeof (double))]
        [XmlElement("sampler2D", typeof (fx_sampler2D_type))]
        [XmlChoiceIdentifier("ItemElementName")]
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
        [XmlIgnore]
        public ItemChoiceType ItemElementName
        {
            get { return this.itemElementNameField; }
            set
            {
                this.itemElementNameField = value;
                this.RaisePropertyChanged("ItemElementName");
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