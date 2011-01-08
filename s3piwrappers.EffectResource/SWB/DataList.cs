﻿using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.SWB
{
    public class DataList<TElement> : DependentList<TElement>
        where TElement : DataElement, IEquatable<TElement>
    {
        public DataList(EventHandler handler) : base(handler) { }
        public DataList(EventHandler handler, Stream s) : base(handler, s) { }
        public DataList(EventHandler handler, long size) : base(handler, size){}
        public DataList(EventHandler handler, IEnumerable<TElement> ilt) : base(handler, ilt){}
        protected override int ReadCount(Stream s)
        {
            return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadInt32();
        }

        protected override void WriteCount(Stream s, int count)
        {
            new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
        }
        public override void Add()
        {
            base.Add(new object[] { });
        }

        protected override TElement CreateElement(Stream s)
        {
            return (TElement)Activator.CreateInstance(typeof(TElement), new object[] { 0, elementHandler, s });
        }

        protected override void WriteElement(Stream s, TElement element)
        {
            element.UnParse(s);
        }
    }

}