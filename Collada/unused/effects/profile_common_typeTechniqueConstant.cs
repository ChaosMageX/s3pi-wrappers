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
    public class profile_common_typeTechniqueConstant : object, INotifyPropertyChanged
    {
        private fx_common_color_or_texture_type emissionField;

        private fx_common_color_or_texture_type reflectiveField;

        private fx_common_float_or_param_type reflectivityField;

        private fx_common_transparent_type transparentField;

        private fx_common_float_or_param_type transparencyField;

        private fx_common_float_or_param_type index_of_refractionField;

        /// <remarks />
        public fx_common_color_or_texture_type emission
        {
            get { return this.emissionField; }
            set
            {
                this.emissionField = value;
                this.RaisePropertyChanged("emission");
            }
        }

        /// <remarks />
        public fx_common_color_or_texture_type reflective
        {
            get { return this.reflectiveField; }
            set
            {
                this.reflectiveField = value;
                this.RaisePropertyChanged("reflective");
            }
        }

        /// <remarks />
        public fx_common_float_or_param_type reflectivity
        {
            get { return this.reflectivityField; }
            set
            {
                this.reflectivityField = value;
                this.RaisePropertyChanged("reflectivity");
            }
        }

        /// <remarks />
        public fx_common_transparent_type transparent
        {
            get { return this.transparentField; }
            set
            {
                this.transparentField = value;
                this.RaisePropertyChanged("transparent");
            }
        }

        /// <remarks />
        public fx_common_float_or_param_type transparency
        {
            get { return this.transparencyField; }
            set
            {
                this.transparencyField = value;
                this.RaisePropertyChanged("transparency");
            }
        }

        /// <remarks />
        public fx_common_float_or_param_type index_of_refraction
        {
            get { return this.index_of_refractionField; }
            set
            {
                this.index_of_refractionField = value;
                this.RaisePropertyChanged("index_of_refraction");
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