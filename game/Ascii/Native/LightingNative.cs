using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class LightingNative
    {
        [DllImport("libascii")]
        public static extern UInt64 lighting_create_light(ref Light l);
        
        [DllImport("libascii")]
        public static extern void lighting_destry_light(UInt64 handle);
        
        [DllImport("libascii")]
        public static extern bool lighting_has_space(int count);
    }
}