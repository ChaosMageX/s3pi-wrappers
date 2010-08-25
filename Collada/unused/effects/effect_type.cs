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
    public class effect_type : object, INotifyPropertyChanged
    {
        private asset_type assetField;

        private fx_annotate_type[] annotateField;

        private fx_newparam_type[] newparamField;

        private profile_common_type[] profile_COMMONField;

        private profile_bridge_type[] profile_BRIDGEField;

        private extra_type[] extraField;

        private string idField;

        private string nameField;

        /// <remarks />
        public asset_type asset
        {
            get { return this.assetField; }
            set
            {
                this.assetField = value;
                this.RaisePropertyChanged("asset");
            }
        }

        /// <remarks />
        [XmlElement("annotate")]
        public fx_annotate_type[] annotate
        {
            get { return this.annotateField; }
            set
            {
                this.annotateField = value;
                this.RaisePropertyChanged("annotate");
            }
        }

        /// <remarks />
        [XmlElement("newparam")]
        public fx_newparam_type[] newparam
        {
            get { return this.newparamField; }
            set
            {
                this.newparamField = value;
                this.RaisePropertyChanged("newparam");
            }
        }

        /// <remarks />
        [XmlElement("profile_COMMON")]
        public profile_common_type[] profile_COMMON
        {
            get { return this.profile_COMMONField; }
            set
            {
                this.profile_COMMONField = value;
                this.RaisePropertyChanged("profile_COMMON");
            }
        }

        /// <remarks />
        [XmlElement("profile_BRIDGE")]
        public profile_bridge_type[] profile_BRIDGE
        {
            get { return this.profile_BRIDGEField; }
            set
            {
                this.profile_BRIDGEField = value;
                this.RaisePropertyChanged("profile_BRIDGE");
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

        /// <remarks />
        [XmlAttribute(DataType = "ID")]
        public string id
        {
            get { return this.idField; }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
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