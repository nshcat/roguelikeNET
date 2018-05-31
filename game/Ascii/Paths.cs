namespace game.Ascii
{
    public static class Paths
    {
        public static string DataDirectory => Native.PathsNative.path_get_data_path();
           
        public static string UserDirectory => Native.PathsNative.path_get_user_path(); 
    }
}