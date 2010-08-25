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
    public class brep_type : object, INotifyPropertyChanged
    {
        private curves_type curvesField;

        private surface_curves_type surface_curvesField;

        private surfaces_type surfacesField;

        private source_type[] sourceField;

        private vertices_type verticesField;

        private edges_type edgesField;

        private wires_type wiresField;

        private faces_type facesField;

        private pcurves_type pcurvesField;

        private shells_type shellsField;

        private solids_type solidsField;

        private extra_type[] extraField;

        /// <remarks />
        public curves_type curves
        {
            get { return this.curvesField; }
            set
            {
                this.curvesField = value;
                this.RaisePropertyChanged("curves");
            }
        }

        /// <remarks />
        public surface_curves_type surface_curves
        {
            get { return this.surface_curvesField; }
            set
            {
                this.surface_curvesField = value;
                this.RaisePropertyChanged("surface_curves");
            }
        }

        /// <remarks />
        public surfaces_type surfaces
        {
            get { return this.surfacesField; }
            set
            {
                this.surfacesField = value;
                this.RaisePropertyChanged("surfaces");
            }
        }

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
        public edges_type edges
        {
            get { return this.edgesField; }
            set
            {
                this.edgesField = value;
                this.RaisePropertyChanged("edges");
            }
        }

        /// <remarks />
        public wires_type wires
        {
            get { return this.wiresField; }
            set
            {
                this.wiresField = value;
                this.RaisePropertyChanged("wires");
            }
        }

        /// <remarks />
        public faces_type faces
        {
            get { return this.facesField; }
            set
            {
                this.facesField = value;
                this.RaisePropertyChanged("faces");
            }
        }

        /// <remarks />
        public pcurves_type pcurves
        {
            get { return this.pcurvesField; }
            set
            {
                this.pcurvesField = value;
                this.RaisePropertyChanged("pcurves");
            }
        }

        /// <remarks />
        public shells_type shells
        {
            get { return this.shellsField; }
            set
            {
                this.shellsField = value;
                this.RaisePropertyChanged("shells");
            }
        }

        /// <remarks />
        public solids_type solids
        {
            get { return this.solidsField; }
            set
            {
                this.solidsField = value;
                this.RaisePropertyChanged("solids");
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