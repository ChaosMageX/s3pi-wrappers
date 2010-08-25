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
    public class rigid_constraint_typeTechnique_commonLimits : object, INotifyPropertyChanged
    {
        private rigid_constraint_typeTechnique_commonLimitsSwing_cone_and_twist swing_cone_and_twistField;

        private rigid_constraint_typeTechnique_commonLimitsLinear linearField;

        /// <remarks />
        public rigid_constraint_typeTechnique_commonLimitsSwing_cone_and_twist swing_cone_and_twist
        {
            get { return this.swing_cone_and_twistField; }
            set
            {
                this.swing_cone_and_twistField = value;
                this.RaisePropertyChanged("swing_cone_and_twist");
            }
        }

        /// <remarks />
        public rigid_constraint_typeTechnique_commonLimitsLinear linear
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