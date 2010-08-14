using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.GenericRCOLResource;
namespace s3piwrappers
{
    public class GeometryResource : GenericRCOLResource
    {
        public GeometryResource(int APIversion, Stream s) : base(APIversion, s)
        {
            var chunk = ChunkEntries[0];
            var stream = chunk.RCOLBlock.Stream;
            stream.Position = 0L;
            var geom = new GEOM(0, OnResourceChanged, stream);
            ChunkEntries[0] = new ChunkEntry(0,OnResourceChanged,chunk.TGIBlock,geom);
        }
    }
}
