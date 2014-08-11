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
    public partial class SettingsDialog : Form
    {
        private const string kAllAscii = "All ASCII";
        private const string kAllPrint = "All Printable";
        private const string kAlphaNum = "English Alphanumeric";
        private const string kAlphabet = "English Alphabet";
        private const string kNumeric  = "Numeric";

        private FNVSearchTable mSearchTable;
        private uint mFilter32;
        private ulong mFilter64;

        public FNVSearchTable SearchTable
        {
            get { return this.mSearchTable; }
        }

        public uint Filter32
        {
            get { return this.mFilter32; }
        }

        public ulong Filter64
        {
            get { return this.mFilter64; }
        }

        public SettingsDialog(FNVSearchTable searchTable,
            uint filter32, ulong filter64)
        {
            InitializeComponent();
            this.defSearchTablesCmb.Items.Clear();
            this.defSearchTablesCmb.Items.AddRange(new object[] {
                kAllAscii,
                kAllPrint,
                kAlphaNum,
                kAlphabet,
                kNumeric });
            this.mSearchTable = searchTable;
            this.mFilter32 = filter32;
            this.mFilter64 = filter64;
            this.searchTableOutputTxt.Text = searchTable.ToString();
            this.prefixInputTxt.Text = searchTable.Prefix;
            this.suffixOutputTxt.Text = searchTable.Suffix;
            this.filter32Txt.Text = string.Concat("0x", filter32.ToString("X8"));
            this.filter64Txt.Text = string.Concat("0x", filter64.ToString("X16"));
        }

        protected override void OnClosed(EventArgs e)
        {
            bool error = false;
            StringBuilder errorText = new StringBuilder();
            string filter32Str = this.filter32Txt.Text;
            if (filter32Str.StartsWith("0x"))
                filter32Str = filter32Str.Substring(2);
            if (filter32Str.StartsWith("0X"))
                filter32Str = filter32Str.Substring(2);
            uint filter32;
            if (uint.TryParse(filter32Str, System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.CurrentCulture, out filter32))
            {
                this.mFilter32 = filter32;
            }
            else
            {
                error = true;
                errorText.AppendLine("Could not Parse 32-bit Filter: " + this.filter32Txt.Text);
            }
            string filter64Str = this.filter64Txt.Text;
            if (filter64Str.StartsWith("0x"))
                filter64Str = filter64Str.Substring(2);
            if (filter64Str.StartsWith("0X"))
                filter64Str = filter64Str.Substring(2);
            ulong filter64;
            if (ulong.TryParse(filter64Str, System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.CurrentCulture, out filter64))
            {
                this.mFilter64 = filter64;
            }
            else
            {
                error = true;
                errorText.AppendLine("Could not Parse 64-bit Filter: " + this.filter64Txt.Text);
            }
            if (error)
            {
                MessageBox.Show(errorText.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void defSearchTables_Click(object sender, EventArgs e)
        {
            string defSearchTableStr = this.defSearchTablesCmb.SelectedItem as string;
            if (defSearchTableStr == null)
                return;
            string prefix = this.mSearchTable.Prefix;
            string suffix = this.mSearchTable.Suffix;
            switch (defSearchTableStr)
            {
                case kAllAscii: 
                    this.mSearchTable = FNVSearchTable.AllASCII;
                    break;
                case kAllPrint:
                    this.mSearchTable = FNVSearchTable.AllPrintable;
                    break;
                case kAlphaNum:
                    this.mSearchTable = FNVSearchTable.EnglishAlphanumeric;
                    break;
                case kAlphabet:
                    this.mSearchTable = FNVSearchTable.EnglishAlphabet;
                    break;
                case kNumeric:
                    this.mSearchTable = FNVSearchTable.Numeric;
                    break;
                default:
                    throw new InvalidOperationException("Unknown FNV Search Table");
            }
            this.searchTableOutputTxt.Text = this.mSearchTable.ToString();
            this.mSearchTable.Prefix = prefix;
            this.mSearchTable.Suffix = suffix;
        }

        private void addChars_Click(object sender, EventArgs e)
        {
            this.mSearchTable.AddChars(this.searchTableInputTxt.Text);
            this.searchTableOutputTxt.Text = this.mSearchTable.ToString();
        }

        private void removeChars_Click(object sender, EventArgs e)
        {
            this.mSearchTable.RemoveChars(this.searchTableInputTxt.Text);
            this.searchTableOutputTxt.Text = this.mSearchTable.ToString();
        }

        private void setPrefix_Click(object sender, EventArgs e)
        {
            this.mSearchTable.Prefix = this.prefixInputTxt.Text;
            this.prefixOutputTxt.Text = this.mSearchTable.Prefix;
        }

        private void setSuffix_Click(object sender, EventArgs e)
        {
            this.mSearchTable.Suffix = this.suffixInputTxt.Text;
            this.suffixOutputTxt.Text = this.mSearchTable.Suffix;
        }
    }
}
