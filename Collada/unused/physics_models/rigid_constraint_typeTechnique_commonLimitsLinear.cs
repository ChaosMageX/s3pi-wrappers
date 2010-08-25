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
    public class rigid_constraint_typeTechnique_commonLimitsLinear : object, INotifyPropertyChanged
    {
        private targetable_float3_type minField;

        private targetable_float3_type maxField;

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float3_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0 0.0 0.0' attribute.
        public targetable_float3_type min
        {
            get { return this.minField; }
            set
            {
                this.minField = value;
                this.RaisePropertyChanged("min");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float3_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0 0.0 0.0' attribute.
        public targetable_float3_type max
        {
            get { return this.maxField; }
            set
            {
                this.maxField = value;
                this.RaisePropertyChanged("max");
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