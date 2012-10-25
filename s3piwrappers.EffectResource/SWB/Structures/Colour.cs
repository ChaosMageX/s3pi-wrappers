using System.ComponentModel;

namespace s3piwrappers.SWB.Structures
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public struct Colour
    {
        public float R, G, B;
    }
}
