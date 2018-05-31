using System.Runtime.InteropServices;

namespace game.Ascii
{
    public static class Paths
    {
        public static string DataDirectory
        {
            get
            {
                var ptr = Native.PathsNative.path_get_data_path();

                var val = Marshal.PtrToStringAnsi(ptr);
                Native.MemoryNative.free_memory(ptr);
                
                return val;
            }
        }

        public static string UserDirectory
        {
            get
            {
                var ptr = Native.PathsNative.path_get_user_path();

                var val = Marshal.PtrToStringAnsi(ptr);
                Native.MemoryNative.free_memory(ptr);

                return val;
            }
        }
    }
}