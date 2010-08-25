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
    public class fx_newparam_type : object, INotifyPropertyChanged
    {
        private fx_annotate_type[] annotateField;

        private string semanticField;

        private fx_modifier_enum modifierField;

        private bool modifierFieldSpecified;

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

        private fx_sampler1D_type sampler1DField;

        private fx_sampler2D_type sampler2DField;

        private fx_sampler3D_type sampler3DField;

        private fx_samplerCUBE_type samplerCUBEField;

        private fx_samplerRECT_type samplerRECTField;

        private fx_samplerDEPTH_type samplerDEPTHField;

        private string enumField;

        private string sidField;

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
        [XmlElement(DataType = "NCName")]
        public string semantic
        {
            get { return this.semanticField; }
            set
            {
                this.semanticField = value;
                this.RaisePropertyChanged("semantic");
            }
        }

        /// <remarks />
        public fx_modifier_enum modifier
        {
            get { return this.modifierField; }
            set
            {
                this.modifierField = value;
                this.RaisePropertyChanged("modifier");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool modifierSpecified
        {
            get { return this.modifierFieldSpecified; }
            set
            {
                this.modifierFieldSpecified = value;
                this.RaisePropertyChanged("modifierSpecified");
            }
        }

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
        public fx_sampler1D_type sampler1D
        {
            get { return this.sampler1DField; }
            set
            {
                this.sampler1DField = value;
                this.RaisePropertyChanged("sampler1D");
            }
        }

        /// <remarks />
        public fx_sampler2D_type sampler2D
        {
            get { return this.sampler2DField; }
            set
            {
                this.sampler2DField = value;
                this.RaisePropertyChanged("sampler2D");
            }
        }

        /// <remarks />
        public fx_sampler3D_type sampler3D
        {
            get { return this.sampler3DField; }
            set
            {
                this.sampler3DField = value;
                this.RaisePropertyChanged("sampler3D");
            }
        }

        /// <remarks />
        public fx_samplerCUBE_type samplerCUBE
        {
            get { return this.samplerCUBEField; }
            set
            {
                this.samplerCUBEField = value;
                this.RaisePropertyChanged("samplerCUBE");
            }
        }

        /// <remarks />
        public fx_samplerRECT_type samplerRECT
        {
            get { return this.samplerRECTField; }
            set
            {
                this.samplerRECTField = value;
                this.RaisePropertyChanged("samplerRECT");
            }
        }

        /// <remarks />
        public fx_samplerDEPTH_type samplerDEPTH
        {
            get { return this.samplerDEPTHField; }
            set
            {
                this.samplerDEPTHField = value;
                this.RaisePropertyChanged("samplerDEPTH");
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