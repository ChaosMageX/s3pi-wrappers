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
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class camera_typeOpticsTechnique_commonOrthographic : object, INotifyPropertyChanged
    {
        private targetable_float_type[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        private targetable_float_type znearField;

        private targetable_float_type zfarField;

        /// <remarks />
        [XmlElement("aspect_ratio", typeof (targetable_float_type))]
        [XmlElement("xmag", typeof (targetable_float_type))]
        [XmlElement("ymag", typeof (targetable_float_type))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public targetable_float_type[] Items
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
        public ItemsChoiceType[] ItemsElementName
        {
            get { return this.itemsElementNameField; }
            set
            {
                this.itemsElementNameField = value;
                this.RaisePropertyChanged("ItemsElementName");
            }
        }

        /// <remarks />
        public targetable_float_type znear
        {
            get { return this.znearField; }
            set
            {
                this.znearField = value;
                this.RaisePropertyChanged("znear");
            }
        }

        /// <remarks />
        public targetable_float_type zfar
        {
            get { return this.zfarField; }
            set
            {
                this.zfarField = value;
                this.RaisePropertyChanged("zfar");
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