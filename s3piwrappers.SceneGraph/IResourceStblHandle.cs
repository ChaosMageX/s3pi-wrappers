using s3piwrappers.SceneGraph.Managers;

namespace s3piwrappers.SceneGraph
{
    public interface IResourceStblHandle
    {
        string Name { get; }

        ulong OriginalKey { get; }

        ulong Key { get; set; }

        string OriginalHashKey { get; }

        string HashKey { get; set; }

        string[] Locale { get; }

        string this[STBL.Lang lang] { get; set; }

        void Rehash();

        bool CommitChanges();
    }
}
