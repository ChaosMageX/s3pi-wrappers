using System;
using System.Collections.Generic;
using System.Windows.Forms;
using s3piwrappers.CustomForms;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;

namespace s3piwrappers.FreeformJazz
{
    public class JazzGraphNamePrompt : IDisposable
    {
        private const string kPromptText
            = "Enter a new name for the jazz graph, "
            + "which will give it this resource key:\n";

        public readonly List<string> BannedNames = new List<string>();

        private RK mKey;
        private string mName;
        private SingleStringPrompt mPrompt;
        private EventHandler mOnTextChanged;

        private void ParseIID()
        {
            ulong iid = 0;
            if (!this.mName.StartsWith("0x") ||
                !ulong.TryParse(this.mName.Substring(2),
                System.Globalization.NumberStyles.HexNumber, null, out iid))
            {
                iid = FNVHash.HashString64(this.mName);
            }
            this.mKey.IID = iid;
        }

        private void OnResponseTextChanged(object sender, EventArgs e)
        {
            this.mName = this.mPrompt.Response;
            this.ParseIID();
            this.mPrompt.Prompt = kPromptText + this.mKey;
            this.mPrompt.OKEnabled = !string.IsNullOrEmpty(this.mName)
                && !this.BannedNames.Contains(this.mName);
        }

        public JazzGraphNamePrompt()
        {
            this.mKey = new RK(GlobalManager.kJazzTID, 0, 0);
            this.mName = "";
            this.mPrompt = new SingleStringPrompt(
                kPromptText + this.mKey, "Jazz Graph Name", "");
            this.mOnTextChanged
                = new EventHandler(this.OnResponseTextChanged);
            this.mPrompt.ResponseTextChanged += this.mOnTextChanged;
        }

        public JazzGraphNamePrompt(string name, string bannedName)
        {
            this.mName = name ?? "";
            this.mKey = new RK(GlobalManager.kJazzTID, 0, 0);
            this.ParseIID();
            this.mPrompt = new SingleStringPrompt(
                kPromptText + this.mKey, "Jazz Graph Name", this.mName);
            if (!string.IsNullOrEmpty(bannedName))
            {
                this.BannedNames.Add(bannedName);
                if (this.mName.Equals(bannedName))
                {
                    this.mPrompt.OKEnabled = false;
                }
            }
            this.mOnTextChanged
                = new EventHandler(this.OnResponseTextChanged);
            this.mPrompt.ResponseTextChanged += this.mOnTextChanged;
        }

        public RK Key
        {
            get { return this.mKey; }
        }

        public string Name
        {
            get { return this.mName; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                if (!this.mName.Equals(value))
                {
                    if (this.mPrompt == null)
                    {
                        this.mName = value;
                        this.ParseIID();
                    }
                    else
                    {
                        this.mPrompt.Response = value;
                        this.OnResponseTextChanged(null, null);
                    }
                }
            }
        }

        public DialogResult ShowDialog()
        {
            if (this.mPrompt == null)
            {
                return DialogResult.None;
            }
            return this.mPrompt.ShowDialog();
        }

        public void Dispose()
        {
            if (this.mPrompt != null)
            {
                this.mPrompt.ResponseTextChanged -= this.mOnTextChanged;
                this.mPrompt.Dispose();
                this.mPrompt = null;
            }
        }
    }
}
