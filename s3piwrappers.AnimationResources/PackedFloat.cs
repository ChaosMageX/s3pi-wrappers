using System;
using System.Collections.Generic;
using System.Linq;

namespace s3piwrappers
{
    internal class PackedFloat
    {
        //needs work
        public static void CalculateScaleOffset(IEnumerable<float> values, Int32 bitsPerFloat, out float offset, out float scale)
        {
            offset = values.Min();
            scale = (float) ((values.Max() - values.Min())/(Math.Pow(2, bitsPerFloat) - 1));
        }

        public static float Unpack(ulong packed, Int32 position, Int32 bitsPerFloat, float offset, float scale, bool flipped)
        {
            ulong max = (ulong) (Math.Pow(2, bitsPerFloat)-1);
            return Unpack(((packed & max) >> (position*bitsPerFloat))/(float) max, offset, scale, flipped);
        }

        public static float Unpack(float packed, float offset, float scale, bool flipped)
        {
            if (flipped) offset *= -1;
            return (packed*scale) + offset;
        }

        public static void Pack(float unpacked, Int32 position, Int32 bitsPerFloat, float offset, float scale, bool flipped, ref ulong packed)
        {
            ulong max = (ulong) (Math.Pow(2, bitsPerFloat)-1);
            packed |= (ulong) Math.Ceiling((float) (((ulong) Pack(unpacked, offset, scale, flipped)*max) << (position*bitsPerFloat)));
        }

        public static float Pack(float unpacked, float offset, float scale, bool flipped)
        {
            if (flipped) offset *= -1;
            return (unpacked - offset)/scale;
        }
    }
}