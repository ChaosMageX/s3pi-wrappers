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
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class kinematics_model_technique_type : object, INotifyPropertyChanged
    {
        private kinematics_newparam_type[] newparamField;

        private object[] itemsField;

        private link_type[] linkField;

        private object[] items1Field;

        /// <remarks />
        [XmlElement("newparam")]
        public kinematics_newparam_type[] newparam
        {
            get { return this.newparamField; }
            set
            {
                this.newparamField = value;
                this.RaisePropertyChanged("newparam");
            }
        }

        /// <remarks />
        [XmlElement("instance_joint", typeof (instance_joint_type))]
        [XmlElement("joint", typeof (joint_type))]
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
        [XmlElement("link")]
        public link_type[] link
        {
            get { return this.linkField; }
            set
            {
                this.linkField = value;
                this.RaisePropertyChanged("link");
            }
        }

        /// <remarks />
        [XmlElement("formula", typeof (formula_type))]
        [XmlElement("instance_formula", typeof (instance_formula_type))]
        public object[] Items1
        {
            get { return this.items1Field; }
            set
            {
                this.items1Field = value;
                this.RaisePropertyChanged("Items1");
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