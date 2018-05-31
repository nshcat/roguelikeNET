using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class PathsNative
    {
        [DllImport("libascii.so")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string path_get_data_path();

        [DllImport("libascii.so")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string path_get_user_path();
    }
}