using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class SkinControllerResourceHandler : AResourceHandler
    {
        public SkinControllerResourceHandler()
        {
            base.Add(typeof(SkinControllerResource), new List<string>() { "0x00AE6C67" });
        }
    }
}