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
    public class fx_common_color_or_texture_typeTexture : object, INotifyPropertyChanged
    {
        private extra_type[] extraField;

        private string textureField;

        private string texcoordField;

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
        [XmlAttribute(DataType = "NCName")]
        public string texture
        {
            get { return this.textureField; }
            set
            {
                this.textureField = value;
                this.RaisePropertyChanged("texture");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "NCName")]
        public string texcoord
        {
            get { return this.texcoordField; }
            set
            {
                this.texcoordField = value;
                this.RaisePropertyChanged("texcoord");
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