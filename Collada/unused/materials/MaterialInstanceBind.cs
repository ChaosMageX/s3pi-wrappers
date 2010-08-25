using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    
    public class MaterialInstanceBind
    {
        private string semanticField;

        private string targetField;

        [XmlAttribute(DataType = "NCName")]
        public string semantic
        {
            get { return semanticField; }
            set { semanticField = value; }
        }

        [XmlAttribute(DataType = "token")]
        public string target
        {
            get { return targetField; }
            set { targetField = value; }
        }
    }
}