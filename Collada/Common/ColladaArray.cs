using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public abstract class ColladaArray<T> 
        : TargetableNamedComponent, IColladaArray
    {
        protected ColladaArray()
        {
            Items = new List<T>();
        }
        [XmlIgnore]
        public List<T> Items { get; private set; }

        [XmlAttribute("count")]
        public Int32 Count { get { return Items.Count; }set{} }

        [XmlText]
        public String Value
        {
            get
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Items.Count; i++)
                {
                    if (i > 0) sb.Append(" ");
                    sb.Append(Items[i].ToString());
                }
                return sb.ToString();
            }
            set
            {
                Items.Clear();
                foreach (var s in value.Split()) Items.Add(Parse(s));
            }
        }

        protected abstract T Parse(String s);
    }
}