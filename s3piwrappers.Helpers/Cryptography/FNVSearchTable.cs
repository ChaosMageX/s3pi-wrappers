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
                version = table.mVersion;
                current = '\0';
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (version == table.mVersion && index < table.mSize)
                {
                    current = (char) table.mTable[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (version != table.mVersion)
                {
                    throw new InvalidOperationException(
                        "Enumerator Version Mismatch");
                }
                index = table.mSize + 1;
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
                    if ((index == 0) || (index == (table.mSize + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (version != table.mVersion)
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

        private int mSize;
        private readonly byte[] mTable = new byte[256];
        private int mVersion;
        private object mSyncRoot;

        public int Count
        {
            get { return mSize; }
        }

        public byte[] Table
        {
            get
            {
                var result = new byte[mSize];
                if (mSize > 0)
                {
                    Array.Copy(mTable, 0, result, 0, mSize);
                }
                return result;
            }
        }

        public char[] ToCharArray()
        {
            var results = new char[mSize];
            if (mSize > 0)
            {
                Array.Copy(mTable, 0, results, 0, mSize);
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
                if (index < 0 || index >= mSize)
                {
                    throw new IndexOutOfRangeException();
                }
                return (char)mTable[index];
            }
        }

        #endregion

        #region Prefix And Suffix

        private byte[] mPrefix = new byte[0];
        private string mPrefixStr = "";
        private int mPrefixLength;

        public byte[] PrefixBytes
        {
            get
            {
                var result = new byte[mPrefixLength];
                if (mPrefixLength > 0)
                {
                    Array.Copy(mPrefix, 0, result, 0, mPrefixLength);
                }
                return result;
            }
        }

        public string Prefix
        {
            get
            {
                /*if (mPrefixLength == 0)
                {
                    return "";
                }
                var result = new char[mPrefixLength];
                Array.Copy(mPrefix, 0, result, 0, mPrefixLength);
                return new string(result);/* */
                return mPrefixStr;
            }
            set
            {
                mPrefixStr = "";
                if (value == null)
                {
                    mPrefixLength = 0;
                    mPrefix = new byte[0];
                }
                else
                {
                    mPrefixLength = value.Length;
                    mPrefix = new byte[mPrefixLength];
                    if (mPrefixLength > 0)
                    {
                        int realLength = 0;
                        for (int i = 0; i < mPrefixLength; i++)
                        {
                            if (value[i] < 0xff)
                            {
                                mPrefix[i]
                                    = FNVHash.LCAlphabetConversion[value[i]];
                                mPrefixStr += value[i];
                                realLength++;
                            }
                        }
                        mPrefixLength = realLength;
                    }
                }
            }
        }

        private byte[] mSuffix = new byte[0];
        private string mSuffixStr = "";
        private int mSuffixLength;

        public byte[] SuffixBytes
        {
            get
            {
                var result = new byte[mSuffixLength];
                if (mSuffixLength > 0)
                {
                    Array.Copy(mSuffix, 0, result, 0, mSuffixLength);
                }
                return result;
            }
        }

        public string Suffix
        {
            get
            {
                /*if (mSuffixLength == 0)
                {
                    return "";
                }
                var result = new char[mSuffixLength];
                Array.Copy(mSuffix, 0, result, 0, mSuffixLength);
                return new string(result);/* */
                return mSuffixStr;
            }
            set
            {
                mSuffixStr = "";
                if (value == null)
                {
                    mSuffixLength = 0;
                    mSuffix = new byte[0];
                }
                else
                {
                    mSuffixLength = value.Length;
                    mSuffix = new byte[mSuffixLength];
                    if (mSuffixLength > 0)
                    {
                        int realLength = 0;
                        for (int i = 0; i < mSuffixLength; i++)
                        {
                            if (value[i] < 0xff)
                            {
                                mSuffix[i]
                                    = FNVHash.LCAlphabetConversion[value[i]];
                                mSuffixStr += value[i];
                                realLength++;
                            }
                        }
                        mSuffixLength = realLength;
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
            mSize = chars.Length;
            Array.Copy(chars, 0, mTable, 0, mSize);
            Array.Sort(mTable, 0, mSize, null);
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
            if (value > 0xff || mSize == 0)
            {
                return -1;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med;
            int lo = 0;
            int hi = mSize - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (mTable[med] == b)
                    return med;
                if (mTable[med] < b)
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
            if (value > 0xff || mSize == 256)
            {
                return false;
            }
            if (mSize == 0)
            {
                mTable[0] = FNVHash.LCAlphabetConversion[value];
                mSize++;
                mVersion++;
                return true;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med;
            int lo = 0;
            int hi = mSize - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (mTable[med] == b)
                    return false;
                if (mTable[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            Array.Copy(mTable, lo, mTable, lo + 1, mSize - lo);
            mTable[lo] = b;
            mSize++;
            mVersion++;
            return true;
        }

        public int AddChars(char[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (mSize == 256)
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
            if (mSize == 256)
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
            if (value > 0xff || mSize == 0)
            {
                return false;
            }
            byte b = FNVHash.LCAlphabetConversion[value];
            int med = -1;
            int lo = 0;
            int hi = mSize - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (mTable[med] == b)
                    break;
                if (mTable[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            if (lo > hi)
            {
                return false;
            }
            mSize--;
            Array.Copy(mTable, med + 1, mTable, med, mSize - med);
            mTable[mSize] = 0;
            mVersion++;
            return true;
        }

        public int RemoveChars(char[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (mSize == 0)
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
            if (mSize == 0)
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
            if (index < 0 || index >= mSize)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            mSize--;
            Array.Copy(mTable, index + 1, mTable, index, mSize - index);
            mTable[mSize] = 0;
            mVersion++;
        }

        #endregion

        public char GetNextChar(char value)
        {
            if (value > 0xff || mSize == 0)
            {
                return value;
            }
            byte st, b = FNVHash.LCAlphabetConversion[value];
            if (b < mTable[0])
            {
                return (char)mTable[0];
            }
            if (b >= mTable[mSize - 1])
            {
                return value;
            }
            int med;
            int lo = 0;
            int hi = mSize - 1;
            while (hi - lo > 1)
            {
                med = lo + ((hi - lo) >> 1);
                st = mTable[med];
                if (b < st)
                    hi = med;
                else if (b > st)
                    lo = med;
                else
                    lo = hi = med;
            }
            if (lo == hi)
            {
                return (char)mTable[hi + 1];
            }
            else
            {
                return (char)mTable[hi];
            }
        }

        public char GetPrevChar(char value)
        {
            if (value > 0xff || mSize == 0)
            {
                return value;
            }
            byte st, b = FNVHash.LCAlphabetConversion[value];
            if (b <= mTable[0])
            {
                return value;
            }
            if (b > mTable[mSize - 1])
            {
                return (char)mTable[mSize - 1];
            }
            int med;
            int lo = 0;
            int hi = mSize - 1;
            while (hi - lo > 1)
            {
                med = lo + ((hi - lo) >> 1);
                st = mTable[med];
                if (b < st)
                    hi = med;
                else if (b > st)
                    lo = med;
                else
                    lo = hi = med;
            }
            if (lo == hi)
            {
                return (char)mTable[lo - 1];
            }
            else
            {
                return (char)mTable[lo];
            }
        }

        #region ICollection<char> Implementation

        public void Add(char item)
        {
            AddChar(item);
        }

        public void Clear()
        {
            if (mSize > 0)
            {
                Array.Clear(mTable, 0, mSize);
                mSize = 0;
            }
            mVersion++;
        }

        public bool Contains(char item)
        {
            if (item > 0xff || mSize == 0)
            {
                return false;
            }
            byte b = FNVHash.LCAlphabetConversion[item];
            int med;
            int lo = 0;
            int hi = mSize - 1;
            while (lo <= hi)
            {
                med = lo + ((hi - lo) >> 1);
                if (mTable[med] == b)
                    return true;
                if (mTable[med] < b)
                    lo = med + 1;
                else
                    hi = med - 1;
            }
            return false;
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            Array.Copy(mTable, 0, array, arrayIndex, mSize);
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
            Array.Copy(mTable, 0, array, index, mSize);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (mSyncRoot == null)
                {
                    Interlocked.CompareExchange<object>(
                        ref mSyncRoot, new object(), null);
                }
                return mSyncRoot;
            }
        }

        #endregion
    }
}
