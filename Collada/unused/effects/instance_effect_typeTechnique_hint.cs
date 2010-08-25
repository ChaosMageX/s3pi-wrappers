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
    public class instance_effect_typeTechnique_hint : object, INotifyPropertyChanged
    {
        private string platformField;

        private string profileField;

        private string refField;

        /// <remarks />
        [XmlAttribute(DataType = "NCName")]
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
        [XmlAttribute(DataType = "NCName")]
        public string profile
        {
            get { return this.profileField; }
            set
            {
                this.profileField = value;
                this.RaisePropertyChanged("profile");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "NCName")]
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