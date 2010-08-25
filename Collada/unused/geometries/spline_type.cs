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
    public class spline_type : object, INotifyPropertyChanged
    {
        private source_type[] sourceField;

        private spline_typeControl_vertices control_verticesField;

        private extra_type[] extraField;

        private bool closedField;

        public spline_type()
        {
            this.closedField = false;
        }

        /// <remarks />
        [XmlElement("source")]
        public source_type[] source
        {
            get { return this.sourceField; }
            set
            {
                this.sourceField = value;
                this.RaisePropertyChanged("source");
            }
        }

        /// <remarks />
        public spline_typeControl_vertices control_vertices
        {
            get { return this.control_verticesField; }
            set
            {
                this.control_verticesField = value;
                this.RaisePropertyChanged("control_vertices");
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
        [XmlAttribute]
        [DefaultValue(false)]
        public bool closed
        {
            get { return this.closedField; }
            set
            {
                this.closedField = value;
                this.RaisePropertyChanged("closed");
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