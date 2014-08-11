using System.ComponentModel;

namespace s3piwrappers.SWB.Structures
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public struct Color
    {
        public float R, G, B;

        public Color(float r, float g, float b) { R = r; G = g; B = b; }
    }
}
