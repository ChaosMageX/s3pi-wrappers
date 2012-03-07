using System;
using System.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class DefaultEffect : Effect, IEquatable<DefaultEffect>
    {
        public DefaultEffect(int apiVersion, EventHandler handler, ISection section) : base(apiVersion, handler, section)
        {
            throw new NotSupportedException();
        }

        public DefaultEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
            throw new NotSupportedException();
        }

        protected override void Parse(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void UnParse(Stream stream)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DefaultEffect other)
        {
            return base.Equals(other);
        }
    }
}
