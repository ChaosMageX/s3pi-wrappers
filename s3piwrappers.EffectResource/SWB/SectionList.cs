using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.Reflection;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.SWB
{
    public abstract class SectionList<TSection> : AHandlerList<TSection>, IGenericAdd
        where TSection : Section, IEquatable<TSection>
    {
        public SectionList(EventHandler handler) : base(handler)
        {
        }

        public SectionList(EventHandler handler, Stream s) : base(handler)
        {
            Parse(s);
        }

        protected virtual TSection CreateElement(UInt16 type, UInt16 version, Stream s)
        {
            return (TSection) Activator.CreateInstance(GetSectionType(type), new object[] {0, handler, type, version, s});
        }

        protected abstract Type GetSectionType(UInt16 id);

        protected void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            UInt16 blockType;
            blockType = s.ReadUInt16();
            while (blockType != 0xFFFF)
            {
                UInt16 blockVersion = s.ReadUInt16();
                var sectionType = GetSectionType(blockType);
                var blocklist = CreateElement(blockType, blockVersion, stream);
                base.Add(blocklist);
                blockType = s.ReadUInt16();
            }
        }

        public void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            foreach (var list in this)
            {
                s.Write(list.Type);
                s.Write(list.Version);
                list.UnParse(stream);
            }
            s.Write((ushort) 0xFFFF);
        }

        public bool Add(params object[] fields)
        {
            if (fields == null | fields.Length == 0)
            {
                return false;
            }

            var t = GetSectionType((ushort) (int) fields[0]);
            object[] args = null;
            ConstructorInfo ctor = null;
            if (fields.Length == 2)
            {
                ctor = t.GetConstructor(new Type[] {typeof (int), typeof (EventHandler), typeof (ushort), typeof (ushort)});
                args = new object[] {0, handler, (ushort) (int) fields[0], (ushort) (int) fields[1]};
            }
            else if (fields.Length == 1 && typeof (TSection).IsAssignableFrom(fields[0].GetType()))
            {
                ctor = t.GetConstructor(new Type[] {typeof (int), typeof (EventHandler), t});
                args = new object[] {0, handler, fields[0]};
            }
            else
            {
                return false;
            }
            if (ctor != null) base.Add((TSection) ctor.Invoke(args));
            return true;
        }

        public void Add()
        {
            throw new NotSupportedException();
        }
    }
}
