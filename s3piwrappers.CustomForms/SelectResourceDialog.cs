using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.CustomForms
{
    public partial class SelectResourceDialog : Form
    {
        public enum Category
        {
            None = -1,
            Current = 0,
            GameCore = 1,
            GameContent = 2,
            DDSImages = 3,
            Thumbnails = 4,
            CustomContent = 5
        }
        private static readonly object[] sCategoryNames = new object[]
        {
            "Current",
            "Game Core",
            "Game Content",
            "DDS Images",
            "Thumbnails",
            "Custom Content"
        };

        private class IIDComparer : IComparer<ResourceMgr.ResEntry>
        {
            public int Compare(ResourceMgr.ResEntry x, 
                               ResourceMgr.ResEntry y)
            {
                return x.IID == y.IID ? 0 : (x.IID < y.IID ? 1 : -1);
            }
        }
        private static readonly IIDComparer sIIDComparer 
            = new IIDComparer();

        private class GIDComparer : IComparer<ResourceMgr.ResEntry>
        {
            public int Compare(ResourceMgr.ResEntry x, 
                               ResourceMgr.ResEntry y)
            {
                return x.GID == y.GID ? 0 : (x.GID < y.GID ? 1 : -1);
            }
        }
        private static readonly GIDComparer sGIDComparer
            = new GIDComparer();

        private class NameComparer : IComparer<ResourceMgr.ResEntry>
        {
            public int Compare(ResourceMgr.ResEntry x, 
                               ResourceMgr.ResEntry y)
            {
                if (x.Name == y.Name)
                    return 0;
                if (x.Name == null)
                    return 1;
                if (y.Name == null)
                    return -1;
                return x.Name.CompareTo(y.Name);
            }
        }
        private static readonly NameComparer sNameComparer 
            = new NameComparer();

        private ResourceMgr mResourceManager;

        private ResourceMgr.ResPackage mSelectedPackage;

        private int mColIndex;
        private int mResourceCount;
        private ResourceMgr.ResEntry[] mResources;

        public SelectResourceDialog(ResourceMgr resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException("resourceManager");
            }
            this.mResourceManager = resourceManager;

            InitializeComponent();

            //this.categoryCMB.Items.AddRange(sCategoryNames);
            this.RefreshCategories();

            this.mColIndex = 0;
            this.resDGV.Columns[0].HeaderCell.SortGlyphDirection
                = SortOrder.Ascending;

            this.mResourceCount = 0;
            this.mResources = new ResourceMgr.ResEntry[0];
        }

        public void RefreshCategories()
        {
            ResourceMgr.ResPackage[] packages = null;
            ComboBox.ObjectCollection items = this.categoryCMB.Items;
            items.Clear();
            packages = this.mResourceManager.Current;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.Current);
            }
            packages = this.mResourceManager.GameCore;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.GameCore);
            }
            packages = this.mResourceManager.GameContent;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.GameContent);
            }
            packages = this.mResourceManager.DDSImages;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.DDSImages);
            }
            packages = this.mResourceManager.Thumbnails;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.Thumbnails);
            }
            packages = this.mResourceManager.CustomContent;
            if (packages != null && packages.Length > 0)
            {
                items.Add(Category.CustomContent);
            }
        }

        public Category SelectedCategory
        {
            get
            {
                object obj = this.categoryCMB.SelectedItem;
                return obj == null ? Category.None : (Category)obj;
            }
            set
            {
                object obj = this.categoryCMB.SelectedItem;
                Category cat = obj == null ? Category.None : (Category)obj;
                if (cat != value)
                {
                    if (value == Category.None)
                    {
                        this.categoryCMB.SelectedIndex = -1;
                    }
                    else
                    {
                        int index = this.categoryCMB.Items.IndexOf(value);
                        this.categoryCMB.SelectedIndex = index;
                    }
                    this.UpdatePackageCMB();
                }
            }
        }

        public bool SelectCategoryEnabled
        {
            get { return this.categoryCMB.Enabled; }
            set { this.categoryCMB.Enabled = value; }
        }

        public ResourceMgr.ResPackage SelectedPackage
        {
            get { return this.mSelectedPackage; }
            set
            {
                if (this.mSelectedPackage != value)
                {
                    if (value == null)
                    {
                        this.mSelectedPackage = value;
                        this.packageCMB.SelectedIndex = -1;
                        this.UpdateResources();
                    }
                    else
                    {
                        int index = this.packageCMB.Items.IndexOf(value);
                        if (index >= 0)
                        {
                            this.mSelectedPackage = value;
                            this.packageCMB.SelectedIndex = index;
                            this.UpdateResources();
                        }
                    }
                }
            }
        }

        public bool SelectPackageEnabled
        {
            get { return this.packageCMB.Enabled; }
            set { this.packageCMB.Enabled = value; }
        }

        public ResourceMgr.ResEntry SelectedResource
        {
            get
            {
                if (this.mSelectedPackage == null ||
                    this.resDGV.SelectedRows.Count == 0)
                {
                    return null;
                }
                return this.mResources[this.resDGV.SelectedRows[0].Index];
            }
        }

        private void categoryFormat(object sender, ListControlConvertEventArgs e)
        {
            e.Value = sCategoryNames[(int)e.ListItem];
        }

        private void UpdatePackageCMB()
        {
            this.packageCMB.SelectedIndex = -1;
            this.packageCMB.Items.Clear();
            this.mSelectedPackage = null;
            this.mResourceCount = 0;
            this.resDGV.RowCount = 0;
            this.okBTN.Enabled = false;

            ResourceMgr.ResPackage[] packages = null;
            switch (this.SelectedCategory)
            {
                case Category.Current:
                    packages = this.mResourceManager.Current;
                    break;
                case Category.GameCore:
                    packages = this.mResourceManager.GameCore;
                    break;
                case Category.GameContent:
                    packages = this.mResourceManager.GameContent;
                    break;
                case Category.DDSImages:
                    packages = this.mResourceManager.DDSImages;
                    break;
                case Category.Thumbnails:
                    packages = this.mResourceManager.Thumbnails;
                    break;
                case Category.CustomContent:
                    packages = this.mResourceManager.CustomContent;
                    break;
            }
            if (packages != null && packages.Length > 0)
            {
                this.packageCMB.Items.AddRange(packages);
            }
        }

        private void categorySelectChangeCommitted(object sender, EventArgs e)
        {
            this.UpdatePackageCMB();
            if (this.categoryCMB.SelectedIndex != -1)
            {
                this.packageCMB.Focus();
            }
        }

        private void UpdateResources()
        {
            this.mResourceCount = 0;
            this.resDGV.RowCount = 0;
            this.okBTN.Enabled = false;

            if (this.mSelectedPackage != null)
            {
                this.mResourceCount
                    = this.mSelectedPackage.Entries.Length;
                if (this.mResources.Length < this.mResourceCount)
                {
                    this.mResources
                        = new ResourceMgr.ResEntry[this.mResourceCount];
                }
                Array.Copy(this.mSelectedPackage.Entries, 0,
                    this.mResources, 0, this.mResourceCount);

                switch (this.mColIndex)
                {
                    case 0:
                        Array.Sort(this.mResources, 0,
                            this.mResourceCount, sIIDComparer);
                        break;
                    case 1:
                        Array.Sort(this.mResources, 0,
                            this.mResourceCount, sGIDComparer);
                        break;
                    case 2:
                        Array.Sort(this.mResources, 0,
                            this.mResourceCount, sNameComparer);
                        break;
                }
                DataGridViewColumnHeaderCell hc
                    = this.resDGV.Columns[this.mColIndex].HeaderCell;
                if (hc.SortGlyphDirection == SortOrder.Descending)
                {
                    Array.Reverse(this.mResources, 0, this.mResourceCount);
                }
                this.resDGV.RowCount = this.mResourceCount;
            }
        }

        private void packageSelectChangeCommitted(object sender, EventArgs e)
        {
            this.mSelectedPackage 
                = this.packageCMB.SelectedItem as ResourceMgr.ResPackage;
            
            this.UpdateResources();
            if (this.mSelectedPackage != null)
            {
                this.resDGV.Focus();
            }
        }

        private void resColumnHeaderMouseClick(object sender, 
            DataGridViewCellMouseEventArgs e)
        {
            this.mColIndex = e.ColumnIndex;
            DataGridViewColumnHeaderCell hc;
            DataGridViewColumnCollection cols = this.resDGV.Columns;
            for (int i = cols.Count - 1; i >= 0; i--)
            {
                if (i != this.mColIndex)
                {
                    hc = cols[i].HeaderCell;
                    hc.SortGlyphDirection = SortOrder.None;
                }
            }
            hc = cols[this.mColIndex].HeaderCell;
            if (hc.SortGlyphDirection == SortOrder.Ascending)
            {
                hc.SortGlyphDirection = SortOrder.Descending;
            }
            else
            {
                hc.SortGlyphDirection = SortOrder.Ascending;
            }
            switch (this.mColIndex)
            {
                case 0:
                    Array.Sort(this.mResources, 0,
                        this.mResourceCount, sIIDComparer);
                    break;
                case 1:
                    Array.Sort(this.mResources, 0,
                        this.mResourceCount, sGIDComparer);
                    break;
                case 2:
                    Array.Sort(this.mResources, 0,
                        this.mResourceCount, sNameComparer);
                    break;
            }
            if (hc.SortGlyphDirection == SortOrder.Descending)
            {
                Array.Reverse(this.mResources, 0, this.mResourceCount);
            }
            this.resDGV.Invalidate();
        }

        private void resCellValueNeeded(object sender, 
            DataGridViewCellValueEventArgs e)
        {
            if (this.mSelectedPackage != null &&
                e.RowIndex < this.mResourceCount)
            {
                ResourceMgr.ResEntry res = this.mResources[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = "0x" + res.IID.ToString("X16");
                        break;
                    case 1:
                        e.Value = "0x" + res.GID.ToString("X8");
                        break;
                    case 2:
                        e.Value = res.Name;
                        break;
                }
            }
        }

        private void resCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.okBTN.Enabled = true;
        }

        private void resCellContentDoubleClick(object sender, 
            DataGridViewCellEventArgs e)
        {
            this.okBTN.PerformClick();
        }
    }
}
