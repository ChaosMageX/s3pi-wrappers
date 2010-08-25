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
    public class rigid_body_typeTechnique_common : object, INotifyPropertyChanged
    {
        private rigid_body_typeTechnique_commonDynamic dynamicField;

        private targetable_float_type massField;

        private object[] mass_frameField;

        private targetable_float3_type inertiaField;

        private object itemField;

        private rigid_body_typeTechnique_commonShape[] shapeField;

        /// <remarks />
        public rigid_body_typeTechnique_commonDynamic dynamic
        {
            get { return this.dynamicField; }
            set
            {
                this.dynamicField = value;
                this.RaisePropertyChanged("dynamic");
            }
        }

        /// <remarks />
        public targetable_float_type mass
        {
            get { return this.massField; }
            set
            {
                this.massField = value;
                this.RaisePropertyChanged("mass");
            }
        }

        /// <remarks />
        [XmlArrayItem("rotate", typeof (rotate_type), IsNullable = false)]
        [XmlArrayItem("translate", typeof (translate_type), IsNullable = false)]
        public object[] mass_frame
        {
            get { return this.mass_frameField; }
            set
            {
                this.mass_frameField = value;
                this.RaisePropertyChanged("mass_frame");
            }
        }

        /// <remarks />
        public targetable_float3_type inertia
        {
            get { return this.inertiaField; }
            set
            {
                this.inertiaField = value;
                this.RaisePropertyChanged("inertia");
            }
        }

        /// <remarks />
        [XmlElement("instance_physics_material", typeof (instance_physics_material_type))]
        [XmlElement("physics_material", typeof (physics_material_type))]
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
        [XmlElement("shape")]
        public rigid_body_typeTechnique_commonShape[] shape
        {
            get { return this.shapeField; }
            set
            {
                this.shapeField = value;
                this.RaisePropertyChanged("shape");
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