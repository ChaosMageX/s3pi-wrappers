using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class SoundEffect : Effect, IEquatable<SoundEffect>
    {
        private static readonly bool isTheSims4 = false;

        #region Constructors
        public SoundEffect(int apiVersion, EventHandler handler, SoundEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public SoundEffect(int apiVersion, EventHandler handler, ISection section) 
            : base(apiVersion, handler, section)
        {
        }

        public SoundEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private ulong mSoundId;
        private float mLocationUpdateDelta = 0.25f;
        private float mPlayTime;
        private float mVolume;
        private byte mByte01; //version 2; The Sims 4?
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public ulong SoundId
        {
            get { return mSoundId; }
            set
            {
                mSoundId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float LocationUpdateDelta
        {
            get { return mLocationUpdateDelta; }
            set
            {
                mLocationUpdateDelta = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public float PlayTime
        {
            get { return mPlayTime; }
            set
            {
                mPlayTime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float Volume
        {
            get { return mVolume; }
            set
            {
                mVolume = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0xF;

            s.Read(out mSoundId);
            s.Read(out mLocationUpdateDelta);
            s.Read(out mPlayTime);
            s.Read(out mVolume);
            if (isTheSims4 && mSection.Version >= 0x0002 && stream.Position < stream.Length)
            {
                s.Read(out mByte01);
            }
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mSoundId);
            s.Write(mLocationUpdateDelta);
            s.Write(mPlayTime);
            s.Write(mVolume);
            if (isTheSims4 && mSection.Version >= 0x0002)
            {
                s.Write(mByte01);
            }
        }
        #endregion

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                if (!isTheSims4 || mSection.Version < 0x0002)
                {
                    fields.Remove("Byte01");
                }
                return fields;
            }
        }

        public bool Equals(SoundEffect other)
        {
            return base.Equals(other);
        }
    }
}
