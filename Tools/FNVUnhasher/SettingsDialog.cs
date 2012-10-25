using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.Helpers.Cryptography;

namespace FNVUnhasher
{
    public partial class SettingsDialog : Form
    {
        private FNVSearchTable mSearchTable;
        private uint mFilter32;
        private ulong mFilter64;

        public FNVSearchTable SearchTable
        {
            get { return mSearchTable; }
        }

        public uint Filter32
        {
            get { return mFilter32; }
        }

        public ulong Filter64
        {
            get { return mFilter64; }
        }

        public SettingsDialog(FNVSearchTable searchTable,
                              uint filter32, ulong filter64)
        {
            InitializeComponent();
            defSearchTablesCmb.Items.Clear();
            defSearchTablesCmb.Items.AddRange(new object[]
                {
                    "All ASCII",
                    "All Printable",
                    "English Alphanumeric",
                    "English Alphabet",
                    "Numeric"
                });
            mSearchTable = searchTable;
            mFilter32 = filter32;
            mFilter64 = filter64;
            searchTableOutputTxt.Text = searchTable.ToString();
            prefixInputTxt.Text = searchTable.Prefix;
            suffixOutputTxt.Text = searchTable.Suffix;
            filter32Txt.Text = string.Concat("0x", filter32.ToString("X8"));
            filter64Txt.Text = string.Concat("0x", filter64.ToString("X16"));
        }

        protected override void OnClosed(EventArgs e)
        {
            bool error = false;
            var errorText = new StringBuilder();
            string filter32Str = filter32Txt.Text;
            if (filter32Str.StartsWith("0x"))
                filter32Str = filter32Str.Substring(2);
            if (filter32Str.StartsWith("0X"))
                filter32Str = filter32Str.Substring(2);
            uint filter32;
            if (uint.TryParse(filter32Str, NumberStyles.HexNumber,
                              CultureInfo.CurrentCulture, out filter32))
            {
                mFilter32 = filter32;
            }
            else
            {
                error = true;
                errorText.AppendLine("Could not Parse 32-bit Filter: " + filter32Txt.Text);
            }
            string filter64Str = filter64Txt.Text;
            if (filter64Str.StartsWith("0x"))
                filter64Str = filter64Str.Substring(2);
            if (filter64Str.StartsWith("0X"))
                filter64Str = filter64Str.Substring(2);
            ulong filter64;
            if (ulong.TryParse(filter64Str, NumberStyles.HexNumber,
                               CultureInfo.CurrentCulture, out filter64))
            {
                mFilter64 = filter64;
            }
            else
            {
                error = true;
                errorText.AppendLine("Could not Parse 64-bit Filter: " + filter64Txt.Text);
            }
            if (error)
            {
                MessageBox.Show(errorText.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void defSearchTables_Click(object sender, EventArgs e)
        {
            var defSearchTableStr = defSearchTablesCmb.SelectedItem as string;
            if (defSearchTableStr == null)
                return;
            if (defSearchTableStr.Equals("All ASCII"))
                mSearchTable = FNVSearchTable.AllASCII;
            if (defSearchTableStr.Equals("All Printable"))
                mSearchTable = FNVSearchTable.AllPrintable;
            if (defSearchTableStr.Equals("English Alphanumeric"))
                mSearchTable = FNVSearchTable.EnglishAlphanumeric;
            if (defSearchTableStr.Equals("English Alphabet"))
                mSearchTable = FNVSearchTable.EnglishAlphabet;
            if (defSearchTableStr.Equals("Numeric"))
                mSearchTable = FNVSearchTable.Numeric;
            searchTableOutputTxt.Text = mSearchTable.ToString();
        }

        private void addChars_Click(object sender, EventArgs e)
        {
            mSearchTable.AddChars(searchTableInputTxt.Text);
            searchTableOutputTxt.Text = mSearchTable.ToString();
        }

        private void removeChars_Click(object sender, EventArgs e)
        {
            mSearchTable.RemoveChars(searchTableInputTxt.Text);
            searchTableOutputTxt.Text = mSearchTable.ToString();
        }

        private void setPrefix_Click(object sender, EventArgs e)
        {
            mSearchTable.Prefix = prefixInputTxt.Text;
            prefixOutputTxt.Text = mSearchTable.Prefix;
        }

        private void setSuffix_Click(object sender, EventArgs e)
        {
            mSearchTable.Suffix = suffixInputTxt.Text;
            suffixOutputTxt.Text = mSearchTable.Suffix;
        }
    }
}
