using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{
    
    
    public class GeometryInstance : ExtendableInstance<Geometry>
    {
        public GeometryInstance()
        {
        }

        public GeometryInstance(Geometry src) : base(src)
        {
        }

        //[XmlElement("bind_material")]
        //public BindMaterial BindMaterial { get; set; }
    }
}