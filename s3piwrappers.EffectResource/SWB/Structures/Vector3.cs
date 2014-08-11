using System.ComponentModel;

namespace s3piwrappers.SWB.Structures
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public struct Vector3
    {
        public float X, Y, Z;

        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
    }
}
