using System;
using System.Collections.Generic;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;

namespace s3piwrappers
{
    public class FrameList : AResource.DependentList<Frame>
    {
        private CurveType mType;
        public FrameList(EventHandler handler, CurveType type)
            : base(handler)
        {
            mType = type;
        }
        public FrameList(EventHandler handler, CurveType type, IList<Frame> ilt)
            : base(handler, ilt)
        {
            mType = type;
        }
        public FrameList(EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> floats)
            : base(handler)
        {
            mType = type;
            Parse(s, info, floats);
        }

        public CurveType Type
        {
            get { return mType; }
        }

        private void Parse(Stream s, CurveDataInfo info, IList<float> floats)
        {
            for (int i = 0; i < info.FrameCount; i++)
            {
                ((IList<Frame>)this).Add(CreateElement(s, info, floats));
            }
        }
        public void UnParse(Stream s, CurveDataInfo info, IList<float> floats)
        {
            info.FrameDataOffset = (uint)s.Position;
            info.FrameCount = Count;
            for (int i = 0; i < Count; i++)
            {
                this[i].UnParse(s, info, floats);
            }
        }
        public override void Add()
        {
            Add(new object[] { });
        }

        protected virtual Frame CreateElement(Stream s, CurveDataInfo info, IList<float> floats)
        {
            return Frame.CreateInstance(0, handler,mType, s, info, floats);
        }

        protected virtual void WriteElement(Stream s, CurveDataInfo info, IList<float> floats, Frame element)
        {
            element.UnParse(s, info, floats);
        }

        #region Unused
        protected override Frame CreateElement(Stream s)
        {
            throw new NotSupportedException();
        }

        protected override void WriteElement(Stream s, Frame element)
        {
            throw new NotSupportedException();
        } 
        #endregion
    }
}