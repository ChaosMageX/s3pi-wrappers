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
    public class kinematics_axis_info_type : object, INotifyPropertyChanged
    {
        private kinematics_newparam_type[] newparamField;

        private common_bool_or_param_type activeField;

        private common_bool_or_param_type lockedField;

        private kinematics_index_type[] indexField;

        private kinematics_limits_type limitsField;

        private object[] itemsField;

        private string sidField;

        private string nameField;

        private string axisField;

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
        public common_bool_or_param_type active
        {
            get { return this.activeField; }
            set
            {
                this.activeField = value;
                this.RaisePropertyChanged("active");
            }
        }

        /// <remarks />
        public common_bool_or_param_type locked
        {
            get { return this.lockedField; }
            set
            {
                this.lockedField = value;
                this.RaisePropertyChanged("locked");
            }
        }

        /// <remarks />
        [XmlElement("index")]
        public kinematics_index_type[] index
        {
            get { return this.indexField; }
            set
            {
                this.indexField = value;
                this.RaisePropertyChanged("index");
            }
        }

        /// <remarks />
        public kinematics_limits_type limits
        {
            get { return this.limitsField; }
            set
            {
                this.limitsField = value;
                this.RaisePropertyChanged("limits");
            }
        }

        /// <remarks />
        [XmlElement("formula", typeof (formula_type))]
        [XmlElement("instance_formula", typeof (instance_formula_type))]
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
        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return this.sidField; }
            set
            {
                this.sidField = value;
                this.RaisePropertyChanged("sid");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string name
        {
            get { return this.nameField; }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string axis
        {
            get { return this.axisField; }
            set
            {
                this.axisField = value;
                this.RaisePropertyChanged("axis");
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