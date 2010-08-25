using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public enum fx_sampler_wrap_enum
    {
        /// <remarks />
        WRAP,

        /// <remarks />
        CLAMP,

        /// <remarks />
        BORDER,

        /// <remarks />
        MIRROR,

        /// <remarks />
        MIRROR_ONCE,
    }
}