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
    public class rigid_constraint_typeTechnique_commonSpring : object, INotifyPropertyChanged
    {
        private rigid_constraint_typeTechnique_commonSpringAngular angularField;

        private rigid_constraint_typeTechnique_commonSpringLinear linearField;

        /// <remarks />
        public rigid_constraint_typeTechnique_commonSpringAngular angular
        {
            get { return this.angularField; }
            set
            {
                this.angularField = value;
                this.RaisePropertyChanged("angular");
            }
        }

        /// <remarks />
        public rigid_constraint_typeTechnique_commonSpringLinear linear
        {
            get { return this.linearField; }
            set
            {
                this.linearField = value;
                this.RaisePropertyChanged("linear");
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