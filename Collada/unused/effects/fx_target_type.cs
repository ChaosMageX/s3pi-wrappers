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
    public class fx_target_type : object, INotifyPropertyChanged
    {
        private fx_target_typeBinary binaryField;

        private string platformField;

        private string targetField;

        private string optionsField;

        /// <remarks />
        public fx_target_typeBinary binary
        {
            get { return this.binaryField; }
            set
            {
                this.binaryField = value;
                this.RaisePropertyChanged("binary");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string platform
        {
            get { return this.platformField; }
            set
            {
                this.platformField = value;
                this.RaisePropertyChanged("platform");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string target
        {
            get { return this.targetField; }
            set
            {
                this.targetField = value;
                this.RaisePropertyChanged("target");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string options
        {
            get { return this.optionsField; }
            set
            {
                this.optionsField = value;
                this.RaisePropertyChanged("options");
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