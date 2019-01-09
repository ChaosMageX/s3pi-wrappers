using System;
using System.Collections;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    public class AsKVPList : DependentList<AsKVP>
    {
        DictionaryEntry entry;
        public AsKVPList(DictionaryEntry entry) : base(null) { this.entry = entry; }
        public override void Add() { this.Add(new AsKVP(0, null, entry)); }
        protected override AsKVP CreateElement(Stream s) { throw new NotImplementedException(); }
        protected override void WriteElement(Stream s, AsKVP element) { throw new NotImplementedException(); }
    }
}
