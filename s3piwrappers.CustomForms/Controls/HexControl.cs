using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace s3piwrappers.CustomForms.Controls
{
    public class HexControl : ABuiltInValueControl
    {
        int rowLength = 16;

        RichTextBox rtb = new RichTextBox()
        {
            Font = new Font(FontFamily.GenericMonospace, 10),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            ReadOnly = true,
        };

        public HexControl(Stream s)
            : base(s)
        {
            if (s == null || s == Stream.Null)
                return;

            rtb.Text = GetHex(s);
        }

        public override bool IsAvailable { get { return true; } }

        public override Control ValueControl { get { return rtb; } }

        public override IEnumerable<ToolStripItem> GetContextMenuItems(EventHandler cbk) { yield break; }

        private String GetHex(Stream s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            byte[] b = new byte[s.Length];
            s.Read(b, 0, b.Length);

            int padLength = 8;// +b.Length.ToString("X").Length;
            string rowFmt = "X" + padLength;

            sb.Append("".PadLeft(padLength + 2));
            for (int col = 0; col < rowLength; col++) sb.Append(col.ToString("X2") + " ");
            sb.AppendLine();
            sb.Append("".PadLeft(padLength + 2));
            for (int col = 0; col < rowLength; col++) sb.Append("---");
            sb.AppendLine();

            for (int row = 0; row < b.Length; row += rowLength)
            {
                sb.Append(row.ToString(rowFmt) + ": ");

                int col = 0;
                for (; col < rowLength && row + col < b.Length; col++) sb.Append(b[row + col].ToString("X2") + " ");
                for (; col < rowLength; col++) sb.Append("   ");

                sb.Append(" : ");
                for (col = 0; col < rowLength && row + col < b.Length; col++)
                    sb.Append(b[row + col] < 0x20 || b[row + col] > 0x7e ? '.' : (char)b[row + col]);

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
