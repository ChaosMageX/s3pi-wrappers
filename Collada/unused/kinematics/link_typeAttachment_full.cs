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
    public class link_typeAttachment_full : object, INotifyPropertyChanged
    {
        private object[] itemsField;

        private link_type linkField;

        private string jointField;

        /// <remarks />
        [XmlElement("rotate", typeof (rotate_type))]
        [XmlElement("translate", typeof (translate_type))]
        public object[] Items
        {
            get { return this.itemsField; }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        public link_type link
        {
            get { return this.linkField; }
            set
            {
                this.linkField = value;
                this.RaisePropertyChanged("link");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string joint
        {
            get { return this.jointField; }
            set
            {
                this.jointField = value;
                this.RaisePropertyChanged("joint");
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