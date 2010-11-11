using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;
using LightResource;

namespace s3piwrappers

{
    public enum LightType : uint
    {
        Ambient = 1,
        Directional = 2,
        Point = 3,
        Spot = 4,
        LampShade = 5,
        TubeLight = 6,
        SquareWindow = 7,
        CircularWindow = 8,
        SquareAreaLight = 9,
        DiscAreaLight = 10,
        WorldLight = 11
    }
    public enum OccluderType
    {
        Disc,
        Rectangle
    }


    public class LITE : ARCOLBlock
    {
        

        public abstract class Light : AHandlerElement, IEquatable<Light>
        {
            private Vector3 mTransform;
            private Colour3f mColour;
            private float mIntensity;

            protected Light(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mTransform = new Vector3(0, handler);
                mColour = new Colour3f(0, handler);
            }
            protected Light(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }

            protected Light(int APIversion, EventHandler handler, Light basis)
                : base(APIversion, handler)
            {
                mColour = basis.mColour;
                mIntensity = basis.mIntensity;
                mTransform = basis.mTransform;

            }

            [ElementPriority(0)]
            public abstract LightType Type { get; }

            [ElementPriority(1)]
            public Vector3 Transform
            {
                get { return mTransform; }
                set { if(mTransform!=value){mTransform = value; OnElementChanged();} }
            }

            [ElementPriority(2)]
            public Colour3f Colour
            {
                get { return mColour; }
                set { if(mColour!=value){mColour = value; OnElementChanged();} }
            }

            [ElementPriority(3)]
            public float Intensity
            {
                get { return mIntensity; }
                set { if(mIntensity!=value){mIntensity = value; OnElementChanged();} }
            }
            public string Value
            {
                get
                {
                    var sb = new StringBuilder();
                    foreach (var f in ContentFields)
                    {
                        if (f != "Value") sb.AppendFormat("{0}: {1}\r\n", f, this[f]);
                    }
                    return sb.ToString();
                }
            }
            protected virtual void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                mTransform = new Vector3(0, handler, s);
                mColour = new Colour3f(0, handler, s);
                mIntensity = br.ReadSingle();
            }
            public virtual void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                mTransform.UnParse(s);
                mColour.UnParse(s);
                bw.Write(mIntensity);
            }
            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public abstract override AHandlerElement Clone(EventHandler handler);

            public bool Equals(Light other)
            {
                return base.Equals(other);
            }
        }
        [ConstructorParameters(new object[] { LightType.Point })]
        public class PointLight : Light
        {
            public PointLight(int APIversion, EventHandler handler, PointLight basis) : base(APIversion, handler, basis) { }
            public PointLight(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public PointLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            public override LightType Type
            {
                get { return LightType.Point; }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new PointLight(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { LightType.WorldLight })]
        public class WorldLight : Light
        {
            public WorldLight(int APIversion, EventHandler handler, WorldLight basis) : base(APIversion, handler, basis) { }
            public WorldLight(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public WorldLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            public override LightType Type
            {
                get { return LightType.WorldLight; }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new WorldLight(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { LightType.Spot })]
        public class SpotLight : Light
        {
            private Vector3 mAt;
            private float mFalloffAngle;
            private float mBlurScale;

            public SpotLight(int APIversion, EventHandler handler, SpotLight basis)
                : base(APIversion, handler, basis)
            {
                mAt = new Vector3(0, handler, basis.mAt);
                mFalloffAngle = basis.mFalloffAngle;
                mBlurScale = basis.mBlurScale;
            }
            public SpotLight(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mAt = new Vector3(0, handler);
            }
            public SpotLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            [ElementPriority(4)]
            public Vector3 At
            {
                get { return mAt; }
                set { if(mAt!=value){mAt = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public float FalloffAngle
            {
                get { return mFalloffAngle; }
                set { if(mFalloffAngle!=value){mFalloffAngle = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float BlurScale
            {
                get { return mBlurScale; }
                set { if(mBlurScale!=value){mBlurScale = value; OnElementChanged();} }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                var br = new BinaryReader(s);
                mAt = new Vector3(0, handler, s);
                mFalloffAngle = br.ReadSingle();
                mBlurScale = br.ReadSingle();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                var bw = new BinaryWriter(s);
                mAt.UnParse(s);
                bw.Write(mFalloffAngle);
                bw.Write(mBlurScale);
            }
            public override LightType Type
            {
                get { return LightType.Spot; }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SpotLight(requestedApiVersion, handler, this);
            }
        }

        [ConstructorParameters(new object[] { LightType.LampShade })]
        public class LampShadeLight : Light
        {
            private Vector3 mAt;
            private float mFalloffAngle;
            private float mShadeLightRigMultiplier;
            private float mBottomAngle;
            private Colour3f mShadeColour;

            public LampShadeLight(int APIversion, EventHandler handler, LampShadeLight basis)
                : base(APIversion, handler, basis)
            {
                mAt = new Vector3(0, handler, basis.mAt);
                mFalloffAngle = basis.mFalloffAngle;
                mShadeLightRigMultiplier = basis.mShadeLightRigMultiplier;
                mBottomAngle = basis.mBottomAngle;
                mShadeColour = new Colour3f(0, handler, basis.mShadeColour);
            }
            public LampShadeLight(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mAt = new Vector3(0, handler);
                mShadeColour = new Colour3f(0, handler);
            }
            public LampShadeLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            [ElementPriority(4)]
            public Vector3 At
            {
                get { return mAt; }
                set { if(mAt!=value){mAt = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public float FalloffAngle
            {
                get { return mFalloffAngle; }
                set { if(mFalloffAngle!=value){mFalloffAngle = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float ShadeLightRigMultiplier
            {
                get { return mShadeLightRigMultiplier; }
                set { if(mShadeLightRigMultiplier!=value){mShadeLightRigMultiplier = value; OnElementChanged();} }
            }

            [ElementPriority(7)]
            public float BottomAngle
            {
                get { return mBottomAngle; }
                set { if(mBottomAngle!=value){mBottomAngle = value; OnElementChanged();} }
            }

            [ElementPriority(8)]
            public Colour3f ShadeColour
            {
                get { return mShadeColour; }
                set { if(mShadeColour!=value){mShadeColour = value; OnElementChanged();} }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                var br = new BinaryReader(s);
                mAt = new Vector3(0, handler, s);
                mFalloffAngle = br.ReadSingle();
                mShadeLightRigMultiplier = br.ReadSingle();
                mBottomAngle = br.ReadSingle();
                mShadeColour = new Colour3f(0, handler, s);
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                var bw = new BinaryWriter(s);
                mAt.UnParse(s);
                bw.Write(mFalloffAngle);
                bw.Write(mShadeLightRigMultiplier);
                bw.Write(mBottomAngle);
                mShadeColour.UnParse(s);
            }
            public override LightType Type
            {
                get { return LightType.LampShade; }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new LampShadeLight(requestedApiVersion, handler, this);
            }
        }

        [ConstructorParameters(new object[] { LightType.TubeLight })]
        public class TubeLight : Light
        {
            private Vector3 mAt;
            private float mTubeLength;
            private float mBlurScale;

            public TubeLight(int APIversion, EventHandler handler, TubeLight basis)
                : base(APIversion, handler, basis)
            {
                mAt = new Vector3(0, handler, basis.mAt);
                mTubeLength = basis.mTubeLength;
            }
            public TubeLight(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mAt = new Vector3(0, handler);
            }
            public TubeLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            [ElementPriority(4)]
            public Vector3 At
            {
                get { return mAt; }
                set { if(mAt!=value){mAt = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public float TubeLength
            {
                get { return mTubeLength; }
                set { if(mTubeLength!=value){mTubeLength = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float BlurScale
            {
                get { return mBlurScale; }
                set { if (mBlurScale != value){mBlurScale = value; OnElementChanged();} }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                var br = new BinaryReader(s);
                mAt = new Vector3(0, handler, s);
                mTubeLength = br.ReadSingle();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                var bw = new BinaryWriter(s);
                mAt.UnParse(s);
                bw.Write(mTubeLength);
            }
            public override LightType Type
            {
                get { return LightType.TubeLight; }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new TubeLight(requestedApiVersion, handler, this);
            }
        }


        [ConstructorParameters(new object[] { LightType.SquareAreaLight })]
        public class SquareAreaLight : Light
        {
            private Vector3 mAt;
            private Vector3 mRight;
            private float mWidth;
            private float mHeight;
            private float mFalloffAngle;
            private float mWindowTopBottomAngle;

            public SquareAreaLight(int APIversion, EventHandler handler, SquareAreaLight basis)
                : base(APIversion, handler, basis)
            {
                mAt = new Vector3(0, handler, basis.mAt);
                mRight = new Vector3(0, handler, basis.mRight);
                mWidth = basis.mWidth;
                mHeight = basis.mHeight;
                mFalloffAngle = basis.mFalloffAngle;

            }
            public SquareAreaLight(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mAt = new Vector3(0, handler);
                mRight = new Vector3(0, handler);
            }
            public SquareAreaLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            [ElementPriority(4)]
            public Vector3 At
            {
                get { return mAt; }
                set { if(mAt!=value){mAt = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public Vector3 Right
            {
                get { return mRight; }
                set { if(mRight!=value){mRight = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float Width
            {
                get { return mWidth; }
                set { if(mWidth!=value){mWidth = value; OnElementChanged();} }
            }

            [ElementPriority(7)]
            public float Height
            {
                get { return mHeight; }
                set { if(mHeight!=value){mHeight = value; OnElementChanged();} }
            }

            [ElementPriority(8)]
            public float FalloffAngle
            {
                get { return mFalloffAngle; }
                set { if(mFalloffAngle!=value){mFalloffAngle = value; OnElementChanged();} }
            }

            [ElementPriority(9)]
            public float WindowTopBottomAngle
            {
                get { return mWindowTopBottomAngle; }
                set { if(mWindowTopBottomAngle!=value){mWindowTopBottomAngle = value; OnElementChanged();} }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                var br = new BinaryReader(s);
                mAt = new Vector3(0, handler, s);
                mRight = new Vector3(0, handler, s);
                mWidth = br.ReadSingle();
                mHeight = br.ReadSingle();
                mFalloffAngle = br.ReadSingle();
                mWindowTopBottomAngle = br.ReadSingle();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                var bw = new BinaryWriter(s);
                mAt.UnParse(s);
                mRight.UnParse(s);
                bw.Write(mWidth);
                bw.Write(mHeight);
                bw.Write(mFalloffAngle);
                bw.Write(mWindowTopBottomAngle);
            }
            public override LightType Type
            {
                get { return LightType.SquareAreaLight; }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SquareAreaLight(requestedApiVersion, handler, this);
            }
        }

        [ConstructorParameters(new object[] { LightType.DiscAreaLight })]
        public class DiscAreaLight : Light
        {
            private Vector3 mAt;
            private Vector3 mRight;
            private float mRadius;

            public DiscAreaLight(int APIversion, EventHandler handler, DiscAreaLight basis)
                : base(APIversion, handler, basis)
            {
                mAt = new Vector3(0, handler, basis.mAt);
                mRight = new Vector3(0, handler, basis.mRight);
                mRadius = basis.mRadius;

            }
            public DiscAreaLight(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mAt = new Vector3(0, handler);
                mRight = new Vector3(0, handler);
            }
            public DiscAreaLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

            [ElementPriority(4)]
            public Vector3 At
            {
                get { return mAt; }
                set { if(mAt!=value){mAt = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public Vector3 Right
            {
                get { return mRight; }
                set { if(mRight!=value){mRight = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float Radius
            {
                get { return mRadius; }
                set { if(mRadius!=value){mRadius = value; OnElementChanged();} }
            }

            protected override void Parse(Stream s)
            {
                base.Parse(s);
                var br = new BinaryReader(s);
                mAt = new Vector3(0, handler, s);
                mRight = new Vector3(0, handler, s);
                mRadius = br.ReadSingle();
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                var bw = new BinaryWriter(s);
                mAt.UnParse(s);
                mRight.UnParse(s);
                bw.Write(mRadius);
            }
            public override LightType Type
            {
                get { return LightType.DiscAreaLight; }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new DiscAreaLight(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { LightType.SquareWindow })]
        public class SquareWindowLight : SquareAreaLight
        {
            public SquareWindowLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
            public SquareWindowLight(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public SquareWindowLight(int APIversion, EventHandler handler, SquareWindowLight basis) : base(APIversion, handler, basis) { }
            public override LightType Type
            {
                get { return LightType.SquareWindow; }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SquareWindowLight(requestedApiVersion, handler, this);
            }
        }
        [ConstructorParameters(new object[] { LightType.CircularWindow })]
        public class CircularWindowLight : DiscAreaLight
        {
            public CircularWindowLight(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
            public CircularWindowLight(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public CircularWindowLight(int APIversion, EventHandler handler, CircularWindowLight basis) : base(APIversion, handler, basis) { }
            public override LightType Type
            {
                get { return LightType.CircularWindow; }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new CircularWindowLight(requestedApiVersion, handler, this);
            }
        }

        public class LightList : AResource.DependentList<Light>
        {
            public LightList(EventHandler handler, Stream s, uint count) : base(handler) { Parse(s,count);}
            public LightList(EventHandler handler) : base(handler) { }
            public LightList(EventHandler handler, long size) : base(handler, size) { }
            public LightList(EventHandler handler, IList<Light> ilt) : base(handler, ilt) { }

            public override void Add()
            {
                throw new NotSupportedException();
            }
            public override bool Add(params object[] fields)
            {
                if(fields.Length ==1&&typeof(LightType).IsAssignableFrom(fields[0].GetType()))
                {
                    Type t = GetElementType(fields);
                    ((IList<Light>)this).Add((Light) Activator.CreateInstance(t, new object[] {0, handler}));
                    return true;
                }
                return base.Add(fields);
            }
            protected override Light CreateElement(Stream s)
            {
                var br = new BinaryReader(s);
                var end = 0x80 + s.Position;
                var t = GetElementType((LightType)br.ReadUInt32());
                var l =(Light)Activator.CreateInstance(t, new object[] { 0, handler, s });
                while(s.Position != end)if(br.ReadByte() != 0 && Settings.Checking)
                    throw new InvalidDataException("Expected 0x00 padding");
                return l;
            }

            protected override void WriteElement(Stream s, Light element)
            {
                var bw = new BinaryWriter(s);
                var end = 0x80 + s.Position;
                bw.Write((UInt32)element.Type);
                element.UnParse(s);
                if (s.Position != end) bw.Write(new byte[end - s.Position]);
            }
            protected override void WriteCount(Stream s, uint count) { }

            private void Parse(Stream s, uint count)
            {
                for (int i = 0; i < count; i++)
                {
                    ((IList<Light>)this).Add(CreateElement(s));
                }
            }
            protected override Type GetElementType(params object[] fields)
            {

                if (fields.Length == 1 && typeof(LightType).IsAssignableFrom(fields[0].GetType()))
                {
                    LightType t = (LightType)fields[0];
                    switch (t)
                    {
                        case LightType.WorldLight:
                            return typeof(WorldLight);
                        case LightType.TubeLight:
                            return typeof(TubeLight);
                        case LightType.SquareWindow:
                            return typeof(SquareWindowLight);
                        case LightType.SquareAreaLight:
                            return typeof(SquareAreaLight);
                        case LightType.Spot:
                            return typeof(SpotLight);
                        case LightType.LampShade:
                            return typeof(LampShadeLight);
                        case LightType.DiscAreaLight:
                            return typeof(DiscAreaLight);
                        case LightType.Point:
                            return typeof(PointLight);
                        case LightType.CircularWindow:
                            return typeof(CircularWindowLight);
                    }
                }
                else if (fields.Length == 1 && typeof(Light).IsAssignableFrom(fields[0].GetType()))
                {
                    return fields[0].GetType();
                }
                return base.GetElementType(fields);
            }
        }
        public class Occluder : AHandlerElement, IEquatable<Occluder>
        {
            private OccluderType mType;
            private Vector3 mOrigin;
            private Vector3 mNormal;
            private Vector3 mXAxis;
            private Vector3 mYAxis;
            private float mPairOffset;

            public Occluder(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mNormal = new Vector3(0, handler);
                mOrigin = new Vector3(0, handler);
                mXAxis = new Vector3(0, handler);
                mYAxis = new Vector3(0, handler);
                
            }
            public Occluder(int APIversion, EventHandler handler, Occluder basis)
                : this(APIversion, handler, basis.mType, basis.mNormal, basis.mOrigin, basis.mPairOffset, basis.mXAxis, basis.mYAxis) { }
            public Occluder(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public Occluder(int APIversion, EventHandler handler, OccluderType type, Vector3 normal, Vector3 origin, float pairOffset, Vector3 xAxis, Vector3 yAxis)
                : base(APIversion, handler)
            {
                mType = type;
                mNormal = new Vector3(0, handler, normal);
                mOrigin = new Vector3(0, handler, origin);
                mXAxis = new Vector3(0, handler, xAxis);
                mYAxis = new Vector3(0, handler, yAxis);
                mPairOffset = pairOffset;
            }

            [ElementPriority(1)]
            public OccluderType Type
            {
                get { return mType; }
                set { if(mType!=value){mType = value; OnElementChanged();} }
            }

            [ElementPriority(2)]
            public Vector3 Origin
            {
                get { return mOrigin; }
                set { if(mOrigin!=value){mOrigin = value; OnElementChanged();} }
            }

            [ElementPriority(3)]
            public Vector3 Normal
            {
                get { return mNormal; }
                set { if(mNormal!=value){mNormal = value; OnElementChanged();} }
            }

            [ElementPriority(4)]
            public Vector3 XAxis
            {
                get { return mXAxis; }
                set { if(mXAxis!=value){mXAxis = value; OnElementChanged();} }
            }

            [ElementPriority(5)]
            public Vector3 YAxis
            {
                get { return mYAxis; }
                set { if(mYAxis!=value){mYAxis = value; OnElementChanged();} }
            }

            [ElementPriority(6)]
            public float PairOffset
            {
                get { return mPairOffset; }
                set { if(mPairOffset!=value){mPairOffset = value; OnElementChanged();} }
            }

            public string Value
            {
                get
                {
                    var sb = new StringBuilder();
                    foreach (var f in ContentFields)
                    {
                        if (f != "Value") sb.AppendFormat("{0}: {1}\r\n", f, this[f]);
                    }
                    return sb.ToString();
                }
            }
            private void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                mType = (OccluderType)br.ReadUInt32();
                mOrigin = new Vector3(0, handler, s);
                mNormal = new Vector3(0, handler, s);
                mXAxis = new Vector3(0, handler, s);
                mYAxis = new Vector3(0, handler, s);
                mPairOffset = br.ReadSingle();
            }
            public void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                bw.Write((UInt32)mType);
                mOrigin.UnParse(s);
                mNormal.UnParse(s);
                mXAxis.UnParse(s);
                mYAxis.UnParse(s);
                bw.Write(mPairOffset);

            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Occluder(requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public bool Equals(Occluder other)
            {
                return base.Equals(other);
            }
        }
        public class OccluderList : AResource.DependentList<Occluder>
        {
            public OccluderList(EventHandler handler, Stream s, uint count)
                : base(handler)
            {
                Parse(s, count);
            }
            public OccluderList(EventHandler handler) : base(handler) { }
            public OccluderList(EventHandler handler, long size) : base(handler, size) { }
            public OccluderList(EventHandler handler, IList<Occluder> ilt) : base(handler, ilt) { }
            private void Parse(Stream s, uint count)
            {
                for (int i = 0; i < count; i++)
                {
                    ((IList<Occluder>)this).Add(CreateElement(s));
                }
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override Occluder CreateElement(Stream s)
            {
                return new Occluder(0, handler, s);
            }

            protected override void WriteElement(Stream s, Occluder element)
            {
                element.UnParse(s);
            }
            protected override void WriteCount(Stream s, uint count) { }
        }

        private UInt32 mVersion = 4;
        private LightList mLights;
        private OccluderList mOccluders;
        public LITE(int APIversion, EventHandler handler)
            : base(APIversion, handler, null) { }
        public LITE(int APIversion, EventHandler handler, LITE basis)
            : base(APIversion, handler, null)
        {
            mVersion = basis.mVersion;
            mLights = new LightList(handler, basis.mLights.Select(l => l.Clone(handler)).Cast<Light>().ToList());
            mOccluders = new OccluderList(handler, basis.mOccluders.Select(l => l.Clone(handler)).Cast<Occluder>().ToList());
        }
        public LITE(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(2)]
        public LightList Lights
        {
            get { return mLights; }
            set { if(mLights!=value){mLights = value; OnRCOLChanged(this, new EventArgs());} }
        }

        [ElementPriority(3)]
        public OccluderList Occluders
        {
            get { return mOccluders; }
            set { if(mOccluders!=value){mOccluders = value; OnRCOLChanged(this, new EventArgs());} }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new LITE(requestedAPIversion, handler, this);
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (Settings.Checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            int val1 = br.ReadInt32();
            byte lightCount = br.ReadByte();
            byte occluderCount = br.ReadByte();
            //int valExpected1 = 4 + 128 * lightCount + 14 * occluderCount;
            //if (val1 != valExpected1 && Settings.Checking)throw new InvalidDataException(String.Format("Values don't match. Expected 0x{0:X8}, but got 0x{1:X8}", valExpected1, val1));
            int val2 = br.ReadUInt16();
            //int valExpected2 = 14 * occluderCount;
            //if (val2 != valExpected2 && Settings.Checking)throw new InvalidDataException(String.Format("Values don't match. Expected 0x{0:X8}, but got 0x{1:X8}", valExpected1, val2));
            mLights = new LightList(handler, s, lightCount);
            mOccluders = new OccluderList(handler, s, occluderCount);

        }

        public override Stream UnParse()
        {
            if (mLights == null) mLights = new LightList(handler);
            if (mOccluders == null) mOccluders = new OccluderList(handler);
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            int val1 = 4 + 128 * mLights.Count + 14 * mOccluders.Count;
            bw.Write(val1);
            bw.Write((byte)mLights.Count);
            bw.Write((byte)mOccluders.Count);
            bw.Write((ushort)(14 * mOccluders.Count));
            mLights.UnParse(s);
            mOccluders.UnParse(s);
            return s;

        }

        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\r\n", mVersion);
                if (mLights.Count > 0)
                {
                    sb.AppendFormat("Lights:\r\n");
                    for (int i = 0; i < mLights.Count; i++)
                    {
                        sb.AppendFormat("==Light[{0}]==:\r\n{1}\r\n", i, mLights[i].Value);

                    }
                }
                if (mOccluders.Count > 0)
                {
                    sb.AppendFormat("Occluders:\r\n");
                    for (int i = 0; i < mOccluders.Count; i++)
                    {
                        sb.AppendFormat("==Occluder[{0}]==:\r\n{1}\r\n", i, mOccluders[i].Value);

                    }
                }
                return sb.ToString();
            }
        }
        public override uint ResourceType
        {
            get { return 0x03B4C61DU; }
        }

        public override string Tag
        {
            get { return "LITE"; }
        }
    }
}
