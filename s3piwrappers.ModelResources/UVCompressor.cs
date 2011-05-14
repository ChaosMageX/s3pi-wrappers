using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers
{
    public static class UVCompressor
    {
        private const int kMaxUVChannels = 3;
        
        public static float[] GetMaxUVs(IEnumerable<Vertex> vertices)
        {
            var maxuvs = new float[kMaxUVChannels];
            foreach (var vertex in vertices)
            {
                for (int i = 0; i < vertex.UV.Length; i++)
                {
                    foreach (var value in vertex.UV[i])
                    {
                        var abs = Math.Abs(value);
                        if (abs > maxuvs[i]) maxuvs[i] = abs;
                    }
                }
            }
            return maxuvs;
        }
        public static float[] GetUVScales(VRTF vrtf)
        {
            var uvchannels = vrtf.Layouts.Where(x => x.Usage == VRTF.ElementUsage.UV).OrderBy(x => x.UsageIndex).ToArray();
            var maxUvs = new float[kMaxUVChannels];
            for (int i = 0; i < uvchannels.Length; i++)
            {
                maxUvs[i] = 1f;
            }
            return GetUVScales(vrtf, maxUvs);
        }
        public static float[] GetUVScales(VRTF vrtf, IEnumerable<Vertex> vertices)
        {
            return GetUVScales(vrtf,GetMaxUVs(vertices));
        }
        public static float[] GetUVScales(VRTF vrtf, float[] maxuvs)
        {
            var uvchannels = vrtf.Layouts.Where(x => x.Usage == VRTF.ElementUsage.UV).OrderBy(x => x.UsageIndex).ToArray();
            if (!uvchannels.Any(channel => GetMaxPackedUVFromElementFormat(channel.Format) != null)) return null;
            var uvscales = new float[kMaxUVChannels];
            for (int i = 0; i < uvscales.Length; i++)
            {
                var layout = uvchannels[i];
                var maxval = GetMaxPackedUVFromElementFormat(layout.Format);
                if (maxval != null)
                {
                    uvscales[i] = maxuvs[i] / (float)maxval;
                }
            }
            return uvscales;
        }
        private static ulong? GetMaxPackedUVFromElementFormat(VRTF.ElementFormat format)
        {
            switch (format)
            {
                case VRTF.ElementFormat.Short2:
                    return (ulong)short.MaxValue;
                default:
                    return null;
            }
        }
    }
}
