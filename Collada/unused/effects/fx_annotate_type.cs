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
    public class fx_annotate_type : object, INotifyPropertyChanged
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

        private string float2x2Field;

        private string float3x3Field;

        private string float4x4Field;

        private string stringField;

        private string nameField;

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
        public string @string
        {
            get { return this.stringField; }
            set
            {
                this.stringField = value;
                this.RaisePropertyChanged("string");
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