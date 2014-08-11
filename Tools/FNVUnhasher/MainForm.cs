using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Helpers.Cryptography;

namespace FNVUnhasher
{
    public partial class MainForm : Form
    {
        private const string kXor32 = "FNV 24 (Xor 32)";
        private const string kFNV32 = "FNV 32";
        private const string kXor64 = "FNV 48 (Xor 64)";
        private const string kFNV64 = "FNV 64";

        public MainForm()
        {
            InitializeComponent();
            
            this.unhashCmb.Items.Clear();
            this.unhashCmb.Items.AddRange(new object[] {
            kFNV32, kFNV64, kXor32, kXor64 });

            ContextMenuStrip resultsMenu = new ContextMenuStrip();
            ToolStripItemCollection resultsMenuItems = resultsMenu.Items;

            ToolStripMenuItem copyResultsTSMI = new ToolStripMenuItem("Copy Results");
            copyResultsTSMI.Click += new EventHandler(copyResults_Click);
            resultsMenuItems.Add(copyResultsTSMI);

            ToolStripMenuItem copyResultWordsTSMI = new ToolStripMenuItem("Copy Result Words");
            copyResultWordsTSMI.Click += new EventHandler(copyResultWords_Click);
            resultsMenuItems.Add(copyResultWordsTSMI);

            ToolStripMenuItem copyResultTimesTSMI = new ToolStripMenuItem("Copy Result Times");
            copyResultTimesTSMI.Click += new EventHandler(copyResultTimes_Click);
            resultsMenuItems.Add(copyResultTimesTSMI);

            ToolStripMenuItem copyResultItersTSMI = new ToolStripMenuItem("Copy Result Iters");
            copyResultItersTSMI.Click += new EventHandler(copyResultIters_Click);
            resultsMenuItems.Add(copyResultItersTSMI);

            ToolStripSeparator separator1 = new ToolStripSeparator();
            resultsMenuItems.Add(separator1);

            ToolStripMenuItem selectAllResultsTSMI = new ToolStripMenuItem("Select All Results");
            selectAllResultsTSMI.Click += new EventHandler(selectAllResults_Click);
            resultsMenuItems.Add(selectAllResultsTSMI);
            
            this.resultsLST.ContextMenuStrip = resultsMenu;

            ContextMenuStrip notifyMenu = new ContextMenuStrip();
            ToolStripMenuItem showMainFormTSMI = new ToolStripMenuItem("Show FNV Unhasher");
            showMainFormTSMI.Click += new EventHandler(showMainFormTSMI_Click);
            notifyMenu.Items.Add(showMainFormTSMI);
            this.notifyIcon.ContextMenuStrip = notifyMenu;
        }

        private void showMainFormTSMI_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private FNVSearchTable searchTable = FNVSearchTable.EnglishAlphabet;
        private uint filter32 = uint.MaxValue;
        private ulong filter64 = ulong.MaxValue;
        private FNVUnhasher32 unhasher32;
        private FNVUnhasher64 unhasher64;
        private int prevResultCount;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.unhasher32 != null)
            {
                this.unhasher32.Stop();
                this.unhasher32 = null;
            }
            if (this.unhasher64 != null)
            {
                this.unhasher64.Stop();
                this.unhasher64 = null;
            }
            base.OnClosing(e);
        }

        private void hash_Click(object sender, EventArgs e)
        {
            if (this.unhasher32 != null && !this.unhasher32.Finished)
                return;
            if (this.unhasher64 != null && !this.unhasher64.Finished)
                return;
            string strToHash = this.inputTxt.Text;
            uint xor32Hash  = FNVHash.HashString24(strToHash) & filter32;
            uint fnv32Hash  = FNVHash.HashString32(strToHash) & filter32;
            ulong xor64Hash = FNVHash.HashString48(strToHash) & filter64;
            ulong fnv64Hash = FNVHash.HashString64(strToHash) & filter64;
            this.resultsLST.Items.AddRange(new object[] 
            {
                string.Concat("FNV Hash of ", strToHash),
                string.Concat("0x", fnv32Hash.ToString("X8"), "         | FNV 32"),
                string.Concat("0x", fnv64Hash.ToString("X16"), " | FNV 64"),
                string.Concat("0x", xor32Hash.ToString("X8"), "         | Xor Folded FNV 32"),
                string.Concat("0x", xor64Hash.ToString("X16"), " | Xor Folded FNV 64")
            });
        }

        private void unhash_Click(object sender, EventArgs e)
        {
            string unhashCmbStr = unhashCmb.SelectedItem as string;
            if (this.unhasher32 != null && !this.unhasher32.Finished)
                return;
            if (this.unhasher64 != null && !this.unhasher64.Finished)
                return;
            if (unhashCmbStr == null)
                MessageBox.Show("No Unhashing Algorithm selected", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (unhashCmbStr.Equals(kFNV32) || unhashCmbStr.Equals(kXor32))
            {
                if (this.unhasher64 != null)
                {
                    this.unhasher64.Stop();
                    this.unhasher64 = null;
                }
                if (this.unhasher32 == null || this.unhasher32.Finished)
                {
                    uint hash;
                    string input = this.inputTxt.Text;
                    if (input.StartsWith("0x"))
                        input = input.Substring(2);
                    else if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (uint.TryParse(input, System.Globalization.NumberStyles.HexNumber,
                        System.Globalization.CultureInfo.CurrentCulture, out hash))
                    {
                        bool xorFold = !unhashCmbStr.Equals(kFNV32);
                        if (xorFold && (hash & filter32) > 0xffffffU)
                        {
                            MessageBox.Show("Xor 32 will never find matches for 0x" + (hash & filter32).ToString("X8")
                                + ".\nIt is greater than 24 bits in length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            this.resultsLST.Items.Add("Unhash results for 0x" + hash.ToString("X8"));
                            int maxChars = (int)this.maxCharsNUM.Value;
                            int maxMatches = (int)this.maxMatchesNUM.Value;
                            this.unhasher32 = new FNVUnhasher32(hash, this.searchTable, maxChars, maxMatches, xorFold, filter32);
                            this.prevResultCount = 0;
                            this.unhasher32.Start();
                            this.updateTimer.Start();
                        }
                    }
                }
            }
            else if (unhashCmbStr.Equals(kFNV64) || unhashCmbStr.Equals(kXor64))
            {
                if (this.unhasher32 != null)
                {
                    this.unhasher32.Stop();
                    this.unhasher32 = null;
                }
                if (this.unhasher64 == null || this.unhasher64.Finished)
                {
                    ulong hash;
                    string input = this.inputTxt.Text;
                    if (input.StartsWith("0x"))
                        input = input.Substring(2);
                    else if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (ulong.TryParse(input, System.Globalization.NumberStyles.HexNumber,
                        System.Globalization.CultureInfo.CurrentCulture, out hash))
                    {
                        bool xorFold = !unhashCmbStr.Equals(kFNV64);
                        if (xorFold && (hash & filter64) > 0xffffffffffffUL)
                        {
                            MessageBox.Show("Xor 64 will never find matches for 0x" + (hash & filter64).ToString("X16")
                                + ".\nIt is greater than 48 bits in length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            this.resultsLST.Items.Add("Unhash results for 0x" + hash.ToString("X16"));
                            int maxChars = (int)this.maxCharsNUM.Value;
                            int maxMatches = (int)this.maxMatchesNUM.Value;
                            this.unhasher64 = new FNVUnhasher64(hash, this.searchTable, maxChars, maxMatches, xorFold, filter64);
                            this.prevResultCount = 0;
                            this.unhasher64.Start();
                            this.updateTimer.Start();
                        }
                    }
                }
            }
        }

        private void stopUnhash_Click(object sender, EventArgs e)
        {
            if (this.unhasher32 != null)
            {
                this.unhasher32.Stop();
                this.unhasher32 = null;
            }
            if (this.unhasher64 != null)
            {
                this.unhasher64.Stop();
                this.unhasher64 = null;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.unhasher32 != null)
            {
                this.UpdateUI(this.unhasher32);
            }
            if (this.unhasher64 != null)
            {
                this.UpdateUI(this.unhasher64);
            }
        }

        private void UpdateUI(IFNVUnhasher unhasher)
        {
            ulong iterations = unhasher.Iterations;
            int increment = (int)((double)iterations / (double)unhasher.MaxIterations * 1000000.0);
            this.unHashingProgress.Value = increment;
            if (unhasher.Finished)
            {
                this.updateTimer.Stop();
                //this.resultsTXT.Lines = unhasher.Results;
                //this.endTimesTXT.Lines = unhasher.ElapsedTimeStrings;
            }
            increment = unhasher.ResultCount;
            string matchStr = string.Concat("Matches: ", increment.ToString());
            if (increment > this.prevResultCount)
            {
                //this.resultsTXT.Lines = unhasher.Results;
                //this.endTimesTXT.Lines = unhasher.ElapsedTimeStrings;
                int numPadding = unhasher.MaxResultCount.ToString().Length;
                int wordPadding = unhasher.MaxResultCharCount
                    + unhasher.PrefixLength + unhasher.SuffixLength;
                string[] words = unhasher.Results;
                string[] times = unhasher.ElapsedTimeAtResultStrings;
                string[] iters = unhasher.IterationsAtResultStrings;
                for (int i = this.prevResultCount; i < increment; i++)
                {
                    this.resultsLST.Items.Add(string.Join(" | ", new string[] {
                            (i + 1).ToString().PadLeft(numPadding),
                            words[i].PadRight(wordPadding),
                            times[i].PadLeft(20), // ddd.hh:mm:ss.sssssss
                            iters[i].PadLeft(23) }, 0, 4)); // 999 quadrillion iterations
                }
                this.matchCountTXT.Text = matchStr;
                this.prevResultCount = increment;
            }
            string timeStr = string.Concat("Elapsed Time: ", unhasher.ElapsedTime.ToString());
            this.elapsedTimeTXT.Text = timeStr;
            string iterStr = string.Concat("Iterations: ", iterations.ToString("##,#"));
            this.iterationsTXT.Text = iterStr;
            this.notifyIcon.BalloonTipText = string.Join(Environment.NewLine, new string[] { matchStr, timeStr, iterStr }, 0, 3);
        }

        private void settings_Click(object sender, EventArgs e)
        {
            using (SettingsDialog sDialog = new SettingsDialog(searchTable, filter32, filter64))
            {
                DialogResult result = sDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.searchTable = sDialog.SearchTable;
                    this.filter32 = sDialog.Filter32;
                    this.filter64 = sDialog.Filter64;
                }
            }
        }

        private void copyResults_Click(object sender, EventArgs e)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (object item in this.resultsLST.SelectedItems)
            {
                sb.AppendLine(item.ToString());
                i++;
            }
            if (i > 0)
            {
                Clipboard.SetText(sb.ToString());
            }/* */
        }

        private void copyResultWords_Click(object sender, EventArgs e)
        {
            /*if (this.unhasher32 != null)
            {
                string[] data = this.unhasher32.Results;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }
            if (this.unhasher64 != null)
            {
                string[] data = this.unhasher64.Results;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }/* */
            int i = 0;
            string str;
            string[] strArray;
            char[] sep = new char[] { '|' };
            StringBuilder sb = new StringBuilder();
            foreach (object item in this.resultsLST.SelectedItems)
            {
                str = item.ToString();
                strArray = str.Split(sep, 4, StringSplitOptions.None);
                if (strArray.Length > 0)
                {
                    sb.AppendLine(strArray.Length > 1 ? strArray[1].Trim() : strArray[0]);
                    i++;
                }
            }
            if (i > 0)
            {
                Clipboard.SetText(sb.ToString());
            }/* */
        }

        private void copyResultTimes_Click(object sender, EventArgs e)
        {
            /*if (this.unhasher32 != null)
            {
                string[] data = this.unhasher32.ElapsedTimeStrings;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }
            if (this.unhasher64 != null)
            {
                string[] data = this.unhasher64.ElapsedTimeStrings;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }/* */
            int i = 0;
            string str;
            string[] strArray;
            char[] sep = new char[] { '|' };
            StringBuilder sb = new StringBuilder();
            foreach (object item in this.resultsLST.SelectedItems)
            {
                str = item.ToString();
                strArray = str.Split(sep, 4, StringSplitOptions.None);
                if (strArray.Length > 0)
                {
                    sb.AppendLine(strArray.Length > 2 ? strArray[2] : strArray[0]);
                    i++;
                }
            }
            if (i > 0)
            {
                Clipboard.SetText(sb.ToString());
            }/* */
        }

        private void copyResultIters_Click(object sender, EventArgs e)
        {
            /*if (this.unhasher32 != null)
            {
                string[] data = this.unhasher32.IterationsAtResultStrings;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }
            if (this.unhasher64 != null)
            {
                string[] data = this.unhasher64.IterationsAtResultStrings;
                Clipboard.SetText(string.Join(Environment.NewLine, data, 0, data.Length));
            }/* */
            int i = 0;
            string str;
            string[] strArray;
            char[] sep = new char[] { '|' };
            StringBuilder sb = new StringBuilder();
            foreach (object item in this.resultsLST.SelectedItems)
            {
                str = item.ToString();
                strArray = str.Split(sep, 4, StringSplitOptions.None);
                if (strArray.Length > 0)
                {
                    sb.AppendLine(strArray.Length > 3 ? strArray[3] : strArray[0]);
                    i++;
                }
            }
            if (i > 0)
            {
                Clipboard.SetText(sb.ToString());
            }/* */
        }

        private void removeResults_Click(object sender, EventArgs e)
        {
            ListBox.ObjectCollection items = this.resultsLST.Items;
            ListBox.SelectedObjectCollection selectedItems = this.resultsLST.SelectedItems;
            object[] itemsToRemove = new object[selectedItems.Count];
            selectedItems.CopyTo(itemsToRemove, 0);
            for (int i = itemsToRemove.Length - 1; i >= 0; i--)
            {
                items.Remove(itemsToRemove[i]);
            }
        }

        private void clearResults_Click(object sender, EventArgs e)
        {
            this.resultsLST.Items.Clear();
        }

        private void selectAllResults_Click(object sender, EventArgs e)
        {
            for (int i = this.resultsLST.Items.Count - 1; i >= 0; i--)
            {
                this.resultsLST.SetSelected(i, true);
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
