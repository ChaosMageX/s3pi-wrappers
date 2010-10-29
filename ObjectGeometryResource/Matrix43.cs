﻿using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class Matrix43 : AHandlerElement
    {

        private Vector3 mRight, mUp, mBack, mTranslate;

        public Matrix43(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mRight = new Vector3(0, handler, 1, 0, 0);
            mUp = new Vector3(0, handler, 0, 1, 0);
            mBack = new Vector3(0, handler, 0, 0, 1);
            mTranslate = new Vector3(0, handler, 0, 0, 0);
        }
        public Matrix43(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }

        public Matrix43(int APIversion, EventHandler handler, Vector3 back, Vector3 right, Vector3 translate, Vector3 up)
            : base(APIversion, handler)
        {
            mRight = new Vector3(0, handler, right);
            mUp = new Vector3(0, handler, up);
            mBack = new Vector3(0, handler, back);
            mTranslate = new Vector3(0, handler, translate);
        }
        public Matrix43(int APIversion, EventHandler handler, Matrix43 basis)
            : this(APIversion, handler, basis.mBack, basis.mRight, basis.mTranslate, basis.mUp)
        {
        }
        [ElementPriority(1)]
        public Vector3 Right
        {
            get { return mRight; }
            set { mRight = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public Vector3 Up
        {
            get { return mUp; }
            set { mUp = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public Vector3 Back
        {
            get { return mBack; }
            set { mBack = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public Vector3 Translate
        {
            get { return mTranslate; }
            set { mTranslate = value; OnElementChanged(); }
        }

        private void Parse(Stream s)
        {
            float m00, m01, m02, m03;
            float m10, m11, m12, m13;
            float m20, m21, m22, m23;
            var br = new BinaryReader(s);
            m00 = br.ReadSingle();
            m01 = br.ReadSingle();
            m02 = br.ReadSingle();
            m03 = br.ReadSingle();

            m10 = br.ReadSingle();
            m11 = br.ReadSingle();
            m12 = br.ReadSingle();
            m13 = br.ReadSingle();

            m20 = br.ReadSingle();
            m21 = br.ReadSingle();
            m22 = br.ReadSingle();
            m23 = br.ReadSingle();

            mRight = new Vector3(0, handler, m00, m01, m02);
            mUp = new Vector3(0, handler, m10, m11, m12);
            mBack = new Vector3(0, handler, m20, m21, m22);
            mTranslate = new Vector3(0, handler, m03, m13, m23);
        }
        public void UnParse(Stream s)
        {
            var bw = new BinaryWriter(s);
            bw.Write(mRight.X);
            bw.Write(mRight.Y);
            bw.Write(mRight.Z);
            bw.Write(mTranslate.X);

            bw.Write(mUp.X);
            bw.Write(mUp.Y);
            bw.Write(mUp.Z);
            bw.Write(mTranslate.Y);

            bw.Write(mBack.X);
            bw.Write(mBack.Y);
            bw.Write(mBack.Z);
            bw.Write(mTranslate.Z);
        }
        public string Value
        {
            get { return String.Format("{0}\r\n{1}\r\n{2}\r\n{3}", Right, Up, Back, Translate); }
        }
        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3}", Right, Up, Back, Translate);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new Matrix43(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}