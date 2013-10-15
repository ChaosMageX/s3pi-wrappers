using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.JazzGraph
{
    public abstract class AChunkObject
    {
        public static readonly GenericRCOLResource.ChunkReference NullCRef
            = new GenericRCOLResource.ChunkReference(0, null, 0);

        private static uint sInstanceCount = 0;

        public readonly uint InstanceNum;
        public readonly GenericRCOLResource.ChunkReference ChunkReference;

        public readonly uint ChunkType;
        public readonly string ChunkTag;

        public AChunkObject(uint chunkType, string chunkTag)
        {
            this.InstanceNum = sInstanceCount++;
            this.ChunkReference 
                = new GenericRCOLResource.ChunkReference(0, null);
            this.ChunkType = chunkType;
            this.ChunkTag = chunkTag;
        }

        public class InstantiationComparer : IComparer<AChunkObject>
        {
            public static readonly InstantiationComparer Instance
                = new InstantiationComparer();

            public int Compare(AChunkObject x, AChunkObject y)
            {
                return x.InstanceNum == y.InstanceNum 
                    ? 0 : (x.InstanceNum > y.InstanceNum ? 1 : -1);
            }
        }

        public abstract GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames);
    }
}
