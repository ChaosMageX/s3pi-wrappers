using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class RigResourceHandler : AResourceHandler
    {
        public RigResourceHandler()
        {
            Add(typeof(RigResource), new List<string>() { "0x8EAF13DE" });
        }
    }
}
