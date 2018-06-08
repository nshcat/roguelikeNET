using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class MemoryNative
    {
        
        /// <summary>
        /// This is needed since on windows using mingw, the CLR will call the wrong free() (different heap)
        /// which leads to corruption. This method will ensure to always call the correct one.
        /// </summary>
        /// <param name="p">Pointer to memory block that needs to be freed</param>
        [DllImport("libascii.so")]
        public static extern void free_memory(IntPtr p);  
    }
}