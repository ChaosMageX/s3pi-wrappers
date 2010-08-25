using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    /// <remarks />
    [XmlInclude(typeof (fx_stenciltarget_type))]
    [XmlInclude(typeof (fx_depthtarget_type))]
    [XmlInclude(typeof (fx_colortarget_type))]
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class fx_rendertarget_type : object, INotifyPropertyChanged
    {
        private object itemField;

        private string indexField;

        private string mipField;

        private image_face_enum faceField;

        private string sliceField;

        public fx_rendertarget_type()
        {
            this.indexField = "0";
            this.mipField = "0";
            this.faceField = image_face_enum.POSITIVE_X;
            this.sliceField = "0";
        }

        /// <remarks />
        [XmlElement("instance_image", typeof (instance_image_type))]
        [XmlElement("param", typeof (fx_rendertarget_typeParam))]
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
        [XmlAttribute(DataType = "nonNegativeInteger")]
        [DefaultValue("0")]
        public string index
        {
            get { return this.indexField; }
            set
            {
                this.indexField = value;
                this.RaisePropertyChanged("index");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "nonNegativeInteger")]
        [DefaultValue("0")]
        public string mip
        {
            get { return this.mipField; }
            set
            {
                this.mipField = value;
                this.RaisePropertyChanged("mip");
            }
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(image_face_enum.POSITIVE_X)]
        public image_face_enum face
        {
            get { return this.faceField; }
            set
            {
                this.faceField = value;
                this.RaisePropertyChanged("face");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "nonNegativeInteger")]
        [DefaultValue("0")]
        public string slice
        {
            get { return this.sliceField; }
            set
            {
                this.sliceField = value;
                this.RaisePropertyChanged("slice");
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