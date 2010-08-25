﻿using System;
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
    public class surface_type : object, INotifyPropertyChanged
    {
        private object itemField;

        private orient_type[] orientField;

        private origin_type originField;

        private string sidField;

        private string nameField;

        /// <remarks />
        [XmlElement("cone", typeof (cone_type))]
        [XmlElement("cylinder", typeof (surface_typeCylinder))]
        [XmlElement("nurbs_surface", typeof (nurbs_surface_type))]
        [XmlElement("plane", typeof (plane_type))]
        [XmlElement("sphere", typeof (sphere_type))]
        [XmlElement("swept_surface", typeof (swept_surface_type))]
        [XmlElement("torus", typeof (torus_type))]
        public object Item
        {
            get { return this.itemField; }
            set
            {
                this.itemField = value;
                this.RaisePropertyChanged("Item");
            }
        }

        /// <remarks />
        [XmlElement("orient")]
        public orient_type[] orient
        {
            get { return this.orientField; }
            set
            {
                this.orientField = value;
                this.RaisePropertyChanged("orient");
            }
        }

        /// <remarks />
        public origin_type origin
        {
            get { return this.originField; }
            set
            {
                this.originField = value;
                this.RaisePropertyChanged("origin");
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