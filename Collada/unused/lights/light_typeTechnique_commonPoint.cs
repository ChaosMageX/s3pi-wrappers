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
    public class light_typeTechnique_commonPoint : object, INotifyPropertyChanged
    {
        private targetable_float3_type colorField;

        private targetable_float_type constant_attenuationField;

        private targetable_float_type linear_attenuationField;

        private targetable_float_type quadratic_attenuationField;

        /// <remarks />
        public targetable_float3_type color
        {
            get { return this.colorField; }
            set
            {
                this.colorField = value;
                this.RaisePropertyChanged("color");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='1.0' attribute.
        public targetable_float_type constant_attenuation
        {
            get { return this.constant_attenuationField; }
            set
            {
                this.constant_attenuationField = value;
                this.RaisePropertyChanged("constant_attenuation");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0' attribute.
        public targetable_float_type linear_attenuation
        {
            get { return this.linear_attenuationField; }
            set
            {
                this.linear_attenuationField = value;
                this.RaisePropertyChanged("linear_attenuation");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type targetable_float_type is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='0.0' attribute.
        public targetable_float_type quadratic_attenuation
        {
            get { return this.quadratic_attenuationField; }
            set
            {
                this.quadratic_attenuationField = value;
                this.RaisePropertyChanged("quadratic_attenuation");
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