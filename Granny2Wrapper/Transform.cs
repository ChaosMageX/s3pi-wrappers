﻿using System;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers.Granny2
{
    public class Transform : GrannyElement
    {
        private TransformFlags mFlags;
        private Triple mPosition;
        private Quad mOrientation;
        private Triple mScaleShearX;
        private Triple mScaleShearY;
        private Triple mScaleShearZ;

        public Transform(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mFlags = TransformFlags.Position | TransformFlags.Orientation | TransformFlags.ScaleShear;
            mPosition = new Triple(0, handler);
            mOrientation = new Quad(0, handler, 0, 0, 0, 1);
            mScaleShearX = new Triple(0, handler, 1, 0, 0);
            mScaleShearY = new Triple(0, handler, 0, 1, 0);
            mScaleShearZ = new Triple(0, handler, 0, 0, 1);
        }
        public Transform(int APIversion, EventHandler handler, Transform basis)
            : this(APIversion, handler, basis.mFlags, basis.mPosition, basis.mOrientation, basis.mScaleShearX, basis.mScaleShearY, basis.mScaleShearZ) { }
        public Transform(int APIversion, EventHandler handler, TransformFlags flags, Triple position, Quad orientation, Triple scaleShearX, Triple scaleShearY, Triple scaleShearZ)
            : base(APIversion, handler)
        {
            mFlags = flags;
            mPosition = new Triple(0, handler, position);
            mOrientation = new Quad(0, handler, orientation);
            mScaleShearX = new Triple(0, handler, scaleShearX);
            mScaleShearY = new Triple(0, handler, scaleShearY);
            mScaleShearZ = new Triple(0, handler, scaleShearZ);
        }
        internal Transform(int APIversion, EventHandler handler, _Transform t) : base(APIversion, handler) { FromStruct(t); }

        [ElementPriority(1)]
        public TransformFlags Flags
        {
            get { return mFlags; }
            set { mFlags = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public Triple Position
        {
            get { return mPosition; }
            set { mPosition = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public Quad Orientation
        {
            get { return mOrientation; }
            set { mOrientation = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public Triple ScaleShearX
        {
            get { return mScaleShearX; }
            set { mScaleShearX = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public Triple ScaleShearY
        {
            get { return mScaleShearY; }
            set { mScaleShearY = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public Triple ScaleShearZ
        {
            get { return mScaleShearZ; }
            set { mScaleShearZ = value; OnElementChanged(); }
        }

        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("Position:\n{0}\n", mPosition);
                sb.AppendFormat("Orientation:\n{0}\n", mOrientation);
                sb.AppendFormat("Scale/Shear:\n{0}\n{1}\n{2}", mScaleShearX, mScaleShearY, mScaleShearZ);
                return sb.ToString();
            }
        }

        internal void FromStruct(_Transform data)
        {
            mFlags = data.Flags;
            mPosition = new Triple(0, handler, data.Position);
            mOrientation = new Quad(0, handler, data.Orientation);
            mScaleShearX = new Triple(0, handler, data.ScaleShear_0);
            mScaleShearY = new Triple(0, handler, data.ScaleShear_1);
            mScaleShearZ = new Triple(0, handler, data.ScaleShear_2);
        }

        internal _Transform ToStruct()
        {
            var t = new _Transform();
            t.Flags = mFlags;
            t.Position = mPosition.ToStruct();
            t.Orientation = mOrientation.ToStruct();
            t.ScaleShear_0 = mScaleShearX.ToStruct();
            t.ScaleShear_1 = mScaleShearY.ToStruct();
            t.ScaleShear_2 = mScaleShearZ.ToStruct();
            return t;
        }
    }
}