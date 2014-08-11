using System;

namespace s3piwrappers.Helpers.Undo
{
    public interface IHasUndoManager
    {
        UndoManager UndoManager { get; }
    }
}
