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
    public class fx_sources_type : object, INotifyPropertyChanged
    {
        private object[] itemsField;

        /// <remarks />
        [XmlElement("import", typeof (fx_sources_typeImport))]
        [XmlElement("inline", typeof (string))]
        public object[] Items
        {
            get { return this.itemsField; }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
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