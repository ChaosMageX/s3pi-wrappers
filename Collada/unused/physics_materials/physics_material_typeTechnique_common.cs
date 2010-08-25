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
    public class physics_material_typeTechnique_common : object, INotifyPropertyChanged
    {
        private targetable_float_type dynamic_frictionField;

        private targetable_float_type restitutionField;

        private targetable_float_type static_frictionField;

        /// <remarks />
        public targetable_float_type dynamic_friction
        {
            get { return this.dynamic_frictionField; }
            set
            {
                this.dynamic_frictionField = value;
                this.RaisePropertyChanged("dynamic_friction");
            }
        }

        /// <remarks />
        public targetable_float_type restitution
        {
            get { return this.restitutionField; }
            set
            {
                this.restitutionField = value;
                this.RaisePropertyChanged("restitution");
            }
        }

        /// <remarks />
        public targetable_float_type static_friction
        {
            get { return this.static_frictionField; }
            set
            {
                this.static_frictionField = value;
                this.RaisePropertyChanged("static_friction");
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