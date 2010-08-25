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
    public class tristrips_type : object, INotifyPropertyChanged
    {
        private input_local_offset_type[] inputField;

        private p_type[] pField;

        private extra_type[] extraField;

        private string nameField;

        private ulong countField;

        private string materialField;

        /// <remarks />
        [XmlElement("input")]
        public input_local_offset_type[] input
        {
            get { return this.inputField; }
            set
            {
                this.inputField = value;
                this.RaisePropertyChanged("input");
            }
        }

        /// <remarks />
        [XmlElement("p")]
        public p_type[] p
        {
            get { return this.pField; }
            set
            {
                this.pField = value;
                this.RaisePropertyChanged("p");
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

        /// <remarks />
        [XmlAttribute]
        public ulong count
        {
            get { return this.countField; }
            set
            {
                this.countField = value;
                this.RaisePropertyChanged("count");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "NCName")]
        public string material
        {
            get { return this.materialField; }
            set
            {
                this.materialField = value;
                this.RaisePropertyChanged("material");
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