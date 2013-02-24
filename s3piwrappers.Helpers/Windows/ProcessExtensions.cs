using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace s3piwrappers.Helpers.Windows
{
    /// <summary>
    /// Class by mikeobrien https://gist.github.com/mikeobrien
    /// source: https://gist.github.com/mikeobrien/4046380
    /// </summary>
    public static class ProcessExtensions
    {
        public static bool IsWindowsService(this Process process)
        {
            return process.GetParent().ProcessName == "services";
        }

        public static Process GetParent(this Process process)
        {
            var parentPid = 0;
            var hnd = Kernel32.CreateToolhelp32Snapshot(Kernel32.Th32CsSnapprocess, 0);

            if (hnd == IntPtr.Zero) return null;

            var processInfo = new Kernel32.Processentry32 { dwSize = (uint)Marshal.SizeOf(typeof(Kernel32.Processentry32)) };

            if (Kernel32.Process32First(hnd, ref processInfo) == false) return null;

            do
            {
                if (process.Id == processInfo.th32ProcessID)
                    parentPid = (int)processInfo.th32ParentProcessID;
            }
            while (parentPid == 0 && Kernel32.Process32Next(hnd, ref processInfo));

            return parentPid > 0 ? Process.GetProcessById(parentPid) : null;
        }
        private static class Kernel32
        {
            public const uint Th32CsSnapprocess = 2;

            [StructLayout(LayoutKind.Sequential)]
            public struct Processentry32
            {
                public uint dwSize;
                public uint cntUsage;
                public uint th32ProcessID;
                public IntPtr th32DefaultHeapID;
                public uint th32ModuleID;
                public uint cntThreads;
                public uint th32ParentProcessID;
                public int pcPriClassBase;
                public uint dwFlags;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szExeFile;
            };

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessId);

            [DllImport("kernel32.dll")]
            public static extern bool Process32First(IntPtr hSnapshot, ref Processentry32 lppe);

            [DllImport("kernel32.dll")]
            public static extern bool Process32Next(IntPtr hSnapshot, ref Processentry32 lppe);
        }
    }
}