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
    public class rigid_constraint_typeTechnique_commonSpringLinear : object, INotifyPropertyChanged
    {
        private targetable_float_type stiffnessField;

        private targetable_float_type dampingField;

        private targetable_float_type target_valueField;

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='1.0' attribute.
        public targetable_float_type stiffness
        {
            get { return this.stiffnessField; }
            set
            {
                this.stiffnessField = value;
                this.RaisePropertyChanged("stiffness");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0' attribute.
        public targetable_float_type damping
        {
            get { return this.dampingField; }
            set
            {
                this.dampingField = value;
                this.RaisePropertyChanged("damping");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0' attribute.
        public targetable_float_type target_value
        {
            get { return this.target_valueField; }
            set
            {
                this.target_valueField = value;
                this.RaisePropertyChanged("target_value");
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