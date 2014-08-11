using System.ComponentModel;

namespace s3piwrappers.SWB.Structures
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Rectangle
    {
        public float Left, Top, Right, Bottom;

        public Rectangle(float left, float top, float right, float bottom)
        {
            Left = left; Top = top; Right = right; Bottom = bottom;
        }
    }
}
