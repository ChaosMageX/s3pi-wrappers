using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Helpers.Cryptography
{
    public static class FNVCLIP
    {
        private static bool isAO(string text)
        {
            return text.Equals("a", StringComparison.Ordinal)
                || text.Equals("o", StringComparison.Ordinal);
        }

        /// <summary>
        /// Get the "generic" CLIP name, removing age and species.
        /// </summary><param name="text">
        /// The CLIP name from which to get the generic value.
        /// </param><returns>The generic CLIP name.</returns>
        public static string GetGenericValue(string text)
        {
            int index = text.IndexOf('_', 0, text.Length);
            if (index > 0 && index <= 5)
            {
                string x2y = FNVHash.SubstringToLower(text, 0, index);
                int index2 = x2y.IndexOf('2', 0, index);
                if (index2 > 0 && index2 <= 2)
                {
                    string x = x2y.Substring(0, index2);
                    string y = x2y.Substring(index2 + 1);
                    if (y.Length > 0 && !(isAO(x) && isAO(y)))
                    {
                        return string.Concat(
                            (x[0] == 'o' ? "o" : "a"), "2",
                            (y[0] == 'o' ? "o" : "a"), "_",
                            text.Substring(index + 1));
                    }
                }
                else if (!isAO(x2y))
                {
                    return string.Concat("a_", text.Substring(index + 1));
                }
            }
            return text;
        }

        /// <summary>
        /// Get the 64-bit FNV hash for use as the IID 
        /// for a CLIP, but ignoring age and species.
        /// </summary><param name="text">
        /// The CLIP name to get the generic hash for.
        /// </param><returns>The generic hash value.</returns>
        public static ulong GetGenericHash(string text)
        {
            string value = GetGenericValue(text);

            ulong hash = FNVHash.HashString64(text);
            hash &= 0x7FFFFFFFFFFFFFFF;

            return hash;
        }

        private static byte Mask(string actor)
        {
            switch (actor)
            {
                case "b": return 0x01;
                case "p": return 0x02;
                case "c": return 0x03;
                case "t": return 0x04;
                case "h": return 0x05;
                case "e": return 0x06;
                case "ad": return 0x08;
                case "cd": return 0x09;
                case "al": return 0x0A;
                case "ac": return 0x0D;
                case "cc": return 0x0E;
                case "ah": return 0x10;
                case "ch": return 0x11;
                case "ab": return 0x12;
                case "ar": return 0x13;
                default: return 0x00;
            }
        }

        /// <summary>
        /// Get the 64-bit FNV hash for use as the IID 
        /// for a CLIP of a given name.
        /// </summary><param name="text">
        /// The CLIP name to get the hash for.
        /// </param><returns>The hash value.</returns>
        public static ulong HashString(string text)
        {
            string value = text;
            ulong mask = 0;

            int index = text.IndexOf('_', 0, text.Length);
            if (index > 0 && index <= 5)
            {
                string x2y = FNVHash.SubstringToLower(text, 0, index);
                int index2 = x2y.IndexOf('2', 0, index);
                if (index2 > 0 && index2 <= 2)
                {
                    string x = x2y.Substring(0, index2);
                    string y = x2y.Substring(index2 + 1);
                    if (y.Length > 0 && !(isAO(x) && isAO(y)))
                    {
                        byte xAge = Mask(x);
                        byte yAge = Mask(y);
                        value = string.Concat(
                            (x[0] == 'o' ? "o" : "a"), "2",
                            (y[0] == 'o' ? "o" : "a"), "_",
                            text.Substring(index + 1));
                        mask = (ulong)(0x8000 | xAge << 8 | yAge);
                    }
                }
                else if (!isAO(x2y))
                {
                    byte xAge = Mask(x2y);
                    value = string.Concat("a_", text.Substring(index + 1));
                    mask = (ulong)(0x8000 | xAge << 8);
                }
            }

            ulong hash = FNVHash.HashString64(value);
            hash &= 0x7FFFFFFFFFFFFFFF;
            hash ^= mask << 48;

            return hash;
        }
    }
}
