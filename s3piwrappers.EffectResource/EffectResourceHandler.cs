using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class EffectResourceHandler : AResourceHandler
    {
        public EffectResourceHandler()
        {
            Add(typeof (EffectResource), new List<string> {"0xEA5118B0"});
        }
    }
}
