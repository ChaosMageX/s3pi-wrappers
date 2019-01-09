using System;
using System.Collections;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers.CustomForms.PropertyGrid
{
    public class AsKVP : AHandlerElement, IEquatable<AsKVP>
    {
        static List<string> contentFields = new List<string>(new string[] { "Key", "Val", });
        DictionaryEntry entry;

        public AsKVP(int APIversion, EventHandler handler, AsKVP basis) : this(APIversion, handler, basis.entry.Key, basis.entry.Value) { }
        public AsKVP(int APIversion, EventHandler handler, DictionaryEntry entry) : this(APIversion, handler, entry.Key, entry.Value) { }
        public AsKVP(int APIversion, EventHandler handler, object key, object value) : base(APIversion, handler) { entry = new DictionaryEntry(key, value); }

        public override List<string> ContentFields { get { return contentFields; } }
        public override int RecommendedApiVersion { get { return 0; } }

        public override TypedValue this[string index]
        {
            get
            {
                switch (contentFields.IndexOf(index))
                {
                    case 0: return new TypedValue(entry.Key.GetType(), entry.Key, "X");
                    case 1: return new TypedValue(entry.Value.GetType(), entry.Value, "X");
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (contentFields.IndexOf(index))
                {
                    case 0: entry.Key = value.Value; break;
                    case 1: entry.Value = value.Value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Type GetType(string index)
        {
            switch (contentFields.IndexOf(index))
            {
                case 0: return entry.Key.GetType();
                case 1: return entry.Value.GetType();
                default: throw new IndexOutOfRangeException();
            }
        }

        public override AHandlerElement Clone(EventHandler handler) { return new AsKVP(requestedApiVersion, handler, this); }

        #region IEquatable<AsKVP> Members

        public bool Equals(AsKVP other) { return this.entry.Key.Equals(other.entry.Key) && this.entry.Value.Equals(other.entry.Value); }

        #endregion
    }
}
