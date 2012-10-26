using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.SWB
{
    public abstract class SectionList<TSection> : DependentList<TSection>
        where TSection : Section, IEquatable<TSection>
    {
        protected SectionList(EventHandler handler) : base(handler)
        {
        }

        protected SectionList(EventHandler handler, Stream s) : base(handler)
        {
            Parse(s);
        }

        protected virtual TSection CreateElement(UInt16 type, UInt16 version, Stream s)
        {
            return (TSection) Activator.CreateInstance(GetSectionType(type), 0, handler, version, s);
        }

        protected abstract Type GetSectionType(UInt16 id);

        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            ushort blockType = s.ReadUInt16();
            while (blockType != 0xFFFF)
            {
                UInt16 blockVersion = s.ReadUInt16();
                Type sectionType = GetSectionType(blockType);
                TSection blocklist = CreateElement(blockType, blockVersion, stream);
                base.Add(blocklist);
                blockType = s.ReadUInt16();
            }
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            foreach (TSection list in this)
            {
                s.Write(list.Type);
                s.Write(list.Version);
                list.UnParse(stream);
            }
            s.Write((ushort) 0xFFFF);
        }

        protected override TSection CreateElement(Stream s)
        {
            throw new NotImplementedException();
        }

        protected override void WriteElement(Stream s, TSection element)
        {
            throw new NotImplementedException();
        }
    }
}
