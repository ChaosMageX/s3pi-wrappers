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
    public class nurbs_surface_type : object, INotifyPropertyChanged
    {
        private source_type[] sourceField;

        private nurbs_surface_typeControl_vertices control_verticesField;

        private extra_type[] extraField;

        private ulong degree_uField;

        private bool closed_uField;

        private ulong degree_vField;

        private bool closed_vField;

        public nurbs_surface_type()
        {
            this.closed_uField = false;
            this.closed_vField = false;
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
        public nurbs_surface_typeControl_vertices control_vertices
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
        public ulong degree_u
        {
            get { return this.degree_uField; }
            set
            {
                this.degree_uField = value;
                this.RaisePropertyChanged("degree_u");
            }
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(false)]
        public bool closed_u
        {
            get { return this.closed_uField; }
            set
            {
                this.closed_uField = value;
                this.RaisePropertyChanged("closed_u");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ulong degree_v
        {
            get { return this.degree_vField; }
            set
            {
                this.degree_vField = value;
                this.RaisePropertyChanged("degree_v");
            }
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(false)]
        public bool closed_v
        {
            get { return this.closed_vField; }
            set
            {
                this.closed_vField = value;
                this.RaisePropertyChanged("closed_v");
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