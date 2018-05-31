using System;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    public static class Memory
    {
        /// <summary>
        /// Interprets given pointed-to memory block as a C-string in ANSI encoding.
        /// This calls the correct heap deallocation function afterwards to prevent memory leaks.
        /// This is needed, since in the case of the native library being compiled with MinGW, the
        /// CLR calls the wrong free() method (not the MinGW one), thereby corrupting memory.
        /// This method uses a function exposed by the native library itself to always call the correct
        /// free() implementation.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string PtrToString(IntPtr p)
        {
            var val = Marshal.PtrToStringAnsi(p);
            Native.MemoryNative.free_memory(p);          
            return val;
        }
    }
}