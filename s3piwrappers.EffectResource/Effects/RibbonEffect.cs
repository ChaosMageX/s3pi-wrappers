/*
 * Contribution from ChaosMageX
 */
using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;
using System.Collections.Generic;

namespace s3piwrappers.Effects
{
    public class RibbonEffect : Effect, IEquatable<RibbonEffect>
    {
        public RibbonEffect(int apiVersion, EventHandler handler, RibbonEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public RibbonEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mResource = new ResourceReference(0, handler);
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
            mFloatList05 = new DataList<FloatValue>(handler);
            mFloatList06 = new DataList<FloatValue>(handler);
            mFloatList07 = new DataList<FloatValue>(handler);
            mColourList01 = new DataList<ColourValue>(handler);
            mVector3List01 = new DataList<Vector3ValueLE>(handler);
            mVector3List02 = new DataList<Vector3ValueLE>(handler);
        }
        public RibbonEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s) { }

        private UInt32 mInt01;
        private UInt32 mInt02;
        private UInt32 mInt03;
        private DataList<FloatValue> mFloatList01;
        private DataList<FloatValue> mFloatList02;
        private float mFloat01;
        private float mFloat02;
        private float mFloat03;
        private DataList<ColourValue> mColourList01;
        private DataList<FloatValue> mFloatList03;
        private DataList<Vector3ValueLE> mVector3List01;
        private DataList<FloatValue> mFloatList04;
        private DataList<Vector3ValueLE> mVector3List02;
        private DataList<FloatValue> mFloatList05;
        private DataList<FloatValue> mFloatList06;
        private DataList<FloatValue> mFloatList07;
        private UInt32 mInt04;
        private float mFloat04;
        private ResourceReference mResource;
        private UInt32 mInt05;  //0xFFFFFFFF
        private float mFloat05;
        private float mFloat06;
        private float mFloat07;

        private float mFloat08; //LE
        private float mFloat09; //LE
        private float mFloat10; //LE

        private float mFloat11;
        private float mFloat12;
        private UInt64 mLong01; //0xFFFFFFFFFFFFFFFF
        private UInt64 mLong02; //0xFFFFFFFFFFFFFFFF
        private UInt32 mInt06;  //0x00000000

        

        [ElementPriority(31)]
        public uint Int06
        {
            get { return mInt06; }
            set { mInt06 = value; OnElementChanged(); }
        }

        [ElementPriority(30)]
        public ulong Long02
        {
            get { return mLong02; }
            set { mLong02 = value; OnElementChanged(); }
        }

        [ElementPriority(29)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }

        [ElementPriority(28)]
        public float Float12
        {
            get { return mFloat12; }
            set { mFloat12 = value; OnElementChanged(); }
        }

        [ElementPriority(27)]
        public float Float11
        {
            get { return mFloat11; }
            set { mFloat11 = value; OnElementChanged(); }
        }

        [ElementPriority(26)]
        public float Float10
        {
            get { return mFloat10; }
            set { mFloat10 = value; OnElementChanged(); }
        }

        [ElementPriority(25)]
        public float Float09
        {
            get { return mFloat09; }
            set { mFloat09 = value; OnElementChanged(); }
        }

        [ElementPriority(24)]
        public float Float08
        {
            get { return mFloat08; }
            set { mFloat08 = value; OnElementChanged(); }
        }

        [ElementPriority(23)]
        public float Float07
        {
            get { return mFloat07; }
            set { mFloat07 = value; OnElementChanged(); }
        }

        [ElementPriority(22)]
        public float Float06
        {
            get { return mFloat06; }
            set { mFloat06 = value; OnElementChanged(); }
        }

        [ElementPriority(21)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }

        [ElementPriority(20)]
        public uint Int05
        {
            get { return mInt05; }
            set { mInt05 = value; OnElementChanged(); }
        }

        [ElementPriority(19)]
        public ResourceReference Resource
        {
            get { return mResource; }
            set { mResource = value; OnElementChanged(); }
        }

        [ElementPriority(18)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }

        [ElementPriority(17)]
        public uint Int04
        {
            get { return mInt04; }
            set { mInt04 = value; OnElementChanged(); }
        }

        [ElementPriority(16)]
        public DataList<FloatValue> FloatList07
        {
            get { return mFloatList07; }
            set { mFloatList07 = value; OnElementChanged(); }
        }

        [ElementPriority(15)]
        public DataList<FloatValue> FloatList06
        {
            get { return mFloatList06; }
            set { mFloatList06 = value; OnElementChanged(); }
        }

        [ElementPriority(14)]
        public DataList<FloatValue> FloatList05
        {
            get { return mFloatList05; }
            set { mFloatList05 = value; OnElementChanged(); }
        }

        [ElementPriority(13)]
        public DataList<Vector3ValueLE> Vector3List02
        {
            get { return mVector3List02; }
            set { mVector3List02 = value; OnElementChanged(); }
        }

        [ElementPriority(12)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }

        [ElementPriority(11)]
        public DataList<Vector3ValueLE> Vector3List01
        {
            get { return mVector3List01; }
            set { mVector3List01 = value; OnElementChanged(); }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }

        [ElementPriority(9)]
        public DataList<ColourValue> ColourList01
        {
            get { return mColourList01; }
            set { mColourList01 = value; OnElementChanged(); }
        }

        [ElementPriority(8)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }

        [ElementPriority(7)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }

        [ElementPriority(6)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }

        [ElementPriority(5)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }

        [ElementPriority(3)]
        public uint Int03
        {
            get { return mInt03; }
            set { mInt03 = value; OnElementChanged(); }
        }

        [ElementPriority(2)]
        public uint Int02
        {
            get { return mInt02; }
            set { mInt02 = value; OnElementChanged(); }
        }

        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mInt02);
            s.Read(out mInt03);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat01);
            s.Read(out mFloat02);
            s.Read(out mFloat03);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            mVector3List02 = new DataList<Vector3ValueLE>(handler, stream);
            mFloatList05 = new DataList<FloatValue>(handler, stream);
            mFloatList06 = new DataList<FloatValue>(handler, stream);
            mFloatList07 = new DataList<FloatValue>(handler, stream);
            s.Read(out mInt04);
            s.Read(out mFloat04);
            mResource = new ResourceReference(0, handler, stream);
            s.Read(out mInt05);
            s.Read(out mFloat05);
            s.Read(out mFloat06);
            s.Read(out mFloat07);

            s.Read(out mFloat08, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat09, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat10, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat11);
            s.Read(out mFloat12);
            s.Read(out mLong01);
            s.Read(out mLong02);
            s.Read(out mInt06);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mInt01);
            s.Write(mInt02);
            s.Write(mInt03);
            mFloatList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            s.Write(mFloat01);
            s.Write(mFloat02);
            s.Write(mFloat03);
            mColourList01.UnParse(stream);
            mFloatList03.UnParse(stream);
            mVector3List01.UnParse(stream);
            mFloatList04.UnParse(stream);
            mVector3List02.UnParse(stream);
            mFloatList05.UnParse(stream);
            mFloatList06.UnParse(stream);
            mFloatList07.UnParse(stream);
            s.Write(mInt04);
            s.Write(mFloat04);
            mResource.UnParse(stream);
            s.Write(mInt05);
            s.Write(mFloat05);
            s.Write(mFloat06);
            s.Write(mFloat07);

            s.Write(mFloat08, ByteOrder.LittleEndian); //LE
            s.Write(mFloat09, ByteOrder.LittleEndian); //LE
            s.Write(mFloat10, ByteOrder.LittleEndian); //LE

            s.Write(mFloat11);
            s.Write(mFloat12);
            s.Write(mLong01);
            s.Write(mLong02);
            s.Write(mInt06);
        }

        public bool Equals(RibbonEffect other)
        {
            return base.Equals(other);
        }
    }
}