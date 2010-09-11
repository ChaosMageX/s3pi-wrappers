using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers.SWB.Structures
{
    [DataGridExpandable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Vector2
    {
        public float X, Y;
    }
}
