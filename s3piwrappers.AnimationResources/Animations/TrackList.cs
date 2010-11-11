using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class TrackList : AResource.DependentList<Track>
    {
        public TrackList(EventHandler handler) : base(handler) {}
        public TrackList(EventHandler handler, IList<Track> ilt) : base(handler, ilt) {}
        public TrackList(EventHandler handler, long size) : base(handler, size) {}
        public TrackList(EventHandler handler, long size, IList<Track> ilt) : base(handler, size, ilt) {}


        public override void Add()
        {
            base.Add(new object[] { });
        }

        protected override Track CreateElement(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }

        protected override void WriteElement(System.IO.Stream s, Track element)
        {
            throw new NotSupportedException();
        }
    }
}