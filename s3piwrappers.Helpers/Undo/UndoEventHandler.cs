using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Helpers.Undo
{
    public delegate void UndoEventHandler(UndoManager sender, Command cmd);
}
