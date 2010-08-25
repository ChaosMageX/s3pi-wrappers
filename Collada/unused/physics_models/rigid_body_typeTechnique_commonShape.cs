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
    public class rigid_body_typeTechnique_commonShape : object, INotifyPropertyChanged
    {
        private rigid_body_typeTechnique_commonShapeHollow hollowField;

        private targetable_float_type massField;

        private targetable_float_type densityField;

        private object itemField;

        private object item1Field;

        private object[] itemsField;

        private extra_type[] extraField;

        /// <remarks />
        public rigid_body_typeTechnique_commonShapeHollow hollow
        {
            get { return this.hollowField; }
            set
            {
                this.hollowField = value;
                this.RaisePropertyChanged("hollow");
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
        public targetable_float_type density
        {
            get { return this.densityField; }
            set
            {
                this.densityField = value;
                this.RaisePropertyChanged("density");
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
        [XmlElement("box", typeof (box_type))]
        [XmlElement("capsule", typeof (capsule_type))]
        [XmlElement("cylinder", typeof (cylinder_type))]
        [XmlElement("instance_geometry", typeof (instance_geometry_type))]
        [XmlElement("plane", typeof (plane_type))]
        [XmlElement("sphere", typeof (sphere_type))]
        public object Item1
        {
            get { return this.item1Field; }
            set
            {
                this.item1Field = value;
                this.RaisePropertyChanged("Item1");
            }
        }

        /// <remarks />
        [XmlElement("rotate", typeof (rotate_type))]
        [XmlElement("translate", typeof (translate_type))]
        public object[] Items
        {
            get { return this.itemsField; }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        [XmlElement("extra")]
        public extra_type[] extra
        {
            get { return this.extraField; }
            set
            {
                this.extraField = value;
                this.RaisePropertyChanged("extra");
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