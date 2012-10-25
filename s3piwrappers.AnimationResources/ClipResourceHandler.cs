using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class ClipResourceHandler : AResourceHandler
    {
        public ClipResourceHandler()
        {
            Add(typeof (ClipResource), new List<string> {"0x6B20C4F3"});
        }
    }
}
