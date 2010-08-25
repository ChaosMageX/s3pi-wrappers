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
    public class fx_target_typeBinaryHex : object, INotifyPropertyChanged
    {
        private string formatField;

        private byte[][] textField;

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string format
        {
            get { return this.formatField; }
            set
            {
                this.formatField = value;
                this.RaisePropertyChanged("format");
            }
        }

        /// <remarks />
        [XmlText(DataType = "hexBinary")]
        public byte[][] Text
        {
            get { return this.textField; }
            set
            {
                this.textField = value;
                this.RaisePropertyChanged("Text");
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