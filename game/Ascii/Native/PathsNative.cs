using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class PathsNative
    {
        [DllImport("libascii")]
        public static extern IntPtr path_get_data_path();

        [DllImport("libascii")]
        public static extern IntPtr path_get_user_path();
    }
}