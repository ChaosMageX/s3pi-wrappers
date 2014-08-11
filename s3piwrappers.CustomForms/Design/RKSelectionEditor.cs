using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using s3pi.Interfaces;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.CustomForms.Design
{
    public class RKSelectionEditor : UITypeEditor
    {
        private uint mTID;
        private ResourceMgr mMgr;

        public RKSelectionEditor(uint resourceType)
        {
            this.mTID = resourceType;
            this.mMgr = ResourceMgr.GetResourceManager(resourceType);
        }

        public override UITypeEditorEditStyle GetEditStyle(
            System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(
            System.ComponentModel.ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc
                    = (IWindowsFormsEditorService)provider.GetService(
                        typeof(IWindowsFormsEditorService));
                if (edSvc == null)
                {
                    return value;
                }
                SelectResourceDialog srd 
                    = new SelectResourceDialog(this.mMgr);
                if (edSvc.ShowDialog(srd) != 
                    System.Windows.Forms.DialogResult.OK)
                {
                    return value;
                }
                ResourceMgr.ResEntry res = srd.SelectedResource;
                if (res == null)
                {
                    return value;
                }
                Type t = value.GetType();
                RK key = new RK(this.mTID, res.GID, res.IID);
                if (t.Equals(typeof(RK)))
                {
                    return key;
                }
                if (t.IsInterface && 
                    typeof(IResourceKey).IsAssignableFrom(t))
                {
                    return key;
                }
                if (t.Equals(typeof(string)))
                {
                    return key.ToString();
                }
            }
            return value;
        }
    }
}
