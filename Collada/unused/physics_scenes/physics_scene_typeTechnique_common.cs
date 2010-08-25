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
    public class physics_scene_typeTechnique_common : object, INotifyPropertyChanged
    {
        private targetable_float3_type gravityField;

        private targetable_float_type time_stepField;

        /// <remarks />
        public targetable_float3_type gravity
        {
            get { return this.gravityField; }
            set
            {
                this.gravityField = value;
                this.RaisePropertyChanged("gravity");
            }
        }

        /// <remarks />
        public targetable_float_type time_step
        {
            get { return this.time_stepField; }
            set
            {
                this.time_stepField = value;
                this.RaisePropertyChanged("time_step");
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