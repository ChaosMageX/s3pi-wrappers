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
        public MainForm()
        {
            InitializeComponent();
        }

        private FNVSearchTable searchTable = FNVSearchTable.EnglishAlphabet;
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
            else if (unhashCmbStr == "32")
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
                    if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (uint.TryParse(input, System.Globalization.NumberStyles.HexNumber,
                        System.Globalization.CultureInfo.CurrentCulture, out hash))
                    {
                        int maxChars = (int)this.maxCharsNUM.Value;
                        int maxMatches = (int)this.maxMatchesNUM.Value;
                        this.unhasher32 = new FNVUnhasher32(hash, this.searchTable, maxChars, maxMatches);
                        this.prevResultCount = 0;
                        this.unhasher32.Start();
                        this.updateTimer.Start();
                    }
                }
            }
            else if (unhashCmbStr == "64")
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
                    if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (ulong.TryParse(input, System.Globalization.NumberStyles.HexNumber,
                        System.Globalization.CultureInfo.CurrentCulture, out hash))
                    {
                        int maxChars = (int)this.maxCharsNUM.Value;
                        int maxMatches = (int)this.maxMatchesNUM.Value;
                        this.unhasher64 = new FNVUnhasher64(hash, this.searchTable, maxChars, maxMatches);
                        this.prevResultCount = 0;
                        this.unhasher64.Start();
                        this.updateTimer.Start();
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
                ulong iterations = this.unhasher32.Iterations;
                int increment = (int)((double)iterations / (double)this.unhasher32.MaxIterations * 1000000.0);
                this.unHashingProgress.Value = increment;
                if (this.unhasher32.Finished)
                {
                    this.updateTimer.Stop();
                    this.resultsTXT.Lines = this.unhasher32.Results;
                    this.endTimesTXT.Lines = this.unhasher32.ElapsedTimeStrings;
                }
                if (this.unhasher32.ResultCount > this.prevResultCount)
                {
                    this.resultsTXT.Lines = this.unhasher32.Results;
                    this.endTimesTXT.Lines = this.unhasher32.ElapsedTimeStrings;
                    this.prevResultCount = this.unhasher32.ResultCount;
                    this.matchCountTXT.Text = string.Concat("Matches: ", this.prevResultCount.ToString());
                }
                this.iterationsTXT.Text = string.Concat("Iterations: ", iterations.ToString("##,#"));
            }
            if (this.unhasher64 != null)
            {
                ulong iterations = this.unhasher64.Iterations;
                int increment = (int)((double)iterations / (double)this.unhasher64.MaxIterations * 1000000.0);
                this.unHashingProgress.Value = increment;
                if (this.unhasher64.Finished)
                {
                    this.updateTimer.Stop();
                    this.resultsTXT.Lines = this.unhasher64.Results;
                    this.endTimesTXT.Lines = this.unhasher64.ElapsedTimeStrings;
                }
                if (this.unhasher64.ResultCount > this.prevResultCount)
                {
                    this.resultsTXT.Lines = this.unhasher64.Results;
                    this.endTimesTXT.Lines = this.unhasher64.ElapsedTimeStrings;
                    this.prevResultCount = this.unhasher64.ResultCount;
                    this.matchCountTXT.Text = string.Concat("Matches: ", this.prevResultCount.ToString());
                }
                this.iterationsTXT.Text = string.Concat("Iterations: ", iterations.ToString("##,#"));
            }
        }

        private void settings_Click(object sender, EventArgs e)
        {
            using (SettingsDialog sDialog = new SettingsDialog(this.searchTable))
            {
                DialogResult result = sDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.searchTable = sDialog.SearchTable;
                }
            }
        }
    }
}
