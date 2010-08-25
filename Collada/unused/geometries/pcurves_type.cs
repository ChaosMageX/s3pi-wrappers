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
    public class pcurves_type : object, INotifyPropertyChanged
    {
        private input_local_offset_type[] inputField;

        private string vcountField;

        private p_type pField;

        private extra_type[] extraField;

        private string idField;

        private string nameField;

        private ulong countField;

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
        public string vcount
        {
            get { return this.vcountField; }
            set
            {
                this.vcountField = value;
                this.RaisePropertyChanged("vcount");
            }
        }

        /// <remarks />
        public p_type p
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
        [XmlAttribute(DataType = "ID")]
        public string id
        {
            get { return this.idField; }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
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