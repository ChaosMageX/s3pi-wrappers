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
    public class rigid_constraint_typeTechnique_common : object, INotifyPropertyChanged
    {
        private rigid_constraint_typeTechnique_commonEnabled enabledField;

        private rigid_constraint_typeTechnique_commonInterpenetrate interpenetrateField;

        private rigid_constraint_typeTechnique_commonLimits limitsField;

        private rigid_constraint_typeTechnique_commonSpring springField;

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type rigid_constraint_typeTechnique_commonEnabled is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='true' attribute.
        public rigid_constraint_typeTechnique_commonEnabled enabled
        {
            get { return this.enabledField; }
            set
            {
                this.enabledField = value;
                this.RaisePropertyChanged("enabled");
            }
        }

        /// <remarks />
        // CODEGEN Warning: DefaultValue attribute on members of type rigid_constraint_typeTechnique_commonInterpenetrate is not supported in this version of the .Net Framework.
        // CODEGEN Warning: 'default' attribute supported only for primitive types.  Ignoring default='false' attribute.
        public rigid_constraint_typeTechnique_commonInterpenetrate interpenetrate
        {
            get { return this.interpenetrateField; }
            set
            {
                this.interpenetrateField = value;
                this.RaisePropertyChanged("interpenetrate");
            }
        }

        /// <remarks />
        public rigid_constraint_typeTechnique_commonLimits limits
        {
            get { return this.limitsField; }
            set
            {
                this.limitsField = value;
                this.RaisePropertyChanged("limits");
            }
        }

        /// <remarks />
        public rigid_constraint_typeTechnique_commonSpring spring
        {
            get { return this.springField; }
            set
            {
                this.springField = value;
                this.RaisePropertyChanged("spring");
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