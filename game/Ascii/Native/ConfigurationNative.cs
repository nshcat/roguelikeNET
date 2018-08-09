using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public enum EntryType
    {
        Float = 0,
	    Double,
	    Int,
	    UInt,
	    String,
	    Bool
    }
    
    public static class ConfigurationNative
    {
	    [DllImport("libascii")]
	    public static extern float configuration_get_float(string path);
	    
	    [DllImport("libascii")]
	    public static extern double configuration_get_double(string path);
	    
	    [DllImport("libascii")]
	    public static extern int configuration_get_int(string path);
	    
	    [DllImport("libascii")]
	    public static extern uint configuration_get_uint(string path);
	    
	    [DllImport("libascii")]
	    public static extern IntPtr configuration_get_string(string path);
	    
	    [DllImport("libascii")]
	    public static extern bool configuration_get_boolean(string path);
    }
}