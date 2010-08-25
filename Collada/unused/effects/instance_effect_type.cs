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
    public class instance_effect_type : object, INotifyPropertyChanged
    {
        private instance_effect_typeTechnique_hint[] technique_hintField;

        private instance_effect_typeSetparam[] setparamField;

        private extra_type[] extraField;

        private string urlField;

        private string sidField;

        private string nameField;

        /// <remarks />
        [XmlElement("technique_hint")]
        public instance_effect_typeTechnique_hint[] technique_hint
        {
            get { return this.technique_hintField; }
            set
            {
                this.technique_hintField = value;
                this.RaisePropertyChanged("technique_hint");
            }
        }

        /// <remarks />
        [XmlElement("setparam")]
        public instance_effect_typeSetparam[] setparam
        {
            get { return this.setparamField; }
            set
            {
                this.setparamField = value;
                this.RaisePropertyChanged("setparam");
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

        /// <remarks />
        [XmlAttribute(DataType = "anyURI")]
        public string url
        {
            get { return this.urlField; }
            set
            {
                this.urlField = value;
                this.RaisePropertyChanged("url");
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

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string name
        {
            get { return this.nameField; }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("name");
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