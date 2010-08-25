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
    public class fx_common_transparent_type : fx_common_color_or_texture_type
    {
        private fx_opaque_enum opaqueField;

        public fx_common_transparent_type()
        {
            this.opaqueField = fx_opaque_enum.A_ONE;
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(fx_opaque_enum.A_ONE)]
        public fx_opaque_enum opaque
        {
            get { return this.opaqueField; }
            set
            {
                this.opaqueField = value;
                this.RaisePropertyChanged("opaque");
            }
        }
    }
}