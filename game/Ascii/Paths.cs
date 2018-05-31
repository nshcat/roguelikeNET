using System.Runtime.InteropServices;

namespace game.Ascii
{
    public static class Paths
    {
        public static string DataDirectory => Memory.PtrToString(Native.PathsNative.path_get_data_path());

        public static string UserDirectory => Memory.PtrToString(Native.PathsNative.path_get_user_path());
    }
}