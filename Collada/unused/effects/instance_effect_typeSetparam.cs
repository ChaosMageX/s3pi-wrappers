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
    public class instance_effect_typeSetparam : object, INotifyPropertyChanged
    {
        private bool boolField;

        private string bool2Field;

        private string bool3Field;

        private string bool4Field;

        private long intField;

        private string int2Field;

        private string int3Field;

        private string int4Field;

        private double floatField;

        private string float2Field;

        private string float3Field;

        private string float4Field;

        private string float2x1Field;

        private string float2x2Field;

        private string float2x3Field;

        private string float2x4Field;

        private string float3x1Field;

        private string float3x2Field;

        private string float3x3Field;

        private string float3x4Field;

        private string float4x1Field;

        private string float4x2Field;

        private string float4x3Field;

        private string float4x4Field;

        private string enumField;

        private instance_image_type sampler_imageField;

        private instance_effect_typeSetparamSampler_states sampler_statesField;

        private string refField;

        /// <remarks />
        public bool @bool
        {
            get { return this.boolField; }
            set
            {
                this.boolField = value;
                this.RaisePropertyChanged("bool");
            }
        }

        /// <remarks />
        public string bool2
        {
            get { return this.bool2Field; }
            set
            {
                this.bool2Field = value;
                this.RaisePropertyChanged("bool2");
            }
        }

        /// <remarks />
        public string bool3
        {
            get { return this.bool3Field; }
            set
            {
                this.bool3Field = value;
                this.RaisePropertyChanged("bool3");
            }
        }

        /// <remarks />
        public string bool4
        {
            get { return this.bool4Field; }
            set
            {
                this.bool4Field = value;
                this.RaisePropertyChanged("bool4");
            }
        }

        /// <remarks />
        public long @int
        {
            get { return this.intField; }
            set
            {
                this.intField = value;
                this.RaisePropertyChanged("int");
            }
        }

        /// <remarks />
        public string int2
        {
            get { return this.int2Field; }
            set
            {
                this.int2Field = value;
                this.RaisePropertyChanged("int2");
            }
        }

        /// <remarks />
        public string int3
        {
            get { return this.int3Field; }
            set
            {
                this.int3Field = value;
                this.RaisePropertyChanged("int3");
            }
        }

        /// <remarks />
        public string int4
        {
            get { return this.int4Field; }
            set
            {
                this.int4Field = value;
                this.RaisePropertyChanged("int4");
            }
        }

        /// <remarks />
        public double @float
        {
            get { return this.floatField; }
            set
            {
                this.floatField = value;
                this.RaisePropertyChanged("float");
            }
        }

        /// <remarks />
        public string float2
        {
            get { return this.float2Field; }
            set
            {
                this.float2Field = value;
                this.RaisePropertyChanged("float2");
            }
        }

        /// <remarks />
        public string float3
        {
            get { return this.float3Field; }
            set
            {
                this.float3Field = value;
                this.RaisePropertyChanged("float3");
            }
        }

        /// <remarks />
        public string float4
        {
            get { return this.float4Field; }
            set
            {
                this.float4Field = value;
                this.RaisePropertyChanged("float4");
            }
        }

        /// <remarks />
        public string float2x1
        {
            get { return this.float2x1Field; }
            set
            {
                this.float2x1Field = value;
                this.RaisePropertyChanged("float2x1");
            }
        }

        /// <remarks />
        public string float2x2
        {
            get { return this.float2x2Field; }
            set
            {
                this.float2x2Field = value;
                this.RaisePropertyChanged("float2x2");
            }
        }

        /// <remarks />
        public string float2x3
        {
            get { return this.float2x3Field; }
            set
            {
                this.float2x3Field = value;
                this.RaisePropertyChanged("float2x3");
            }
        }

        /// <remarks />
        public string float2x4
        {
            get { return this.float2x4Field; }
            set
            {
                this.float2x4Field = value;
                this.RaisePropertyChanged("float2x4");
            }
        }

        /// <remarks />
        public string float3x1
        {
            get { return this.float3x1Field; }
            set
            {
                this.float3x1Field = value;
                this.RaisePropertyChanged("float3x1");
            }
        }

        /// <remarks />
        public string float3x2
        {
            get { return this.float3x2Field; }
            set
            {
                this.float3x2Field = value;
                this.RaisePropertyChanged("float3x2");
            }
        }

        /// <remarks />
        public string float3x3
        {
            get { return this.float3x3Field; }
            set
            {
                this.float3x3Field = value;
                this.RaisePropertyChanged("float3x3");
            }
        }

        /// <remarks />
        public string float3x4
        {
            get { return this.float3x4Field; }
            set
            {
                this.float3x4Field = value;
                this.RaisePropertyChanged("float3x4");
            }
        }

        /// <remarks />
        public string float4x1
        {
            get { return this.float4x1Field; }
            set
            {
                this.float4x1Field = value;
                this.RaisePropertyChanged("float4x1");
            }
        }

        /// <remarks />
        public string float4x2
        {
            get { return this.float4x2Field; }
            set
            {
                this.float4x2Field = value;
                this.RaisePropertyChanged("float4x2");
            }
        }

        /// <remarks />
        public string float4x3
        {
            get { return this.float4x3Field; }
            set
            {
                this.float4x3Field = value;
                this.RaisePropertyChanged("float4x3");
            }
        }

        /// <remarks />
        public string float4x4
        {
            get { return this.float4x4Field; }
            set
            {
                this.float4x4Field = value;
                this.RaisePropertyChanged("float4x4");
            }
        }

        /// <remarks />
        public string @enum
        {
            get { return this.enumField; }
            set
            {
                this.enumField = value;
                this.RaisePropertyChanged("enum");
            }
        }

        /// <remarks />
        public instance_image_type sampler_image
        {
            get { return this.sampler_imageField; }
            set
            {
                this.sampler_imageField = value;
                this.RaisePropertyChanged("sampler_image");
            }
        }

        /// <remarks />
        public instance_effect_typeSetparamSampler_states sampler_states
        {
            get { return this.sampler_statesField; }
            set
            {
                this.sampler_statesField = value;
                this.RaisePropertyChanged("sampler_states");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "token")]
        public string @ref
        {
            get { return this.refField; }
            set
            {
                this.refField = value;
                this.RaisePropertyChanged("ref");
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