using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    public static class Logging
    {
        [DllImport("granny2.dll", EntryPoint = "GrannySetLogFileName", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetLogFileName(string FileName, bool Clear);
    }
}