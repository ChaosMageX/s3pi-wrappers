using System.ComponentModel;

namespace s3piwrappers.SWB.Structures
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public struct Vector2
    {
        public float X, Y;

        public Vector2(float x, float y) { X = x; Y = y; }
    }
}
