using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace s3piwrappers.Collada.Common
{
    
    
    public class Primitive
    {
        public Primitive()
        {
            Items = new List<ulong>();
        }
        [XmlIgnore]
        public List<ulong> Items { get; set; }
        [XmlText]
        public string Text
        {
            get
            {
                
                var sb = new StringBuilder();
                for (int i = 0; i < Items.Count; i++)
                {
                    if(i>0)sb.Append(" ");
                    sb.Append(Items[i].ToString());
                }
                return sb.ToString();
            }
            set
            {
                var v = value.Split(' ');
                foreach (var s in v)
                {
                    Items.Add(ulong.Parse(s));
                }
            }
        }
    }
}