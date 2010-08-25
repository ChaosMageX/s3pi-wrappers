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
    public class polygons_typePH : object, INotifyPropertyChanged
    {
        private p_type pField;

        private string[] hField;

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
        [XmlElement("h")]
        public string[] h
        {
            get { return this.hField; }
            set
            {
                this.hField = value;
                this.RaisePropertyChanged("h");
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