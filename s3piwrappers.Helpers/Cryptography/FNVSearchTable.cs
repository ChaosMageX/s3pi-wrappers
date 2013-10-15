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
        public struct Enumerator 
            : IEnumerator<char>, IDisposable, IEnumerator
        {
            private readonly FNVSearchTable table;
            private int index;
            private readonly int version;
            private char current;

            internal Enumerator(FNVSearchTable table)
            {
                this.table = table;
                index = 0;
                version = table._version;
                current = '\0';
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (version == table._version && index < table._size)
                {
                    current = (char) table._table[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (version != table._version)
                {
                    throw new InvalidOperationException(
                        "Enumerator Version Mismatch");
                }
                index = table._size + 1;
                current = '\0';
                return false;
            }

            public char Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get
                {
                    if ((index == 0) || (index == (table._size + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (version != table._version)
                {
                    throw new InvalidOperationException(
                        "Enumerator Version Mismatch");
                }
                index = 0;
                current = '\0';
            }
        }

        #region Predefined Search Tables

        private static readonly byte[] lcAllAscii = new byte[]
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
            0x40, 0xd7, 0xdf,                                                 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
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

        private static readonly byte[] lcAllVisual = new byte[]
        {
                  0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
            0x40, 0xd7, 0xdf,                                                 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e,
            0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
            0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
            0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
            0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
        }; // length = 134

        public static FNVSearchTable AllVisual
        {
            get { return new FNVSearchTable(lcAllVisual); }
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

        private int _size;
        private readonly byte[] _table = new byte[256];
        private int _version;
        private object _syncRoot;

        public int Count
        {
            get { return _size; }
        }

        public byte[] Table
        {
            get
            {
                var result = new byte[_size];
                if (_size > 0)
                {
                    Array.Copy(_table, 0, result, 0, _size);
                }
                return result;
            }
        }

        public char[] ToCharArray()
        {
            var results = new char[_size];
            if (_size > 0)
            {
                Array.Copy(_table, 0, results, 0, _size);
            }
            return results;
        }

        public override string ToString()
        {
            return new string(ToCharArray());
        }

        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= _size)
                {
                    throw new IndexOutOfRangeException();
                }
                return (char)_table[index];
            }
        }

        #endregion

        #region Prefix And Suffix

        private byte[] _prefix = new byte[0];
        private int _prefixLength;

        public byte[] PrefixBytes
        {
            get
            {
                var result = new byte[_prefixLength];
                if (_prefixLength > 0)
                {
                    Array.Copy(_prefix, 0, result, 0, _prefixLength);
                }
                return result;
            }
        }

        public string Prefix
        {
            get
            {
                if (_prefixLength == 0)
                {
                    return "";
                }
                var result = new char[_prefixLength];
                Array.Copy(_prefix, 0, result, 0, _prefixLength);
                return new string(result);
            }
            set
            {
                if (value == null)
                {
                    _prefixLength = 0;
                    _prefix = new byte[0];
                }
                else
                {
                    _prefixLength = value.Length;
                    _prefix = new byte[_prefixLength];
                    if (_prefixLength > 0)
                    {
                        int realLength = 0;
                        for (int i = 0; i < _prefixLength; i++)
                        {
                            if (value[i] < 0xff)
                            {
                                _prefix[i]
                                    = FNVHash.LCAlphabetConversion[value[i]];
                                realLength++;
                            }
                        }
                        _prefixLength = realLength;
                    }
                }
            }
        }

        private byte[] _suffix = new byte[0];
        private int _suffixLength;

        public byte[] SuffixBytes
        {
            get
            {
                var result = new byte[_suffixLength];
                if (_suffixLength > 0)
                {
                    Array.Copy(_suffix, 0, result, 0, _suffixLength);
                }
                return result;
            }
        }

        public string Suffix
        {
            get
            {
                if (_suffixLength == 0)
                {
                    return "";
                }
                var result = new char[_suffixLength];
                Array.Copy(_suffix, 0, result, 0, _suffixLength);
                return new string(result);
            }
            set
            {
                if (value == null)
                {
                    _suffixLength = 0;
                    _suffix = new byte[0];
                }
                else
                {
                    _suffixLength = value.Length;
                    _suffix = new byte[_suffixLength];
                    if (_prefixLength > 0)
                    {
                        int realLength = 0;
                        for (int i = 0; i < _suffixLength; i++)
                        {
                            if (value[i] < 0xff)
                            {
                                _suffix[i]
                                    = FNVHash.LCAlphabetConversion[value[i]];
                                realLength++;
                            }
                        }
                        _suffixLength = realLength;
                    }
                }
            }
        }

        #endregion

        private FNVSearchTable(byte[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            _size = chars.Length;
            Array.Copy(chars, 0, _table, 0, _size);
            Array.Sort(_table, 0, _size, null);
        }

        public FNVSearchTable(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            AddChars(chars);
        }

        public FNVSearchTable(string chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            AddChars(chars);
        }

        public int IndexOf(char value)
        {
            if (value > 0xff || _size == 0)
            {
                return -1;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med;
            int lo = 0;
            int hi = _size - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (_table[med] == b)
                    return med;
                if (_table[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            return ~lo;
        }

        #region Add And Remove Characters

        private const char maxChar = 'ÿ';

        public bool AddChar(char value)
        {
            if (value > 0xff || _size == 256)
            {
                return false;
            }
            if (_size == 0)
            {
                _table[0] = FNVHash.LCAlphabetConversion[value];
                _size++;
                _version++;
                return true;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med;
            int lo = 0;
            int hi = _size - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (_table[med] == b)
                    return false;
                if (_table[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            Array.Copy(_table, lo, _table, lo + 1, _size - lo);
            _table[lo] = b;
            _size++;
            _version++;
            return true;
        }

        public int AddChars(char[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (_size == 256)
            {
                return 0;
            }
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
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (_size == 256)
            {
                return 0;
            }
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
            if (value > 0xff || _size == 0)
            {
                return false;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med = -1;
            int lo = 0;
            int hi = _size - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (_table[med] == b)
                    break;
                if (_table[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            if (lo > hi)
            {
                return false;
            }
            _size--;
            Array.Copy(_table, med + 1, _table, med, _size - med);
            _table[_size] = 0;
            _version++;
            return true;
        }

        public int RemoveChars(char[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (_size == 0)
            {
                return 0;
            }
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
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (_size == 0)
            {
                return 0;
            }
            int removed = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (RemoveChar(values[i]))
                    removed++;
            }
            return removed;
        }

        public void RemoveCharAt(int index)
        {
            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            _size--;
            Array.Copy(_table, index + 1, _table, index, _size - index);
            _table[_size] = 0;
            _version++;
        }

        #endregion

        public char GetNextChar(char value)
        {
            if (value > 0xff || _size == 0)
            {
                return value;
            }
            byte st, b = FNVHash.LCAlphabetConversion[value];
            if (b < _table[0])
            {
                return (char)_table[0];
            }
            if (b >= _table[_size - 1])
            {
                return value;
            }
            int med;
            int lo = 0;
            int hi = _size - 1;
            while (hi - lo > 1)
            {
                med = lo + ((hi - lo) >> 1);
                st = _table[med];
                if (b < st)
                    hi = med;
                else if (b > st)
                    lo = med;
                else
                    lo = hi = med;
            }
            if (lo == hi)
            {
                return (char)_table[hi + 1];
            }
            else
            {
                return (char)_table[hi];
            }
        }

        public char GetPrevChar(char value)
        {
            if (value > 0xff || _size == 0)
            {
                return value;
            }
            byte st, b = FNVHash.LCAlphabetConversion[value];
            if (b <= _table[0])
            {
                return value;
            }
            if (b > _table[_size - 1])
            {
                return (char)_table[_size - 1];
            }
            int med;
            int lo = 0;
            int hi = _size - 1;
            while (hi - lo > 1)
            {
                med = lo + ((hi - lo) >> 1);
                st = _table[med];
                if (b < st)
                    hi = med;
                else if (b > st)
                    lo = med;
                else
                    lo = hi = med;
            }
            if (lo == hi)
            {
                return (char)_table[lo - 1];
            }
            else
            {
                return (char)_table[lo];
            }
        }

        #region ICollection<char> Implementation

        public void Add(char item)
        {
            AddChar(item);
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_table, 0, _size);
                _size = 0;
            }
            _version++;
        }

        public bool Contains(char item)
        {
            if (item > 0xff || _size == 0)
            {
                return false;
            }
            byte b = FNVHash.LCAlphabetConversion[item];
            int med;
            int lo = 0;
            int hi = _size - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (_table[med] == b)
                    return true;
                if (_table[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            return false;
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            Array.Copy(_table, 0, array, arrayIndex, _size);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(char item)
        {
            return RemoveChar(item);
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
            Array.Copy(_table, 0, array, index, _size);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(
                        ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion
    }
}
