﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace s3piwrappers.CustomForms.Controls
{
    public class TextControl : ABuiltInValueControl
    {
        //TODO: static constructor read this from file
        //TODO: temporarily use the one from s3pi TextResource wrapper source
        static uint[] resourceTypes = new uint[] {
            0x024A0E52, // Spore UTF-8 with encoding bytes NameGeneration.package
            0x025C90A6, // Cascading Style Sheet
            0x025C95B6, // XML with encoding bytes -- <graph/>
            0x029E333B, // Audio controllers
            0x02C9EFF2, // Audio Submix
            0x024A0E52, // SimCity5 Config
            0x02FAC0B6, // Spore "Guide notes" from Text.package?
            0x0333406C, // XML with or without encoding bytes -- <base/> (with), various (without)
            0x03B33DDF, // XML without encoding bytes -- <base/>
            0x0604ABDA, // XML with encoding bytes -- <DreamTree/>
            0x0A98EAF0, // SimCity5 UI JavaScript Object
            0x1F886EAD, // Various configuration bits
            0x2B6CAB5F, // Spore Note in UI.package
            0x67771F5C, // SimCity5 UI JavaScript Object
            0x73E93EEB, // XML without encoding bytes -- <manifest/>
            0xA8D58BE5, // XML without encoding bytes -- <skills_store/>
            0xD4D9FBE5, // XML without encoding bytes -- <patternlist/>
            0xDD3223A7, // XML without encoding bytes -- <buff_store/>
            0xDD6233D6, // SimCity5 UI HTML
            0xE5105066, // ?
            0xE5105067, // XML without encoding bytes -- <RecipeMasterList_store/>
            0xE5105068, // ?
            0xF0FF5598, // Shortcut definitions
        };

        RichTextBox rtb = new RichTextBox()
        {
            Font = new Font(FontFamily.GenericMonospace, 8),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            ReadOnly = true,
        };

        public TextControl(Stream s)
            : base(s)
        {
            if (s == null || s == Stream.Null)
                return;

            rtb.Text = new StreamReader(s).ReadToEnd();
        }

        public TextControl(string s)
            : base(null)
        {
            if (s == null)
                return;

            rtb.Text = s;
        }

        public override bool IsAvailable { get { return true; } }

        public override Control ValueControl { get { return rtb; } }

        public override IEnumerable<ToolStripItem> GetContextMenuItems(EventHandler cbk) { yield break; }
    }
}
