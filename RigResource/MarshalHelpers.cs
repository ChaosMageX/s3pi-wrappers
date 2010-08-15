using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace s3piwrappers
{
    public static class MarshalHelpers
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
