using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    internal static class MarshalHelpers
    {
        public static T S<T>(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) throw new InvalidOperationException();
            return (T) Marshal.PtrToStructure(ptr, typeof (T));
        }

        public static IntPtr Ptr<T>(this T obj)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }
    }
}
