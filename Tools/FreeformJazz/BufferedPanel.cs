using System.Drawing;
using System.Windows.Forms;

namespace s3piwrappers.FreeformJazz
{
    [ToolboxBitmap(typeof(Panel))]
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            base.SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint
                , true);
        }
    }
}
