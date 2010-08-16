using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class GeometryResourceHandler : AResourceHandler
    {
        public GeometryResourceHandler()
        {
            Add(typeof(GeometryResource), new List<string>() { "0x015A1849" });
        }
    }
}
