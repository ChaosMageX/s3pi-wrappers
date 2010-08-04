using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class SkinToneResourceHandler : AResourceHandler
    {
        public SkinToneResourceHandler()
        {
            Add(typeof(SkinToneResource),new List<string>(new string[]{"0x0354796A"}));
        }
    }
}