using System;

namespace s3piwrappers.Helpers.Cryptography
{
    public static class FNVHash
    {
        //public const int LCAlphabetConversionLength = 256;
        public static readonly byte[] LCAlphabetConversion = new byte[]
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
            0x40, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
            0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
            0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
            0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
            0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
            0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
            0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xd7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xdf,
            0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
            0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
        };
        // 0x00 - 0x40, 0x5b - 0xbf, 0xd7, 0xdf, 0xe0 - 0xff

        public static string ToLower(string str)
        {
            return SubstringToLower(str, 0, str.Length);
        }

        public static string SubstringToLower(string str, int index)
        {
            return SubstringToLower(str, index, str.Length - index);
        }

#if UNSAFE
        unsafe
#endif
        public static string SubstringToLower(
            string str, int index, int length)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
#if !UNSAFE
            char[] chars = str.Substring(index, length).ToCharArray();
#else
            string result = str.Substring(index, length);
            fixed (char* chars = result)
#endif
            {
                for (int i = 0; i < length; i++)
                {
                    if (chars[i] > 0xff)
                    {
                        throw new IndexOutOfRangeException(
                            "Unicode Not Allowed");
                    }
                    chars[i] = (char)LCAlphabetConversion[chars[i]];
                }
            }
#if UNSAFE
            return result;
#else
            return new string(chars);
#endif
        }

        public const uint TS3Offset32 = 0x811c9dc5u;
        public const uint TS3Prime32 = 0x01000193u;

#if UNSAFE
        unsafe 
#endif
        public static uint HashString32(string str)
        {
            uint num = TS3Offset32;
            int length = str.Length;
#if !UNSAFE
            char[] chars = str.ToCharArray();
#else 
            fixed (char* chars = str)
#endif
            {
                for (int i = 0; i < length; i++)
                {
                    if (chars[i] > 0xff)
                    {
                        throw new IndexOutOfRangeException(
                            "Unicode Not Allowed");
                    }
                    num = (num*TS3Prime32) ^ LCAlphabetConversion[chars[i]];
                }
            }
            return num;
        }

        public static uint HashString24(string str)
        {
            uint num = HashString32(str);
            return ((num >> 0x18) ^ (num & 0xffffffU));
        }

        public static uint XorFoldHashString32(string str)
        {
            uint num = HashString32(str);
            return ((num >> 0x18) ^ (num & 0xffffffU));
        }

        public const ulong TS3Offset64 = 0xcbf29ce484222325UL;
        public const ulong TS3Prime64 = 0x00000100000001b3UL;

#if UNSAFE
        unsafe
#endif
        public static ulong HashString64(string str)
        {
            ulong num = TS3Offset64;
            int length = str.Length;
#if !UNSAFE
            char[] chars = str.ToCharArray();
#else
            fixed (char* chars = str)
#endif
            {
                for (int i = 0; i < length; i++)
                {
                    if (chars[i] > 0xff)
                    {
                        throw new IndexOutOfRangeException(
                            "Unicode Not Allowed");
                    }
                    num = (num*TS3Prime64) ^ LCAlphabetConversion[chars[i]];
                }
            }
            return num;
        }

        public static ulong HashString48(string str)
        {
            ulong num = HashString64(str);
            return ((num >> 0x30) ^ (num & 0xffffffffffffUL));
        }

        public static ulong XorFoldHashString64(string str)
        {
            ulong num = HashString64(str);
            return ((num >> 0x30) ^ (num & 0xffffffffffffUL));
        }
    }
}
