using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3pi.Interfaces;
using s3pi.Extensions;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.CustomForms
{
    public partial class SelectEditRIEDialog : Form
    {
        public enum Hashing
        {
            None,
            FNV32,
            FNV64,
            FNVCLIP
        }

        private enum ColumnName
        {
            Tag = 0,
            TID = 1,
            GID = 2,
            IID = 3,
            Comp = 4,
            Name = 5
        }

        private class TagComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                string xKey = "0x" + x.ResourceType.ToString("X8");
                string yKey = "0x" + y.ResourceType.ToString("X8");
                if (!ExtList.Ext.ContainsKey(xKey))
                {
                    return ExtList.Ext.ContainsKey(yKey) ? 1 : 0;
                }
                if (!ExtList.Ext.ContainsKey(yKey))
                {
                    return ExtList.Ext.ContainsKey(xKey) ? -1 : 0;
                }
                xKey = ExtList.Ext[xKey][0];
                yKey = ExtList.Ext[yKey][0];
                return xKey.CompareTo(yKey);
            }
        }
        private static readonly TagComparer sTagComparer
            = new TagComparer();

        private class TIDComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                return x.ResourceType == y.ResourceType ? 0
                    : (x.ResourceType < y.ResourceType ? 1 : -1);
            }
        }
        private static readonly TIDComparer sTIDComparer
            = new TIDComparer();

        private class GIDComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                return x.ResourceGroup == y.ResourceGroup ? 0
                    : (x.ResourceGroup < y.ResourceGroup ? 1 : -1);
            }
        }
        private static readonly GIDComparer sGIDComparer
            = new GIDComparer();

        private class IIDComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                return x.Instance == y.Instance ? 0 
                    : (x.Instance < y.Instance ? 1 : -1);
            }
        }
        private static readonly IIDComparer sIIDComparer
            = new IIDComparer();

        private class CompComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                return x.Compressed == y.Compressed ? 0
                    : (x.Compressed < y.Compressed ? 1 : -1);
            }
        }
        private static readonly CompComparer sCompComparer
            = new CompComparer();

        private class NameComparer : IComparer<INamedResourceIndexEntry>
        {
            public int Compare(INamedResourceIndexEntry x,
                               INamedResourceIndexEntry y)
            {
                string xName = x.ResourceName;
                string yName = y.ResourceName;
                if (xName == yName)
                    return 0;
                if (xName == null)
                    return 1;
                if (yName == null)
                    return -1;
                return xName.CompareTo(yName);
            }
        }
        private static readonly NameComparer sNameComparer
            = new NameComparer();

        private static string[] sTagDropDown;

        private static void LoadTagDropDown()
        {
            List<string> list = new List<string>();
            try
            {
                foreach (KeyValuePair<string, List<string>> pair in ExtList.Ext)
                {
                    if (pair.Key.StartsWith("0x"))
                    {
                        list.Add(pair.Value[0] + " " + pair.Key);
                    }
                }
            }
            catch { }
            sTagDropDown = list.ToArray();
        }

        private INamedResourceIndexEntry[] mResources;

        private int mColIndex;

        private Hashing mAutoHashing;
        private bool bEnablePasteRKs;
        private bool bDetectConflicts;

        public SelectEditRIEDialog(
            INamedResourceIndexEntry[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Length == 0)
            {
                throw new ArgumentException(
                    "must have at least one non-null value",
                    "resources");
            }
            for (int i = resources.Length - 1; i >= 0; i--)
            {
                if (resources[i] == null)
                {
                    throw new ArgumentException(
                        "cannot have null values",
                        "resources");
                }
            }
            this.mResources = resources;

            InitializeComponent();

            if (sTagDropDown == null)
            {
                LoadTagDropDown();
            }
            this.resTagColumn.Items.Clear();
            this.resTagColumn.Items.AddRange(sTagDropDown);

            this.mColIndex = 0;

            this.mAutoHashing = Hashing.None;
            this.bEnablePasteRKs = true;
            this.bDetectConflicts = false;
        }

        public string Message
        {
            get { return this.messageLBL.Text; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (!this.messageLBL.Text.Equals(value))
                {
                    this.messageLBL.Text = value;
                }
            }
        }

        public Hashing AutomaticHashing
        {
            get { return this.mAutoHashing; }
            set
            {
                if (this.mAutoHashing != value)
                {
                    this.mAutoHashing = value;
                    bool flag = value == Hashing.None;
                    this.fnv32BTN.Enabled = flag;
                    this.fnv64BTN.Enabled = flag;
                    this.fnvClipBTN.Enabled = flag;
                }
            }
        }

        public bool EnablePasteResourceKeys
        {
            get { return this.bEnablePasteRKs; }
            set
            {
                if (this.bEnablePasteRKs != value)
                {
                    this.bEnablePasteRKs = value;
                    this.pasteRKsBTN.Enabled = value;
                }
            }
        }

        public bool DetectConflicts
        {
            get { return this.bDetectConflicts; }
            set
            {
                if (this.bDetectConflicts != value)
                {
                    this.bDetectConflicts = value;
                    if (value)
                    {
                        this.HighlightConflicts();
                    }
                    else
                    {
                        this.ClearConflictHighlighting();
                    }
                }
            }
        }

        public bool ReadonlyTID
        {
            get { return this.resTIDColumn.ReadOnly; }
            set
            {
                if (this.resTIDColumn.ReadOnly != value)
                {
                    this.resTagColumn.ReadOnly = value;
                    this.resTIDColumn.ReadOnly = value;
                }
            }
        }

        public bool ReadonlyGID
        {
            get { return this.resGIDColumn.ReadOnly; }
            set 
            {
                if (this.resGIDColumn.ReadOnly != value)
                {
                    this.resGIDColumn.ReadOnly = value;
                }
            }
        }

        public bool ReadonlyIID
        {
            get { return this.resIIDColumn.ReadOnly; }
            set
            {
                if (this.resIIDColumn.ReadOnly != value)
                {
                    this.resIIDColumn.ReadOnly = value;
                }
            }
        }

        public bool ReadonlyCompressed
        {
            get { return this.resCompColumn.ReadOnly; }
            set
            {
                if (this.resCompColumn.ReadOnly != value)
                {
                    this.resCompColumn.ReadOnly = value;
                    this.compressAllCHK.Enabled = !value &&
                        this.resDGV.SelectedRows != null &&
                        this.resDGV.SelectedRows.Count > 0;
                }
            }
        }

        public bool ReadonlyName
        {
            get { return this.resNameColumn.ReadOnly; }
            set
            {
                if (this.resNameColumn.ReadOnly != value)
                {
                    this.resNameColumn.ReadOnly = value;
                }
            }
        }

        public INamedResourceIndexEntry[] SelectedResources
        {
            get
            {
                DataGridViewSelectedRowCollection rows 
                    = this.resDGV.SelectedRows;
                if (rows == null || rows.Count == 0)
                {
                    return null;
                }
                INamedResourceIndexEntry[] ries
                    = new INamedResourceIndexEntry[rows.Count];
                for (int i = 0; i < rows.Count; i++)
                {
                    ries[i] = this.mResources[rows[i].Index];
                }
                return ries;
            }
        }

        public bool CopySelectedRKsToClipboard()
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows == null || rows.Count == 0)
            {
                return false;
            }
            INamedResourceIndexEntry rie;
            if (rows.Count == 1)
            {
                rie = this.mResources[rows[0].Index];
                Clipboard.SetText(string.Format(
                    "0x{0:X8}-0x{1:X8}-0x{2:X16}",
                    rie.ResourceType, rie.ResourceGroup,
                    rie.Instance));
            }
            else
            {
                StringBuilder sb 
                    = new StringBuilder(rows.Count * 42);
                for (int i = 0; i < rows.Count; i++)
                {
                    rie = this.mResources[rows[i].Index];
                    sb.AppendLine(string.Format(
                        "0x{0:X8}-0x{1:X8}-0x{2:X16}",
                        rie.ResourceType, rie.ResourceGroup,
                        rie.Instance));
                }
                Clipboard.SetText(sb.ToString());
            }
            return true;
        }

        private List<TGIBlock> mClipboardRKs;

        private static List<TGIBlock> ParseRKsFromClipboard()
        {
            string str = Clipboard.GetText();
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            string[] rkStrs = str.Split('\n');
            TGIBlock rk;
            List<TGIBlock> rks = new List<TGIBlock>(rkStrs.Length);
            for (int i = 0; i < rkStrs.Length; i++)
            {
                rk = new TGIBlock(0, null);
                if (AResourceKey.TryParse(rkStrs[i], rk))
                {
                    rks.Add(rk);
                }
            }
            return rks;
        }

        private void ClearConflictHighlighting()
        {
            int i;
            DataGridViewCell cell;
            DataGridViewCellCollection cells;
            foreach (DataGridViewRow row in this.resDGV.Rows)
            {
                cells = row.Cells;
                cell = cells[0];
                if (cell.Style.BackColor == Color.Orange)
                {
                    for (i = cells.Count - 1; i >= 0; i--)
                    {
                        cell = cells[i];
                        cell.Style.BackColor = SystemColors.Control;
                    }
                }
            }
        }

        private void HighlightConflicts()
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                int i, j, k;
                bool hasConflicts = false;
                DataGridViewCell cell;
                DataGridViewCellCollection cells;
                INamedResourceIndexEntry rie1, rie2;
                for (i = rows.Count - 1; i >= 0; i--)
                {
                    rie1 = this.mResources[rows[i].Index];
                    for (j = i - 1; j >= 0; j--)
                    {
                        rie2 = this.mResources[rows[j].Index];
                        if (rie1.ResourceType == rie2.ResourceType &&
                            rie1.ResourceGroup == rie2.ResourceGroup &&
                            rie1.Instance == rie2.Instance)
                        {
                            hasConflicts = true;
                            cells = rows[i].Cells;
                            cell = cells[0];
                            if (cell.Style.BackColor != Color.Orange)
                            {
                                for (k = cells.Count; k >= 0; k--)
                                {
                                    cell = cells[k];
                                    cell.Style.BackColor = Color.Orange;
                                }
                            }
                            cells = rows[i].Cells;
                            cell = cells[0];
                            if (cell.Style.BackColor != Color.Orange)
                            {
                                for (k = cells.Count; k >= 0; k--)
                                {
                                    cell = cells[k];
                                    cell.Style.BackColor = Color.Orange;
                                }
                            }
                        }
                    }
                }
                this.okBTN.Enabled = !hasConflicts;
            }
        }

        private void resSelectionChanged(object sender, EventArgs e)
        {
            bool flag = this.resDGV.SelectedRows != null && 
                        this.resDGV.SelectedRows.Count > 0;
            this.copyRKsBTN.Enabled = flag;

            bool flag2 = flag && this.mAutoHashing == Hashing.None;
            this.fnv32BTN.Enabled = flag2;
            this.fnv64BTN.Enabled = flag2;
            this.fnvClipBTN.Enabled = flag2;

            this.compressAllCHK.Enabled 
                = flag && !this.resCompColumn.ReadOnly;

            if (flag && this.bEnablePasteRKs)
            {
                this.mClipboardRKs = ParseRKsFromClipboard();
            }

            int count = 0;
            INamedResourceIndexEntry rie;
            foreach (DataGridViewRow row in this.resDGV.SelectedRows)
            {
                rie = this.mResources[row.Index];
                if (rie.Compressed == 0xFFFF)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                this.compressAllCHK.CheckState = CheckState.Unchecked;
            }
            else if (count < this.resDGV.SelectedRows.Count)
            {
                this.compressAllCHK.CheckState = CheckState.Indeterminate;
            }
            else
            {
                this.compressAllCHK.CheckState = CheckState.Checked;
            }

            this.okBTN.Enabled = flag;
            if (this.bDetectConflicts)
            {
                this.ClearConflictHighlighting();
                this.HighlightConflicts();
            }
        }

        private void copyRKs_Click(object sender, EventArgs e)
        {
            this.CopySelectedRKsToClipboard();
        }

        private void pasteRKs_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0 &&
                this.mClipboardRKs != null && this.mClipboardRKs.Count > 0)
            {
                int index;
                TGIBlock rk;
                INamedResourceIndexEntry rie;
                int count = Math.Min(rows.Count, this.mClipboardRKs.Count);
                for (int i = 0; i < count; i++)
                {
                    index = rows[i].Index;
                    rk = this.mClipboardRKs[i];
                    rie = this.mResources[index];
                    if (!this.resTIDColumn.ReadOnly)
                    {
                        rie.ResourceType = rk.ResourceType;
                        this.resDGV.UpdateCellValue(
                            (int)ColumnName.Tag, index);
                        this.resDGV.UpdateCellValue(
                            (int)ColumnName.TID, index);
                    }
                    if (!this.resGIDColumn.ReadOnly)
                    {
                        rie.ResourceGroup = rk.ResourceGroup;
                        this.resDGV.UpdateCellValue(
                            (int)ColumnName.GID, index);
                    }
                    if (!this.resIIDColumn.ReadOnly)
                    {
                        rie.Instance = rk.Instance;
                        this.resDGV.UpdateCellValue(
                            (int)ColumnName.IID, index);
                    }
                }
                if (this.bDetectConflicts)
                {
                    this.ClearConflictHighlighting();
                    this.HighlightConflicts();
                }
                switch ((ColumnName)this.mColIndex)
                {
                    case ColumnName.Tag:
                    case ColumnName.TID:
                    case ColumnName.GID:
                    case ColumnName.IID:
                        this.UpdateResDGV();
                        break;
                }
            }
        }

        private void compressAll_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                ushort comp;
                if (this.compressAllCHK.CheckState == CheckState.Unchecked)
                {
                    this.compressAllCHK.CheckState = CheckState.Checked;
                    comp = 0xFFFF;
                }
                else
                {
                    this.compressAllCHK.CheckState = CheckState.Unchecked;
                    comp = 0x0000;
                }
                INamedResourceIndexEntry rie;
                foreach (DataGridViewRow row in rows)
                {
                    rie = this.mResources[row.Index];
                    rie.Compressed = comp;
                    this.resDGV.UpdateCellValue(
                        (int)ColumnName.Comp, row.Index);
                }
                if (this.mColIndex == (int)ColumnName.Comp)
                {
                    this.UpdateResDGV();
                }
            }
        }

        private void fnv32_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                INamedResourceIndexEntry rie;
                foreach (DataGridViewRow row in rows)
                {
                    rie = this.mResources[row.Index];
                    rie.Instance = FNVHash.HashString32(rie.ResourceName);
                    this.resDGV.UpdateCellValue(
                        (int)ColumnName.IID, row.Index);
                }
                if (this.bDetectConflicts)
                {
                    this.ClearConflictHighlighting();
                    this.HighlightConflicts();
                }
                if (this.mColIndex == (int)ColumnName.IID)
                {
                    this.UpdateResDGV();
                }
            }
        }

        private void fnv64_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                INamedResourceIndexEntry rie;
                foreach (DataGridViewRow row in rows)
                {
                    rie = this.mResources[row.Index];
                    rie.Instance = FNVHash.HashString64(rie.ResourceName);
                    this.resDGV.UpdateCellValue(
                        (int)ColumnName.IID, row.Index);
                }
                if (this.bDetectConflicts)
                {
                    this.ClearConflictHighlighting();
                    this.HighlightConflicts();
                }
                if (this.mColIndex == (int)ColumnName.IID)
                {
                    this.UpdateResDGV();
                }
            }
        }

        private void fnvClip_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows
                    = this.resDGV.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                INamedResourceIndexEntry rie;
                foreach (DataGridViewRow row in rows)
                {
                    rie = this.mResources[row.Index];
                    rie.Instance = FNVCLIP.HashString(rie.ResourceName);
                    this.resDGV.UpdateCellValue(
                        (int)ColumnName.IID, row.Index);
                }
                if (this.bDetectConflicts)
                {
                    this.ClearConflictHighlighting();
                    this.HighlightConflicts();
                }
                if (this.mColIndex == (int)ColumnName.IID)
                {
                    this.UpdateResDGV();
                }
            }
        }

        private bool CheckResDGVSorted(int rowIndex)
        {
            DataGridViewColumnHeaderCell hc
                = this.resDGV.Columns[this.mColIndex].HeaderCell;
            if (hc.SortGlyphDirection == SortOrder.None)
            {
                return true;
            }
            int num = 0;
            INamedResourceIndexEntry x, y;
            if (rowIndex > 0)
            {
                x = this.mResources[rowIndex - 1];
                y = this.mResources[rowIndex];
                switch ((ColumnName)this.mColIndex)
                {
                    case ColumnName.Tag:
                        num = sTagComparer.Compare(x, y);
                        break;
                    case ColumnName.TID:
                        num = sTIDComparer.Compare(x, y);
                        break;
                    case ColumnName.GID:
                        num = sGIDComparer.Compare(x, y);
                        break;
                    case ColumnName.IID:
                        num = sIIDComparer.Compare(x, y);
                        break;
                    case ColumnName.Comp:
                        num = sCompComparer.Compare(x, y);
                        break;
                    case ColumnName.Name:
                        num = sNameComparer.Compare(x, y);
                        break;
                }
                if (hc.SortGlyphDirection == SortOrder.Descending)
                {
                    num = -num;
                }
                if (num < 0)
                {
                    return false;
                }
            }
            if (rowIndex < (this.mResources.Length - 1))
            {
                x = this.mResources[rowIndex];
                y = this.mResources[rowIndex + 1];
                switch ((ColumnName)this.mColIndex)
                {
                    case ColumnName.Tag:
                        num = sTagComparer.Compare(x, y);
                        break;
                    case ColumnName.TID:
                        num = sTIDComparer.Compare(x, y);
                        break;
                    case ColumnName.GID:
                        num = sGIDComparer.Compare(x, y);
                        break;
                    case ColumnName.IID:
                        num = sIIDComparer.Compare(x, y);
                        break;
                    case ColumnName.Comp:
                        num = sCompComparer.Compare(x, y);
                        break;
                    case ColumnName.Name:
                        num = sNameComparer.Compare(x, y);
                        break;
                }
                if (hc.SortGlyphDirection == SortOrder.Descending)
                {
                    num = -num;
                }
                if (num < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateResDGV()
        {
            switch ((ColumnName)this.mColIndex)
            {
                case ColumnName.Tag:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sTagComparer);
                    break;
                case ColumnName.TID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sTIDComparer);
                    break;
                case ColumnName.GID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sGIDComparer);
                    break;
                case ColumnName.IID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sIIDComparer);
                    break;
                case ColumnName.Comp:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sCompComparer);
                    break;
                case ColumnName.Name:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sNameComparer);
                    break;
            }
            DataGridViewColumnHeaderCell hc
                = this.resDGV.Columns[this.mColIndex].HeaderCell;
            if (hc.SortGlyphDirection == SortOrder.Descending)
            {
                Array.Reverse(this.mResources, 0, this.mResources.Length);
            }
            this.resDGV.RowCount = this.mResources.Length;
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
            switch ((ColumnName)this.mColIndex)
            {
                case ColumnName.Tag:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sTagComparer);
                    break;
                case ColumnName.TID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sTIDComparer);
                    break;
                case ColumnName.GID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sGIDComparer);
                    break;
                case ColumnName.IID:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sIIDComparer);
                    break;
                case ColumnName.Comp:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sCompComparer);
                    break;
                case ColumnName.Name:
                    Array.Sort(this.mResources, 0,
                        this.mResources.Length, sNameComparer);
                    break;
            }
            if (hc.SortGlyphDirection == SortOrder.Descending)
            {
                Array.Reverse(this.mResources, 0, this.mResources.Length);
            }
            this.resDGV.Invalidate();
        }

        private void resCellValueNeeded(object sender, 
            DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < this.mResources.Length)
            {
                string str;
                INamedResourceIndexEntry rie = this.mResources[e.RowIndex];
                switch ((ColumnName)e.ColumnIndex)
                {
                    case ColumnName.Tag:
                        str = "0x" + rie.ResourceType.ToString("X8");
                        if (ExtList.Ext.ContainsKey(str))
                        {
                            e.Value = ExtList.Ext[str][0] + " " + str;
                        }
                        else
                        {
                            e.Value = "";
                        }
                        break;
                    case ColumnName.TID:
                        e.Value = "0x" + rie.ResourceType.ToString("X8");
                        break;
                    case ColumnName.GID:
                        e.Value = "0x" + rie.ResourceGroup.ToString("X8");
                        break;
                    case ColumnName.IID:
                        e.Value = "0x" + rie.Instance.ToString("X16");
                        break;
                    case ColumnName.Comp:
                        e.Value = rie.Compressed == 0xFFFF;
                        break;
                    case ColumnName.Name:
                        e.Value = rie.ResourceName;
                        break;
                }
            }
        }

        private void resCellValidating(object sender, 
            DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < this.mResources.Length)
            {
                uint val32;
                ulong val64;
                DataGridViewRow row = this.resDGV.Rows[e.RowIndex];
                row.ErrorText = "";
                string str = e.FormattedValue.ToString();
                switch ((ColumnName)e.ColumnIndex)
                {
                    case ColumnName.TID:
                        if (str.StartsWith("0x") && !uint.TryParse(
                            str.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out val32))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Type ID " +
                                "must be a 32-bit unsigned integer";
                        }
                        else if (!uint.TryParse(str, out val32))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Type ID " +
                                "must be a 32-bit unsigned integer";
                        }
                        break;
                    case ColumnName.GID:
                        if (str.StartsWith("0x") && !uint.TryParse(
                            str.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out val32))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Group ID " +
                                "must be a 32-bit unsigned integer";
                        }
                        else if (!uint.TryParse(str, out val32))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Group ID " +
                                "must be a 32-bit unsigned integer";
                        }
                        break;
                    case ColumnName.IID:
                        if (str.StartsWith("0x") && !ulong.TryParse(
                            str.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out val64))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Type ID " +
                                "must be a 64-bit unsigned integer";
                        }
                        else if (!ulong.TryParse(str, out val64))
                        {
                            e.Cancel = true;
                            row.ErrorText = "Type ID " +
                                "must be a 64-bit unsigned integer";
                        }
                        break;
                }
            }
        }

        private void resCellValuePushed(object sender, 
            DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < this.mResources.Length)
            {
                string str;
                uint val32;
                ulong val64;
                INamedResourceIndexEntry rie = this.mResources[e.RowIndex];
                switch ((ColumnName)e.ColumnIndex)
                {
                    case ColumnName.Tag:
                        string[] strs = e.Value.ToString().Split(' ');
                        val32 = uint.Parse(strs[1].Substring(2), 
                            System.Globalization.NumberStyles.HexNumber);
                        rie.ResourceType = val32;
                        if (this.mColIndex == (int)ColumnName.Tag ||
                            this.mColIndex == (int)ColumnName.TID)
                        {
                            if (this.CheckResDGVSorted(e.RowIndex))
                            {
                                this.resDGV.UpdateCellValue(
                                    (int)ColumnName.TID, e.RowIndex);
                            }
                            else
                            {
                                this.UpdateResDGV();
                            }
                        }
                        else
                        {
                            this.resDGV.UpdateCellValue(
                                (int)ColumnName.TID, e.RowIndex);
                        }
                        if (this.bDetectConflicts)
                        {
                            this.ClearConflictHighlighting();
                            this.HighlightConflicts();
                        }
                        break;
                    case ColumnName.TID:
                        str = e.Value.ToString();
                        if (str.StartsWith("0x"))
                        {
                            val32 = uint.Parse(str.Substring(2),
                                System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            val32 = uint.Parse(str);
                        }
                        rie.ResourceType = val32;
                        if (this.mColIndex == (int)ColumnName.Tag ||
                            this.mColIndex == (int)ColumnName.TID)
                        {
                            if (this.CheckResDGVSorted(e.RowIndex))
                            {
                                this.resDGV.UpdateCellValue(
                                    (int)ColumnName.Tag, e.RowIndex);
                            }
                            else
                            {
                                this.UpdateResDGV();
                            }
                        }
                        else
                        {
                            this.resDGV.UpdateCellValue(
                                (int)ColumnName.Tag, e.RowIndex);
                        }
                        if (this.bDetectConflicts)
                        {
                            this.ClearConflictHighlighting();
                            this.HighlightConflicts();
                        }
                        break;
                    case ColumnName.GID:
                        str = e.Value.ToString();
                        if (str.StartsWith("0x"))
                        {
                            val32 = uint.Parse(str.Substring(2),
                                System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            val32 = uint.Parse(str);
                        }
                        rie.ResourceGroup = val32;
                        if (this.mColIndex == (int)ColumnName.GID &&
                            !this.CheckResDGVSorted(e.RowIndex))
                        {
                            this.UpdateResDGV();
                        }
                        if (this.bDetectConflicts)
                        {
                            this.ClearConflictHighlighting();
                            this.HighlightConflicts();
                        }
                        break;
                    case ColumnName.IID:
                        str = e.Value.ToString();
                        if (str.StartsWith("0x"))
                        {
                            val64 = ulong.Parse(str.Substring(2),
                                System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            val64 = ulong.Parse(str);
                        }
                        rie.Instance = val64;
                        if (this.mColIndex == (int)ColumnName.IID &&
                            !this.CheckResDGVSorted(e.RowIndex))
                        {
                            this.UpdateResDGV();
                        }
                        if (this.bDetectConflicts)
                        {
                            this.ClearConflictHighlighting();
                            this.HighlightConflicts();
                        }
                        break;
                    case ColumnName.Comp:
                        bool comp = (bool)e.Value;
                        rie.Compressed = (ushort)(comp ? 0xFFFF : 0x0000);
                        if (this.mColIndex == (int)ColumnName.Comp &&
                            !this.CheckResDGVSorted(e.RowIndex))
                        {
                            this.UpdateResDGV();
                        }
                        break;
                    case ColumnName.Name:
                        str = e.Value.ToString();
                        rie.ResourceName = str;
                        if (this.mAutoHashing != Hashing.None)
                        {
                            ulong iid = 0;
                            if (!str.StartsWith("0x") || 
                                !ulong.TryParse(str.Substring(2), 
                                System.Globalization.NumberStyles.HexNumber,
                                null, out iid))
                            {
                                switch (this.mAutoHashing)
                                {
                                    case Hashing.FNV32:
                                        iid = FNVHash.HashString32(str);
                                        break;
                                    case Hashing.FNV64:
                                        iid = FNVHash.HashString64(str);
                                        break;
                                    case Hashing.FNVCLIP:
                                        iid = FNVCLIP.HashString(str);
                                        break;
                                }
                            }
                            rie.Instance = iid;
                            if (this.mColIndex == (int)ColumnName.IID ||
                                this.mColIndex == (int)ColumnName.Name)
                            {
                                if (this.CheckResDGVSorted(e.RowIndex))
                                {
                                    this.resDGV.UpdateCellValue(
                                        (int)ColumnName.IID, e.RowIndex);
                                }
                                else
                                {
                                    this.UpdateResDGV();
                                }
                            }
                            else
                            {
                                this.resDGV.UpdateCellValue(
                                    (int)ColumnName.IID, e.RowIndex);
                            }
                            if (this.bDetectConflicts)
                            {
                                this.ClearConflictHighlighting();
                                this.HighlightConflicts();
                            }
                        }
                        if (this.mColIndex == (int)ColumnName.Name &&
                            !this.CheckResDGVSorted(e.RowIndex))
                        {
                            this.UpdateResDGV();
                        }
                        break;
                }
            }
        }
    }
}
