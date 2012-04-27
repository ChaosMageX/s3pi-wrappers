using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        string this[Managers.STBL.Lang lang] { get; set; }

        void Rehash();

        bool CommitChanges();
    }
}
