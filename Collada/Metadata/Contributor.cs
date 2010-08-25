using System.Xml.Serialization;

namespace s3piwrappers.Collada.Metadata
{
    public class Contributor
    {
        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("author_email")]
        public string AuthorEmail { get; set; }

        [XmlElement("author_website", DataType = "anyURI")]
        public string AuthorWebsite { get; set; }

        [XmlElement("authoring_tool")]
        public string AuthoringTool { get; set; }

        [XmlElement("comments")]
        public string Comments { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("source_data", DataType = "anyURI")]
        public string SourceData { get; set; }
    }
}