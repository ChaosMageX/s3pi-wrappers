using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ArtToolInfo
    {
        public String FromArtToolName;
        public Int32 ArtToolMajorRevision;
        public Int32 ArtToolMinorRevision;
        public Single UnitsPerMeter;
        public Triple Origin;
        public Triple RightVector;
        public Triple UpVector;
        public Triple BackVector;
        public Variant Extra;
    }
}