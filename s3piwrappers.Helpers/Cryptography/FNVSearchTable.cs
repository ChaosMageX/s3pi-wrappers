using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace s3piwrappers.Helpers.Cryptography
{
    [Serializable]
    public class FNVSearchTable : ICollection<char>, ICollection
    {
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<char>, IDisposable, IEnumerator
        {
            private FNVSearchTable table;
            private int index;
            private int version;
            private char current;
            internal Enumerator(FNVSearchTable table)
            {
                this.table = table;
                this.index = 0;
                this.version = table._version;
                this.current = '\0';
            }
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.version == this.table._version && this.index < this.table._size)
                {
                    this.current = (char)this.table._table[this.index];
                    this.index++;
                    return true;
                }
                return this.MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (this.version != this.table._version)
                {
                    throw new InvalidOperationException("Enumerator Version Mismatch");
                }
                this.index = this.table._size + 1;
                this.current = '\0';
                return false;
            }

            public char Current
            {
                get
                {
                    return this.current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.table._size + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return this.Current;
                }
            }
            void IEnumerator.Reset()
            {
                if (this.version != this.table._version)
                {
                    throw new InvalidOperationException("Enumerator Version Mismatch");
                }
                this.index = 0;
                this.current = '\0';
            }
        }

        #region Predefined Search Tables
        private static readonly byte[] lcAllAscii = new byte[]
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f, 
            0x40, 0xdf,                                                       0x5b, 0x5c, 0x5d, 0x5e, 0x5f, 
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f, 
            0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f, 
            0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f, 
            0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf, 
            0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf, 
            0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef, 
            0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
        }; // length = 199

        public static FNVSearchTable AllASCII
        {
            get { return new FNVSearchTable(lcAllAscii); }
        }

        private static readonly byte[] lcAllPrintable = new byte[]
        {
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f, 
            0x40,                                                             0x5b, 0x5c, 0x5d, 0x5e, 0x5f, 
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e
        }; // length = 69

        public static FNVSearchTable AllPrintable
        {
            get { return new FNVSearchTable(lcAllPrintable); }
        }

        private static readonly byte[] lcEnglishAlphanumeric = new byte[]
        {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
                  0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a
        }; // length = 36

        public static FNVSearchTable EnglishAlphanumeric
        {
            get { return new FNVSearchTable(lcEnglishAlphanumeric); }
        }

        private static readonly byte[] lcEnglishAlphabet = new byte[]
        {
                  0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a
        }; // length = 26

        public static FNVSearchTable EnglishAlphabet
        {
            get { return new FNVSearchTable(lcEnglishAlphabet); }
        }

        private static readonly byte[] lcNumeric = new byte[]
        {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39
        }; // length = 10

        public static FNVSearchTable Numeric
        {
            get { return new FNVSearchTable(lcNumeric); }
        }
        #endregion

        #region Fields And Properties
        private int _size = 0;
        private byte[] _table = new byte[256];
        private int _version;
        private object _syncRoot;

        public int Count
        {
            get { return this._size; }
        }

        public byte[] Table
        {
            get
            {
                byte[] result = new byte[this._size];
                if (this._size > 0)
                    Array.Copy(this._table, 0, result, 0, this._size);
                return result;
            }
        }

        public char[] ToCharArray()
        {
            char[] results = new char[this._size];
            if (this._size > 0)
            {
                Array.Copy(this._table, 0, results, 0, this._size);
            }
            return results;
        }

        public override string ToString()
        {
            return new string(this.ToCharArray());
        }

        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= this._size)
                    throw new IndexOutOfRangeException();
                return (char)this._table[index];
            }
        }
        #endregion

        #region Prefix And Suffix
        private byte[] _prefix = new byte[0];
        private int _prefixLength = 0;

        public byte[] PrefixBytes
        {
            get
            {
                byte[] result = new byte[this._prefixLength];
                if (this._prefixLength > 0)
                    Array.Copy(this._prefix, 0, result, 0, this._prefixLength);
                return result;
            }
        }

        public string Prefix
        {
            get
            {
                if (this._prefixLength == 0)
                    return "";
                char[] result = new char[this._prefixLength];
                Array.Copy(this._prefix, 0, result, 0, this._prefixLength);
                return new string(result);
            }
            set
            {
                this._prefixLength = value.Length;
                this._prefix = new byte[this._prefixLength];
                if (this._prefixLength > 0)
                {
                    int realLength = 0;
                    for (int i = 0; i < this._prefixLength; i++)
                    {
                        if (value[i] < 0xff)
                        {
                            this._prefix[i] = FNVHash.LCAlphabetConversion[value[i]];
                            realLength++;
                        }
                    }
                    this._prefixLength = realLength;
                }
            }
        }

        private byte[] _suffix = new byte[0];
        private int _suffixLength = 0;

        public byte[] SuffixBytes
        {
            get
            {
                byte[] result = new byte[this._suffixLength];
                if (this._suffixLength > 0)
                    Array.Copy(this._suffix, 0, result, 0, this._suffixLength);
                return result;
            }
        }

        public string Suffix
        {
            get
            {
                if (this._suffixLength == 0)
                    return "";
                char[] result = new char[this._suffixLength];
                Array.Copy(this._suffix, 0, result, 0, this._suffixLength);
                return new string(result);
            }
            set
            {
                this._suffixLength = value.Length;
                this._suffix = new byte[this._suffixLength];
                if (this._prefixLength > 0)
                {
                    int realLength = 0;
                    for (int i = 0; i < this._suffixLength; i++)
                    {
                        if (value[i] < 0xff)
                        {
                            this._suffix[i] = FNVHash.LCAlphabetConversion[value[i]];
                            realLength++;
                        }
                    }
                    this._suffixLength = realLength;
                }
            }
        }
        #endregion

        private FNVSearchTable(byte[] chars)
        {
            this._size = chars.Length;
            Array.Copy(chars, 0, this._table, 0, this._size);
        }

        public FNVSearchTable(char[] chars)
        {
            this.AddChars(chars);
        }

        public FNVSearchTable(string chars)
        {
            this.AddChars(chars);
        }

        #region Add And Remove Characters
        private const char maxChar = 'ÿ';

        public bool AddChar(char value)
        {
            if (value > 0xff) return false;
            if (this._size == 256) return false;
            if (this._size == 0)
            {
                this._table[0] = FNVHash.LCAlphabetConversion[value];
                this._size++;
                this._version++;
                return true;
            }
            byte st, b = FNVHash.LCAlphabetConversion[value];
            int i;
            for (i = 0; i < this._size; i++)
            {
                st = this._table[i];
                if (b == st) return false;
                if (b < st) break;
            }
            int insertIndex = i;
            for (i = this._size; i > insertIndex; i--)
            {
                this._table[i] = this._table[i - 1];
            }
            this._table[insertIndex] = b;
            this._size++;
            this._version++;
            return true;
        }

        public int AddChars(char[] values)
        {
            int added = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (AddChar(values[i]))
                    added++;
            }
            return added;
        }

        public int AddChars(string values)
        {
            int added = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (AddChar(values[i]))
                    added++;
            }
            return added;
        }

        public bool RemoveChar(char value)
        {
            if (value > 0xff) return false;
            if (this._size == 0) return false;
            byte st, b = FNVHash.LCAlphabetConversion[value];
            int i;
            for (i = 0; i < this._size; i++)
            {
                st = this._table[i];
                if (b < st) return false;
                if (b == st) break;
            }
            if (i == this._size) return false;
            this._size--;
            for (; i < this._size; i++)
            {
                this._table[i] = this._table[i + 1];
            }
            this._table[this._size] = 0;
            this._version++;
            return true;
        }

        public int RemoveChars(char[] values)
        {
            int removed = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (RemoveChar(values[i]))
                    removed++;
            }
            return removed;
        }

        public int RemoveChars(string values)
        {
            int removed = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (RemoveChar(values[i]))
                    removed++;
            }
            return removed;
        }
        #endregion

        #region ICollection<char> Implementation
        public void Add(char item)
        {
            this.AddChar(item);
        }

        public void Clear()
        {
            if (this._size > 0)
            {
                Array.Clear(this._table, 0, this._size);
                this._size = 0;
            }
            this._version++;
        }

        public bool Contains(char item)
        {
            if (item > 0xff) return false;
            if (this._size == 0) return false;
            byte b = FNVHash.LCAlphabetConversion[item];
            for (int i = 0; i < this._size; i++)
            {
                if (b == this._table[i])
                    return true;
            }
            return false;
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            Array.Copy(this._table, 0, array, arrayIndex, this._size);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(char item)
        {
            return this.RemoveChar(item);
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
        #endregion

        #region ICollection Implementation
        public void CopyTo(Array array, int index)
        {
            Array.Copy(this._table, 0, array, index, this._size);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }
        #endregion
    }
}
