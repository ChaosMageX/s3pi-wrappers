using System.Collections;

namespace s3piwrappers.SWB
{
    public interface ISection
    {
        ushort Version { get; }
        ushort Type { get; }
        IEnumerable Items { get; }
    }
}