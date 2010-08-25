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
    public class convex_mesh_type : object, INotifyPropertyChanged
    {
        private source_type[] sourceField;

        private vertices_type verticesField;

        private object[] itemsField;

        private extra_type[] extraField;

        private string convex_hull_ofField;

        /// <remarks />
        [XmlElement("source")]
        public source_type[] source
        {
            get { return this.sourceField; }
            set
            {
                this.sourceField = value;
                this.RaisePropertyChanged("source");
            }
        }

        /// <remarks />
        public vertices_type vertices
        {
            get { return this.verticesField; }
            set
            {
                this.verticesField = value;
                this.RaisePropertyChanged("vertices");
            }
        }

        /// <remarks />
        [XmlElement("lines", typeof (lines_type))]
        [XmlElement("linestrips", typeof (linestrips_type))]
        [XmlElement("polygons", typeof (polygons_type))]
        [XmlElement("polylist", typeof (polylist_type))]
        [XmlElement("triangles", typeof (triangles_type))]
        [XmlElement("trifans", typeof (trifans_type))]
        [XmlElement("tristrips", typeof (tristrips_type))]
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
        [XmlAttribute(DataType = "anyURI")]
        public string convex_hull_of
        {
            get { return this.convex_hull_ofField; }
            set
            {
                this.convex_hull_ofField = value;
                this.RaisePropertyChanged("convex_hull_of");
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