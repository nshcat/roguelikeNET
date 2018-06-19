using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class InputNative
    {
        [DllImport("libascii")]
        public static extern void input_begin();
        
        [DllImport("libascii")]
        public static extern void input_end();
        
        [DllImport("libascii")]
        public static extern bool input_has_key(int key);
    }
}