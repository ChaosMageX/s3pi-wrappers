using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    class AudioResourceHandler : AResourceHandler
    {
        public AudioResourceHandler()
        {
            Add(typeof(AudioTunerResource), new List<string> { "0x8070223D" });
        }
    }
}
