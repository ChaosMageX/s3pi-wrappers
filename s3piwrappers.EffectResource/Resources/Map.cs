using System;
using System.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Resources
{
    public class Map : Resource, IEquatable<Map>
    {
        public Map(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            throw new NotSupportedException();
        }

        public Map(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
            throw new NotSupportedException();
        }

        public Map(int apiVersion, EventHandler handler, Resource basis)
            : base(apiVersion, handler, basis)
        {
            throw new NotSupportedException();
        }

        protected override void Parse(Stream s)
        {
            throw new NotImplementedException();
        }

        public override void UnParse(Stream s)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Map other)
        {
            return base.Equals(other);
        }
    }
}
