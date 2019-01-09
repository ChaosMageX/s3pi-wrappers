using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace s3piwrappers.CustomForms.Controls
{
    public interface IBuiltInValueControl
    {
        bool IsAvailable { get; }
        Control ValueControl { get; }
        IEnumerable<ToolStripItem> GetContextMenuItems(EventHandler cbk);
    }
}
