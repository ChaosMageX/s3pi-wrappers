using System;
using System.Collections.Generic;
using s3pi.Interfaces;
using System.IO;

namespace s3piwrappers
{
    public enum CountSize
    {
        Byte,
        Word,
        Dword,
        Qword
    }
    public class ResourceKeyTable
    {
        public struct TablePtr
        {
            public long Position;
            public Int32 Size;
        }
        private CountSize mCountSize;
        private string mFormat;
        private List<IResourceKey> mKeys;

        public ResourceKeyTable() : this(CountSize.Dword, "TGI") { }
        public ResourceKeyTable(CountSize countSize, string format)
        {
            mKeys = new List<IResourceKey>();
            mCountSize = countSize;
            mFormat = format;
        }

        private void SetFormat(string format)
        {
            format = format.Trim().ToUpper();
            if (format.Length != 3)
                throw new ArgumentException("Key format must have a length of 3.", "format");
            if (!format.Contains("T") && !format.Contains("G") && !format.Contains("I"))
                throw new ArgumentException("Key format must contain T,G, and I", "format");

        }
        private UInt32 ReadCount(BinaryReader r)
        {
            switch (mCountSize)
            {
                case CountSize.Byte:
                    return r.ReadByte();
                case CountSize.Word:
                    return r.ReadUInt16();
                case CountSize.Dword:
                    return r.ReadUInt32();
                case CountSize.Qword:
                    return (uint)r.ReadUInt64();
                default: throw new NotSupportedException();
            }
        }
        private void WriteCount(BinaryWriter w, UInt32 count)
        {
            switch (mCountSize)
            {
                case CountSize.Byte: w.Write((Byte)count); break;
                case CountSize.Word: w.Write((UInt16)count); break;
                case CountSize.Dword: w.Write((UInt32)count); break;
                case CountSize.Qword: w.Write((UInt64)count); break;
                default: throw new NotSupportedException();
            }
        }
        private IResourceKey ReadKey(BinaryReader r)
        {
            uint t = 0;
            uint g = 0;
            ulong i = 0;
            foreach (var c in mFormat)
            {
                switch (c)
                {
                    case 'T': t = r.ReadUInt32(); break;
                    case 'G': g = r.ReadUInt32(); break;
                    case 'I': i = r.ReadUInt64(); break;

                }
            }
            return new AResource.TGIBlock(0, null, t, g, i);
        }
        private void WriteKey(BinaryWriter w, IResourceKey key)
        {

            foreach (var c in mFormat)
            {
                switch (c)
                {
                    case 'T': w.Write(key.ResourceType); break;
                    case 'G': w.Write(key.ResourceGroup); break;
                    case 'I': w.Write(key.Instance); break;

                }
            }
        }
        public ICollection<IResourceKey> Keys { get { return mKeys; } }

        public int Add(IResourceKey key)
        {
            if (mKeys.Contains(key))
            {
                return mKeys.IndexOf(key);
            }
            mKeys.Add(key);
            return mKeys.Count -1;
        }
        public void Add(IEnumerable<IResourceKey> keys)
        {
            foreach (var k in keys) Add(k);
        }
        public IResourceKey this[int index]
        {
            get { return mKeys[index]; }
        }
        public TablePtr BeginRead(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            TablePtr ptr;
            ptr.Position = br.ReadUInt32() + s.Position;
            ptr.Size = br.ReadInt32();
            long pos = s.Position;
            s.Seek(ptr.Position, SeekOrigin.Begin);
            mKeys.Clear();
            var c = ReadCount(br);
            for (int i = 0; i < c; i++) mKeys.Add(ReadKey(br));
            s.Seek(pos, SeekOrigin.Begin);
            return ptr;
        }
        public void EndRead(Stream s, TablePtr ptr)
        {
            if (s.Position != ptr.Position)
                throw new InvalidDataException(string.Format("Bad offset, expected 0x{0:X16},but got 0x{1:X16}.", ptr.Position, s.Position));
            s.Seek(ptr.Size, SeekOrigin.Current);
        }
        public long BeginWrite(Stream s)
        {
            mKeys.Clear();
            var pos = s.Position;
            var bw = new BinaryWriter(s);
            bw.Write(0U);
            bw.Write(0U);
            return pos;
        }
        public void EndWrite(Stream s, long startPos)
        {
            var bw = new BinaryWriter(s);
            long begin = s.Position;
            WriteCount(bw, (uint)mKeys.Count);
            for (int i = 0; i < mKeys.Count; i++) WriteKey(bw, mKeys[i]);
            long size = s.Position - begin;
            long pos = s.Position;
            s.Seek(startPos, SeekOrigin.Begin);
            bw.Write((uint)(begin - (s.Position + 4)));
            bw.Write((uint)size);
            s.Seek(pos, SeekOrigin.Begin);
        }
    }
}