using System;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    public class S3PIPropertyGrid : System.Windows.Forms.PropertyGrid
    {
        AApiVersionedFieldsCTD target;
        public S3PIPropertyGrid() : base() { HelpVisible = false; ToolbarVisible = false; }

        public AApiVersionedFields s3piObject
        {
            set
            {
                if (value != null) { target = new AApiVersionedFieldsCTD(value); SelectedObject = target; }
                else SelectedObject = null;
            }
        }
    }
}
