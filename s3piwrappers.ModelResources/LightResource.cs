using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers
{

    public class LightResourceHandler :AResourceHandler
    {
        public LightResourceHandler()
        {
            Add(typeof(LightResource), new List<string>() { "0x03B4C61D" });
        }
    }
    //quick hack to get this alternate chunk handler into the wrapper manager 
    public class LightResource : GenericRCOLResource
    {
        public LightResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            var chunk = ChunkEntries[0];
            var stream = chunk.RCOLBlock.Stream;
            stream.Position = 0L;
            var lite = new LITE(0, OnResourceChanged, stream);
            ChunkEntries[0] = new ChunkEntry(0, OnResourceChanged, chunk.TGIBlock, lite);
        }
    }
}
