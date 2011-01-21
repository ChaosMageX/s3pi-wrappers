using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class RigResourceHandler : AResourceHandler
    {
        public RigResourceHandler()
        {
            if(IntPtr.Size ==4)Add(typeof(RigResource), new List<string>() { "0x8EAF13DE" });
        }
    }
}
