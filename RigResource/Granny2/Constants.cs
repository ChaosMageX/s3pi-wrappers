using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    public static class Constants
    {

        static Constants()
        {
            IntPtr hwnd = LoadLibrary("granny2.dll");
            GrannyFileInfoType = Constants.GetProcAddress(hwnd, "GrannyFileInfoType").S<IntPtr>();
            GrannyFileMV_Old = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_Old").S<IntPtr>();
            GrannyFileMV_LittleEndian32Bit = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_32Bit_LittleEndian").S<IntPtr>();
            GrannyFileMV_BigEndian32Bit = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_32Bit_BigEndian").S<IntPtr>();
            GrannyFileMV_LittleEndian64Bit = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_64Bit_LittleEndian").S<IntPtr>();
            GrannyFileMV_BigEndian64Bit = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_64Bit_BigEndian").S<IntPtr>();
            GrannyFileMV_ThisPlatform = Constants.GetProcAddress(hwnd, "GrannyGRNFileMV_ThisPlatform").S<IntPtr>();
            FreeLibrary(hwnd);
        }

        public static IntPtr GrannyFileInfoType;
        public static IntPtr GrannyFileMV_Old;
        public static IntPtr GrannyFileMV_LittleEndian32Bit;
        public static IntPtr GrannyFileMV_BigEndian32Bit;
        public static IntPtr GrannyFileMV_LittleEndian64Bit;
        public static IntPtr GrannyFileMV_BigEndian64Bit;
        public static IntPtr GrannyFileMV_ThisPlatform;

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string name);
        [DllImport("kernel32.dll")]
        private static extern IntPtr FreeLibrary(IntPtr hwnd);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hwnd, string name);
    }
}