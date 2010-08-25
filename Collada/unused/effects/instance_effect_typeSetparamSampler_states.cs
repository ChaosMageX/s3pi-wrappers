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
    public class instance_effect_typeSetparamSampler_states : object, INotifyPropertyChanged
    {
        private fx_sampler_wrap_enum wrap_sField;

        private fx_sampler_wrap_enum wrap_tField;

        private fx_sampler_wrap_enum wrap_pField;

        private fx_sampler_min_filter_enum minfilterField;

        private fx_sampler_mag_filter_enum magfilterField;

        private fx_sampler_mip_filter_enum mipfilterField;

        private string border_colorField;

        private byte mip_max_levelField;

        private byte mip_min_levelField;

        private float mip_biasField;

        private uint max_anisotropyField;

        private extra_type[] extraField;

        public instance_effect_typeSetparamSampler_states()
        {
            this.wrap_sField = fx_sampler_wrap_enum.WRAP;
            this.wrap_tField = fx_sampler_wrap_enum.WRAP;
            this.wrap_pField = fx_sampler_wrap_enum.WRAP;
            this.minfilterField = fx_sampler_min_filter_enum.LINEAR;
            this.magfilterField = fx_sampler_mag_filter_enum.LINEAR;
            this.mipfilterField = fx_sampler_mip_filter_enum.LINEAR;
            this.mip_max_levelField = ((0));
            this.mip_min_levelField = ((0));
            this.mip_biasField = ((0F));
            this.max_anisotropyField = ((1));
        }

        /// <remarks />
        [DefaultValue(fx_sampler_wrap_enum.WRAP)]
        public fx_sampler_wrap_enum wrap_s
        {
            get { return this.wrap_sField; }
            set
            {
                this.wrap_sField = value;
                this.RaisePropertyChanged("wrap_s");
            }
        }

        /// <remarks />
        [DefaultValue(fx_sampler_wrap_enum.WRAP)]
        public fx_sampler_wrap_enum wrap_t
        {
            get { return this.wrap_tField; }
            set
            {
                this.wrap_tField = value;
                this.RaisePropertyChanged("wrap_t");
            }
        }

        /// <remarks />
        [DefaultValue(fx_sampler_wrap_enum.WRAP)]
        public fx_sampler_wrap_enum wrap_p
        {
            get { return this.wrap_pField; }
            set
            {
                this.wrap_pField = value;
                this.RaisePropertyChanged("wrap_p");
            }
        }

        /// <remarks />
        [DefaultValue(fx_sampler_min_filter_enum.LINEAR)]
        public fx_sampler_min_filter_enum minfilter
        {
            get { return this.minfilterField; }
            set
            {
                this.minfilterField = value;
                this.RaisePropertyChanged("minfilter");
            }
        }

        /// <remarks />
        [DefaultValue(fx_sampler_mag_filter_enum.LINEAR)]
        public fx_sampler_mag_filter_enum magfilter
        {
            get { return this.magfilterField; }
            set
            {
                this.magfilterField = value;
                this.RaisePropertyChanged("magfilter");
            }
        }

        /// <remarks />
        [DefaultValue(fx_sampler_mip_filter_enum.LINEAR)]
        public fx_sampler_mip_filter_enum mipfilter
        {
            get { return this.mipfilterField; }
            set
            {
                this.mipfilterField = value;
                this.RaisePropertyChanged("mipfilter");
            }
        }

        /// <remarks />
        public string border_color
        {
            get { return this.border_colorField; }
            set
            {
                this.border_colorField = value;
                this.RaisePropertyChanged("border_color");
            }
        }

        /// <remarks />
        [DefaultValue(typeof (byte), "0")]
        public byte mip_max_level
        {
            get { return this.mip_max_levelField; }
            set
            {
                this.mip_max_levelField = value;
                this.RaisePropertyChanged("mip_max_level");
            }
        }

        /// <remarks />
        [DefaultValue(typeof (byte), "0")]
        public byte mip_min_level
        {
            get { return this.mip_min_levelField; }
            set
            {
                this.mip_min_levelField = value;
                this.RaisePropertyChanged("mip_min_level");
            }
        }

        /// <remarks />
        [DefaultValue(typeof (float), "0")]
        public float mip_bias
        {
            get { return this.mip_biasField; }
            set
            {
                this.mip_biasField = value;
                this.RaisePropertyChanged("mip_bias");
            }
        }

        /// <remarks />
        [DefaultValue(typeof (uint), "1")]
        public uint max_anisotropy
        {
            get { return this.max_anisotropyField; }
            set
            {
                this.max_anisotropyField = value;
                this.RaisePropertyChanged("max_anisotropy");
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