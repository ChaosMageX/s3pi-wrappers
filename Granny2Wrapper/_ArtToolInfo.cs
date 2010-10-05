using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _ArtToolInfo
    {
        public String FromArtToolName;
        public Int32 ArtToolMajorRevision;
        public Int32 ArtToolMinorRevision;
        public Single UnitsPerMeter;
        public _Triple Origin;
        public _Triple RightVector;
        public _Triple UpVector;
        public _Triple BackVector;
        public _Variant Extra;
    }
}