using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    internal class AudioResourceHandler : AResourceHandler
    {
        public AudioResourceHandler()
        {
            Add(typeof (AudioTunerResource), new List<string> {"0x8070223D"});
        }
    }
}
