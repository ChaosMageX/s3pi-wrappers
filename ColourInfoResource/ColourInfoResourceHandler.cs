using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class ColourInfoResourceHandler : AResourceHandler
    {
        public ColourInfoResourceHandler()
        {
            Add(typeof(ColourInfoResource), new List<string>(new string[] { "0x063261DA", "0x06302271", "0x3A65AF29", "0x06326213" }));
        }
    }
}