using System.IO;
using s3pi.GenericRCOLResource;

namespace s3piwrappers
{
    public class ObjectGeometryResource : GenericRCOLResource
    {
        public ObjectGeometryResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            for (int i = 0; i < ChunkEntries.Count; i++)
            {
                var chunkEntry = ChunkEntries[i];
                if (chunkEntry.TGIBlock.ResourceType == 0x00000000)
                {
                    var block = new VBSI(0, this.OnResourceChanged, new MemoryStream(chunkEntry.RCOLBlock.AsBytes));
                    ChunkEntries[i] = new ChunkEntry(0, this.OnResourceChanged, chunkEntry.TGIBlock, block);

                }
            }
        }
    }
}