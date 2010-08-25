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
    public class swept_surface_type : object, INotifyPropertyChanged
    {
        private curve_type curveField;

        private double[] itemsField;

        private ItemsChoiceType2[] itemsElementNameField;

        private extra_type[] extraField;

        /// <remarks />
        public curve_type curve
        {
            get { return this.curveField; }
            set
            {
                this.curveField = value;
                this.RaisePropertyChanged("curve");
            }
        }

        /// <remarks />
        [XmlElement("axis", typeof (double))]
        [XmlElement("direction", typeof (double))]
        [XmlElement("origin", typeof (double))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public double[] Items
        {
            get { return this.itemsField; }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType2[] ItemsElementName
        {
            get { return this.itemsElementNameField; }
            set
            {
                this.itemsElementNameField = value;
                this.RaisePropertyChanged("ItemsElementName");
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