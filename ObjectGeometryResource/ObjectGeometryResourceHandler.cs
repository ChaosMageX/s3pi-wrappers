using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class ObjectGeometryResourceHandler : AResourceHandler
    {
        public ObjectGeometryResourceHandler()
        {
            base.Add(typeof(ObjectGeometryResource),new List<String>(){"0x01661233","0x01D10F34"});
        }
    }
}
