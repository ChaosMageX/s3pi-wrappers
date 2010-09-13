using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class SoundEffect : Effect, IEquatable<SoundEffect>
    {
        public SoundEffect(int apiVersion, EventHandler handler, SoundEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public SoundEffect(int apiVersion, EventHandler handler, ISection section) : base(apiVersion, handler, section) { }
        public SoundEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s) { }


        #region Fields
        private UInt32 mUint01;
        private UInt64 mLong01;
        private float mFloat01 = 0.25f;
        private float mFloat02;
        private float mFloat03;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public uint Uint01
        {
            get { return mUint01; }
            set { mUint01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ulong AudioResourceInstance
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }
        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mUint01);
            s.Read(out mLong01);
            s.Read(out mFloat01);
            s.Read(out mFloat02);
            s.Read(out mFloat03);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mUint01);
            s.Write(mLong01);
            s.Write(mFloat01);
            s.Write(mFloat02);
            s.Write(mFloat03);
        }

        public bool Equals(SoundEffect other)
        {
            return base.Equals(other);
        }
    }
}