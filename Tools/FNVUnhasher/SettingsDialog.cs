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
        private FNVSearchTable mSearchTable;

        public FNVSearchTable SearchTable
        {
            get { return this.mSearchTable; }
        }

        public SettingsDialog(FNVSearchTable searchTable)
        {
            InitializeComponent();
            this.defSearchTablesCmb.Items.Clear();
            this.defSearchTablesCmb.Items.AddRange(new object[] {
                "All ASCII",
                "All Printable",
                "English Alphanumeric",
                "English Alphabet",
                "Numeric" });
            this.mSearchTable = searchTable;
            this.searchTableOutputTxt.Text = searchTable.ToString();
            this.prefixInputTxt.Text = searchTable.Prefix;
            this.suffixOutputTxt.Text = searchTable.Suffix;
        }

        private void defSearchTables_Click(object sender, EventArgs e)
        {
            string defSearchTableStr = this.defSearchTablesCmb.SelectedItem as string;
            if (defSearchTableStr == null)
                return;
            if (defSearchTableStr.Equals("All ASCII"))
                this.mSearchTable = FNVSearchTable.AllASCII;
            if (defSearchTableStr.Equals("All Printable"))
                this.mSearchTable = FNVSearchTable.AllPrintable;
            if (defSearchTableStr.Equals("English Alphanumeric"))
                this.mSearchTable = FNVSearchTable.EnglishAlphanumeric;
            if (defSearchTableStr.Equals("English Alphabet"))
                this.mSearchTable = FNVSearchTable.EnglishAlphabet;
            if (defSearchTableStr.Equals("Numeric"))
                this.mSearchTable = FNVSearchTable.Numeric;
            this.searchTableOutputTxt.Text = this.mSearchTable.ToString();
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
