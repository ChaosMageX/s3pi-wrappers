using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class HairToneResourceHandler : AResourceHandler
    {
        public HairToneResourceHandler()
        {
            Add(typeof(HairToneResource), new List<string>(new string[] { "0x03555BA8" }));
        }
    }
}