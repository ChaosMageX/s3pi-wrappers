using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using s3piwrappers.Helpers.Cryptography;

namespace FNVUnhasher
{
    public partial class MainForm : Form
    {
        private const string kXor32 = "24 (Xor 32)";
        private const string kFNV32 = "32";
        private const string kXor64 = "48 (Xor 64)";
        private const string kFNV64 = "64";

        public MainForm()
        {
            InitializeComponent();
            unhashCmb.Items.Clear();
            unhashCmb.Items.AddRange(new object[]
                {
                    kFNV32, kFNV64, kXor32, kXor64
                });
        }

        private FNVSearchTable searchTable = FNVSearchTable.EnglishAlphabet;
        private uint filter32 = uint.MaxValue;
        private ulong filter64 = ulong.MaxValue;
        private FNVUnhasher32 unhasher32;
        private FNVUnhasher64 unhasher64;
        private int prevResultCount;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (unhasher32 != null)
            {
                unhasher32.Stop();
                unhasher32 = null;
            }
            if (unhasher64 != null)
            {
                unhasher64.Stop();
                unhasher64 = null;
            }
            base.OnClosing(e);
        }

        private void hash_Click(object sender, EventArgs e)
        {
            if (unhasher32 != null && !unhasher32.Finished)
                return;
            if (unhasher64 != null && !unhasher64.Finished)
                return;
            string strToHash = inputTxt.Text;
            uint xor32Hash = FNVHash.HashString24(strToHash) & filter32;
            uint fnv32Hash = FNVHash.HashString32(strToHash) & filter32;
            ulong xor64Hash = FNVHash.HashString48(strToHash) & filter64;
            ulong fnv64Hash = FNVHash.HashString64(strToHash) & filter64;
            resultsTXT.Lines = new[]
                {
                    string.Concat("0x", fnv32Hash.ToString("X8")),
                    string.Concat("0x", fnv64Hash.ToString("X16")),
                    string.Concat("0x", xor32Hash.ToString("X8")),
                    string.Concat("0x", xor64Hash.ToString("X16"))
                };
            endTimesTXT.Lines = new[]
                {
                    "FNV 32",
                    "FNV 64",
                    "Xor Folded FNV 32",
                    "Xor Folded FNV 64"
                };
        }

        private void unhash_Click(object sender, EventArgs e)
        {
            var unhashCmbStr = unhashCmb.SelectedItem as string;
            if (unhasher32 != null && !unhasher32.Finished)
                return;
            if (unhasher64 != null && !unhasher64.Finished)
                return;
            if (unhashCmbStr == null)
                MessageBox.Show("No Unhashing Algorithm selected", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (unhashCmbStr.Equals(kFNV32) || unhashCmbStr.Equals(kXor32))
            {
                if (unhasher64 != null)
                {
                    unhasher64.Stop();
                    unhasher64 = null;
                }
                if (unhasher32 == null || unhasher32.Finished)
                {
                    uint hash;
                    string input = inputTxt.Text;
                    if (input.StartsWith("0x"))
                        input = input.Substring(2);
                    if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (uint.TryParse(input, NumberStyles.HexNumber,
                                      CultureInfo.CurrentCulture, out hash))
                    {
                        bool xorFold = !unhashCmbStr.Equals(kFNV32);
                        if (xorFold && (hash & filter32) > 0xffffffU)
                            MessageBox.Show("Xor 32 will never find matches for 0x" + (hash & filter32).ToString("X8")
                                + ".\nIt is greater than 24 bits in length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            var maxChars = (int) maxCharsNUM.Value;
                            var maxMatches = (int) maxMatchesNUM.Value;
                            unhasher32 = new FNVUnhasher32(hash, searchTable, maxChars, maxMatches, xorFold, filter32);
                            prevResultCount = 0;
                            unhasher32.Start();
                            updateTimer.Start();
                        }
                    }
                }
            }
            else if (unhashCmbStr.Equals(kFNV64) || unhashCmbStr.Equals(kXor64))
            {
                if (unhasher32 != null)
                {
                    unhasher32.Stop();
                    unhasher32 = null;
                }
                if (unhasher64 == null || unhasher64.Finished)
                {
                    ulong hash;
                    string input = inputTxt.Text;
                    if (input.StartsWith("0x"))
                        input = input.Substring(2);
                    if (input.StartsWith("0X"))
                        input = input.Substring(2);
                    if (ulong.TryParse(input, NumberStyles.HexNumber,
                                       CultureInfo.CurrentCulture, out hash))
                    {
                        bool xorFold = !unhashCmbStr.Equals(kFNV64);
                        if (xorFold && (hash & filter64) > 0xffffffffffffUL)
                            MessageBox.Show("Xor 64 will never find matches for 0x" + (hash & filter64).ToString("X16")
                                + ".\nIt is greater than 48 bits in length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        var maxChars = (int) maxCharsNUM.Value;
                        var maxMatches = (int) maxMatchesNUM.Value;
                        unhasher64 = new FNVUnhasher64(hash, searchTable, maxChars, maxMatches, xorFold, filter64);
                        prevResultCount = 0;
                        unhasher64.Start();
                        updateTimer.Start();
                    }
                }
            }
        }

        private void stopUnhash_Click(object sender, EventArgs e)
        {
            if (unhasher32 != null)
            {
                unhasher32.Stop();
                unhasher32 = null;
            }
            if (unhasher64 != null)
            {
                unhasher64.Stop();
                unhasher64 = null;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (unhasher32 != null)
            {
                ulong iterations = unhasher32.Iterations;
                var increment = (int) (iterations/(double) unhasher32.MaxIterations*1000000.0);
                unHashingProgress.Value = increment;
                if (unhasher32.Finished)
                {
                    updateTimer.Stop();
                    resultsTXT.Lines = unhasher32.Results;
                    endTimesTXT.Lines = unhasher32.ElapsedTimeStrings;
                }
                if (unhasher32.ResultCount > prevResultCount)
                {
                    resultsTXT.Lines = unhasher32.Results;
                    endTimesTXT.Lines = unhasher32.ElapsedTimeStrings;
                    prevResultCount = unhasher32.ResultCount;
                    matchCountTXT.Text = string.Concat("Matches: ", prevResultCount.ToString());
                }
                iterationsTXT.Text = string.Concat("Iterations: ", iterations.ToString("##,#"));
            }
            if (unhasher64 != null)
            {
                ulong iterations = unhasher64.Iterations;
                var increment = (int) (iterations/(double) unhasher64.MaxIterations*1000000.0);
                unHashingProgress.Value = increment;
                if (unhasher64.Finished)
                {
                    updateTimer.Stop();
                    resultsTXT.Lines = unhasher64.Results;
                    endTimesTXT.Lines = unhasher64.ElapsedTimeStrings;
                }
                if (unhasher64.ResultCount > prevResultCount)
                {
                    resultsTXT.Lines = unhasher64.Results;
                    endTimesTXT.Lines = unhasher64.ElapsedTimeStrings;
                    prevResultCount = unhasher64.ResultCount;
                    matchCountTXT.Text = string.Concat("Matches: ", prevResultCount.ToString());
                }
                iterationsTXT.Text = string.Concat("Iterations: ", iterations.ToString("##,#"));
            }
        }

        private void settings_Click(object sender, EventArgs e)
        {
            using (var sDialog = new SettingsDialog(searchTable, filter32, filter64))
            {
                DialogResult result = sDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    searchTable = sDialog.SearchTable;
                    filter32 = sDialog.Filter32;
                    filter64 = sDialog.Filter64;
                }
            }
        }
    }
}
